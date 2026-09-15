// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Microsoft.Extensions.Logging;
using Vegalot.External.XianjinIot.Drill.Models.Ack;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace Vegalot.External.XianjinIot.Drill.Command.Manager;

internal class RegistAckCommand : SimpleCommand<Device>
{
    private readonly ILogger<RegistAckCommand> _logger;

    public RegistAckCommand(IServiceProvider serviceProvider, Device device, ILogger<RegistAckCommand> logger,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _logger = logger;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        _logger.LogInformation($"01-102 RegistAckCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");
        var targetResponse = new DeviceServiceInvokeResponse();
        if (deviceServiceInvokeRequest == null || deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment") || string.IsNullOrEmpty(deviceServiceInvokeRequest.Params["PayloadSegment"]!.ToString()))
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"RegistAckCommand  PayloadSegment 参数不正确：{deviceServiceInvokeRequest.Params["PayloadSegment"]}";
            await Task.CompletedTask;
            return targetResponse;
        }
        string payloadString = deviceServiceInvokeRequest.Params["PayloadSegment"].ToString();

        var registAckPayload = JsonSerializer.Deserialize<DrillRegistAckPayload>(payloadString);
        if (registAckPayload == null || registAckPayload.body == null || registAckPayload.header == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"DrillRegistAckPayload 实体转换出错：NUll";
            return targetResponse;
        }
        //获取注册ack 结果
        return targetResponse;
    }
}
