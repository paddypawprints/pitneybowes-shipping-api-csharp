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

using System.Collections.Generic;

namespace PitneyBowes.Developer.ShippingApi
{
    public interface IShipment
    {
        /// <summary>
        /// REQUIRED. A unique identifier for each transaction that cannot exceed 25 characters.
        /// </summary>
        string TransactionId { get; set; }
        string MinimalAddressValidation { get; set; }
        /// <summary>
        /// Shipper rate plan, if available.
        /// Important: Do not include this header if creating a scan-based return (SBR) label.
        /// </summary>
        string ShipperRatePlan { get; set; }
        /// <summary>
        /// REQUIRED. Origin address. See Create a Shipment for considerations when specifying multiple address lines when using 
        /// MINIMAL_ADDRESS_VALIDATION.
        /// </summary>
        IAddress FromAddress { get; set; }
        /// <summary>
        /// REQUIRED.Destination address.
        /// Note: You can specify multiple address lines in the shipment’s destination address.See address object for information on how 
        /// the API processes multiple address lines.
        /// </summary>
        IAddress ToAddress { get; set; }
        /// <summary>
        /// INTERNATIONAL SHIPMENTS ONLY. Required if the return shipment is not going to the fromAddress but is instead to an alternate 
        /// return address.
        /// </summary>
        IAddress AltReturnAddress { get; set; }
        /// <summary>
        /// REQUIRED. Contains physical characteristics of the parcel.
        /// </summary>
        IParcel Parcel { get; set; }
        /// <summary>
        /// REQUIRED. Information related to the shipment rates.
        /// </summary>
        IEnumerable<IRates> Rates { get; set; }
        IRates AddRates(IRates r);
        /// <summary>
        /// A list of shipment documents pertaining to a shipment, including the label.
        /// </summary>
        IEnumerable<IDocument> Documents { get; set; }
        IDocument AddDocument(IDocument d);
        /// <summary>
        /// Each object in this array defines a shipment option. The available options depend on the carrier, origin country, and destination country.
        /// If you are creating a shipment, this array is required and must contain the SHIPPER_ID option.
        /// </summary>
        IEnumerable<IShipmentOptions> ShipmentOptions { get; set; }
        IShipmentOptions AddShipmentOptions(IShipmentOptions o);
        /// <summary>
        /// ONLY FOR: international, APO/FPO/DPO, territories/possessions, and FAS shipments. Customs related information.
        /// </summary>
        ICustoms Customs { get; set; }
        ShipmentType ShipmentType { get; set; }
        string ShipmentId { get; set; }
        string ParcelTrackingNumber { get; set; }
    }
}