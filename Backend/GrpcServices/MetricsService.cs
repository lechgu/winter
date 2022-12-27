using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using OpenTelemetry.Proto.Collector.Metrics.V1;
using Winter.Backend.Hubs;
using Winter.Shared.Dto;
using static OpenTelemetry.Proto.Collector.Metrics.V1.MetricsService;

namespace Winter.Backend.GrpcServices;

public class MetricsService : MetricsServiceBase
{
    private readonly IHubContext<MetricsHub> hubContext;

    public MetricsService(IHubContext<MetricsHub> hubContext)
    {
        this.hubContext = hubContext;
    }

    public override async Task<ExportMetricsServiceResponse> Export(ExportMetricsServiceRequest request, ServerCallContext context)
    {
        Console.WriteLine(request);
        // var sm = request.ResourceMetrics[0].ScopeMetrics[0];
        // var counter = new LongCounter
        // {
        //     Scope = sm.Scope.Name,
        //     Name = sm.Metrics[0].Name,
        //     Value = sm.Metrics[0].Gauge.DataPoints[0].AsInt,
        //     TimeStamp = DateTime.UtcNow
        // };
        // await hubContext.Clients.All.SendAsync("Notify", counter);
        await Task.Delay(0);
        return new ExportMetricsServiceResponse();
    }
}