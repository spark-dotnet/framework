using Spark.Templates.Api.Application.Tasks;
using Coravel;

namespace Spark.Templates.Api.Application.Startup
{
    public static class Scheduler
    {
        public static IServiceProvider RegisterScheduledJobs(this IServiceProvider services)
        {
            services.UseScheduler(scheduler =>
            {
                // example scheduled job
                //scheduler
                //    .Schedule<ExampleTask>()
                //    .EveryFiveMinutes();
            });
            return services;
        }
    }
}
