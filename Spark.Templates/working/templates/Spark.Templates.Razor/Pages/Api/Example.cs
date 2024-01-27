using Spark.Library.Routing;

namespace Spark.Templates.Razor.Pages.Api;

public class Example : IRoute
{
    public void Map(WebApplication app)
    {
        app.MapGet("/api/example", () =>
        {
            return Results.Ok(new { Hello = "World" });
        });
    }
}