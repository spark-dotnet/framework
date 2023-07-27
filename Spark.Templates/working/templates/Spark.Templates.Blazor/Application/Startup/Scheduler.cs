using Spark.Templates.Blazor.Application.Jobs;
using Coravel;

namespace Spark.Templates.Blazor.Application.Startup
{
    public static class Scheduler
    {
        public static IServiceProvider RegisterScheduledJobs(this IServiceProvider services)
        {
            services.UseScheduler(scheduler =>
            {
                // example scheduled job
                //scheduler
                //    .Schedule<ExampleJob>()
                //    .EveryFiveMinutes();
            });
            return services;
        }
    }
}
