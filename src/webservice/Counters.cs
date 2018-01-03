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

using System;
using System.Collections.Generic;

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Class to collect statistics on API performance
    /// </summary>
    public class Counters
    {
        /// <summary>
        /// Count of the number or error responses
        /// </summary>
        public int ErrorCount;
        /// <summary>
        /// Dictionary to store a count of the API response times in 10ms buckets. Bucketing is used to reduce the memory consumption of the 
        /// statistrics in the case where the SDK has been running a long time. 
        /// </summary>
        public Dictionary<int, int> CallHistogram = new Dictionary<int, int>();
        /// <summary>
        /// Look up the bucket and increment the count
        /// </summary>
        /// <param name="t"></param>
        public void AddCall(TimeSpan t)
        {
            int bucket = ((int)t.TotalMilliseconds) / 10; // 10 millisecond buckets
            if (!CallHistogram.ContainsKey((bucket))) CallHistogram.Add(bucket, 0);
            CallHistogram[bucket] = CallHistogram[bucket] + 1;
        }
    }
}