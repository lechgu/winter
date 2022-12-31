using System.Net.Http;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Winter.Frontend.Services;
using Winter.Shared.Dto;

namespace Frontend.Features.Metrics;



public partial class MetricsPage : ComponentBase
{
    [Inject]
    SettingsProvider SettingsProvider { get; set; } = default!;

    [Inject]
    AppState AppState { get; set; } = default!;

    HxGrid<Counter> grid = default!;

    protected override async Task OnInitializedAsync()
    {
        var settings = await SettingsProvider.GetSettingsAsync();
        var url = $"{settings.ServiceUrl}/hubs/metrics";
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();
        hubConnection.On<Counter[]>("Notify", async counters =>
        {
            foreach (var counter in counters)
            {
                AppState.UpsertCounter(counter);
            }
            await grid.RefreshDataAsync();
        });
        await hubConnection.StartAsync();
    }

    Task<GridDataProviderResult<Counter>> GetGridData(GridDataProviderRequest<Counter> request)
    {
        return Task.FromResult(new GridDataProviderResult<Counter>
        {
            Data = AppState.Counters,
            TotalCount = AppState.Counters.Count
        });
    }

}
