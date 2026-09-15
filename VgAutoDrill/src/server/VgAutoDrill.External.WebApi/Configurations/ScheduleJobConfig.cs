
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.WebApi.ScheduleJobs;

namespace VgAutoDrill.Admin.WebApi.Configurations
{
    public static class ScheduleJobConfig
    {
        public static void AddScheduleConfiguration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IAPIHelper, APIHelper>();
            //services.AddSingleton<ExternalOrderWorkerJob>();
            services.AddHostedService<ExternalOrderWorkerJob>();
        }
    }
}
