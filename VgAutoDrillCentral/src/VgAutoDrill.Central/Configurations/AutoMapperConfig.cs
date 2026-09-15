using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.AutoMapper;

namespace VgAutoDrill.Central.Configurations;

public static class AutoMapperConfig
{
    public static void AddAutoMapperConfiguration(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddAutoMapper(cfg => cfg.AddIgnoreMapAttribute(),
            typeof(Admin.Model.AutoMapper.DomainToViewModelMappingProfile).Assembly,
            typeof(Admin.Model.AutoMapper.ThirdPartyToViewModelMappingProfile).Assembly,
            typeof(Core.AutoMapper.DomainToViewModelMappingProfile).Assembly,
            typeof(Core.AutoMapper.ThirdPartyToViewModelMappingProfile).Assembly);
    }
}
