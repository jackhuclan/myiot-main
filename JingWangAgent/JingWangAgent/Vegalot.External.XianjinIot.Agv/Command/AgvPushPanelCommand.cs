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
/// 下发上料指令
/// </summary>
internal class AgvPushPanelCommand : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotAgvOptions _xianJinIotAgvOptions;
    private readonly ILogger<AgvPushPanelCommand> _logger;

    public AgvPushPanelCommand(
        ILogger<AgvPushPanelCommand> logger,
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        IOptions<XianJinIotAgvOptions> options,
        XianjInIotDefaultAgv device,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, mqttClient, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _xianJinIotAgvOptions = options.Value;
        _logger = logger;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        //02-103 下发装载料仓务指令
        _logger.LogInformation($"AgvPushPanelCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");

        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Message = "暂未运行逻辑";

        var isexistPayloadSegment = deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment");
        if (!isexistPayloadSegment || deviceServiceInvokeRequest.Params["PayloadSegment"] == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"PayloadSegment 参数不正确：{deviceServiceInvokeRequest.Params["PayloadSegment"]}";
            return targetResponse;
        }

        var agvPushPanelPayload = JsonSerializer.Deserialize<AgvPushPanelPayload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToStr());
        if (agvPushPanelPayload == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"AgvPushPanelPayload 实体转换出错：NUll";
            return targetResponse;
        }
        var agvIsWorking = await CheckCommandIsWorking(IotTopic.PUSH_PANEL_ACK_TOPIC, agvPushPanelPayload.body.taskCode);
        if (agvIsWorking)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"AGV正在执行任务中...";
            return targetResponse;
        }
        byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveId"].ToInt();

        try
        {
            await InitializePlcRegisterPoints(slaveID, [4004,4009], agvPushPanelPayload.body.taskCode);
           

            _ = Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation($"TaskCode:{agvPushPanelPayload.body.taskCode},ack处理中\r\n");
                    await CommandCallback(agvPushPanelPayload, slaveID);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, $"TaskCode:{agvPushPanelPayload.body.taskCode},ack处理异常\r\n");
                }
            }).ContinueWith(async t =>//执行完后初始化plc点位
            {
                await InitializePlcRegisterPoints(slaveID, [4004, 4009], agvPushPanelPayload.body.taskCode);
            });
            targetResponse.Code = ErrorCodes.Sys.SUCCESS;
            targetResponse.Message = "已接收指令";
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"TaskCode:{agvPushPanelPayload.body.taskCode},指令处理失败\r\n");
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"指令处理失败: {ex.Message}";
        }

        return targetResponse;
    }

    private async Task CommandCallback(AgvPushPanelPayload agvPushPanelPayload, byte slaveID)
    {
        //02-209 上报上料指令接收ACK
        var taskCode = agvPushPanelPayload.body.taskCode;
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 3003, 1);
        _logger.LogInformation($"TaskCode:{taskCode},给PLC3003写入PushPanelAck 信号完成\r\n");
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
                var waitAck = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4004, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC4004PushPanelAck 信号完成\r\n");
                if (waitAck[0] != 1)
                {
                    isWaittingAck = true;
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC 接收 PushPanelAck 信号···\r\n");
                    await Task.Delay(100);
                    continue;
                }
                var agvPushPanelAckPayload = new AgvPushPanelAckPayload()
                {
                    body = new AgvPushPanelAckBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        code = 200,
                        msg = "指令接收成功"
                    }
                };
                await _mqttClient.PublishStringAsyncEnhance(IotTopic.PUSH_PANEL_ACK_TOPIC, agvPushPanelAckPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{taskCode},给MES发送PushPanelAck 信号完成\r\n");
                _logger.LogInformation($"TaskCode:{taskCode},PushPanel-CommandCallback用时：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode},清除4004信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4004, 0);
                stopwatch.Stop();
                try
                {
                    _logger.LogInformation($"TaskCode:{taskCode},动作执行中\r\n");

                    await CommandReport(agvPushPanelPayload, slaveID);
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

    private async Task CommandReport(AgvPushPanelPayload agvPushPanelPayload, byte slaveID)
    {
        //02-215 上报完成上料
        var taskCode = agvPushPanelPayload.body.taskCode;
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
                var waitCompleteSilo = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4009, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC4009接收 PushPanel 信号完成\r\n");
                if (waitCompleteSilo[0] != 1)
                {
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC 接收 PushPanel 信号···\r\n");
                    await Task.Delay(100);
                    continue;
                }
                var agvPushPanelReportPayload = new AgvPushPanelReportPayload()
                {
                    body = new AgvPushPanelReportBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        status = 1,
                        msg = "上料动作完成"
                    }
                };
                await _mqttClient.PublishStringAsyncEnhance(IotTopic.PUSH_PANEL_REPORT_TOPIC, agvPushPanelReportPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{taskCode}, 给MES发送push_panel_report完成 \r\n");
                _logger.LogInformation($"TaskCode:{taskCode},PushPanel-CommandReport：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode}, PLC 接收到 PushPanel 清除4009信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4009, 0);
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
