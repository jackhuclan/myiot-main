// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using Vegalot.External.XianjinIot.Drill.Models;
using Vegalot.External.XianjinIot.Drill.Models.Report;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Fundation.Utils;

namespace Vegalot.External.XianjinIot.Drill;

internal class XianjinDrillLoadPanelToDrillComplete : IDrillLoadPanelToDrillComplete
{
    private readonly IMqttClient _mqttClient;
    private readonly ILogger<XianjinDrillLoadPanelToDrillComplete> _logger;
    private readonly XianJinIotDrillOptions _xianJinIotOptions;

    public XianjinDrillLoadPanelToDrillComplete(
        IMqttClient mqttClient,
        ILogger<XianjinDrillLoadPanelToDrillComplete> logger,
        IOptions<XianJinIotDrillOptions> options)
    {
        _mqttClient = mqttClient;
        _logger = logger;
        _xianJinIotOptions = options.Value;
    }
    public void PanelToDrillComplete(string content)
    {
        try
        {
            var data = content.Split(',');
            if (data.Length != 2)
            {
                _logger.LogDebug($"XianjinDrillLoadPanelToDrillComplete  PanelToDrillComplete content 内容不对");
                return;
            }

            var drillReceivePanelAckPayload = new DrillUpPanelReportPayload()
            {
                body = new DrillUpPanelReportBody()
                {
                    sn = CommonModel.sn,
                    taskCode = CommonModel.UpPanelReportTaskCode,
                    status = data[0].ToInt(),
                    msg = data[1],
                }
            };
            drillReceivePanelAckPayload.ModifyHeader();
            CommonModel.UpPanelReportTaskCode = string.Empty;
            _logger.LogError($"03-212 上板到钻机完成实体 {JsonSerializer.Serialize(drillReceivePanelAckPayload)}");
            _mqttClient.PublishStringAsyncEnhance(IotTopic.UP_PANEL_REPORT_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillReceivePanelAckPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce).GetAwaiter().GetResult();


        }
        catch (Exception e)
        {

            _logger.LogError($"03-212 PanelToDrillComplete 异常：{e.Message}");
        }
    }
}
