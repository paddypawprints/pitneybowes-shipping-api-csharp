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
using System.Reflection;
using System.Collections.Generic;

#if NETSTANDARD1_3
namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Compatibility extension methods for .net standard 1.3. Used in Linq provider.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Can typeFrom be assigned to TypeTo (with cast if ncessary).
        /// </summary>
        /// <param name="typeTo"></param>
        /// <param name="typeFrom"></param>
        /// <returns></returns>
        public static bool IsAssignableFrom(this Type typeTo, Type typeFrom)
        {
            return typeTo.GetTypeInfo().IsAssignableFrom(typeFrom.GetTypeInfo());
        }
        /// <summary>
        /// Get property from a type by name
        /// </summary>
        /// <param name="t">Type</param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(this Type t, string propName)
        {
            return t.GetRuntimeProperty(propName);
        }
        /// <summary>
        /// Get all properties for a type as an enumeration
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperties(this Type t)
        {
            return t.GetRuntimeProperties();
        }
        /// <summary>
        /// Get all interfaces implemented by a type
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetInterfaces(this Type t)
        {
            return t.GetTypeInfo().ImplementedInterfaces;
        }
        /// <summary>
        /// Get generic arguments from a generic type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Type[] GetGenericArguments(this Type t)
        {
            return t.GetTypeInfo().GenericTypeArguments;
        }
    }
}
#endif
