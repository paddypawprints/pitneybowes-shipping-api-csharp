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

namespace PitneyBowes.Developer.ShippingApi.Rules
{
    /// <summary>
    /// Extension methods to make working with the rules easier. They apply to interfaces and, therefore need to be extension methods rather than 
    /// regular methods.
    /// </summary>
    public static class ConvenienceExtensions
    {
        /// <summary>
        /// Extension method to determine if an IEnumberable contains a value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Contains<T>(this IEnumerable<T> enumerable, T value)
        {
            foreach( var t in enumerable)
            {
                if (t == null) return value == null; 
                else if (t.Equals(value)) return true;
            }
            return false;
        }
        /// <summary>
        /// Is the parcel within the boundaries set by the DimensioRule.
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static bool IsWithin(this IParcelDimension parcel, DimensionRule rule)
        {
            // TODO: convert to parcel units
            if (parcel.Height < rule.MinParcelDimensions.Height || parcel.Length < rule.MinParcelDimensions.Length || parcel.Width < rule.MinParcelDimensions.Width)
                return false;
            if (parcel.Height > rule.MaxParcelDimensions.Height || parcel.Length > rule.MaxParcelDimensions.Length || parcel.Width > rule.MaxParcelDimensions.Width)
                return false;
//            if (parcel.IrregularParcelGirth < rule.MinLengthPlusGirth)
//                return false;
//            if (parcel.IrregularParcelGirth > rule.MaxLengthPlusGirth)
 //               return false;
            return true;
        }
        /// <summary>
        /// Is the parce weight within the boundaries set by the WeightRule.
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static bool IsWithin(this IParcelWeight parcel, WeightRule rule)
        {
            // TODO: convert to parce units
            if (parcel.Weight < rule.MinWeight) return false;
            if (parcel.Weight > rule.MaxWeight) return false;
            return true;
        }
    }
}
