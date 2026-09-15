using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Alarm;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Fundation.Event;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Property;
using VgAutoDrill.Fundation.State;
using VgAutoDrill.Fundation.Store;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

namespace VgAutoDrill.Fundation.Iot;

public abstract class Device : IDisposable
{
    public const int DEFAULT_MAX_TASKS_LIMIT = 1000;
    public const string GLOBAL_EXCEPTION_EVENT_ID = "ExceptionEventId";
    public const string GLOBAL_EXCEPTION_EVENT_NAME = "ExceptionEventName";
    public const string GLOBAL_EXCEPTION_EVENT_MESSAGE = "ExceptionEventMessage";

    public event Func<DeviceStatusChangedEventArgs, Task> OnStatusChanged;

    /// <summary>
    /// Mqtt是否连接
    /// </summary>
    public bool MqttConnected => MqttClientWrapper.IsConnected;

    /// <summary>
    /// 产品id，设备所归属的产品
    /// </summary>
    public string ProductId { get; private set; } = string.Empty;

    /// <summary>
    /// 设备id，所有设备id必须唯一
    /// </summary>
    public string DeviceId { get; private set; } = string.Empty;

    /// <summary>
    /// 设备与mqtt连接上后得到clientid
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    public string DeviceName { get; private set; } = string.Empty;

    public DeviceStatus Status
    {
        get => _status;
        set
        {
            DeviceStatus oldStatus = _status;
            bool statusChanged = _status != value;
            _status = value;

            if (statusChanged)
            {
                OnStatusChanged?.Invoke(new DeviceStatusChangedEventArgs
                {
                    DeviceId = DeviceId,
                    DeviceName = DeviceName,
                    OldStatus = oldStatus,
                    NewStatus = _status,
                });
            }
        }
    }

    /// <summary>
    /// 定义收集数据的方式
    /// </summary>
    public Action<Device> CollectDataFunc { get; set; } = (device) => { };

    /// <summary>
    /// 定义要监控的属性
    /// </summary>
    public WatchableProperties WatchingProperties { get; private set; }

    /// <summary>
    /// 设备收到的中控下发的调度任务
    /// </summary>
    public PriorityQueue<DeviceServiceInvokeRequest, int> SchedulingTasks { get; private set; }

    /// <summary>
    /// 设备驱动引擎<see cref="ManualEngine"/> <seealso cref="AutomaticEngine"/>
    /// </summary>
    public IDeviceEngine Engine { get; }

    /// <summary>
    /// 设备与底层硬件的连接器
    /// </summary>
    public IDeviceConnector Connector { get; }

    /// <summary>
    /// 设备描述符
    /// </summary>
    public DeviceDescriptor DeviceDescriptor { get; private set; }

    /// <summary>
    /// 设备上装载的板料信息
    /// </summary>
    public PanelList PayloadPanels { get => _payloadPanels; set => _payloadPanels = value; }

    /// <summary>
    /// 设备上装载的刀盘信息
    /// </summary>
    public CutterTrays PayloadCutterTrays { get => _payloadCutterTrays; set => _payloadCutterTrays = value; }

    /// <summary>
    /// 全局服务
    /// </summary>
    public IServiceProvider ApplicationServices { get; }

    public IHttpRequestInvoker HttpRequestInvoker { get; }
    public IMqttClientWrapper MqttClientWrapper { get; }
    public ILoggerFactory LoggerFactory { get; }
    public CentralWebOptions CentralWebOptions { get; }
    public IDeviceProvider DeviceProvider { get; }
    public IMessageChannel DataExporter { get; }
    public IEventHandlerContainer EventContainer { get; }
    public IStateHandlerContainer StateContainer { get; }
    public IPropertyHandlerContainer PropertyContainer { get; }
    public IAlarmHandlerContainer AlarmContainer { get; }
    public IScheduleHandlerContainer ScheduleContainer { get; }
    public IObjectFactory ObjectFactory { get; }
    public IPeriodicTimerExecutorFactory PeriodicTimers { get; }
    public IDrillFilePathLocator DrillFilePathLocator { get; }
    public IDeviceStore DeviceStore { get; }

    /// <summary>
    /// 远程控制指令集合
    /// </summary>
    public CommandSet Commands { get; } = new();

    private readonly ILogger<Device> _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private volatile PanelList _payloadPanels = new();
    private volatile CutterTrays _payloadCutterTrays = new();
    private volatile DeviceStatus _status;

    protected Device(DeviceDescriptor deviceDescriptor,
        IDeviceEngine deviceEngine,
        IServiceProvider serviceProvider)
    {
        Engine = deviceEngine;
        Connector = Engine.DeviceConnector;
        DeviceDescriptor = deviceDescriptor;
        ProductId = deviceDescriptor.ProductId;
        DeviceId = deviceDescriptor.DeviceId;
        DeviceName = deviceDescriptor.DeviceName;
        WatchingProperties = new WatchableProperties();
        SchedulingTasks = new PriorityQueue<DeviceServiceInvokeRequest, int> { };

        ApplicationServices = serviceProvider;
        MqttClientWrapper = serviceProvider.GetRequiredService<IMqttClientWrapper>();
        HttpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        LoggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        CentralWebOptions = serviceProvider.GetRequiredService<IOptions<CentralWebOptions>>().Value;
        DeviceProvider = serviceProvider.GetRequiredService<IDeviceProvider>();
        DataExporter = serviceProvider.GetRequiredService<IMessageChannelFactory>().Create(deviceDescriptor, CentralWebOptions.Channel);
        EventContainer = serviceProvider.GetRequiredService<IEventHandlerContainer>();
        StateContainer = serviceProvider.GetRequiredService<IStateHandlerContainer>();
        PropertyContainer = serviceProvider.GetRequiredService<IPropertyHandlerContainer>();
        AlarmContainer = serviceProvider.GetRequiredService<IAlarmHandlerContainer>();
        ScheduleContainer = serviceProvider.GetRequiredService<IScheduleHandlerContainer>();
        PeriodicTimers = serviceProvider.GetRequiredService<IPeriodicTimerExecutorFactory>();
        ObjectFactory = serviceProvider.GetRequiredService<IObjectFactory>();
        DrillFilePathLocator = serviceProvider.GetRequiredService<IDrillFilePathLocator>();
        _applicationLifetime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();
        DeviceStore = serviceProvider.GetRequiredService<IDeviceStore>();
        _logger = LoggerFactory.CreateLogger<Device>();

        _logger.LogInformation($"FundationVersion={deviceDescriptor.FundationVersion}");
        _logger.LogInformation($"AgentVersion={deviceDescriptor.AgentVersion}");

        ConfigureWatchingExceptionEvent();
        ConfigureExternalDataReport();
        ConfigureServiceCapabilities();

        if (CentralWebOptions.Channel.ToLowerInvariant() == "http")
        {
            PeriodicTimers["10s"]!.OnTick += LoadDeviceDescriptorRemotely;
        }

        _applicationLifetime.ApplicationStarted.Register(async () => await OnApplicationStarted());
        _applicationLifetime.ApplicationStopping.Register(async () => await OnApplicationStopping());

        PayloadPanels.CollectionChanged += OnPanelsChanged;
        PayloadCutterTrays.CollectionChanged += OnCutterTraysChanged;
        OnStatusChanged += OnDeviceStatusChanged;
    }

    /// <summary>
    /// 设备状态发生改变时候自动触发
    /// </summary>
    /// <param name="arg">设备状态参数</param>
    /// <returns></returns>
    protected virtual Task OnDeviceStatusChanged(DeviceStatusChangedEventArgs arg)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 程序启动时候，启动引擎
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task FireEngine(CancellationToken cancellationToken)
    {
        await Initialize();
        await Engine.Fire(this, cancellationToken);
    }

    /// <summary>
    /// 中控远程通用控制指令
    /// </summary>
    /// <param name="deviceServiceInvokeRequest"></param>
    /// <returns></returns>
    public virtual async Task<DeviceServiceInvokeResponse> RemoteCommand(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        if (deviceServiceInvokeRequest != null)
        {
            var commandStr = deviceServiceInvokeRequest.ServiceName;
            if (string.IsNullOrWhiteSpace(commandStr)) { commandStr = deviceServiceInvokeRequest.ServiceId; }
            if (!string.IsNullOrWhiteSpace(commandStr)
                && Commands.ContainsKey(commandStr))
            {
                return await Commands[commandStr].Invoke(deviceServiceInvokeRequest);
            }
        }

        return await ResponseFail();
    }

    /// <summary>
    /// 中控通知设备调度任务完成
    /// </summary>
    /// <param name="deviceServiceInvokeRequest"></param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> CompleteSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    /// <summary>
    /// 中控通知设备调度任务被取消
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> CancelSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    /// <summary>
    /// 中控下发给设备一个新的调度任务
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    /// <summary>
    /// 设备开始工作
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> Work(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    /// <summary>
    /// 设备开始等待模式
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> Standby(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    /// <summary>
    /// 设备关闭
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> Shutdown(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    /// <summary>
    /// 设备进入维护状态
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> Maintain(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    /// <summary>
    /// 设备读取属性
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual async Task<DeviceServiceInvokeResponse> ReadProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await PropertyContainer[GetType()].ReadProperties(deviceServiceInvokeRequest);
    }

    /// <summary>
    /// 往设备写属性
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual async Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await PropertyContainer[GetType()].WriteProperties(deviceServiceInvokeRequest);
    }

    /// <summary>
    /// 设备准备上料
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    /// <summary>
    /// 设备执行上料
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    /// <summary>
    /// 设备完成上料
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    /// <summary>
    /// 设备准备下料
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    /// <summary>
    /// 设备执行下料
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    /// <summary>
    /// 设备完成下料
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public virtual Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return ResponseSuccess();
    }

    public virtual Task<bool> CheckStatus(DeviceServiceInvokeRequest request)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// 返回成功给调用者
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public Task<DeviceServiceInvokeResponse> ResponseSuccess(string message = "", Dictionary<string, object?>? data = default)
    {
        return Response(ErrorCodes.Sys.SUCCESS, message, data ?? new Dictionary<string, object?> { });
    }

    /// <summary>
    /// 返回失败给调用者
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public Task<DeviceServiceInvokeResponse> ResponseFail(string message = "", Dictionary<string, object?>? data = default)
    {
        return Response(ErrorCodes.Sys.FAIL, message, data ?? new Dictionary<string, object?> { });
    }

    /// <summary>
    /// 返回响应信息给调用者
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public Task<DeviceServiceInvokeResponse> Response(string code, string message = "", Dictionary<string, object?>? data = default)
    {
        return Task.FromResult(new DeviceServiceInvokeResponse
        {
            Code = code,
            Message = message,
            Params = data ?? new Dictionary<string, object?> { }
        });
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        Engine.Dispose();
        OnStatusChanged -= OnDeviceStatusChanged;
        PayloadPanels.CollectionChanged -= OnPanelsChanged;
        PayloadCutterTrays.CollectionChanged -= OnCutterTraysChanged;
    }

    /// <summary>
    /// 添加一条指令
    /// </summary>
    /// <param name="remoteCommand">指令</param>
    protected void AddCommand(IRemoteCommand remoteCommand)
    {
        Commands.AddCommand(remoteCommand);
    }

    /// <summary>
    /// 添加一条指令
    /// </summary>
    /// <param name="commandName">指令简称,如果包含'/'将被认为是commandPath,即服务的全路径地址</param>
    /// <param name="command">指令动作</param>
    /// <param name="commandUsage">指令用途种类</param>
    protected void AddCommand(string commandName, CommandFunction command, CommandUsageKind commandUsage = CommandUsageKind.Both)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandName);
        ArgumentNullException.ThrowIfNull(command);
        var isCommandPath = commandName.Contains('/');

        Commands[commandName] = ObjectFactory.CreateObject<DelegateCommand<Device>>
            (this,
            new CommandDescriptor
            (
                commandName,
                isCommandPath ? commandName : Topics.Downstream.ServiceInvokeTopic(ProductId, DeviceId, commandName, Topics.Downstream.ServiceInvokeTopicTemplate),
                commandUsage
            ),
            command);
    }

    protected virtual void ConfigureServiceCapabilities()
    {
        AddCommand(Topics.Services.REMOTE_COMMAND_SERVICE_ID, RemoteCommand);
        AddCommand(Topics.Services.CANCEL_SCHEDULE_SERVICE_ID, CancelSchedule);
        AddCommand(Topics.Services.COMPLETE_SCHEDULE_SERVICE_ID, CompleteSchedule);
        AddCommand(Topics.Services.SCHEDULE_TASK_SERVICE_ID, ScheduleTask);
        AddCommand(Topics.Services.WORK_SERVICE_ID, Work);
        AddCommand(Topics.Services.STANDBY_SERVICE_ID, Standby);
        AddCommand(Topics.Services.SHUTDOWN_SERVICE_ID, Shutdown);
        AddCommand(Topics.Services.MAINTAIN_SERVICE_ID, Maintain);
        AddCommand(Topics.Services.PREPARE_LOAD_MATERIAL_SERVICE_ID, PrepareLoadMaterial);
        AddCommand(Topics.Services.INVOKE_LOAD_MATERIAL_SERVICE_ID, InvokeLoadMaterial);
        AddCommand(Topics.Services.COMPLETE_LOAD_MATERIAL_SERVICE_ID, CompleteLoadMaterial);
        AddCommand(Topics.Services.PREPARE_UNLOAD_MATERIAL_SERVICE_ID, PrepareUnloadMaterial);
        AddCommand(Topics.Services.INVOKE_UNLOAD_MATERIAL_SERVICE_ID, InvokeUnloadMaterial);
        AddCommand(Topics.Services.COMPLETE_UNLOAD_MATERIAL_SERVICE_ID, CompleteUnloadMaterial);
        AddCommand(Topics.Services.PROPERTIES_READ_SERVICE_ID, ReadProperties);
        AddCommand(Topics.Services.PROPERTIES_WRITE_SERVICE_ID, WriteProperties);
    }

    /// <summary>
    /// 监控全局异常
    /// </summary>
    protected void ConfigureWatchingExceptionEvent()
    {
        WatchingProperties.AddProperty(GLOBAL_EXCEPTION_EVENT_ID, "")
                        .AddProperty(GLOBAL_EXCEPTION_EVENT_NAME, "")
                        .AddProperty(GLOBAL_EXCEPTION_EVENT_MESSAGE, "");
        WatchingProperties.Property(GLOBAL_EXCEPTION_EVENT_ID)
            .When(p => p.IsValueChanged && !string.IsNullOrEmpty(p.NewValue.ToStr()))
            .TriggerAlways(async () =>
            {
                await DataExporter.DeviceEventReport(
                    new DeviceEventReportRequest()
                    {
                        DeviceId = DeviceDescriptor.DeviceId,
                        ProductId = DeviceDescriptor.ProductId,
                        EventId = WatchingProperties.Property(GLOBAL_EXCEPTION_EVENT_ID).NewValue.ToStr(),
                        EventName = WatchingProperties.Property(GLOBAL_EXCEPTION_EVENT_NAME).NewValue.ToStr(),
                    }
                 );

                WatchingProperties.Property(GLOBAL_EXCEPTION_EVENT_ID).SetValue(string.Empty);
                WatchingProperties.Property(GLOBAL_EXCEPTION_EVENT_NAME).SetValue(string.Empty);
                WatchingProperties.Property(GLOBAL_EXCEPTION_EVENT_MESSAGE).SetValue(string.Empty);
            });
    }

    protected virtual void ConfigureExternalDataReport()
    {
        var externalDataReportFactories = ApplicationServices.GetServices<IExternalDataReportFactory>();
        if (externalDataReportFactories != null && externalDataReportFactories.Any())
        {
            foreach (var factory in externalDataReportFactories)
            {
                var externalDataReport = factory.Create(this);
                if (externalDataReport == null) continue;

                DataExporter.OnDeviceEventReport += r => externalDataReport.OnDeviceEventReport(r);
                DataExporter.OnDevicePropertiesReport += r => externalDataReport.OnDevicePropertiesReport(r);
                DataExporter.OnDeviceServiceReport += r => externalDataReport.OnDeviceServiceReport(r);
                DataExporter.OnDeviceStatusReport += r => externalDataReport.OnDeviceStatusReport(r);
                DataExporter.OnDeviceAlarmReport += r => externalDataReport.OnDeviceAlarmReport(r);
                DataExporter.OnDevicePanelChanged += r => externalDataReport.OnDevicePanelChanged(r);
                DataExporter.OnDeviceCutterTrayChanged += r => externalDataReport.OnDeviceCutterTrayChanged(r);
                DataExporter.OnDeviceBrokenToolFault += r => externalDataReport.OnDeviceBrokenToolFault(r);
            }
        }
    }

    /// <summary>
    /// 初始化设备
    /// </summary>
    /// <returns></returns>
    protected virtual Task Initialize()
    {
        DeviceDescriptor.PanelLimit = DeviceDescriptor.LayerLimit * DeviceDescriptor.SpindleNum;
        return Task.CompletedTask;
    }

    protected virtual Task<DeviceCutterTrayChangedResponse> OnCutterTraysChanged(string locationCode)
    {
        DeviceStore.SavePayloadCutterTrays(this);
        return DataExporter.DeviceCutterTrayChangedReport(new DeviceCutterTrayChangedRequest
        {
            ProductId = DeviceDescriptor.ProductId,
            DeviceId = DeviceDescriptor.DeviceId,
            CutterTrayList = PayloadCutterTrays,
        });
    }

    protected virtual Task<DevicePanelChangedResponse> OnPanelsChanged(string locationCode)
    {
        DeviceStore.SavePayloadPanels(this);
        return DataExporter.DevicePanelChangedReport(new DevicePanelChangedRequest
        {
            ProductId = DeviceDescriptor.ProductId,
            DeviceId = DeviceDescriptor.DeviceId,
            PanelList = PanelList.FromList(PayloadPanels.Where(x => x.LocationCode == locationCode).ToList()),
        });
    }

    /// <summary>
    /// do something after application started
    /// </summary>
    protected virtual async Task OnApplicationStarted()
    {
        await DeviceStore.LoadPayloadPanels(this);
        await DeviceStore.LoadPayloadCutterTrays(this);
        await DeviceStore.LoadScheduleTasks(this);
    }

    /// <summary>
    /// do something when application is closing
    /// </summary>
    protected virtual async Task OnApplicationStopping()
    {
        await DeviceStore.SavePayloadPanels(this);
        await DeviceStore.SavePayloadCutterTrays(this);
        await DeviceStore.SaveScheduleTasks(this);
    }

    /// <summary>
    /// 从中控获取设备extra配置信息
    /// </summary>
    /// <returns></returns>
    private async Task LoadDeviceDescriptorRemotely()
    {
        var deviceConfigResponse = await HttpRequestInvoker.PostAsJsonAsync<DeviceConfigRequest, DeviceConfigResponse>(
                            CentralWebOptions.DeviceConfig,
                            new DeviceConfigRequest
                            {
                                ProductId = DeviceDescriptor.ProductId,
                                DeviceId = DeviceDescriptor.DeviceId,
                                ClientId = ClientId,
                                ConfigId = Configs.Basic.DEVICE_DESCRIPTOR
                            });

        if (deviceConfigResponse == null
            || deviceConfigResponse.Data == null
            || !(deviceConfigResponse.Data is DeviceDescriptor remoteDeviceDescriptorExtra))
        {
            return;
        }

        this.DeviceDescriptor.HostAddress = remoteDeviceDescriptorExtra.HostAddress;
        this.DeviceDescriptor.DeviceName = remoteDeviceDescriptorExtra.DeviceName;
        this.DeviceDescriptor.Extra = remoteDeviceDescriptorExtra.Extra;
        _logger.LogInformation("LoadDeviceDescriptorRemotely done!");
    }
}
