using dotenv.net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Winter.Backend.Extensions;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
config.AddEnvironmentVariables();
builder.WebHost.ConfigureHosting(config);
builder.Services.ConfigureDependencies(config);
var app = builder.Build();
app.ConfigurePipeline(config);
app.Run();