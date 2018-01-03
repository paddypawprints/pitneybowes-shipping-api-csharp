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

namespace PitneyBowes.Developer.ShippingApi.Rules
{
    /// <summary>
    /// Root of the carrier rules. Defines allowed combination or carrier and origin and destination countries.
    /// </summary>
    public class CarrierRule : IRateRule
    {
        /// <summary>
        /// Carrier
        /// </summary>
        public Carrier Carrier { get; set; }
        /// <summary>
        /// Origin country
        /// </summary>
        public string OriginCountry { get; set; }
        /// <summary>
        /// Desitnation country
        /// </summary>
        public string DestinationCountry { get; set; }
        /// <summary>
        /// Services rules for this carrier
        /// </summary>
        public IndexedList<Services, ServiceRule> ServiceRules { get; set; }

        /// <summary>
        /// Accept method for the visitor pattern
        /// </summary>
        /// <param name="visitor"></param>
        public void Accept(IRateRuleVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
