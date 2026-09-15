using Autofac;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection;

namespace VgAutoDrill.Admin.WebApi.Configurations
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;

            var servicesDllFile = Path.Combine(basePath, "VgAutoDrill.Admin.Application.dll");
            var domainDllFile = Path.Combine(basePath, "VgAutoDrill.Admin.Domain.dll");
            var repositoryDllFile = Path.Combine(basePath, "VgAutoDrill.Admin.Repository.dll");
            var servicesOfCollectDllFile = Path.Combine(basePath, "VgAutoDrill.DataCollect.Application.dll");

            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            var assemblysDomain = Assembly.LoadFrom(domainDllFile);
            builder.RegisterAssemblyTypes(assemblysDomain).AsImplementedInterfaces().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            //var assemblysOfCollectDll = Assembly.LoadFrom(servicesOfCollectDllFile);
            //builder.RegisterAssemblyTypes(assemblysOfCollectDll).AsImplementedInterfaces().InstancePerLifetimeScope()
            //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }
    }
}
