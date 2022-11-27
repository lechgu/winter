using System.IO;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OpenTelemetry.Proto.Collector.Metrics.V1;
using Winter.Backend.Hubs;
using Winter.Shared.Dto;

namespace Winter.Backend.Controllers;

public class MetricsController : Controller
{
    private readonly IHubContext<MetricsHub> hubContext;

    public MetricsController(IHubContext<MetricsHub> hubContext)
    {
        this.hubContext = hubContext;
    }

    [HttpGet("/test")]
    public async Task<IActionResult> Test()
    {
        var counter = new LongCounter
        {
            Scope = "scope",
            Name = "name",
            Value = 42,
            TimeStamp = DateTime.UtcNow
        };
        await hubContext.Clients.All.SendAsync("Notify", counter);
        return Ok(42);
    }

    [HttpPost("/v1/metrics")]
    [Consumes("application/x-protobuf")]
    public async Task<IActionResult> Post()
    {
        var istream = Request.BodyReader.AsStream();
        var request = ExportMetricsServiceRequest.Parser.ParseFrom(istream);
        var response = new ExportMetricsServiceResponse();
        var ostream = new MemoryStream();
        response.WriteTo(ostream);
        var sm = request.ResourceMetrics[0].ScopeMetrics[0];
        var counter = new LongCounter
        {
            Scope = sm.Scope.Name,
            Name = sm.Metrics[0].Name,
            Value = sm.Metrics[0].Gauge.DataPoints[0].AsInt,
            TimeStamp = DateTime.UtcNow
        };
        await hubContext.Clients.All.SendAsync("Notify", counter);
        return File(ostream, "application/x-protobuf");
    }
}