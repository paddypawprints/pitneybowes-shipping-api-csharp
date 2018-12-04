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

using System.Collections.Generic;

namespace PitneyBowes.Developer.ShippingApi.Model
{
    /// <summary>
    /// Object to create a shipment and purchases a shipment label. The API returns the label as either a Base64 string or a link to a PDF.
    /// </summary>
    public class Shipment : IShipment
    {
        /// <summary>
        /// Shipper rate plan, if available.
        /// Important: Do not include this header if creating a scan-based return (SBR) label.
        /// </summary>
        virtual public string ShipperRatePlan { get; set; }
        /// <summary>
        /// Negotiated services rate, if applicable.
        /// </summary>
        virtual public string IntegratorCarrierId { get; set; }
        /// <summary>
        /// Negotiated services rate, if applicable.
        /// </summary>
        virtual public string IntegratorRatePlan { get; set; }
        /// <summary>
        /// Integrator Id
        /// </summary>
        virtual public string IntegratorId { get; set; }
        /// <summary>
        /// MinimalAddressValidation header option
        /// </summary>
        virtual public string MinimalAddressValidation { get; set; }
        /// <summary>
        /// Request devlivery commitment
        /// </summary>
        virtual public bool IncludeDeliveryCommitment { get; set; }
        /// <summary>
        /// REQUIRED. A unique identifier for each transaction that cannot exceed 25 characters.
        /// </summary>
        virtual public string TransactionId { get; set; }
        /// <summary>
        /// REQUIRED. Origin address. See Create a Shipment for considerations when specifying multiple address lines when using 
        /// MINIMAL_ADDRESS_VALIDATION.
        /// </summary>
        virtual public IAddress FromAddress { get; set; }
        /// <summary>
        /// REQUIRED.Destination address.
        /// Note: You can specify multiple address lines in the shipment抯 destination address.See address object for information on how 
        /// the API processes multiple address lines.
        /// </summary>
        virtual public IAddress ToAddress { get; set; }
        /// <summary>
        /// INTERNATIONAL SHIPMENTS ONLY. Required if the return shipment is not going to the fromAddress but is instead to an alternate 
        /// return address.
        /// </summary>
        virtual public IAddress AltReturnAddress { get; set; }
        /// <summary>
        /// REQUIRED. Contains physical characteristics of the parcel.
        /// </summary>
        virtual public IParcel Parcel { get; set; }
        /// <summary>
        /// REQUIRED. Information related to the shipment rates.
        /// </summary>
        virtual public IEnumerable<IRates> Rates { get; set; }
        /// <summary>
        /// Add Rates object to the Rates IEnumerable
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        virtual public IRates AddRates(IRates r)
        {
            return ModelHelper.AddToEnumerable<IRates, Rates>(r, () => Rates, (x) => Rates = x);
        }
        /// <summary>
        /// A list of shipment documents pertaining to a shipment, including the label.
        /// </summary>
        virtual public IEnumerable<IDocument> Documents { get; set; }
        /// <summary>
        /// Add a Document to the Documents IEnumerable
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        virtual public IDocument AddDocument(IDocument d)
        {
            return ModelHelper.AddToEnumerable<IDocument, Document>(d, () => Documents, (x) => Documents = x);
        }
        /// <summary>
        /// Each object in this array defines a shipment option. The available options depend on the carrier, origin country, and destination country.
        /// If you are creating a shipment, this array is required and must contain the SHIPPER_ID option.
        /// </summary>
        virtual public IEnumerable<IShipmentOptions> ShipmentOptions { get; set; }
        /// <summary>
        /// Add option to the ShipmentOptions IEnumerable
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        virtual public IShipmentOptions AddShipmentOptions(IShipmentOptions s)
        {
            return ModelHelper.AddToEnumerable<IShipmentOptions, ShipmentOptions>(s, () => ShipmentOptions, (x) => ShipmentOptions = x);
        }
        /// <summary>
        /// SBR LABELS ONLY.
        ///
        /// If you are creating a scan-based return (SBR) label, set this to RETURN.
        /// </summary>
        virtual public ShipmentType ShipmentType { get; set; }
        /// <summary>
        /// ONLY FOR: international, APO/FPO/DPO, territories/possessions, and FAS shipments. Customs related information.
        /// </summary>
        virtual public ICustoms Customs { get; set; }
        /// <summary>
        /// Unique identifier for the shipment, generated by the system in response to shipment purchase.
        /// </summary>
        virtual public string ShipmentId { get; set; }
        /// <summary>
        /// Tracking number assigned to the shipment by the system.
        /// </summary>
        virtual public string ParcelTrackingNumber { get; set; }
        /// <summary>
        /// Newgistics Only. This array maps client-generated identifiers to fields in the Newgistics package record. 
        /// The information in this array does not appear on the shipping label. The array takes up to three objects, 
        /// and each object maps an identifier to a specific Newgistics field.An object’s sequence in the array 
        /// determines which Newgistics field the object maps to.The first object in the array maps to the Newgistics 
        /// “ReferenceNumber” field; the second to the “AddlRef1” field; and the third to the “AddlRef2” field.
        /// </summary>
        /// <value>The references.</value>
        public IEnumerable<IReference> References { get; set; }
    }
}