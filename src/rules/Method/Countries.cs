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
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace PitneyBowes.Developer.ShippingApi.Rules
{
    /// <summary>
    /// Request object for the countries call
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonObject(MemberSerialization.OptIn)]
    public class CountriesRequest<T> : IShippingApiRequest where T : Country, new()
    {
        /// <summary>
        /// Suffix to add to the file name when recording the response to a file
        /// </summary>
        public string RecordingSuffix => "";
        /// <summary>
        /// Path to use when recording the file
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public string RecordingFullPath(string resource, ISession session)
        {
            return ShippingApiRequest.RecordingFullPath(this, resource, session);
        }
        /// <summary>
        /// REQUIRED. Carrier. Valid value(s): usps
        /// </summary>
        [ShippingApiQuery("carrier")]
        public Carrier Carrier { get; set; }
        /// <summary>
        /// REQUIRED. Carrier. Valid value(s): usps
        /// </summary>
        [ShippingApiQuery("originCountryCode")]
        public string OriginCountryCode { get; set; }
        /// <summary>
        /// Return the URI for the method (after inserting properties)
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public string GetUri(string baseUrl)
        {
            StringBuilder uri = new StringBuilder(baseUrl);
            ShippingApiRequest.SubstitueResourceParameters(this, uri);
            ShippingApiRequest.AddRequestQuery(this, uri);
            return uri.ToString();
        }
        /// <summary>
        /// Return http header values.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tuple<ShippingApiHeaderAttribute, string, string>> GetHeaders()
        {
            return ShippingApiRequest.GetHeaders(this);
        }
        /// <summary>
        /// Serialize request body for http request
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="session"></param>
        public void SerializeBody(StreamWriter writer, ISession session)
        {
            ShippingApiRequest.SerializeBody(this, writer, session);
        }
        /// <summary>
        /// http content-type header. "application/json"
        /// </summary>
        public string ContentType => "application/json";
        /// <summary>
        /// OAUTH token
        /// </summary>
        [ShippingApiHeaderAttribute("Bearer")]
        public StringBuilder Authorization { get; set; }
    }
    /// <summary>
    /// This operation returns a list of supported destination countries to which the carrier offers international shipping services.
    /// </summary>
    public static class CountriesMethods
    {
        /// <summary>
        /// This operation returns a list of supported destination countries to which the carrier offers international shipping services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<IEnumerable<T>>> Countries<T>(CountriesRequest<T> request, ISession session = null) where T : Country, new()
        {
            return await WebMethod.Get<IEnumerable<T>, CountriesRequest<T>>("/shippingservices/v1/countries", request, session);
        }

    }

}