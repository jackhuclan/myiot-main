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
using MQTTnet.Server;
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
/// 下发装载料仓指指令
/// </summary>
internal class AgvLoadSiloCommand : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotAgvOptions _xianJinIotAgvOptions;
    private readonly ILogger<AgvLoadSiloCommand> _logger;

    public AgvLoadSiloCommand(
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        IOptions<XianJinIotAgvOptions> options,
        XianjInIotDefaultAgv device,
        CommandDescriptor commandDescriptor,
        ILogger<AgvLoadSiloCommand> logger)
        : base(serviceProvider, device, mqttClient, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _xianJinIotAgvOptions = options.Value;
        _logger = logger;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        var targetResponse = new DeviceServiceInvokeResponse();
        try
        {
            //02-103 下发装载料仓务指令
            _logger.LogInformation($"AgvLoadSiloCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");
           


            targetResponse.Message = "暂未运行逻辑";

            var isexistPayloadSegment = deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment");
            if (!isexistPayloadSegment || deviceServiceInvokeRequest.Params["PayloadSegment"] == null)
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"PayloadSegment 参数不正确：{deviceServiceInvokeRequest.Params["PayloadSegment"]}";
                return targetResponse;
            }
            var agvLoadSiloPayload = JsonSerializer.Deserialize<AgvLoadSiloPayload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToStr());
            if (agvLoadSiloPayload == null)
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"agvLoadSiloPayload 实体转换出错：NUll";
                return targetResponse;
            }
            var agvIsWorking = await CheckCommandIsWorking(IotTopic.LOAD_SILO_ACK_TOPIC, agvLoadSiloPayload.body.taskCode);
            if (agvIsWorking)
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"AGV正在执行任务中...";
                return targetResponse;
            }
            byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveId"].ToInt();

            try
            {
                await InitializePlcRegisterPoints(slaveID, [4002, 4003], agvLoadSiloPayload.body.taskCode);
                _ = Task.Run(async () =>
                {
                    try
                    {
                        _logger.LogInformation($"TaskCode:{agvLoadSiloPayload.body.taskCode},ack处理中\r\n");
                        await CommandCallback(agvLoadSiloPayload, slaveID);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(ex, $"TaskCode:{agvLoadSiloPayload.body.taskCode},ack处理异常\r\n");
                    }
                }).ContinueWith(async t =>//执行完后初始化plc点位
                {
                    await InitializePlcRegisterPoints(slaveID, [4002, 4003], agvLoadSiloPayload.body.taskCode);
                });

                targetResponse.Code = ErrorCodes.Sys.SUCCESS;
                targetResponse.Message = "已接收指令";
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"TaskCode:{agvLoadSiloPayload.body.taskCode},指令处理失败\r\n");
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"指令处理失败: {ex.Message}";
            
            }
        }
        catch (Exception ex)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"指令处理失败: {ex.Message}";
        }

        return targetResponse;

    }

    private async Task CommandCallback(AgvLoadSiloPayload agvLoadSiloPayload, byte slaveID)
    {
        //02-205 上报装载料仓指令接收ACK
        var taskCode = agvLoadSiloPayload.body.taskCode;
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 3000, 1);
        _logger.LogInformation($"TaskCode:{taskCode},给PLC3000写入LoadSiloAck信号完成\r\n");
        bool isWaittingAck = false;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        while (true)
        {
            try
            {
                var isInit = await InitializeAgvAgent(slaveID);
                if (isWaittingAck && (isInit || stopwatch.ElapsedMilliseconds > _xianJinIotAgvOptions.AckListeningTime))//ack一般要求是立即返回的，如果长时间接收不到，就直接先结束流程
                {
                    _logger.LogInformation($"终止{taskCode}的ack轮询，isWaittingAck：{isWaittingAck}，isInit：{isInit}，ElapsedMilliseconds：{stopwatch.ElapsedMilliseconds}");
                    stopwatch.Stop();
                    isWaittingAck = false;
                    await RemoveTaskCodeRecord(taskCode);
                    return;
                }
                var waitAck = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4002, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC4002接收LoadSiloAck信号完成\r\n");
                if (waitAck[0] != 1)
                {
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC接收LoadSiloAck信号··· \r\n");
                    isWaittingAck = true;
                    await Task.Delay(100);
                    continue;
                }
                var agvLoadSiloAckPayload = new AgvLoadSiloAckPayload()
                {
                    body = new AgvLoadSiloAckBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        code = 200,
                        msg = "指令接收成功",
                    }
                };
                await _mqttClient.PublishStringAsyncEnhance(IotTopic.LOAD_SILO_ACK_TOPIC, agvLoadSiloAckPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{taskCode}, 给MES发送 LoadSiloACK完成 \r\n");
                _logger.LogInformation($"TaskCode:{taskCode},LoadSiloAck-CommandCallback用时：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode}, PLC接收到LoadSiloAck清除4002信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4002, 0);
                stopwatch.Stop();
                try
                {
                    _logger.LogInformation($"TaskCode:{taskCode},动作执行中\r\n");

                    await CommandReport(agvLoadSiloPayload, slaveID);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, $"TaskCode:{taskCode},动作执行异常\r\n");
                }
                return;
            }
            catch (Exception ex)
            {
                await Task.Delay(1000);
                _logger.LogInformation(ex, $"TaskCode:{taskCode},读取ACK信号失败\r\n");
            }
        }
    }

    private async Task CommandReport(AgvLoadSiloPayload agvLoadSiloPayload, byte slaveID)
    {
        //02-206 上报装载料仓完成
        var taskCode = agvLoadSiloPayload.body.taskCode;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        while (true)
        {
            {
                try
                {
                    var isInit = await InitializeAgvAgent(slaveID);
                    if (isInit)
                    {
                        await RemoveTaskCodeRecord(taskCode);
                        return;
                    }
                    var waitCompleteSilo = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4003, 1);
                    _logger.LogInformation($"TaskCode:{taskCode},读取PLC4003接收LoadSilo信号完成\r\n");

                    if (waitCompleteSilo[0] != 1)
                    {
                        _logger.LogInformation($"TaskCode:{taskCode},等待PLC 接收 LoadSilo 信号···\r\n");
                        await Task.Delay(100);
                        continue;
                    }
                    var agvLoadSiloReportPayload = new AgvLoadSiloReportPayload()
                    {
                        body = new AgvLoadSiloReportBody()
                        {
                            sn = InteractingDevice.DeviceId,
                            taskCode = taskCode,
                            status = 1,
                            msg = "提升料仓到位"
                        }
                    };

                    await _mqttClient.PublishStringAsyncEnhance(IotTopic.LOAD_SILO_REPORT_TOPIC, agvLoadSiloReportPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                    _logger.LogInformation($"TaskCode:{taskCode}, 给MES发送 LoadSilo完成 \r\n");
                    _logger.LogInformation($"TaskCode:{taskCode},LoadSilo-CommandReport：【{stopwatch.ElapsedMilliseconds}】ms");
                    _logger.LogInformation($"TaskCode:{taskCode}, PLC接收到 LoadSilo 清除4003信号 \r\n");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4003, 0);
                    stopwatch.Stop();
                    return;
                }
                catch (Exception ex)
                {
                    await Task.Delay(1000);
                    _logger.LogInformation(ex, $"TaskCode:{taskCode},PLC动作执行失败\r\n");
                }
            }
        }
    }

   
}
