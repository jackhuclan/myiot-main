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
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Utils;

namespace Vegalot.External.XianjinIot.Agv.Command;

/// <summary>
/// 上报设备告警
/// </summary>
internal class AgvReportErrorCommand : DeviceShare<XianjInIotDefaultAgv>
{
    private readonly IMqttClient _mqttClient;
    private readonly ILogger<AgvReportErrorCommand> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="device"></param>
    /// <param name="logger"></param>
    public AgvReportErrorCommand(IServiceProvider serviceProvider,
        XianjInIotDefaultAgv device,
        ILogger<AgvReportErrorCommand> logger)
        : base(serviceProvider, device)
    {
        _logger = logger;
    }

    public async void Handle()
    {
        //02 - 219 上报设备报警
        _logger.LogInformation($"上报设备报警信息\r\n");

        byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveId"].ToInt();
        var waitPlc = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4013, 1);
        _logger.LogInformation($"读取PLC4013备报警信息完成\r\n");
        var agvReportErrorPayload = new AgvReportErrorPayload()
        {
            body = new AgvReportErrorBody()
            {
                sn = InteractingDevice.DeviceId,
                code = 200,
                msg = waitPlc[0].ToStr(),
            }
        };
        if (_mqttClient.IsConnected)
        {
            //await _mqttClient.PublishStringAsyncEnhance(IotTopic.RECEIVE_UNIT_ERROR, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(agvReportErrorPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
            await _mqttClient.PublishStringAsyncEnhance(IotTopic.RECEIVE_UNIT_ERROR, agvReportErrorPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
            _logger.LogInformation($"给MES发送备报警信息完成\r\n");
        }
        else
        {
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 3051, 1);
            _logger.LogInformation($"给MES发送备报警信息时MQTT断开连接\r\n");
        }
        return;
    }

    public async void upLoadErr(string errMsg)
    {
        //02 - 219 上报设备报警
        _logger.LogInformation($"上报设备报警信息{errMsg}\r\n");
        var agvReportErrorPayload = new AgvReportErrorPayload()
        {
            body = new AgvReportErrorBody()
            {
                sn = InteractingDevice.DeviceId,
                code = 200,
                msg = errMsg,
            }
        };
        await _mqttClient.PublishStringAsyncEnhance(IotTopic.RECEIVE_UNIT_ERROR,agvReportErrorPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
        return;
    }
}
