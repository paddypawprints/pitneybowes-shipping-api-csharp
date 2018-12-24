using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using PitneyBowes.Developer.ShippingApi;
using PitneyBowes.Developer.ShippingApi.Model;
using PitneyBowes.Developer.ShippingApi.Fluent;
using PitneyBowes.Developer.ShippingApi.Mock;

namespace MyShip
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Globals.GetConfigBaseDirectory());

            var configFile = Globals.GetConfigFileName("shippingapisettings.json");

            configurationBuilder.AddJsonFile(Globals.GetConfigPath("shippingapisettings.json"), optional: false, reloadOnChange: true);
            var Configuration = configurationBuilder.Build();

            var sandbox = new Session() { EndPoint = "https://api-sandbox.pitneybowes.com", Requester = new ShippingApiHttpRequest() };
            Model.RegisterSerializationTypes(sandbox.SerializationRegistry);

            if (Configuration["Mock"] == "true")
            {
                sandbox.Requester = new ShippingAPIMock();
            }

            // Hook in your config provider
            sandbox.GetConfigItem = (s) => Configuration[s];

            // Hook in your logger
            sandbox.LogWarning = (s) => Console.WriteLine(s);
            sandbox.LogError = (s) => Console.WriteLine(s);
            sandbox.LogConfigError = (s) => Console.WriteLine(s);
            sandbox.LogDebug = (s) => Console.WriteLine(s);

            // Hook in your secure API key decryption
            sandbox.GetApiSecret = () => new StringBuilder(Configuration["ApiSecret"]);

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
                    .ShipperId("your shipper id")    // ******* dont forget this one too *******
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

