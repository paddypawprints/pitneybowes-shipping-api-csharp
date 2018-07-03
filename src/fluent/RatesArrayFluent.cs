using Newtonsoft.Json;
using System.Collections.Generic;
using PitneyBowes.Developer.ShippingApi.Model;

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

namespace PitneyBowes.Developer.ShippingApi.Fluent
{
    /// <summary>
    /// Information related to the shipment rates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RatesArrayFluent<T> : List<T> where T : class, IRates, new()
    {
        /// <summary>
        /// Current element of the array. Allows fluent methods to operate on a particular element - usually the last one.
        /// </summary>
        protected T _current = null;
        /// <summary>
        /// Factory method to create a RatesArrayFluent. Use instead of new(). Use at the start of the fluent method chain.
        /// </summary>
        /// <returns></returns>
        public static RatesArrayFluent<T> Create()
        {
            return new RatesArrayFluent<T>();
        }
        /// <summary>
        /// Add a new Rates object to the end of the RatesArray. Set Current to this object
        /// </summary>
        /// <returns></returns>
        public RatesArrayFluent<T> Add() 
        {
            Add(new T());
            _current = FindLast((x) => true);
            return this;
        }
        /// <summary>
        /// Set current to the first element of the RatesArray
        /// </summary>
        /// <returns></returns>
        public RatesArrayFluent<T> First()
        {
            _current = Find((x) => true);
            return this;
        }
        /// <summary>
        /// Return Rates object at the Current position
        /// </summary>
        /// <returns></returns>
        public T Current()
        {
            return _current;
        }

        /// <summary>
        /// Set Current to the next element in the RatesArray
        /// </summary>
        /// <returns></returns>
        public RatesArrayFluent<T> Next()
        {
            var i = IndexOf(_current);
            _current = this[i + 1];
            return this;
        }
        /// <summary>
        /// If true, Current is the last element in the RatesArray
        /// </summary>
        /// <returns></returns>
        public bool IsLast()
        {
            var i = IndexOf(_current);
            return (i == Count - 1);
        }
        /// <summary>
        /// REQUIRED. Carrier name. Valid values include: USPS
        /// </summary>
        /// <param name="c">The carrier</param>
        /// <returns></returns>
        public RatesArrayFluent<T> Carrier(Carrier c) 
        {
            _current.Carrier = c;
            return this;
        }

        /// <summary>
        /// Carrier service
        /// </summary>
        /// <param name="s">The service</param>
        /// <returns></returns>
        public RatesArrayFluent<T> Service(Services s) 
        {
            _current.ServiceId = s;
            return this;
        }
        /// <summary>
        /// Used in rating call to rate shop service.
        /// </summary>
        /// <returns></returns>
        public RatesArrayFluent<T> RateShopService()
        {
            _current.ServiceId = null;
            return this;
        }
        /// <summary>
        /// The parcel type
        /// </summary>
        /// <param name="t">Parcel type</param>
        /// <returns></returns>
        public RatesArrayFluent<T> ParcelType(ParcelType t) 
        {
            _current.ParcelType = t;
            return this;
        }
        /// <summary>
        /// Used in rating call to rate shop parcel type.
        /// </summary>
        /// <returns></returns>
        public RatesArrayFluent<T> RateShopParcelType()
        {
            _current.ParcelType = null;
            return this;
        }
        /// <summary>
        /// Add a special service.
        /// In a return object, this includes the service fees and optional tax information.
        /// </summary>
        /// <typeparam name="S">Concrete type for ISpecialService</typeparam>
        /// <param name="c">Special service code</param>
        /// <param name="f">Fee</param>
        /// <param name="parameters">Parameters</param>
        /// <returns></returns>
        public RatesArrayFluent<T> SpecialService<S>(SpecialServiceCodes c, decimal f, params IParameter[] parameters) where S:ISpecialServices, new()
        {
            var s = new S() { SpecialServiceId = c, Fee = f };

            foreach( var p in parameters )
            {
                s.AddParameter(p);
            }
            _current.AddSpecialservices(s);
            return this;
        }
        /// <summary>
        /// Add a special servicve to the end of the services array
        /// </summary>
        /// <typeparam name="S">Concrete type for ISpecialService</typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public RatesArrayFluent<T> SpecialService<S>(S s) where S : ISpecialServices, new()
        {
            _current.AddSpecialservices(s);
            return this;
        }
        /// <summary>
        /// Sets the postal code for the current rates item where the shipment is tendered to the carrier. Postal code of Shipment fromAddress is 
        /// used in absence of this field. When an inductionPostalCode is present, this postal code is used instead of the postal code in the 
        /// shipment’s fromAddress when calculating rates and when determining if the shipment can be added to a manifest.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public RatesArrayFluent<T> InductionPostalCode(string s) 
        {
            _current.InductionPostalCode = s;
            return this;
        }
        /// <summary>
        /// Volumetric weight calculated based on weight and volume.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="w"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        public RatesArrayFluent<T> DimensionalWeight<S>(decimal w, UnitOfWeight u) where S: IParcelWeight, new()
        {
            _current.DimensionalWeight = new S(){Weight = w, UnitOfMeasurement = u};
            return this;
        }
        /// <summary>
        /// Time in transit for the shipment.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public RatesArrayFluent<T> DeliveryCommitment(IDeliveryCommitment c) 
        {
            _current.DeliveryCommitment = c;
             return this;
        }
        /// <summary>
        /// Type of currency referenced in the piece price. For example: USD, CAD, EUR
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public RatesArrayFluent<T> CurrencyCode(string s) 
        {
            _current.CurrencyCode = s;
            return this;
        }
        /// <summary>
        /// Convenience method to add insurance special service.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public RatesArrayFluent<T> Insurance(decimal amount)
        {
            return SpecialService<SpecialServices>(SpecialServiceCodes.Ins, 0M, new Parameter("INPUT_VALUE", amount.ToString()));
        }
    }
}