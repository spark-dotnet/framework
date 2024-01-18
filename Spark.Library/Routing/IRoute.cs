using Microsoft.AspNetCore.Builder;

namespace Spark.Library.Routing;

public interface IRoute
{
    void Map(WebApplication app);
}