namespace Spark.Templates.Razor.Application.Middleware
{
    public class PageNotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        public PageNotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration config)
        {
            await _next(context);

            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                string originalPath = context.Request.Path.Value;
                context.Items["originalPath"] = originalPath;
                context.Request.Path = config.GetValue<string>("Spark:PageNotFoundPath", "/page-not-found");
                await _next(context);
            }
        }
    }

    public static class PageNotFoundMiddlewareExtensions
    {
        public static IApplicationBuilder UsePageNotFoundMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PageNotFoundMiddleware>();
        }
    }
}
