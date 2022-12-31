using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Winter.Frontend.Services;
using Winter.Shared.Dto;

namespace Frontend.Features.Metrics;



public partial class MetricsPage : ComponentBase
{
    [Inject]
    AppState AppState { get; set; } = default!;

    [Inject]
    MegaHub MegaHub { get; set; } = default!;

    HxGrid<Counter> grid = default!;

    protected override async Task OnInitializedAsync()
    {
        MegaHub.MetricsChanged += async () =>
        {
            await grid.RefreshDataAsync();
        };
        await MegaHub.ConnectAsync();
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
