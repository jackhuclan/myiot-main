using System.Text.Json;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using VegaIot.External.XianjinIot.Models;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Agv;

namespace VegaIot.External.XianjinIot.Commands.AGV;

internal class AgvWalkLocationCommand : SimpleCommand<DefaultAgv>
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotOptions _xianJinIotOptions;

    public AgvWalkLocationCommand(
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

        var agvWalkLocationPayload = JsonSerializer.Deserialize<AgvWalkLocationPayload>(targetResponse.Params["PayloadSegment"].ToString());
        if (agvWalkLocationPayload == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"createTaskPayload实体转换出错：NUll";
            await CommandCallback(targetResponse);
            return targetResponse;
        }
        await CommandCallback(targetResponse);

        deviceServiceInvokeRequest.Params["MoveTargetPos"] = agvWalkLocationPayload.body.coordinate;
        deviceServiceInvokeRequest.Params["CheckMovePoint"] = InteractingDevice.configExtra["CheckMovePoint"];
        deviceServiceInvokeRequest.Params["Unlock"] = false;
        deviceServiceInvokeRequest.Params["count"] = "0";
        var taskCode = "VegaMove" + InteractingDevice.DeviceId + DateTime.Now.ToString("yyyyMMddHHmmss");
        var reqCode = taskCode + "_" + 0.ToString();
        deviceServiceInvokeRequest.Params["taskCode"] = taskCode;
        var deviceOperationResponse = await InteractingDevice.Move(deviceServiceInvokeRequest);
        var errormsg = string.Format($"Move：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
        //logger.LogDebug(errormsg);
        if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
        {
            deviceOperationResponse.Message = errormsg;
            await CommandReport(targetResponse, 0);
            return deviceOperationResponse;
        }
        InteractingDevice.failTaskCode = taskCode;

        deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, deviceServiceInvokeRequest, taskCode);
        errormsg = string.Format($"AGV到达{taskCode}：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
        InteractingDevice.sendCancelBeforeArrivedDevice = false;

        if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
        {
            deviceOperationResponse.Message = errormsg;
            await CommandReport(targetResponse, 0);
            return deviceOperationResponse;
        }

        targetResponse.Code = ErrorCodes.Sys.SUCCESS;
        targetResponse.Message = "移动完成";
        await CommandReport(targetResponse, 1);
        return targetResponse;
    }

    private async Task CommandCallback(DeviceServiceInvokeResponse targetResponse)
    {
        var agvWalkLocationPayload = new AgvWalkLocationAckPayload()
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
        await _mqttClient.PublishBinaryAsync("jlc/mes/report/agv/walk_location_ack", JsonSerializer.Serialize(agvWalkLocationPayload).GetBytes(), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
    }

    private async Task CommandReport(DeviceServiceInvokeResponse targetResponse, int status)
    {
        var agvWalkLocationPayload = new AgvWalkLocationReoprtPayload()
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
                msg = targetResponse.Code == ErrorCodes.Sys.SUCCESS ? "移动到位" : targetResponse.Message,
            }
        };
        await _mqttClient.PublishBinaryAsync("jlc/mes/report/agv/walk_location_reoprt", JsonSerializer.Serialize(agvWalkLocationPayload).GetBytes(), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
    }
}
