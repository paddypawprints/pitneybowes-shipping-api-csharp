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

using System.Threading.Tasks;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// POST, PUT, GET, DELETE
    /// </summary>
    public enum HttpVerb
    {
        /// <summary>
        /// Http POST
        /// </summary>
        POST,
        /// <summary>
        /// Http PUT
        /// </summary>
        PUT,
        /// <summary>
        /// Http GET
        /// </summary>
        GET,
        /// <summary>
        /// Http DELETE
        /// </summary>
        DELETE
    }
    /// <summary>
    /// Interface for http call. Allows the call to be replaced by a mock for testing.
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// Http request method.
        /// </summary>
        /// <typeparam name="Response"></typeparam>
        /// <typeparam name="Request"></typeparam>
        /// <param name="resource"></param>
        /// <param name="verb"></param>
        /// <param name="request"></param>
        /// <param name="deleteBody"></param>
        /// <param name="session"></param>
        /// <returns></returns>
         Task<ShippingApiResponse<Response>> HttpRequest<Response, Request>(string resource, HttpVerb verb, Request request, bool deleteBody = false, ISession session = null) where Request : IShippingApiRequest;
    }
}
