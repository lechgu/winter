using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

// var serviceName = "MyCompany.MyProduct.MyService";
// var serviceVersion = "1.0.0";

// using var tracerProvider = Sdk.CreateTracerProviderBuilder()
//     .AddSource(serviceName)
//     .SetResourceBuilder(
//         ResourceBuilder.CreateDefault()
//             .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
//     .AddConsoleExporter()
//     .AddOtlpExporter(opt =>
//     {
//         opt.Protocol = OtlpExportProtocol.Grpc;
//         opt.Endpoint = new Uri("http://localhost:8080");
//     })
//     .Build();

// var myActivitySource = new ActivitySource(serviceName);

// using var activity = myActivitySource.StartActivity("SayHello");
// activity?.SetTag("foo", 1);
// activity?.SetTag("bar", "Hello, World!");
// activity?.SetTag("baz", new int[] { 1, 2, 3 });

// Meter s_meter = new Meter("HatCo.HatStore", "1.0.0");
// s_meter.CreateObservableGauge<int>("hats")
// Counter<int> s_hatsSold = s_meter.CreateCounter<int>(name: "hats-sold",
//                                                             unit: "Hats",
//                                                             description: "The number of hats sold in our store");

// using MeterProvider meterProvider = Sdk.CreateMeterProviderBuilder()
//                .AddMeter("HatCo.HatStore")
//                   //    .AddConsoleExporter()
//                   .AddOtlpExporter(opts =>
//                   {
//                       opts.Protocol = OtlpExportProtocol.Grpc;
//                       opts.Endpoint = new Uri("http://localhost:8080");
//                       opts.TimeoutMilliseconds = 100;
//                       opts.ExportProcessorType = ExportProcessorType.Simple;
//                   })
//                .Build();

// Console.WriteLine("Press any key to exit");
// while (!Console.KeyAvailable)
// {
//     // Pretend our store has a transaction each second that sells 4 hats
//     Thread.Sleep(1000);
//     s_hatsSold.Add(4);
// }

var meter = new Meter("TestMeter");

var providerBuilder = Sdk.CreateMeterProviderBuilder()
.AddMeter(meter.Name)
.AddOtlpExporter(opts =>
{
    opts.Protocol = OtlpExportProtocol.Grpc;
    opts.Endpoint = new Uri("http://localhost:8080");
    opts.TimeoutMilliseconds = 100;
    opts.ExportProcessorType = ExportProcessorType.Simple;
})
.AddConsoleExporter();


// var counter = meter.CreateCounter<int>("counter", "Seconds", "measure seconds");
var gauge = meter.CreateObservableGauge("gauge", () =>
                {
                    return new List<Measurement<int>>()
                    {
                        new Measurement<int>(
                            DateTime.UtcNow.Second)
                    };
                });

using var provider = providerBuilder.Build();

System.Console.WriteLine("Press any key to exit.");
while (!Console.KeyAvailable)
{
    // counter.Add(1);
    provider.ForceFlush();
    Task.Delay(1000).Wait();
}
