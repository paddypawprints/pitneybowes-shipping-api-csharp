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

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// An object containing the merchant’s name, address, company, phone, and email for the merchant signup render method.
    /// </summary>
    public interface IUserInfo
    {
        /// <summary>
        /// Merchant first name. Optional.
        /// </summary>
        string FirstName { get; set; }
        /// <summary>
        /// Merchant last name. Optional.
        /// </summary>
        string LastName { get; set; }
        /// <summary>
        /// Merchant company name. Required.
        /// </summary>
        string Company { get; set; }
        /// <summary>
        /// Merchant address. Required.
        /// </summary>
        IAddress Address { get; set; }
        /// <summary>
        /// Merchant phone number. Required.
        /// </summary>
        string Phone { get; set; }
        /// <summary>
        /// Merchant email address. Required.
        /// </summary>
        string Email { get; set; }
    }

    public static partial class InterfaceValidators
    {
        /// <summary>
        /// Extension method validator for IUserInfo.
        /// If false, the object is not valid. If true, the object may or may not be valid.
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static bool IsValid(this IUserInfo u)
        {
            if (u.Company == null || u.Company.Equals("")) return false;
            if (u.Address == null || !u.Address.IsValidDeliveryAddress()) return false;
            if (u.Phone == null || u.Phone.Equals("")) return false;
            if (u.Email == null || u.Email.Equals("")) return false;
            return true;
        }
    }

}
