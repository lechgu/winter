using System.Threading.Tasks;
using Grpc.Core;
using OpenTelemetry.Proto.Collector.Metrics.V1;
using static OpenTelemetry.Proto.Collector.Metrics.V1.MetricsService;

namespace Winter.Backend.GrpcServices;

public class MetricsService : MetricsServiceBase
{
    public override Task<ExportMetricsServiceResponse> Export(ExportMetricsServiceRequest request, ServerCallContext context)
    {
        Console.WriteLine(request.ToString());
        Console.Out.Flush();
        return Task.FromResult(new ExportMetricsServiceResponse());
    }
}