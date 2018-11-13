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
    /// Root interface for customs clearance information that is used to fill out a commercial invoice
    /// </summary>
    public interface ICustoms
    {
        /// <summary>
        /// Customs clearance information that is used to fill out a commercial invoice
        /// </summary>
        ICustomsInfo CustomsInfo { get; set; }
        /// <summary>
        /// The commodity information about each item in an international shipment
        ///used for customs clearance.
        ///The maximum number of objects in the array is **30**.
        /// </summary>
        /// <value>The customs items.</value>
        IEnumerable<ICustomsItems> CustomsItems { get; set; }
        /// <summary>
        /// Add a new CustomsItem to CustomsItems IEnumberable
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        ICustomsItems AddCustomsItems(ICustomsItems c);
    }

    public static partial class InterfaceValidators
    {
        /// <summary>
        /// If false, the object underlying the interface is not valid. If true, the object may or may not be valid.
        /// </summary>
        /// <param name="customs"></param>
        /// <returns></returns>
        public static bool IsValid(this ICustoms customs)
        {
            if (!customs.CustomsInfo.IsValid()) return false;
            if (customs.CustomsItems == null) return true;
            int count = 0;
            foreach( var i in customs.CustomsItems )
            {
                count++;
                if (!i.IsValid()) return false;
            }
            return count <= 30;
        }
    }
}