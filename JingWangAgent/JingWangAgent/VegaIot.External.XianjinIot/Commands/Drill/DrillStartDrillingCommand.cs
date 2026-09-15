using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using VegaIot.External.XianjinIot.Models;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Drill;

namespace VegaIot.External.XianjinIot.Commands.Drill;

internal class DrillStartDrillingCommand : SimpleCommand<DefaultDrill>
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotOptions _xianJinIotOptions;

    public DrillStartDrillingCommand(
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        IOptions<XianJinIotOptions> options,
        DefaultDrill device,
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
            await Callback(targetResponse);
            return targetResponse;
        }

        var createTaskPayload = JsonSerializer.Deserialize<DrillStartDrillingPayload>(targetResponse.Params["PayloadSegment"].ToString());
        if (createTaskPayload == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"createTaskPayload实体转换出错：{createTaskPayload}";
            await Callback(targetResponse);
            return targetResponse;
        }
        _xianJinIotOptions.TaskCode = createTaskPayload.body.taskCode;
        //todo 调用开始打板

        //回调先进接口
        await CommandReport(targetResponse, 1);

        targetResponse.Code = ErrorCodes.Sys.SUCCESS;
        targetResponse.Message = "创建完成";
        return targetResponse;
    }

    private async Task Callback(DeviceServiceInvokeResponse targetResponse)
    {
        var createTaskAckPayload = new DrillStartDrillingAckPayload()
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
        await _mqttClient.PublishBinaryAsync("jlc/mes/drilling/report/start_drilling_ack", JsonSerializer.Serialize(createTaskAckPayload).GetBytes(), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
    }

    private async Task CommandReport(DeviceServiceInvokeResponse targetResponse, int status)
    {
        var agvWalkLocationPayload = new DrillStartDrillingReportPayload()
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
        await _mqttClient.PublishBinaryAsync("jlc/mes/report/agv/start_drilling_reoprt", JsonSerializer.Serialize(agvWalkLocationPayload).GetBytes(), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
    }
}
