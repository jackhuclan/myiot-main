using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Store;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Common;

/// <summary>
/// 库位数据
/// </summary>
public class LocationData<TDevice> : DeviceShare<TDevice>
    where TDevice : Device
{
    protected readonly ILogger<LocationData<TDevice>> logger;
    private readonly IDeviceStore _deviceStore;
    public int LayerLimit;

    public LocationData(IServiceProvider serviceProvider,
        IDeviceStore deviceStore,
        TDevice device) : base(serviceProvider, device)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        logger = loggerFactory.CreateLogger<LocationData<TDevice>>();
        _deviceStore = deviceStore;
        LayerLimit = DeviceDescriptor.LayerLimit.ToInt();
    }

    #region Trigger Propertyies

    /// <summary>
    /// 是否能呼叫（呼叫锁定）
    /// </summary>
    public volatile bool IsCanCallAgv = true;

    /// <summary>
    /// 是否人员操作--就绪
    /// </summary>
    public volatile bool IsReady = false;

    /// <summary>
    /// 是否AGV 在工作
    /// </summary>
    public volatile bool IsAgvWorking = false;

    /// <summary>
    /// 是否在执行中
    /// </summary>
    public volatile bool IsBusy = false;

    private volatile string _currentEventTraceId = "";

    private volatile string _trancationMessage = "";

    protected Dictionary<string, float> ItemPanelInfoWhenCompleteLoadMaterial = new Dictionary<string, float>();

    /// <summary>
    /// 退pin 呼叫上料
    /// </summary>
    public bool UpinCallAgvUploadSilo { get; set; } = false;

    /// <summary>
    /// 退Pin 呼叫下料
    /// </summary>
    public bool UpinCallAgvDownLoadSilo { get; set; } = false;

    /// <summary>
    /// 上Pin 呼叫上料
    /// </summary>
    public bool PinCallAgvUploadSilo { get; set; } = false;

    /// <summary>
    /// 上pin 呼叫下料
    /// </summary>
    public bool PinCallAgvDownLoadSilo { get; set; } = false;

    #endregion Trigger Propertyies

    /// <summary>
    /// 页面绑定model
    /// </summary>
    ///

    #region OtherProperties

    public CurrentContext CurrentContext { get; set; } = new();

    public PanelList Panels { get; set; } = new();
    public string Position { get; set; } = "1";

    //public string LocationCode => $"{InteractingDevice.DeviceId}{Position.ToString().PadLeft(3, '0')}";
    public string LocationCode { get; set; }

    public string? PositonCode { get; set; }
    public DateTime CallAgvTimes { get; set; } = DateTime.Now;

    public string TransactionMessage
    {
        get { return _trancationMessage; }
        set { _trancationMessage = value; }
    }

    public string TransactionId { get; set; } = string.Empty;

    public string CurrentEventTraceId
    {
        get { return _currentEventTraceId; }
        set { _currentEventTraceId = value; }
    }

    /// <summary>
    /// 上料AGV 外点
    /// </summary>
    public string? FeedAGVOutputPoint { get; set; }

    /// <summary>
    /// 上料AGV 内点
    /// </summary>
    public string? FeedAGVInnerPoint { get; set; }

    /// <summary>
    /// 上料AGV  任务结束点
    /// </summary>
    public string? FeedAGVRestPoint { get; set; }

    /// <summary>
    /// 运输AGV 外点
    /// </summary>
    public string? TransAGVOutputPoint { get; set; }

    /// <summary>
    /// 运输AGV 内点
    /// </summary>
    public string? TransAGVInnerPoint { get; set; }

    /// <summary>
    /// 运输AGV 任务结束点
    /// </summary>
    public string? TransAGVRestPoint { get; set; }

    /// <summary>
    /// 运输AGV 中间点
    /// </summary>
    public string? TransAGVMiddlePoint { get; set; }

    #endregion OtherProperties

    /// <summary>
    /// 加载本地的点位配置信息
    /// </summary>
    public virtual void LoadLocalPointConfig()
    {
        FeedAGVOutputPoint = FeedAGVOutputPoint ?? DeviceDescriptor.Extra["ShelfAgvPositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
        FeedAGVInnerPoint = FeedAGVInnerPoint ?? DeviceDescriptor.Extra["ShelfInnerPositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
        FeedAGVRestPoint = FeedAGVRestPoint ?? DeviceDescriptor.Extra["ShelfAgvIdlePositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
        TransAGVOutputPoint = TransAGVOutputPoint ?? DeviceDescriptor.Extra["TransShelfAgvPositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
        TransAGVInnerPoint = TransAGVInnerPoint ?? DeviceDescriptor.Extra["TransShelfInnerPositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
        TransAGVRestPoint = TransAGVRestPoint ?? DeviceDescriptor.Extra["TransShelfIdlePositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
        TransAGVMiddlePoint = TransAGVMiddlePoint ?? DeviceDescriptor.Extra["transSpindMiddleAgvPosition"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
    }

    public async Task<DeviceServiceInvokeResponse> SetSiloHandler(DeviceServiceInvokeRequest request)
    {
        if (request.PayloadPanels == null)
        {
            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = "下发的板料信息不能为空",
            };
        }

        Panels.ForEach(p =>
        {
            var temp = request.PayloadPanels.FirstOrDefault(t => t.Layer == p.Layer);
            if (temp != null)
            {
                p.ItemCode = temp.ItemCode ?? string.Empty;
                p.SiloCode = temp.SiloCode ?? string.Empty;
                p.PanelCode = temp.PanelCode ?? string.Empty;
                p.ProductStatus = temp.ProductStatus;
                p.Pcs = temp.Pcs;
                p.Barcode = temp.Barcode ?? string.Empty;
                p.PanelLength = temp.PanelLength;
                p.PanelWidth = temp.PanelWidth;
                p.PanelThickness = temp.PanelThickness;
                p.PinOffset = temp.PinOffset;
                p.LotId = temp.LotId;
            }
        });

        await SavePanels();
        await CancelSchdule();

        return new DeviceServiceInvokeResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty,
        };
    }

    public async Task<DeviceServiceInvokeResponse> RemoveSiloHandler(DeviceServiceInvokeRequest request)
    {
        CurrentContext.Reset();
        Panels.SetNoPayload();

        await SavePanels();
        await CancelSchdule();

        return new DeviceServiceInvokeResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty,
        };
    }

    public async Task<DeviceServiceInvokeResponse> ResetStatusHandler(DeviceServiceInvokeRequest request)
    {
        await CancelSchdule();

        IsReady = true;
        IsAgvWorking = false;
        IsCanCallAgv = true;

        return new DeviceServiceInvokeResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty,
        };
    }

    public async Task<CancelScheduleTaskResponse?> CancelSchdule()
    {
        //IsCanCallAgv = true;
        //IsAgvWorking = false;
        var requestParam = new CancelScheduleTaskRequest()
        {
            DeviceId = DeviceDescriptor.DeviceId,
            ProductId = DeviceDescriptor.ProductId,
            Params = new Dictionary<string, object?>()
                           {
                               { "DeviceCode", LocationCode},
                               { "CancelReason","人工取消Ready,进行板料维护!"}
                           }
        };

        return await HttpRequestInvoker.PostAsJsonAsync<CancelScheduleTaskRequest, CancelScheduleTaskResponse>(DeviceDescriptor.Extra["CancelSchedule"].ToStr(), requestParam);
    }

    public async Task CompleteSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        await ReportingProcess($"{LocationCode} CompleteSchedule_Start : (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)},IsCanCallAgv= {IsCanCallAgv},IsAgvWorking ={IsAgvWorking},ScheduledStatus={deviceServiceInvokeRequest?.ScheduledStatus.ToStr()}", GetTraceId(deviceServiceInvokeRequest!));
        switch (deviceServiceInvokeRequest?.ScheduledStatus)
        {
            case ScheduledTaskStatus.Completed:
                TransactionMessage = $"{LocationCode}: CompleteSchedule --Completed：重置 IsCanCallAgv= true，自动再次发起呼叫";
                logger.LogInformation(TransactionMessage);
                CallAgvTimes = DateTime.Now;
                IsCanCallAgv = true;
                IsAgvWorking = false;
                break;

            case ScheduledTaskStatus.Failed:
                TransactionMessage = $"{LocationCode}: CompleteSchedule --Failed：IsWorking=True,需要手动重置上料信号后才能再次发起呼叫";
                logger.LogInformation(TransactionMessage);
                CallAgvTimes = DateTime.Now;
                IsCanCallAgv = true;
                break;

            case ScheduledTaskStatus.Canceled:
                TransactionMessage = $"{LocationCode}: CompleteSchedule --Canceled：重置 IsCanCallAgv= true,自动再次发起呼叫";
                logger.LogInformation(TransactionMessage);
                CallAgvTimes = DateTime.Now;
                IsCanCallAgv = true;
                IsAgvWorking = false;
                break;

            default:
                break;
        }

        InteractingDevice.WatchingProperties.Property("TranscationChange").SetValue(true);
        logger.LogInformation("CompleteSchedule_Rev Finish");
        await ReportingProcess($"{LocationCode} CompleteSchedule_Finish : (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)},IsCanCallAgv= {IsCanCallAgv},IsAgvWorking ={IsAgvWorking},IsReady ={IsReady}", GetTraceId(deviceServiceInvokeRequest!));
    }

    public async Task CancelSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        CallAgvTimes = DateTime.Now;
        IsAgvWorking = false;
        IsCanCallAgv = true;
        await ReportingProcess($"{LocationCode} CancelSchedule Finish : (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)},IsCanCallAgv= {IsCanCallAgv},IsAgvWorking ={IsAgvWorking},IsReady ={IsReady}", GetTraceId(deviceServiceInvokeRequest!));

        // return Task.CompletedTask;
    }

    public bool SiloIsNotExistByPosition()
    {
        return Panels.All(s => s.ProductStatus == ProductStatus.EmptyPayload);
    }

    public async Task SetAllCookedStatus()
    {
        foreach (var panel in Panels)
        {
            if (!string.IsNullOrEmpty(panel.ItemCode))
                panel.ProductStatus = ProductStatus.Finished_POST_BUFFER;
        }

        await SavePanels();

        //return Task.CompletedTask;
    }

    public async Task UpdateSiloInfoByPosition(List<Panel> newPanel)
    {
        int panelsIndex = 0;
        for (int i = 0; i < Panels.Count; i++)
        {
            {
                if (panelsIndex > LayerLimit || panelsIndex >= newPanel.Count)
                {
                    return;
                }

                Panels[i] = newPanel[panelsIndex++];
            }
        }

        await SavePanels();
    }

    public async Task SetEmptySiloInLocation()
    {
        PanelList panelList = new PanelList();

        for (int i = 0; i < Panels.Count; i++)
        {
            panelList.Add(new Panel
            {
                Position = Position.ToInt(),
                Layer = i,
                LocationCode = LocationCode,
                ProductStatus = ProductStatus.EmptySiloBox
            });
        }

        await UpdateSiloInfoByPosition(panelList);
    }

    public async Task UpdateSiloInfoByPositionAndIndex(int layer, PanelList panels)
    {
        for (int i = 0; i < panels.Count; i++)
        {
            Panels[layer + i] = panels[i];
        }
        await SavePanels();
    }

    public async Task UpdatePanelStatusFromEmptyPayloadToEmptySiloBox()
    {
        Panels.ForEach(panel =>
        {
            if (panel.ProductStatus == ProductStatus.EmptyPayload)
            {
                panel.ProductStatus = ProductStatus.EmptySiloBox;
            }
        });

        await SavePanels();
    }

    public virtual async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        await ReportingProcess($"{LocationCode} PrepareLoadMaterial Start : (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)})", GetTraceId(deviceServiceInvokeRequest!));

        if (!SiloIsNotExistByPosition())
        {
            logger.LogError($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}---{Position}号料架 存在料仓不能上料!");

            await ReportingProcess($"{LocationCode} {Position}号料架 存在料仓不能上料!", GetTraceId(deviceServiceInvokeRequest!));

            return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"{Position} 料架上有料仓，不能上料 \r\n {JsonSerializer.Serialize(Panels)}", deviceServiceInvokeRequest.Params);
        }

        logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}---{Position}号料架 不存在料仓 可以上料!");
        await ReportingProcess($"{LocationCode} PrepareLoadMaterial  Finish :(CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)})", GetTraceId(deviceServiceInvokeRequest!));

        return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(Panels), deviceServiceInvokeRequest.Params);
    }

    public virtual async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        IsAgvWorking = true;
        await ReportingProcess($"{LocationCode} InvokeLoadMaterial Finish (IsAgvWorking = {IsAgvWorking},CurrentEventTraceId ={GetTraceId(deviceServiceInvokeRequest!)}) 。。。", GetTraceId(deviceServiceInvokeRequest!));
        return await ResponseSuccess(deviceServiceInvokeRequest.ReplyTopic!);
    }

    public virtual async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        if (deviceServiceInvokeRequest.Params.ContainsKey("IsLastStep") && deviceServiceInvokeRequest.Params["IsLastStep"].ToBool())
        {
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(Panels));
        }

        await ReportingProcess($"{LocationCode} CompleteLoadMaterial Start : (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)}", GetTraceId(deviceServiceInvokeRequest!));

        var operationEntity = new List<Panel> { };
        logger.LogInformation($"CompleteLoadMaterial {Position}号料架 上料完成未修改前 PayloadPanels {JsonSerializer.Serialize(Panels)}");

        if (!deviceServiceInvokeRequest.Params.ContainsKey("LoadingPanel"))
        {
            logger.LogError($"CompleteLoadMaterial 上生料获取的LoadingPanel的信息 为空");
            await SetEmptySiloInLocation();
        }
        else
        {
            logger.LogInformation($"CompleteLoadMaterial 上生料获取的LoadingPanel的信息：{deviceServiceInvokeRequest.Params["LoadingPanel"]}");

            var loadSiloInfo = JsonSerializer.Deserialize<SwapPanel>(deviceServiceInvokeRequest.Params["LoadingPanel"]?.ToString()!
                       , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            operationEntity = loadSiloInfo?.PanelList;
            if (operationEntity != null && operationEntity.Count > 0)
            {
                bool isAllClicker = DeviceExtensions.SiloIsHaveNoRawByPosition(operationEntity);
                logger.LogInformation($"CompleteLoadMaterial isAllClicker:{isAllClicker}");

                ItemPanelInfoWhenCompleteLoadMaterial.Clear();
                for (int i = 0; i < operationEntity.Count; i++)
                {
                    //operationEntity[i].Position = Position.ToInt();
                    //operationEntity[i].LocationCode = LocationCode;
                    //operationEntity[i].ProductStatus = ConvertProductStatus(isAllClicker, operationEntity[i].ProductStatus, deviceServiceInvokeRequest);
                    await ModifyPanels(isAllClicker, operationEntity[i], deviceServiceInvokeRequest, ItemPanelInfoWhenCompleteLoadMaterial);
                }

                await UpdateSiloInfoByPosition(operationEntity);
            }
            else
            {
                logger.LogDebug($"上料仓获取的板材信息为空");
                await ReportingProcess($"{LocationCode} (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)} CompleteLoadMaterial Fail_Message :上生料获取的板材信息为 null。。。", GetTraceId(deviceServiceInvokeRequest!));
                await SetEmptySiloInLocation();
            }

            logger.LogDebug($"CompleteLoadMaterial (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)}, {Position} 号料架 上料完成后 PayloadPanels {JsonSerializer.Serialize(Panels)}");
        }

        IsAgvWorking = false;
        CurrentContext.IsExistSilo = true;

        await BindLineSideStock(operationEntity?.FirstOrDefault()?.SiloCode!, PositonCode!, "1", deviceServiceInvokeRequest);
        await ReportingProcess($"{LocationCode} CompleteLoadMaterial (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)}, Finish (IsAgvWorking = {IsAgvWorking})。。。", GetTraceId(deviceServiceInvokeRequest!));
        return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadPanels));
    }

    public virtual async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        await ReportingProcess($"{LocationCode} PrepareUnloadMaterial Start : (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)})", GetTraceId(deviceServiceInvokeRequest!));

        if (SiloIsNotExistByPosition())
        {
            logger.LogError($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}---{Position}号料架 无料仓不能下料!");
            await ReportingProcess($"{LocationCode}  (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)}{Position}号料架 料架上无料仓，不能下料!", GetTraceId(deviceServiceInvokeRequest!));
            return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"{Position} 料架上无料仓，不能下料 \r\n {JsonSerializer.Serialize(Panels)}", deviceServiceInvokeRequest.Params);
        }

        logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}---{Position}号料架 存在料仓 可以下料!");
        await ReportingProcess($"{LocationCode} PrepareUnloadMaterial Finish， (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)} {Position} 存在料仓 可以下料!", GetTraceId(deviceServiceInvokeRequest!));
        return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(Panels), deviceServiceInvokeRequest.Params);
    }

    public virtual async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        IsAgvWorking = true;
        await ReportingProcess($"{LocationCode} InvokeUnloadMaterial Finish (IsAgvWorking = {IsAgvWorking},CurrentEventTraceId ={GetTraceId(deviceServiceInvokeRequest!)}) 。。。", GetTraceId(deviceServiceInvokeRequest!));
        return await InteractingDevice.ResponseSuccess(deviceServiceInvokeRequest.ReplyTopic!);
    }

    public virtual async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        await ReportingProcess($"{LocationCode} CompleteUnloadMaterial Start : (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)}", GetTraceId(deviceServiceInvokeRequest!));

        deviceServiceInvokeRequest!.Params["UnloadingPanel"] = new SwapPanel() { SpindleId = Position.ToInt(), PanelList = Panels };

        logger.LogInformation($"CompleteUnloadMaterial {Position}号料架 下料完成 UnloadingPanel 赋值：{JsonSerializer.Serialize(deviceServiceInvokeRequest!.Params["UnloadingPanel"])}");

        await BindLineSideStock(Panels.FirstOrDefault()?.SiloCode!, PositonCode!, "0", deviceServiceInvokeRequest);
        await SetNoPayload();

        logger.LogInformation($"CompleteUnloadMaterial {Position}号料架 下料完成后 PayloadPanels {JsonSerializer.Serialize(Panels)}");

        IsAgvWorking = false;
        CurrentContext.IsExistSilo = false;

        await ReportingProcess($"{LocationCode} CompleteUnloadMaterial Finish : IsAgvWorking :{IsAgvWorking}, (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)}", GetTraceId(deviceServiceInvokeRequest!));
        return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(Panels), deviceServiceInvokeRequest.Params);
    }

    public ProductStatus ConvertProductStatus(bool isAllClicker, ProductStatus oldStatus, DeviceServiceInvokeRequest request)
    {
        if (oldStatus == ProductStatus.EmptySiloBox) return oldStatus;

        if (InteractingDevice.DeviceDescriptor.DeviceKind == DeviceKind.PanelSiloFork)
        {
            if (isAllClicker && oldStatus == ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1) return ProductStatus.Finished_POST_BUFFER;
        }

        if (oldStatus == ProductStatus.PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_1) return ProductStatus.Finished_PRE_BUFFER;

        return oldStatus;
    }

    protected async Task ModifyPanels(bool isAllClicker, Panel panel, DeviceServiceInvokeRequest request, Dictionary<string, float> itemPanelInfo)
    {
        try
        {
            panel.Position = Position.ToInt();
            panel.LocationCode = LocationCode;
            panel.ProductStatus = ConvertProductStatus(isAllClicker, panel.ProductStatus, request);
            // 从中控获取板长
            if (!string.IsNullOrWhiteSpace(panel.ItemCode)
                  && panel.ProductStatus == ProductStatus.Finished_PRE_BUFFER
                  && DeviceDescriptor.Extra.ContainsKey("GetItemInfo"))
            {
                if (itemPanelInfo.TryGetValue(panel.ItemCode, out float panthLength))
                {
                    panel.PanelLength = panthLength;
                }
                else
                {
                    var itemInfo = await HttpRequestInvoker.GetFromJsonAsync<MaterialPanelInfo>(string.Format(DeviceDescriptor.Extra["GetItemInfo"].ToStr(), panel.ItemCode));
                    if (itemInfo != null)
                    {
                        panel.PanelLength = itemInfo.panelLength;
                        itemPanelInfo.TryAdd(panel.ItemCode, itemInfo.panelWidth);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"{DateTime.Now.ToLongTimeString()}---{LocationCode} 料架 获取板长失败，原因：{ex.Message}", ex);
        }
    }

    public async Task<string> ForeCallAgv()
    {
        string message = "";

        logger.LogInformation($"ForeCallAgv {LocationCode}号料架 ,TracId:{TransactionId}");
        var requestParam = new CancelScheduleTaskRequest()
        {
            DeviceId = DeviceDescriptor.DeviceId,
            ProductId = DeviceDescriptor.ProductId,
            TraceId = TransactionId ?? string.Empty
        };

        IsReady = false;
        logger.LogInformation($"Trace_Ready : ForeCallAgv {LocationCode} 设置Ready=false");
        if (!string.IsNullOrEmpty(TransactionId))
        {
            var cancelSchduleResult = await HttpRequestInvoker.PostAsJsonAsync<CancelScheduleTaskRequest, CancelScheduleTaskResponse>(DeviceDescriptor.Extra["CancelSchedule"].ToStr(), requestParam);
            if (cancelSchduleResult != null && cancelSchduleResult.Code == ErrorCodes.Sys.SUCCESS)
            {
                logger.LogInformation($"ForeCallAgv： {LocationCode}号料架 取消后 准备强制呼叫AGV ...");
                await Task.Delay(4 * 1000);
                logger.LogInformation($"ForeCallAgv： {LocationCode}号料架 取消后 执行强制呼叫AGV ...");
                var result = await CallAgv(false);
                logger.LogInformation($"ForeCallAgv： {LocationCode}号料架 强制呼叫结果:{JsonSerializer.Serialize(result)}");
                if (result != null && result.Code == 0)
                {
                    CallAgvTimes = DateTime.Now;
                }

                message = result?.Message!;
            }
            else
            {
                message = cancelSchduleResult == null ? "接口无返回" : cancelSchduleResult.Message;
            }
        }
        else
        {
            logger.LogInformation($"ForeCallAgv: {LocationCode}号料架 直接 强制呼叫AGV ...");
            var result = await CallAgv(false);
            if (result != null && result.Code == 0)
            {
                CallAgvTimes = DateTime.Now;
            }

            message = result?.Message!;
        }

        IsReady = true;
        logger.LogInformation($"Trace_Ready : ForeCallAgv: {LocationCode} 设置Ready=true");
        TransactionMessage = message;
        WatchingProperties.Property("TranscationChange").SetValue(true);
        return message;
    }

    public async Task<BaseResponse> CallAgv(bool is_auxiliary)
    {
        BaseResponse response = new BaseResponse();
        bool IsHaveNoSilo = true;
        string spindleBehavior = "-1";
        try
        {
            if (!MqttClientWrapper.IsConnected)
            {
                response.Message = $"{LocationCode}: 呼叫AGV Mqtt 连接断开 不上报消息";
                return response;
            }

            if (is_auxiliary)
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(CallAgvTimes);
                if (timeSpan.TotalSeconds < InteractingDevice.DeviceDescriptor.Extra["CallAgvTimeInterval"].ToInt())
                {
                    logger.LogDebug($"{LocationCode}: 小于呼叫间隔不能呼叫");
                    response.Message = $" 小于呼叫间隔不能呼叫";
                    return response;
                }

                CallAgvTimes = DateTime.Now;

                if (!IsCanCallAgv)
                {
                    // response.Message = $" {LocationCode}: 呼叫agv信号为 false 不能呼叫";
                    response.Message = $"[重复触发]--{TransactionMessage}";
                    return response;
                }
            }

            logger.LogInformation($"{LocationCode}:  is_auxiliary: {is_auxiliary} ");

            IsHaveNoSilo = SiloIsNotExistByPosition();

            spindleBehavior = IsHaveNoSilo ? "0" : "1";

            int callAgvSplinesStatus = Position.ToInt() - 1;

            logger.LogInformation($"{LocationCode}: spindleBehavior:{spindleBehavior}, IsHaveNoSilo: {IsHaveNoSilo} （true:上料仓,false:下料仓 ");

            if (IsHaveNoSilo)  //无料仓呼叫 上料
            {
                var req = new DeviceEventReportRequest()
                {
                    ProductId = DeviceDescriptor.ProductId,
                    DeviceId = DeviceDescriptor.DeviceId,
                    ClientId = InteractingDevice.ClientId,
                    EventId = Events.REQUEST_AGV_LOAD_SILO_ONLY + "#" + Position,
                    RequestInputProductStatus = (ProductStatus)CurrentContext.PanelTypeWhenExistNoSilo,
                    RequestInteractionBehavior = InteractionBehavior.Make(this.DeviceDescriptor.DeviceKind, InteractionBehavior.FRONT_LOAD_SILO_ONLY),
                    RequestDeviceKind = DeviceDescriptor.DeviceKind,
                    RequestMaterialKind = MaterialKind.PanelSilo,
                    RequestInteractionDirection = InteractionPosition.Front,
                    Params = new Dictionary<string, object?>()
                        {
                            { "SpindleNum", 1 },
                            { "ShelfIndex",Position },
                            { "Spindles",  $"{FeedAGVOutputPoint}"},
                            { "ShelfInnerPos",  $"{FeedAGVInnerPoint}"},
                            { "TransSpindles",  $"{TransAGVOutputPoint}"},
                            { "TransShelfInnerPos",  $"{TransAGVInnerPoint}"},
                            { "TransActionPos",  $"{TransAGVMiddlePoint}"},
                            { "ShelfAgvIdlePos",  $"{FeedAGVRestPoint}"},
                            { "TransShelfAgvIdlePos",  $"{TransAGVRestPoint}"},
                            { "SpindleBehavior", spindleBehavior },
                            { "DeviceCode",LocationCode},
                            { "LocationCode",LocationCode},
                            { "SiloCode",""},
                            { "ItemCode",CurrentContext.ItemCode},
                            {"IsAuxiliary",is_auxiliary },
                            {"ForceCall",!is_auxiliary},
                            {"AssignAGV",CurrentContext.AGVCode }
                        },

                    PayloadPanels = Panels
                };

                logger.LogInformation($"{LocationCode}: 上报 上料 请求参数：  {JsonSerializer.Serialize(req)}  ");

                var result = await DataExporter.DeviceEventReport(req);

                if (result == null)
                {
                    response.Message = $"{DateTime.Now.ToString()}  ----  和服务器断开连接，呼叫失败";
                }
                else
                {
                    if (result.Code == ErrorCodes.Sys.SUCCESS)
                    {
                        IsCanCallAgv = false;
                        TransactionId = result.TraceId!;
                        response.Code = 0;
                        response.Message = $"{DateTime.Now.ToString()} {LocationCode} ----呼叫成功   {result.TraceId}";
                    }
                    else
                    {
                        response.Message = $"{DateTime.Now.ToString()}  {LocationCode} ----呼叫失败  {result.Message}";
                    }
                }

                logger.LogInformation($"{LocationCode}: 上报 上料 信息结果：  {JsonSerializer.Serialize(result, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}  ");
                return response;
            }
            else  //有料仓呼叫 下料
            {
                var req = new DeviceEventReportRequest()
                {
                    ProductId = DeviceDescriptor.ProductId,
                    DeviceId = DeviceDescriptor.DeviceId,
                    ClientId = InteractingDevice.ClientId,
                    EventId = Events.REQUEST_AGV_UNLOAD_SILO_ONLY + "#" + Position,
                    RequestOutputProductStatus = DeviceExtensions.ConvertOutPutStatus(Panels),
                    RequestInteractionBehavior = InteractionBehavior.Make(this.DeviceDescriptor.DeviceKind, InteractionBehavior.FRONT_UNLOAD_SILO_ONLY),
                    RequestDeviceKind = DeviceDescriptor.DeviceKind,
                    RequestMaterialKind = MaterialKind.PanelSilo,
                    RequestInteractionDirection = InteractionPosition.Front,
                    Params = new Dictionary<string, object?>()
                      {
                            { "SpindleNum", 1 },
                            { "ShelfIndex",Position },
                            { "Spindles",  $"{FeedAGVOutputPoint}"},
                            { "ShelfInnerPos",  $"{FeedAGVInnerPoint}"},
                            { "TransSpindles",  $"{TransAGVOutputPoint}"},
                            { "TransShelfInnerPos",  $"{TransAGVInnerPoint}"},
                            { "TransActionPos",  $"{TransAGVMiddlePoint}"},
                            { "ShelfAgvIdlePos",  $"{FeedAGVRestPoint}"},
                            { "TransShelfAgvIdlePos",  $"{TransAGVRestPoint}"},
                            { "SpindleBehavior", spindleBehavior },
                            { "DeviceCode",LocationCode},
                            { "LocationCode",LocationCode},
                            { "SiloCode",Panels.FirstOrDefault(p=>!string.IsNullOrEmpty(p.SiloCode) )?.SiloCode },
                            { "ItemCode",""},
                            {"IsAuxiliary",is_auxiliary },
                            {"ForceCall",!is_auxiliary},
                            {"AssignAGV",CurrentContext.AGVCode }
                      },
                    PayloadPanels = Panels
                };

                logger.LogInformation($"{LocationCode} 上报 下料 请求参数：  {JsonSerializer.Serialize(req)}  ");
                var result = await DataExporter.DeviceEventReport(req);
                if (result == null)
                {
                    response.Message = $"{DateTime.Now.ToString()}  ----  和服务器断开连接，呼叫失败";
                }
                else
                {
                    if (result.Code == ErrorCodes.Sys.SUCCESS)
                    {
                        IsCanCallAgv = false;
                        TransactionId = result.TraceId!;

                        response.Code = 0;
                        response.Message = $"{DateTime.Now.ToString()} {LocationCode}  ----呼叫成功   {result.TraceId}";
                    }
                    else if (result.Code == ErrorCodes.Sys.FAIL)
                    {
                        response.Message = $"{DateTime.Now.ToString()}  {LocationCode} ----呼叫失败  {result.Message}";
                    }
                }

                logger.LogInformation($" {LocationCode}: 上报 下料 信息结果：  {JsonSerializer.Serialize(result, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}  ");

                return response;
            }
        }
        catch (Exception ee)
        {
            logger.LogError($"{LocationCode}: 呼叫AGV异常 ： {ee} ");

            response.Message = $"呼叫AGV异常 ： {ee.Message} ";
            return response;
        }
    }

    /// <summary>
    /// 加载板料
    /// </summary>
    /// <returns></returns>
    public async Task LoadPanels()
    {
        Panels.Clear();

        await _deviceStore.LoadPayloadPanels(LocationCode, Panels);

        if (Panels == null || !Panels.Any())
        {
            CurrentContext.Reset();
            Panels = Panel.HasSilo.NoPanelForSingleSpindle("", Position.ToInt(), 0, LayerLimit);
            Panels.SetNoPayload();
            Panels.SetLocationCode(LocationCode);
        }
    }

    public async Task SetNoPayload()
    {
        CurrentContext.Reset();
        Panels = Panel.NoSilo.PanelForSingleSpindle(CurrentContext.ShelfIndex, 0, LayerLimit);
        Panels.SetLocationCode(LocationCode);
        Panels.SetNoPayload();
        await SavePanels();
    }

    /// <summary>
    /// 保存板料信息
    /// </summary>
    /// <returns></returns>
    public async Task SavePanels()
    {
        await _deviceStore.SavePayloadPanels(Panels);
    }

    public async Task RaiseCollectPanelChange()
    {
        try
        {
            Panels.SetLocationCode(LocationCode);

            try
            {
                if (InteractingDevice.PayloadPanels.Exists(p => p == null))
                    InteractingDevice.PayloadPanels.RemoveAll(p => p == null);

                if (InteractingDevice.PayloadPanels.Exists(p => p.LocationCode == LocationCode))
                    InteractingDevice.PayloadPanels.RemoveAll(p => p.LocationCode == LocationCode);

                InteractingDevice.PayloadPanels.AddRange(Panels);
                await InteractingDevice.DeviceStore.SavePayloadPanels(InteractingDevice);
            }
            catch (Exception e)
            {
                // logger.LogError(e, $"RaiseCollectPanelChange: LocationCode={LocationCode},InteractingDevice.PayloadPanels:{JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} \r\n  ErrorMessage: {e.ToString()}");
                logger.LogError(e, $"RaiseCollectPanelChange: LocationCode={LocationCode},PayloadPanels   ErrorMessage: {e.ToString()}");
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }

    public async Task ReportingProcess(string processMessage, string traceId = "")
    {
        var request = new AddScheduleLogRequest
        {
            DeviceId = DeviceDescriptor.DeviceId,
            TraceId = string.IsNullOrEmpty(traceId) ? CurrentEventTraceId : traceId,
            Message = processMessage
        };

        await HttpRequestInvoker.PostAsJsonAsync<AddScheduleLogRequest, AddScheduleLogResponse>(CentralWebOptions.ProcessReportSchedule, request);
    }

    public string GetTraceId(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        CurrentEventTraceId = string.Empty;
        if (deviceServiceInvokeRequest != null && deviceServiceInvokeRequest.Params.TryGetValue(ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID, out object? traceId)
                && traceId != null)
        {
            CurrentEventTraceId = traceId.ToStr();
        }

        return CurrentEventTraceId;
    }

    #region 绑定/解绑托盘库位

    protected async Task BindLineSideStock(string siloCode, string positionCode, string operationCode, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        if (DeviceDescriptor.Extra.GetConfig("ShelfAGV").ToStr() != "STD"
            ||
            (deviceServiceInvokeRequest.Params.ContainsKey("AgvKind") && deviceServiceInvokeRequest.Params["AgvKind"].ToStr() == "Std"))
        {
            return;
        }

        try
        {
            var _Request200 = new BindSiloStockEntity()
            {
                clientCode = "VEGA001",
                indBind = operationCode,
                podCode = siloCode,
                reqCode = $"{LocationCode}_{DateTime.Now.Ticks.ToString()}",
                reqTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                mapDataCode = positionCode
            };

            logger.LogInformation($"BindLineSideStock:{LocationCode}: 料架与料仓绑定 : 请求参数: {JsonSerializer.Serialize(_Request200)} \r\n");

            var res200 = await HttpRequestInvoker.PostAsJsonAsync<BindSiloStockEntity?, STDResponse>(DeviceDescriptor.Extra["BindLineSideStockUrl"].ToStr(), _Request200, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });

            logger.LogInformation($"BindLineSideStock : {LocationCode} 料架与料仓绑定 : 返回内容:  {JsonSerializer.Serialize(res200, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            })} \r\n");
        }
        catch (Exception ex)
        {
            logger.LogError($"{LocationCode}: BindLineSideStock Error", ex);
        }
    }

    #endregion 绑定/解绑托盘库位
}
