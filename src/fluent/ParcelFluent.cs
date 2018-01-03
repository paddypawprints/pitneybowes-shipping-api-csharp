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
using PitneyBowes.Developer.ShippingApi.Model;

namespace PitneyBowes.Developer.ShippingApi.Fluent
{
    /// <summary>
    /// Class to provide fluent interface for parcel physical properties. 
    /// </summary>
    public class ParcelFluent<T> where T : IParcel, new()
    {
        private IParcel _parcel;

        /// <summary>
        /// Factory method to create a ParcelFluent. Use instead of new at the start of fluent method chain.
        /// </summary>
        /// <returns></returns>
        public static ParcelFluent<T> Create()
        {
            var p = new ParcelFluent<T>() { _parcel = new T() };
            p._parcel.CurrencyCode = "USD";
            return p;
        }

 
        private ParcelFluent()
        {
         
        }
        /// <summary>
        /// Implicit cast into the underlying IParcel class.
        /// </summary>
        /// <param name="p"></param>
        public static implicit operator T( ParcelFluent<T> p)
        {
            return (T)p._parcel;
        }
        /// <summary>
        /// Set parcel dimension
        /// </summary>
        /// <param name="l">Length</param>
        /// <param name="h">Height</param>
        /// <param name="w">Width</param>
        /// <param name="u">Units - defaults to inches</param>
        /// <returns></returns>
        public ParcelFluent<T> Dimension(decimal l, decimal h, decimal w, UnitOfDimension u = UnitOfDimension.IN) 
        {
            _parcel.Dimension = new ParcelDimension() { Length = l, Height = h, Width = w, UnitOfMeasurement = u };
            return this;
        }

        /// <summary>
        /// Set parcel weight
        /// </summary>
        /// <param name="d">Weight</param>
        /// <param name="unit">Units - defaults to OZ</param>
        /// <returns></returns>
        public ParcelFluent<T> Weight(decimal d, UnitOfWeight unit = UnitOfWeight.OZ) 
       {
            _parcel.Weight = new ParcelWeight() { Weight = d, UnitOfMeasurement = unit };
            return this;
        }
        /// <summary>
        /// Value of goods
        /// </summary>
        /// <param name="d">Value</param>
        /// <returns></returns>
        public ParcelFluent<T> ValueOfGoods( decimal d) 
        { 
            _parcel.ValueOfGoods = d;
            return this;
        }
        /// <summary>
        /// Currency of value of goods.
        /// </summary>
        /// <param name="s">Three letter currency code.</param>
        /// <returns></returns>
        public ParcelFluent<T> CurrencyCode( string s) 
        {
            _parcel.CurrencyCode = s;
            return this;
        }
        
    }
}