// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json.Linq;
using NLog;
using Vegalot.External.XianjinIot.Common.Models;

namespace Vegalot.External.XianjinIot.Common;

public static class IMqttClientExtension
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    public static async Task PublishStringAsyncEnhance<TPayload>(this IMqttClient mqttClient, string topic, TPayload payload, MqttQualityOfServiceLevel qualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce, bool isNeedBodySign = true,bool isNeedLog=true) where TPayload : BasePayload<BaseBody>
    {
        Log($"PublishStringAsyncEnhance   begin",isNeedLog);
        try
        {
            if (isNeedBodySign && !string.IsNullOrWhiteSpace(CommonModel.Key))//body整体签名,放在signature里面
            {
                //泛型的反序列化用Dictionary<string, object>，或者JObject来做
                JObject jo = JObject.Parse(JsonSerializer.Serialize(payload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) }));
                var body = jo["body"]?.ToString() ?? "";
                Log($"获取到的body:{body}", isNeedLog);              
                if (!string.IsNullOrWhiteSpace(body))
                {
                    payload.header.signature = HMACSigner.Sign(body, CommonModel.Key);
                }
            }
            string data = JsonSerializer.Serialize(payload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });
            Log($"topic:{topic},payload:{data}", isNeedLog);            
            data = Base64Convert.FromStringToBase64String(data);
            await mqttClient.PublishStringAsync(topic, data, qualityOfServiceLevel);
        }
        catch (Exception ee)
        {
            _logger.Info($"{ee.Message}");
        }
        finally
        {
            Log($"PublishStringAsyncEnhance   end", isNeedLog);
        }
    }

    public static void Log(string message,bool isNeedLog=true)
    {
        if (isNeedLog)
        {
            _logger.Info(message);
        }
    }


}

