using VgAutoDrill.Admin.Common.Configuration;

namespace VgAutoDrill.External.WebApi.ScheduleJobs
{
    public static class ExternalSetup
    {
        public static void AddExternalPlugins(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ExternalOptions>(configuration.GetSection(ExternalOptions.Options));
        }
    }
}
