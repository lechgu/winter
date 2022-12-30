using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using OpenTelemetry.Proto.Collector.Metrics.V1;
using OpenTelemetry.Proto.Metrics.V1;
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
        var counters = MetricsFromRequest(request);
        foreach (var counter in counters)
        {
            Console.WriteLine(counter.Resource);
            Console.WriteLine(counter.Scope);
            Console.WriteLine(counter.Name);
            Console.WriteLine(counter.Value);
        }
        await hubContext.Clients.All.SendAsync("Notify", counters);
        return new ExportMetricsServiceResponse();
    }

    static Counter[] MetricsFromRequest(ExportMetricsServiceRequest request)
    {
        var counters = new List<Counter>();
        foreach (var rm in request.ResourceMetrics)
        {
            foreach (var sm in rm.ScopeMetrics)
            {
                foreach (var m in sm.Metrics)
                {
                    NumberDataPoint? dp = null;

                    Console.WriteLine(m);
                    dp = m.Sum.DataPoints.FirstOrDefault();
                    if (dp is not null)
                    {
                        var counter = new Counter
                        {
                            Resource = rm.Resource.Attributes.FirstOrDefault(x => x.Key == "service.name")?.Value?.StringValue ?? "??",
                            Scope = sm.Scope.Name,
                            Name = m.Name,
                            Value = dp.AsInt,
                            Timestamp = DateTimeOffset.UtcNow
                        };
                        counters.Add(counter);
                    }
                    if (dp is not null)
                    {
                        var counter = new Counter
                        {
                            Resource = rm.Resource.Attributes.FirstOrDefault(x => x.Key == "service.name")?.Value?.StringValue ?? "??",
                            Scope = sm.Scope.Name,
                            Name = m.Name,
                            Value = dp.AsInt,
                            Timestamp = DateTimeOffset.UtcNow
                        };
                        counters.Add(counter);
                    }
                }
            }
        }
        return counters.ToArray();
    }
}