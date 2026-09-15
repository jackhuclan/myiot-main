using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using VegaIot.External.XianjinIot.Models;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Agv.AgvDevice;
using VgDeviceGateway.Devices.Drill;

namespace VegaIot.External.XianjinIot.Commands.Drill;

internal class DrillReceivePanelCommand : SimpleCommand<DefaultDrill>
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotOptions _xianJinIotOptions;
    private readonly ILogger<BackPanelAgv> logger;

    public DrillReceivePanelCommand(IServiceProvider serviceProvider,
             ILogger<BackPanelAgv> logger,
        IMqttClient mqttClient,
         IOptions<XianJinIotOptions> options,
        DefaultDrill device,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _xianJinIotOptions = options.Value;
        this.logger = logger;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Code = ErrorCodes.Sys.FAIL;
        targetResponse.Message = "暂未开始逻辑";
        var startTime = DateTime.Now;
        var isTimeOut = false;
        var isFinishedWork = false;
        var errorMsg = "";
        var postAndGetTimeout = 180; //configExtra["PostAndGetTimeout"].ToInt(); todo
        var isexistPayloadSegment = targetResponse.Params.ContainsKey("PayloadSegment");
        if (!isexistPayloadSegment || deviceServiceInvokeRequest.Params["PayloadSegment"] == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"PayloadSegment 参数不正确：{deviceServiceInvokeRequest.Params["PayloadSegment"]}";
            await CommandCallback(targetResponse);
            return targetResponse;
        }

        var drillReceivePanelPayload = JsonSerializer.Deserialize<DrillReceivePanelPayload>(targetResponse.Params["PayloadSegment"].ToString());
        if (drillReceivePanelPayload == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"DrillReceivePanelPayload 实体转换出错：Null";
            await CommandCallback(targetResponse);
            return targetResponse;
        }
        //第一步：收到上料命令,并回调
        targetResponse.Message = "已收到receive_panel指令";
        var callbackResult = await CommandCallback(targetResponse);
        if (callbackResult.Code != ErrorCodes.Sys.SUCCESS)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"receive_panel_ack 回调结果：{callbackResult.Code}_{callbackResult.Message}";
            return targetResponse;
        }
        //调用钻机PRE
        while (true)
        {
            targetResponse = await InteractingDevice.PrepareLoadMaterial(deviceServiceInvokeRequest);

            var isexist = targetResponse.Params.ContainsKey("DeviceIsException");
            if (isexist && targetResponse.Params["DeviceIsException"].ToBool())
            {
                InteractingDevice.Status = DeviceStatus.Exception;
                errorMsg = $"\r\n 等待 设备存在异常结束isexist:{isexist},{targetResponse.Message} \r\n";
                logger.LogDebug(errorMsg);
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
                errorMsg = $"\r\n 等待 postAndGet 信号超时：{targetResponse.Message}\r\n";
                logger.LogDebug(errorMsg);
                break;
            }
            isFinishedWork = InteractingDevice.CanProceedNextStep();
            if (isFinishedWork)// 异常
            {
                errorMsg = $"\r\n 程序异常跳出循环postAndGet：{targetResponse.Message} \r\n";
                logger.LogDebug(errorMsg);
                break;
            }
            await Task.Delay(1000);
        }

        if (targetResponse.Code != ErrorCodes.Sys.SUCCESS)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = errorMsg;
            await CommandReport(targetResponse, 0);
            return targetResponse;
        }
        targetResponse.Message = "钻机准备接料指令receive_panel执行完成";
        targetResponse.Code = ErrorCodes.Sys.SUCCESS;
        var reportResult = await CommandReport(targetResponse, 1);
        if (reportResult.Code != ErrorCodes.Sys.SUCCESS)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"receive_panel_report 回调结果：{reportResult.Code}_{reportResult.Message}";
            return targetResponse;
        }
        return targetResponse;
    }

    private async Task<DeviceServiceInvokeResponse> CommandCallback(DeviceServiceInvokeResponse targetResponse)
    {
        var reportresponse = new DeviceServiceInvokeResponse();
        var receiveMaterialsAckPayload = new DrillReceivePanelAckPayload()
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
                msg = targetResponse.Code == ErrorCodes.Sys.SUCCESS ? "处理成功" : targetResponse.Message,
            }
        };

        var reportresult = await _mqttClient.PublishBinaryAsync("jlc/mes/report/drilling/receive_panel_ack", JsonSerializer.Serialize(receiveMaterialsAckPayload).GetBytes(), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
        var message = $"receive_panel_ack 上报结果：{reportresult.IsSuccess}_ReasonCode：{reportresult.ReasonCode}_ReasonString：{reportresult.ReasonString}";
        logger.LogDebug(message);
        reportresponse.Code = reportresult.IsSuccess ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL;
        reportresponse.Message = message;
        return reportresponse;
    }

    /// <summary>
    /// 0-未知异常，1-完成，2-皮带卡带
    /// </summary>
    /// <param name="targetResponse"></param>
    /// <returns></returns>
    private async Task<DeviceServiceInvokeResponse> CommandReport(DeviceServiceInvokeResponse targetResponse, int status)
    {
        var reportresponse = new DeviceServiceInvokeResponse();
        var drillReceivePanelReportPayload = new DrillReceivePanelReportPayload()
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
                msg = targetResponse.Message,
            }
        };

        var reportresult = await _mqttClient.PublishBinaryAsync("jlc/mes/report/drilling/receive_panel_reoprt", JsonSerializer.Serialize(drillReceivePanelReportPayload).GetBytes(), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
        var message = $"receive_panel_reoprt 上报结果：{reportresult.IsSuccess}_ReasonCode:{reportresult.ReasonCode}";
        logger.LogDebug(message);
        reportresponse.Code = reportresult.IsSuccess ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL;
        reportresponse.Message = message;
        return reportresponse;
    }
}
