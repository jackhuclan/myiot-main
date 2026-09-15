// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using Newtonsoft.Json.Linq;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Utils;

namespace Vegalot.External.XianjinIot.Drill;

internal class XianjinIotDrillClientMqttApplicationMessageListener : DefaultClientMqttApplicationMessageListener
{
    private readonly IMqttClient _mqttClient;
    private readonly IDeviceProvider _deviceProvider;
    private readonly ILogger<XianjinIotDrillClientMqttApplicationMessageListener> _logger;

    public XianjinIotDrillClientMqttApplicationMessageListener(IMqttClient mqttClient, IDeviceProvider deviceProvider, ILoggerFactory loggerFactory)
        : base(mqttClient, deviceProvider, loggerFactory)
    {
        _mqttClient = mqttClient;
        _deviceProvider = deviceProvider;
        _logger = loggerFactory.CreateLogger<XianjinIotDrillClientMqttApplicationMessageListener>();
    }

    public override async Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
    {
        try
        {
            var device = _deviceProvider.Devices[0];

            _logger.LogInformation($"arg: {JsonSerializer.Serialize(arg)}");
            _logger.LogInformation($"Topic:{arg.ApplicationMessage.Topic}");
            _logger.LogInformation($"Payload:{arg.ApplicationMessage.PayloadSegment.ToStr()}");
            var messages = Base64Convert.FromBase64StringToString(arg.ApplicationMessage.PayloadSegment.ToStr());
            _logger.LogInformation($"Payload  :{messages} CommonModel.Key:{CommonModel.Key}");

            var verifySignatureResult = await VerifySignature(messages, arg.ApplicationMessage.Topic);//验签
            if (!verifySignatureResult)//不成功，中断消息处理
            {
                _logger.LogInformation($"验签失败,中断消息处理");
                return;
            }

            var deviceServiceInvokeRequest = new DeviceServiceInvokeRequest()
            {
                ProductId = device.ProductId,
                DeviceId = device.DeviceId,
                ServiceId = Topics.Services.INVOKE_LOAD_MATERIAL_SERVICE_ID,
                CallerRequestInteractionDirection = InteractionPosition.Rear,
                CallerRequestMaterialKind = MaterialKind.Panel,
                Params = new Dictionary<string, object?>
            {
                   { "PayloadSegment",Base64Convert.FromBase64StringToString(arg.ApplicationMessage.PayloadSegment.ToStr())},
            }
            };
            await Services[arg.ApplicationMessage.Topic].Invoke(deviceServiceInvokeRequest);
            await Task.CompletedTask;
        }
        catch (Exception ee)
        {
            _logger.LogInformation($"OnApplicationMessageReceivedAsync: {ee.Message}");
        }
    }

    /// <summary>
    /// 验签
    /// </summary>
    /// <param name="PayloadSegmentString"></param>
    /// <param name="topic"></param>
    /// <returns></returns>
    public async Task<bool> VerifySignature(string PayloadSegmentString, string topic)
    {
        if (string.IsNullOrWhiteSpace(CommonModel.Key) || topic.Contains("regist_ack"))//注册ack不需要验签
        {
            return true;
        }
        var jo = JObject.Parse(PayloadSegmentString);
        var signature = jo["header"]?["signature"]?.ToString().Replace("\r\n", "").Replace(" ", "");
        if (string.IsNullOrWhiteSpace(signature))
        {
            return true;//如果signature 不存在，说明不需要验签
        }
        var body = jo["body"]?.ToString();
        if (string.IsNullOrWhiteSpace(body))
        {
            _logger.LogInformation($"接收到的消息body为空，无法验签");
            return false;
        }
        var rsaEncryptBodyResult = HMACSigner.Sign(body, CommonModel.Key);
        _logger.LogInformation($"验签：接收到的body加密值{rsaEncryptBodyResult}与header.signature值{signature}对比");
        if (rsaEncryptBodyResult != signature)
        {
            _logger.LogInformation($"验签失败");
            return false;
        }
        _logger.LogInformation($"topic:{topic}验签成功");
        await Task.CompletedTask;
        return true;
    }
}
