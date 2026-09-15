// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Modbus.Device;
using MQTTnet.Client;
using Newtonsoft.Json.Linq;
using NModbus.Device;
using Vegalot.External.XianjinIot.Agv.Command;
using Vegalot.External.XianjinIot.Agv.Models.Report;
using Vegalot.External.XianjinIot.Common;
using Vegalot.External.XianjinIot.Common.Manager;
using Vegalot.External.XianjinIot.Common.Models;
using Vegalot.External.XianjinIot.Common.Models.Report;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;

namespace Vegalot.External.XianjinIot.Agv;

internal class XianJinIotAgvClientMqttListener : DefaultClientMqttListener
{
    private readonly IMqttClient _mqttClient;
    private readonly IObjectFactory _objectFactory;
    private readonly XianJinIotAgvOptions _xinjinIotDrillOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<XianJinIotAgvClientMqttListener> _logger;
    private bool flag = true;
    private IModbusMaster modbusIpMaster;

    public XianJinIotAgvClientMqttListener(IClientMqttApplicationMessageListener applicationMessageListner,
        IMqttClient mqttClient,
        IObjectFactory objectFactory,
         IServiceProvider serviceProvider,
        IOptions<XianJinIotAgvOptions> xinjinIotDrillOptions,
        ILoggerFactory loggerFactory)
        : base(applicationMessageListner, mqttClient, loggerFactory)
    {
        _mqttClient = mqttClient;
        _serviceProvider = serviceProvider;
        _objectFactory = objectFactory;
        _xinjinIotDrillOptions = xinjinIotDrillOptions.Value;
        _logger = loggerFactory.CreateLogger<XianJinIotAgvClientMqttListener>();
    }

    public override async Task OnConnected(Device device)
    {
        try
        {
            _logger.LogInformation("XianJinIotAgvClientMqttListener OnConnected 进入");
            CommonModel.sn = device.DeviceId;
            _xinjinIotDrillOptions.DeviceId = device.DeviceId;

            var publicKeyAckCommand = _objectFactory.GetOrCreate<PublicKeyAckCommand>(device,
                new CommandDescriptor("public_key_ack", $"jlc/mes/issued/{device.DeviceId}/public_key_ack", CommandUsageKind.Both));
            var registAckCommand = _objectFactory.GetOrCreate<RegistAckCommand>(device,
                new CommandDescriptor("regist_ack", $"jlc/mes/issued/{device.DeviceId}/regist_ack", CommandUsageKind.Both));
            var configCommand = _objectFactory.GetOrCreate<ConfigCommand>(device,
                new CommandDescriptor("config", $"jlc/mes/issued/{device.DeviceId}/config", CommandUsageKind.Both));
            var oTAUpgradeCommand = _objectFactory.GetOrCreate<AOTUpgradeCommand>(device,
                new CommandDescriptor("OTA_upgrade", $"jlc/mes/issued/{device.DeviceId}/OTA_upgrade", CommandUsageKind.Both));


            var agvAdjustHeightCommand = _objectFactory.GetOrCreate<AgvAdjustHeightCommand>(device,
                new CommandDescriptor("height", $"jlc/mes/issued/agv/{device.DeviceId}/adjust_height", CommandUsageKind.Both));
            var agvUpHeightNumberCommand = _objectFactory.GetOrCreate<AgvUpHeightNumberCommand>(device,
                new CommandDescriptor("heightnumber", $"jlc/mes/issued/agv/{device.DeviceId}/height_number", CommandUsageKind.Both));
            var agvLoadSiloCommand = _objectFactory.GetOrCreate<AgvLoadSiloCommand>(device,
                new CommandDescriptor("loadsilo", $"jlc/mes/issued/agv/{device.DeviceId}/load_silo", CommandUsageKind.Both));
            var agvUnloadSiloCommand = _objectFactory.GetOrCreate<AgvUnloadSiloCommand>(device,
                new CommandDescriptor("unloadsilo", $"jlc/mes/issued/agv/{device.DeviceId}/unload_silo", CommandUsageKind.Both));
            var agvPushPanelCommand = _objectFactory.GetOrCreate<AgvPushPanelCommand>(device,
                new CommandDescriptor("pushpanel", $"jlc/mes/issued/agv/{device.DeviceId}/push_panel", CommandUsageKind.Both));
            var agvReceivePanelCommand = _objectFactory.GetOrCreate<AgvReceivePanelCommand>(device,
                new CommandDescriptor("receivepanel", $"jlc/mes/issued/agv/{device.DeviceId}/receive_panel", CommandUsageKind.Both));
            var agvStopWorkingCommand = _objectFactory.GetOrCreate<AgvStopWorkingCommand>(device,
                new CommandDescriptor("stopworking", $"jlc/mes/issued/agv/{device.DeviceId}/stop_working", CommandUsageKind.Both));
            var agvLockingUnit900Command = _objectFactory.GetOrCreate<AgvLockingUnit900Command>(device,
                new CommandDescriptor("900lockingunit", $"jlc/mes/issued/agv/{device.DeviceId}/900_locking_unit", CommandUsageKind.Both));
            var agvLockingUnit950Command = _objectFactory.GetOrCreate<AgvLockingUnit950Command>(device,
                new CommandDescriptor("950lockingunit", $"jlc/mes/issued/agv/{device.DeviceId}/950_locking_unit", CommandUsageKind.Both));

            await SubscribeService(publicKeyAckCommand);
            await SubscribeService(registAckCommand);
            await SubscribeService(configCommand);
            await SubscribeService(oTAUpgradeCommand);

            await SubscribeService(agvAdjustHeightCommand);
            await SubscribeService(agvUpHeightNumberCommand);
            await SubscribeService(agvLoadSiloCommand);
            await SubscribeService(agvUnloadSiloCommand);
            await SubscribeService(agvPushPanelCommand);
            await SubscribeService(agvReceivePanelCommand);
            await SubscribeService(agvStopWorkingCommand);
            await SubscribeService(agvLockingUnit900Command);
            await SubscribeService(agvLockingUnit950Command);

            //请求公钥
            await ReportPublicKey(device);
        }
        catch (Exception ee)
        {
            _logger.LogInformation($"XianJinIotAgvClientMqttListener OnConnected {ee.Message}");
        }
        finally
        {
            _logger.LogInformation("XianJinIotAgvClientMqttListener OnConnected 结束");
        }
    }

    private async Task ReportPublicKey(Device device)
    {
        PublicKeyPayLoad publicKeyPayLoad = new PublicKeyPayLoad()
        {
            body = new ReportBaseBodyEntity()
            {
                sn = device.DeviceId
            }
        };

        _logger.LogInformation($"01-201 设备:{device.DeviceId} 上报请求公钥  实体 {JsonSerializer.Serialize(publicKeyPayLoad, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        })} ");

        await _mqttClient.PublishStringAsyncEnhance(IotTopic.PUBLIC_KEY_TOPIC, publicKeyPayLoad, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce,false);
    }

    public override async Task OnDisonnected(Device device)
    {

        try
        {
            _logger.LogInformation("XianJinIotDrillClientMqttListener OnDisonnected 进入");
            await PLCAlarm(device, 1);
            CommonModel.Key = string.Empty;
            CommonModel.PublicKey = string.Empty;
            flag = false;
        }
        finally
        {
            _logger.LogInformation("XianJinIotDrillClientMqttListener OnDisonnected 结束");
        }

    }

    /// <summary>
    /// 心跳检测(2秒执行一次(可配置))
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    public override async Task<bool> OnHeartbeat(Device device)
    {
        _logger.LogInformation($"_mqttClient连接状态：{_mqttClient.IsConnected}");
        await PLCAlarm(device, 0);       
        await AgvReportState(device);//监听agv给Mes上报agv设备手动/自动状态
        await AgvReportError(device);//监听agv给Mes上报agv报错信息
        return true;
    }

    /// <summary>
    /// PLC警报0、正常  1、报警
    /// </summary>
    /// <param name="device"></param>
    /// <param name="value">0、正常  1、报警</param>
    /// <returns></returns>
    private async Task PLCAlarm(Device device, ushort value = 0)
    {
        modbusIpMaster = await PLCConnect(device);
        var slaveID = (byte)device.DeviceDescriptor.Extra["SlaveId"].ToInt();
        modbusIpMaster.WriteSingleRegister(slaveID, 3051, value);//value=1,AGV报警
        if (value == 0)//value=0正常，PLC红心0、1闪烁
        {
            for (var i = 0; i < 8; i++)//250ms的闪烁间隔
            {
                modbusIpMaster.WriteSingleRegister(slaveID, 3053, (ushort)(i % 2 == 0 ? 0 : 1));
                await Task.Delay(250);
            }
        }
        await Task.CompletedTask;
    }


    private async Task<IModbusMaster> PLCConnect(Device device)
    {
        if (!string.IsNullOrEmpty(device.DeviceDescriptor.Extra["ModbusTcpUri"].ToStr()))
        {
            if (modbusIpMaster == null)
            {
                var plcUri = new Uri(device.DeviceDescriptor.Extra["ModbusTcpUri"].ToStr());
                modbusIpMaster = ((IModbusOperator)device.Engine.DeviceConnector).ModbusIpMasterWrapper.CreateIp(plcUri.Host, plcUri.Port);
            }       
        }
        await Task.CompletedTask;
        return modbusIpMaster;
     }


    public async Task AgvReportError(Device device)
    {
        modbusIpMaster = await PLCConnect(device);
        var slaveID = (byte)device.DeviceDescriptor.Extra["SlaveId"].ToInt();
        var value = modbusIpMaster.ReadHoldingRegisters(slaveID, 4013, 1);
        if (value[0] != 1)
        {
            return;
        }
        _logger.LogInformation($"读取PLC4013备报警信息完成:{value[0]}");
        var agvReportErrorPayload = new AgvReportErrorPayload()
        {
            body = new AgvReportErrorBody()
            {
                sn = device.DeviceId,
                code = 200,
                msg = value[0].ToStr(),
            }
        };
        await _mqttClient.PublishStringAsyncEnhance(IotTopic.RECEIVE_UNIT_ERROR, agvReportErrorPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce,true,false);
        _logger.LogInformation($"给MES发送备报警信息完成\r\n");
        return;
    }


    public async Task AgvReportState(Device device)
    {
        modbusIpMaster = await PLCConnect(device);
        var slaveID = (byte)device.DeviceDescriptor.Extra["SlaveId"].ToInt();
        var value = modbusIpMaster.ReadHoldingRegisters(slaveID, 4011, 1);
        _logger.LogInformation($"读取PLC4011设备状态信息完成:{value[0]}");
        var agvReportErrorPayload = new AgvReportStatePayload()
        {
            body = new AgvReportStateBody()
            {
                sn = device.DeviceId,
                receiveUnitState = value[0].ToInt(),
                msg = value[0].ToStr(),
            }
        };
        await _mqttClient.PublishStringAsyncEnhance(IotTopic.RECEIVE_UNIT_STATE, agvReportErrorPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce,true,false);
        _logger.LogInformation($"给MES上传设备状态完成\r\n");
        return;
    }

}
