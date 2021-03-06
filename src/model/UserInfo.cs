﻿/*
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

namespace PitneyBowes.Developer.ShippingApi.Model
{
    /// <summary>
    /// An object containing the merchant抯 name, address, company, phone, and email for the merchant signup render method.
    /// </summary>
    public class UserInfo : IUserInfo
    {
        /// <summary>
        /// Merchant first name. Optional.
        /// </summary>
        virtual public string FirstName{get; set;}
        /// <summary>
        /// Merchant last name. Optional.
        /// </summary>
        virtual public string LastName{get; set;}
        /// <summary>
        /// Merchant company name. Required.
        /// </summary>
        virtual public string Company{get; set;}
        /// <summary>
        /// Merchant address. Required.
        /// </summary>
        virtual public IAddress Address{get; set;}
        /// <summary>
        /// Merchant phone number. Required.
        /// </summary>
        virtual public string Phone{get; set;}
        /// <summary>
        /// Merchant email address. Required.
        /// </summary>
        virtual public string Email{get; set;}
    }
}
