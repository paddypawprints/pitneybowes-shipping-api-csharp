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

namespace PitneyBowes.Developer.ShippingApi.Rules
{
    /// <summary>
    /// The special services applicable for this combination of service type, rate type, and parcel type.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SpecialServicesRule : IRateRule
    {
        /// <summary>
        /// The abbreviated name of the special service. 
        /// </summary>
        [JsonProperty("specialServiceId")]
        public SpecialServiceCodes SpecialServiceId { get; set; }
        /// <summary>
        /// The full name of the special service.
        /// </summary>
        [JsonProperty("brandedName")]
        public string BrandedName { get; set; }
        /// <summary>
        /// The ID of the special service rate category.
        /// </summary>
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }
        /// <summary>
        /// The full name of the special service rate category.
        /// </summary>
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }
        /// <summary>
        /// If true, then applying the special service to the shipment allows the shipper to track the shipment.
        /// </summary>
        [JsonProperty("trackable")]
        public bool Trackable { get; set; }
        /// <summary>
        /// Constraints you must following if you select this special service. If the carrier requires input for the special service, 
        /// these are the parameters governing input.
        /// </summary>
        public Dictionary<string, ServicesParameterRule> InputParameterRules { get; set; }
        /// <summary>
        /// Prerequisites for applying the special service. If you select this special service, you must also select the other special
        /// services in this array.
        /// </summary>
        public IndexedList<SpecialServiceCodes, ServicesPrerequisiteRule> PrerequisiteRules { get; set; }
        /// <summary>
        /// 	Special services that you cannot use with this special service. If you select this special service, you cannot select any of the 
        /// 	special services in this array.
        /// </summary>
        [JsonProperty("incompatibleSpecialServices")]
        public IEnumerable<SpecialServiceCodes> IncompatibleSpecialServices { get; set; }
        [JsonProperty("inputParameterRules")]
        internal IEnumerable<ServicesParameterRule> SerializerInputParameterRules
        {
            get
            {
                if (InputParameterRules == null) return null;
                else return InputParameterRules.Values;
            }
            set
            {
                if (InputParameterRules == null) InputParameterRules = new Dictionary<string, ServicesParameterRule>();
                foreach(var p in value)
                {
                    InputParameterRules.Add(p.Name, p);
                }
            }
        }
        [JsonProperty("prerequisiteRules")]
        internal IEnumerable<ServicesPrerequisiteRule> SerializerPrerequisiteRules
        {
            get => PrerequisiteRules;
            set
            {
                if (PrerequisiteRules == null) PrerequisiteRules = new IndexedList<SpecialServiceCodes, ServicesPrerequisiteRule>();
                foreach(var p in value)
                {
                    PrerequisiteRules.Add(p.SpecialServiceId, p);
                }
            }
        }

        /// <summary>
        /// Visitor pattern Accept method
        /// </summary>
        /// <param name="visitor"></param>
        public void Accept(IRateRuleVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Check parameter value against the rules to see if the value is within min and max values 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public bool IsValidParameterValues(ISpecialServices services)
        {
            foreach (var ip in services.InputParameters)
            {
                if (!InputParameterRules.ContainsKey(ip.Name)) return false;
                if (decimal.TryParse(ip.Value, out decimal value))
                {
                    if (value < InputParameterRules[ip.Name].MinValue) return false;
                    if (value > InputParameterRules[ip.Name].MaxValue) return false;
                }
            }
             return true;
        }

        /// <summary>
        /// Checks a special service against the rules to see whter it has the required input parameters
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public bool HasRequiredParameters(ISpecialServices services)
        {
            Dictionary<string, bool> foundRequired = new Dictionary<string, bool>();
            foreach (var p in InputParameterRules.Values)
            {
                if (p.Required) foundRequired.Add(p.Name, false);
            }
            foreach (var ip in services.InputParameters)
            {
                if (!InputParameterRules.ContainsKey(ip.Name)) return false;
                if (foundRequired.ContainsKey(ip.Name)) foundRequired[ip.Name] = true;
            }
            foreach (var f in foundRequired)
            {
                if (!f.Value) return false;
            }
            return true;
        }
        /// <summary>
        /// Checks a number of special services against the rules to see whether prerequisite services are present
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public bool IsValidPrerequisites(IEnumerable<ISpecialServices> services)
        {
            if (PrerequisiteRules == null) return true;
            foreach(var ss in services)
            {
                if (!PrerequisiteRules.ContainsKey(ss.SpecialServiceId)) continue;
                foreach(var r in PrerequisiteRules[ss.SpecialServiceId])
                {
                    if ( ss.Value < r.MinInputValue ) return false;
                }
            }
            return true;
        }
    }
}
