using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Winter.Frontend.Services;
using Winter.Shared.Dto;

namespace Frontend.Features.Logs;

public partial class LogsPage : ComponentBase
{
    [Inject]
    AppState AppState { get; set; } = default!;

    [Inject]
    MegaHub MegaHub { get; set; } = default!;

    HxGrid<LogRecord> grid = default!;

    protected override async Task OnInitializedAsync()
    {
        MegaHub.LogsChanged += async () =>
        {
            await grid.RefreshDataAsync();
        };
        await MegaHub.ConnectAsync();
    }

    Task<GridDataProviderResult<LogRecord>> GetGridData(GridDataProviderRequest<LogRecord> request)
    {
        return Task.FromResult(new GridDataProviderResult<LogRecord>
        {
            Data = AppState.LogRecords,
            TotalCount = AppState.LogRecords.Count
        });
    }
}