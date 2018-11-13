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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PitneyBowes.Developer.ShippingApi.Report
{
    /// <summary>
    /// Base report class implementing common functionality to support linq queries. Works closely with ReportProviderBase 
    /// - extend both to implement your report.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReportBase<T> : IOrderedQueryable<T>
    {
        /// <summary>
        /// Linq Query provider. Extend ReportProviderBase to implement your report.
        /// </summary>
        public IQueryProvider Provider { get; protected set; }
        /// <summary>
        /// Linq expression
        /// </summary>
        public Expression Expression { get; private set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportBase()
        {
            Expression = Expression.Constant(this);
        }
        /// <summary>
        /// Constructor as required by LINQ
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="expression"></param>
        public ReportBase(IQueryProvider provider, Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression is null");
            }

            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression type mismatch");
            }

            Provider = provider ?? throw new ArgumentNullException("provider is null");
            Expression = expression;
        }
        /// <summary>
        /// Type of object returned - row in a report.
        /// </summary>
        public Type ElementType
        {
            get => typeof(T);
        }
        /// <summary>
        /// Enumerator for the result set
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
        }
        /// <summary>
        /// Untyped Enumerator for the result set (not used)
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (Provider.Execute<IEnumerable>(Expression)).GetEnumerator();
        }

    }


}

