// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vegalot.External.XianjinIot.Common.Models.Report;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot;
using System.Text.Json;

namespace Vegalot.External.XianjinIot.Common.Manager;
public class ReceiveUnitStateAckCommand : SimpleCommand<Device>
{

    private readonly ILogger<RegistAckCommand> _logger;
    private readonly Device _device;

    public ReceiveUnitStateAckCommand(IServiceProvider serviceProvider, Device device, ILogger<RegistAckCommand> logger,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _logger = logger;
        _device = device;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        _logger.LogInformation($"01-102 RegistAckCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");
        var targetResponse = new DeviceServiceInvokeResponse();
        if (deviceServiceInvokeRequest == null || deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment") || string.IsNullOrEmpty(deviceServiceInvokeRequest.Params["PayloadSegment"]!.ToString()))
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"RegistAckCommand  PayloadSegment 参数不正确：{deviceServiceInvokeRequest?.Params?["PayloadSegment"]?.ToString()??""}";
            await Task.CompletedTask;
            return targetResponse;
        }
        string payloadString = deviceServiceInvokeRequest?.Params["PayloadSegment"]?.ToString()??"";
        _logger.LogInformation($"验签返回消息ReceiveUnitStateAck：{payloadString}");
        var registAckPayload = JsonSerializer.Deserialize<RegistAckPayload>(payloadString);
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
