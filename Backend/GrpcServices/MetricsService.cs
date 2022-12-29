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
        await hubContext.Clients.All.SendAsync("Notify", request.ToString());
        return new ExportMetricsServiceResponse();
    }
}