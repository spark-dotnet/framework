using Microsoft.AspNetCore.Builder;

namespace Spark.Library.Routing;

public static class RouteRegistration
{
    public static void MapMinimalApis<T>(this WebApplication app)
    {
        var routes = typeof(T).Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IRoute))
                        && !t.IsAbstract && !t.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IRoute>();

        foreach (var route in routes)
        {
            route.Map(app);
        }
    }
}