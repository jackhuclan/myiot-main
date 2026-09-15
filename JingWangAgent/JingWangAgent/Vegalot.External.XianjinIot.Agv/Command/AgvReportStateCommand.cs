// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using Vegalot.External.XianjinIot.Agv.Models.Report;
using Vegalot.External.XianjinIot.Common;
using Vegalot.External.XianjinIot.Common.Models;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;

namespace Vegalot.External.XianjinIot.Agv.Command;

/// <summary>
/// 上报设备状态
/// </summary>
internal class AgvReportStateCommand : DeviceShare<XianjInIotDefaultAgv>
{
    private readonly IMqttClient _mqttClient;
    private readonly ILogger<AgvReportStateCommand> _logger;

    public AgvReportStateCommand(IServiceProvider serviceProvider,
        XianjInIotDefaultAgv device,
        IMqttClient mqttClient,
        ILogger<AgvReportStateCommand> logger)
        : base(serviceProvider, device)
    {
        _mqttClient = mqttClient;
        _logger = logger;
    }

    public async void Handle()
    {
        //02 - 219 上报设备状态
        _logger.LogInformation($"上报设备状态信息\r\n");

        byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveId"].ToInt();
        var waitPlc = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4011, 1);
        _logger.LogInformation($"读取PLC4011设备状态信息完成\r\n");
        var agvReportErrorPayload = new AgvReportStatePayload()
        {
            body = new AgvReportStateBody()
            {
                sn = InteractingDevice.DeviceId,
                receiveUnitState = waitPlc[0].ToInt(),
                msg = waitPlc[0].ToStr(),
            }
        };
        if (_mqttClient.IsConnected)
        {
            await _mqttClient.PublishStringAsyncEnhance(IotTopic.RECEIVE_UNIT_STATE, agvReportErrorPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
            _logger.LogInformation($"给MES上传设备状态完成\r\n");
        }
        else
        {

            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 3051, 1);
            _logger.LogInformation($"给MES上传设备状态时MQTT断开连接\r\n");
        }
        return;
    }
}
