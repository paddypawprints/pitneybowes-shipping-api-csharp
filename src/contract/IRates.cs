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

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Information related to the shipment rates.
    /// </summary>
    public interface IRates
    {
        /// <summary>
        /// REQUIRED. Carrier name. Valid values include: USPS
        /// </summary>
        Carrier Carrier { get; set; }
        /// <summary>
        /// Carrier service
        /// </summary>
        Services? ServiceId { get; set; }
        /// <summary>
        /// The parcel type
        /// </summary>
        ParcelType? ParcelType { get; set; }
        /// <summary>
        /// The requested special services.
        /// In a return object, this includes the service fees and optional tax information.
        /// </summary>
        IEnumerable<ISpecialServices> SpecialServices { get; set; }
        /// <summary>
        /// Add a special service to the SpecialServices IEnumerable
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        ISpecialServices AddSpecialservices(ISpecialServices s);
        /// <summary>
        /// Postal code where the shipment is tendered to the carrier. Postal code of Shipment fromAddress is used in absence of this field.
        ///
        /// When an inductionPostalCode is present, this postal code is used instead of the postal code in the shipment’s fromAddress when 
        /// calculating rates and when determining if the shipment can be added to a manifest.
        /// </summary>
        string InductionPostalCode { get; set; }
        /// <summary>
        /// Volumetric weight calculated based on weight and volume.
        /// </summary>
        IParcelWeight DimensionalWeight { get; set; }
        /// <summary>
        /// Response only - The base service charge payable to the carrier, excluding special service charges.
        /// </summary>
        decimal BaseCharge { get; set; }
        /// <summary>
        /// The total amount payable to the carrier, including special service charges.
        /// </summary>
        decimal TotalCarrierCharge { get; set; }
        /// <summary>
        /// Response only. Base service charge, excluding special service charges, payable by the shipper.
        /// These rates are based on the X-PB-Shipper-Rate-Plan and are included only when the request header includes the X-PB-Shipper-Rate-Plan 
        /// parameter.
        /// Do not use this field for shippers with their own NSAs (USPS Negotiated Service Agreements). Use baseCharge instead.
        /// </summary>
        decimal AlternateBaseCharge { get; set; }
        /// <summary>
        /// Response only. Total charge, including special service charges, payable by the shipper.
        /// These rates are based on the X-PB-Shipper-Rate-Plan and are included only when the request header includes the X-PB-Shipper-Rate-Plan 
        /// parameter.
        /// Do not use this field for shippers with their own NSAs (USPS Negotiated Service Agreements). Use totalCarrierCharge instead.
        /// </summary>
        decimal AlternateTotalCharge { get; set; }
        /// <summary>
        /// Time in transit for the shipment.
        /// </summary>
        IDeliveryCommitment DeliveryCommitment { get; set; }
        /// <summary>
        /// Type of currency referenced in the piece price. For example: USD, CAD, EUR
        /// </summary>
        string CurrencyCode { get; set; }
        /// <summary>
        /// Response only. Destination Zone based on the fromAddress and toAddress specified.
        /// </summary>
        int? DestinationZone { get; set; }
    }
}