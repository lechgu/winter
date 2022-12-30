using System.Globalization;
using System.Net.Http;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Winter.Frontend.Services;
using Winter.Shared.Dto;

namespace Frontend.Features.Metrics;

public record CounterDescription(string Resource, string Scope, string Name);

public partial class MetricsPage : ComponentBase
{
    [Inject]
    HttpClient HttpClient { get; set; } = default!;

    [Inject]
    SettingsProvider SettingsProvider { get; set; } = default!;

    HxGrid<Counter> grid = default!;
    Dictionary<CounterDescription, Counter> counterCache = new();

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
                var desc = new CounterDescription(counter.Resource, counter.Scope, counter.Name);
                counterCache[desc] = counter;
            }
            await grid.RefreshDataAsync();
        });
        await hubConnection.StartAsync();
    }

    Task<GridDataProviderResult<Counter>> GetGridData(GridDataProviderRequest<Counter> request)
    {
        return Task.FromResult(new GridDataProviderResult<Counter>
        {
            Data = counterCache.Values,
            TotalCount = counterCache.Count
        });
    }

}
