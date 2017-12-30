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
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Request object for RenderIFrame call
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class RenderRequest : ShippingApiRequest
    {
        /// <summary>
        /// Http content type
        /// </summary>
        public override string ContentType { get => "application/json"; }
        /// <summary>
        /// Accept-Language http header
        /// </summary>
        [ShippingApiHeaderAttribute("Accept-Language")]
        public string AcceptLanguage { get => "en-US"; }
        /// <summary>
        /// Authentication token
        /// </summary>
        [ShippingApiHeaderAttribute("Bearer")]
        public override StringBuilder Authorization { get; set; }
        /// <summary>
        /// REQUIRED. The URL of the web page where the iframe is rendered. This value is required to identify the host when Pitney Bowes 
        /// returns the tokenized payment information in a window post message. The iframe works correctly only if this URL is accurate. 
        /// For example: dev08.net, or localhost:4200, or https://127.0.0.1/web/.
        /// </summary>
        [JsonProperty("hostUri")]
        public string HostUri { get; set; }
        /// <summary>
        /// REQUIRED. The payment service. Unless instructed otherwise, set this to purchase_power. This provides an iframe that lets a merchant 
        /// choose either Purchase Power or a credit card as the payment method. The iframe guides the merchant through setting up the payment 
        /// method.
        /// </summary>
        [JsonProperty("renderType")]
        public string RenderType { get; set; }

        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }
        /// <summary>
        /// The HTTPS URL of the stylesheet to use when rendering the iframe. This is optional. If this element is omitted, a default style is used.
        /// </summary>
        [JsonProperty("styleSheet")]
        public string StyleSheet { get; set; }
        /// <summary>
        /// An object containing the merchant’s name, address, company, phone, and email.
        /// </summary>
        [JsonProperty("userInfo")]
        public IUserInfo UserInfo { get; set; }
    }
    /// <summary>
    /// Class to hold frame return type from the render call. Too specific to be generally useful so it doesnt get an interface.
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// Indicates that the URL is for an iframe.
        /// </summary>
        public string renderType;
        /// <summary>
        /// The URL for including the iframe in your application.
        /// </summary>
        public string render;
    }
    /// <summary>
    /// Class to hold frame return type from the render call. Too specific to be generally useful so it doesnt get an interface.
    /// </summary>
    public class RenderResponse
    {
        /// <summary>
        /// Iframe details for purchase power
        /// </summary>
        public Frame purchasePower;
        /// <summary>
        /// The token. 
        /// </summary>
        public string accessToken;
    }

    /// <summary>
    /// Class for merchant signup web method. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MerchantSignupRequest : ShippingApiRequest
    {
        /// <summary>
        /// Http content type
        /// </summary>
        public override string ContentType => "application/json";
        /// <summary>
        /// Authentication token
        /// </summary>
        [ShippingApiHeader("Bearer")]
        public override StringBuilder Authorization { get; set; }
        /// <summary>
        /// REQUIRED. Your Pitney Bowes developer ID.
        /// </summary>
        public string DeveloperId { get; set; }
        /// <summary>
        /// REQUIRED. The new merchant’s contact address. This can be different from the merchant’s billing address.
        /// </summary>
        [JsonProperty("contact")]
        public IAddress Contact { get; set; }
        /// <summary>
        /// REQUIRED. The merchant’s payment information.
        /// </summary>
        [JsonProperty("paymentInfo")]
        public IEnumerable<IPaymentInfo> PaymentInfo { get; set; }
        /// <summary>
        /// The initial balance transferred to the merchant’s PB Postage Account. Ensure the balance can cover the 
        /// cost of the merchant’s first postage label.
        /// </summary>
        [JsonProperty("initialPostageBalance")]
        public decimal IntialPostageValue { get; set; }
        /// <summary>
        /// The amount to add to the merchant’s PB Postage Account when the balance falls below the thresholdAmount value. The default value 
        /// for this field is 400.
        /// </summary>
        [JsonProperty("refillAmount")]
        public decimal RefillAmount { get; set; }
        /// <summary>
        /// The PB Postage Account is refilled when the account balance falls below this value. The default value for this field is 100.
        /// </summary>
        [JsonProperty("thresholdAmount")]
        public decimal ThresholdAmount { get; set; }
    }

    /// <summary>
    /// Request object for the MerchantAuthorization service call
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MerchantCredentialsRequest : ShippingApiRequest
    {
        /// <summary>
        /// Http content type
        /// </summary>
        public override string ContentType => "application/json";
        /// <summary>
        /// Authentication token
        /// </summary>
        [ShippingApiHeader("Bearer")]
        public override StringBuilder Authorization { get; set; }
        /// <summary>
        /// REQUIRED. The identifier for the developer
        /// </summary>
        public string DeveloperId { get; set; }
        /// <summary>
        /// REQUIRED. The username for the merchant’s PB Postage Account.
        /// </summary>
        [JsonProperty("username")]
        public string UserName { get; set; }
        /// <summary>
        /// REQUIRED. The username for the merchant’s PB Postage Account.
        /// </summary>
        [JsonProperty("password")]
        public StringBuilder Password { get; set; }
    }

    /// <summary>
    /// Request object for the MerchantRegister web service call
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MerchantRegisterRequest : ShippingApiRequest
    {
        /// <summary>
        /// Http content type
        /// </summary>
        public override string ContentType => "application/json";
        /// <summary>
        /// Authentication token
        /// </summary>
        [ShippingApiHeader("Bearer")]
        public override StringBuilder Authorization { get; set; }
        /// <summary>
        /// REQUIRED. The identifier for the developer
        /// </summary>
        public string DeveloperId { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
        /// <summary>
        /// Street address and/or apartment and/or P.O.Box.You can specify up to three address lines. For USPS domestic destinations, 
        /// ensure that the street address is specified as the last of the 3 address lines. This way, the street address is printed 
        /// right above the city, state, postal zip code, per USPS label guidelines.
        /// </summary>
        [JsonProperty("addressLines", Order = 5)]
        public IEnumerable<string> AddressLines { get;set;}
        /// <summary>
        /// The city or town name.
        /// </summary>
        [JsonProperty("cityTown", Order = 6)]
        public string CityTown {get;set;}
        /// <summary>
        /// State or province name. For US address, use the 2-letter state code.
        /// </summary>
        [JsonProperty("stateProvince", Order = 7)]
        public string StateProvince {get; set;}
        /// <summary>
        /// Postal/Zip code. For US addresses, either the 5-digit or 9-digit zip code.
        /// </summary>
        [JsonProperty("postalCode", Order = 8)]
        public string PostalCode {get;set;}
        /// <summary>
        /// Two-character country code from the ISO country list.
        /// </summary>
        [JsonProperty("countryCode", Order = 9)]
        public string CountryCode{ get;set;}
        /// <summary>
        /// Name of the company
        /// </summary>
        [JsonProperty("company", Order = 0)]
        public string Company { get;set; }
        /// <summary>
        /// First and last name
        /// </summary>
        [JsonProperty("name", Order = 1)]
        public string Name{ get;set; }
        /// <summary>
        /// Phone number
        /// </summary>
        [JsonProperty("phone", Order = 2)]
        public string Phone{get;set; }
        /// <summary>
        /// 	Email Address
        /// </summary>
        [JsonProperty("email", Order = 3)]
        public string Email{ get;set; }
        /// <summary>
        /// Indicates whether this is a residential address. It is recommended that this parameter be passed in as the address 
        /// verification process is more accurate with it.
        /// </summary>
        [JsonProperty("residential", Order = 4)]
        public bool Residential { get;set;}
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class MerchantAutoRefillRuleRequest : ShippingApiRequest
    {
        /// <summary>
        /// Http content type
        /// </summary>
        public override string ContentType => "application/json";
        /// <summary>
        /// Authentication token
        /// </summary>
        [ShippingApiHeader("Bearer")]
        public override StringBuilder Authorization { get; set; }
        public string DeveloperId { get; set; }
        public string ShipperId { get; set; }
        [JsonProperty("merchantID")]
        public string MerchantID { get; set; }
        [JsonProperty("threshold")]
        public decimal Threshold { get; set; }
        [JsonProperty("addAmount")]
        public decimal AddAmount { get; set; }
        [JsonProperty("enabled")]
        public Boolean Enabled { get; set; }

    }
    /// <summary>
    /// Request class for merchant account balance service call. GET /v1/ledger/accounts/{accountNumber}/balance
    /// </summary>
    public class AccountBalanceRequest : ShippingApiRequest
    {
        /// <summary>
        /// Http content type
        /// </summary>
        public override string ContentType => "application/json";
        /// <summary>
        /// Authentication token.
        /// </summary>
        [ShippingApiHeader("Bearer")]
        public override StringBuilder Authorization { get; set; }
        /// <summary>
        /// The paymentAccountNumber found in the merchant object.
        /// </summary>
        public string AccountNumber { get; set; }
    }

    /// <summary>
    /// Response class for merchant account balance service call. GET /v1/ledger/accounts/{accountNumber}/balance
    /// </summary>
    public class AccountBalanceResponse
    {
        /// <summary>
        /// The paymentAccountNumber found in the merchant object.
        /// </summary>
        [JsonProperty("accountNumber")]
        string AccountNumber { get; set; }
        /// <summary>
        /// The balance
        /// </summary>
        [JsonProperty("balance")]
        decimal Balance { get; set; }
        /// <summary>
        /// Balance currency
        /// </summary>
        [JsonProperty("currencyCode")]
        string CurrencyCode { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class MerchantDeactivateRequest : ShippingApiRequest
    {
        /// <summary>
        /// Http content type
        /// </summary>
        public override string ContentType => "application/json";
        /// <summary>
        /// Authentication token
        /// </summary>
        [ShippingApiHeader("Bearer")]
        public override StringBuilder Authorization { get; set; }

        public string DeveloperId { get; set; }

        public string AccountId { get; set; }

        [JsonProperty("reason")]
        string Reason { get; set; }

    }

    public static partial class Api
    {
        /// <summary>
        /// This POST operation returns the URL for an iframe that handles merchant payment information via a third-party payment service.
        /// The operation is part of the Merchant Signup API, which allows merchants to sign up for their own PB Postage Accounts from within 
        /// your application.
        ///
        /// Things to Consider:
        ///    * You must retrieve the merchant’s name, address, company, phone and email before making this API call.
        ///    * The request’s renderType element determines which payment service to use.Unless told otherwise, set this to purchase_power.
        ///    * The returned access token is for one-time use only.
        ///    * If desired, you can apply specific styles to the iframe by including the HTTPS URL of a style sheet in the POST request.The 
        ///    style sheet must be hosted over HTTPS or the iframe will not be able to use it and the UI will not render correctly.
        /// </summary>
        /// <typeparam name="RenderResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<RenderResponse>> RenderIFrame<RenderResponse>(RenderRequest request, ISession session = null) 
        {
            return await WebMethod.Post<RenderResponse, RenderRequest>("/shippingservices/v1/payment/render", request, session);
        }
        /// <summary>
        /// If you use the Merchant Signup model, use this POST operation to create the merchant’s PB Postage Account. The operation uses 
        /// tokenized information collected from a payment iframe that was included in your application through the Payment Iframe API.
        /// </summary>
        /// <typeparam name="IMerchant"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<IMerchant>> MerchantSignup<IMerchant>(MerchantSignupRequest request, ISession session = null)
        {
            return await WebMethod.Post<IMerchant, MerchantSignupRequest>("/shippingservices/v1/developers/{DeveloperId}/merchants/signup", request, session);
        }
        /// <summary>
        /// This operation retrieves the Shipper ID for a merchant who has signed up for an individual postage account using your personalized 
        /// Pitney Bowes registration link. 
        /// Once you retrieve the Shipper ID, you can request transactions on the merchant’s behalf.
        ///
        /// Important: This API call sends the username and password of the merchant’s PB Postage Account.It is highly recommended that you use 
        /// TLS encryption when passing the merchant credentials using this POST operation.
        /// </summary>
        /// <typeparam name="IMerchant"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<IMerchant>> MerchantAuthorization<IMerchant>(MerchantCredentialsRequest request, ISession session = null)
        {
            return await WebMethod.Post<IMerchant, MerchantCredentialsRequest>("/shippingservices/v1/developers/{DeveloperId}/merchants/credentials", request, session);
        }
        /// <summary>
        /// Use this API call if you use a Pitney Bowes bulk postage account to manage funds on behalf of your merchants. When a merchant signs up 
        /// for your service, use this API call to register the merchant with Pitney Bowes and to retrieve the merchant’s unique Shipper ID. You 
        /// must reference the Shipper ID when printing shipping labels on the merchant’s behalf.
        ///
        /// The API call returns the merchant’s Shipper ID in the postalReportingNumber field in the response object.
        ///
        /// Note: For information on PB bulk postage accounts, please contact the Pitney Bowes support team at ShippingAPISupport @pb.com.
        /// </summary>
        /// <typeparam name="IMerchant"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<IMerchant>> MerchantRegistration<IMerchant>(MerchantRegisterRequest request, ISession session = null)
        {
            return await WebMethod.Post<IMerchant, MerchantRegisterRequest>("/shippingservices/v1//developers/{DeveloperId}/merchants/registration" , request, session);
        }
        /// <summary>
        /// This operation retrieves the auto refill settings for a merchant’s PB Postage Account. If auto refill is enabled (which is the default), 
        /// the postage account automatically replenishes by a pre-determined amount when the balance falls below the pre-determined refill threshold.
        /// This API operation retrieves the refill settings; to change the settings, see Update Refill Settings.
        /// 
        /// When does auto refill trigger?
        ///     * The auto refill process enables a postage account to be replenished automatically based on pre-determined refill and threshold 
        ///     settings.The auto refill rule is triggered when a postage label is requested and the postage account’s balance falls below the 
        ///     refill threshold.The system prints the label while refilling the account.If a label is requested but the account’s balance is not 
        ///     enough to cover the label’s postage, the system returns an error and indicates a refill is in progress.
        ///     * Pitney Bowes recommends that you set the refill amount to exceed the refill threshold.The Create Merchant API uses default 
        ///     settings of 400 for the refill amount and 100 for the refill threshold.
        ///     * For new accounts, ensure the account has sufficient funds to cover the cost of the first label.
        ///     * You can retrieve and update refill settings through the Merchants API.
        ///     * Note: If an account does not have auto refill enabled, you must periodically check the account balance to ensure it has 
        ///     sufficient funds for creating shipping labels.When funds are low, you must manually refill the account.
        /// </summary>
        /// <typeparam name="IAutoRefill"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<IAutoRefill>> MerchantAutoRefillSettings<IAutoRefill>(MerchantAutoRefillRuleRequest request, ISession session = null)
        {
            return await WebMethod.Get<IAutoRefill, MerchantAutoRefillRuleRequest>("/shippingservices/v1/developers/{DeveloperId}/merchants/{AccountId}/autorefillrule", request, session);
        }
        /// <summary>
        /// This operation updates the auto refill settings on a merchant’s PB Postage Account. To retrieve the current settings, see Get Refill Settings. 
        /// </summary>
        /// <typeparam name="IAutoRefill"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<IAutoRefill>> MerchantUpdateAutoRefill<IAutoRefill>(MerchantAutoRefillRuleRequest request, ISession session = null)
        {
            return await WebMethod.Post<IAutoRefill, MerchantAutoRefillRuleRequest>("/shippingservices/v1/developers/{DeveloperId}/merchants/{ShipperId}/autorefillrule", request, session);
        }
        /// <summary>
        /// This GET operation retrieves a merchant’s account balance.
        /// </summary>
        /// <typeparam name="AccountBalanceResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<AccountBalanceResponse>> MerchantAccountBalance<AccountBalanceResponse>(AccountBalanceRequest request, ISession session = null)
        {
            return await WebMethod.Get<AccountBalanceResponse, AccountBalanceRequest>("/shippingservices/v1/ledger/accounts/{AccountId}/balance ", request, session);
        }
        /// <summary>
        /// This POST operation disables an active merchant account and starts the process of refunding the merchant’s postage balance. Once deactivated, the account can no longer print labels. Refunds are processed within 30 days. After 30 days the account’s transaction history is deleted. Eventually Pitney Bowes deletes the account.
        ///
        /// To reactivate an account after deactivating it, you must contact PB Support at ShippingAPISupport @pb.com.
        ///
        /// Things to Consider:
        ///     * You must issue this call using the same developer ID that was used to register the merchant account.
        ///     * The merchant’s account must be in the ACTIVE state, as defined by the merchantStatus field in the merchant object.
        ///     * The merchant is uniquely identified by the postalReportingNumber field.Do not use a different field.
        /// </summary>
        /// <typeparam name="IMerchant"></typeparam>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
    public async static Task<ShippingApiResponse<IMerchant>> MerchantDeactivateAccount<IMerchant>(MerchantDeactivateRequest request, ISession session = null)
        {
            return await WebMethod.Get<IMerchant, MerchantDeactivateRequest>("/shippingservices/v2/developers/{DeveloperId}/accounts/{ShipperId}/deactivate ", request, session);
        }
    }
}
