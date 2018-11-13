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
    /// An address. If part of a response, this object also specifies address validation status, unless minimum validation is enabled.
    /// <a href="https://shipping.pitneybowes.com/reference/resource-objects.html#object-address"/>
    /// </summary>

    public interface IAddress
    {
        /// <summary>
        /// Street address and/or apartment and/or P.O. Box. You can specify up to
        /// three address lines.
        /// For USPS domestic destinations, ensure that the street address is
        /// specified as the last of the 3 address lines.This way, the street
        /// address is printed right above the city, state, postal zip code, per
        /// USPS label guidelines.
        /// 
        /// See <a href="https://shipping.pitneybowes.com/api/post-shipments.html"/> for considerations when specifying
        /// multiple lines in a shipment's ``fromAddress`` when
        /// ``MINIMAL_ADDRESS_VALIDATION`` is enabled.
        /// </summary>
        /// <value>The address lines.</value>
        IEnumerable<string> AddressLines { get; set; }
        /// <summary>
        /// Method to add an address line without having to know the implementation of the IEnumerable AddressLines
        /// </summary>
        /// <param name="s"></param>
        void AddAddressLine( string s);
        /// <summary>
        /// Gets or sets the city town.
        /// </summary>
        /// <value>The city town.</value>
        string CityTown { get; set; }
        /// <summary>
        /// Gets or sets the state province. For US address, use the 2-letter state code.
        /// </summary>
        /// <value>The state province.</value>
        string StateProvince { get; set; }
        /// <summary>
        /// Gets or sets the postal code. Two-character country code from the ISO country list.
        /// </summary>
        /// <value>The postal code.</value>
        string PostalCode { get; set; }
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>The country code.</value>
        string CountryCode { get; set; }
        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        string Company { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        string Phone { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        string Email { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:PitneyBowes.Developer.ShippingApi.IAddress"/> is residential. Indicates whether this is a residential address. It is recommended that
        /// this parameter be passed in as the address verification process is more accurate with it.
        /// </summary>
        /// <value><c>true</c> if residential; otherwise, <c>false</c>.</value>
        bool Residential { get; set; }
        /// <summary>
        /// The 2-digit delivery point, when available.
        /// </summary>
        /// <value>The delivery point.</value>
        string DeliveryPoint { get; set; }
        /// <summary>
        /// The last four characters of the USPS carrier route code. The carrier route is the area served by a 
        /// particular USPS mail carrier. The full carrier route code is a nine-character string comprising the 
        /// five-digit postal code appended by these four characters.
        /// </summary>
        /// <value>The carrier route.</value>
        string CarrierRoute { get; set; }
        /// <summary>
        /// Pickup Request Only. Tax identification number. This is optional for pickup requests.
        /// </summary>
        /// <value>The tax identifier.</value>
        string TaxId { get; set; }
        /// <summary>
        /// The response returns this field only if
        /// ``MINIMAL_ADDRESS_VALIDATION`` is **NOT** enabled
        /// This indicates whether any action has been performed on the address during cleansing.
        /// </summary>
        /// <value>The status.</value>
        AddressStatus Status { get; set; }
    }

    public static partial class InterfaceValidators
    {
        /// <summary>
        /// If false, the object underlying the interface is not a valid address. If true, the object may or may not be valid.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool IsValidDeliveryAddress(this IAddress a)
        {
            bool empty = true;
            foreach(var l in a.AddressLines)
            {
                empty = empty && (l == null || l.Equals(""));
            }
            return empty;

        }
    }

}