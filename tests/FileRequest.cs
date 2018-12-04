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
using System.IO;
using PitneyBowes.Developer.ShippingApi;
using PitneyBowes.Developer.ShippingApi.Mock;
using PitneyBowes.Developer.ShippingApi.Model;
namespace tests
{
    public static class FileRequest
    {
        public static ShippingApiResponse Request(string fullPath, ISession session)
        {
            List<ShippingApiMethod> methods = new List<ShippingApiMethod>() {
                new ShippingApiMethod() {Verb=HttpVerb.POST,
                    UriRegex = "shippingservices/v1/addresses/verify-suggest",
                    RequestType=typeof(VerifySuggestResponse),
                    RequestInterface = null,
                    ResponseType = typeof(Address) },
                new ShippingApiMethod() {Verb=HttpVerb.POST,
                    UriRegex="shippingservices/v1/addresses/verify",
                    RequestType=typeof(Address),
                    RequestInterface=typeof(IAddress),
                    ResponseType=typeof(Address)},
                new ShippingApiMethod() {Verb=HttpVerb.GET,
                    UriRegex="shippingservices/v1/manifests?originalTransactionId=(?<TransactionId>[^/]+)",
                    RequestType=typeof(RetryManifestRequest),
                    RequestInterface=null,
                    ResponseType=typeof(Manifest)},
                new ShippingApiMethod() {Verb=HttpVerb.POST,
                    UriRegex="shippingservices/v1/manifests",
                    RequestType=typeof(Manifest),
                    RequestInterface=typeof(IManifest),
                    ResponseType=typeof(Manifest)},
                new ShippingApiMethod() {Verb=HttpVerb.GET,
                    UriRegex="shippingservices/v1/manifests/(?<ManifestId>[^/]+)",
                    RequestType=typeof(ReprintManifestRequest),
                    RequestInterface=null,
                    ResponseType=typeof(Manifest)},
                new ShippingApiMethod() {Verb=HttpVerb.POST,
                    UriRegex="shippingservices/v1/rates",
                    RequestType=typeof(Shipment),
                    RequestInterface=typeof(IShipment),
                    ResponseType=typeof(Shipment)},
                new ShippingApiMethod() {Verb=HttpVerb.POST,
                    UriRegex="shippingservices/v1/shipments",
                    RequestType=typeof(Shipment),
                    RequestInterface=typeof(IShipment),
                    ResponseType=typeof(Shipment)},
                new ShippingApiMethod() {Verb=HttpVerb.DELETE,
                    UriRegex="shippingservices/v1/shipments/(?<ShipmentId>[^/]+)",
                    RequestType=typeof(CancelShipmentRequest),
                    RequestInterface=null,
                    ResponseType=typeof(CancelShipmentResponse)},
                new ShippingApiMethod() {Verb=HttpVerb.GET,
                    UriRegex="shippingservices/v1/shipments/(?<ShipmentId>[^/]+)",
                    RequestType=typeof(ReprintShipmentRequest),
                    RequestInterface=null,
                    ResponseType=typeof(Shipment)
                },
            };


            if (File.Exists(fullPath))
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                using (var mimeStream = new MimeStream(fileStream))
                {
                    mimeStream.SeekNextPart(); //request
                    mimeStream.ClearHeaders();
                    var request = RequestDeserializer.Request(mimeStream, methods, session);
                    if (request.Method.Verb == HttpVerb.POST && request.Method.UriRegex == "shippingservices/v1/shipments")
                    {
                        var shipment = (IShipment)request.Request;
                        shipment.TransactionId = Guid.NewGuid().ToString().Substring(15);
                        foreach( var option in shipment.ShipmentOptions )
                        {
                            if (option.ShipmentOption == ShipmentOption.SHIPPER_ID)
                            {
                                option.Value = session.GetConfigItem("ShipperID");
                            }
                        }
                    }
                    return request.Call(session);
                }
            }
            return null;
        }
    }
}
