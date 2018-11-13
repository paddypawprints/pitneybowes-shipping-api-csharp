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
using System.Text;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Transaction report line items
    /// </summary>
    [CodeGeneration( GenerateJsonWrapper = true, GenerateModel = false)]
    public interface ITransaction
    {
        /// <summary>
        /// Unique identifier used while creating the shipment.
        ///
        /// Note: Prefix the transactionId with a % symbol.
        /// For example:%12343345
        /// </summary>
        string TransactionId {get;set;}
        /// <summary>
        /// Date and time of of the transaction.
        /// </summary>
        DateTimeOffset TransactionDateTime { get; set; }
        /// <summary>
        /// Type of transaction.
        /// </summary>
        TransactionType TransactionType { get; set; }
        /// <summary>
        /// Name of the developer account used to print the shipment label.
        /// </summary>
        string DeveloperName { get; set; }
        /// <summary>
        /// The developer ID used to print the shipment label.
        /// </summary>
        string DeveloperId { get; set; }
        /// <summary>
        /// The developer’s postage payment method. This is populated only for transactions that use the bulk postage account model.
        /// </summary>
        string DeveloperPostagePaymentMethod { get; set; }
        /// <summary>
        /// Rate plan of the developer.
        /// </summary>
        string DeveloperRatePlan { get; set; }
        /// <summary>
        /// Amount charged to the developer.
        /// </summary>
        decimal? DeveloperRateAmount { get; set; }
        /// <summary>
        /// Postage balance in the developer’s postage account.
        /// </summary>
        decimal? DeveloperPostagePaymentAccountBalance { get; set; }
        /// <summary>
        /// Name of the merchant.
        /// </summary>
        string MerchantName { get; set; }
        /// <summary>
        /// The value of the postalReportingNumber field in the merchant object. This value is also the merchant’s Shipper ID.
        /// </summary>
        string MerchantId { get; set; }
        /// <summary>
        /// The merchant’s postage payment method. This is populated only for transactions that use the individual postage account model.
        /// </summary>
        string MerchantPostageAccountPaymentMethod { get; set; }
        /// <summary>
        /// Rate plan of the merchant.
        /// </summary>
        string MerchantRatePlan { get; set; }
        /// <summary>
        /// Amount charged to the merchant.
        /// </summary>
        decimal? MerchantRate { get; set; }
        /// <summary>
        /// Postage balance in the merchant’s postage account.
        /// </summary>
        decimal? ShipperPostagePaymentAccountBalance { get; set; }
        /// <summary>
        /// Currently not used.
        /// </summary>
        decimal? LabelFee { get; set; }
        /// <summary>
        /// Parcel tracking number of the shipment.
        /// </summary>
        string ParcelTrackingNumber { get; set; }
        /// <summary>
        /// Weight in ounces.
        /// </summary>
        decimal? WeightInOunces { get; set; }
        /// <summary>
        /// Zone
        /// </summary>
        int? Zone { get; set; }
        /// <summary>
        /// Package length in inches.
        /// </summary>
        decimal? PackageLengthInInches { get; set; }
        /// <summary>
        /// Package width in inches.
        /// </summary>
        decimal? PackageWidthInInches { get; set; }
        /// <summary>
        /// Package height in inches.
        /// </summary>
        decimal? PackageHeightInInches { get; set; }
        /// <summary>
        ///  Indicates whether cubic pricing was used.
        /// </summary>
        PackageTypeIndicator? PackageTypeIndicator { get; set; }
        /// <summary>
        /// Package type
        /// </summary>
        ParcelType? PackageType { get; set; }
        /// <summary>
        /// Mail class or service.
        /// </summary>
        string MailClass { get; set; }
        /// <summary>
        /// International country group code.
        /// </summary>
        string InternationalCountryPriceGroup { get; set; }
        /// <summary>
        /// Origination address.
        /// </summary>
        string OriginationAddress { get; set; }
        /// <summary>
        /// Origin zip code.
        /// </summary>
        string OriginZip { get; set; }
        /// <summary>
        /// Destination address.
        /// </summary>
        string DestinationAddress { get; set; }
        /// <summary>
        /// Destination zip code.
        /// </summary>
        string DestinationZip { get; set; }
        /// <summary>
        /// Destination country.
        /// </summary>
        string DestinationCountry { get; set; }
        /// <summary>
        /// Postage deposit amount.
        /// </summary>
        decimal? PostageDepositAmount { get; set; }
        /// <summary>
        /// Credit card fee.
        /// </summary>
        decimal? CreditCardFee { get; set; }
        /// <summary>
        /// Gets or sets the print status.
        /// </summary>
        /// <value>The print status.</value>
        SBRPrintStatus? PrintStatus { get; set; }
        /// <summary>
        /// Refund status.
        /// </summary>
        string RefundStatus { get; set; }
        /// <summary>
        /// Reason for refund denial.
        /// </summary>
        string RefundDenialReason { get; set; }
        /// <summary>
        /// Indicates who requested the refund.
        /// </summary>
        /// <value>The refund requestor.</value>
        string RefundRequestor { get; set; }
        /// <summary>
        /// APV Adjustments Only. The reason for an APV adjustment based on the new information received from USPS. 
        /// </summary>
        /// <value>The adjustment reason.</value>
        AdjustmentReason? AdjustmentReason { get; set; }
        /// <summary>
        /// Applies only to the following:
        /// APV Adjustments: The unique identifier that USPS assigned to the APV adjustment.If you want to appeal 
        /// the adjustment, you must send this identifier to USPS.
        /// Holiday Guarantee Refunds: The claim number assigned by the the Holiday Guarantee program.
        /// </summary>
        /// <value>The external identifier.</value>
        string ExternalId { get; set; }
    }
}
