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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PitneyBowes.Developer.ShippingApi.Rules
{
    /// <summary>
    /// At the top level, rules are organized by service type. Within a service type, each set of rules applies to a combination of parcel 
    /// type(parcelTypeRules.parcelType) and rate type(parcelTypeRules.rateTypeId).
    ///
    /// Rules tell you:
    ///     * Compatible special services(parcelTypeRules.specialServiceRules)
    ///     * Required special services(parcelTypeRules.specialServiceRules.prerequisiteRules)
    ///     * Incompatible special services(parcelTypeRules.specialServiceRules.incompatibleSpecialServices)
    ///     * Required input parameters(parcelTypeRules.specialServiceRules.inputParameterRules)
    ///     * Weight constraints(parcelTypeRules.weightRules)
    ///     * Dimension constraints(parcelTypeRules.dimensionRules)
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ServiceRule : IRateRule
    {
        /// <summary>
        ///  The abbreviated name of the carrier-specific service.
        /// </summary>
        [JsonProperty("serviceId")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Services ServiceId { get; set; }
        /// <summary>
        /// The full name of the service.
        /// </summary>
        [JsonProperty("brandedName")]
        public string BrandedName { get; set; }
        /// <summary>
        /// 	The available service options.
        /// </summary>
        public IndexedList<ParcelType, ParcelTypeRule> ParcelTypeRules { get; internal set; }

        [JsonProperty("parcelTypeRules")]
        internal IEnumerable<ParcelTypeRule> SerializerParcelTypeRules {
            get => ParcelTypeRules;
            set
            {
                if (ParcelTypeRules == null) ParcelTypeRules = new IndexedList<ParcelType, ParcelTypeRule>();
                foreach( var r in value)
                {
                    ParcelTypeRules.Add(r.ParcelType, r);
                }
                
            }
        }
        /// <summary>
        /// Required for visitor interface
        /// </summary>
        /// <param name="visitor"></param>
        public void Accept(IRateRuleVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
