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
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Globalization;
using PitneyBowes.Developer.ShippingApi.Model;
using PitneyBowes.Developer.ShippingApi.Report;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Request object for Transaction Page service call
    /// </summary>
    public class ReportRequest : ShippingApiRequest, IReportRequest
    {
        /// <summary>
        /// If recorded, the file will be named by the developer id and the page number
        /// </summary>
        public override string RecordingSuffix => DeveloperId+"-Page"+Page;
        /// <summary>
        /// /Developer Id
        /// </summary>
        public string DeveloperId { get; set; }
        /// <summary>
        /// The beginning of the date range for transactions.
        /// </summary>
        [ShippingApiQuery("fromDate",Format = "{0:yyyy-MM-ddTHH:mm:ssZ}")]
        public DateTimeOffset FromDate { get; set; }
        /// <summary>
        /// The end of the date range for transactions.
        /// </summary>
        [ShippingApiQuery("toDate",Format = "{0:yyyy-MM-ddTHH:mm:ssZ}")]
        public DateTimeOffset ToDate { get; set; }
        /// <summary>
        /// Unique identifier used while creating the shipment.
        /// Note: Prefix the transactionId with a % symbol.
        /// For example: %12343345
        /// </summary>
        [ShippingApiQuery("transactionId", true)]
        public string TransactionId { get; set; }
        /// <summary>
        /// 	Parcel tracking number of the shipment.
        /// </summary>
        [ShippingApiQuery("parcelTrackingNumber", true)]
        public string ParcelTrackingNumber { get; set; }
        /// <summary>
        /// The value of the postalReportingNumber element in the merchant object. This value is also the merchant’s Shipper ID.
        /// </summary>
        [ShippingApiQuery("merchantId", true)]
        public string MerchantId { get; set; }
        /// <summary>
        /// Transaction Type.
        /// </summary>
        [ShippingApiQuery("transactionType", true)]
        public TransactionType? TransactionType { get; set; }
        /// Page size of the result set for the specified query filters.Specifically, the number of transactions to return per page in the result set.Default page size is 20.
        [ShippingApiQuery("size", true)]
        public int? PageSize { get; set; }
        /// <summary>
        /// The index number of the page to return. Page index numbering starts at 0. Specifying page=0 returns the first page of the result set.
        /// </summary>
        [ShippingApiQuery("page", true)]
        public int Page { get; set; }
        /// <summary>
        /// Define a property to sort on and a sort order.Use the following form: property name,sort direction
        ///     * Sort direction can be ascending (asc) or descending (desc).
        ///     * For example, the following indicates the result set should be sorted in descending order of the transactionId:
        ///         sort= transactionId, desc
        /// </summary>
        [ShippingApiQuery("sort", true)]
        public string Sort { get; set; }
        /// <summary>
        /// Authentication token
        /// </summary>
        [ShippingApiHeader("Bearer")]
        public override StringBuilder Authorization { get; set; }
        /// <summary>
        /// Http header accept-language
        /// </summary>
        [ShippingApiHeader("Accept-Language")]
        public string AcceptLanguage { get; set; }
        /// <summary>
        /// Http header content type - application/json
        /// </summary>
        public override string ContentType { get => "application/json";}
        /// <summary>
        /// Constructor - sets AcceptLanguage
        /// </summary>
        public ReportRequest()
        {
            AcceptLanguage = CultureInfo.CurrentCulture.Name;
        }
        /// <summary>
        /// Validate that the request is ok.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            return ToDate != null && FromDate != null;
        }
    }

    /// <summary>
    /// Response object for GET /v2/ledger/developers/{developerId}/transactions/reports
    /// </summary>
    public class TransactionPageResponse
    {
        /// <summary>
        /// Detailed information associated with the transaction.
        /// </summary>
        [JsonProperty("content")]
        public IEnumerable<Transaction> Content { get; set; }
        /// <summary>
        /// Total Number of pages in the result set.
        /// </summary>
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
        /// <summary>
        /// Total number of transactions in the result set for the specified query filters.
        /// </summary>
        [JsonProperty("totalElements")]
        public int TotalElements { get; set; }
        /// <summary>
        /// If true, this is the last page of the result set.
        /// </summary>
        [JsonProperty("last")]
        public bool LastPage { get; set; }
        /// <summary>
        /// Number of transactions per page in the result set. The default size is 20.
        /// </summary>
        [JsonProperty("size")]
        public int Size { get; set; }
        /// <summary>
        /// The index number of the page being returned. Page index numbering starts at 0. If number is set to 4, this is the 5th page of the result set.
        /// </summary>
        [JsonProperty("number")]
        public int PageIndex { get; set; }
        /// <summary>
        /// Sort order, if specified in the request.
        /// </summary>
        [JsonProperty("sort")]
        public IEnumerable<ITransactionSort> Sort { get; set; }
        /// <summary>
        /// Number of transactions in the current page.
        /// </summary>
        [JsonProperty("numberOfElements")]
        public int Count { get; set; }
        /// <summary>
        /// If true, this is the first page of the result set.
        /// </summary>
        [JsonProperty("first")]
        public bool FirstPage { get; set; }
    }

    /// <summary>
    /// Implementation for the transaction report API call. /shippingservices/v2/ledger/developers/{DeveloperId}/transactions
    /// </summary>
    /// <typeparam name="T">Concrete class to hold the report rows</typeparam>
    public class TransactionsReport<T> : ReportBase<T>
    {
        /// <summary>
        /// API developer id.
        /// </summary>
        public string DeveloperId { get; set; }
        private ISession _session;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="session">Optional session - if omitted or null this will look for the DefaultSession in the Globals object</param>
        /// <param name="maxPages">Maximum number of pages to return.</param>
        public TransactionsReport( string developerId, int maxPages = -1, ISession session = null) : base()
        {
            Provider = new TransactionsReportProvider(developerId, session, maxPages);
            DeveloperId = developerId;
            _session = session;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="expression"></param>
        /// <param name="developerId"></param>
        /// <param name="session">Optional session - if omitted or null this will look for the DefaultSession in the Globals object</param>
        public TransactionsReport( TransactionsReportProvider provider, Expression expression, string developerId, ISession session = null) : base(provider,expression)
        {
            DeveloperId = developerId;
            _session = session;
        }

        /// <summary>
        /// Call the shipping API ledger endpoint to retrieve a single report page
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="session">Optional session - if omitted or null this will look for the DefaultSession in the Globals object</param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<TransactionPageResponse>> TransactionPage(ReportRequest request, ISession session = null)
        {
            if (session == null) session = Globals.DefaultSession;
            request.Authorization = new StringBuilder(session.AuthToken.AccessToken);
            return await WebMethod.Get<TransactionPageResponse, ReportRequest>("/shippingservices/v2/ledger/developers/{DeveloperId}/transactions/reports", request, session);
        }

        /// <summary>
        /// Enumerator that returns rows on the current page and fetches the next page until the report is complete.
        /// </summary>
        /// <param name="request">Page request object</param>
        /// <param name="filter">Delegate to filter out report rows that are not of interest. Note the filtering occurs client side, after the page has been fetched, not server side.</param>
        /// <param name="maxPages">Maximum number of pages to fetch. (-1 means no limit)</param>
        /// <param name="session">Optional session - if omitted or null this will look for the DefaultSession in the Globals object</param>
        /// <returns></returns>
        public static IEnumerable<Transaction> Report(ReportRequest request, Func<Transaction, bool> filter = null, int maxPages = -1, ISession session = null)
        {
            if (session == null) session = Globals.DefaultSession;
            request.Page = 0;
            TransactionPageResponse page;
            do
            {
                var response = TransactionPage(request, session).GetAwaiter().GetResult();
                if (!response.Success) break;
                page = response.APIResponse;
                request.Page += 1;
                foreach (var t in page.Content)
                {
                    if ( filter == null || (filter != null && filter(t)) ) yield return t;
                }
            } while (!page.LastPage && ( maxPages == -1 || request.Page < maxPages) );
        }
    }

    /// <summary>
    /// Query provider for the shipping API Transactions report
    /// </summary>
    public class TransactionsReportProvider : ReportProviderBase, IQueryProvider
    {
        private string _developerId;
        internal ISession _session;
        private int _maxPages;

        /// <summary>
        /// Constructor for query provider for the shipping API transactions report
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="session"></param>
        /// <param name="maxPages">Maximum number of pages to fetch (-1 means no limit)</param>
        public TransactionsReportProvider( string developerId, ISession session, int maxPages = -1) :base()
        {
            _developerId = developerId;
            _session = session;
            _maxPages = maxPages;
        }

        /// <summary>
        /// Queryable's collection-returning standard query operators call this method.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns> 
        public override IQueryable<TResult> CreateQuery<TResult>(Expression expression) 
        {
            return new TransactionsReport<TResult>(this, expression, _developerId);
        }

        /// <summary>
        /// Execute the report, calling the shipping API
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <param name="IsEnumerable"></param>
        /// <returns></returns>
        public override object Execute<TResult>(Expression expression, bool IsEnumerable)
        {
            return Execute<TResult, Transaction, ReportRequest, ReportRequestFinder>(
                expression,
                IsEnumerable,
                x => TransactionsReport<Transaction>.Report( x, null, _maxPages, _session),
                x => x.DeveloperId = _developerId
            );
        }
    }

    internal class ReportRequestFinder : RequestFinderVisitor<ReportRequest, Transaction>
    {
        protected override Expression VisitBinary(BinaryExpression be)
        {
            Expression exp;
            if(( exp = AssignExpressionValue<string>(be, ExpressionType.Equal, "MerchantId", (x, y) => x.MerchantId = y))!=null) return exp;
            if ((exp = AssignExpressionValue<TransactionType>(be, ExpressionType.Equal, "TransactionType", (x, y) => x.TransactionType = y))!=null)return exp;
            if ((exp = AssignExpressionValue<DateTimeOffset>(be, ExpressionType.GreaterThanOrEqual, "TransactionDateTime", (x, y) => x.FromDate = y))!=null) return exp;
            if ((exp = AssignExpressionValue<DateTimeOffset>(be, ExpressionType.LessThanOrEqual, "TransactionDateTime", (x, y) => x.ToDate = y))!=null) return exp;
            return base.VisitBinary(be);
        }
    }
}
