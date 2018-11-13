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
using Newtonsoft.Json;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Token object returned by the call POST /oauth/token
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The OAuth token.
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
        /// <summary>
        /// Token type - BearerToken
        /// </summary>
        [JsonProperty(PropertyName = "tokenType")]
        public string TokenType { get; set; }
        /// <summary>
        /// Datetime the token was issued
        /// </summary>
        [JsonProperty(PropertyName = "issuedAt")]
        [JsonConverter(typeof(UnixMillisecondsTimeConverter))]
        public DateTimeOffset IssuedAt { get; set; }
        /// <summary>
        /// The OAuth expiry period in seconds. It is recommended that the token be re-used while it has not expired.
        /// </summary>
        [JsonProperty(PropertyName = "expiresIn")]
        public long ExpiresIn { get; set; }
        /// <summary>
        /// Not documented
        /// </summary>
        [JsonProperty(PropertyName = "clientID")]
        public string ClientID { get; set; }
        /// <summary>
        /// Issuer of the token - pitneybowes
        /// </summary>
        [JsonProperty(PropertyName = "org")]
        public string Org { get; set; }
        /// <summary>
        /// Determine whether the token has expired
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsExpired(DateTimeOffset time)
        {
            return IssuedAt.AddSeconds(ExpiresIn) < time;
        }
    }
}