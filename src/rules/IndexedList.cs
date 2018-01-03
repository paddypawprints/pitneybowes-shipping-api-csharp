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
using System.Collections;
using System.Collections.Generic;

namespace PitneyBowes.Developer.ShippingApi.Rules
{
    /// <summary>
    /// List data structure where items can be associated with a key, and can be retrieved en mass with that key. It is like a multivalue dictionary.
    /// Used to hold the rate rules
    /// </summary>
    /// <typeparam name="K">Key type</typeparam>
    /// <typeparam name="T">List element type</typeparam>
    public class IndexedList<K,T> :  IEnumerable<T>
    {
        private Dictionary<K, List<T>> _dictionary = new Dictionary<K, List<T>>();
        /// <summary>
        /// Return the list of items associated with a key value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> this[K key] { get => _dictionary[key]; set { _dictionary[key] = value; } }
        /// <summary>
        /// Add an item with a key value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(K key, T value)
        {
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key, new List<T>());
            }
            _dictionary[key].Add(value);
        }
        /// <summary>
        /// If true, the IndexedList contains the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(K key) => _dictionary.ContainsKey(key);
        /// <summary>
        /// Emumerator for the entire
        /// </summary>
        /// <returns></returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (var k in _dictionary.Keys)
                foreach (var v in _dictionary[k])
                {
                    yield return v;
                }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
