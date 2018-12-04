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
namespace PitneyBowes.Developer.ShippingApi.Mock
{
    /// <summary>
    /// Bundle method and request together for return type for RequestDeserializer.
    /// </summary>
    public class ShippingApiMethodRequest
    {
        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>The method.</value>
        public ShippingApiMethod Method { get; set; }
        /// <summary>
        /// Gets or sets the request object.
        /// </summary>
        /// <value>The request.</value>
        public IShippingApiRequest Request { get; set; }
        /// <summary>
        /// Call the specified method.
        /// </summary>
        /// <returns>The call.</returns>
        /// <param name="session">Session.</param>
        public ShippingApiResponse Call(ISession session)
        {
            return Method.Call(Request, session);
        }
    }
}
