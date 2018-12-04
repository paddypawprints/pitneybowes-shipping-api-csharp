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

namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// CREDIT CARD ONLY. The credit card information. This field is required if the paymentMethod field is set to CC.
    /// </summary>
    public interface ICcPaymentDetails
    {
        /// <summary>
        /// The type of credit card - Visa, Mastercard etc,
        /// </summary>
        CreditCardType CcType { get; set; }
        /// <summary>
        /// The tokenized credit card number.
        /// </summary>
        string CcTokenNumber { get; set; }
        /// <summary>
        /// The month and year the card expires, entered as the two-digit month and four-digit year separated by a backslash. 
        /// For example: 06/2021
        /// </summary>
        string CcExpirationDate { get; set; }
        /// <summary>
        /// The three- or four-digit Card Verification Value.
        /// </summary>
        string CccvvNumber { get; set; }
        /// <summary>
        /// The address associated with the credit card account.
        /// </summary>
        IAddress CcAddress { get; set; }
    }
}
