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
using System.Collections.Generic;

namespace PitneyBowes.Developer.ShippingApi.Model
{
    /// <summary>
    /// Tracking status of a shipment.
    /// </summary>
    public class TrackingStatus : ITrackingStatus
    {
        /// <summary>
        /// The number of packages tracked by this number.
        /// </summary>
        virtual public string PackageCount{get; set;}
        /// <summary>
        /// REQUIRED. Valid Value(s): USPS
        /// </summary>
        virtual public string Carrier{get; set;}
        /// <summary>
        /// REQUIRED. The tracking number for the shipment.
        /// </summary>
        virtual public string TrackingNumber{get; set;}
        /// <summary>
        /// Reference Number for the shipment.
        /// </summary>
        virtual public string ReferenceNumber{get; set;}
        /// <summary>
        /// Most recent Package Status.
        /// </summary>
        virtual public TrackingStatusCode Status{get; set;}
        /// <summary>
        /// Date indicating when the tracking status was posted
        /// </summary>
        virtual public DateTimeOffset UpdatedDateTime{get; set;}
        /// <summary>
        /// Date indicating when the package was shipped.
        /// </summary>
        virtual public DateTimeOffset ShipDateTime{get; set;}
        /// <summary>
        /// Date indicating when the package will be delivered.
        /// </summary>
        virtual public DateTimeOffset EstimatedDeliveryDateTime{get; set;}
        /// <summary>
        /// Date indicating when the package was delivered.
        /// </summary>
        virtual public DateTimeOffset DeliveryDateTime{get; set;}
        /// <summary>
        /// Delivery location.
        /// </summary>
        virtual public string DeliveryLocation{get; set;}
        /// <summary>
        /// Description of where the package was delivered.
        /// </summary>
        virtual public string DeliveryLocationDescription{get; set;}
        /// <summary>
        /// Name of the person who signed for the package.
        /// </summary>
        virtual public string SignedBy{get; set;}
        /// <summary>
        /// Unit of measure for the package抯 weight.
        /// </summary>
        virtual public Decimal Weight{get; set;}
        /// <summary>
        /// Unit of measure for the package抯 weight.
        /// </summary>
        virtual public UnitOfWeight? WeightOUM{get; set;}
        /// <summary>
        /// If the package was not delivered the first time, this field will indicate the date in YYYY-MM-DD format that the package was re-attempted to be delivered.
        /// </summary>
        virtual public string ReattemptDate{get; set;}
        /// <summary>
        /// f the package was not delivered the first time, this field will indicate the time in HH:MM:SS format that the package was re-attempted to be delivered.
        /// </summary>
        virtual public DateTime ReattemptTime{get; set;}
        /// <summary>
        /// The destination address.
        /// </summary>
        virtual public IAddress DestinationAddress{get; set;}
        /// <summary>
        /// The senders address
        /// </summary>
        virtual public IAddress SenderAddress{get; set;}
        /// <summary>
        /// Scan information from the barcode on the shipment label.
        /// </summary>
        virtual public IEnumerable<ITrackingEvent> ScanDetailsList{get; set;}
    }
}
