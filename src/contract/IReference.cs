// /*
// Copyright 2016 Pitney Bowes Inc.
//
// Licensed under the MIT License(the "License"); you may not use this file except in compliance with the License.  
// You may obtain a copy of the License in the README file or at
//    https://opensource.org/licenses/MIT 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed 
// on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the License 
// for the specific language governing permissions and limitations under the License.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// */
using System;
namespace PitneyBowes.Developer.ShippingApi
{
    /// <summary>
    /// Newgistics Only. This array maps client-generated identifiers to fields in the Newgistics package record. 
    /// The information in this array does not appear on the shipping label. The array takes up to three objects, 
    /// and each object maps an identifier to a specific Newgistics field.An object’s sequence in the array determines 
    /// which Newgistics field the object maps to.The first object in the array maps to the Newgistics “ReferenceNumber” 
    /// field; the second to the “AddlRef1” field; and the third to the “AddlRef2” field.
    ///
    /// Use the syntax shown below.Enter the names in the order shown.If you enter different names, the system will 
    /// change them to the names below.In the value fields, enter the client-generated identifiers.The values must be 
    /// strings of no more than 50 characters each. For additional information on using references, merchants can contact 
    /// their Newgistics representatives.
    /// </summary>
    public interface IReference
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        string Value { get; set; }
    }
}
