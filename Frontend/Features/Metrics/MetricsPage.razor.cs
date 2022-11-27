using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Winter.Shared.Dto;

namespace Frontend.Features.Metrics;

public partial class MetricsPage : ComponentBase
{
    [Inject]
    HttpClient HttpClient { get; set; } = default!;

    LongCounter? counter;

    protected override async Task OnInitializedAsync()
    {
        var url = $"{HttpClient.BaseAddress}hubs/metrics";
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();
        hubConnection.On<LongCounter>("Notify", counter =>
        {
            this.counter = counter;
            StateHasChanged();
        });
        await hubConnection.StartAsync();
    }
}