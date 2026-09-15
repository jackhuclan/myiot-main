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
/// 下发准备接料指令
/// </summary>
internal class AgvReceivePanelCommand : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotAgvOptions _xianJinIotAgvOptions;
    private readonly ILogger<AgvReceivePanelCommand> _logger;

    public AgvReceivePanelCommand(
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        IOptions<XianJinIotAgvOptions> options,
        XianjInIotDefaultAgv device,
        CommandDescriptor commandDescriptor,
        ILogger<AgvReceivePanelCommand> logger)
        : base(serviceProvider, device, mqttClient, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _xianJinIotAgvOptions = options.Value;
        _logger = logger;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        //02-106 下发装载料仓务指令
        _logger.LogInformation($"AgvReceivePanelCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");

        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Message = "暂未运行逻辑";

        var isexistPayloadSegment = deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment");
        if (!isexistPayloadSegment || deviceServiceInvokeRequest.Params["PayloadSegment"] == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"PayloadSegment 参数不正确：{deviceServiceInvokeRequest.Params["PayloadSegment"]}";
            return targetResponse;
        }

        var agvReceivePanelPayload = JsonSerializer.Deserialize<AgvReceivePanelPayload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToStr());
        if (agvReceivePanelPayload == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"agvLoadSiloPayload 实体转换出错：NUll";
            return targetResponse;
        }
        var agvIsWorking = await CheckCommandIsWorking(IotTopic.RECEIVE_PANEL_ACK_TOPIC, agvReceivePanelPayload.body.taskCode);
        if (agvIsWorking)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"AGV正在执行任务中...";
            return targetResponse;
        }
        byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveId"].ToInt();
        try
        {
            await InitializePlcRegisterPoints(slaveID, [4005, 4008, 4016], agvReceivePanelPayload.body.taskCode);            
            _ =Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation($"TaskCode:{agvReceivePanelPayload.body.taskCode},ack处理中\r\n");
                    await CommandCallback(agvReceivePanelPayload, slaveID);                   
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, $"TaskCode:{agvReceivePanelPayload.body.taskCode},ack处理异常\r\n");
                }
            }).ContinueWith(async t =>//执行完后初始化plc点位
            {
                await InitializePlcRegisterPoints(slaveID, [4005, 4008,4016], agvReceivePanelPayload.body.taskCode);
            });

            targetResponse.Code = ErrorCodes.Sys.SUCCESS;
            targetResponse.Message = "已接收指令";
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"TaskCode:{agvReceivePanelPayload.body.taskCode},指令处理失败\r\n");
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"指令处理失败: {ex.Message}";
        }

        return targetResponse;
    }

    private async Task CommandCallback(AgvReceivePanelPayload agvReceivePanelPayload, byte slaveID)
    {
        //02-210 准备接料指令接收ACK
        var taskCode = agvReceivePanelPayload.body.taskCode;
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 3004, 1);
        _logger.LogInformation($"TaskCode:{taskCode},给PLC3004写入ReceivePanelAck 信号完成\r\n");
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
                var waitAck = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4005, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC4005ReceivePanelAck 信号完成\r\n");
                if (waitAck[0] != 1)
                {
                    isWaittingAck = true;
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC4005接收 ReceivePanelAck 信号···\r\n");
                    await Task.Delay(100);
                    continue;
                }
                var agvReceivePanelAckPayload = new AgvReceivePanelAckPayload()
                {
                    body = new AgvReceivePanelAckBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        code = 200,
                        msg = "指令接收成功",
                    }
                };
                await _mqttClient.PublishStringAsyncEnhance(IotTopic.RECEIVE_PANEL_ACK_TOPIC, agvReceivePanelAckPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{taskCode},给MES发送ReceivePanelAck信号完成 \r\n");
                _logger.LogInformation($"TaskCode:{taskCode},ReceivePanelAck-CommandCallback用时：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode},PLC接收到 ReceivePanelAck 清除4005信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4005, 0);
                stopwatch.Stop();
                try
                {
                    _logger.LogInformation($"TaskCode:{taskCode},动作执行中\r\n");
                    await CommandReport(agvReceivePanelPayload, slaveID);
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
            }
        }
    }

    private async Task CommandReport(AgvReceivePanelPayload agvReceivePanelPayload, byte slaveID)
    {
        //02-214 上报完成准备接料
        var taskCode = agvReceivePanelPayload.body.taskCode;
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
                var waitCompleteSilo = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4008, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC4008 接收ReceivePanel信号完成\r\n");
                if (waitCompleteSilo[0] != 1)
                {
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC 接收 ReceivePanel 信号···\r\n");
                    await Task.Delay(100);
                    continue;
                }
                var agvReceivePanelReportPayload = new AgvReceivePanelReportPayload()
                {
                    body = new AgvReceivePanelReportBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = agvReceivePanelPayload.body.taskCode,
                        status = 1,
                        msg = "接料完成"
                    }
                };
                await _mqttClient.PublishStringAsyncEnhance(IotTopic.RECEIVE_PANEL_REPORT_TOPIC, agvReceivePanelReportPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{taskCode},给MES发送ReceivePanel信号完成\r\n");
                _logger.LogInformation($"TaskCode:{taskCode},ReceivePanel-CommandReport：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode},PLC接收到 ReceivePanel 清除4008信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4008, 0);
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


    private async Task CommandStartPinReport(AgvReceivePanelPayload agvReceivePanelPayload, byte slaveID)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        while (true)
        {
            try
            {
                var value = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4016, 1);
                if (value[0] != 1)
                {
                    _logger.LogInformation($"TaskCode:{agvReceivePanelPayload.body.taskCode},等待PLC 接收 StartPin 信号···\r\n");
                    await Task.Delay(100);
                    continue;
                }
                var agvReceivePanelReportPayload = new AgvReceivePanelReportPayload()
                {
                    body = new AgvReceivePanelReportBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = agvReceivePanelPayload.body.taskCode,
                        status = 1,
                        msg = "agv开始下熟料旋转"
                    }
                };
                await _mqttClient.PublishStringAsyncEnhance(IotTopic.RECEIVE_PANEL_REPORT_TOPIC, agvReceivePanelReportPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{agvReceivePanelPayload.body.taskCode},给MES发送StartPin信号完成\r\n");
                _logger.LogInformation($"TaskCode:{agvReceivePanelPayload.body.taskCode},StartPin-CommandReport：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{agvReceivePanelPayload.body.taskCode},PLC接收到 StartPin 清除4016信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4016, 0);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"TaskCode:{agvReceivePanelPayload.body.taskCode},PLC动作执行失败\r\n");
                await Task.Delay(1000);
            }
        }
    }
}
