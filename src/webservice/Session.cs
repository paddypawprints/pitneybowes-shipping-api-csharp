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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Serialization;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Various environmental items that get injected into most methods. They can be overridden to configure the system.
    /// </summary>
    public class Session : ISession
    {
        private Dictionary<string, string> _configs = new Dictionary<string, string>();
        private ReaderWriterLockSlim _lock;
        /// <summary>
        /// Constructor sets defaults for most items so the system can run with minimal configuration.
        /// 
        /// Default parameters
        ///     Record = false;
        ///     RecordPath = Globals.GetPath(Path.GetTempPath(), "recordings", "shippingApi");
        ///     RecordOverwrite = false;
        ///     Retries = 3;
        ///     UserAgent = "Pitney Bowes CSharp SDK 1.0";
        ///     ThrowExceptions = false;
        ///     
        /// Configuration is set to use the dictionary in the session object
        ///     The configuration item "SANDBOX_ENDPOINT" is defined "https://api-sandbox.pitneybowes.com"
        ///     The configuration item "PRODUCTION_ENDPOINT" is defined "https://api-sandbox.pitneybowes.com"
        ///
        ///  Logging is disabled
        ///
        ///  API secret is retrieved from the "ApiSecret" config item
        ///
        /// </summary>
        public Session()
        {
            _configs.Add("SANDBOX_ENDPOINT", "https://api-sandbox.pitneybowes.com");
            _configs.Add("PRODUCTION_ENDPOINT", "https://api-sandbox.pitneybowes.com");
            GetConfigItem = (s) => {
                if (!_configs.ContainsKey(s))
                    throw new ArgumentException(string.Format("Config string {0} not found", s));
                return _configs[s];
            };
            AddConfigItem  = (k, v) => { _configs.Add(k, v); };
            LogWarning = (s) => { };
            LogError = (s) => { };
            LogConfigError = (s) => { };
            LogDebug = (s) => { };
            GetApiSecret = () => { return new StringBuilder(GetConfigItem("ApiSecret")); };
            SerializationRegistry = new SerializationRegistry();
            Counters = new Dictionary<string, Counters>();
            _lock = new ReaderWriterLockSlim();
        }
        /// <summary>
        /// Object to hold mappings between the service contract interfaces, wrapper classes that implement the json/web service messages 
        /// and concrete objects that are created during deserialization.
        /// </summary>
        public SerializationRegistry SerializationRegistry { get; }
        /// <summary>
        /// Requester encapsulates the http request and response. Two subclasses are provided, one that calls the web service and the other 
        /// that calls a mock interface which provides responses from messages stored in the file system.
        /// </summary>
        public IHttpRequest Requester { get; set; } 
        /// <summary>
        /// The current token is cached in the session.
        /// </summary>
        public Token AuthToken { get; set; }
        /// <summary>
        /// Time out for the call. The SDK will attempt to return within the timeout, although this is not guaranteed when token 
        /// authentication is required.
        /// </summary>
        public int TimeOutMilliseconds { get; set; }
        /// <summary>
        /// Number of retries in the event of network errors.
        /// </summary>
        public int Retries { 
            get 
            {
                if ( GetConfigItem("Retries") == null)
                {
                    return _retries;
                }
                else
                {
                    return int.Parse(GetConfigItem("Retries"));
                }
            } 
            set => _retries = value; 
        }
        private int _retries = 3;
        /// <summary>
        /// Defines whether to throw exceptions due to deserialization and http errors. If exceptions are not thrown, errors can be seen in the 
        /// Errors member of the response object.
        /// </summary>
        public bool ThrowExceptions 
        {
            get
            {
                if (GetConfigItem("ThrowExceptions") == null)
                {
                    return _throwExceptions;
                }
                else
                {
                    return bool.Parse(GetConfigItem("ThrowExceptions"));
                }
            }
            set => _throwExceptions = value;
        }
        private bool _throwExceptions = false;
        /// <summary>
        /// Delegate to return a configuration item. Plug in your own config provider.
        /// </summary>
        public Func<string, string> GetConfigItem { get; set; }
        /// <summary>
        /// Adds an item to the default configuration provider - a dictionary stored in the session.
        /// </summary>
        public Action<string, string> AddConfigItem { get; set; }
        /// <summary>
        /// Delegate to log warnings. Plug in your own logger here.
        /// </summary>
        public Action<string> LogWarning { get; set; }
        /// <summary>
        /// Delegate to log errors. Plug in your own logger here.
        /// </summary>
        public Action<string> LogError { get; set; }
        /// <summary>
        /// Delegate to log fatal errors dur to configration missing or otherwise screwed up - e.g. not having deserialization classes defined.
        /// </summary>
        public Action<string> LogConfigError { get; set; }
        /// <summary>
        /// Delegate to log debug messages. Plug in your own logger here.
        /// </summary>
        public Action<string> LogDebug { get; set; }
        /// <summary>
        /// Delegate to get the API secret. Best practice is to store the API secret encrypted and only decrypt at the last minute. The SDK
        /// does not store the API secret (except if you are using the default config from the sesssion, in which case the API Secret is store in 
        /// cleartext in memory). Authentication is infrequent - at startup and then every 10 hours, so a reasonable implementation would be to 
        /// store the API secret encrypted on disk, and then decrypt it in the GetApiSecret call.
        /// </summary>
        public Func<StringBuilder> GetApiSecret { get; set; }
        /// <summary>
        /// Sets the endpoint for the service.
        /// </summary>
        public string EndPoint { get; set; }
        /// <summary>
        /// Flag to indicate whether messages should be recorded. Recorded messages can be used for debugging or for replay by the mock requester.
        /// </summary>
        public bool Record 
        {
            get
            {
                if (GetConfigItem("RecordAPICalls") == null)
                {
                    return _record;
                }
                else
                {
                    return bool.Parse(GetConfigItem("RecordAPICalls"));
                }
            }
            set => _record = value;

        }
        private bool _record = false;
        /// <summary>
        /// Path for the recording files.
        /// </summary>
        public string RecordPath {
            get => GetConfigItem("RecordRoot") ??  _recordPath;
            set => _recordPath = value;
        }
        private string _recordPath = Globals.GetPath(Path.GetTempPath(), "recordings", "shippingApi");
        /// <summary>
        /// Flag to indicate whether to overwrite existing recording files.
        /// </summary>
        public bool RecordOverwrite 
        {
            get
            {
                if (GetConfigItem("RecordOverwrite") == null)
                {
                    return _recordOverwrite;
                }
                else
                {
                    return bool.Parse(GetConfigItem("RecordOverwrite"));
                }
            }
            set => _recordOverwrite = value;
        }
        private bool _recordOverwrite = true;
        /// <summary>
        /// Counters holds statistics for each method call.
        /// </summary>
        public Dictionary<string, Counters> Counters { get; internal set; }
        /// <summary>
        /// Gets or sets the JSON.Net trace writer.
        /// </summary>
        /// <value>The trace writer.</value>
        public ITraceWriter TraceWriter { get; set; }

        private void AddCounterIfRequired(string key)
        {
            _lock.EnterWriteLock();
            try
            {
                if (!Counters.ContainsKey(key))
                {
                    Counters.Add(key, new Counters());
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }

        }
        /// <summary>
        /// Update the performance counters with the result of the latest service call.
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="success">Whether the call was successful</param>
        /// <param name="time">Call duration in milliseconds</param>
        public void UpdateCounters(string uri, bool success, TimeSpan time)
        {
            AddCounterIfRequired(uri);
            _lock.EnterReadLock();
            try
            {
                if (!success)
                {
                    Counters[uri].ErrorCount++;
                }
                else
                {
                    Counters[uri].AddCall(time);
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
        /// <summary>
        /// Reset the counters
        /// </summary>
        public void ClearCounters()
        {
            _lock.EnterWriteLock();
            try
            {
                Counters.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}