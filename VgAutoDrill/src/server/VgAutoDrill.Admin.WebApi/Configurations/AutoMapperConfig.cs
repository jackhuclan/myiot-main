using VgAutoDrill.Admin.Model.AutoMapper;

namespace VgAutoDrill.Admin.WebApi.Configurations
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ThirdPartyToViewModelMappingProfile));
        }
    }
}
