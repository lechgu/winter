using Microsoft.AspNetCore.Builder;
using Winter.Backend.Middleware;

namespace Winter.Backend.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseNotFoundRetrier(this IApplicationBuilder builder, NotFoundRetrierOptions? options = null)
    {
        return builder.UseMiddleware<NotFoundRetrier>(options ?? new NotFoundRetrierOptions());
    }

}