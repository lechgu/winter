using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        if (!int.TryParse(config["PORT"], out int port))
        {
            port = 80;
        }

        builder.ConfigureKestrel(opts =>
        {
            opts.ListenAnyIP(port);
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
        app.MapControllers();
        app.MapHub<MetricsHub>("hubs/metrics");
    }
}
