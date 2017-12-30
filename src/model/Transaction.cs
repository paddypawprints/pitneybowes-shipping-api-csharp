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

namespace PitneyBowes.Developer.ShippingApi.Model
{
    /// <summary>
    /// Transaction report line items
    /// </summary>
    public class Transaction : ITransaction
    {
        /// <summary>
        /// Unique identifier used while creating the shipment.
        ///
        /// Note: Prefix the transactionId with a % symbol.
        /// For example:%12343345
        /// </summary>
        virtual public string TransactionId { get; set; }
        /// <summary>
        /// Date and time of of the transaction.
        /// </summary>
        virtual public DateTimeOffset TransactionDateTime { get; set; }
        /// <summary>
        /// Type of transaction.
        /// </summary>
        virtual public TransactionType TransactionType { get; set; }
        /// <summary>
        /// Name of the developer account used to print the shipment label.
        /// </summary>
        virtual public string DeveloperName { get; set; }
        /// <summary>
        /// The developer ID used to print the shipment label.
        /// </summary>
        virtual public string DeveloperId { get; set; }
        /// <summary>
        /// The developer’s postage payment method. This is populated only for transactions that use the bulk postage account model.
        /// </summary>
        virtual public string DeveloperPostagePaymentMethod { get; set; }
        /// <summary>
        /// Rate plan of the developer.
        /// </summary>
        virtual public string DeveloperRatePlan { get; set; }
        /// <summary>
        /// Amount charged to the developer.
        /// </summary>
        virtual public Decimal? DeveloperRateAmount { get; set; }
        /// <summary>
        /// Postage balance in the developer’s postage account.
        /// </summary>
        virtual public Decimal? DeveloperPostagePaymentAccountBalance { get; set; }
        /// <summary>
        /// Name of the merchant.
        /// </summary>
        virtual public string MerchantName { get; set; }
        /// <summary>
        /// The value of the postalReportingNumber field in the merchant object. This value is also the merchant’s Shipper ID.
        /// </summary>
        virtual public string MerchantId { get; set; }
        /// <summary>
        /// The merchant’s postage payment method. This is populated only for transactions that use the individual postage account model.
        /// </summary>
        virtual public string MerchantPostageAccountPaymentMethod { get; set; }
        /// <summary>
        /// Rate plan of the merchant.
        /// </summary>
        virtual public string MerchantRatePlan { get; set; }
        /// <summary>
        /// Amount charged to the merchant.
        /// </summary>
        virtual public Decimal? MerchantRate { get; set; }
        /// <summary>
        /// Postage balance in the merchant’s postage account.
        /// </summary>
        virtual public Decimal? ShipperPostagePaymentAccountBalance { get; set; }
        /// <summary>
        /// Currently not used.
        /// </summary>
        virtual public Decimal? LabelFee { get; set; }
        /// <summary>
        /// Parcel tracking number of the shipment.
        /// </summary>
        virtual public string ParcelTrackingNumber { get; set; }
        /// <summary>
        /// Weight in ounces.
        /// </summary>
        virtual public Decimal? WeightInOunces { get; set; }
        /// <summary>
        /// Zone
        /// </summary>
        virtual public int? Zone { get; set; }
        /// <summary>
        /// Package length in inches.
        /// </summary>
        virtual public Decimal? PackageLengthInInches { get; set; }
        /// <summary>
        /// Package width in inches.
        /// </summary>
        virtual public Decimal? PackageWidthInInches { get; set; }
        /// <summary>
        /// Package height in inches.
        /// </summary>
        virtual public Decimal? PackageHeightInInches { get; set; }
        /// <summary>
        ///  Indicates whether cubic pricing was used.
        /// </summary>
        virtual public PackageTypeIndicator? PackageTypeIndicator { get; set; }
        /// <summary>
        /// Package type
        /// </summary>
        virtual public ParcelType? PackageType { get; set; }
        /// <summary>
        /// Mail class or service.
        /// </summary>
        virtual public string MailClass { get; set; }
        /// <summary>
        /// International country group code.
        /// </summary>
        virtual public string InternationalCountryPriceGroup { get; set; }
        /// <summary>
        /// Origination address.
        /// </summary>
        virtual public string OriginationAddress { get; set; }
        /// <summary>
        /// Origin zip code.
        /// </summary>
        virtual public string OriginZip { get; set; }
        /// <summary>
        /// Destination address.
        /// </summary>
        virtual public string DestinationAddress { get; set; }
        /// <summary>
        /// Destination zip code.
        /// </summary>
        virtual public string DestinationZip { get; set; }
        /// <summary>
        /// Destination country.
        /// </summary>
        virtual public string DestinationCountry { get; set; }
        /// <summary>
        /// Postage deposit amount.
        /// </summary>
        virtual public Decimal? PostageDepositAmount { get; set; }
        /// <summary>
        /// Credit card fee.
        /// </summary>
        virtual public Decimal? CreditCardFee { get; set; }
        /// <summary>
        /// Refund status.
        /// </summary>
        virtual public string RefundStatus { get; set; }
        /// <summary>
        /// Reason for refund denial.
        /// </summary>
        virtual public string RefundDenialReason { get; set; }
    }
}
