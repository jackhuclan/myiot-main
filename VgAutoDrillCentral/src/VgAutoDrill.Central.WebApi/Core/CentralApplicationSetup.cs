using VgAutoDrill.Central.Core;
using VgAutoDrill.OpenAPI;

namespace VgAutoDrill.Central.WebApi.Core;

public static class CentralApplicationSetup
{
    public static void AddCentral(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCentralCore(configuration);

        services.AddSingleton<MqttHandler>();
        services.AddSingleton<IDeviceServiceInvoker, MqttServerDeviceServiceInvoker>();
        services.AddSingleton<IHttpRequestInvoker, HttpRequestInvoker>();
    }
}
