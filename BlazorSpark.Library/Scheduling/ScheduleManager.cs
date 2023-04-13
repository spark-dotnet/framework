using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coravel.Scheduling;
using Coravel.Events.Interfaces;
using Coravel.Scheduling.HostedService;
using Coravel.Scheduling.Schedule;
using Coravel.Scheduling.Schedule.Interfaces;
using Coravel.Scheduling.Schedule.Mutex;
using Microsoft.Extensions.DependencyInjection;
using Coravel;

namespace BlazorSpark.Library.Scheduling
{
    public static class ScheduleManager
    {
        public static IServiceCollection Setup(this IServiceCollection services)
        {
            services.AddScheduler();
            return services;
        }

    }
}
