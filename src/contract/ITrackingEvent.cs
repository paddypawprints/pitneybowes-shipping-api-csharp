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

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Scan information from the barcode on the shipment label.
    /// </summary>
    public interface ITrackingEvent
    {
        /// <summary>
        /// Event date received from the Carrier.
        /// </summary>
        DateTimeOffset EventDateTime { get; set; }
        /// <summary>
        /// Event location - city
        /// </summary>
        string EventCity { get; set; }
        /// <summary>
        /// Event location - State or Province
        /// </summary>
        string EventState { get; set; }
        /// <summary>
        /// Event location - postal code
        /// </summary>
        string PostalCode { get; set; }
        /// <summary>
        /// Event location - country
        /// </summary>
        string Country { get; set; }
        /// <summary>
        /// Scan event Type - carrier specific
        /// </summary>
        string ScanType { get; set; }
        /// <summary>
        /// Scan event description
        /// </summary>
        string ScanDescription { get; set; }
        /// <summary>
        /// Package status
        /// </summary>
        string PackageStatus { get; set; }
    }
}
