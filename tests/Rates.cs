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

using System;
using PitneyBowes.Developer.ShippingApi;
using PitneyBowes.Developer.ShippingApi.Model;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace tests
{
    public class RatesTest : TestSession
    {
        public RatesTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            InitializeFramework();
        }

        [Fact]
        public void BasicRate()
        {
            List<string> fromAddressLine = new List<string>() {
                "8445 Graves Ave #25"
            };

            List<string> toAddressLine = new List<string>() {
                "10553 KERRIGAN Ct"
            };

            Address fromAddress = new Address
            {
                AddressLines = fromAddressLine,
                Email = "djfblsjvsvs@gmail.com",
                CityTown = "Santee",
                Name = "X X",
                CountryCode = "US",
                PostalCode = "92071",
                StateProvince = "CA",
            };

            Address toAddress = new Address
            {
                AddressLines = toAddressLine,
                Email = "xyz@gmail.com",
                CityTown = "Santee",
                Name = "X",
                CountryCode = "US",
                PostalCode = "92071",
                StateProvince = "CA",
            };


            ParcelDimension parcelDimension = new ParcelDimension
            {
                Height = 1,
                Length = 1,
                Width = 1,
                UnitOfMeasurement = UnitOfDimension.IN
            };

            ParcelWeight parcelWeight = new ParcelWeight
            {
                UnitOfMeasurement = UnitOfWeight.OZ,
                Weight = 1
            };

            Parcel parcel = new Parcel
            {
                CurrencyCode = "US",
                Dimension = parcelDimension,
                Weight = parcelWeight
            };

            Rates rates = new Rates
            {
                Carrier = Carrier.USPS,
                ParcelType = ParcelType.LGENV
            };

            Shipment shipment = new Shipment
            {
                FromAddress = fromAddress,
                ToAddress = toAddress,
                Parcel = parcel,
                Rates = new List<Rates> {
                            rates
                    }
            };

            try
            {
                // Get rates
                ShippingApiResponse shippingApiResponse = Api.Rates(shipment).Result;
            }
            catch (Exception e)
            {

            }
        }
    }
}
