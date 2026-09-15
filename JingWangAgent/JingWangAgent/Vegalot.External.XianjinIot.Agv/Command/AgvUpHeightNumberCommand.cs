// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
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

internal class AgvUpHeightNumberCommand : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotAgvOptions _xianJinIotAgvOptions;
    private readonly ILogger<AgvUpHeightNumberCommand> _logger;

    public AgvUpHeightNumberCommand(
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        IOptions<XianJinIotAgvOptions> options,
        XianjInIotDefaultAgv device,
        CommandDescriptor commandDescriptor,
        ILogger<AgvUpHeightNumberCommand> logger)
        : base(serviceProvider, device, mqttClient, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _xianJinIotAgvOptions = options.Value;
        _logger = logger;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        //02-110 下发提升机构运行的层数
        _logger.LogInformation($"AgvUpHeightNumberCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");

        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Message = "暂未运行逻辑";

        var isexistPayloadSegment = deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment");
        if (!isexistPayloadSegment || deviceServiceInvokeRequest.Params["PayloadSegment"] == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"PayloadSegment 参数不正确：{deviceServiceInvokeRequest.Params["PayloadSegment"]}";
            return targetResponse;
        }

        var agvUpHeightNumberPayload = JsonSerializer.Deserialize<AgvUpHeightNumberPayload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToStr());
        if (agvUpHeightNumberPayload == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"AgvUpHeightNumberPayload 实体转换出错：NUll";
            return targetResponse;
        }
        var agvIsWorking = await CheckCommandIsWorking(IotTopic.HEIGHT_NUMBER_ACK, agvUpHeightNumberPayload.body.taskCode);
        if (agvIsWorking)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"AGV正在执行任务中...";
            return targetResponse;
        }
        byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveId"].ToInt();

        try
        {
            await InitializePlcRegisterPoints(slaveID, [4010, 4001], agvUpHeightNumberPayload.body.taskCode);
            _=Task.Run(async() =>
            {
                try
                {
                    _logger.LogInformation($"TaskCode:{agvUpHeightNumberPayload.body.taskCode},ack处理中\r\n");
                    await CommandCallback(agvUpHeightNumberPayload, slaveID);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, $"TaskCode:{agvUpHeightNumberPayload.body.taskCode},ack处理异常\r\n");
                }
            }).ContinueWith(async t =>//执行完后初始化plc点位
            {
                await InitializePlcRegisterPoints(slaveID, [4010, 4001], agvUpHeightNumberPayload.body.taskCode);
            }); 
            targetResponse.Code = ErrorCodes.Sys.SUCCESS;
            targetResponse.Message = "已接收指令";
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"TaskCode:{agvUpHeightNumberPayload.body.taskCode},指令处理失败\r\n");
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"指令处理失败: {ex.Message}";
        }

        return targetResponse;
    }

    private async Task CommandCallback(AgvUpHeightNumberPayload agvUpHeightNumberPayload, byte slaveID)
    {
        //02-205 上报装载料仓指令接收ACK
        var taskCode = agvUpHeightNumberPayload.body.taskCode;
        var heightNum = agvUpHeightNumberPayload.body.siloFloor;
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 3008, (ushort)heightNum);
        _logger.LogInformation($"TaskCode:{taskCode},给PLC3008写入UpHeightNumberAck 信号完成\r\n");
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
                var waitAck = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4010, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC4010UpHeightNumberAck 信号完成\r\n");
                if (waitAck[0] != 1)
                {
                    isWaittingAck = true;
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC 接收 UpHeightNumberAck 信号···\r\n");
                    await Task.Delay(100);
                    continue;
                }

                var agvUpHeightNumberAckPayload = new AgvUpHeightNumberAckPayload()
                {
                    body = new AgvUpHeightNumberAckBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        code = 200,
                        msg = "指令接收成功",
                    }
                };

                    await _mqttClient.PublishStringAsyncEnhance(IotTopic.HEIGHT_NUMBER_ACK, agvUpHeightNumberAckPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                    _logger.LogInformation($"TaskCode:{taskCode},给MES发送UpHeightNumberAck信号完成\r\n");
              

                _logger.LogInformation($"TaskCode:{taskCode},UpHeightNumberAck-CommandCallback用时：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode},PLC接收到 UpHeightNumberAck 清除4010信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4010, 0);
                stopwatch.Stop();
                try
                {
                    _logger.LogInformation($"TaskCode:{taskCode},动作执行中\r\n");

                    await CommandReport(agvUpHeightNumberPayload, slaveID);
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

    private async Task CommandReport(AgvUpHeightNumberPayload agvUpHeightNumberPayload, byte slaveID)
    {
        //02-206 上报装载料仓完成
        var taskCode = agvUpHeightNumberPayload.body.taskCode;
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
                var waitCompleteSilo = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4001, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC4001UpHeightNumber信号完成\r\n");
                if (waitCompleteSilo[0] != 1)
                {
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC 接收 UpHeightNumber 信号···\r\n");
                    await Task.Delay(100);
                    continue;
                }
                var agvUpHeightNumerReportPayload = new AgvUpHeightNumberReportPayload()
                {
                    body = new AgvUpHeightNumberReportBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        status = 1,
                        msg = "提升料仓指定层",
                    }
                };

                await _mqttClient.PublishStringAsyncEnhance(IotTopic.ADJUST_HEIGHT_END_TOPIC, agvUpHeightNumerReportPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{taskCode},给MES发送UpHeightNumber信号完成 \r\n");
                _logger.LogInformation($"TaskCode:{taskCode},UpHeightNumberAck-CommandReport：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode},PLC接收到 UpHeightNumber 清除4001信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4001, 0);
                stopwatch.Stop();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"TaskCode:{taskCode},PLC动作执行失败\r\n");
            }
        }   
    }
}
