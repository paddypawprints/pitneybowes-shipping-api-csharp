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
        [ShippingApiHeader("X-PB-TransactionId")]
        public string TransactionId {get;set;}
        public string ShipmentToCancel {get;set;}
        [JsonProperty("carrier")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Carrier Carrier {get;set;}
        [JsonProperty("cancelInitiator")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CancelInitiator CancelInitiator {get;set;}
    }

    /// <summary>
    /// Response object for the CancelShipment API call
    /// </summary>
    public class CancelShipmentResponse 
    {
        [JsonProperty("carrier")]
        public Carrier Carrier {get;set;}
        [JsonProperty("cancelInitiator")]
        public CancelInitiator CancelInitiator {get;set;}
        [JsonProperty("totalCarrierCharge")]
        public decimal TotalCarrierCharge {get;set;}
        [JsonProperty("parcelTrackingNumber")]
        public string ParcelTrackingNumber {get;set;}
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
        /// This POST operation creates a shipment and purchases a shipment label.The API returns the label as either a Base64 string or a link 
        /// to a PDF.
        ///
        /// Things to Consider:
        ///    * All addresses are validated for accuracy prior to processing a shipment request.Complete addresses, including address line(s) and 
        ///    city/state/zip, are always validated by default. An error is returned if validation fails.You can modify this behavior in the 
        ///    request body’s shipmentOptions field:
        ///       - If the MINIMAL_ADDRESS_VALIDATION option is set to true, the address line(s) are not included in the validation check.Only 
        ///       the city/state/zip line is checked for validity.The address line(s) are printed on the label exactly as specified in the request.
        ///       - When the MINIMAL_ADDRESS_VALIDATION option is set to true, the shipper takes 100% responsibility for any undelivered packages 
        ///       due to violation of Carrier addressing guidelines and is responsible for any surcharge or adjustment fee levied by the carrier 
        ///       for such a violation.
        ///       - When the MINIMAL_ADDRESS_VALIDATION option is set to false (default), the complete address is included in the validation 
        ///       check, including all address line(s) and the city/state/zip line.
        ///    * In order to successfully print a shipment label, you must specify a SHIPPER_ID in shipment.shipmentOptions and set its value to 
        ///    the merchant’s postalReportingNumber, which is found in the merchant object.
        ///    * Parcels cannot measure more than 108 inches in length and girth combined, with the exception of those using USPS Parcel Select.
        ///    Parcels using Parcel Select can measure up to 130 inches in length and girth combined.
        ///    * For all parcels, the PB Shipping APIs configure length as the longest dimension, followed by height, followed by width.The APIs 
        ///    configure girth as twice the sum of the height and width: girth = 2 * (height + width)
        ///    * In addition to the limit on combined length and girth, other limits apply.Refer to the USPS rules at 
        ///    https://pe.usps.com/text/qsg300/Q201e.htm.
        ///    * For soft packs, the maximum total of length plus height cannot exceed 36 inches.The smallest dimension, or width, cannot exceed 2
        ///    inches.The measurement must be taken prior to placing items in the envelope.
        ///    * You must include the customs object in the request for shipments headed to the following destinations:
        ///       - APO/FPO
        ///       - U.S.territories (except VI and PR)
        ///       - U.S.possessions
        ///       - Freely Associated States
        ///       - Countries outside the U.S.
        ///    * The POST /shipments API call generates single-ply customs declaration forms (PS Form 2976-A and PS Form 2976-B) for participating 
        ///    countries and for APO/FPO/DPO destinations.As of this writing, the following countries participate: Australia, Canada, and Costa Rica.
        ///    * In order to include a shipment in a manifest, you must include the ADD_TO_MANIFEST option set to true in shipment.shipmentOptions.
        ///    * Shipment labels retrieved through URLs are available for 24 hours after label creation.
        /// Scan-Based Return Shipment
        /// 
        /// This operation prints a USPS scan-based return (SBR) label. Unlike prepaid return services, SBR services allow you to print return 
        /// labels without incurring postage charges at the time of print. Postage is automatically deducted only when the return label is scanned 
        /// into the USPS mail stream. If the label is not used, no charges incur.
        ///
        /// Before you can print SBR labels, you must enable SBR services on your account. To enable this service, please contact the PB support 
        /// team at: ShippingAPISupport @pb.com.
        ///
        /// For a sample scan-based return label, see: 
        ///    https://sandbox2-gcsweb.test.pb.com/usps/716434937/inbound/label/fa8ce7a41b2b4f57bd43cfc4a0f7915e.pdf
        ///
        /// Things to Consider:
        ///     * Activating this service on your developer account allows all your merchant accounts to use this service.
        ///     * The following USPS services are supported with SBR labels:
        ///        - Service Name                        Service ID  Parcel Type
        ///        - First-Class Package Return Service  FCM         PKG
        ///        - Priority Mail Return Service        PM          PKG
        ///        - Ground Return Service               PRCLSEL     PKG
        ///     * The following USPS special services can be used with the services specified above:
        ///        - Signature Confirmation (Sig)
        ///        - Insurance (Ins)
        ///     * The supported file formats for SBR labels are:
        ///        - PDF (sizes 4x6, 8x11)
        ///        - PNG(sizes 4x6, 8x11)
        ///        - Note: ZPL2 is currently not supported.
        ///     * SBR labels are supported for domestic shipments only.
        ///     * Note: You cannot print SBR labels to or from APO/FPO, US territories, or international destinations.
        /// To print an SBR label, issue a POST /shipments API call.Set the following:
        ///     * Set shipmentType to RETURN.
        ///     * Set the toAddress to the address where the merchandise is returned to.
        ///     * Set the fromAddress to the address of the user who is returning the merchandise.The following fields are required for the 
        ///     sender’s address:
        ///        - name or company
        ///        - email
        ///        - phone
        ///        - addressLines (address line 1)
        ///        - cityTown
        ///        - stateProvince
        ///        - postalCode
        ///    * Set ADD_TO_MANIFEST to false or omit it all together.Otherwise an error will be returned.
        ///    * Do not include X-PB-Shipper-Rate-Plan and X-PB-Integrator-CarrierId in the request header.
        ///    * There is no charge to print an SBR label.Postage is automatically deducted only when the return label is scanned into the USPS mail 
        ///       stream.
        ///
        /// To view transaction history for SBR labels, use the Transaction Reports API.The API returns the printStatus field, which displays a 
        /// label’s status:
        ///        - SBRPrinted: The SBR label is printed but not yet scanned in to the USPS mailstream.
        ///        - SBRCharged: The SBR label is scanned into the USPS mailstream.
        ///        - NULL: The label is not an SBR label.
        ///    * To retrieve a report that displays only SBR transactions, use the printStatus query parameter when calling the Transaction Reports 
        ///    API.
        ///    * Return labels cannot be voided.If you try to void an SBR label, an error will be returned.
        ///    *Labels retrieved through URLs are available for 24 hours after label creation.
        ///
        /// PMOD Shipment
        /// 
        /// Priority Mail Open and Distribute (PMOD) expedites the movement of lower classes of mail by using Priority Mail to send the mailings to 
        /// a destination center for processing.Shippers place mail pieces into an approved USPS Priority Mail container(sack, tray, or tub), affix 
        /// a PMOD address label to the container, and ship the container to a USPS authorized acceptance location. The postal facility opens the 
        /// container and processes the individual shipments according to their mail classes.The postage price is based on the weight of the contents 
        /// (excluding the tare weight of the external container) and regular Priority Mail distance-based prices.
        /// 
        /// To begin printing PMOD labels, contact the PB support team at ShippingAPISupport @pb.com for requirements.
        /// 
        /// Sequence for Creating PMOD Shipments
        ///     * Create PMOD labels for your shipments, as described on this page.
        ///     * Add the labels to a PMOD manifest form (PS Form 3152). See Create a Manifest for PMOD Shipments.
        ///
        /// Things to Consider:
        ///     * When issuing the API call, set the following:
        ///         - Field Value
        ///         - rates.serviceId PMOD
        ///         - rates.parcelType -Set to one of the following parcel types:
        ///             - SACK — Sack
        ///             - FTTB — Flat Tub Tray
        ///             - HTB — Half Tray Box
        ///             - FTB — Full Tray Box
        ///             - EMMTB — Extended Managed Mail Tray Box
        ///         - rates.specialServices.specialServiceId PMOD_OPTIONS
        ///         - rates.specialServices.inputParameters Add an object for each PMOD option.See PMOD Options for the list of options and their 
        ///         possible values. You must add each option to the array.
        ///         - toAddress - Set this to the destination facility. The shipper is responsible for providing the destination facility address 
        ///         information. The addressing data should be derived from the Drop Entry files located at the USPS FAST web 
        ///         site: http://fast.usps.com. If the DESTINATION_ENTRY_FACILITY PMOD option is set to DDU, include the entire destination address:
        ///         name, addressLines, cityTown, stateProvince, and postalCode. If the option is set to NDC, ADC, ASF or SCF, the address need only 
        ///         include the cityTown, stateProvince, and postalCode.
        ///    * Note: In some cases the ZIP code on a PMOD label does not correspond to the city and state referenced.For example, the Washington DC 
        ///    Network Distribution Center (NDC) is addressed as NDC WASHINGTON DC 20799. The 20799 ZIP code, however, is the Maryland ZIP code 
        ///    where the NDC is located.
        ///    * The maximum weight allowed for each container is 70 pounds.
        ///    * Stealth postage is not allowed with PMOD. You cannot hide postage. If you set the HIDE_TOTAL_CARRIER_CHARGE option to true, it will 
        ///    be ignored.
        ///    * The ADD_TO_MANIFEST option is always considered true for PMOD labels. If you set the option to false, the PB Shipping APIs ignore 
        ///    the setting and still consider the option to be true.
        ///    * After you create the PMOD label, you must add the label to PS Form 3152. See Create a Manifest for PMOD Shipments.
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
    }
}