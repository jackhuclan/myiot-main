using VgAutoDrill.External.WebApi.ScheduleJobs;

namespace VgAutoDrill.External.WebApi.Configurations
{
    public static class ScheduleJobConfig
    {
        public static void AddScheduleConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<ExternalOrderWorkerJob>();
            services.AddHostedService(sp => sp.GetRequiredService<ExternalOrderWorkerJob>());
        }
    }
}
