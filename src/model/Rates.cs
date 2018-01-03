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

namespace PitneyBowes.Developer.ShippingApi.Model
{
    /// <summary>
    /// Information related to the shipment rates.
    /// </summary>
    public class Rates : IRates
    {
        /// <summary>
        /// REQUIRED. Carrier name. Valid values include: USPS
        /// </summary>
        virtual public Carrier Carrier { get; set;}
        /// <summary>
        /// Carrier service
        /// </summary>
        virtual public Services ServiceId { get;set;}
        /// <summary>
        /// The parcel type
        /// </summary>
        virtual public ParcelType ParcelType { get; set;}
        /// <summary>
        /// The requested special services.
        /// In a return object, this includes the service fees and optional tax information.
        /// </summary>
        virtual public IEnumerable<ISpecialServices> SpecialServices { get; set; }
        /// <summary>
        /// Add a special service to the SpecialServices IEnumerable
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        virtual public ISpecialServices AddSpecialservices( ISpecialServices s)
        {
            return ModelHelper.AddToEnumerable<ISpecialServices, SpecialServices>(s, () => SpecialServices, (x) => SpecialServices = x);
        }
        /// <summary>
        /// Postal code where the shipment is tendered to the carrier. Postal code of Shipment fromAddress is used in absence of this field.
        ///
        /// When an inductionPostalCode is present, this postal code is used instead of the postal code in the shipment’s fromAddress when calculating rates and when determining if the shipment can be added to a manifest.
        /// </summary>
        virtual public string InductionPostalCode { get; set;}
        /// <summary>
        /// Volumetric weight calculated based on weight and volume.
        /// </summary>
        virtual public IParcelWeight DimensionalWeight { get; set; }
        /// <summary>
        /// Response only - The base service charge payable to the carrier, excluding special service charges.
        /// </summary>
        virtual public decimal BaseCharge { get; set;}
        /// <summary>
        /// Response only - The total amount payable to the carrier, including special service charges.
        /// </summary>
        virtual public decimal TotalCarrierCharge { get; set;}
        /// <summary>
        /// Response only. Base service charge, excluding special service charges, payable by the shipper.
        /// These rates are based on the X-PB-Shipper-Rate-Plan and are included only when the request header includes the X-PB-Shipper-Rate-Plan 
        /// parameter.
        /// Do not use this field for shippers with their own NSAs (USPS Negotiated Service Agreements). Use baseCharge instead.
        /// </summary>
        virtual public decimal AlternateBaseCharge { get; set;}
        /// <summary>
        /// Response only. Total charge, including special service charges, payable by the shipper.
        /// These rates are based on the X-PB-Shipper-Rate-Plan and are included only when the request header includes the X-PB-Shipper-Rate-Plan 
        /// parameter.
        /// Do not use this field for shippers with their own NSAs (USPS Negotiated Service Agreements). Use totalCarrierCharge instead.
        /// </summary>
        virtual public decimal AlternateTotalCharge { get; set;}
        /// <summary>
        /// Time in transit for the shipment.
        /// </summary>
        virtual public IDeliveryCommitment DeliveryCommitment { get; set; }
        /// <summary>
        /// Type of currency referenced in the piece price. For example: USD, CAD, EUR
        /// </summary>
        virtual public string CurrencyCode { get; set;}
        /// <summary>
        /// Response only. Destination Zone based on the fromAddress and toAddress specified.
        /// </summary>
        virtual public int DestinationZone { get; set;}
    }
}