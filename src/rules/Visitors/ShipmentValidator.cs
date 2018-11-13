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

namespace PitneyBowes.Developer.ShippingApi.Rules
{
    /// <summary>
    /// Class to check whether a shipment complies with all of the carrier rules.
    /// </summary>
    public class ShipmentValidator : IRateRuleVisitor
    {
        /// <summary>
        /// State of the validation determination.
        /// </summary>
        public enum ValidationState
        {
            /// <summary>
            /// The shipment is not vaid - it has violated one or more of the rules
            /// </summary>
            INVALID,
            /// <summary>
            /// The shipment is valid - it complies with ALL rules
            /// </summary>
            VALID,
            /// <summary>
            /// The shipment may or may not be vaid. It complies with all rules evaluated so far but there may be unchecked rules 
            /// that invalidate the shipment.
            /// </summary>
            PROCESSING
        }
        private ValidationState _state;
        private IShipment _shipment;
        private IRates _rate { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ShipmentValidator()
        {
            _state = ValidationState.PROCESSING;
        }
        /// <summary>
        /// If true, the shipment is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _state == ValidationState.VALID;
            }
        }
        /// <summary>
        /// Reason the validation failed.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Validate a shipment with a rule set.
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public bool Validate(IShipment shipment, CarrierRule rules)
        {
            Shipment = shipment;
            Visit(rules);
            return IsValid;
        }

        /// <summary>
        /// Shipment being validated.
        /// </summary>
        public IShipment Shipment
        {
            get
            {
                return _shipment;
            }
            set
            {
                _shipment = value;
                var i = _shipment.Rates.GetEnumerator();
                i.MoveNext();
                _rate = i.Current;
            }
        }

        /// <summary>
        /// Check the carrier, and then the service rules for the shipment.
        /// </summary>
        /// <param name="carrierRule"></param>
        public void Visit(CarrierRule carrierRule)
        {
            if (_rate.Carrier != carrierRule.Carrier)
            {
                _state = ValidationState.INVALID;
                Reason = string.Format("Carrier {0} is not contained in rules", _rate.Carrier);
                return;
            }
            if (Shipment.ToAddress.CountryCode != carrierRule.DestinationCountry)
            {
                _state = ValidationState.INVALID;
                Reason = string.Format("Destination country {0} is not contained in rules", Shipment.ToAddress.CountryCode);
                return;
            }
            if (Shipment.FromAddress.CountryCode != carrierRule.OriginCountry)
            {
                _state = ValidationState.INVALID;
                Reason = string.Format("Origin country {0} is not contained in rules", Shipment.FromAddress.CountryCode);
                return;
            }
            if (!carrierRule.ServiceRules.ContainsKey((Services)_rate.ServiceId))
            {
                _state = ValidationState.INVALID;
                Reason = string.Format("Carrier {0} does not support format {1}", _rate.Carrier, _rate.ServiceId);
                return;
            }
            foreach (var rule in carrierRule.ServiceRules[(Services)_rate.ServiceId])
            {
                if ( _state == ValidationState.PROCESSING)
                    rule.Accept(this);
            }
            if (_state == ValidationState.PROCESSING)
            {
                _state = ValidationState.VALID;
                Reason = "Valid";
            }
        }
        /// <summary>
        /// Check the service rul, then the parcel type rules.
        /// </summary>
        /// <param name="serviceRule"></param>
        public void Visit(ServiceRule serviceRule)
        {
            if (_rate.ServiceId == null ||_rate.ServiceId != serviceRule.ServiceId) return;

            if (!serviceRule.ParcelTypeRules.ContainsKey((ParcelType)_rate.ParcelType))
            {
                _state = ValidationState.INVALID;
                Reason = string.Format("Service {0} does not support parcel type {1}", serviceRule.ServiceId, _rate.ParcelType);
                return;
            }
            foreach ( var rule in serviceRule.ParcelTypeRules[(ParcelType)_rate.ParcelType])
            {
                if (_state == ValidationState.PROCESSING)
                    rule.Accept(this);
            }
        }

        /// <summary>
        /// Check the parcel rule and then the special services rules for the shipment.
        /// </summary>
        /// <param name="parcelRule"></param>
        public void Visit(ParcelTypeRule parcelRule)
        {
            if (_rate.ParcelType == null || _rate.ParcelType != parcelRule.ParcelType) return;

            foreach (var ss in _rate.SpecialServices)
            {
                if (!parcelRule.SpecialServiceRules.ContainsKey(ss.SpecialServiceId))
                {
                    Reason = string.Format("Parcel type {0} does not support special service type {1}", parcelRule.ParcelType, ss.SpecialServiceId);
                    _state = ValidationState.INVALID;
                    return;
                }
                if (!parcelRule.FitsDimensions(_shipment.Parcel.Dimension))
                { 
                    Reason = string.Format("Parcel is outside of the dimension requirements for {0}", parcelRule.ParcelType);
                    _state = ValidationState.INVALID;
                    return;
                }
                if (!parcelRule.HoldsWeight(_shipment.Parcel.Weight))
                {
                    Reason = string.Format("Parcel is outside of the weight requirements for {0}", parcelRule.ParcelType);
                    _state = ValidationState.INVALID;
                    return;
                }
                foreach (var rule in parcelRule.SpecialServiceRules[ss.SpecialServiceId])
                {
                    if (_state == ValidationState.PROCESSING)
                        rule.Accept(this);
                }
            }
 
        }
        /// <summary>
        /// Visit the special services rule node. Check the parameter rules, prerequisite rules and incompatible service rules for the parcel.
        /// </summary>
        /// <param name="specialServicesRule"></param>
        public void Visit(SpecialServicesRule specialServicesRule)
        {
            foreach (var ss in _rate.SpecialServices)
            {
                if ( ss.SpecialServiceId == specialServicesRule.SpecialServiceId)
                {
                    if (specialServicesRule.InputParameterRules != null && specialServicesRule.InputParameterRules.Count > 1)
                    {
                        if (!specialServicesRule.HasRequiredParameters(ss))
                        {
                            _state = ValidationState.INVALID;
                            Reason = string.Format("Special service {0} is missing required parameters", ss.SpecialServiceId);
                            return;
                        }
                        if (!specialServicesRule.IsValidParameterValues(ss))
                        {
                            Reason = string.Format("Special service {0} has value outside the permissable range", ss.SpecialServiceId);
                            _state = ValidationState.INVALID;
                            return;
                        }
                    }
                }
                else
                {
                    if (specialServicesRule.IncompatibleSpecialServices != null && specialServicesRule.IncompatibleSpecialServices.Contains(ss.SpecialServiceId))
                    {
                        Reason = string.Format("Special service {0} is incompatible with other selected services", ss.SpecialServiceId);
                        _state = ValidationState.INVALID;
                        return;
                    }
                }
            }
            if (specialServicesRule.PrerequisiteRules != null && !specialServicesRule.IsValidPrerequisites(_rate.SpecialServices))
            {
                Reason = string.Format("Special service {0} is missing prerequisites", specialServicesRule.SpecialServiceId);
                _state = ValidationState.INVALID;
                return;

            }

        }
    }
}
