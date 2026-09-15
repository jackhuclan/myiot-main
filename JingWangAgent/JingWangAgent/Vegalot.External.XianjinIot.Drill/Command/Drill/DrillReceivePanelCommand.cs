// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text;
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

internal class DrillReceivePanelCommand : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly XianJinIotDrillOptions _xianJinIotDrillOptions;
    private readonly ILogger<DrillReceivePanelCommand> _logger;

    public DrillReceivePanelCommand(IServiceProvider serviceProvider,
        ILogger<DrillReceivePanelCommand> logger,
        IMqttClient mqttClient,
        IOptions<XianJinIotDrillOptions> options,
        DefaultDrill device,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _xianJinIotDrillOptions = options.Value;
        _logger = logger;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        //03-101 下发准备接料指令-----皮带钻起来--读码器打开
        _logger.LogInformation($"DrillReceivePanelCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");
        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Code = ErrorCodes.Sys.FAIL;
        targetResponse.Message = "暂未开始逻辑";
        string taskCode = string.Empty;
        try
        {



            if (deviceServiceInvokeRequest == null || deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment"))
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"PayloadSegment 参数不正确";
                return targetResponse;
            }

            var drillReceivePanelPayload = JsonSerializer.Deserialize<DrillReceivePanelPayload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToString());
            if (drillReceivePanelPayload == null)
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"DrillReceivePanelPayload 实体转换出错：Null";
                return targetResponse;
            }
            var drillIsWorking = await CheckCommandIsWorking(drillReceivePanelPayload.body.taskCode);
            if (drillIsWorking)
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"钻机正在执行任务中...";
                _logger.LogInformation($"03-101  钻机正在执行任务中...");
                return targetResponse;
            }
            taskCode = drillReceivePanelPayload.body.taskCode;
            //ack  03-201
            await Ack(taskCode);
            await AddTaskCode(taskCode);

            //03-201 后续钻机动作 上报动作结束
            var response = await ActionAndReport(targetResponse, drillReceivePanelPayload, taskCode);
            if (response.Code == ErrorCodes.Sys.SUCCESS)
            {
                //启动线程判断该轴是否上料完成
                byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveID"].ToInt();
                int position = drillReceivePanelPayload.body.axleNum;//todo get
                                                                     //判断该轴有无板子
                WaitLoadComplete(deviceServiceInvokeRequest, slaveID, position, taskCode);
            }

            return targetResponse;
        }
        catch (Exception ee)
        {
            _logger.LogInformation($"DrillReceivePanelCommand  Invoke  异常{ee.Message}");
            targetResponse.Message = $"DrillReceivePanelCommand {ee.Message}";
            if (!string.IsNullOrEmpty(taskCode))
            {
                await RemoveTaskCodeRecord(taskCode);
            }
            return targetResponse;
        }
    }

    #region Complete
    public static string UshortToString(ushort[] data)
    {
        List<byte> byteData = new List<byte>();
        for (int i = 0; i < data.Length; i++)
        {
            byteData.AddRange(BitConverter.GetBytes(data[i]));
        }

        return Encoding.UTF8.GetString(byteData.ToArray());
    }
    private void WaitLoadComplete(DeviceServiceInvokeRequest deviceServiceInvokeRequest, byte slaveID, int position, string taskCode)
    {
        Task.Factory.StartNew(async (o) =>
        {
            InvokeResult invokeResult = InvokeResult.Fail();
            List<PanelInfo> panelInfos = new List<PanelInfo>();
            string message = string.Empty;
            int state = 0;
            try
            {
                var data = o.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                byte slaveIDTemp = Encoding.UTF8.GetBytes(data[0])[0];
                int positionTemp = int.Parse(data[1]);
                string taskCodeTemp = data[2];
                invokeResult = ValidataCompleteEnd(slaveIDTemp, positionTemp);
                //获取payloadpanel
                // 上报二维码信息
                message = invokeResult.Message;
                Dictionary<int, string> barcode = new Dictionary<int, string>();
                for (int i = 0; i < InteractingDevice.spindleNum; i++)
                {
                    string content = ReadContent(slaveID, i);
                    barcode.Add(i + 1, content);
                }
                var result = barcode.Where(s => s.Key == positionTemp).Where(s => !string.IsNullOrWhiteSpace(s.Value));
                if (!_xianJinIotDrillOptions.ReportBarcodeAllOnOff)
                {
                    result.Where(s => s.Key == positionTemp);

                }
                panelInfos = result.Select(s => new PanelInfo() { axleNum = s.Key, QRCode = s.Value }).ToList();

                _logger.LogDebug($"ValidataCompleteEnd  上层 panelInfos {JsonSerializer.Serialize(panelInfos)} ");

                state = invokeResult.Success ? 1 : 3;


            }
            catch (Exception e)
            {
                state = 0;
                message = $"{e.Message}";
            }
            finally
            {
                if (state != 1)
                {
                    try
                    {
                        await RemoveTaskCodeRecord(taskCode);
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["WarningInfoWriteOnPlc"].ToUshort(), 110);
                        _logger.LogDebug($"钻机给buffer报警编号：110");
                    }
                    catch (Exception)
                    {
                        _logger.LogDebug($"中控给钻机下发异常");
                    }
                }

                var drillLoadPanelReportPayload = new DrillLoadPanelReportPayload()
                {
                    body = new DrillLoadPanelReportBody()
                    {
                        sn = InteractingDevice.DeviceId,
                        taskCode = taskCode,
                        status = state,
                        msg = invokeResult.Message,
                        panelInfos = panelInfos
                    }
                };
                drillLoadPanelReportPayload.ModifyHeader();
                _logger.LogInformation($"03-203 上报上料完成  DrillReceivePanelCommand WaitLoadComplete  上报的 drillLoadPanelReportPayload {JsonSerializer.Serialize(drillLoadPanelReportPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })} ");

                await _mqttClient.PublishStringAsyncEnhance(IotTopic.LOAD_PANEL_REPORT_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillLoadPanelReportPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
            }
        }, string.Join(',', slaveID, position, taskCode));
    }

    private string ReadContent(byte slaveID, int index)
    {
        //1.读取数据个数
        var length = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra[$"Spline{index + 1}CodeReaderCodeNum"].ToUshort(), 1)[0];
        //2.读内容
        if (length == 0) return string.Empty;

        var byteContent = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra[$"Spline{index + 1}CodeReaderCodeStartPosition"].ToUshort(), length);
        //3.转成文本
        string content = UshortToString(byteContent);
        _logger.LogInformation($"03-203 读到的轴 {index + 1} 的二维码是 {content}");

        return content;
    }

    private InvokeResult ValidataCompleteEnd(byte slaveID, int position)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        string message = string.Empty;
        while (stopwatch.ElapsedMilliseconds < _xianJinIotDrillOptions.LoadCompletePrepareTimeOut)
        {
            Thread.Sleep(100);
            var raw = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 1);
            if ((raw[0] & 1 << position - 1) == 0)
            {
                _logger.LogDebug($"ValidataCompleteEnd  上料完成 轴{position} 未收到生料");
                message = $"上料完成 轴{position} 未收到生料";
                continue;
            }
            var ushorts = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReceiveBoardFlag"].ToUshort(), 1);
            // //判断是否上料动作完成
            _logger.LogDebug($"ValidataCompleteEnd {InteractingDevice.DeviceId} 上料动作未完成 寄存器的值 {ushorts[0]} ");
            if (ushorts[0] == 0)
            {
                _logger.LogDebug($"ValidataCompleteEnd  上料动作未完成");
                message = $" 轴{position} 上料动作未完成";
                continue;
            }
            _logger.LogDebug(message);
            stopwatch.Stop();
            _logger.LogDebug($"ValidataCompleteEnd  验证上料动作完成 用时 {stopwatch.ElapsedMilliseconds} ms");
            return InvokeResult.Ok("完成");
        }
        stopwatch.Stop();
        _logger.LogDebug($"ValidataCompleteEnd  上料动作未完成 超时");
        return InvokeResult.Fail($"{message} 超时");
    }

    #endregion Complete

    #region Prepare

    private async Task ExecutePrepare(byte slaveID, int position)
    {
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvUnLoadingAndLoading"].ToUshort(), 1);//  508
        //调整结束
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvArrivalEnd"].ToUshort(), 1);//  509
        await Task.Delay(InteractingDevice.DeviceDescriptor.Extra["WritePlcDelay"].ToInt());
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnLoadRequestOnPlc"].ToUshort(), 0);//  507
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferLoadRequestOnPlc"].ToUshort(), 0);//  506
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferLoadRequestOnPlc"].ToUshort(), 1);//  506
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnloadAndLoadSpline"].ToUshort(), position.ToUshort());//  514 轴号
    }

    private InvokeResult CanExecutePrepare(byte slaveID, int position)
    {
        //buffer准备上料
        var bufferOnAgvPosition = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1);
        if (!bufferOnAgvPosition[0])
        {
            _logger.LogDebug($"CanExecutePrepare-->Buffer不在agv对接层不能执行上料");
            return InvokeResult.Fail($"Buffer不在agv对接层不能执行上料-->Prepare-->84->{DeviceDescriptor.DeviceId}");
        }

        var work = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStatusFlag"].ToUshort(), 4);
        var model = work[1] == 1;
        if (!model)
        {
            _logger.LogDebug($"CanExecutePrepare-->手动状态下agv不能给Buffer上料");
            return InvokeResult.Fail($"手动状态下agv不能给Buffer上料-->Prepare-->84->{DeviceDescriptor.DeviceId}");
        }

        var raw = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 1);
        if ((raw[0] & 1 << position - 1) != 0)
        {
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["WarningInfoWriteOnPlc"].ToUshort(), (ushort)(90 + position));
            _logger.LogDebug($"CanExecutePrepare-->轴{position} 有生料不能上料");
            return InvokeResult.Fail($"轴{position} 有生料不能上料84-->Prepare-->->{DeviceDescriptor.DeviceId}");
        }

        var bufferOnWorking = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnWorking"].ToUshort(), 3);
        if (bufferOnWorking[0] > 1 || bufferOnWorking[2] > 1)
        {
            _logger.LogDebug($"CanExecutePrepare-->Buffer正在给钻机上下料：上料 {bufferOnWorking[0] > 1} 下料 {bufferOnWorking[2] > 1}");
            return InvokeResult.Fail($"Buffer正在给钻机上下料：上料 {bufferOnWorking[0] > 1} 下料 {bufferOnWorking[2] > 1}->84->{DeviceDescriptor.DeviceId}-->Prepare");
        }
        var bufferOnWorkingSingleSplindle = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnWorkingSingleSplindle"].ToUshort(), 3);
        _logger.LogDebug($"CanExecutePrepare-->agv正在给buffer 单个轴上下料：上料 {bufferOnWorkingSingleSplindle[0] > 1} 下料 {bufferOnWorkingSingleSplindle[2] > 1}");
        if (!(bufferOnWorkingSingleSplindle[0] <= 1 && bufferOnWorkingSingleSplindle[2] <= 1))
        {
            _logger.LogDebug($"CanExecutePrepare-->agv正在给buffer 单个轴上下料：上料 {bufferOnWorkingSingleSplindle[0] > 1} 下料 {bufferOnWorkingSingleSplindle[2] > 1}");
            return InvokeResult.Fail($"agv正在给buffer 单个轴上下料：上料 {bufferOnWorkingSingleSplindle[0] > 1} 下料 {bufferOnWorkingSingleSplindle[2] > 1}->84->{DeviceDescriptor.DeviceId}");
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
            var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReadyFlag"].ToUshort(), 1);
            var work = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStatusFlag"].ToUshort(), 4);
            var model = work[1] == 1;
            var raw = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 1);
            var bufferPosition = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvPositionEnd"].ToUshort(), 1);
            var readyFlag = ready[0] == 1;
            var isPower = work[0] == 1;
            var isNoError = work[2] == 0;
            var isNoFalt = work[3] == 0;
            var hasNoBoard = (raw[0] & 1 << position - 1) == 0;
            var isPosition = bufferPosition[0] == position;
            var result = readyFlag && isPower && model && isNoError && isNoFalt && isPosition;
            message = $"buffer准备状态 {readyFlag},上电状态：{isPower}, 自动模式:{model} 非报警状态:{isNoFalt} 非急停状态:{isNoFalt},buffer生料层 轴{position}无板子状态：{hasNoBoard} buffer是否定位完成{isPosition}->ValidatePrepareEnd-->84->{DeviceDescriptor.DeviceId}";
            _logger.LogDebug(message);
            if (result)
            {
                stopwatch.Stop();
                _logger.LogInformation($"ValidatePrepareEnd 用时{stopwatch.ElapsedMilliseconds} ms");

                return InvokeResult.Ok();
            }
        } while (stopwatch.ElapsedMilliseconds < _xianJinIotDrillOptions.LoadPrepareTimeOut);

        return InvokeResult.Fail($"{message} 超时");
    }

    #endregion Prepare

    #region Ack

    private async Task Ack(string taskCode)
    {
        var drillReceivePanelAckPayload = new DrillReceivePanelAckPayload()
        {
            body = new DrillReceivePanelAckBody()
            {
                sn = InteractingDevice.DeviceId,
                taskCode = taskCode,
                code = 200,
                msg = "指令接收成功",
            }
        };
        drillReceivePanelAckPayload.ModifyHeader();
        _logger.LogInformation($"03-201 上报准备接料指令接收ACK  DrillReceivePanelCommand Ack  上报的 drillReceivePanelAckPayload {JsonSerializer.Serialize(drillReceivePanelAckPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })} ");

        await _mqttClient.PublishStringAsyncEnhance(IotTopic.RECEIVE_PANEL_ACK_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillReceivePanelAckPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
    }

    #endregion Ack

    #region Invoke

    private async Task<DeviceServiceInvokeResponse> ActionAndReport(DeviceServiceInvokeResponse targetResponse, DrillReceivePanelPayload? drillReceivePanelPayload, string taskCode)
    {
        var message = "未知异常";
        int resultState = 0;
        byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveID"].ToInt();
        try
        {
            int position = drillReceivePanelPayload.body.axleNum;//todo get
            if (position <= 0 || position > InteractingDevice.spindleNum)
            {
                message = "03-101 下发准备接料指令中轴号不正确";
                resultState = 3;
            }
            var invokeResult = CanExecutePrepare(slaveID, position);
            if (invokeResult == null || !invokeResult.Success)
            {
                resultState = 4;
                message = invokeResult == null ? "执行准备的条件不满足" : invokeResult.Message;
                return targetResponse;
            }
            await ExecutePrepare(slaveID, position);//等待微调结束
            invokeResult = ValidatePrepareEnd(slaveID, position);
            if (invokeResult == null || !invokeResult.Success)
            {
                resultState = 5;
                message = invokeResult == null ? "等待微调整超时" : invokeResult.Message;
                return targetResponse;
            }
            // 皮带转起来
            await InvokeLoadMaterialLocal(slaveID, position);
            //启动一个线程监控是否上料完成
            resultState = 1;
            message = "完成";
            targetResponse.Code = ErrorCodes.Sys.SUCCESS;
            return targetResponse;
        }
        catch (Exception e)
        {
            resultState = 0;
            message = "未知异常";
            _logger.LogError($"CanExecutePrepare-->Invoke--{e.Message}");
            return targetResponse;
        }
        finally
        {
            if (resultState != 1)
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
            }
            targetResponse.Message = message;
            //上报设备 01-202
            var drillReceivePanelReportPayload = new DrillReceivePanelReportPayload()
            {
                body = new DrillReceivePanelReportBody()
                {
                    sn = InteractingDevice.DeviceId,
                    status = resultState,
                    taskCode = taskCode,
                    msg = message
                }
            };
            drillReceivePanelReportPayload.ModifyHeader();
            _logger.LogInformation($"03-202 上报准备接料完成  DrillReceivePanelCommand ActionAndReport  上报的 drillReceivePanelReportPayload {JsonSerializer.Serialize(drillReceivePanelReportPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })} ");

            await _mqttClient.PublishStringAsyncEnhance(IotTopic.RECEIVE_PANEL_REPORT_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillReceivePanelReportPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
        }
    }

    private async Task<DeviceServiceInvokeResponse> InvokeLoadMaterialLocal(byte slaveID, int position)
    {
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
        SetPosition(slaveID, position.ToUshort());
        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort(), 1);
        try
        {
            var codeReaderOnOff = InteractingDevice.DeviceDescriptor.Extra["CodeReaderOnOff"].ToBool();
            if (codeReaderOnOff)
            {
                _ = Task.Factory.StartNew((index) =>
                 {
                     try
                     {
                         int codeIndex = (int)index;
                         InteractingDevice.codeReaderBaseSetting.SendScanMesssageToSignal(codeIndex, "1", true);
                         _logger.LogInformation($"轴{codeIndex + 1} 自动触发读码 线程开始");
                         Task.Delay(2000).Wait();
                         do
                         {
                             ushort curTrigger = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort(), 1)[0];
                             var codeReaderNum = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra[$"Spline{codeIndex + 1}CodeReaderCodeNum"].ToUshort(), 1)[0];
                             if (curTrigger == 0 || codeReaderNum != 0)
                             {
                                 break;
                             }
                             InteractingDevice.codeReaderBaseSetting.SendScanMesssageToSignal(codeIndex, "1");
                             Task.Delay(3000).Wait();
                         } while (true);
                         _logger.LogInformation($"轴{codeIndex + 1}  自动触发读码 线程结束");
                     }
                     catch (Exception ee)
                     {
                         _logger.LogDebug($"轴{position}开始上料 多次读码器异常 {ee.Message}");
                     }
                 }, position - 1);
            }
        }
        catch (Exception e)
        {
            _logger.LogDebug($"轴{position}开始上料读码器异常 {e.Message}");
        }

        _logger.LogDebug($"轴{position}开始上料");
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
