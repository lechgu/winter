using System.Threading.Tasks;
using Grpc.Core;
using OpenTelemetry.Proto.Collector.Trace.V1;
using static OpenTelemetry.Proto.Collector.Trace.V1.TraceService;

namespace Winter.Backend.GrpcServices;

public class TraceService : TraceServiceBase
{
    public override Task<ExportTraceServiceResponse> Export(ExportTraceServiceRequest request, ServerCallContext context)
    {
        Console.WriteLine(request.ToString());
        return base.Export(request, context);
    }
}