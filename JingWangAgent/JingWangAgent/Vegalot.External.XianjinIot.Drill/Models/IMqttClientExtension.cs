// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MQTTnet.Client;
using MQTTnet.Protocol;
using NLog;

namespace Vegalot.External.XianjinIot.Drill.Models;
internal static class IMqttClientExtension
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    public static async Task PublishStringAsyncEnhance(this IMqttClient mqttClient, string topic, string payload = null, MqttQualityOfServiceLevel qualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce)
    {
        _logger.Info($"PublishStringAsyncEnhance   begin");
        try
        {
            _logger.Info($"topic:{topic}");
            _logger.Info($"payload:{payload}");
            await mqttClient.PublishStringAsync(topic, payload, qualityOfServiceLevel);
        }
        catch (Exception ee)
        {
            _logger.Info($"{ee.Message}");

        }
        finally
        {
            _logger.Info($"PublishStringAsyncEnhance   end");
        }
    }

}
