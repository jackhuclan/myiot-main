using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceServiceInvocation;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core;

internal class DeviceServiceInvocationReplayer : BackgroundService
{
    private readonly ILogger<DeviceServiceInvocationReplayer> _logger;
    private readonly DeviceServiceInvocationReplayerOptions _options;
    private readonly PeriodicTimer _timer;
    private readonly IDeviceManager _deviceHolder;
    private readonly IDistributedCache _distributedCache;
    private readonly IDeviceServiceInvocationService _deviceServiceInvocationService;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private volatile bool _isBusy = false;

    public DeviceServiceInvocationReplayer(IOptions<DeviceServiceInvocationReplayerOptions> options,
        IDeviceServiceInvoker deviceServiceInvoker,
        IDeviceManager deviceHolder,
        IDistributedCache distributedCache,
        IDeviceServiceInvocationService deviceServiceInvocationService,
        ISysConfigManager sysConfigManager,
        ILoggerFactory loggerFactory)
    {
        _options = options.Value;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_options.PrefetchInterval));
        _logger = loggerFactory.CreateLogger<DeviceServiceInvocationReplayer>();
        _deviceHolder = deviceHolder;
        _distributedCache = distributedCache;
        _deviceServiceInvocationService = deviceServiceInvocationService;
        _sysConfigManager = sysConfigManager;
    }

    protected override async System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (_timer)
        {
            while (!stoppingToken.IsCancellationRequested
                && !_isBusy
                && await _timer.WaitForNextTickAsync())
            {
                //if (await _sysConfigManager.GetCentralControlSystemIsMaintaining())
                //{
                //    _logger.LogWarning($"系统即将停机维护，暂停后台任务执行");
                //    continue;
                //}

                try
                {
                    _autoResetEvent.WaitOne();
                    _isBusy = true;
                    _logger.LogDebug($"begin to do DeviceServiceInvocationReplayer ExecuteAsync");

                    //表中所有的数据，
                    //已处理成功的，标识成功，7天后删除；
                    //处理超时的，标识超时，7天后删除；
                    //从数据库获取_options.PrefetchCount条记录的DeviceServiceInvocationArgs，
                    //order by MqttQualityOfServiceLevel desc; loop each DeviceServiceInvocationArgs
                    var originalRequests = await _deviceServiceInvocationService.GetDeviceServiceInvocationList(new GetDeviceServiceInvocationListReq
                    {
                        IsDealed = false,
                        IsTimeout = false,
                    });

                    foreach (var originalDb in originalRequests)
                    {
                        //释放CPU 若干时间，避免服务器CPU 过高运行，
                        await Task.Delay(300);

                        var updateDbRequest = new AddOrUpdateDeviceServiceInvocationReq
                        {
                            Id = originalDb.Id,
                        };
                        if (!originalDb.Retries.HasValue || originalDb.Retries == 0)
                        {
                            updateDbRequest.FirstInvocationTimestamp = DateTime.Now;
                        }
                        // 如果超过调用次数，直接取消等待的调用;
                        if (originalDb.Retries < _options.MaxRetries)
                        {
                            DeviceServiceInvokeRequest convertedServiceRequest = null;
                            try
                            {
                                convertedServiceRequest = JsonSerializer.Deserialize<DeviceServiceInvokeRequest>(originalDb.Payload);
                            }
                            catch (Exception)
                            {
                                _logger.LogError($"Fail to convert DeviceServiceInvokeRequest,payload:{originalDb.Payload}");
                            }

                            if (convertedServiceRequest != null)
                            {
                                var deviceProxy = _deviceHolder.GetOnlineDevice(convertedServiceRequest.DeviceId);
                                //设备不在线时，视为已尝试了一次；
                                if (deviceProxy != null)
                                {
                                    if (string.IsNullOrEmpty(convertedServiceRequest.ServiceId))
                                    {
                                        convertedServiceRequest.ServiceId = Topics.Services.CANCEL_SCHEDULE_SERVICE_ID;
                                        convertedServiceRequest.ScheduledStatus = Fundation.Iot.Schedule.ScheduledTaskStatus.Canceled;
                                    }
                                    convertedServiceRequest.TargetDeviceId = convertedServiceRequest.DeviceId;
                                    convertedServiceRequest.TargetProductId = convertedServiceRequest.ProductId;
                                    convertedServiceRequest.TargetClientId = deviceProxy.ClientId;
                                    var response = await deviceProxy.InvokeService(convertedServiceRequest);
                                    if (response != null && response.Code == ErrorCodes.Sys.SUCCESS)
                                    {
                                        if (!string.IsNullOrEmpty(originalDb.RoutingKey))
                                        {
                                            await _distributedCache.RemoveAsync(originalDb.RoutingKey);
                                        }
                                        updateDbRequest.IsDealed = true;
                                    }
                                }
                            }

                            updateDbRequest.Retries = originalDb.Retries + 1;
                        }
                        else
                        {
                            //标记为已超时，下次不会再处理了
                            updateDbRequest.IsTimeout = true;
                        }

                        updateDbRequest.LastInvocationTimestamp = DateTime.Now;
                        await _deviceServiceInvocationService.AddDeviceServiceInvocationList(new List<AddOrUpdateDeviceServiceInvocationReq> { updateDbRequest });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
                finally
                {
                    _isBusy = false;
                    _autoResetEvent.Set();
                    _logger.LogDebug($"End to do DeviceServiceInvocationReplayer ExecuteAsync");
                }
            }
        }
    }
}
