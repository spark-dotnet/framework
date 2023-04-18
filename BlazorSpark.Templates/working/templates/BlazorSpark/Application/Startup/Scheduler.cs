using BlazorSpark.Default.Application.Tasks;
using Coravel;

namespace BlazorSpark.Default.Application.Startup
{
    public static class Scheduler
    {
        public static IServiceProvider RegisterScheduledJobs(this IServiceProvider services)
        {
            services.UseScheduler(scheduler =>
            {
                scheduler
                    .Schedule<ExampleTask>()
                    .EveryFiveMinutes();
            });
            return services;
        }
    }
}
