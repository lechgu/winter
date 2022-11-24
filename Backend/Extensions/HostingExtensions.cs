using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            opts.ListenAnyIP(port, lo =>
            {
                lo.Protocols = HttpProtocols.Http2;
            });
        });
    }

    public static void ConfigureDependencies(this IServiceCollection services, IConfiguration config)
    {

    }

    public static void ConfigurePipeline(this WebApplication app, IConfiguration config)
    {
    }
}
