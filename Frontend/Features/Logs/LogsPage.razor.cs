using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Winter.Frontend.Services;
using Winter.Shared.Dto;

namespace Frontend.Features.Logs;

public partial class LogsPage : ComponentBase, IAsyncDisposable
{
    HubConnection? hubConnection;
    [Inject]
    SettingsProvider SettingsProvider { get; set; } = default!;

    [Inject]
    AppState AppState { get; set; } = default!;

    HxGrid<LogRecord> grid = default!;

    protected override async Task OnInitializedAsync()
    {
        if (hubConnection is null)
        {
            var settings = await SettingsProvider.GetSettingsAsync();
            var url = $"{settings.ServiceUrl}/hubs/logs";
            hubConnection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();
            hubConnection.On<LogRecord[]>("Notify", async logRecords =>
            {
                foreach (var logRecord in logRecords)
                {
                    AppState.AddLogRecord(logRecord);
                    await grid.RefreshDataAsync();
                }
            });
            await hubConnection.StartAsync();
        }
    }

    Task<GridDataProviderResult<LogRecord>> GetGridData(GridDataProviderRequest<LogRecord> request)
    {
        return Task.FromResult(new GridDataProviderResult<LogRecord>
        {
            Data = AppState.LogRecords,
            TotalCount = AppState.LogRecords.Count
        });
    }

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }
}