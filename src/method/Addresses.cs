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

using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PitneyBowes.Developer.ShippingApi.Json;


namespace PitneyBowes.Developer.ShippingApi
{

    /// <summary>
    /// Address suggestions.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class AddressSuggestions
    {
        /// <summary>
        /// The part of the address that was changed from the original.
        /// </summary>
        [JsonProperty("suggestionType")]
        public string SuggestionType { get; set; } //TODO - figure out possible values and convert to enum
        /// <summary>
        /// Each address object provides an alternative suggested address.
        /// </summary>
        /// <value>The addresses.</value>
        [JsonProperty("address")]
        public IEnumerable<IAddress> Addresses { get; set; }
    }

    /// <summary>
    /// Response object for VerifySuggestAddress web method
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VerifySuggestResponse
    {
        /// <summary>
        /// Validated address
        /// </summary>
        [JsonProperty("address")]
        public IAddress Address { get; set; }
        /// <summary>
        /// List of address suggestions.
        /// </summary>
        [JsonProperty("suggestions")]
        public AddressSuggestions Suggestions { get; set; }
    }

    /// <summary>
    /// Pitney Bowes Shipping API methods
    /// </summary>
    public static partial class Api
    {
        /// <summary>
        /// Address validation verifies and cleanses postal addresses within the United
        /// States to help ensure packages are rated accurately and shipments arrive at
        /// their final destinations on time.This API call sends an address to be
        /// verified.The response returns a valid address.
        /// </summary>
        /// <returns>The address.</returns>
        /// <param name="request">Request.</param>
        /// <param name="session">Session.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async static Task<ShippingApiResponse<T>> VerifyAddress<T>(T request, ISession session = null) where T : IAddress, new()
        {
            var verifyRequest = new JsonAddress<T>(request);
            return await WebMethod.Post<T, JsonAddress<T>>("/shippingservices/v1/addresses/verify", verifyRequest, session);
        }
        /// <summary>
        /// Verifies the suggest address. This POST operation obtains suggested addresses in cases where the
        /// Address Validation API call has returned an error.
        /// </summary>
        /// <returns>The suggest address.</returns>
        /// <param name="request">Request.</param>
        /// <param name="session">Session.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async static Task<ShippingApiResponse<VerifySuggestResponse>> VerifySuggestAddress<T>(T request, ISession session = null) where T : IAddress, new()
        {
            var verifyRequest = new JsonAddress<T>(request) { Suggest = true };
            return await WebMethod.Post<VerifySuggestResponse, JsonAddress<T>>("/shippingservices/v1/addresses/verify-suggest", verifyRequest, session);
        }
    }
}
