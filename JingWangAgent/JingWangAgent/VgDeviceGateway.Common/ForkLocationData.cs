// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Common;

public class ForkLocationData<TDevice> : DeviceShare<TDevice> where TDevice : Device
{
    private readonly ILogger<ForkLocationData<TDevice>> _logger;
    public int LayerLimit;
    private volatile int _position = 1;
    private volatile string _locationCode = string.Empty;
    private volatile string _transcationId = string.Empty;
    private volatile string _transactionMessage = string.Empty;
    private volatile bool _isReady;

    public PanelList Panels { get; set; } = new();
    public int Position { get => _position; set => _position = value; }
    public string LocationCode { get => _locationCode; set => _locationCode = value; }
    public string TranscationId { get => _transcationId; set => _transcationId = value; }
    public string TransactionMessage { get => _transactionMessage; set => _transactionMessage = value; }
    public bool IsReady { get => _isReady; set => _isReady = value; }
    public CurrentContext CurrentContext { get; set; } = new();

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
    public string? FeedAGVIdePoint { get; set; }

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
    public string? TransAGVIdePoint { get; set; }

    /// <summary>
    /// 运输AGV 中间点
    /// </summary>
    public string? TransAGVMiddlePoint { get; set; }

    public ForkLocationData(IServiceProvider serviceProvider, TDevice device) : base(serviceProvider, device)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<ForkLocationData<TDevice>>();
    }

    public virtual void SetConfigs()
    {
        LayerLimit = DeviceDescriptor.LayerLimit.ToInt();
        FeedAGVOutputPoint = DeviceDescriptor.Extra["ShelfAgvPositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
        FeedAGVInnerPoint = DeviceDescriptor.Extra["ShelfInnerPositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
        FeedAGVIdePoint = DeviceDescriptor.Extra["ShelfAgvIdlePositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
        TransAGVOutputPoint = DeviceDescriptor.Extra["TransShelfAgvPositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
        TransAGVInnerPoint = DeviceDescriptor.Extra["TransShelfInnerPositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
        TransAGVIdePoint = DeviceDescriptor.Extra["TransShelfIdlePositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
        TransAGVMiddlePoint = DeviceDescriptor.Extra["transSpindMiddleAgvPosition"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries)?[Position.ToInt() - 1];
    }

    /// <summary>
    /// 通过中控下发过来的调度请求
    /// </summary>
    /// <param name="deviceServiceInvokeRequest"></param>
    /// <returns></returns>
    public Task<DeviceServiceInvokeResponse> CompleteSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        switch (deviceServiceInvokeRequest?.ScheduledStatus)
        {
            case ScheduledTaskStatus.Completed:
                ResetLocationAsCompleted();
                break;

            case ScheduledTaskStatus.Failed:
                ResetLocationAsFailed();
                break;

            case ScheduledTaskStatus.Canceled:
                ResetLocationAsCanceled();
                break;

            default:
                break;
        }

        return Task.FromResult(new DeviceServiceInvokeResponse { Code = ErrorCodes.Sys.SUCCESS });
    }

    private void ResetLocationAsCanceled()
    {
        TranscationId = string.Empty;
        TransactionMessage = $"{LocationCode}: CompleteSchedule --Canceled";
        IsReady = false;
        _logger.LogInformation(TransactionMessage);
    }

    private void ResetLocationAsFailed()
    {
        TranscationId = string.Empty;
        TransactionMessage = $"{LocationCode}: CompleteSchedule --Failed";
        _logger.LogError(TransactionMessage);
    }

    private void ResetLocationAsCompleted()
    {
        TranscationId = string.Empty;
        TransactionMessage = $"{LocationCode}: CompleteSchedule --Completed";
        _logger.LogInformation(TransactionMessage);
    }

    /// <summary>
    /// 通过中控下发过来的调度请求
    /// </summary>
    /// <param name="deviceServiceInvokeRequest"></param>
    /// <returns></returns>
    public Task<DeviceServiceInvokeResponse> CancelSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        switch (deviceServiceInvokeRequest?.ScheduledStatus)
        {
            case ScheduledTaskStatus.Canceled:
                ResetLocationAsCanceled();
                break;

            default:
                break;
        }

        return Task.FromResult(new DeviceServiceInvokeResponse { Code = ErrorCodes.Sys.SUCCESS });
    }

    /// <summary>
    /// 向中控上报调度取消
    /// </summary>
    /// <returns></returns>
    public async Task<DeviceServiceInvokeResponse> ReportCancelSchedule()
    {
        if (string.IsNullOrEmpty(TranscationId))
        {
            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = "当前库位不存在调度TranscationId",
            };
        }

        var requestParam = new CancelScheduleTaskRequest()
        {
            DeviceId = DeviceDescriptor.DeviceId,
            ProductId = DeviceDescriptor.ProductId,
            TraceId = TranscationId,
            Params = new Dictionary<string, object?>()
            {
                { "DeviceCode", LocationCode}
            }
        };

        var response = await HttpRequestInvoker.PostAsJsonAsync<CancelScheduleTaskRequest, CancelScheduleTaskResponse>(DeviceDescriptor.Extra["CancelSchedule"].ToStr(), requestParam) ?? new CancelScheduleTaskResponse();
        if (response.Code == ErrorCodes.Sys.SUCCESS)
        {
            ResetLocationAsCanceled();
        }

        return new DeviceServiceInvokeResponse
        {
            Code = response.Code,
            Message = response.Message,
        };
    }

    /// <summary>
    /// 向中控上报调度失败
    /// </summary>
    /// <returns></returns>
    public async Task<DeviceServiceInvokeResponse> ReportFailedSchedule()
    {
        var requestParam = new FailScheduleTaskRequest()
        {
            DeviceId = DeviceDescriptor.DeviceId,
            ProductId = DeviceDescriptor.ProductId,
            TraceId = TranscationId,
            Params = new Dictionary<string, object?>()
            {
                { "DeviceCode", LocationCode}
            }
        };

        var response = await HttpRequestInvoker.PostAsJsonAsync<FailScheduleTaskRequest, FailScheduleTaskResponse>(CentralWebOptions.ScheduleFail, requestParam) ?? new FailScheduleTaskResponse();
        if (response.Code == ErrorCodes.Sys.SUCCESS)
        {
            ResetLocationAsFailed();
        }

        return new DeviceServiceInvokeResponse
        {
            Code = response.Code,
            Message = response.Message,
        };
    }

    /// <summary>
    /// 向中控上报完成调度
    /// </summary>
    /// <returns></returns>
    public async Task<DeviceServiceInvokeResponse> ReportCompleteSchedule()
    {
        var requestParam = new CompleteScheduleTaskRequest()
        {
            DeviceId = DeviceDescriptor.DeviceId,
            ProductId = DeviceDescriptor.ProductId,
            TraceId = TranscationId,
            Params = new Dictionary<string, object?>()
            {
                { "DeviceCode", LocationCode}
            }
        };

        var response = await HttpRequestInvoker.PostAsJsonAsync<CompleteScheduleTaskRequest, CompleteScheduleTaskResponse>(CentralWebOptions.ScheduleComplete, requestParam) ?? new CompleteScheduleTaskResponse();
        if (response.Code == ErrorCodes.Sys.SUCCESS)
        {
            ResetLocationAsCompleted();
        }

        return new DeviceServiceInvokeResponse
        {
            Code = response.Code,
            Message = response.Message,
        };
    }

    public async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        int position = 0;
        try
        {
            position = deviceServiceInvokeRequest.GetPosition();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "DeviceServiceInvokeRequest.EventId should contain position", deviceServiceInvokeRequest.Params);
        }

        if (!Panels.IsEmptyPayload)
        {
            var msg = $"{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}---{Position}号料架 存在料仓不能上料!";
            return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, msg, deviceServiceInvokeRequest.Params);
        }

        return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "", deviceServiceInvokeRequest.Params);
    }
    public async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

    public async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        if (deviceServiceInvokeRequest.Params.ContainsKey("IsLastStep") && deviceServiceInvokeRequest.Params["IsLastStep"].ToBool())
        {
            var msg = $"CompleteLoadMaterial {Position}号料架上料完成IsLastStep";
            _logger.LogInformation(msg);
            var response = await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, msg);
            if (response.Code == ErrorCodes.Sys.SUCCESS)
            {
                ResetLocationAsCompleted();
            }

            return response;
        }

        if (!deviceServiceInvokeRequest.Params.ContainsKey("LoadingPanel"))
        {
            var msg = $"CompleteLoadMaterial 上生料获取的LoadingPanel的信息为空";
            _logger.LogError(msg);
            return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, msg);
        }

        _logger.LogInformation($"CompleteLoadMaterial 上生料获取的LoadingPanel的信息：{deviceServiceInvokeRequest.Params["LoadingPanel"]}");
        var loadSiloInfo = JsonSerializer.Deserialize<SwapPanel>(deviceServiceInvokeRequest.Params["LoadingPanel"]?.ToString()!);
        var operationEntity = loadSiloInfo?.PanelList;
        _logger.LogInformation($"CompleteLoadMaterial {Position}号料架 上料完成未修改前 PayloadPanels {JsonSerializer.Serialize(Panels)}");

        if (operationEntity == null || operationEntity.Count == 0)
        {
            var msg = $"{Position}号上生料获取的板材信息为null";
            _logger.LogError(msg);
            return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, msg);
        }
        else
        {
            for (int i = 0; i < operationEntity.Count; i++)
            {
                operationEntity[i].Position = Position;
                operationEntity[i].LocationCode = LocationCode;
                Panels[i] = operationEntity[i];
            }

            _logger.LogInformation($"CompleteLoadMaterial {Position} 号料架 上料完成后 PayloadPanels {JsonSerializer.Serialize(Panels)}");
        }

        return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
    }

    public async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

    public async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        int position = 0;
        try
        {
            position = deviceServiceInvokeRequest.GetPosition();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "DeviceServiceInvokeRequest.EventId should contain position", deviceServiceInvokeRequest.Params);
        }

        if (Panels.IsEmptyPayload)
        {
            var msg = $"{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}---{Position}号料架 不存在料仓不能下料!";
            return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, msg, deviceServiceInvokeRequest.Params);
        }

        return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "", deviceServiceInvokeRequest.Params);
    }

    public async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        _logger.LogInformation($"CompleteUnloadMaterial {Position}号料架 开始下料...");
        CurrentContext.Reset();
        Panels.SetNoPayload(LocationCode);

        _logger.LogInformation($"CompleteUnloadMaterial {Position}号料架 下料完成 {Panels.PanelSnapshot}");
        return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
    }

    public async Task<DeviceEventReportResponse> StartNewSchedule(string agvCode = "")
    {
        var spindleBehavior = Panels.IsEmptyPayload ? "0" : "1";
        var req = new DeviceEventReportRequest()
        {
            ProductId = DeviceDescriptor.ProductId,
            DeviceId = DeviceDescriptor.DeviceId,
            ClientId = InteractingDevice.ClientId,
            EventId = (Panels.IsEmptyPayload ? Events.REQUEST_AGV_LOAD_SILO_ONLY : Events.REQUEST_AGV_UNLOAD_SILO_ONLY) + "#" + Position,
            RequestInputProductStatus = ProductStatus.Noop,
            RequestInteractionBehavior = InteractionBehavior.Make(this.DeviceDescriptor.DeviceKind, Panels.IsEmptyPayload ? InteractionBehavior.FRONT_LOAD_SILO_ONLY : InteractionBehavior.FRONT_UNLOAD_SILO_ONLY),
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
                { "ShelfAgvIdlePos",  $"{FeedAGVIdePoint}"},
                { "TransShelfAgvIdlePos",  $"{TransAGVIdePoint}"},
                { "SpindleBehavior", spindleBehavior },
                { "DeviceCode",LocationCode},
                { "LocationCode",LocationCode},
                { "SiloCode",Panels.SiloCode},
                { "ItemCode",Panels.UndrilledItemCodes.FirstOrDefault()??string.Empty},
                { "IsAuxiliary",string.IsNullOrEmpty(agvCode) },
                { "ForceCall",!string.IsNullOrEmpty(agvCode)},
                { "AssignAGV", agvCode??string.Empty }
            },

            PayloadPanels = Panels
        };

        _logger.LogInformation($"{Position}号库位发起调度：  {JsonSerializer.Serialize(req)}  ");

        var response = await DataExporter.DeviceEventReport(req);
        if (response != null && response.Code == ErrorCodes.Sys.SUCCESS)
        {
            TranscationId = response.TraceId ?? string.Empty;
            TransactionMessage = $"{LocationCode} 调度上报成功 {TranscationId}" + Environment.NewLine;
        }

        return response ?? new DeviceEventReportResponse();
    }

    public async Task<DeviceServiceInvokeResponse> SetSiloHandler(DeviceServiceInvokeRequest request)
    {
        Panels.ForEach(p =>
        {
            var temp = request?.PayloadPanels.FirstOrDefault(t => t.Layer == p.Layer);
            if (temp != null)
            {
                p.ItemCode = temp.ItemCode ?? string.Empty;
                p.SiloCode = temp.SiloCode ?? string.Empty;
                p.PanelCode = temp.PanelCode ?? string.Empty;
                p.ProductStatus = temp.ProductStatus;
                p.Pcs = temp.Pcs;
                p.Barcode = temp.Barcode ?? string.Empty;
            }
        });

        await ReportCancelSchedule();

        return new DeviceServiceInvokeResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty,
        };
    }
}
