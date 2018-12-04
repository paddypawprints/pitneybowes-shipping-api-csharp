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
using PitneyBowes.Developer.ShippingApi.Json;
using System.Text;

namespace PitneyBowes.Developer.ShippingApi
{

    [JsonObject(MemberSerialization.OptIn)]
    internal class TokenRequest : ShippingApiRequest, IDisposable
    {
        private string _key; 

        [JsonProperty(PropertyName="grant_type")]
        public string GrantType {get => "client_credentials";}

        [ShippingApiHeaderAttribute("Basic")]
        public override StringBuilder Authorization {get;set;}

        public override string RecordingSuffix => _key;

        public override string ContentType  => "application/x-www-form-urlencoded"; 

        public void BasicAuth(string key, StringBuilder secret ) //TODO: make this better
        {
            _key = key;
            var authHeader = new StringBuilder();
            authHeader.Append( key).Append(':').Append( secret);
            secret.Clear();
            var buffer = new char[authHeader.Length];
            authHeader.CopyTo(0, buffer, 0, buffer.Length);
            authHeader.Clear();
            var bytes = Encoding.UTF8.GetBytes( buffer );
            var base64array = new char[(bytes.Length * 4) / 3 + 10];
            Convert.ToBase64CharArray(bytes, 0, bytes.Length, base64array, 0);
            if (Authorization == null) Authorization = new StringBuilder(base64array.Length);
            else Authorization.Clear();
            foreach( var c in base64array)
            {
                Authorization.Append(c);
                if (c == '=') break;
            }
            bytes.Initialize();
            buffer.Initialize();
            base64array.Initialize();
        }

        public void Dispose()
        {
            if (Authorization!=null)
                Authorization.Clear();
        }

    }

    /// <summary>
    /// Each request to the PB Shipping APIs requires authentication via an OAuth token. This API generates the OAuth token based on the 
    /// base64-encoded value of the API key and secret associated with your PB Shipping APIs developer account. The token expires after 10 
    /// hours, after which you must create a new one.
    ///
    /// Note: If you do not have your API key and secret, retrieve them from Developer Hub.
    ///
    /// Things to Consider:
    ///    * Each authorization token in valid for 10 hours.
    ///    * It is recommended that each valid token be reused until it expires.
    ///  * Multiple concurrent valid tokens are allowed.
    /// </summary>
    public static class TokenMethods
    {

#pragma warning disable IDE1006 // Naming Styles
        /// <summary>
        /// Each request to the PB Shipping APIs requires authentication via an OAuth token. This API generates the OAuth token based on the 
        /// base64-encoded value of the API key and secret associated with your PB Shipping APIs developer account. The token expires after 10 
        /// hours, after which you must create a new one.
        ///
        /// Note: If you do not have your API key and secret, retrieve them from Developer Hub.
        ///
        /// Things to Consider:
        ///    * Each authorization token in valid for 10 hours.
        ///    * It is recommended that each valid token be reused until it expires.
        ///   * Multiple concurrent valid tokens are allowed.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        internal static async Task<ShippingApiResponse<Token>> token(ISession session = null) 

#pragma warning restore IDE1006 // Naming Styles
        {
            using (var request = new TokenRequest())
            {
                if (session == null) session = Globals.DefaultSession;
                request.BasicAuth(session.GetConfigItem("ApiKey"), session.GetApiSecret());
                return await session.Requester.HttpRequest<Token, TokenRequest>("/oauth/token", HttpVerb.POST, request, false, session);
            }
        }
    }
}