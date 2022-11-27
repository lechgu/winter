using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Winter.Backend.Hubs;

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

    public static void ConfigurePipeline(this WebApplication app, IConfiguration _)
    {
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
