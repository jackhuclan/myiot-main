using VgAutoDrill.Admin.WebApi.ScheduleJobs;

namespace VgAutoDrill.Admin.WebApi.Configurations
{
    public static class ScheduleJobConfig
    {
        public static void AddScheduleConfiguration(this IServiceCollection services)
        {
            services.AddHostedService<AdminGeneralJob>();
            services.AddHostedService<AdminAutoGenerateCutterGroupDataJob>();
            services.AddHostedService<RegularDeleteDataJob>();
        }
    }
}
