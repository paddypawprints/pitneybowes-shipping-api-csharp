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

namespace PitneyBowes.Developer.ShippingApi.Model
{
    /// <summary>
    /// A merchant抯 payment method.
    /// </summary>
    public class PaymentInfo : IPaymentInfo
    {
        /// <summary>
        ///  REQUIRED.The payment type.Set this to the following:
        ///     * For Purchase Power, either:
        ///         - Create one one paymentInfo object with paymentType to POSTAGE_AND_SUBSCRIPTION, or
        ///         - Create two paymentInfo objects, one with paymentType set to SUBSCRIPTION and the other with paymentType set to POSTAGE.
        ///     * For Credit Card payment: Create two paymentInfo objects, one with paymentType set to SUBSCRIPTION and the other with 
        ///     paymentType set to POSTAGE.
        /// </summary>
        virtual public PaymentType PaymentType{get; set;}
        /// <summary>
        /// REQUIRED. The payment method. Possible values:
        /// For Purchase Power payment, set this to PurchasePower.
        /// For Credit Card payment, set this to CC.
        /// </summary>
        virtual public PaymentMethod PaymentMethod{get; set;}
        /// <summary>
        /// PURCHASE POWER ONLY. The merchant抯 encrypted TIN and, if applicable, encrypted BPN. The object includes an encrypted BPN only if the 
        /// merchant already has a Purchase Power account.
        /// </summary>
        virtual public IPpPaymentDetails PpPaymentDetails{get; set;}
        /// <summary>
        /// CREDIT CARD ONLY.The credit card information.This field is required if the paymentMethod field is set to CC.
        /// </summary>
        virtual public ICcPaymentDetails CcPaymentDetails{get; set;}
    }
}
