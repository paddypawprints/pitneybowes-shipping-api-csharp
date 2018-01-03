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
using Newtonsoft.Json;

namespace PitneyBowes.Developer.ShippingApi.Rules
{
    /// <summary>
    /// Constraints you must following if you select this special service. If the carrier requires input for the special 
    /// service, these are the parameters governing input.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ServicesParameterRule
    {
        /// <summary>
        /// The type of constraint. This is usually set to INPUT_VALUE.
        /// </summary>
        [JsonProperty("name")]
        public string Name{get; set;}
        /// <summary>
        /// The full name of the parcel type.
        /// </summary>
        [JsonProperty("brandedName")]
        public string BrandedName{get; set;}
        /// <summary>
        /// If true, this constraint must be followed.
        /// </summary>
        [JsonProperty("required")]
        public Boolean Required{get; set;}
        /// <summary>
        /// The minimum input value for this parcel type rule.
        /// </summary>
        [JsonProperty("minValue")]
        public Decimal MinValue{get; set;}
        /// <summary>
        /// The maximum input value for this parcel type rule.
        /// </summary>
        [JsonProperty("maxValue")]
        public Decimal MaxValue{get; set;}
        /// <summary>
        /// An amount that is automatically provided for this parcel type rule. You do not need to include anything equal 
        /// to or below this amount in your request, as it is already provided.
        /// </summary>
        [JsonProperty("freeValue")]
        public Decimal FreeValue{get; set;}
        /// <summary>
        /// Format
        /// </summary>
        [JsonProperty("format")]
        public string Format{get; set;}
        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty("description")]
        public string Description{get; set;}
    }
}
