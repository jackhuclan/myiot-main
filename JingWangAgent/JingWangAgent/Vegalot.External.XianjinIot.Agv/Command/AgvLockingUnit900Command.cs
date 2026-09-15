// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using Vegalot.External.XianjinIot.Agv.Models.Ack;
using Vegalot.External.XianjinIot.Agv.Models.Command;
using Vegalot.External.XianjinIot.Common;
using Vegalot.External.XianjinIot.Common.Models;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace Vegalot.External.XianjinIot.Agv.Command;

/// <summary>
/// 下发AGV调整到指定高度
/// </summary>
internal class AgvLockingUnit900Command : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotAgvOptions _xianJinIotAgvOptions;
    private readonly ILogger<AgvLockingUnit900Command> _logger;

    public AgvLockingUnit900Command(
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        IOptions<XianJinIotAgvOptions> options,
        XianjInIotDefaultAgv device,
        CommandDescriptor commandDescriptor,
        ILogger<AgvLockingUnit900Command> logger)
        : base(serviceProvider, device, mqttClient, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _xianJinIotAgvOptions = options.Value;
        _logger = logger;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        //02-220 上报900对接单元指令接收ACK
        _logger.LogInformation($"AgvLockingUnit900Command  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");

        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Code = ErrorCodes.Sys.FAIL;
        targetResponse.Message = "暂未运行逻辑";

        var isexistPayloadSegment = deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment");
        if (!isexistPayloadSegment || deviceServiceInvokeRequest.Params["PayloadSegment"] == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"PayloadSegment 参数不正确：{deviceServiceInvokeRequest.Params["PayloadSegment"]}";
            return targetResponse;
        }

        var agvLocakingUnit900Payload = JsonSerializer.Deserialize<AgvLockingUnit900Payload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToStr());
        if (agvLocakingUnit900Payload == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"AgvAdjustHeightPayload 实体转换出错：NUll";
            return targetResponse;
        }
        var agvIsWorking = await CheckCommandIsWorking(IotTopic.LOCKING_UNIT_ACK_900_TOPIC, agvLocakingUnit900Payload.body.taskCode);
        if (agvIsWorking)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"AGV正在执行任务中...";
            return targetResponse;
        }
        byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveId"].ToInt();
        try
        {
            await InitializePlcRegisterPoints(slaveID, [4014], agvLocakingUnit900Payload.body.taskCode);
            _ = Task.Run(async () =>
            {
                await CommandCallback(agvLocakingUnit900Payload, slaveID);

            }).ContinueWith(async t => {

                await InitializePlcRegisterPoints(slaveID, [4014], agvLocakingUnit900Payload.body.taskCode);
            });                        
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"TaskCode:{agvLocakingUnit900Payload.body.taskCode},指令处理失败\r\n");
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"指令处理失败: {ex.Message}";
        }
        return targetResponse;
    }

    private async Task CommandCallback(AgvLockingUnit900Payload agvLocakingUnit900Payload, byte slaveID)
    {
        var taskCode = agvLocakingUnit900Payload.body.taskCode;
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 3006, 1);
        _logger.LogInformation($"TaskCode:{taskCode},给PLC3006写入AgvLockingUnit900Ack 信号完成\r\n");
        bool isWaittingAck = false;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        while (true)
        {
            try
            {
                var isInit = await InitializeAgvAgent(slaveID);
                if (isWaittingAck && (isInit || stopwatch.ElapsedMilliseconds > _xianJinIotAgvOptions.AckListeningTime))//ack一般要求是立即返回的，如果长时间接收不到，就直接先结束流程)
                {
                    _logger.LogInformation($"终止{taskCode}的ack轮询，isWaittingAck：{isWaittingAck}，isInit：{isInit}，ElapsedMilliseconds：{stopwatch.ElapsedMilliseconds}");
                    stopwatch.Stop();
                    isWaittingAck = false;
                    await RemoveTaskCodeRecord(taskCode);
                    return;
                }
                var waitAck = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4014, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC4014AgvLockingUnit900Ack 信号完成\r\n");

                if (waitAck[0] != 1)
                {
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC 接收 等待AgvLockingUnit900Ack 信号···\r\n");
                    isWaittingAck = true;
                    await Task.Delay(100);
                    continue;
                }
                var agvLockingUnit900AckkPayload = new AgvLockingUnit900AckPayload()
                {
                    body = new AgvLockingUnit900AckBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        code = 200,
                        msg = "指令接收成功",
                    }
                };
                await _mqttClient.PublishStringAsyncEnhance(IotTopic.LOCKING_UNIT_ACK_900_TOPIC, agvLockingUnit900AckkPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{taskCode},给MES发送AgvLockingUnit900Ack信号完成\r\n");
                _logger.LogInformation($"TaskCode:{taskCode},AgvLockingUnit900Ack-CommandCallback用时：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode},PLC 已接收到 AgvLockingUnit900Ack 清除4014信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4014, 0);
                stopwatch.Stop();
                return;
            }
            catch (Exception ex)
            {
                await Task.Delay(1000);
                _logger.LogInformation(ex, $"TaskCode:{taskCode},读取ACK信号失败\r\n");
            }
        }

    }

    
}
