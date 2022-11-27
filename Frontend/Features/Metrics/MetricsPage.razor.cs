using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Winter.Shared.Dto;

namespace Frontend.Features.Metrics;

public partial class MetricsPage : ComponentBase
{
    LongCounter? counter;

    protected override async Task OnInitializedAsync()
    {
        var url = "http://localhost:8080/hubs/metrics";
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();
        hubConnection.On<LongCounter>("Notify", counter => this.counter = counter);
        await hubConnection.StartAsync();
    }
}