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
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Interface for all API requests. Implement this interface if you want to add new methods. In most instances you will want to inherit from
    /// ShippingApiRequest as well.
    /// </summary>
    public interface IShippingApiRequest
    {
        /// <summary>
        /// Http Content-Type header. For almost all purposes implement as a constant value returning "application/json"
        /// </summary>
        string ContentType {get;}
        /// <summary>
        /// OAUTH token. This is set automatically, implement as {get;set;}
        /// </summary>
        StringBuilder Authorization {get;set;}
        /// <summary>
        /// Return the actual URI by substuting for the {} elements. To use properties from the object inherit from ShippingAPiRequest
        /// ShippingApiRequest.GetUri()
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        string GetUri(string baseUrl);
        /// <summary>
        /// Get headers. To use properties from the object inherit from ShippingAPiRequest
        /// ShippingApiRequest.GetHeaders()
        /// </summary>
        /// <returns></returns>
        IEnumerable<Tuple<ShippingApiHeaderAttribute, string, string>> GetHeaders();
        /// <summary>
        /// Return the body of the http message. To use properties from the object inherit from ShippingAPiRequest
        /// ShippingApiRequest.SerializeBody()
        /// </summary>
        void SerializeBody(StreamWriter writer, ISession session);
        /// <summary>
        /// When using the recorder, add this string to the end of the file name. USed if there are multiple items which would be otherwise
        /// named the same.
        /// </summary>
        string RecordingSuffix { get; }
        /// <summary>
        /// Full path to the recording file.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        string RecordingFullPath(string resource, ISession session);
    }
}