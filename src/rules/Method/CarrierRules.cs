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

namespace PitneyBowes.Developer.ShippingApi.Rules
{
    /// <summary>
    /// Carrier rules (rating-services) request object
    /// </summary>
    public class RatingServicesRequest : ShippingApiRequest
    {
        /// <summary>
        /// http header content-type. application/json.
        /// </summary>
        public override string ContentType => "application/json";
        /// <summary>
        /// OAUTH token.
        /// </summary>
        [ShippingApiHeader("Bearer")]
        public override StringBuilder Authorization {get;set;}
        /// <summary>
        /// REQUIRED. The carrier name. Currently this must be set to: USPS
        /// </summary>
        [ShippingApiQuery("carrier")]
        public Carrier Carrier { get; set; }
        /// <summary>
        /// REQUIRED. The two-character ISO country code for the country where the shipment originates.
        /// </summary>
        [ShippingApiQuery("originCountryCode")]
        public string OriginCountryCode { get; set; }
        /// <summary>
        /// REQUIRED. The two-character ISO country code for the country of the shipment’s destination address.
        /// </summary>
        [ShippingApiQuery("destinationCountryCode")]
        public string DestinationCountryCode { get; set; }
    }

    /// <summary>
    /// Before posting a shipment, you can retrieve the carrier’s shipment rules, including the available services and parcel types, with 
    /// weight and dimension restrictions.
    ///
    /// Things to Consider:
    ///     * The call to retrieve rate rules is an expensive API call because of the size of the returned data.It is recommended that you 
    ///     cache the returned data and make the API call only once a day.
    ///     * At the top level, rules are organized by service type.
    ///     * Within a service type, each set of rules applies to a combination of parcel type (parcelTypeRules.parcelType) and rate type 
    ///     (parcelTypeRules.rateTypeId).
    ///
    /// Rules tell you:
    ///     * Compatible special services (parcelTypeRules.specialServiceRules)
    ///     * Required special services (parcelTypeRules.specialServiceRules.prerequisiteRules)
    ///     * Incompatible special services (parcelTypeRules.specialServiceRules.incompatibleSpecialServices)
    ///     * Required input parameters (parcelTypeRules.specialServiceRules.inputParameterRules)
    ///     * Weight constraints (parcelTypeRules.weightRules)
    ///     * Dimension constraints (parcelTypeRules.dimensionRules)
    /// </summary>
    public static class CarrierRulesMethods
    {
        /// <summary>
        /// Before posting a shipment, you can retrieve the carrier’s shipment rules, including the available services and parcel types, with 
        /// weight and dimension restrictions.
        ///
        /// Things to Consider:
        ///     * The call to retrieve rate rules is an expensive API call because of the size of the returned data.It is recommended that you 
        ///     cache the returned data and make the API call only once a day.
        ///     * At the top level, rules are organized by service type.
        ///     * Within a service type, each set of rules applies to a combination of parcel type (parcelTypeRules.parcelType) and rate type 
        ///     (parcelTypeRules.rateTypeId).
        ///
        /// Rules tell you:
        ///     * Compatible special services (parcelTypeRules.specialServiceRules)
        ///     * Required special services (parcelTypeRules.specialServiceRules.prerequisiteRules)
        ///     * Incompatible special services (parcelTypeRules.specialServiceRules.incompatibleSpecialServices)
        ///     * Required input parameters (parcelTypeRules.specialServiceRules.inputParameterRules)
        ///     * Weight constraints (parcelTypeRules.weightRules)
        ///     * Dimension constraints (parcelTypeRules.dimensionRules)
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        /// </summary>
        public async static Task<ShippingApiResponse<CarrierRule>> RatingServices(RatingServicesRequest request, ISession session = null) 
        {
            var response =  await WebMethod.Get<ServiceRule[], RatingServicesRequest>("/shippingservices/v1/information/rules/rating-services", request, session);
            var carrierRuleResponse = new ShippingApiResponse<CarrierRule>
            {
                Errors = response.Errors,
                HttpStatus = response.HttpStatus,
                Success = response.Success
            };
            if (response.Success)
            {
                carrierRuleResponse.APIResponse = new CarrierRule()
                {
                    Carrier = request.Carrier,
                    DestinationCountry = request.DestinationCountryCode,
                    OriginCountry = request.OriginCountryCode,
                    ServiceRules = new IndexedList<Services, ServiceRule>()
                };
                foreach (var s in response.APIResponse)
                {
                    carrierRuleResponse.APIResponse.ServiceRules.Add(s.ServiceId, s);
                }
            }
            return carrierRuleResponse;

        }
    }

}
  