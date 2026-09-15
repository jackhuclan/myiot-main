// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.DependencyInjection;
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
/// 下发AGV调整到指定高度
/// </summary>
internal class AgvAdjustHeightCommand : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotAgvOptions _xianJinIotAgvOptions;
    private readonly ILogger<AgvAdjustHeightCommand> _logger;

    public AgvAdjustHeightCommand(
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        IOptions<XianJinIotAgvOptions> options,
        XianjInIotDefaultAgv device,
        CommandDescriptor commandDescriptor,
        ILogger<AgvAdjustHeightCommand> logger)
        : base(serviceProvider, device, mqttClient, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _xianJinIotAgvOptions = options.Value;
        _logger = logger;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        //02-104 下发指定料仓层上升到指定高度指令
        _logger.LogInformation($"AgvAdjustHeightCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");

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
        var agvAdjustHeightPayload = new AgvAdjustHeightPayload();
         agvAdjustHeightPayload = JsonSerializer.Deserialize<AgvAdjustHeightPayload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToStr());
        if (agvAdjustHeightPayload == null)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"AgvAdjustHeightPayload 实体转换出错：NUll";
            return targetResponse;
        }                       
        var agvIsWorking = await CheckCommandIsWorking(IotTopic.ADJUST_HEIGHT_ACK_TOPIC, agvAdjustHeightPayload.body.taskCode);
        if (agvIsWorking)
        {
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"AGV正在执行任务中...";
            return targetResponse;
        }
        byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveId"].ToInt();
        try
        {
            await InitializePlcRegisterPoints(slaveID, [4000,4001], agvAdjustHeightPayload.body.taskCode);
            //异步等待plc任务完成
            _ = Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation($"TaskCode:{agvAdjustHeightPayload.body.taskCode},ack处理中\r\n");
                    await CommandCallback(agvAdjustHeightPayload, slaveID);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, $"TaskCode:{agvAdjustHeightPayload.body.taskCode},ack处理异常\r\n");
                }
            }).ContinueWith(async t =>//执行完后初始化plc点位
            {
                await InitializePlcRegisterPoints(slaveID, [4000, 4001], agvAdjustHeightPayload.body.taskCode);
            });

            targetResponse.Code = ErrorCodes.Sys.SUCCESS;
            targetResponse.Message = "已接收指令";
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"TaskCode:{agvAdjustHeightPayload.body.taskCode},指令处理失败\r\n");
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"指令处理失败: {ex.Message}";
        }
        return targetResponse;
    }

    private async Task CommandCallback(AgvAdjustHeightPayload agvAdjustHeightPayload, byte slaveID)
    {
        //02-203 上报装载料仓指令接收ACK
        var taskCode = agvAdjustHeightPayload.body.taskCode;
        var height = agvAdjustHeightPayload.body.height;
        ushort register = height > ushort.MaxValue ? (ushort)3002 : (ushort)3001;
        if (register == 3001)
        {
            //var oldValue = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 3001, 1);//3001 用作命令提升高度时作业使用的点位，记录要提升的高度值
            //var realValue = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 3031, 1);//代表agv插齿真实实时高度，所以只需要命令高度和真实高度比较即可
            //if (realValue[0] == (ushort)height )
            //{
            //    _logger.LogInformation($"agvAdjustHeightPayload:高度一致，不需要等待ACK，直接默认到达指定高度oldValue:{oldValue[0]},height:{height},realValue{realValue[0]}");
            //    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4000, 1);//高度一致，直接默认到达指定高度
            //    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4001, 1);//高度一致，直接默认到达指定高度
            //    noWaitPlcAck = true;
            //}
        }       
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, register, (ushort)height);
        _logger.LogInformation($"agvAdjustHeightPayload:{JsonSerializer.Serialize(agvAdjustHeightPayload)}");
        _logger.LogInformation($"height:{height},TaskCode:{taskCode},给PLC{register}写入 AdjustHeightAck 信号完成···\r\n");

        bool isWaittingAck = false;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        while (true) //
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
                var waitAck = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4000, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC的4000 AdjustHeightAck 信号完成···\r\n");
                _logger.LogInformation($"mqtt状态：{_mqttClient.IsConnected}");
                if (waitAck[0] != 1)
                {
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC 接收 AdjustHeightAck 信号···\r\n");
                    isWaittingAck = true;
                    await Task.Delay(100);
                    continue;
                                    
                }
                var agvAdjustHeightAckPayload = new AgvAdjustHeightAckPayload()
                {
                    body = new AgvAdjustHeightAckBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        code = 200,
                        msg = "指令接收成功",
                    }
                };
                await _mqttClient.PublishStringAsyncEnhance(IotTopic.ADJUST_HEIGHT_ACK_TOPIC, agvAdjustHeightAckPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{taskCode},给MES发送ADJUST_HEIGHT_ACK_TOPIC信号完成 \r\n");
                _logger.LogInformation($"TaskCode:{taskCode},AdjustHeightAck-CommandCallback用时：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode},PLC 已接收到 AdjustHeightAck 清除4000信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4000, (ushort)0);
                stopwatch.Stop();
                try
                {
                    _logger.LogInformation($"TaskCode:{taskCode},动作执行中\r\n");
                    await CommandReport(agvAdjustHeightPayload, slaveID);
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

    private async Task CommandReport(AgvAdjustHeightPayload agvAdjustHeightPayload, byte slaveID)
    {
        //02-204 上报装载料仓完成
        var taskCode = agvAdjustHeightPayload.body.taskCode;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        while (true)
        {

            try
            {
                var isInit = await InitializeAgvAgent(slaveID);
                if (isInit)
                {
                    stopwatch.Stop();
                    await RemoveTaskCodeRecord(taskCode);
                    return;
                }
                var waitCompleteSilo = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 4001, 1);
                _logger.LogInformation($"TaskCode:{taskCode},读取PLC的4001 AdjustHeight 信号完成···\r\n");
                if (waitCompleteSilo[0] != 1)
                {
                    _logger.LogInformation($"TaskCode:{taskCode},等待PLC 接收 AdjustHeight 信号···\r\n");
                    await Task.Delay(100);
                    continue;
                }

                var agvAdjustHeightReportPayload = new AgvAdjustHeightReportPayload()
                {
                    body = new AgvAdjustHeightReportBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        status = 1,
                        msg = "到达指定高度"
                    }
                };

                await _mqttClient.PublishStringAsyncEnhance(IotTopic.ADJUST_HEIGHT_END_TOPIC, agvAdjustHeightReportPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                _logger.LogInformation($"TaskCode:{taskCode},给MES发送adjust_height_report信号完成 \r\n");
                _logger.LogInformation($"TaskCode:{taskCode},已接收到AdjustHeight-CommandReport：【{stopwatch.ElapsedMilliseconds}】ms");
                _logger.LogInformation($"TaskCode:{taskCode}, PLC 已接收到AdjustHeight 清除4001信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 4001, 0);
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
   
