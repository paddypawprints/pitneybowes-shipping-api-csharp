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
    /// Fluent class for creating manifest requests
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ManifestFluent<T> where T : IManifest, new()
    {
        private T _manifest;

        /// <summary>
        /// Static factory method to start the fluent method chain - use instead of new.
        /// </summary>
        /// <returns></returns>
        public static ManifestFluent<T> Create()
        {
            var p = new ManifestFluent<T>() { _manifest = new T() };
            return p;
        }

        private ManifestFluent()
        {

        }
        /// <summary>
        /// Extract the underlying manifest object with a cast - implicit casting doesnt work - not sure how to fix it.
        /// </summary>
        /// <param name="m"></param>
        public static implicit operator T(ManifestFluent<T> m)
        {
            return (T)m._manifest;
        }
        /// <summary>
        /// Underlying manifest object that is being populated.
        /// </summary>
        public IManifest Manifest 
        {
            get => _manifest;
            set => _manifest = (T)value;
        }
        /// <summary>
        /// Call the shipping API to submit the manifest request. Replaces the underlying manifest object with the one 
        /// returned from the call
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public ManifestFluent<T> Submit(ISession session = null)
        {
            if (session == null)
            {
                session = Globals.DefaultSession;
            }

            var response = Api.CreateManifest(_manifest, session).GetAwaiter().GetResult();
            if (response.Success)
            {
                _manifest = response.APIResponse;
            }
            else
            {
                throw new ShippingAPIException(response);
            }

            return this;
        }
        /// <summary>
        /// Call the shipping API to reprint the manifest request. Replaces the underlying manifest object with the one 
        /// returned from the call
        /// </summary>
        /// <param name="manifestId"></param>
        /// <param name="session"></param>
        /// <returns></returns>

        public ManifestFluent<T> Reprint(string manifestId, ISession session = null)
        {
            if (session == null)
            {
                session = Globals.DefaultSession;
            }

            var request = new ReprintManifestRequest() { ManifestId = manifestId };
            var response = Api.ReprintManifest<T>(request, session).GetAwaiter().GetResult();
            if (response.Success)
            {
                _manifest = response.APIResponse;
            }
            else
            {
                throw new ShippingAPIException(response);
            }

            return this;
        }
        /// <summary>
        /// Call the shipping API to retry the manifest request. Replaces the underlying manifest object with the one 
        /// returned from the call
        /// </summary>
        /// <param name="originalId"></param>
        /// <param name="session"></param>
        /// <returns></returns>

        public ManifestFluent<T> Retry(string originalId, ISession session = null)
        {
            if (session == null)
            {
                session = Globals.DefaultSession;
            }

            var request = new RetryManifestRequest() { OriginalTransactionId = originalId };
            var response = Api.RetryManifest<T>(request).GetAwaiter().GetResult();
            if (response.Success)
            {
                _manifest = response.APIResponse;
            }
            else
            {
                throw new ShippingAPIException(response);
            }

            return this;
        }
        /// <summary>
        /// Gets or sets the carrier. Valid value(s): USPS
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public ManifestFluent<T> Carrier(Carrier c)
        {
            _manifest.Carrier = c;
            return this;
        }
        /// <summary>
        /// Gets or sets the submission date, the date the shipments are tendered to the carrier.Default value is the current date in GMT/UTC.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public ManifestFluent<T> SubmissionDate(DateTime s)
        {
            _manifest.SubmissionDate = s;
            return this;
        }
        /// <summary>
        /// Gets or sets the induction postal code. Postal code where the shipments are tendered to the carrier.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ManifestFluent<T> FromAddress(IAddress a)
        {
            _manifest.FromAddress = a;
            return this;
        }
        /// <summary>
        /// Gets or sets the induction postal code. Postal code where the shipments are tendered to the carrier.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public ManifestFluent<T> InductionPostalCode(string p)
        {
            _manifest.InductionPostalCode = p;
            return this;
        }
        /// <summary>
        /// Gets or sets the parcel tracking numbers. Required if you choose to list shipment tracking numbers in the request object. 
        /// </summary>
        /// <param name="tl"></param>
        /// <returns></returns>
        public ManifestFluent<T> ParcelTrackingNumbers(IEnumerable<string> tl)
        {
            foreach (var t in tl)
                _manifest.AddParcelTrackingNumber(t);
            return this;
        }
        /// <summary>
        /// Adds the parcel tracking number to the end of the ParcelTrackingNumbersList
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public ManifestFluent<T> AddParcelTrackingNumber(string t)
        {
            _manifest.AddParcelTrackingNumber(t);
            return this;
        }
        /// <summary>
        /// Gets or sets the parameters. Use the Enum ManifestParameter to see options. 
        /// </summary>
        /// <param name="pl"></param>
        /// <returns></returns>
        public ManifestFluent<T> Parameters(IEnumerable<IParameter> pl)
        {
            foreach (var p in pl)
                _manifest.AddParameter(p);
            return this;
        }
        /// <summary>
        /// Adds the parameter to the end of the Parameters list.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public ManifestFluent<T> AddParameter(IParameter p)
        {
            _manifest.AddParameter(p);
            return this;
        }
        /// <summary>
        /// Adds the parameter to the end of the Parameters list.
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ManifestFluent<T> AddParameter<P>(string name, string value) where P : IParameter, new()
        {
            var p = new P
            {
                Name = name,
                Value = value
            };
            _manifest.AddParameter(p);
            return this;
        }
        /// <summary>
        /// Adds the parameter to the end of the Parameters list.
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="param"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ManifestFluent<T> AddParameter<P>(ManifestParameter param, string value) where P : IParameter, new()
        {
            AddParameter<P>(param.ToString(), value);
            return this;
        }
        /// <summary>
        /// Unique transaction ID, generated by client, up to 25 characters.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public ManifestFluent<T> TransactionId(string t)
        {
            _manifest.TransactionId = t;
            return this;
        }

    }
}

