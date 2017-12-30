/*
Copyright 2016 Pitney Bowes Inc.

Licensed under the MIT License(the "License"); you may not use this file except in compliance with the License.  
You may obtain a copy of the License in the README file or at
   https://opensource.org/licenses/MIT 
Unless required by applicable law or agreed to in writing, software distributed under the License is distributed 
on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the License 
for the specific language governing permissions and limitations under the License.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/

using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Base class for the api response to allow polymorphic access to aspects of the response without knowing the actual response type.
    /// </summary>
    public class ShippingApiResponse
    {
        /// <summary>
        /// Set default values
        /// </summary>
        public ShippingApiResponse()
        {
            Success = false;
            HttpStatus = HttpStatusCode.Unused;
        }
        private List<ErrorDetail> _errors;
        /// <summary>
        /// Http status code of the response
        /// </summary>
        public HttpStatusCode HttpStatus { get; set; }
        /// <summary>
        /// Time taken by the response
        /// </summary>
        public TimeSpan RequestTime { get; set; }
        /// <summary>
        /// Any errors returned as part of the API call
        /// </summary>
        public List<ErrorDetail> Errors 
        { 
            get { 
                if (_errors == null) _errors = new List<ErrorDetail>();
                return _errors;
            }
            set
            {
                _errors = value;
            }
        }
        /// <summary>
        /// Whether the API call succeeded.
        /// </summary>
        public bool Success { get; set; }
    }

    /// <summary>
    /// Shipping API Response class with generic parameter hold the returned object if the call was successful.
    /// </summary>
    /// <typeparam name="Response"></typeparam>
    public class ShippingApiResponse<Response> : ShippingApiResponse
    {
        internal void ProcessResponseAttribute(string propName, IEnumerable<string> values)
        {
            var propertyInfo = this.GetType().GetProperty(propName);
            if (propertyInfo == null) return;
            foreach (object attribute in propertyInfo.GetCustomAttributes(true))
            {
                if (attribute is ShippingApiHeaderAttribute)
                {
                    var sa = attribute as ShippingApiHeaderAttribute;
                    var v = new StringBuilder();
                    bool firstValue = true;
                    foreach (var value in values)
                    {
                        if (!firstValue) { firstValue = false; v.Append(','); }
                        v.Append(value);
                    }
                    propertyInfo.SetValue(this, v.ToString());
                }
            }
        }
        /// <summary>
        /// Convenience operator to allow the response object to be retieved by casting.
        /// </summary>
        /// <param name="r"></param>
        public static explicit operator Response(ShippingApiResponse<Response> r)
        {
            return r.APIResponse;
        }

        /// <summary>
        /// Set default falues
        /// </summary>
        public ShippingApiResponse() : base() { }
        /// <summary>
        /// Response object from the request. If the call was not successful this opject will be null
        /// </summary>
        public Response APIResponse = default(Response);


        private static void DeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            throw new JsonSerializationException("Error deserializing", e.ErrorContext.Error);
        }

        internal static void Deserialize(ISession session, RecordingStream respStream, ShippingApiResponse<Response> apiResponse, long streamPos = 0)
        {
            var deserializer = new JsonSerializer();
            deserializer.Error += DeserializationError;
            deserializer.ContractResolver = new ShippingApiContractResolver();
            ((ShippingApiContractResolver)deserializer.ContractResolver).Registry = session.SerializationRegistry;
            var recording = respStream.IsRecording;
            respStream.IsRecording = false;
            respStream.Seek(streamPos, SeekOrigin.Begin);
            JsonConverter converter = new ShippingApiResponseTypeConverter<Response>();
            Type t = (Type)converter.ReadJson(new JsonTextReader(new StreamReader(respStream)), typeof(Type), null, deserializer);
            respStream.Seek(streamPos, SeekOrigin.Begin);
            respStream.IsRecording = recording;
            if (t == typeof(ErrorFormat1))
            {
                var error = (ErrorFormat1[])deserializer.Deserialize(new StreamReader(respStream), typeof(ErrorFormat1[]));
                foreach (var e in error)
                {
                    apiResponse.Errors.Add(new ErrorDetail() { ErrorCode = e.ErrorCode, Message = e.Message, AdditionalInfo = e.AdditionalInfo });
                }
                apiResponse.APIResponse = default(Response);
            }
            else if (t == typeof(ErrorFormat2))
            {
                var error = (ErrorFormat2)deserializer.Deserialize(new StreamReader(respStream), typeof(ErrorFormat2));
                foreach (var e in error.Errors)
                {
                    apiResponse.Errors.Add(new ErrorDetail() { ErrorCode = e.ErrorCode, Message = e.ErrorDescription, AdditionalInfo = string.Empty });
                }
                apiResponse.APIResponse = default(Response);
            }
            else if (t == typeof(ErrorFormat3))
            {
                var error = (ErrorFormat3[])deserializer.Deserialize(new StreamReader(respStream), typeof(ErrorFormat3[]));
                foreach (var e in error)
                {
                    apiResponse.Errors.Add(new ErrorDetail() { ErrorCode = e.Key, Message = e.Message, AdditionalInfo = string.Empty });
                }
                apiResponse.APIResponse = default(Response);
            }
            else
            {
                apiResponse.APIResponse = (Response)deserializer.Deserialize(new StreamReader(respStream), t);
            }
        }
    }
}