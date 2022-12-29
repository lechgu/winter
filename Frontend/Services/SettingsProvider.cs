using System.Net.Http;
using System.Text.Json;
using Winter.Frontend.Dto;

namespace Winter.Frontend.Services;


public class SettingsProvider
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    private readonly HttpClient httpClient;
    private Settings? cachedSettings;

    public SettingsProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Settings> GetSettingsAsync()
    {
        if (cachedSettings is null)
        {
            var response = await httpClient.GetAsync("settings.json");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                cachedSettings = JsonSerializer.Deserialize<Settings>(json, jsonSerializerOptions)!;
            }
            else
            {
                var baseUrl = httpClient.BaseAddress!.ToString();
                if (baseUrl.EndsWith('/'))
                {
                    baseUrl = baseUrl[..^1];
                }
                cachedSettings = new Settings
                {
                    ServiceUrl = baseUrl
                };
            }
        }
        return cachedSettings;
    }
}