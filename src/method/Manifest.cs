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

using System.Text;
using System.Threading.Tasks;
using PitneyBowes.Developer.ShippingApi.Json;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Request object for RetryManifest service call
    /// </summary>
    public class RetryManifestRequest : ShippingApiRequest
    {
        public override string ContentType { get => "application/json"; }

        [ShippingApiHeaderAttribute("Bearer")]
        public override StringBuilder Authorization { get; set; }

        [ShippingApiHeaderAttribute("X-PB-TransactionId")]
        public string TransactionId { get; set; }

        [ShippingApiQuery("originalTransactionId")]
        public string OriginalTransactionId { get; set; }
    }
    /// <summary>
    /// Request object for ReprintManifest service call
    /// </summary>
    public class ReprintManifestRequest : ShippingApiRequest
    {
        /// <summary>
        /// Http header content type - application/json
        /// </summary>
        public override string ContentType { get => "application/json";  }
        /// <summary>
        /// Authorization token
        /// </summary>
        [ShippingApiHeaderAttribute("Bearer")]
        public override StringBuilder Authorization { get; set; }
        /// <summary>
        /// ManifestId to reprint - as returned by the CreateManifest call
        /// </summary>
        public string ManifestId { get; set; }
    }
    public static partial class Api
    {
        /// <summary>
        /// This operation creates an end-of-day manifest that combines all trackable shipments into a single form that is scanned by the carrier 
        /// as an acceptance of all the shipments.
        ///
        /// Things to Consider:
        ///     * All shipments with the ADD_TO_MANIFEST option set to true are eligible for inclusion in the manifest.
        ///     * A shipment is eligible for inclusion both on and before its shipment date.
        ///     * If an eligible shipment is not included in a manifest request within 24 hours of the specified shipment date, it is automatically 
        ///     manifested.
        ///     * Up to 5000 shipments can be included in a single manifest request.
        ///     * Shipments, once manifested, cannot be re-manifested.
        ///     * When creating the manifest, the MANIFEST_TYPE parameter parameter is not required. It can be either left out or set to NORMAL.
        ///     * You can add shipments to the manifest by specifying Shipper ID, tracking numbers, or both:
        ///         - If you specify Shipper ID, the form will include all eligible shipments created with that Shipper ID. To specify Shipper ID, 
        ///         add the SHIPPER_ID parameter to the parameters array.
        ///         - If you specify tracking numbers, the form will include all eligible shipments with those tracking numbers.Specify tracking 
        ///         numbers in the parcelTrackingNumbers array.
        ///     * If you specify both Shipper ID and tracking numbers, ensure the tracking numbers belong to the Shipper ID.
        ///     * You can filter further by specifying an inductionPostalCode. When specified, the inductionPostalCode value in the manifest 
        ///     request must match the rates.inductionPostalCode value of the shipment.If a shipment has no rates.inductionPostalCode, the 
        ///     value in the manifest request must match the shipment’s fromAddress.postalCode.
        ///     * If a manifest request contains shipments with different inductionPostalCode values, then a multi-page manifest is created, with one inductionPostalCode value per page. The pages are accessed via a single PDF.
        ///     * Manifest documents retrieved through URLs are available for 24 hours after creation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<T>> CreateManifest<T>(T request, ISession session = null) where T : IManifest, new()
        {
            var manifestRequest = new JsonManifest<T>(request);
            return await WebMethod.Post<T, JsonManifest<T>>("/shippingservices/v1/manifests", manifestRequest, session);
        }
        /// <summary>
        /// Use this operation to reprint a manifest for which the initial Create Manifest request was successful but the response object did not save properly.
        ///
        /// Do not use this operation in the following cases:
        ///     * If the initial request returned an error.Instead create a new manifest.
        ///     * If the initial request returned no response at all.Instead retry the manifest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<T>> ReprintManifest<T>(ReprintManifestRequest request, ISession session = null) where T : IManifest, new()
        {
            return await WebMethod.Post<T, ReprintManifestRequest> ("/shippingservices/v1/manifests/{ManifestId} ", request, session);
        }
        /// <summary>
        /// This operation retries a Create Manifest request that was submitted but received no response. You can use this operation only if the request received 
        /// no response at all. If the request returned an error, you must instead create a new manifest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<T>> RetryManifest<T>(RetryManifestRequest request, ISession session = null) where T : IManifest, new()
        {
            return await WebMethod.Post<T, RetryManifestRequest>("/shippingservices/v1/manifests", request, session);
        }

    }
}
