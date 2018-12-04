/*
Copyright 2018 Pitney Bowes Inc.

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
using System.IO;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Implements the web service call for Pitney Bowes we services
    /// </summary>
    public class ShippingApiHttpRequest : IHttpRequest
    {
        internal static void AddRequestHeaders(HttpRequestMessage request, ShippingApiHeaderAttribute attribute, string propValue, string propName, RecordingStream recordingStream)
        {
            if (attribute.OmitIfEmpty && (propValue == null || propValue.Equals(String.Empty))) return;
            switch (propName)
            {
                case "Authorization":
                    request.Headers.Authorization = new AuthenticationHeaderValue(attribute.Name, propValue);
                    recordingStream.WriteRecordCRLF(string.Format("Authorization: {0} {1}", attribute.Name, propValue));
                    break;

                default:
                    request.Headers.Add(attribute.Name, propValue);
                    recordingStream.WriteRecordCRLF(string.Format("{0}:{1}", attribute.Name, propValue));
                    break;
            }

        }
        /// <summary>
        /// Call the web service and return the response
        /// </summary>
        /// <typeparam name="Response"></typeparam>
        /// <typeparam name="Request"></typeparam>
        /// <param name="resource"></param>
        /// <param name="verb"></param>
        /// <param name="request"></param>
        /// <param name="deleteBody"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task<ShippingApiResponse<Response>> HttpRequest<Response, Request>(string resource, HttpVerb verb, Request request, bool deleteBody = false, ISession session = null) where Request : IShippingApiRequest
        {
            return await HttpRequestStatic<Response, Request>(resource, verb, request, deleteBody, session);
        }
        internal async static Task<ShippingApiResponse<Response>> HttpRequestStatic<Response, Request>(string resource, HttpVerb verb, Request request, bool deleteBody = false, ISession session = null) where Request : IShippingApiRequest
        {
            if (session == null) session = Globals.DefaultSession;
            var client = Globals.Client(session.EndPoint);

            //            client.Timeout = new TimeSpan(0, 0, 0, 0, session.TimeOutMilliseconds);

            using (var recordingStream = new RecordingStream(null, request.RecordingFullPath(resource, session), FileMode.Create, RecordingStream.RecordType.MultipartMime))
            {
#pragma warning disable CS0618
                recordingStream.OpenRecord(session.Record);
#pragma warning restore CS0618
                string uriBuilder = request.GetUri(resource);

                HttpResponseMessage httpResponseMessage;
                using (HttpRequestMessage requestMessage = new HttpRequestMessage())
                {
                    using (var stream = new MemoryStream())
                    {
                        recordingStream.SetBaseStream(stream, "text/httpRequest");
                        recordingStream.WriteRecordCRLF(string.Format("{0} {1} HTTP/1.1", verb.ToString(), uriBuilder));
                        recordingStream.WriteRecordCRLF(string.Format("{0}: {1}", "Content-Type", request.ContentType));
                        foreach (var h in request.GetHeaders())
                        {
                            AddRequestHeaders(requestMessage, h.Item1, h.Item2, h.Item3, recordingStream);
                        }
                        recordingStream.WriteRecordCRLF("");

                        if (verb == HttpVerb.PUT || verb == HttpVerb.POST || (verb == HttpVerb.DELETE && deleteBody))
                        {

                            using (var writer = new StreamWriter(recordingStream))
                            using (var reqContent = new StreamContent(stream))
                            {
                                request.SerializeBody(writer, session);
                                stream.Seek(0, SeekOrigin.Begin);
                                requestMessage.Content = reqContent;
                                reqContent.Headers.ContentType = new MediaTypeHeaderValue(request.ContentType);
                                if (verb == HttpVerb.PUT)
                                {
                                    requestMessage.Method = HttpMethod.Put;
                                }
                                else if (verb == HttpVerb.DELETE)
                                {
                                    requestMessage.Method = HttpMethod.Delete;
                                }
                                else
                                {
                                    requestMessage.Method = HttpMethod.Post;
                                }
                                requestMessage.RequestUri = new Uri(client.BaseAddress + uriBuilder);
                                httpResponseMessage = await client.SendAsync(requestMessage);
                            }
                        }

                        else if (verb == HttpVerb.DELETE)
                        {
                            requestMessage.Method = HttpMethod.Delete;
                            requestMessage.RequestUri = new Uri(client.BaseAddress + uriBuilder);
                            httpResponseMessage = await client.SendAsync(requestMessage);

                        }
                        else
                        {
                            requestMessage.Method = HttpMethod.Get;
                            requestMessage.RequestUri = new Uri(client.BaseAddress + uriBuilder);
                            httpResponseMessage = await client.SendAsync(requestMessage);

                        }
                    }
                }

                using (var respStream = await httpResponseMessage.Content.ReadAsStreamAsync())
                {
                    recordingStream.SetBaseStream( respStream, "text/httpResponse");

                    recordingStream.WriteRecordCRLF(string.Format("HTTP/1.1 {0} {1}", (int)httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase));
                    recordingStream.WriteRecordCRLF(string.Format("Content-Length: {0}", httpResponseMessage.Content.Headers.ContentLength));
                    recordingStream.WriteRecordCRLF(string.Format("Content-Type: {0}", httpResponseMessage.Content.Headers.ContentType));
                    recordingStream.WriteRecordCRLF("");

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        var apiResponse = new ShippingApiResponse<Response> { HttpStatus = httpResponseMessage.StatusCode, Success = httpResponseMessage.IsSuccessStatusCode };
                        var sb = new StringBuilder();
                        foreach (var h in httpResponseMessage.Headers)
                        {
                            apiResponse.ProcessResponseAttribute(h.Key, h.Value);
                            bool firstValue = true;
                            foreach (var s in h.Value)
                            {
                                if (firstValue)
                                {
                                    firstValue = false;
                                }
                                else
                                {
                                    sb.Append(';');
                                }
                                sb.Append(s);
                            }
                            recordingStream.WriteRecordCRLF(string.Format("{0}:{1}", h.Key, sb.ToString()));
                            sb.Clear();
                        }
                        recordingStream.WriteRecordCRLF("");
                        ShippingApiResponse<Response>.Deserialize(session, recordingStream, apiResponse);
                        return apiResponse;
                    }
                    else
                    {
                        var apiResponse = new ShippingApiResponse<Response> { HttpStatus = httpResponseMessage.StatusCode, Success = httpResponseMessage.IsSuccessStatusCode };
                        try
                        {
                            ShippingApiResponse<Response>.Deserialize(session, recordingStream, apiResponse);
                        }
                        catch (JsonException)
                        {
                            session.LogWarning(String.Format("http {0} request to {1} failed to deserialize with error {2}", verb.ToString(), uriBuilder, httpResponseMessage.StatusCode));
                            apiResponse.Errors.Add(new ErrorDetail() { ErrorCode = "HTTP " + httpResponseMessage.Version + " " + httpResponseMessage.StatusCode.ToString(), Message = httpResponseMessage.ReasonPhrase });
                        }
                        return apiResponse;
                    }
                }
            }
        }
    }
}