// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using VegaIot.External.XianjinIot.Commands;
using VegaIot.External.XianjinIot.Commands.AGV;
using VegaIot.External.XianjinIot.Commands.Drill;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Infrastructure;

namespace VegaIot.External.XianjinIot;

internal class XianJinIotClientMqttListener : DefaultClientMqttListener
{
    private readonly XianJinIotOptions _xianJinIotOptions;
    private readonly IMqttClient _mqttClient;
    private readonly IObjectFactory _objectFactory;

    public XianJinIotClientMqttListener(XianjinIClientMqttApplicationMessageListner applicationMessageListner,
        IMqttClient mqttClient,
        IOptions<XianJinIotOptions> options2,
        IObjectFactory objectFactory,
        ILoggerFactory loggerFactory)
        : base(applicationMessageListner, mqttClient, loggerFactory)
    {
        _xianJinIotOptions = options2.Value;
        _mqttClient = mqttClient;
        _objectFactory = objectFactory;
    }

    public override async Task OnConnected(Device device)
    {
        _xianJinIotOptions.DeviceId = device.DeviceId;
        var drillingCreateTaskCallback = _objectFactory.GetOrCreate<DrillCreateTaskCommand>(device);
        var configCallback = _objectFactory.GetOrCreate<ConfigCallback>(device);
        var oTAUpgradeCallback = _objectFactory.GetOrCreate<OTAUpgradeCallback>(device);
        var publicKeyAckCallback = _objectFactory.GetOrCreate<PublicKeyAckCallback>(device);
        var registAckCallback = _objectFactory.GetOrCreate<RegistAckCallback>(device);

        var preUnloadMaterialCommand = _objectFactory.GetOrCreate<DrillPushPanelCommand>(device);
        var createTaskCommand = _objectFactory.GetOrCreate<DrillCreateTaskCommand>(device);

        var agvwalkLocationCommand = _objectFactory.GetOrCreate<AgvWalkLocationCommand>(device);
        var agvAdjustHeightCommand = _objectFactory.GetOrCreate<AgvAdjustHeightCommand>(device);
        var agvLoadSiloCommand = _objectFactory.GetOrCreate<AgvLoadSiloCommand>(device);
        var agvUpSiloFloorCommand = _objectFactory.GetOrCreate<AgvUpSiloFloorCommand>(device);
        var agvPushPanelCommand = _objectFactory.GetOrCreate<AgvPushPanelCommand>(device);
        var agvReceivePanelCommand = _objectFactory.GetOrCreate<AgvReceivePanelCommand>(device);
        var agvUnloadSiloCommand = _objectFactory.GetOrCreate<AgvUnloadSiloCommand>(device);

        var drillReceivePanelCommand = _objectFactory.GetOrCreate<DrillReceivePanelCommand>(device);
        var drillCreateTaskCommand = _objectFactory.GetOrCreate<DrillCreateTaskCommand>(device);
        var drillLoadDrillingFileCommand = _objectFactory.GetOrCreate<DrillLoadDrillingFileCommand>(device);
        var drillStartDrillingCommand = _objectFactory.GetOrCreate<DrillStartDrillingCommand>(device);
        var drillPushPanelCommand = _objectFactory.GetOrCreate<DrillPushPanelCommand>(device);

        await SubscribeService($"jlc/mes/lssued/{device.DeviceId}/public_key_ack", publicKeyAckCallback.Invoke);
        await SubscribeService($"jlc/mes/lssued/{device.DeviceId}/regist_ack", registAckCallback.Invoke);
        await SubscribeService($"jlc/mes/lssued/{device.DeviceId}/config", configCallback.Invoke);
        await SubscribeService($"jlc/mes/lssued/{device.DeviceId}/OTA_upgrade", oTAUpgradeCallback.Invoke);

        if (DeviceKindExtensions.IsAGV(device.DeviceDescriptor.DeviceKind))
        {
            await SubscribeService($"jlc/mes/lssued/agv/{device.DeviceId}/walk_location", agvwalkLocationCommand.Invoke);
            await SubscribeService($"jlc/mes/lssued/agv/{device.DeviceId}/adjust_height", agvAdjustHeightCommand.Invoke);
            await SubscribeService($"jlc/mes/lssued/agv/{device.DeviceId}/load_silo", agvLoadSiloCommand.Invoke);
            await SubscribeService($"jlc/mes/lssued/agv/{device.DeviceId}/up_silo_floor", agvUpSiloFloorCommand.Invoke);
            await SubscribeService($"jlc/mes/lssued/agv/{device.DeviceId}/push_panel", agvPushPanelCommand.Invoke);
            await SubscribeService($"jlc/mes/lssued/agv/{device.DeviceId}/receive_panel", agvReceivePanelCommand.Invoke);
            await SubscribeService($"jlc/mes/lssued/agv/{device.DeviceId}/unload_silo", agvUnloadSiloCommand.Invoke);
        }

        if (DeviceKindExtensions.IsDrill(device.DeviceDescriptor.DeviceKind))
        {
            await SubscribeService($"jlc/mes/issued/drilling/{device.DeviceId}/receive_panel", drillReceivePanelCommand.Invoke);
            await SubscribeService($"jlc/mes/issued/drilling/{device.DeviceId}/create_task", drillCreateTaskCommand.Invoke);
            await SubscribeService($"jlc/mes/issued/drilling/{device.DeviceId}/load_drilling_file", drillLoadDrillingFileCommand.Invoke);
            await SubscribeService($"jlc/mes/issued/drilling/{device.DeviceId}/start_drilling", drillStartDrillingCommand.Invoke);
            await SubscribeService($"jlc/mes/issued/drilling/{device.DeviceId}/push_panel", drillPushPanelCommand.Invoke);
        }

        //_mqttClient.PublishBinaryAsync("jlc/mes/report/public_key",)
        //_mqttClient.PublishBinaryAsync("jlc/mes/report/regist",)
        //_mqttClient.PublishBinaryAsync("jlc/mes/report/device_info",)
        //_mqttClient.PublishBinaryAsync("jlc/mes/report/OTA_upgrade_ack",)
    }

    public override Task OnDisonnected(Device device)
    {
        return Task.CompletedTask;
    }

    public override Task<bool> OnHeartbeat(Device device) => throw new NotImplementedException();
}
