using Microsoft.AspNetCore.SignalR.Client;
using Winter.Shared.Dto;

namespace Winter.Frontend.Services;

public class MegaHub
{
    readonly AppState appState;
    private readonly SettingsProvider settingsProvider;
    bool connected = false;
    HubConnection? logsHub;
    public event Action? LogsChanged;

    public MegaHub(AppState appState, SettingsProvider settingsProvider)
    {
        this.appState = appState;
        this.settingsProvider = settingsProvider;
    }

    public async Task ConnectAsync()
    {
        if (!connected)
        {
            var settings = await settingsProvider.GetSettingsAsync();
            var logsUrl = $"{settings.ServiceUrl}/hubs/logs";
            logsHub = new HubConnectionBuilder()
                .WithUrl(logsUrl)
                .Build();
            logsHub.On<LogRecord[]>("Notify", logRecords =>
            {
                foreach (var logRecord in logRecords)
                {
                    appState.AddLogRecord(logRecord);
                }
                LogsChanged?.Invoke();
            });

            await logsHub.StartAsync();
            connected = true;
        }
    }
}