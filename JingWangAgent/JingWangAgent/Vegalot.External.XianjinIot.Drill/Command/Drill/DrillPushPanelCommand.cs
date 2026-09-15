// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using Vegalot.External.XianjinIot.Drill.Models;
using Vegalot.External.XianjinIot.Drill.Models.Ack;
using Vegalot.External.XianjinIot.Drill.Models.Command;
using Vegalot.External.XianjinIot.Drill.Models.Report;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Drill;

namespace Vegalot.External.XianjinIot.Drill.Command.Drill;

internal class DrillPushPanelCommand : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotDrillOptions _options;
    private readonly ILogger<DrillPushPanelCommand> _logger;
    private readonly XianJinIotDrillOptions _xianJinIotOptions;

    public DrillPushPanelCommand(
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        ILogger<DrillPushPanelCommand> logger,
        IOptions<XianJinIotDrillOptions> options,
        DefaultDrill device,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _options = options.Value;
        _logger = logger;
        _xianJinIotOptions = options.Value;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        //03-104 下发钻孔任务指令
        _logger.LogInformation($"DrillPushPanelCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");
        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Code = ErrorCodes.Sys.FAIL;
        targetResponse.Message = "暂未开始逻辑";
        string taskcode = string.Empty;
        try
        {
            if (deviceServiceInvokeRequest == null || deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment") || string.IsNullOrEmpty(deviceServiceInvokeRequest.Params["PayloadSegment"]!.ToString()))
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"DrillPushPanelCommand PayloadSegment 参数不正确";
                return targetResponse;
            }

            var drillPushPanelPayload = JsonSerializer.Deserialize<DrillPushPanelPayload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToString());
            if (drillPushPanelPayload == null)
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"drillPushPanelPayload 实体转换出错：Null";
                return targetResponse;
            }


            var drillIsWorking = await CheckCommandIsWorking(drillPushPanelPayload.body.taskCode);
            if (drillIsWorking)
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"钻机正在执行任务中...";
                _logger.LogInformation($"03-104  钻机正在执行任务中...");
                return targetResponse;
            }
            taskcode = drillPushPanelPayload.body.taskCode;
            await Ack(drillPushPanelPayload);// 准备 皮带转起来
            await AddTaskCode(drillPushPanelPayload.body.taskCode);
            //
            //  await ActionAndReport(targetResponse, drillPushPanelPayload);
            //启动线程判断该轴是否上料完成
            byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveID"].ToInt();
            int position = drillPushPanelPayload.body.axleNum;//todo get
            var taskCode = drillPushPanelPayload.body.taskCode;
            //判断该轴有无板子
            WaitUnloadComplete(deviceServiceInvokeRequest, slaveID, position, taskCode);
            return targetResponse;
        }
        catch (Exception ee)
        {
            _logger.LogInformation($"DrillPushPanelCommand  Invoke  异常{ee.Message}");
            targetResponse.Message = $"DrillPushPanelCommand {ee.Message}";
            if (!string.IsNullOrEmpty(taskcode))
            {
                await RemoveTaskCodeRecord(taskcode);
            }
            return targetResponse;
        }
    }

    private async Task Ack(DrillPushPanelPayload? drillPushPanelPayload)
    {
        //03-204 钻孔任务指令接收ack
        var drillPushPanelAckPayload = new DrillPushPanelAckPayload()
        {
            body = new DrillPushPanelAckBody()
            {
                sn = InteractingDevice.DeviceId,
                taskCode = drillPushPanelPayload.body.taskCode,
                code = 200,
                msg = "指令接收成功",
            }
        };
        drillPushPanelAckPayload.ModifyHeader();
        _logger.LogInformation($"03-210 上报接收退板指令接收ACK DrillPushPanelCommand   drillPushPanelPayload {JsonSerializer.Serialize(drillPushPanelAckPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })}");

        await _mqttClient.PublishStringAsyncEnhance(IotTopic.PUSH_PANEL_ACK_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillPushPanelAckPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
    }

    #region Complete

    private void WaitUnloadComplete(DeviceServiceInvokeRequest deviceServiceInvokeRequest, byte slaveID, int position, string taskCode)
    {
        Task.Factory.StartNew(async (o) =>
        {
            _logger.LogInformation($"WaitUnloadComplete  {o.ToString()}");
            InvokeResult invokeResult = InvokeResult.Fail();
            string message = string.Empty;
            int state = 0;
            try
            {
                var data = o.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                byte slaveIDTemp = Convert.ToByte(data[0]);
                int positionTemp = int.Parse(data[1]);
                string taskCodeTemp = data[2];

                byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveID"].ToInt();
                int position = positionTemp;//todo get
                if (position <= 0 || position > InteractingDevice.spindleNum)
                {
                    message = "03-105 下发退板指令 轴号不正确";
                    _logger.LogError(message);
                    return;
                }
                // 判断是否有板子
                var clinker = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferClinkerStatusOnPlc"].ToUshort(), 1); //145
                if ((clinker[0] & 1 << position - 1) == 0)
                {
                    var errorMessage = $"轴{position}无熟料不能下料84->{DeviceDescriptor.DeviceId}";
                    _logger.LogError(errorMessage);
                    message = errorMessage;
                    state = 2;
                    return;
                }
                // 准备
                invokeResult = CanExecutePrepare(slaveID, position);
                if (invokeResult == null || !invokeResult.Success)
                {
                    message = invokeResult == null ? "执行下料准备的条件不满足" : invokeResult.Message;
                    _logger.LogError(message);
                    return;
                }
                await ExecutePrepare(slaveID, position);//等待微调结束
                invokeResult = ValidatePrepareEnd(slaveID, position);
                if (invokeResult == null || !invokeResult.Success)
                {
                    message = invokeResult == null ? "等待微调整超时" : invokeResult.Message;
                    _logger.LogError(message);
                    return;
                }
                // 皮带转起来
                await InvokeUnloadMaterialLocal(slaveID, position);

                invokeResult = ValidataCompleteEnd(slaveIDTemp, positionTemp);

                message = invokeResult.Message;

                state = invokeResult.Success ? 1 : 0;
            }
            catch (Exception e)
            {
                state = 0;
                message = $"{e.Message}";
            }
            finally
            {
                var drillPushPanelReportPayload = new DrillPushPanelReportPayload()
                {
                    body = new DrillPushPanelReportBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        status = state,
                        msg = message,
                    }
                };
                drillPushPanelReportPayload.ModifyHeader();
                if (state != 1)
                {
                    await RemoveTaskCodeRecord(taskCode);
                }
                _logger.LogInformation($"03-211 上报退板完成   DrillPushPanelCommand ValidataCompleteEnd  上报的 drillPushPanelReportPayload {JsonSerializer.Serialize(drillPushPanelReportPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })} ");
                await _mqttClient.PublishStringAsyncEnhance(IotTopic.PUSH_PANEL_REPORT_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillPushPanelReportPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
            }
        }, string.Join(',', slaveID, position, taskCode));
    }

    private InvokeResult ValidataCompleteEnd(byte slaveID, int position)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        string message = string.Empty;
        while (stopwatch.ElapsedMilliseconds < _xianJinIotOptions.UnloadCompletePrepareTimeOut)
        {
            Thread.Sleep(100);

            var isfinished = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnloadOKOnPlc"].ToUshort(), 1);// 409
            _logger.LogDebug($"ValidataCompleteEnd {InteractingDevice.DeviceId} 上下料未完成 寄存器的值 {isfinished[0]} ");
            if (isfinished[0] != 1)
            {
                _logger.LogDebug($"ValidataCompleteEnd  下料动作未完成");
                message = $" 轴{position} 下料动作未完成";
                continue;
            }

            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferIsLoadAndUnloadOnPlc"].ToUshort(), 0);
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnloadOKOnPlc"].ToUshort(), 0);// 409 BufferUnloadOKOnPlc
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvArrivalEnd"].ToUshort(), 0);

            _logger.LogDebug("下料完成");
            stopwatch.Stop();
            _logger.LogDebug($"ValidataCompleteEnd  验证下料动作完成 用时 {stopwatch.ElapsedMilliseconds} ms");
            return InvokeResult.Ok("完成");
        }
        stopwatch.Stop();
        _logger.LogDebug($"ValidataCompleteEnd  下料动作未完成 超时");
        return InvokeResult.Fail($"{message} 超时");
    }

    #endregion Complete

    #region Prepare

    private async Task ExecutePrepare(byte slaveID, int position)
    {
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvUnLoadingAndLoading"].ToUshort(), 1);

        //调整结束
        _logger.LogDebug($"不在单个轴 上下过程中  将要赋值509---506---507");
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvArrivalEnd"].ToUshort(), 1);
        await Task.Delay(InteractingDevice.DeviceDescriptor.Extra["WritePlcDelay"].ToInt());
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferLoadRequestOnPlc"].ToUshort(), 0); // 506
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnLoadRequestOnPlc"].ToUshort(), 0); // 507
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnLoadRequestOnPlc"].ToUshort(), 1); // 507
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnloadAndLoadSpline"].ToUshort(), position.ToUshort());// 514 轴号
    }

    private InvokeResult CanExecutePrepare(byte slaveID, int position)
    {
        var bufferOnAgvPosition = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1);
        if (!bufferOnAgvPosition[0])
        {
            _logger.LogDebug($"Buffer不在agv对接层不能执行下料");
            return InvokeResult.Fail($"Buffer不在agv对接层不能执行下料-->Prepare-->84->{DeviceDescriptor.DeviceId}");
        }
        var work = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStatusFlag"].ToUshort(), 4);
        var model = work[1] == 1;
        if (!model)
        {
            _logger.LogDebug($"手动状态下agv不能给Buffer下料");
            return InvokeResult.Fail($"手动状态下agv不能给Buffer下料-->Prepare-->84->{DeviceDescriptor.DeviceId}");
        }
        var clinker = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferClinkerStatusOnPlc"].ToUshort(), 1); //145
        if ((clinker[0] & 1 << position - 1) == 0)
        {
            var errorMessage = $"轴{position}无熟料不能下料84->{DeviceDescriptor.DeviceId}";
            _logger.LogError(errorMessage);
            return InvokeResult.Fail($"轴{position}无熟料不能下料-->Prepare-->84->{DeviceDescriptor.DeviceId}");
        }
        var bufferOnWorking = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnWorking"].ToUshort(), 3);
        if (bufferOnWorking[0] > 1 || bufferOnWorking[2] > 1)
        {
            _logger.LogDebug($"Buffer正在给钻机上下料：上料 {bufferOnWorking[0] > 1} 下料 {bufferOnWorking[2] > 1}");
            return InvokeResult.Fail($"Buffer正在给钻机上下料：上料 {bufferOnWorking[0] > 1} 下料 {bufferOnWorking[2] > 1}-->Prepare-->84->{DeviceDescriptor.DeviceId}");
        }
        return InvokeResult.Ok();
    }

    private InvokeResult ValidatePrepareEnd(byte slaveID, int position)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        string message = string.Empty;
        do
        {
            Thread.Sleep(100);
            var bufferPosition = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvPositionEnd"].ToUshort(), 1);
            var work = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStatusFlag"].ToUshort(), 4);
            var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReadyFlag"].ToUshort(), 1);
            var clinker = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferClinkerStatusOnPlc"].ToUshort(), 1); //145
            var model = work[1] == 1;
            var readyFlag = ready[0] == 1;
            var isPower = work[0] == 1;
            var isNoError = work[2] == 0;
            var isNoFalt = work[3] == 0;
            var hasBoard = (clinker[0] & 1 << position - 1) == 1 << position - 1;
            var isPosition = bufferPosition[0] == 10 + position;
            var result = readyFlag && isPower && model && isNoError && isNoFalt && isPosition;
            message = $"机器尚未准备好:buffer准备状态 {readyFlag},上电状态：{isPower}, 自动模式:{model} 非报警状态:{isNoFalt} 非急停状态:{isNoFalt},buffer熟料层 轴：{position}有板子状态：{hasBoard}  buffer是否定位完成{isPosition}->84->{DeviceDescriptor.DeviceId}";
            _logger.LogDebug($"下熟料 DrillPushPanelCommand ValidatePrepareEnd :{message}");
            if (result)
            {
                stopwatch.Stop();
                return InvokeResult.Ok();
            }
        } while (stopwatch.ElapsedMilliseconds < _xianJinIotOptions.UnloadPrepareTimeOut);

        return InvokeResult.Fail($"{message} 超时");
    }

    #endregion Prepare

    #region Invoke

    private async Task<DeviceServiceInvokeResponse> ActionAndReport(DeviceServiceInvokeResponse targetResponse, DrillPushPanelPayload? drillPushPanelPayload)
    {
        var message = "未知异常";
        try
        {
            byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveID"].ToInt();
            int position = drillPushPanelPayload.body.axleNum;//todo get
            if (position <= 0 || position > InteractingDevice.spindleNum)
            {
                message = "03-105 下发退板指令 轴号不正确";
            }
            var invokeResult = CanExecutePrepare(slaveID, position);
            if (invokeResult == null || !invokeResult.Success)
            {
                message = invokeResult == null ? "执行下料准备的条件不满足" : invokeResult.Message;
                return targetResponse;
            }
            await ExecutePrepare(slaveID, position);//等待微调结束
            invokeResult = ValidatePrepareEnd(slaveID, position);
            if (invokeResult == null || !invokeResult.Success)
            {
                message = invokeResult == null ? "等待微调整超时" : invokeResult.Message;
                return targetResponse;
            }
            // 皮带转起来
            await InvokeUnloadMaterialLocal(slaveID, position);
            message = "完成";
            return targetResponse;
        }
        catch (Exception e)
        {
            message = "未知异常";
            _logger.LogError($"CanExecutePrepare-->Invoke--{e.Message}");
            return targetResponse;
        }
        finally
        {
            //上报 下料准备完成
        }
    }

    private async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterialLocal(byte slaveID, int position)
    {
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferIsLoadAndUnloadOnPlc"].ToUshort(), 0);
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
        SetPosition(slaveID, position.ToUshort());
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStartUnLoadingBoard"].ToUshort(), 1);
        _logger.LogDebug($"轴{position}开始下料");
        return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
    }

    private void SetPosition(byte slaveID, ushort position)
    {
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReceiveBoardFlag"].ToUshort(), 0);
        InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort(), new ushort[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnloadAndLoadSpline"].ToUshort(), position);
    }

    #endregion Invoke
}
