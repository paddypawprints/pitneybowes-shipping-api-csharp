using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using PitneyBowes.Developer.ShippingApi;
using PitneyBowes.Developer.ShippingApi.Model;
using PitneyBowes.Developer.ShippingApi.Fluent;
using System.Diagnostics;

namespace MyShip
{
    class Program
    {
        static void Main(string[] args)
        {
            double throughput = 15;
            int numThreads = 30;
            int testLength = 36000;
            int sleeptime = 100;
            Globals.MaxHttpConnections = 200;
            //--------------------
            object threadlock = new object();
            DateTime? firstlabel = null;
            int labelsPerThread = (int)(throughput * testLength / numThreads);
            var sandbox = new Session() { EndPoint = "https://api-perf.pitneybowes.com", Requester = new ShippingApiHttpRequest() };

            sandbox.AddConfigItem("ApiKey", "rx9SgEMSkihx9CJRJhH7FkAPfa4U3rxN");
            sandbox.AddConfigItem("ApiSecret", "1AWINoX1lLvNHRo5");
            sandbox.AddConfigItem("ShipperID", "9024510429");
            sandbox.AddConfigItem("DeveloperID", "23232379");

            Model.RegisterSerializationTypes(sandbox.SerializationRegistry);
            Globals.DefaultSession = sandbox;
            string s = Guid.NewGuid().ToString().Substring(0,25 - string.Format("{0}{1}", numThreads, labelsPerThread*numThreads).Length);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Task> TaskList = new List<Task>();
            using (var timers = new FileStream("timers.txt", FileMode.Truncate))
            using (var writer = new StreamWriter(timers))
            {
                for (int i = 0; i < numThreads; i++)
                {
                    int k = i;
                    Console.WriteLine("Starting task {0}", i);
                    var LastTask = new Task(() =>
                    {
                        lock (threadlock)
                        {
                            if (firstlabel == null) firstlabel = DateTime.Now;
                        }
                        for (int j = 0; j < labelsPerThread; j++)
                        {
                            var labeltimer = new Stopwatch();
                            labeltimer.Start();
                            var label = print(s, k, j);
                            labeltimer.Stop();
                            if (label != null ) writer.WriteLine("{0} {1} {2} {3}", labeltimer.Elapsed, label.ParcelTrackingNumber, label.TransactionId, label.ShipmentId);
                            else writer.WriteLine("{0}", labeltimer.Elapsed);
                            Thread.Sleep(sleeptime);
                        }
                    });
                    LastTask.Start();
                    TaskList.Add(LastTask);
                }
                Task.WaitAll(TaskList.ToArray());
                stopwatch.Stop();
                Console.WriteLine("Start {0} End {1} Time {2}", firstlabel, DateTime.Now, stopwatch.Elapsed);
            }
            foreach (var ctr in sandbox.Counters)
            {
                foreach (var c in ctr.Value.CallHistogram)
                {

                    Console.WriteLine("{0},{1},{2}", ctr.Key, c.Key, c.Value);
                }
            }
        }

        static Shipment print( string s, int i, int j )
        {
            var shipment = ShipmentFluent<Shipment>.Create()
                .ToAddress((Address)AddressFluent<Address>.Create()
                    .AddressLines("643 Greenway Rd")
                    .PostalCode("28607")
                    .CountryCode("US"))
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
                                .ShipperId("9024510429")
                    )
               .TransactionId( string.Format("{0}{1}{2}", s, i, j));

            var label = Api.CreateShipment((Shipment)shipment).GetAwaiter().GetResult();
            if (!label.Success)
            {
                Console.WriteLine("Label {0} {1} failed:{1}", i, string.Format("{0}-{1}-{2}", s, i, j, label.HttpStatus));
                foreach( var e in label.Errors)
                {
                    Console.WriteLine("    {0} {1}", e.ErrorCode, e.Message);
                }
            }
            return label.APIResponse;
        }
    }
}

