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

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Base class for Shipping Api request attributes. All attribute classes have a Name and Format
    /// </summary>
    public class ShippingApiAttribute : Attribute
    {
        /// <summary>
        /// Name of the property when used in a request. Either header or query parameter. 
        /// </summary>
        public string Name { get;set; }
        /// <summary>
        /// Format string for the property value to convert to a value for the header or url query
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// If the property value is null do not add the header or query
        /// </summary>
        public bool OmitIfEmpty { get; set; }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="omitIfEmpty"></param>
        public ShippingApiAttribute(string name, bool omitIfEmpty = true)
        {
            Name = name;
            OmitIfEmpty = omitIfEmpty;
        }

    }
    /// <summary>
    /// Attribute to indicate that a property value should go in the http header rather than the body json message.
    /// </summary>
    public class ShippingApiHeaderAttribute : ShippingApiAttribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="omitIfEmpty"></param>
        public ShippingApiHeaderAttribute(string name, bool omitIfEmpty = true) : base(name, omitIfEmpty) { }
    }
    /// <summary>
    /// Attribute class to indicate a property value is part of the query parameters rather than be included in the json message body.
    /// </summary>
    public class ShippingApiQueryAttribute : ShippingApiAttribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="omitIfEmpty"></param>
        public ShippingApiQueryAttribute(string name, bool omitIfEmpty = true) : base(name, omitIfEmpty) { }
    }

}