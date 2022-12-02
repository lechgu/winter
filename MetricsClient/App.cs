using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var meter = new Meter("TestMeter");

var providerBuilder = Sdk.CreateMeterProviderBuilder()
.AddMeter(meter.Name)
.AddOtlpExporter(opts =>
{
    opts.Protocol = OtlpExportProtocol.HttpProtobuf;
    opts.Endpoint = new Uri("http://localhost:4318/v1/metrics");

});

var gauge = meter.CreateObservableGauge("NumberOfProcesses", () =>
                {
                    return new List<Measurement<int>>()
                    {
                        new Measurement<int>(
                            Process.GetProcesses().Length
                            )
                    };
                });

using var provider = providerBuilder.Build();

Console.WriteLine("Press any key to exit.");
while (!Console.KeyAvailable)
{
    provider.ForceFlush();
    Task.Delay(1000).Wait();
}
