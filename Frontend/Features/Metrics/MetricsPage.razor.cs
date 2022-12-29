using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using OpenTelemetry.Proto.Metrics.V1;
using Winter.Frontend.Services;
using Winter.Shared.Dto;

namespace Frontend.Features.Metrics;

public partial class MetricsPage : ComponentBase
{
    [Inject]
    HttpClient HttpClient { get; set; } = default!;

    [Inject]
    SettingsProvider SettingsProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var settings = await SettingsProvider.GetSettingsAsync();
        var url = $"{settings.ServiceUrl}/hubs/metrics";
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();
        hubConnection.On<string>("Notify", metrics =>
        {
            Console.WriteLine(metrics);
            StateHasChanged();
        });
        await hubConnection.StartAsync();
    }
}
