
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
using PitneyBowes.Developer.ShippingApi.Model;


namespace PitneyBowes.Developer.ShippingApi.Fluent
{
    /// <summary>
    /// Set values in an array of shipment options
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ShipmentOptionsArrayFluent<T> : List<T> where T : class, IShipmentOptions, new()
    {
        /// <summary>
        /// Static factory method to start the fluent method chain.
        /// </summary>
        /// <returns></returns>
        public static ShipmentOptionsArrayFluent<T> Create()
        {
            return new ShipmentOptionsArrayFluent<T>();
        }
        /// <summary>
        /// Current array member - method that set values operate on the current member.
        /// </summary>
        protected T _current = null;
        /// <summary>
        /// Add a new item to the array and set the current member to theis object.
        /// </summary>
        /// <returns></returns>
        public ShipmentOptionsArrayFluent<T> Add() 
        {
            Add(new T());
            _current = FindLast((x) => true);
            return this;
        }
        /// <summary>
        /// Set the current option as the first member of the array.
        /// </summary>
        /// <returns></returns>
        public ShipmentOptionsArrayFluent<T> First()
        {
            _current = Find((x) => true);
            return this;
        }
        /// <summary>
        /// Set the current option to be the next member of the array.
        /// </summary>
        /// <returns></returns>
        public ShipmentOptionsArrayFluent<T> Next()
        {
            var i = IndexOf(_current);
            _current = this[i + 1];
            return this;
        }
        /// <summary>
        /// If true, this is the last element in the array.
        /// </summary>
        /// <returns></returns>
        public bool IsLast()
        {
            var i = IndexOf(_current);
            return (i == Count - 1);
        }
        /// <summary>
        /// Set the values of the current option.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ShipmentOptionsArrayFluent<T> Option(ShipmentOption option, string value ) 
        {
            _current.ShipmentOption = option;
            _current.Value = value;
            return this;
        }
        /// <summary>
        /// Set the shipper ID
        /// </summary>
        /// <param name="shipperId"></param>
        /// <returns></returns>
        public ShipmentOptionsArrayFluent<T> ShipperId( string shipperId)
        {
            return Add().Option(ShipmentOption.SHIPPER_ID, shipperId);
        }
        /// <summary>
        /// Set the ADD_TO_MANIFEST flag, add it to the array.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ShipmentOptionsArrayFluent<T> AddToManifest(bool value = true)
        {
            return Add().Option(ShipmentOption.ADD_TO_MANIFEST, value.ToString());
        }
        /// <summary>
        /// Set the MINIMAL_ADDRESS_VALIDATION flag, add it to the array.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ShipmentOptionsArrayFluent<T> MinimalAddressvalidation(bool value = true)
        {
            return Add().Option(ShipmentOption.MINIMAL_ADDRESS_VALIDATION, value.ToString());
        }
        /// <summary>
        /// Set shipment option value, add it ot the array.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ShipmentOptionsArrayFluent<T> AddOption(ShipmentOption option, string value)
        {
            return Add().Option(option, value);
        }

    }
}