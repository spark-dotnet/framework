using Spark.Templates.Razor.Application.Jobs;
using Coravel;

namespace Spark.Templates.Razor.Application.Startup
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
