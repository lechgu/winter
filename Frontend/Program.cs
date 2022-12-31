using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Havit.Blazor.Components.Web;
using Winter.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHxServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<SettingsProvider>();
builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<MegaHub>();

await builder.Build().RunAsync();
