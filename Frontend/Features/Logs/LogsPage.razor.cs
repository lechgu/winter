using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Winter.Frontend.Services;
using Winter.Shared.Dto;

namespace Frontend.Features.Logs;

public partial class LogsPage : ComponentBase
{
    [Inject]
    SettingsProvider SettingsProvider { get; set; } = default!;

    HxGrid<LogRecord> grid = default!;
    readonly List<LogRecord> logRecordsCache = new();

    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine("OnInitializedAsync");
        var settings = await SettingsProvider.GetSettingsAsync();
        var url = $"{settings.ServiceUrl}/hubs/logs";
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();
        hubConnection.On<LogRecord[]>("Notify", async logRecords =>
        {
            foreach (var logRecord in logRecords)
            {
                logRecordsCache.Add(logRecord);
                await grid.RefreshDataAsync();
            }
        });
        await hubConnection.StartAsync();
    }

    Task<GridDataProviderResult<LogRecord>> GetGridData(GridDataProviderRequest<LogRecord> request)
    {
        return Task.FromResult(new GridDataProviderResult<LogRecord>
        {
            Data = logRecordsCache,
            TotalCount = logRecordsCache.Count
        });
    }
}