using System;
using System.IO;
using System.Collections.Generic;
using PitneyBowes.Developer.ShippingApi;
using PitneyBowes.Developer.ShippingApi.Model;
using PitneyBowes.Developer.ShippingApi.Fluent;

namespace MyShip
{
    class Program
    {
        static void Main(string[] args)
        {
            var sandbox = new Session() { EndPoint = "https://api-sandbox.pitneybowes.com", Requester = new ShippingApiHttpRequest() };

            sandbox.AddConfigItem("ApiKey", "Ci4vEAgBP8Aww7TBwGOKhr43uKTPNyfO");
            sandbox.AddConfigItem("ApiSecret", "wgNEtZkNbP0iV8h0");
            sandbox.AddConfigItem("ShipperID", "9014888410");
            sandbox.AddConfigItem("DeveloperID", "46841939");

            Model.RegisterSerializationTypes(sandbox.SerializationRegistry);
            Globals.DefaultSession = sandbox;

            var shipment = ShipmentFluent<Shipment>.Create()
                .ToAddress((Address)AddressFluent<Address>.Create()
                    .AddressLines("643 Greenway Rd")
                    .PostalCode("28607")
                    .CountryCode("US")
                    .Verify())
               .FromAddress((Address)AddressFluent<Address>.Create()
                    .Company("Pitney Bowes Inc")
                    .AddressLines("27 Waterview Drive")
                    .CityTown("Shelton").StateProvince("CT").PostalCode("06484")
                    .CountryCode("US")
                    )
               .Parcel((Parcel)ParcelFluent<Parcel>.Create()
                    .Dimension(12, 12, 10)
                    .Weight(16m, UnitOfWeight.OZ))
               .Rates(RatesArrayFluent<Rates>.Create()
                    .USPSPriority<Rates, Parameter>())
               .Documents((List<IDocument>)DocumentsArrayFluent<Document>.Create()
                    .ShippingLabel(ContentType.URL, Size.DOC_4X6, FileFormat.PDF))
               .ShipmentOptions(ShipmentOptionsArrayFluent<ShipmentOptions>.Create()
                    .ShipperId("9014888410")
                    )
               .TransactionId(Guid.NewGuid().ToString().Substring(15));

            var label = Api.CreateShipment((Shipment)shipment).GetAwaiter().GetResult();
            if (label.Success)
            {
                var sw = new StreamWriter("label.pdf");
                foreach (var d in label.APIResponse.Documents)
                {
                    Api.WriteToStream(d, sw.BaseStream).GetAwaiter().GetResult();
                }
            }
        }
    }
}

