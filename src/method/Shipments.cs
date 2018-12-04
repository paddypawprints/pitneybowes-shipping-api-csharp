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

using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PitneyBowes.Developer.ShippingApi.Json;
using System.Text;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Request object for CancelShipment API call
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CancelShipmentRequest : ShippingApiRequest
    {
        /// <summary>
        /// ContentType header - application/json
        /// </summary>
        public override string ContentType { get => "application/json"; }
        /// <summary>
        /// Authentication token
        /// </summary>
        [ShippingApiHeader("Bearer")]
        public override StringBuilder Authorization { get; set; }
        /// <summary>
        /// REQUIRED. Unique transaction ID.
        /// </summary>
        [ShippingApiHeader("X-PB-TransactionId")]
        public string TransactionId {get;set;}
        /// <summary>
        /// REQUIRED. Shipment ID used when printing the shipment label.
        /// </summary>
        public string ShipmentToCancel {get;set;}
        /// <summary>
        /// REQUIRED. Abbreviated name of the carrier. Valid value(s): USPS
        /// </summary>
        [JsonProperty("carrier")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Carrier Carrier {get;set;}
        /// <summary>
        /// Indicates the initiator of refund will be the Shipper. Valid value(s): SHIPPER
        /// </summary>
        [JsonProperty("cancelInitiator")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CancelInitiator CancelInitiator {get;set;}
    }

    /// <summary>
    /// Response object for the CancelShipment API call
    /// </summary>
    public class CancelShipmentResponse 
    {
        /// <summary>
        /// Abbreviated name of the carrier. Valid value(s): USPS
        /// </summary>
        [JsonProperty("carrier")]
        public Carrier Carrier {get;set;}
        /// <summary>
        /// Indicates the initiator of refund will be the Shipper. Valid value(s): SHIPPER
        /// </summary>
        [JsonProperty("cancelInitiator")]
        public CancelInitiator CancelInitiator {get;set;}
        /// <summary>
        /// The total amount payable to the carrier.
        /// </summary>
        [JsonProperty("totalCarrierCharge")]
        public decimal TotalCarrierCharge {get;set;}
        /// <summary>
        /// Tracking number associated with a shipment parcel.
        /// </summary>
        [JsonProperty("parcelTrackingNumber")]
        public string ParcelTrackingNumber {get;set;}
        /// <summary>
        /// Current status of the shipment refund. Automatically set to: INITIATED
        /// </summary>
        [JsonProperty("status")]
        public RefundStatus RefundStatus {get;set;}
    }

    /// <summary>
    /// Request object for the ReprintShipmentCall
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ReprintShipmentRequest : ShippingApiRequest
    {
        /// <summary>
        /// ContentType header - application/json
        /// </summary>
        public override string ContentType { get => "application/json"; }
        /// <summary>
        /// Authentication token
        /// </summary>
        [ShippingApiHeader("Bearer")]
        public override StringBuilder Authorization { get; set; }
        /// <summary>
        /// Shipment to reprint
        /// </summary>
        public string Shipment {get;set;}
    }

    public static partial class Api
    {
        /// <summary>
        /// <para>
        /// This POST operation creates a shipment and purchases a shipment label.The API returns the label as either a Base64 string or a link 
        /// to a PDF.
        /// </para>
        /// <para>
        /// Scan-Based Return Shipment
        /// </para>
        /// <para>
        /// This operation prints a USPS scan-based return (SBR) label. Unlike prepaid return services, SBR services allow you to print return 
        /// labels without incurring postage charges at the time of print. Postage is automatically deducted only when the return label is scanned 
        /// into the USPS mail stream. If the label is not used, no charges incur.
        /// </para>
        /// Before you can print SBR labels, you must enable SBR services on your account. To enable this service, please contact the PB support 
        /// team at: ShippingAPISupport @pb.com.
        /// <para>
        /// PMOD Shipment
        /// </para>
        /// Priority Mail Open and Distribute (PMOD) expedites the movement of lower classes of mail by using Priority Mail to send the mailings to 
        /// a destination center for processing.Shippers place mail pieces into an approved USPS Priority Mail container(sack, tray, or tub), affix 
        /// a PMOD address label to the container, and ship the container to a USPS authorized acceptance location. The postal facility opens the 
        /// container and processes the individual shipments according to their mail classes.The postage price is based on the weight of the contents 
        /// (excluding the tare weight of the external container) and regular Priority Mail distance-based prices.
        /// <para>
        /// To begin printing PMOD labels, contact the PB support team at ShippingAPISupport @pb.com for requirements.
        /// </para>
        /// </summary>
        /// <typeparam name="T">Class to use for the shipment object. Implements IShipment</typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns>ShippingApiResponse</returns>
        public async static Task<ShippingApiResponse<T>> CreateShipment<T>(T request, ISession session = null) where T:IShipment, new()
        {
            var wrapped = new JsonShipment<T>(request);
            return await WebMethod.Post< T, JsonShipment<T>>( "/shippingservices/v1/shipments", wrapped, session );
        }
        /// <summary>
        /// Use this operation to cancel an unused shipment label and initiate a request for an electronic refund.
        ///
        /// Note: In addition to this API, Pitney Bowes offers an Auto Refund service that automatically submits unused labels for electronic refund. For more information, see the Auto Refund FAQ.To enable the service for your account, please contact PB Support at ShippingAPISupport@pb.com.
        ///
        /// Things to Consider:
        ///     * All unused labels must be submitted for electronic refund to USPS within 30 days of printing.
        ///     * It can take up to 14 days for USPS to process and refund the value of the shipment label after submission.
        ///     * Approved postage refunds will be automatically credited to the payment account used to print the shipment label.
        ///     * You cannot request refunds for SBR labels (i.e., labels created with “shipmentType” set to “RETURN”). SBR labels do not incur 
        ///     charges unless they are used.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<CancelShipmentResponse>> CancelShipment( CancelShipmentRequest request, ISession session = null)
        {
            return await WebMethod.DeleteWithBody<CancelShipmentResponse, CancelShipmentRequest>( "/shippingservices/v1/shipments/{ShipmentToCancel}", request, session );
        }
        /// <summary>
        /// Use this operation to reprint a shipment label for which the initial Create Shipment request was successful but the response object did not save properly.
        ///
        /// Note: The number of reprints of a shipment label will be scrutinized and restricted.
        ///
        /// Do not use this operation in the following cases:
        ///     * If the initial request returned an error.Instead create a new shipment with a new Transaction ID.
        ///     * If the initial request received no response at all. Instead retry the shipment.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<T>>  ReprintShipment<T>(ReprintShipmentRequest request, ISession session = null) where T : IShipment, new()
        {
            return  await WebMethod.Get<T, ReprintShipmentRequest>( "/shippingservices/v1/shipments/{Shipment}", request, session );
        }
        //TODO: Retry shipment
    }
}