// /*
// Copyright 2018 Pitney Bowes Inc.
//
// Licensed under the MIT License(the "License"); you may not use this file except in compliance with the License.  
// You may obtain a copy of the License in the README file or at
//    https://opensource.org/licenses/MIT 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed 
// on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the License 
// for the specific language governing permissions and limitations under the License.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// */

namespace PitneyBowes.Developer.ShippingApi.Model
{
    /// <summary>
    /// 4X8 Labels Only. In a shipment request, this defines additional information to be printed on the 2-inch Doc Tab of a 4X8 label. 
    /// You can specify information returned by the Create Shipment API as well as custom information. 
    /// </summary>
    public class DocTab: IDocTab
    {
        /// <summary>
        /// The field to be printed. This can be an existing field returned by the Create Shipment API call or a custom field that you define. 
        /// Each object in the docTab array must have a name value.
        /// </summary>
        /// <value>The name.</value>
        virtual public string Name { get; set; }
        /// <summary>
        /// <summary>
        /// If docTab.name is set to a field returned by the Create Shipment API call, this defines how the field name is displayed on the label.
        /// For example, if you set the following:
        ///     { "name": "parcelTrackingNumber","displayName": "Tracking Number" }
        /// The label will display:
        ///     Tracking Number: &lt;parcelTrackingNumber&gt; where &lt;parcelTrackingNumber&gt; is the value of the parcelTrackingNumber field.
        /// </summary>
        /// <value>The display name.</value>
        /// </summary>
        /// <value>The display name.</value>
        virtual public string DisplayName { get; set; }
        /// <summary>
        /// To display a field and value that are not returned in Create Shipment, enter the new name in the docTab.name field and enter 
        /// the value here. For example: { "name": "DiscountCode", "value": "JUN40" }
        /// </summary>
        /// <value>The value.</value>
        virtual public string Value { get; set; }
    }
}
