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

internal class AgvScheduledReoprtCommand : SimpleCommand<DefaultAgv>
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotOptions _xianJinIotOptions;

    public AgvScheduledReoprtCommand(
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
        PeriodicTimers["5s"]!.OnTick += async () =>
        {
            //var properties = InteractingDevice.ReadProperties(deviceServiceInvokeRequest).Result;

            targetResponse.Code = ErrorCodes.Sys.SUCCESS;
            targetResponse.Message = "上报进度";
            await CommandReport(targetResponse, 1);
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
    private async Task CommandReport(DeviceServiceInvokeResponse targetResponse, int schedule)
    {
        var agvScheduledPayload = new AgvScheduledPayload()
        {
            header = new HeaderEntity()
            {
                signature = _xianJinIotOptions.Signature,
                signCode = _xianJinIotOptions.SignCode
            },
            body = new AgvScheduledBody()
            {
                sn = InteractingDevice.DeviceId,
                taskCode = _xianJinIotOptions.TaskCode,
                schedule = schedule
            }
        };
        await _mqttClient.PublishBinaryAsync("jlc/mes/report/drilling/drilling_task_reoprt", JsonSerializer.Serialize(agvScheduledPayload).GetBytes(), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
    }
}
