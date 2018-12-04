// /*
// Copyright 2016 Pitney Bowes Inc.
//
// Licensed under the MIT License(the "License"); you may not use this file except in compliance with the License.  
// You may obtain a copy of the License in the README file or at
//    https://opensource.org/licenses/MIT 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed 
// on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the License 
// for the specific language governing permissions and limitations under the License.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// */
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace PitneyBowes.Developer.ShippingApi.Mock
{
    /// <summary>
    /// From a http stream of the request and list of ApiMethods, figure out the request type, deserialize the stream and populate the request.
    /// </summary>
    public static class RequestDeserializer
    {
        private static void DeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            throw new JsonSerializationException("Error deserializing", e.ErrorContext.Error);
        }
        /// <summary>
        /// Request the specified mimeStream, methods and session.
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="mimeStream">MIME stream.</param>
        /// <param name="methods">Methods.</param>
        /// <param name="session">Session.</param>
        public static ShippingApiMethodRequest Request(MimeStream mimeStream, List<ShippingApiMethod> methods, ISession session)
        {
            mimeStream.ReadHeaders(); // reads http headers as well

            Dictionary<string, string> headers = new Dictionary<string, string>();

            foreach( var h in mimeStream.Headers.Keys)
            {
                StringBuilder sb = new StringBuilder();
                bool first = true;
                foreach( var s in mimeStream.Headers[h] )
                {
                    if (first) first = false;
                    else sb.Append(';');
                    sb.Append(s);
                }
                headers.Add(h, sb.ToString());
            }

            var firstLine = mimeStream.FirstLine.Split(' ');
            var verb = firstLine[0];
            string pattern = "(?<file>/[a-zA-Z0-9/]+)(\\?(?<parameters>.*))*";
            var urlRegex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            var match = urlRegex.Match(firstLine[1]);
            if (!match.Success)
            {
                throw new Exception("Could not parse URI");
            }
            var uri = match.Groups[urlRegex.GroupNumberFromName("file")].Value;
            var parameters = match.Groups[urlRegex.GroupNumberFromName("parameters")].Value;

            var requestParameters = new Dictionary<string, string>();

            if (parameters != null && !parameters.Equals(""))
            {
                var p1 = parameters.Split('&');
                foreach (var p2 in p1)
                {
                    var h = p2.Split('=');
                    requestParameters[h[0]] = h[1];
                }
            }
            try
            {
                var deserializer = new JsonSerializer();
                deserializer.Error += DeserializationError;
                deserializer.ContractResolver = new ShippingApiContractResolver();
                if (session.TraceWriter != null)
                {
                    deserializer.TraceWriter = session.TraceWriter;
                }

                foreach (var method in methods )
                {
                    if (method.Verb.ToString() != verb)
                    {
                        continue;
                    }
                    var re = new Regex( method.UriRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                    var m = re.Match(uri);
                    if (m.Success)
                    {
                        IShippingApiRequest request = null;
                        // create the request
                        using (var reader = new StreamReader(mimeStream))
                        {
                            ((ShippingApiContractResolver)deserializer.ContractResolver).Registry = session.SerializationRegistry;
                            // if wrapped create wrapper object 
                            if (method.RequestInterface != null)
                            {
                                var obj = deserializer.Deserialize(reader, method.RequestType);
                                Type[] typeArgs = { obj.GetType() };
                                var wrapperType = session.SerializationRegistry.GetWrapperFor(method.RequestInterface).MakeGenericType(typeArgs);
                                request = (IShippingApiRequest)Activator.CreateInstance(wrapperType, obj);
                            }
                            else
                            {
                                request = (IShippingApiRequest)deserializer.Deserialize(reader, method.RequestType);
                            }

                        }

                        // set params in the URI
                        for (int g = 0; g < m.Groups.Count; g++)
                        {
                            var paramName = re.GroupNameFromNumber(g);
                            // set property of the request matching capture name
                            if (!Regex.IsMatch(paramName, "^\\d+$"))
                            {
                                PropertyInfo prop = method.RequestType.GetProperty("paramName");
                                prop.SetValue(request, match.Groups[g].Value, null);
                            }
                        }

                        // query properties
                        ShippingApiRequest.ProcessRequestAttributes<ShippingApiQueryAttribute>(request,
                           (a, s, v, p) => {
                               // p is prop name
                               if (requestParameters.ContainsKey(s))
                               {
                                    PropertyInfo prop = request.GetType().GetProperty(p);
                                    //TODO: better handling of JSON encoding
                                    var sb = new StringBuilder(requestParameters[s]).Replace("\"", "\\\"").Append("\"");
                                    sb.Insert(0, "\"");
                                    var tx = new StringReader(sb.ToString());
                                    var o = deserializer.Deserialize(tx, prop.PropertyType);
                                    prop.SetValue(request, o );
                               }
                           }
                        );

                        // header properties
                        ShippingApiRequest.ProcessRequestAttributes<ShippingApiHeaderAttribute>(request,
                            (a, s, v, p) => {
                                if (headers.ContainsKey(s))
                                {
                                    PropertyInfo prop = request.GetType().GetProperty(p);
                                    var sb = new StringBuilder(headers[s]).Replace("\"", "\\\"").Append("\"");
                                    sb.Insert(0, "\"");
                                    var tx = new StringReader(sb.ToString());
                                    var o = deserializer.Deserialize(tx, prop.PropertyType);
                                    prop.SetValue(request, o);
                                }
                            }
                         );                                                                             
                        return new ShippingApiMethodRequest() { Method = method, Request = request};
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }
    }
}
