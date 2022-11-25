using Microsoft.AspNetCore.Http;

namespace Winter.Host.Middleware;

public sealed class NotFoundRetrierOptions
{
    public Predicate<PathString> When { get; set; } = _ => true;
}

public class NotFoundRetrier
{
    private readonly RequestDelegate next;
    private readonly NotFoundRetrierOptions options;

    public NotFoundRetrier(RequestDelegate next, NotFoundRetrierOptions opts)
    {
        this.next = next;
        this.options = opts;
    }

    public async Task Invoke(HttpContext context)
    {

        bool retry = options.When(context.Request.Path);
        await next.Invoke(context);
        if (context.Response.StatusCode == 404 && retry)
        {
            context.Request.Path = "/index.html";
            await next.Invoke(context);
        }
    }
}
