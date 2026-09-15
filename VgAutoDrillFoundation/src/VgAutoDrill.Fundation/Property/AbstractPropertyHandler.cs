using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Property;

public abstract class AbstractPropertyHandler<TDevice> : DeviceShare<TDevice>, IPropertyHandler
    where TDevice : Device
{
    private readonly ILogger _logger;
    public AbstractPropertyHandler(IServiceProvider serviceProvider, TDevice device) : base(serviceProvider, device)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<AbstractPropertyHandler<TDevice>>();
    }

    public abstract void AddWatchingProperties();
    public abstract void CollectPropertyValues();

    public virtual async Task<DeviceServiceInvokeResponse> ReadProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        ThrowHelper.ThrowArgumentNullException(deviceServiceInvokeRequest);

        _logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{DeviceDescriptor.ProductId}-{DeviceDescriptor.DeviceName}-{DeviceDescriptor.DeviceId} before ReadProperties!");
        try
        {
            CollectPropertyValues();

            var response = await ResponseSuccess();
            var values = WatchingProperties.GetValues();
            if (deviceServiceInvokeRequest.Params.Count == 0)
            {
                response.Params = values;
            }
            else
            {
                var responseDic = new Dictionary<string, object?>();
                foreach (var requestEntry in deviceServiceInvokeRequest.Params)
                {
                    if (values.ContainsKey(requestEntry.Key))
                        responseDic.Add(requestEntry.Key, values[requestEntry.Key]);
                }

                response.Params = responseDic;
            }

            _logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{DeviceDescriptor.ProductId}-{DeviceDescriptor.DeviceName}-{DeviceDescriptor.DeviceId} after ReadProperties!");
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return await ResponseFail(e.Message);
        }
    }

    public virtual async Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        ThrowHelper.ThrowArgumentNullException(deviceServiceInvokeRequest);

        _logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{DeviceDescriptor.ProductId}-{DeviceDescriptor.DeviceName}-{DeviceDescriptor.DeviceId} begin to WriteProperties!");

        try
        {
            if (deviceServiceInvokeRequest.Params != null)
            {
                WatchingProperties.SetValues(deviceServiceInvokeRequest.Params);
            }
        }
        catch (Exception e)
        {
            return await ResponseFail(e.Message);
        }

        _logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{DeviceDescriptor.ProductId}-{DeviceDescriptor.DeviceName}-{DeviceDescriptor.DeviceId} finish WriteProperties!");
        return await ResponseSuccess();
    }
}
