using VgAutoDrill.Admin.Common.Configuration;

namespace VgAutoDrill.Admin.WebApi.ScheduleJobs
{
    public static class AdminSetup
    {
        public static void AddAdminPlugins(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<InnerOptions>(configuration.GetSection(InnerOptions.Options));
        }
    }
}
