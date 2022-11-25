using System.Threading.Tasks;
using Grpc.Core;
using OpenTelemetry.Proto.Collector.Logs.V1;
using static OpenTelemetry.Proto.Collector.Logs.V1.LogsService;

namespace Winter.Backend.GrpcServices;

public class LogsService : LogsServiceBase
{
    public override Task<ExportLogsServiceResponse> Export(ExportLogsServiceRequest request, ServerCallContext context)
    {
        Console.WriteLine(request.ToString());
        return base.Export(request, context);
    }
}