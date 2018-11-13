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
using System.Collections.Generic;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Various environmental items that get injected into most methods. They can be overridden to configure the system.
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// Adds an item to the default configuration provider - a dictionary stored in the session.
        /// </summary>
        [Obsolete("Use configuration settings")]
        Action<string, string> AddConfigItem { get; set; }
        /// <summary>
        /// The current token is cached in the session.
        /// </summary>
        Token AuthToken { get; set; }
        /// <summary>
        /// Sets the endpoint for the service.
        /// </summary>
        string EndPoint { get; set; }
        /// <summary>
        /// Defines whether to throw exceptions due to deserialization and http errors. If exceptions are not thrown, errors can be seen in the 
        /// Errors member of the response object.
        /// </summary>
        [Obsolete("Use configuration setting \"ThrowExceptions\"=\"false\"")]
        bool ThrowExceptions { get; set; }
        /// <summary>
        /// Delegate to get the API secret. Best practice is to store the API secret encrypted and only decrypt at the last minute. The SDK
        /// does not store the API secret (except if you are using the default config from the sesssion, in which case the API Secret is store in 
        /// cleartext in memory). Authentication is infrequent - at startup and then every 10 hours, so a reasonable implementation would be to 
        /// store the API secret encrypted on disk, and then decrypt it in the GetApiSecret call.
        /// </summary>
        Func<StringBuilder> GetApiSecret { get; set; }
        /// <summary>
        /// Delegate to return a configuration item. Plug in your own config provider.
        /// </summary>
        Func<string, string> GetConfigItem { get; set; }
        /// <summary>
        /// Delegate to log fatal errors dur to configration missing or otherwise screwed up - e.g. not having deserialization classes defined.
        /// </summary>
        Action<string> LogConfigError { get; set; }
        /// <summary>
        /// Delegate to log debug messages. Plug in your own logger here.
        /// </summary>
        Action<string> LogDebug { get; set; }
        /// <summary>
        /// Delegate to log errors. Plug in your own logger here.
        /// </summary>
        Action<string> LogError { get; set; }
        /// <summary>
        /// Delegate to log warnings. Plug in your own logger here.
        /// </summary>
        Action<string> LogWarning { get; set; }
        /// <summary>
        /// Flag to indicate whether messages should be recorded. Recorded messages can be used for debugging or for replay by the mock requester.
        /// </summary>
        [Obsolete("Use configuration setting \"RecordAPICalls\"=\"true\" instead")]
        bool Record { get; set; }
        /// <summary>
        /// Flag to indicate whether to overwrite existing recording files.
        /// </summary>
        [Obsolete("Use configuration setting \"RecordOverwrite\"=\"true\" instead")]
        bool RecordOverwrite { get; set; }
        /// <summary>
        /// Path for the recording files.
        /// </summary>
        [Obsolete("Use configuration setting RecordRoot instead")]
        string RecordPath { get; set; }
        /// <summary>
        /// Requester encapsulates the http request and response. Two subclasses are provided, one that calls the web service and the other 
        /// that calls a mock interface which provides responses from messages stored in the file system.
        /// </summary>
        IHttpRequest Requester { get; set; }
        /// <summary>
        /// Number of retries in the event of network errors.
        /// </summary>
        [Obsolete("Use configuration setting Retries instead")]
        int Retries { get; set; }
        /// <summary>
        /// Object to hold mappings between the service contract interfaces, wrapper classes that implement the json/web service messages 
        /// and concrete objects that are created during deserialization.
        /// </summary>
        SerializationRegistry SerializationRegistry { get; }
        /// <summary>
        /// Counters holds statistics for each method call.
        /// </summary>
        Dictionary<string, Counters> Counters { get; }
        /// <summary>
        /// Update the performance counters with the result of the latest service call.
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="success">Whether the call was successful</param>
        /// <param name="time">Call duration in milliseconds</param>
        void UpdateCounters(string uri, bool success, TimeSpan time);
        /// <summary>
        /// Resets the counters to 0
        /// </summary>
        void ClearCounters();
    }
}