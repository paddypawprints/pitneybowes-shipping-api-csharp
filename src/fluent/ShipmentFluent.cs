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
using PitneyBowes.Developer.ShippingApi;

namespace PitneyBowes.Developer.ShippingApi.Fluent
{
    /// <summary>
    /// Object to create a shipment and purchases a shipment label. The API returns the label as either a Base64 string or a link to a PDF.
    /// </summary>
    /// <typeparam name="T">Underlying shipment class</typeparam>
    public class ShipmentFluent<T> where T : IShipment, new()
    {
        private T _shipment;
        /// <summary>
        /// Implicit cast into the underlying type (IShipment object)
        /// </summary>
        /// <param name="s"></param>
        public static implicit operator T(ShipmentFluent<T> s)
        {
            return s._shipment;
        }
        /// <summary>
        /// Factory method to create a ShipmentFluent. Use instead of new(). Use at the start of the fluent method chain.
        /// </summary>
        /// <returns></returns>
        public static ShipmentFluent<T> Create()
        {
            var a = new ShipmentFluent<T>()
            {
                _shipment = new T()
            };
            return a;
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public ShipmentFluent()
        {
            _shipment = new T();
        }
        /// <summary>
        /// SBR LABELS ONLY.
        ///
        /// If you are creating a scan-based return (SBR) label, set this to RETURN.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public ShipmentFluent<T> ShipmentType(ShipmentType t)
        {
            _shipment.ShipmentType = t;
            return this;
        }
        /// <summary>
        /// REQUIRED. A unique identifier for each transaction that cannot exceed 25 characters.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ShipmentFluent<T> TransactionId(string id)
        {
            _shipment.TransactionId = id;
            return this;
        }
        /// <summary>
        /// MinimalAddressValidation header option
        /// </summary>
        public ShipmentFluent<T> MinimalAddressValidation(string m)
        {
            _shipment.MinimalAddressValidation = m;
            return this;
        }
        /// <summary>
        /// Shipper rate plan, if available.
        /// Important: Do not include this header if creating a scan-based return (SBR) label.
        /// </summary>
        /// <param name="r">Rate plan</param>
        /// <returns></returns>
        public ShipmentFluent<T> ShipperRatePlan(string r)
        {
            _shipment.ShipperRatePlan = r;
            return this;
        }
        /// <summary>
        /// REQUIRED. Origin address. See Create a Shipment for considerations when specifying multiple address lines when using 
        /// MINIMAL_ADDRESS_VALIDATION.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ShipmentFluent<T> FromAddress( IAddress a)
        {
            _shipment.FromAddress = a;
            return this;
        }
        /// <summary>
        /// REQUIRED.Destination address.
        /// Note: You can specify multiple address lines in the shipment’s destination address.See address object for information on how 
        /// the API processes multiple address lines.
        /// </summary>
        /// <param name="a">Address</param>
        /// <returns></returns>
        public ShipmentFluent<T> ToAddress(IAddress a)
        {
            _shipment.ToAddress = a;
            return this;
        }
        /// <summary>
        /// INTERNATIONAL SHIPMENTS ONLY. Required if the return shipment is not going to the fromAddress but is instead to an alternate 
        /// return address.
        /// </summary>
        public ShipmentFluent<T> AltReturnAddress(IAddress a)
        {
            _shipment.AltReturnAddress = a;
            return this;
        }
        /// <summary>
        /// REQUIRED. Contains physical characteristics of the parcel.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public ShipmentFluent<T> Parcel(IParcel p)
        {
            _shipment.Parcel = p;
            return this;
        }
        /// <summary>
        /// REQUIRED. Information related to the shipment rates.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public ShipmentFluent<T> Rates(IEnumerable<IRates> r)
        {
            _shipment.Rates = r;
            return this;
        }
        /// <summary>
        /// Add Rates object to the Rates IEnumerable
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public ShipmentFluent<T> AddRates(IRates r)
        {
            _shipment.AddRates(r);
            return this;
        }
        /// <summary>
        /// A list of shipment documents pertaining to a shipment, including the label.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public ShipmentFluent<T> Documents(IEnumerable<IDocument> d)
        {
            _shipment.Documents = d;
            return this;
        }
        /// <summary>
        /// Add a document tot he documents ienumerable.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public ShipmentFluent<T> AddDocument(IDocument d)
        {
            _shipment.AddDocument(d);
            return this;
        }
        /// <summary>
        /// Each object in this array defines a shipment option. The available options depend on the carrier, origin country, and destination country.
        /// If you are creating a shipment, this array is required and must contain the SHIPPER_ID option.
        /// </summary>
        public ShipmentFluent<T> ShipmentOptions(IEnumerable<IShipmentOptions> o)
        {
            _shipment.ShipmentOptions = o;
            return this;
        }
        /// <summary>
        /// Add option to the ShipmentOptions IEnumerable
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public ShipmentFluent<T> AddShipmentOptions(IShipmentOptions o)
        {
            _shipment.AddShipmentOptions(o);
            return this;
        }
        /// <summary>
        /// ONLY FOR: international, APO/FPO/DPO, territories/possessions, and FAS shipments. Customs related information.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public ShipmentFluent<T> Customs(ICustoms c)
        {
            _shipment.Customs = c;
            return this;
        }

    }
}
