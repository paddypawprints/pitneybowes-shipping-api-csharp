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

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Respresents merchants/shippers in the shipping API
    /// </summary>
    public interface IMerchant
    {
        /// <summary>
        /// The merchant’s full name.
        /// </summary>
        string FullName { get; set; }
        /// <summary>
        /// The merchant’s email address.
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// The date that the merchant’s PB Postage Account was registered
        /// </summary>
        DateTimeOffset RegisteredDate { get; set; }
        /// <summary>
        /// The Pitney Bowes customer account number assigned to the merchant.
        /// </summary>
        string PaymentAccountNumber { get; set; }
        /// <summary>
        /// An enterprise account number that is associated with the merchant.
        /// </summary>
        string EnterpriseAccount { get; set; }
        /// <summary>
        /// Any subscription account that the merchant might have.
        /// </summary>
        string SubscriptionAccount { get; set; }
        /// <summary>
        /// The unique ID used to identify the merchant.  Note: This value is also the merchant’s Shipper ID.You must specify 
        /// Shipper ID when creating a shipment.
        /// </summary>
        string PostalReportingNumber { get; set; }
        /// <summary>
        /// The merchant’s status.
        /// </summary>
        string MerchantStatus { get; set; }
        /// <summary>
        /// If you change a merchant’s status from ACTIVE to INACTIVE, you must give a reason for the change. The reason is recorded here. 
        /// For an active merchant, the field is set to null.
        /// </summary>
        string MerchantStatusReason { get; set; }
        /// <summary>
        /// The date the merchant’s PB Postage Account was deactivated, if applicable. For an active merchant, the field is set to null.
        /// A deactivated merchant can no longer print labels.
        /// </summary>
        DateTimeOffset DeactivatedDate { get; set; }
    }
}
