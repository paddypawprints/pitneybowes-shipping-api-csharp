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
    /// Request object for the merchants report API call
    /// </summary>
    public class MerchantsReportRequest : ShippingApiRequest, IReportRequest
    {
        /// <summary>
        /// REQUIRED. The identifier for the developer.
        /// </summary>
        public string DeveloperId { get; set; }
        /// <summary>
        /// The page size of the result set. Specifically, the number of merchants to return per page in the result set. The default 
        /// page size is 20 merchants.
        /// </summary>
        [ShippingApiQuery("size", true)]
        public int? PageSize { get; set; }
        /// <summary>
        /// The index number of the page to return. Page index numbering starts at 0. Specify page=0 to return the 1st page; specify page=1 to return the 2nd page, and so on.
        /// In the response object, the API displays the index number in the number field.
        /// </summary>
        [ShippingApiQuery("page", true)]
        public int Page { get; set; }
        /// <summary>
        /// Define a property to sort on and a sort order. Use the following form: property name, sort direction
        /// Sort direction can be ascending(asc) or descending(desc).
        /// For example, sort = fullName, desc indicates the result set should be sorted in descending order of the full name of each merchant.
        /// </summary>
        [ShippingApiQuery("sort", true)]
        public string Sort { get; set; }
        /// <summary>
        /// OAUTH token - supplied automatically
        /// </summary>
        [ShippingApiHeaderAttribute("Bearer")]
        public override StringBuilder Authorization { get; set; }
        /// <summary>
        /// Http Accept-Language header - defaults to the current culture
        /// </summary>
        [ShippingApiHeaderAttribute("Accept-Language")]
        public string AcceptLanguage { get; set; }
        /// <summary>
        /// Http Content-Type header. application/json
        /// </summary>
        public override string ContentType { get => "application/json"; }
    
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MerchantsReportRequest()
        {
            AcceptLanguage = CultureInfo.CurrentCulture.Name;
        }
        /// <summary>
        /// Check whether the report request is valid.
        /// </summary>
        /// <returns></returns>
        public bool Validate() => true;

    }

    /// <summary>
    /// Response object for the merchant report API service call
    /// </summary>
    public class MerchantsPageResponse
    {
        /// <summary>
        /// The merchants registered under your Developer ID. The merchant object’s elements are described in the next table below.
        /// </summary>
        [JsonProperty("content")]
        public IEnumerable<Merchant> Content { get; set; }
        /// <summary>
        /// The number of pages in the result set.
        /// </summary>
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
        /// <summary>
        /// 	The number of merchants in the result set.
        /// </summary>
        [JsonProperty("totalElements")]
        public int TotalElements { get; set; }
        /// <summary>
        /// If true, this is the last page of the result set.
        /// </summary>
        [JsonProperty("last")]
        public bool LastPage { get; set; }
        /// <summary>
        /// The number of merchants per page in the result set. The default is 20.
        /// </summary>
        [JsonProperty("size")]
        public int Size { get; set; }
        /// <summary>
        /// The page’s index number.Index numbers start at 0. If the value of number is 0, the API has returned the 1st page of the result set; 
        /// if the value is 1, the API has returned the 2nd page, and so on. To specify the index number in the API call, use the page 
        /// query parameter.
        /// </summary>
        [JsonProperty("number")]
        public int PageIndex { get; set; }
        /// <summary>
        /// The sort order, if specified in the request.
        /// </summary>
        [JsonProperty("sort")]
        public IEnumerable<ITransactionSort> Sort { get; set; }
        /// <summary>
        /// The number of merchants on the current page.
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
    /// Api call to get a report page. /shippingservices/v1/developers/{developerId}/merchants 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MerchantsReport<T> : ReportBase<T>
    {
        /// <summary>
        /// Developer Id
        /// </summary>
        public string DeveloperId { get; set; }
        private ISession _session;
        /// <summary>
        /// Create the merchants report
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="session"></param>
        public MerchantsReport( string developerId, ISession session = null) : base()
        {
            Provider = new MerchantsReportProvider(developerId, session);
            DeveloperId = developerId;
            _session = session;
        }
        /// <summary>
        /// Create the merchants report
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="expression"></param>
        /// <param name="developerId"></param>
        /// <param name="session"></param>
        public MerchantsReport(  MerchantsReportProvider provider, Expression expression, string developerId, ISession session = null) : base(provider,expression)
        {
            DeveloperId = developerId;
            _session = session;
        }
        /// <summary>
        /// Method to get a single page of the merchants report
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async static Task<ShippingApiResponse<MerchantsPageResponse>> MerchantsPage(MerchantsReportRequest request, ISession session = null)
        {
            if (session == null) session = Globals.DefaultSession;
            request.Authorization = new StringBuilder(session.AuthToken.AccessToken);
            return await WebMethod.Get<MerchantsPageResponse, MerchantsReportRequest>("/v1/developers/{DeveloperId}/merchants", request, session);
        }
        /// <summary>
        /// Iterate through the merchant report - fetches pages as necessary.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="filter">delegate mapping merchant record to a boolean. If false, the record is excluded from the report.</param>
        /// <param name="session"></param>
        /// <returns></returns>
        public static IEnumerable<Merchant> Report(MerchantsReportRequest request, Func<Merchant, bool> filter = null, ISession session = null)
        {
            if (session == null) session = Globals.DefaultSession;
            request.Page = 0;
            MerchantsPageResponse page;
            do
            {
                var response = MerchantsPage(request, session).GetAwaiter().GetResult();
                if (!response.Success) break;
                page = response.APIResponse;
                request.Page += 1;
                foreach (var t in page.Content)
                {
                    if ( filter == null || (filter != null && filter(t)) ) yield return t;
                }
            } while (!page.LastPage);
        }
    }
    /// <summary>
    /// Report provider - extends ReportProviderBase
    /// </summary>
    public class MerchantsReportProvider : ReportProviderBase, IQueryProvider
    {
        private string _developerId;
        internal ISession _session;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="session"></param>
        public MerchantsReportProvider( string developerId, ISession session) :base()
        {
            _developerId = developerId;
            _session = session;
        }
        /// <summary>
        /// Queryable's collection-returning standard query operators call this method. 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override IQueryable<TResult> CreateQuery<TResult>(Expression expression) 
        {
            return new MerchantsReport<TResult>(this, expression, _developerId);
        }
        /// <summary>
        /// Run the report.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <param name="isEnumerable"></param>
        /// <returns></returns>
        public override object Execute<TResult>(Expression expression, bool isEnumerable)
        {
            return Execute<TResult, Merchant, MerchantsReportRequest, MerchantsReportRequestFinder>(
                expression,
                isEnumerable,
                x => MerchantsReport<Merchant>.Report( x, null, _session),
                x => x.DeveloperId = _developerId
            );
        }
    }

    internal class MerchantsReportRequestFinder : RequestFinderVisitor<MerchantsReportRequest, Merchant>
    {
        protected override Expression VisitBinary(BinaryExpression be)
        {
            return base.VisitBinary(be);
        }
    }
}
