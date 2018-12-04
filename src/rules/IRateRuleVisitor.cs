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

namespace PitneyBowes.Developer.ShippingApi.Rules
{
    /// <summary>
    /// Interface for all rate rule visitor classes
    /// </summary>
    public interface IRateRuleVisitor
    {
        /// <summary>
        /// Visit CarrierRule node
        /// </summary>
        /// <param name="carrierRule"></param>
        void Visit(CarrierRule carrierRule);
        /// <summary>
        /// Visit ServiceRule node
        /// </summary>
        /// <param name="serviceRule"></param>
        void Visit(ServiceRule serviceRule);
        /// <summary>
        /// Visit ParcelRule node
        /// </summary>
        /// <param name="parcelRule"></param>
        void Visit(ParcelTypeRule parcelRule);
        /// <summary>
        /// Visit SpecialServices rule node
        /// </summary>
        /// <param name="specialServicesRule"></param>
        void Visit(SpecialServicesRule specialServicesRule);
    }
}
