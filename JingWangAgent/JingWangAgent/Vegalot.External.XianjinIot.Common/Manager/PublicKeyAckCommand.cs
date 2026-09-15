// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Tls.Crypto.Impl;
using Vegalot.External.XianjinIot.Common.Models;
using Vegalot.External.XianjinIot.Common.Models.Ack;
using Vegalot.External.XianjinIot.Common.Models.Report;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace Vegalot.External.XianjinIot.Common.Manager;

public class PublicKeyAckCommand : SimpleCommand<Device>
{
    private readonly Device _device;
    private readonly IMqttClient _mqttClient;
    private readonly ILogger<PublicKeyAckCommand> _logger;
    private readonly RSACryptoServiceProvider rsa;

    public PublicKeyAckCommand(IServiceProvider serviceProvider,
        Device device, IMqttClient mqttClient,
        ILogger<PublicKeyAckCommand> logger,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _device = device;
        _mqttClient = mqttClient;
        _logger = logger;
        rsa= new RSACryptoServiceProvider();
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        //DrillPublicKeyAckPayload
        _logger.LogInformation($"01-101 下发公钥  PublicKeyAckCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");
        var targetResponse = new DeviceServiceInvokeResponse();
        if (deviceServiceInvokeRequest == null || deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment") || string.IsNullOrEmpty(deviceServiceInvokeRequest.Params["PayloadSegment"]!.ToString()))
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"PayloadSegment 参数不正确：{(deviceServiceInvokeRequest?.Params==null ? "Params=null" : deviceServiceInvokeRequest?.Params["PayloadSegment"]??"")}";
            await Task.CompletedTask;
            return targetResponse;
        }
        string payloadString = deviceServiceInvokeRequest.Params["PayloadSegment"]?.ToString()??"";
        var drillPublicKeyAckPayload = JsonSerializer.Deserialize<PublicKeyAckPayload>(payloadString);
        if (drillPublicKeyAckPayload == null || drillPublicKeyAckPayload.body == null || drillPublicKeyAckPayload.header == null)
        {
            _logger.LogError($"drillPublicKeyAckPayload 实体转换出错");
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"drillPublicKeyAckPayload 实体转换出错：NUll";
            return targetResponse;
        }
        CommonModel.PublicKey = drillPublicKeyAckPayload.body.PublicKeyTrim(); 
        _logger.LogInformation($"收到服务端返回的公钥PublicKey： {CommonModel.PublicKey}");
        CommonModel.Key = AesEncryption.GenerateAESKeyString(192);
        _logger.LogInformation($"设备端生成的key: {CommonModel.Key}");
        var sessionKey = CommonModel.Key;
        if (!string.IsNullOrWhiteSpace(CommonModel.PublicKey))//公钥加密
        {
            sessionKey = RSAUtils.Encrypt(CommonModel.Key, CommonModel.PublicKey);
            _logger.LogInformation($"设备端生成的key公钥加密后的值 {sessionKey}");
        }
        //上报设备 01-202
        var registPayLoad = new RegistPayloadEntity()
        {
            body = new RegistBodyEntity()
            {
                sn = _device.DeviceId,
                deviceCode = _device.DeviceId,
                sessionKey = sessionKey,
                deviceType = "BORER",
                deviceIp = _device.DeviceDescriptor.HostAddress ?? "127.0.0.1",
                axles_number = _device.DeviceDescriptor.SpindleNum,
                applicationInfoList = new List<ApplicationInfo>()
                {
                    new ApplicationInfo()
                    {
                        name= Assembly.GetEntryAssembly()?.GetName()?.Name??"",
                        version=_device.DeviceDescriptor.AgentVersion.ToString()
                    }
                }
            }
        };
        _logger.LogInformation($"01-202 上报注册信息实体 :{JsonSerializer.Serialize(registPayLoad, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })}");
        await _mqttClient.PublishStringAsyncEnhance(IotTopic.REGIST_TOPIC, registPayLoad, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce,false);

        return targetResponse;
    }
}
