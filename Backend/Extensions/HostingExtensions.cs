using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Winter.Backend.Hubs;
using Winter.Backend.Middleware;

namespace Winter.Backend.Extensions;

public static class HostingExtensions
{
    public static void ConfigureHosting(this ConfigureWebHostBuilder builder, IConfiguration config)
    {
        var (http1Port, http2Port) = GetPorts(config);
        builder.ConfigureKestrel(opts =>
        {
            opts
            .ListenAnyIP(http1Port);
            opts.ListenAnyIP(http2Port, lo =>
            {
                lo.Protocols = HttpProtocols.Http2;
            });
        });
    }

    public static void ConfigureDependencies(this IServiceCollection services, IConfiguration _)
    {
        services.AddCors();
        services.AddControllers();
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
        app.UseCors(x =>
        {
            x.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin();
        });
        var (http1Port, http2Port) = GetPorts(config);
        app.MapControllers().RequireHost($"*:{http1Port}");
        app.MapHub<MetricsHub>("hubs/metrics");
    }

    public static (int, int) GetPorts(IConfiguration config)
    {
        if (!int.TryParse(config["HTTP1_PORT"], out int http1Port))
        {
            http1Port = 80;
        }
        if (!int.TryParse(config["HTTP2_PORT"], out int http2Port))
        {
            http2Port = 90;
        }
        return (http1Port, http2Port);
    }
}
