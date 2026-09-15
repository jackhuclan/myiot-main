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
internal class AgvStopWorkingCommand : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotAgvOptions _xianJinIotAgvOptions;
    private readonly ILogger<AgvStopWorkingCommand> _logger;

    public AgvStopWorkingCommand(
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        IOptions<XianJinIotAgvOptions> options,
        XianjInIotDefaultAgv device,
        CommandDescriptor commandDescriptor,
        ILogger<AgvStopWorkingCommand> logger)
        : base(serviceProvider, device, mqttClient, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _xianJinIotAgvOptions = options.Value;
        _logger = logger;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        //02 - 111 下发上装停止运行的指令
        _logger.LogInformation($"AgvStopWorkingCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");

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

        var agvStopWorkingPayload = JsonSerializer.Deserialize<AgvStopWorkingPayload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToStr());
        if (agvStopWorkingPayload == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"agvStopWorkingPayload 实体转换出错：NUll";
            return targetResponse;
        }
        var agvIsWorking = await CheckCommandIsWorking(IotTopic.STOP_WORKING_ACK_TOPIC, agvStopWorkingPayload.body.taskCode);
        if (agvIsWorking)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"AGV正在执行任务中...";
            return targetResponse;
        }
        byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveId"].ToInt();
        try
        {
            await InitializePlcRegisterPoints(slaveID, [4012], agvStopWorkingPayload.body.taskCode);

            _ = Task.Run(async () =>
            {
                await CommandCallback(agvStopWorkingPayload, slaveID);
            }).ContinueWith(async t =>
            {
                await InitializePlcRegisterPoints(slaveID, [4012], agvStopWorkingPayload.body.taskCode);
            });


        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"TaskCode:{agvStopWorkingPayload.body.taskCode},指令处理失败\r\n");
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"指令处理失败: {ex.Message}";
        }
        return targetResponse;
    }

    private async Task CommandCallback(AgvStopWorkingPayload agvStopWorkingPayload, byte slaveID)
    {
        var taskCode = agvStopWorkingPayload.body.taskCode;
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 3009, 1);
        _logger.LogInformation($"TaskCode:{taskCode},给PLC3009写入AgvStopWorkingAck信号完成 \r\n");
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
                    isWaittingAck = false;
                    stopwatch.Stop();
                    await RemoveTaskCodeRecord(taskCode);
                    return;
                }
                var waitAck = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4012, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC4012AgvStopWorkingAck信号完成 \r\n");
                if (waitAck[0] != 1)
                {
                    isWaittingAck = true;
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC 接收 等待StopWorkingtAck 信号···\r\n");
                    await Task.Delay(100);
                    continue;
                }
                var agvStopWorkingAckPayload = new AgvStopWorkingAckPayload()
                {
                    body = new AgvStopWorkingAckBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        code = 200,
                        msg = "指令接收成功",
                    }
                };

                await _mqttClient.PublishStringAsyncEnhance(IotTopic.STOP_WORKING_ACK_TOPIC, agvStopWorkingAckPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{taskCode},给MES发送StopWorkingtAck信号完成 \r\n");
                _logger.LogInformation($"TaskCode:{taskCode},AgvStopWorkingAck-CommandCallback用时：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode},PLC 已接收到 AgvStopWorkingAck 清除4012信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4012, 0);
                stopwatch.Stop();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"TaskCode:{taskCode},读取ACK信号失败\r\n");
                await Task.Delay(1000);
            }
        }     
    }   
}
