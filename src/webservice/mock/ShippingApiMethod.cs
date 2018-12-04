// /*
// Copyright 2016 Pitney Bowes Inc.
//
// Licensed under the MIT License(the "License"); you may not use this file except in compliance with the License.  
// You may obtain a copy of the License in the README file or at
//    https://opensource.org/licenses/MIT 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed 
// on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the License 
// for the specific language governing permissions and limitations under the License.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// */
using System;
using System.Reflection;
namespace PitneyBowes.Developer.ShippingApi.Mock
{
    /// <summary>
    /// Shipping API method. Used to call the shipping API based on reflection.
    /// </summary>
    public class ShippingApiMethod
    {
        /// <summary>
        /// Http verb.
        /// </summary>
        /// <value>The verb.</value>
        public HttpVerb Verb { get; set; }
        /// <summary>
        /// URI. Regex for URI parameters.
        /// </summary>
        /// <value>The URI regex.</value>
        public string UriRegex { get; set; }
        /// <summary>
        /// Type of request object
        /// </summary>
        /// <value>The type of the request.</value>
        public Type RequestType { get; set; }
        /// <summary>
        /// Interface type of request
        /// </summary>
        /// <value>The wrapper interface.</value>
        public Type RequestInterface { get; set; }
        /// <summary>
        /// Response type of API call
        /// </summary>
        /// <value>The type of the inner response.</value>
        public Type ResponseType { get; set; }
        /// <summary>
        /// Call the shipping API for given request object.
        /// </summary>
        /// <returns>The call.</returns>
        /// <param name="request">Request.</param>
        /// <param name="session">Session.</param>
        public ShippingApiResponse Call( IShippingApiRequest request, ISession session)
        {
            MethodInfo method;
            MethodInfo generic;

            if (request != null)
            {
                switch (Verb)
                {
                    case HttpVerb.DELETE:
                        method = typeof(WebMethod).GetMethod("DeleteWithBodySync");
                        generic = method.MakeGenericMethod(new Type[] { ResponseType, request.GetType() });
                        return (ShippingApiResponse)generic.Invoke(null, new object[] { UriRegex, request, session });
                    case HttpVerb.POST:
                        method = typeof(WebMethod).GetMethod("PostSync");
                        generic = method.MakeGenericMethod(new Type[] { ResponseType, request.GetType() });
                        return (ShippingApiResponse)generic.Invoke(null, new object[] { UriRegex, request, session });
                    case HttpVerb.PUT:
                        method = typeof(WebMethod).GetMethod("PutSync");
                        generic = method.MakeGenericMethod(new Type[] { ResponseType, request.GetType() });
                        return (ShippingApiResponse)generic.Invoke(null, new object[] { UriRegex, request, session });
                    default:
                        throw new Exception("Attempt to GET with a body");
                }
            }
            else
            {
                switch (Verb)
                {
                    case HttpVerb.DELETE:
                        method = typeof(WebMethod).GetMethod("DeleteSync");
                        generic = method.MakeGenericMethod(new Type[] { ResponseType, request.GetType() });
                        return (ShippingApiResponse)generic.Invoke(null, new object[] { UriRegex, request, session });
                    case HttpVerb.GET:
                        method = typeof(WebMethod).GetMethod("GetSync");
                        generic = method.MakeGenericMethod(new Type[] { ResponseType, request.GetType() });
                        return (ShippingApiResponse)generic.Invoke(null, new object[] { UriRegex, request, session });
                    default:
                        throw new Exception(String.Format("Attempt to {0} without a body", Verb.ToString()));
                }
            }
        }
    }
}
