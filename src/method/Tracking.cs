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


using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Request object for for GET /v1/tracking/{trackingNumber}.
    /// </summary>
    public class TrackingRequest : ShippingApiRequest
    {
        /// <summary>
        /// Request content type
        /// </summary>
        public override string ContentType { get => "application/json"; }

        /// <summary>
        /// Token
        /// </summary>
        [ShippingApiHeader("Bearer")]
        public override StringBuilder Authorization { get; set; }

        /// <summary>
        /// REQUIRED. The tracking number for the shipment.
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Valid Value(s): TrackingNumber - Required
        /// </summary>
        [ShippingApiQuery("packageIdentifierType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PackageIdentifierType PackageIdentifierType { get => PackageIdentifierType.TrackingNumber; }

        /// <summary>
        /// Required - Valid Value(s): USPS
        /// </summary>
        [ShippingApiQuery("carrier")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Carrier Carrier { get; set; }
    }

    public static partial class Api
    {
        /// <summary>
        /// Web service methods for GET /v1/tracking/{trackingNumber}.
        /// 
        /// Shipment labels that are printed using the PB Shipping APIs are automatically tracked and their package status can be easily retrieved using this GET operation.
        /// 
        /// Things to Consider:
        /// USPS performs daily scheduled maintenance on tracking services, generally between midnight and 3 AM ET.During this time shippers might experience intermittent timeout errors if requesting USPS tracking status. We recommend that shippers not schedule jobs to obtain USPS tracking during this time period.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<T>> Tracking<T>(TrackingRequest request, ISession session = null) where T : ITrackingStatus, new()
        {
            return await WebMethod.Get<T, TrackingRequest>("/shippingservices/v1/tracking/{TrackingNumber}", request, session);
        }
    }
}
