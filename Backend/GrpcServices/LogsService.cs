using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using OpenTelemetry.Proto.Collector.Logs.V1;
using Winter.Backend.Hubs;
using Winter.Shared.Dto;
using static OpenTelemetry.Proto.Collector.Logs.V1.LogsService;

namespace Winter.Backend.GrpcServices;

public class LogsService : LogsServiceBase
{
    private readonly IHubContext<LogsHub> hubContext;

    public LogsService(IHubContext<LogsHub> hubContext)
    {
        this.hubContext = hubContext;
    }

    public override async Task<ExportLogsServiceResponse> Export(ExportLogsServiceRequest request, ServerCallContext context)
    {
        var records = LogRecordsFromRequest(request);
        await hubContext.Clients.All.SendAsync("Notify", records);
        return new ExportLogsServiceResponse();
    }


    static LogRecord[] LogRecordsFromRequest(ExportLogsServiceRequest request)
    {
        List<LogRecord> logRecords = new();
        foreach (var rl in request.ResourceLogs)
        {
            foreach (var sl in rl.ScopeLogs)
            {
                var lr = sl.LogRecords.FirstOrDefault();
                if (lr is not null)
                {
                    var logRecord = new LogRecord
                    {
                        Resource = rl.Resource.Attributes.FirstOrDefault(x => x.Key == "service.name")?.Value?.StringValue ?? "??",
                        Scope = sl.Scope?.Name ?? "??",
                        Severity = lr.SeverityText,
                        Value = lr.Body.StringValue,
                        Timestamp = DateTimeOffset.UtcNow
                    };
                    logRecords.Add(logRecord);
                }
            }
        }
        return logRecords.ToArray();
    }
}