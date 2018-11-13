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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration; // Required for windows
using PitneyBowes.Developer.ShippingApi;
using PitneyBowes.Developer.ShippingApi.Model;
using System.IO;
using System.Text;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging.Xunit;

namespace tests
{
    public class TestSession
    {
        protected ILogger Logger { get; set; }
        private readonly ITestOutputHelper testOutputHelper;
        private readonly LoggerFactory loggerFactory;

        public TestSession(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            this.loggerFactory = new LoggerFactory(new[] { new XunitLoggerProvider(testOutputHelper) });
            Logger = loggerFactory.CreateLogger(this.GetType().Name);
        }

        private static IConfiguration Configuration { get; set; }

        static object lockObject = new object();
        static bool _initialized = false;

        public void InitializeFramework( )
        {
            if (_initialized) return;
            lock (lockObject)
            {
                // Initialize framework
                if (_initialized) return;
                _initialized = true;

 
                var configurationBuilder = new ConfigurationBuilder();
                configurationBuilder.SetBasePath(Globals.GetConfigBaseDirectory());

                var configFile = Globals.GetConfigFileName("shippingapisettings.json");

                configurationBuilder.AddJsonFile(Globals.GetConfigPath("shippingapisettings.json"), optional: false, reloadOnChange: true);
                Configuration = configurationBuilder.Build();

                var sandbox = new Session() { EndPoint = "https://api-sandbox.pitneybowes.com", Requester = new ShippingApiHttpRequest() };
                Model.RegisterSerializationTypes(sandbox.SerializationRegistry);

                if ( Configuration["Mock"] == "true" )
                {
                    sandbox.Requester = new ShippingAPIMock();
                }

                // Hook in your config provider
                sandbox.GetConfigItem = (s) => Configuration[s];

                // Hook in your logger
                sandbox.LogWarning = (s) => Logger.LogWarning(s);
                sandbox.LogError = (s) => Logger.LogError(s);
                sandbox.LogConfigError = (s) => Logger.LogCritical(s);
                sandbox.LogDebug = (s) => Logger.LogInformation(s);

                // Hook in your secure API key decryption
                sandbox.GetApiSecret = ()=>new StringBuilder(Configuration["ApiSecret"]);

                Globals.DefaultSession = sandbox;

            }
        }

    }
}
