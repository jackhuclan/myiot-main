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
using Vegalot.External.XianjinIot.Agv.Models.Report;
using Vegalot.External.XianjinIot.Common;
using Vegalot.External.XianjinIot.Common.Models;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace Vegalot.External.XianjinIot.Agv.Command;

/// <summary>
/// 下发卸载料仓指令
/// </summary>
internal class AgvUnloadSiloCommand : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotAgvOptions _xianJinIotAgvOptions;
    private readonly ILogger<AgvUnloadSiloCommand> _logger;

    public AgvUnloadSiloCommand(
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        IOptions<XianJinIotAgvOptions> options,
        XianjInIotDefaultAgv device,
        CommandDescriptor commandDescriptor,
        ILogger<AgvUnloadSiloCommand> logger)
        : base(serviceProvider, device, mqttClient, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _xianJinIotAgvOptions = options.Value;
        _logger = logger;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        //02-107 下发卸载料仓指令
        _logger.LogInformation($"AgvUnloadSiloCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");

        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Message = "暂未运行逻辑";

        var isexistPayloadSegment = deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment");
        if (!isexistPayloadSegment || deviceServiceInvokeRequest.Params["PayloadSegment"] == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"PayloadSegment 参数不正确：{deviceServiceInvokeRequest.Params["PayloadSegment"]}";
            return targetResponse;
        }

        var agvUnloadSiloPayload = JsonSerializer.Deserialize<AgvUnloadSiloPayload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToStr());
        if (agvUnloadSiloPayload == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"AgvUnloadSiloPayload 实体转换出错：NUll";
            return targetResponse;
        }
        var agvIsWorking = await CheckCommandIsWorking(IotTopic.UNLOAD_SILO_ACK_TOPIC, agvUnloadSiloPayload.body.taskCode);
        if (agvIsWorking)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"AGV正在执行任务中...";
            return targetResponse;
        }
        byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveId"].ToInt();

        try
        {
            await InitializePlcRegisterPoints(slaveID, [4006,4007], agvUnloadSiloPayload.body.taskCode);
            _ = Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation($"TaskCode:{agvUnloadSiloPayload.body.taskCode},ack处理中\r\n");
                    await CommandCallback(agvUnloadSiloPayload, slaveID);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, $"TaskCode:{agvUnloadSiloPayload.body.taskCode},ack处理异常\r\n");
                }
            }).ContinueWith(async t =>//执行完后初始化plc点位
            {
                await InitializePlcRegisterPoints(slaveID, [4006, 4007], agvUnloadSiloPayload.body.taskCode);
            }); 
            targetResponse.Code = ErrorCodes.Sys.SUCCESS;
            targetResponse.Message = "已接收指令";
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"TaskCode:{agvUnloadSiloPayload.body.taskCode},指令处理失败\r\n");
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"指令处理失败: {ex.Message}";
        }

        targetResponse.Code = ErrorCodes.Sys.SUCCESS;
        targetResponse.Message = "已接收指令";
        return targetResponse;
    }

    private async Task CommandCallback(AgvUnloadSiloPayload agvUnloadSiloPayload, byte slaveID)
    {
        //02-211 上报卸载料仓指令接收ACK
        var taskCode = agvUnloadSiloPayload.body.taskCode;
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 3005, 1);
        _logger.LogInformation($"TaskCode:{taskCode},给PLC3005写入接收 UnLoadSiloAck 信号完成\r\n");
        bool isWaittingAck = false;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        while (true)
        {
            try
            {
                var isInit = await InitializeAgvAgent(slaveID);
                if (isWaittingAck &&(isInit || stopwatch.ElapsedMilliseconds > _xianJinIotAgvOptions.AckListeningTime))//ack一般要求是立即返回的，如果长时间接收不到，就直接先结束流程)
                {
                    _logger.LogInformation($"终止{taskCode}的ack轮询，isWaittingAck：{isWaittingAck}，isInit：{isInit}，ElapsedMilliseconds：{stopwatch.ElapsedMilliseconds}");
                    isWaittingAck = false;
                    stopwatch.Stop();
                    await RemoveTaskCodeRecord(taskCode);
                    return;
                }
                var waitAck = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4006, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC4006 UnLoadSiloAck 信号完成\r\n");
                if (waitAck[0] != 1)
                {
                    isWaittingAck = true;
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC 接收 UnLoadSiloAck 信号···\r\n");
                    await Task.Delay(100);
                    continue;
                }

                var agvUnLoadSiloAckPayload = new AgvUnloadSiloAckPayload()
                {
                    body = new AgvUnloadSiloAckBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        code = 200,
                        msg = "指令接收成功"
                    }
                };
                await _mqttClient.PublishStringAsyncEnhance(IotTopic.UNLOAD_SILO_ACK_TOPIC, agvUnLoadSiloAckPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{taskCode},给MES发送UnLoadSiloAck 信号完成\r\n");
                _logger.LogInformation($"TaskCode:{taskCode},UnLoadSiloAck-CommandCallback用时：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode},PLC接收到 UnLoadSiloAck 清除4006信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4006, 0);
                stopwatch.Stop();
                try
                {
                    _logger.LogInformation($"TaskCode:{taskCode},动作执行中\r\n");

                    await CommandReport(agvUnloadSiloPayload, slaveID);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, $"TaskCode:{taskCode},动作执行异常\r\n");
                }
                return;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"TaskCode:{taskCode},读取ACK信号失败\r\n");
                await Task.Delay(1000);
            }
        }      
    }

    private async Task CommandReport(AgvUnloadSiloPayload agvUnloadSiloPayload, byte slaveID)
    {
        //02-212 上报卸载料仓完成
        var taskCode = agvUnloadSiloPayload.body.taskCode;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        while (true)
        {
            try
            {
                var isInit = await InitializeAgvAgent(slaveID);
                if (isInit)
                {
                    await RemoveTaskCodeRecord(taskCode);
                    return;
                }
                var waitCompleteSilo = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4007, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC4007UnLoadSilo 信号完成\r\n");
                if (waitCompleteSilo[0] != 1)
                {
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC 接收 UnLoadSilo 信号···\r\n");
                    await Task.Delay(100);
                    continue;
                }
                var agvUnLoadSiloPayload = new AgvUnloadSiloReportPayload()
                {
                    body = new AgvUnloadSiloReportBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        status = 1,
                        msg = "卸载料仓完成"
                    }
                };

                await _mqttClient.PublishStringAsyncEnhance(IotTopic.UNLOAD_SILO_REPORT_TOPIC, agvUnLoadSiloPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{taskCode},给MEA发送UnLoadSilo信号完成\r\n");
                _logger.LogInformation($"TaskCode:{taskCode},UnLoadSilo-CommandReport：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode},PLC接收到 UnLoadSilo 清除4007信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4007, 0);
                stopwatch.Stop();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"TaskCode:{taskCode},PLC动作执行失败\r\n");
                await Task.Delay(1000);
            }
        }      
    }  
}
