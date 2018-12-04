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
using PitneyBowes.Developer.ShippingApi;


namespace PitneyBowes.Developer.ShippingApi.Fluent
{
    /// <summary>
    /// Class to create pickup objects and manage them.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PickupFluent<T> where T : IPickup, new()
    {
        private T _pickup;
        /// <summary>
        /// Implicit cast to the underlying IPickup type
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator T(PickupFluent<T> a)
        {
            return a._pickup;
        }
        /// <summary>
        /// Factory method to create a PickupFluent. Use instead of new(). Use at the start of the fluent method chain.
        /// </summary>
        /// <returns></returns>
        public static PickupFluent<T> Create()
        {
            var a = new PickupFluent<T>()
            {
                _pickup = new T()
            };
            return a;
        }
        /// <summary>
        /// Factory method to create a PickupFluent. Use instead of new(). Use at the start of the fluent method chain.
        /// </summary>
        /// <param name="pickup"></param>
        /// <returns></returns>
        public static PickupFluent<T> Create(IPickup pickup)
        {
            var a = new PickupFluent<T>
            {
                _pickup = (T)pickup
            };
            return a;
        }

        private PickupFluent()
        {
            _pickup = new T();
        }

        private PickupFluent(IPickup p)
        {
            _pickup = (T)p;
        }
        /// <summary>
        /// Unique trtansaction identifier. Must be less than 25 chars.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PickupFluent<T> TransactionId(string id)
        {
            _pickup.TransactionId = id;
            return this;
        }
        /// <summary>
        /// Schedule a pickup. Calls API method. Replaces underlying object with the object returned by the Web call
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public PickupFluent<T> Schedule(ISession session = null)
        {
            if (session == null) session = Globals.DefaultSession;
            var response = Api.Schedule<T>(_pickup, session).GetAwaiter().GetResult();
            if (response.Success)
            {
                _pickup = response.APIResponse;
            }
            else
            {
                throw new ShippingAPIException(response);
            }

            return this;
        }
        /// <summary>
        /// Cancels the pickup. Calls API method.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public PickupFluent<T>  Cancel(ISession session = null)
        {
            if (session == null) session = Globals.DefaultSession;
            var cancel = new PickupCancelRequest()
            {
                PickupId = _pickup.PickupId
            };

            var response = Api.CancelPickup(cancel, session).GetAwaiter().GetResult();
            if (response.Success)
            {
                cancel = response.APIResponse;
                //_pickup.Status = cancel.Status; //TODO: add to pickup class
            }
            else
            {
                throw new ShippingAPIException(response);
            }
            return this;
        }

        /// <summary>
        /// Sets the pickup address. Required.
        /// </summary>
        /// <value>The pickup address.</value>
        /// <param name="a"></param>
        /// <returns></returns>
        public PickupFluent<T> PickupAddress( IAddress a)
        {
            _pickup.PickupAddress = a;
            return this;
        }
        /// <summary>
        /// Gets or sets the carrier. Only USPS supported.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public PickupFluent<T> Carrier( Carrier c)
        {
            _pickup.Carrier = c;
            return this;
        }
        /// <summary>
        /// The parcel descriptions. Each object in the array describes a group of parcels.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public PickupFluent<T> PickupSummary(IEnumerable<IPickupCount> s)
        {
            foreach( var p in s)
            {
                _pickup.AddPickupCount(p);
            }
            return this;
        }
        /// <summary>
        /// Add a pickup count object to the PickupSummary IEnumerable
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public PickupFluent<T> AddPickupSummary(IPickupCount s)
        {
            _pickup.AddPickupCount(s);
            return this;
        }
        /// <summary>
        /// Add a pickup count object to the PickupSummary IEnumerable
        /// </summary>
        /// <typeparam name="C">IPickupCountType</typeparam>
        /// <typeparam name="W">IParcelWeight</typeparam>
        /// <param name="s">Service</param>
        /// <param name="c">Count</param>
        /// <param name="w">Total weight</param>
        /// <param name="u">Unit of weight</param>
        /// <returns></returns>
        public PickupFluent<T> AddPickupSummary<C,W>(PickupService s, int c, decimal w, UnitOfWeight u ) where C : IPickupCount, new() where W: IParcelWeight, new()
        {
            var ct = new C
            {
                ServiceId = s,
                TotalWeight = new W { UnitOfMeasurement = u, Weight = w },
                Count = c
            };
            _pickup.AddPickupCount(ct);
            return this;
        }

        /// <summary>
        /// Reference
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public PickupFluent<T> Reference(string s)
        {
            _pickup.Reference = s;
            return this;
        }
        /// <summary>
        /// REQUIRED. The location of the parcel at the pickup location.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public PickupFluent<T> PackageLocation(PackageLocation p)
        {
            _pickup.PackageLocation = p;
            return this;
        }
        /// <summary>
        /// Instructions for picking up the parcel. Required if packageLocation is set to Other.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public PickupFluent<T> SpecialInstructions(string s)
        {
            _pickup.SpecialInstructions = s;
            return this;
        }
        /// <summary>
        /// Response - Scheduled date of the pickup.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public PickupFluent<T> PickupDate( DateTime d)
        {
            _pickup.PickupDate = d;
            return this;
        }
        /// <summary>
        /// Response - 	A confirmation number for the pickup.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public PickupFluent<T> PickupConfirmationNumber( string p)
        {
            _pickup.PickupConfirmationNumber = p;
            return this;
        }
        /// <summary>
        /// Response - The pickup ID. You must specify this ID if canceling the pickup.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public PickupFluent<T> PickupId(string p)
        {
            _pickup.PickupId = p;
            return this;
        }
    }
}
