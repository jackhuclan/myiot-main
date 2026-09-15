// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using MessagePack.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using Newtonsoft.Json.Linq;
using Vegalot.External.XianjinIot.Agv.Models.Ack;
using Vegalot.External.XianjinIot.Agv.Models.Command;
using Vegalot.External.XianjinIot.Common;
using Vegalot.External.XianjinIot.Common.Models;
using Vegalot.External.XianjinIot.Common.Models.Report;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Utils;

namespace Vegalot.External.XianjinIot.Agv;

internal class XianJinIotAgvClientMqttApplicationMessageListener : DefaultClientMqttApplicationMessageListener
{
    private readonly IMqttClient _mqttClient;
    private readonly IDeviceProvider _deviceProvider;
    private readonly XianJinIotAgvOptions _xianJinIotAgvOptions;
    private readonly ILogger<XianJinIotAgvClientMqttApplicationMessageListener> _logger;

    public XianJinIotAgvClientMqttApplicationMessageListener(IMqttClient mqttClient, IOptions<XianJinIotAgvOptions> options, IDeviceProvider deviceProvider, ILoggerFactory loggerFactory)
        : base(mqttClient, deviceProvider, loggerFactory)
    {
        _mqttClient = mqttClient;
        _deviceProvider = deviceProvider;
        _xianJinIotAgvOptions= options.Value;
        _logger = loggerFactory.CreateLogger<XianJinIotAgvClientMqttApplicationMessageListener>();
    }

    public override async Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
    {
        try
        {
            var device = _deviceProvider.Devices[0];

            _logger.LogInformation($"arg: {JsonSerializer.Serialize(arg)}");
            _logger.LogInformation($"Topic:{arg.ApplicationMessage.Topic}");            
            var messages =  Base64Convert.FromBase64StringToString(arg.ApplicationMessage.PayloadSegment.ToStr());
            _logger.LogInformation($"Payload:{messages}");
            var verifySignatureResult = await VerifySignature(messages, arg.ApplicationMessage.Topic);//验签
            if (!verifySignatureResult)//不成功，说明消息格式不正确或者中间被劫持篡改了，中断消息处理
            {
                _logger.LogInformation($"{arg.ApplicationMessage.Topic}验签失败,中断消息处理");
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
                  { "PayloadSegment",messages},
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
    public async  Task<bool> VerifySignature(string PayloadSegmentString,string topic)
    {
        if (!_xianJinIotAgvOptions.VerfiySignatureSwitch)//验签开关
        {
            _logger.LogInformation($"验签开关:{_xianJinIotAgvOptions.VerfiySignatureSwitch},不需要验签");
            return true;
        }     
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
        _logger.LogInformation($"验签：接收到的body加密值{rsaEncryptBodyResult}与header.signature值{signature}对比,会话密钥：{CommonModel.Key}");
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
