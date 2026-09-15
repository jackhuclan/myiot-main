// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using Vegalot.External.XianjinIot.Drill.Command.Drill;
using Vegalot.External.XianjinIot.Drill.Command.Manager;
using Vegalot.External.XianjinIot.Drill.Models;
using Vegalot.External.XianjinIot.Drill.Models.Report;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Drill.Other.CodeReader;

namespace Vegalot.External.XianjinIot.Drill;

internal class XianJinIotDrillClientMqttListener : DefaultClientMqttListener
{
    private readonly IMqttClient _mqttClient;
    private readonly ReportInfo _reportInfo;
    private readonly IObjectFactory _objectFactory;
    private readonly XianJinIotDrillOptions _xinjinIotDrillOptions;
    private readonly ILogger<XianJinIotDrillClientMqttListener> _logger;

    public XianJinIotDrillClientMqttListener(IClientMqttApplicationMessageListener applicationMessageListner,
        IMqttClient mqttClient,
        IObjectFactory objectFactory,
        IOptions<XianJinIotDrillOptions> xinjinIotDrillOptions,
        ILoggerFactory loggerFactory)
        : base(applicationMessageListner, mqttClient, loggerFactory)
    {
        _mqttClient = mqttClient;
        _objectFactory = objectFactory;
        _reportInfo = objectFactory.GetOrCreate<ReportInfo>();
        _xinjinIotDrillOptions = xinjinIotDrillOptions.Value;
        _logger = loggerFactory.CreateLogger<XianJinIotDrillClientMqttListener>();
        CodeReaderBaseSetting.CodeReaderReceiveAction += _reportInfo.CodeReaderReceive;
    }

    public override async Task OnConnected(Device device)
    {
        try
        {
            _logger.LogInformation("XianJinIotDrillClientMqttListener OnConnected 进入");
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

            var drillReceivePanelCommand = _objectFactory.GetOrCreate<DrillReceivePanelCommand>(device,
                new CommandDescriptor("receive_panel", $"jlc/mes/issued/drilling/{device.DeviceId}/receive_panel", CommandUsageKind.Both));
            var drillCreateTaskCommand = _objectFactory.GetOrCreate<DrillCreateTaskCommand>(device,
                new CommandDescriptor("create_task", $"jlc/mes/issued/drilling/{device.DeviceId}/create_task", CommandUsageKind.Both));
            var drillLoadDrillingFileCommand = _objectFactory.GetOrCreate<DrillLoadDrillingFileCommand>(device,
                new CommandDescriptor("load_drilling_file", $"jlc/mes/issued/drilling/{device.DeviceId}/load_drilling_file", CommandUsageKind.Both));
            var drillStartDrillingCommand = _objectFactory.GetOrCreate<DrillStartDrillingCommand>(device,
                new CommandDescriptor("start_drilling", $"jlc/mes/issued/drilling/{device.DeviceId}/start_drilling", CommandUsageKind.Both));
            var drillPushPanelCommand = _objectFactory.GetOrCreate<DrillPushPanelCommand>(device,
                new CommandDescriptor("push_panel", $"jlc/mes/issued/drilling/{device.DeviceId}/push_panel", CommandUsageKind.Both));

            await SubscribeService(publicKeyAckCommand);
            await SubscribeService(registAckCommand);
            await SubscribeService(configCommand);
            await SubscribeService(oTAUpgradeCommand);

            await SubscribeService(drillReceivePanelCommand);
            await SubscribeService(drillCreateTaskCommand);
            await SubscribeService(drillLoadDrillingFileCommand);
            await SubscribeService(drillStartDrillingCommand);
            await SubscribeService(drillPushPanelCommand);

            //请求公钥
            await ReportPublicKey(device);
            ReportPanelAndtask(device);
            ReportDeviceInfAndtask(device);
        }
        catch (Exception ee)
        {
            _logger.LogInformation($"XianJinIotDrillClientMqttListener OnConnected {ee.Message}");
        }
        finally
        {
            _logger.LogInformation("XianJinIotDrillClientMqttListener OnConnected 结束");
        }
    }

    private void ReportDeviceInfAndtask(Device device)
    {
        Task.Factory.StartNew(async () =>
        {
            flag = true;
            _logger.LogError($"上报device_info  开始了 ");
            //WORK  STOP  ALAM  WAIT  IDLE SERV
            Dictionary<string, int> model = new Dictionary<string, int>()
            {
                { "WORK",3},
                { "STOP",4},
                { "ALAM",0},
                { "IDLE",1},
                { "SERV",0},
                { "WAIT",1},
                { "",0},
            };
            var oldPercentage = 0;
            while (flag)
            {
                try
                {
                    var values = device.WatchingProperties.GetValues();
                    var data = string.Join(",\r\n", values.Select(s => string.Join(":", s.Key, s.Value)));
                    _logger.LogInformation($"device_info  上报机器属性  {data} ");
                    //机器状态
                    var state = values["Drill_State"] != null ? values["Drill_State"]!.ToString() : string.Empty;
                    var status = model.ContainsKey(state) ? model[state] : 0;
                    var schedule = values["Drill_Percentage"] != null ? values["Drill_Percentage"].ToInt() : 0;
                    if (schedule != oldPercentage)
                    {
                        if (schedule == 0)
                        {
                            if (values["Drill_Percentage"] == null)
                            {
                                _logger.LogError($"device_info 获取不到进度属性  {data} ");
                                schedule = oldPercentage;
                            }
                            else
                            {
                                if (oldPercentage != 100)
                                {
                                    _logger.LogError($"device_info 中间手动点击从制定孔开始  {data} ");
                                    schedule = oldPercentage;
                                }
                            }

                        }

                        oldPercentage = schedule;
                    }

                    _logger.LogError($"device_info 钻机状态： deviceStatus {state}  上报的值 {status}  进度{schedule} ");


                    var drillDeviceInfoReportPayload = new DrillDeviceInfoReportPayload()
                    {
                        body = new DrillDeviceInfoReportBody()
                        {
                            sn = device.DeviceId,
                            deviceCode = device.DeviceId,
                            deviceStatus = status,
                            taskSchedule = schedule
                        }
                    };

                    drillDeviceInfoReportPayload.ModifyHeader();
                    _logger.LogError($"01-203 上报设备信息 {JsonSerializer.Serialize(drillDeviceInfoReportPayload, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                    })} ");
                    await _mqttClient.PublishStringAsyncEnhance(IotTopic.DEVICE_INFO_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillDeviceInfoReportPayload, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                    })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);

                    Thread.Sleep(CommonModel.ReportDeviceFrequency * 1000);
                }
                catch (Exception)
                {
                }
            }

            _logger.LogError($"上报 device_info 01-103  结束了 ");
        });
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
        _logger.LogInformation($"01-201 上报请求公钥  实体 {JsonSerializer.Serialize(publicKeyPayLoad, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        })} ");
        await _mqttClient.PublishStringAsyncEnhance(IotTopic.PUBLIC_KEY_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(publicKeyPayLoad, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
    }

    private bool flag = true;

    private void ReportPanelAndtask(Device device)
    {
        Task.Factory.StartNew(async () =>
        {
            flag = true;
            _logger.LogError($"上报机器属性  开始了 ");
            //WORK  STOP  ALAM  WAIT  IDLE SERV
            Dictionary<string, int> model = new Dictionary<string, int>()
            {
                { "WORK",1},
                { "STOP",2},
                { "ALAM",0},
                { "IDLE",0},
                { "SERV",0},
                { "WAIT",0},
                { "",0},
            };
            var oldPercentage = 0;
            while (flag)
            {
                Thread.Sleep(5000);
                try
                {
                    var values = device.WatchingProperties.GetValues();
                    var data = string.Join(",\r\n", values.Select(s => string.Join(":", s.Key, s.Value)));
                    _logger.LogInformation($"上报机器属性  {data} ");
                    //机器状态
                    var state = values["Drill_State"] != null ? values["Drill_State"]!.ToString() : string.Empty;
                    var status = model.ContainsKey(state) ? model[state] : 0;
                    var schedule = values["Drill_Percentage"] != null ? values["Drill_Percentage"].ToInt() : 0;
                    if (schedule != oldPercentage)
                    {
                        if (schedule == 0)
                        {
                            if (values["Drill_Percentage"] == null)
                            {
                                _logger.LogError($"获取不到进度属性  {data} ");
                                schedule = oldPercentage;
                            }
                            else
                            {
                                if (oldPercentage != 100)
                                {
                                    _logger.LogError($"中间手动点击从制定孔开始  {data} ");
                                    schedule = oldPercentage;
                                }
                            }

                        }

                        oldPercentage = schedule;
                    }

                    _logger.LogError($" 03-209  钻机状态： state {state}  上报的值 {status}  进度{schedule} ");

                    if (schedule == 100)
                    {
                        status = 4;
                        state = "完成";
                    }
                    if (schedule >= 0 && schedule < 100)
                    {
                        CommonModel.DrillTaskCode = CommonModel.TempDrillTaskCode;
                    }
                    if (string.IsNullOrEmpty(CommonModel.DrillTaskCode))
                    {
                        _logger.LogError($"03-209 上  CommonModel.DrillTaskCode {CommonModel.DrillTaskCode} 不上报信息");
                        continue;
                    }
                    var drillDrillingTaskReportPayload = new DrillDrillingTaskReportPayload()
                    {
                        body = new DrillDrillingTaskReportBody()
                        {
                            sn = device.DeviceId,
                            taskCode = CommonModel.DrillTaskCode,
                            status = status,
                            msg = state,
                            schedule = schedule
                        }
                    };
                    drillDrillingTaskReportPayload.ModifyHeader();
                    _logger.LogError($"03-209 上报钻孔进度 {JsonSerializer.Serialize(drillDrillingTaskReportPayload, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                    })} ");
                    await _mqttClient.PublishStringAsyncEnhance(IotTopic.DRILLING_TASK_REPORT_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillDrillingTaskReportPayload, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                    })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                    if (status == 4 && !string.IsNullOrEmpty(CommonModel.DrillTaskCode))
                    {
                        _logger.LogError($"03-209 上报钻孔进度 停止上报 CommonModel.DrillTaskCode:{CommonModel.DrillTaskCode}   CommonModel.TempDrillTaskCode:{CommonModel.TempDrillTaskCode}");
                        CommonModel.DrillTaskCode = string.Empty;
                        CommonModel.TempDrillTaskCode = string.Empty;

                    }


                }
                catch (Exception)
                {
                }
            }

            _logger.LogError($"上报机器属性  结束了 ");
        });
    }

    public override Task OnDisonnected(Device device)
    {
        try
        {
            _logger.LogInformation("XianJinIotDrillClientMqttListener OnDisonnected 进入");
            CommonModel.Key = string.Empty;
            CommonModel.publicKey = string.Empty;
            flag = false;

            return Task.CompletedTask;
        }
        finally
        {
            _logger.LogInformation("XianJinIotDrillClientMqttListener OnDisonnected 结束");
        }
    }

    public override async Task<bool> OnHeartbeat(Device device)
    {
        await Task.CompletedTask;
        return true;
    }
}
