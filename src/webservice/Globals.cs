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

using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;


namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Singleton type class for system wide parameters. Hold HttpClient object for each url.
    /// </summary>
    public static class Globals
    {
        private static object _clientLock = new object();
        /// <summary>
        /// Default session - so you dont need to pass it to all of the methods
        /// </summary>
        public static ISession DefaultSession { get; set; }
        private static int _timeOutMilliseconds = 100000; //default .net value
        /// <summary>
        /// Web service call timeout. Has to be set when the HttpClient is initialized and cannot be changed.
        /// </summary>
        public static int TimeOutMilliseconds { get { return _timeOutMilliseconds; } set { _timeOutMilliseconds = value; } }
        private static Dictionary<string, HttpClient> _clientLookup = new Dictionary<string, HttpClient>();
        private static string _userAgent = "Pitney Bowes CSharp SDK 1.0";
        /// <summary>
        /// User agent string provided by each http call. Useful for server side troubleshooting and analytics.
        /// </summary>
        public static string UserAgent { get { return _userAgent; } set { _userAgent = value; } }
        /// <summary>
        /// Maximum number of http connections allowed for http client
        /// </summary>
        public static int MaxHttpConnections { get; set; }
        /// <summary>
        /// Per microsoft documentation, you should only have one http client object per url. The client objects are fully thread safe and 
        /// performance is affected if you create them on the fly.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public static HttpClient Client(string baseUrl)
        {
            if (!_clientLookup.TryGetValue(baseUrl, out HttpClient client))
            {
                lock (_clientLock)
                {
                    if (!_clientLookup.TryGetValue(baseUrl, out client))
                    {
                        if (MaxHttpConnections > 0)
                        {
                            client = new HttpClient(new HttpClientHandler
                            {
                                MaxConnectionsPerServer = MaxHttpConnections
                            });
                        }
                        else
                        {
                            client = new HttpClient();
                        }
                        client.BaseAddress = new Uri(baseUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("user-agent", UserAgent);
                        client.Timeout = new TimeSpan(0, 0, 0, 0, TimeOutMilliseconds);
                        _clientLookup.Add(baseUrl, client);
                    }
                    return client;
                }
            }
            else
            {
                return client;
            }
        }
        /// <summary>
        /// Turns an array of folder names into a path with the platform appropriate path separator. 
        /// There is undoutedly a better place to put this, just cant think of it - not in love with "util" classes.
        /// </summary>
        /// <param name="folders"></param>
        /// <returns></returns>
        // 
        public static string GetPath(params string[] folders)
        {
            var sb = new StringBuilder();
            foreach (var s in folders)
            {
                sb.Append(s);
                sb.Append(Path.DirectorySeparatorChar);
            }
            return sb.ToString();
        }
        /// <summary>
        /// Get platform appropriate config file path. %APPDATA% on windows and $HOME on unix type platforms.
        /// </summary>
        /// <returns></returns>
        public static string GetConfigPath(string fileName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return string.Format("{0}\\{1}", Environment.GetEnvironmentVariable("APPDATA"), fileName);
            }
            else
            {
                return string.Format("{0}/.{1}", Environment.GetEnvironmentVariable("HOME"), fileName);
            }
        }
    }
}