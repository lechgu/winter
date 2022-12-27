using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Winter.Backend.GrpcServices;
using Winter.Backend.Hubs;
using Winter.Backend.Middleware;

namespace Winter.Backend.Extensions;

public static class HostingExtensions
{
    const int DEFAULT_OTLP_PORT = 4317;
    const int DEFAULT_FRONTEND_PORT = 80;
    public static void ConfigureHosting(this ConfigureWebHostBuilder builder, IConfiguration config)
    {
        var (otlpPort, frontendPort) = GetPorts(config);
        builder.ConfigureKestrel(opts =>
        {
            opts
            .ListenAnyIP(frontendPort);
            opts.ListenAnyIP(otlpPort, lo =>
            {
                lo.Protocols = HttpProtocols.Http2;
            });
        });
    }

    public static void ConfigureDependencies(this IServiceCollection services, IConfiguration _)
    {
        services.AddGrpc();
        services.AddSignalR();
    }

    public static void ConfigurePipeline(this WebApplication app, IConfiguration config)
    {
        app.UseNotFoundRetrier(new NotFoundRetrierOptions
        {
            When = (path) => !path.StartsWithSegments("/settings.json")
        });
        var staticDir = config["STATIC_DIR"];
        if (!string.IsNullOrEmpty(staticDir) && Directory.Exists(staticDir))
        {
            var fileProvider = new PhysicalFileProvider(staticDir);
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = fileProvider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                FileProvider = fileProvider
            });
        }
        app.UseRouting();
        app.MapGrpcService<MetricsService>();
        app.MapGrpcService<LogsService>();
        app.MapGrpcService<TraceService>();
        app.MapHub<MetricsHub>("hubs/metrics");
    }

    static (int, int) GetPorts(IConfiguration config)
    {
        if (!int.TryParse(config["OTLP_PORT"], out int otlpPort))
        {
            otlpPort = DEFAULT_OTLP_PORT;
        }
        if (!int.TryParse(config["FRONTEND_PORT"], out int frontendPort))
        {
            frontendPort = DEFAULT_FRONTEND_PORT;
        }
        return (otlpPort, frontendPort);
    }
}
