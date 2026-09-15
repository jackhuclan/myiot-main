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

internal class DrillingTaskReoprtCommand : SimpleCommand<DefaultDrill>
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotOptions _xianJinIotOptions;

    public DrillingTaskReoprtCommand(
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
        PeriodicTimers["5s"]!.OnTick += async () =>
        {
            var properties = InteractingDevice.ReadProperties(deviceServiceInvokeRequest).Result;
            //不等于打板结束就上报进度
            if (!properties.Params["Drill_DrillHoleEnd"].ToBool())
            {
                targetResponse.Code = ErrorCodes.Sys.SUCCESS;
                targetResponse.Message = "上报进度";
                await CommandReport(targetResponse, 1, properties.Params["Drill_Percentage"].ToInt());
            }
        };

        targetResponse.Code = ErrorCodes.Sys.SUCCESS;
        targetResponse.Message = "";
        return targetResponse;
    }

    /// <summary>
    /// 上报进度
    /// </summary>
    /// <param name="targetResponse"></param>
    /// <param name="status">0-异常,1-进行中,2-暂停,3-换刀,4-完成</param>
    /// <returns></returns>
    private async Task CommandReport(DeviceServiceInvokeResponse targetResponse, int status, int schedule)
    {
        var agvWalkLocationPayload = new DrillingTaskReoprtPayload()
        {
            header = new HeaderEntity()
            {
                signature = _xianJinIotOptions.Signature,
                signCode = _xianJinIotOptions.SignCode
            },
            body = new DrillingTaskReoprtbody()
            {
                sn = InteractingDevice.DeviceId,
                timestamp = DateTime.Now.ToBinary(),
                taskCode = _xianJinIotOptions.TaskCode,
                status = status,
                msg = targetResponse.Code == ErrorCodes.Sys.SUCCESS ? "移动到位" : targetResponse.Message,
                schedule = schedule
            }
        };
        await _mqttClient.PublishBinaryAsync("jlc/mes/report/drilling/drilling_task_reoprt", JsonSerializer.Serialize(agvWalkLocationPayload).GetBytes(), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
    }
}
