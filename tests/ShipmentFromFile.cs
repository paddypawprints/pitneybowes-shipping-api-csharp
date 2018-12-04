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

using PitneyBowes.Developer.ShippingApi;
using System;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace tests
{
    public class ShipmentFromFile : TestSession
    {
        public ShipmentFromFile(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            InitializeFramework();
        }

        [Fact]
        public void testDataFiles()
        {
            var pwd = Directory.GetCurrentDirectory();
            var directories = pwd.Split(Path.DirectorySeparatorChar);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < directories.Length - 3; i++)
            {
                sb.Append(directories[i]);
                sb.Append(Path.DirectorySeparatorChar);
            }
            sb.Append("testData");

            foreach ( var file in Directory.EnumerateFiles(sb.ToString(), "*.txt"))
            {
                Console.Write("Testing: ");
                Console.WriteLine(file);
                ShippingApiResponse response;
                try
                {
                    response = FileRequest.Request(file, Globals.DefaultSession);
                }
                catch (Exception e)
                {

                    throw;
                }
                Assert.NotNull(response);
                Assert.NotNull(response.ResponseObject);
            }
        }
    }
}
