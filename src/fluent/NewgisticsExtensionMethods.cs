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
using System.Collections.Generic;
using PitneyBowes.Developer.ShippingApi.Model;


namespace PitneyBowes.Developer.ShippingApi.Fluent
{
    /// <summary>
    /// Newgistics extensions.
    /// </summary>
    public static class NewgisticsExtensions
    {
        /// <summary>
        /// Newgistics the options.
        /// </summary>
        /// <returns>The options.</returns>
        /// <param name="f">The object.</param>
        /// <param name="clientFacility">Client facility.</param>
        /// <param name="carrierFacility">Carrier facility.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static ShipmentOptionsArrayFluent<T> NewgisticsOptions<T>(
            this ShipmentOptionsArrayFluent<T> f,
            string clientFacility,
            string carrierFacility

            ) where T : class, IShipmentOptions, new()
        {
            f.Option(ShipmentOption.CLIENT_FACILITY_ID, clientFacility)
             .Add().Option(ShipmentOption.CARRIER_FACILITY_ID, carrierFacility);
            return f;
        }
        /// <summary>
        /// Newgistics rates.
        /// </summary>
        /// <returns>The rates.</returns>
        /// <param name="f">The object</param>
        /// <param name="s">Service</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static RatesArrayFluent<T> NewgisticsRates<T>(
            this RatesArrayFluent<T> f,
            Services s
            ) where T : class, IRates, new()
        {

            f.Carrier(Carrier.NEWGISTICS)
                .Service(s)
                .ParcelType(ParcelType.PKG);
            return f;
        }
        /// <summary>
        /// Reference the specified f, referenceNumber, addlRef1 and addlRef2.
        /// </summary>
        /// <returns>The reference.</returns>
        /// <param name="f">The object</param>
        /// <param name="referenceNumber">Reference number.</param>
        /// <param name="addlRef1">Addl ref1.</param>
        /// <param name="addlRef2">Addl ref2.</param>
        /// <typeparam name="T">IShipment concrete class.</typeparam>
        /// <typeparam name="R">IReference concrete class.</typeparam>
        public static ShipmentFluent<T> Reference<T, R>(
            this ShipmentFluent<T> f,
            string referenceNumber,
            string addlRef1 = null,
            string addlRef2 = null
            ) where T : class, IShipment, new()
            where R : class, IReference, new()
        {
            T shipment = ((T)f);
            var refs = new List<R>();
            shipment.References = refs;
            var refNumber = new R
            {
                Name = "ReferenceNumber",
                Value = referenceNumber
            };
            refs.Add(refNumber);
            if (addlRef1 != null)
            {
                var refAddlRef1 = new R
                {
                    Name = "AddlRef1",
                    Value = addlRef1
                };
                refs.Add(refAddlRef1);
            }
            if (addlRef2 != null)
            {
                var refAddlRef2 = new R
                {
                    Name = "AddlRef2",
                    Value = addlRef2
                };
                refs.Add(refAddlRef2);
            }
            return f;
        }
        /// <summary>
        /// Added notifications to the rate object.
        /// </summary>
        /// <returns>The notifications.</returns>
        /// <param name="r">Fluent object.</param>
        /// <param name="recipientNotificationType">Recipient notification type.</param>
        /// <param name="email">Email.</param>
        /// <typeparam name="T">IRates concrete class.</typeparam>
        /// <typeparam name="S">ISpecialServices concrete class.</typeparam>
        /// <typeparam name="P">IParameter concrete class.</typeparam>
        public static RatesArrayFluent<T> Notifications<T, S, P>(
            this RatesArrayFluent<T> r,
            RecipientNotificationType recipientNotificationType,
            string email
            ) 
            where T : class, IRates, new()
            where S : class, ISpecialServices, new()
            where P : class, IParameter, new()
        {
            var s = new S();
            s.SpecialServiceId = SpecialServiceCodes.NOTIFICATIONS;
            var np = new P();
            np.Name = NotificationType.RECIPIENT_NOTIFICATION_TYPE.ToString();
            np.Value = recipientNotificationType.ToString();
            s.AddParameter(np);
            var em = new P();
            em.Name = NotificationType.RECIPIENT_NOTIFICATION_EMAIL.ToString();
            em.Value = email;
            s.AddParameter(em);
            r.SpecialService(s);
            return r;
        }
        /// <summary>
        /// Adds newgistics style parcel to the Shipment.
        /// </summary>
        /// <returns>The parcel.</returns>
        /// <param name="f">Fluent object.</param>
        /// <param name="p">Parcel to add</param>
        /// <typeparam name="T">IShipment concrete type.</typeparam>
        /// <typeparam name="O">IShipmentOptions concrete type.</typeparam>
        public static ShipmentFluent<T> NewgisticsParcel<T,O>(
            this ShipmentFluent<T> f,
            IParcel p
            ) where T : class, IShipment, new()
              where O: class, IShipmentOptions, new()
        {
            f.Parcel(p);
            if (p.Dimension.IrregularParcelGirth > 0 )
            {
                T shipment = f;
                foreach( var o in shipment.ShipmentOptions )
                {
                    if (o.ShipmentOption == ShipmentOption.IS_RECTANGULAR ) 
                    {
                        o.Value = "false";
                        return f;
                    }
                }
                var rectOpt = new O();
                rectOpt.ShipmentOption = ShipmentOption.IS_RECTANGULAR;
                rectOpt.Value = "false";
            }
            return f;
        }
    }
}
