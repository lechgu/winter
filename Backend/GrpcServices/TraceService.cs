using Grpc.Core;
using OpenTelemetry.Proto.Collector.Trace.V1;
using Winter.Shared.Dto;
using static OpenTelemetry.Proto.Collector.Trace.V1.TraceService;

namespace Winter.Backend.GrpcServices;

public class TraceService : TraceServiceBase
{
    public override async Task<ExportTraceServiceResponse> Export(ExportTraceServiceRequest request, ServerCallContext context)
    {
        Console.WriteLine(request);
        await Task.Delay(0);
        return new ExportTraceServiceResponse();
    }
}