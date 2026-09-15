using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using VegaIot.External.XianjinIot.Models;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Agv;
using VgDeviceGateway.Devices.Agv.AgvDevice;
using VgDeviceGateway.Devices.Common;

namespace VegaIot.External.XianjinIot.Commands.AGV;

internal class AgvPushPanelCommand : SimpleCommand<DefaultAgv>
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotOptions _xianJinIotOptions;
    private readonly ILogger<BackPanelAgv> logger;

    public AgvPushPanelCommand(
        ILogger<BackPanelAgv> logger,
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        IOptions<XianJinIotOptions> options,
        DefaultAgv device,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _xianJinIotOptions = options.Value;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Message = "暂未运行逻辑";
        var isexistPayloadSegment = targetResponse.Params.ContainsKey("PayloadSegment");
        if (!isexistPayloadSegment || deviceServiceInvokeRequest.Params["PayloadSegment"] == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"PayloadSegment 参数不正确：{deviceServiceInvokeRequest.Params["PayloadSegment"]}";
            await CommandCallback(targetResponse);
            return targetResponse;
        }

        var agvPushPanelPayload = JsonSerializer.Deserialize<AgvPushPanelPayload>(targetResponse.Params["PayloadSegment"].ToStr());
        if (agvPushPanelPayload == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"agvPushPanelPayload 实体转换出错：NUll";
            await CommandCallback(targetResponse);
            return targetResponse;
        }
        //第一步：收到上料命令,并回调
        var callbackResult = await CommandCallback(targetResponse);
        if (callbackResult.Code != ErrorCodes.Sys.SUCCESS)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"push_panel_ack 回调结果：{callbackResult.Code}_{callbackResult.Message}";
            return targetResponse;
        }
        //第二步：给PLC下发上料指令（机构动起来）
        deviceServiceInvokeRequest.Params["PlcCommand"] = "push_panel";
        var deviceOperationResponse = await InteractingDevice.PlcOpertaion(deviceServiceInvokeRequest);
        var errormsg = string.Format($"push_panel：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
        if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
        {
            deviceOperationResponse.Message = errormsg;
            await CommandReport(targetResponse, 0);
            return deviceOperationResponse;
        }

        targetResponse.Code = ErrorCodes.Sys.SUCCESS;
        targetResponse.Message = "AGV上料指令push_panel开始执行";
        var reportResult = await CommandReport(targetResponse, 1);
        if (reportResult.Code != ErrorCodes.Sys.SUCCESS)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"push_panel_report 回调结果：{reportResult.Code}_{reportResult.Message}";
            return targetResponse;
        }
        return targetResponse;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="targetResponse">指令结果</param>
    /// <returns></returns>
    private async Task<DeviceServiceInvokeResponse> CommandCallback(DeviceServiceInvokeResponse targetResponse)
    {
        var reportresponse = new DeviceServiceInvokeResponse();
        var agvAdjustHeightAckPayload = new AgvPushPanelAckPayload()
        {
            header = new HeaderEntity()
            {
                signature = _xianJinIotOptions.Signature,
                signCode = _xianJinIotOptions.SignCode
            },
            body = new AckBodyEntity()
            {
                sn = InteractingDevice.DeviceId,
                timestamp = DateTime.Now.ToBinary(),
                taskCode = _xianJinIotOptions.TaskCode,
                code = targetResponse.Code == ErrorCodes.Sys.SUCCESS ? 200 : -1,
                msg = targetResponse.Code == ErrorCodes.Sys.SUCCESS ? "已收到指令" : targetResponse.Message,
            }
        };
        var reportresult = await _mqttClient.PublishBinaryAsync("jlc/mes/report/agv/push_panel_ack", JsonSerializer.Serialize(agvAdjustHeightAckPayload).GetBytes(), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
        var message = $"push_panel_ack 上报结果：{reportresult.IsSuccess}_ReasonCode：{reportresult.ReasonCode}_ReasonString：{reportresult.ReasonString}";
        logger.LogDebug(message);
        reportresponse.Code = reportresult.IsSuccess ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL;
        reportresponse.Message = message;
        return reportresponse;
    }

    private async Task<DeviceServiceInvokeResponse> CommandReport(DeviceServiceInvokeResponse targetResponse, int status)
    {
        var reportresponse = new DeviceServiceInvokeResponse();
        var agvWalkLocationPayload = new DrillPushPanelReportPayload()
        {
            header = new HeaderEntity()
            {
                signature = _xianJinIotOptions.Signature,
                signCode = _xianJinIotOptions.SignCode
            },
            body = new ReportBodyEntity()
            {
                sn = InteractingDevice.DeviceId,
                timestamp = DateTime.Now.ToBinary(),
                taskCode = _xianJinIotOptions.TaskCode,
                status = status,
                msg = targetResponse.Code == ErrorCodes.Sys.SUCCESS ? "已完成指令" : targetResponse.Message,
            }
        };
        var reportresult = await _mqttClient.PublishBinaryAsync("jlc/mes/report/drilling/receive_panel_reoprt", JsonSerializer.Serialize(agvWalkLocationPayload).GetBytes(), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
        var message = $"receive_panel_reoprt 上报结果：{reportresult.IsSuccess}_ReasonCode:{reportresult.ReasonCode}";
        logger.LogDebug(message);
        reportresponse.Code = reportresult.IsSuccess ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL;
        reportresponse.Message = message;
        return reportresponse;
    }

    public async Task<DeviceServiceInvokeResponse> InvokeAndCompleteforLoad(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType operation)
    {
        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Code = ErrorCodes.Sys.FAIL;
        targetResponse.Message = "暂未开始逻辑";
        if (operation != DeviceOperationType.InvokeLoadMaterial || operation != DeviceOperationType.CompleteLoadMaterial)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"InvokeAndCompleteforLoad 内 operation类型传输不正确：{operation}";
            return targetResponse;
        }
        var isexistPayloadSegment = targetResponse.Params.ContainsKey("PayloadSegment");
        if (!isexistPayloadSegment || deviceServiceInvokeRequest.Params["PayloadSegment"] == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = "PayloadSegment 参数不正确";
            return targetResponse;
        }

        var receiveMaterialsPayload = JsonSerializer.Deserialize<DrillReceivePanelPayload>(targetResponse.Params["PayloadSegment"].ToString());

        var startTime = DateTime.Now;
        var isTimeOut = false;
        var isFinishedWork = false;
        var errorMsg = "";
        var postAndGetTimeout = 180; //configExtra["PostAndGetTimeout"].ToInt(); todo

        while (true)
        {
            if (operation == DeviceOperationType.InvokeLoadMaterial)
            {
                targetResponse = await InteractingDevice.InvokeLoadMaterial(deviceServiceInvokeRequest);
            }
            else if (operation == DeviceOperationType.InvokeLoadMaterial)
            {
                targetResponse = await InteractingDevice.CompleteLoadMaterial(deviceServiceInvokeRequest);
            }

            var isexist = targetResponse.Params.ContainsKey("DeviceIsException");
            if (isexist && targetResponse.Params["DeviceIsException"].ToBool())
            {
                InteractingDevice.Status = DeviceStatus.Exception;
                errorMsg = $"\r\n 等待 设备存在异常结束isexist:{isexist},{targetResponse.Message} \r\n";
                //logger.LogDebug(errorMsg);
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = errorMsg;
                break;
            }

            if (targetResponse.Code == ErrorCodes.Sys.SUCCESS)
            {
                break;
            }
            isTimeOut = InteractingDevice.IsTimeout(startTime, postAndGetTimeout);
            if (isTimeOut)//超时或者 异常
            {
                InteractingDevice.Status = DeviceStatus.Exception;
                //logger.LogDebug($"\r\n 等待 postAndGet 信号超时：{response.Message}\r\n");
                errorMsg = $"\r\n 等待 postAndGet 信号超时：{targetResponse.Message}\r\n";
                break;
            }
            isFinishedWork = InteractingDevice.CanProceedNextStep();
            if (isFinishedWork)// 异常
            {
                errorMsg = $"\r\n 程序异常跳出循环postAndGet：{targetResponse.Message} \r\n";
                //logger.LogDebug($"\r\n 程序异常跳出循环postAndGet：{response.Message} \r\n");
                break;
            }
            await Task.Delay(1000);
        }
        targetResponse.Message = errorMsg;
        return targetResponse;
    }
}
