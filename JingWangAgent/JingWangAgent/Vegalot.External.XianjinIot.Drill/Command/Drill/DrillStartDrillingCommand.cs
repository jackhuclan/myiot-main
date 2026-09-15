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

internal class DrillStartDrillingCommand : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly ILogger<DrillStartDrillingCommand> _logger;
    private readonly XianJinIotDrillOptions _xianJinIotOptions;
    private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();


    public DrillStartDrillingCommand(
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        ILogger<DrillStartDrillingCommand> logger,
        IOptions<XianJinIotDrillOptions> options,
        DefaultDrill device,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _logger = logger;
        _xianJinIotOptions = options.Value;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        //03-104 下发钻孔任务指令
        _logger.LogInformation($"DrillStartDrillingCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");
        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Code = ErrorCodes.Sys.FAIL;
        targetResponse.Message = "暂未开始逻辑";
        string taskCode = string.Empty;
        try
        {

            if (deviceServiceInvokeRequest == null || deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment") || string.IsNullOrEmpty(deviceServiceInvokeRequest.Params["PayloadSegment"]!.ToString()))
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"DrillStartDrillingCommand PayloadSegment 参数不正确";
                return targetResponse;
            }

            var drillStartDrillingPayload = JsonSerializer.Deserialize<DrillStartDrillingPayload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToString());
            if (drillStartDrillingPayload == null)
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"drillStartDrillingPayload 实体转换出错：Null";
                return targetResponse;
            }
            var drillIsWorking = await CheckCommandIsWorking(drillStartDrillingPayload.body.taskCode);
            if (drillIsWorking)
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"钻机正在执行任务中...";
                _logger.LogInformation($"03-101  钻机正在执行任务中...");
                return targetResponse;
            }
            taskCode = drillStartDrillingPayload.body.taskCode;

            CommonModel.TempDrillTaskCode = drillStartDrillingPayload.body.taskCode;
            //_cancellationTokenSource.Cancel();
            //_cancellationTokenSource = new CancellationTokenSource();
            //StartDrillingTaskReport();


            //03-204 钻孔任务指令接收ack
            var drillStartDrillingAckPayload = new DrillStartDrillingAckPayload()
            {
                body = new DrillStartDrillingAckBody()
                {
                    sn = InteractingDevice.DeviceId,
                    taskCode = drillStartDrillingPayload.body.taskCode,
                    code = 200,
                    msg = "指令接收成功",
                }
            };
            drillStartDrillingAckPayload.ModifyHeader();
            _logger.LogInformation($"03-207   上报开始钻机指令接收ACK : DrillStartDrillingCommand Invoke  上报的 drillStartDrillingAckPayload {JsonSerializer.Serialize(drillStartDrillingAckPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })} ");

            await _mqttClient.PublishStringAsyncEnhance(IotTopic.START_DRILLING_ACK_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillStartDrillingAckPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);

            await AddTaskCode(taskCode);
            //上报开始钻孔
            // StartDrillingAndReport();
            StartDrillingAndReport(drillStartDrillingPayload);

            targetResponse.Code = ErrorCodes.Sys.SUCCESS;
            targetResponse.Message = "创建完成";
            return targetResponse;
        }
        catch (Exception ee)
        {
            _logger.LogInformation($"DrillStartDrillingCommand  Invoke  异常{ee.Message}");
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"DrillStartDrillingCommand {ee.Message}";
            if (!string.IsNullOrEmpty(taskCode))
            {
                await RemoveTaskCodeRecord(taskCode);
            }
            return targetResponse;
        }
    }

    private void StartDrillingTaskReport()
    {
        Task.Factory.StartNew(async () =>
        {
            _logger.LogError($"上报钻机任务状态  开始了 ");
            Dictionary<string, int> model = new Dictionary<string, int>()
            {
                { "WORK",1},
                { "STOP",2},
                { "ALAM",0},
                { "IDLE",1},
                { "SERV",0},
                { "WAIT",1},
                { "",1},
            };
            var oldPercentage = 0;
            CommonModel.DrillTaskCode = CommonModel.TempDrillTaskCode;
            var oldClinkerState = 0;
            bool isFirstRun = true;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {

                    var values = InteractingDevice.WatchingProperties.GetValues();
                    var data = string.Join(",\r\n", values.Select(s => string.Join(":", s.Key, s.Value)));
                    _logger.LogInformation($"上报机器属性  {data} ");
                    //机器状态
                    var state = values["Drill_State"] != null ? values["Drill_State"]!.ToString() : string.Empty;
                    var status = model.ContainsKey(state) ? model[state] : 0;
                    var schedule = values["Drill_Percentage"] != null ? values["Drill_Percentage"].ToInt() : 0;

                    if (schedule != oldPercentage)
                    {
                        if (schedule == 0)
                        {
                            if (values["Drill_Percentage"] == null)
                            {
                                _logger.LogError($"获取不到进度属性  {data} ");
                                schedule = oldPercentage;
                            }
                            else
                            {
                                if (oldPercentage != 100)
                                {
                                    _logger.LogError($"中间手动点击从制定孔开始  {data} ");
                                    schedule = oldPercentage;
                                }

                            }
                        }

                        oldPercentage = schedule;
                    }
                    _logger.LogError($"钻机状态： state {state}  上报的值 {status}  进度{schedule}  CommonModel.TempDrillTaskCode:{CommonModel.TempDrillTaskCode} CommonModel.DrillTaskCode:{CommonModel.DrillTaskCode}");
                    if (isFirstRun)
                    {
                        isFirstRun = false;
                        if (values.ContainsKey("Buffer_ClinkerLayerBoardStatus"))
                        {
                            oldClinkerState = values["Buffer_ClinkerLayerBoardStatus"]!.ToInt();
                        }
                    }
                    if (schedule == 100)
                    {
                        var clinker = values["Buffer_ClinkerLayerBoardStatus"] != null ? values["Buffer_ClinkerLayerBoardStatus"]!.ToInt() : 0;
                        //判断钻机上面层有料
                        var drillExistBoard = values["Drill_BoardPositionStatus"] != null ? values["Drill_BoardPositionStatus"]!.ToInt() : 0;

                        if (clinker != 0 && drillExistBoard == 0)
                        {
                            status = 4;
                            state = "完成";
                        }
                        if (drillExistBoard != 0)
                        {
                            status = 1;
                            state = "进行中";
                        }
                    }

                    var drillDrillingTaskReportPayload = new DrillDrillingTaskReportPayload()
                    {
                        body = new DrillDrillingTaskReportBody()
                        {
                            sn = InteractingDevice.DeviceId,
                            taskCode = CommonModel.DrillTaskCode,
                            status = status,
                            msg = state,
                            schedule = schedule
                        }
                    };

                    _logger.LogError($"03-209 上报钻孔进度 {JsonSerializer.Serialize(drillDrillingTaskReportPayload, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                    })} ");
                    await _mqttClient.PublishStringAsyncEnhance(IotTopic.DRILLING_TASK_REPORT_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillDrillingTaskReportPayload, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                    })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                    if (status == 4)
                    {
                        _logger.LogError($"上报钻机任务状态  状态变成4 结束线程 ");
                        break;
                    }

                    Thread.Sleep(5000);
                }
                catch (Exception)
                {
                }
            }

            _logger.LogError($"上报钻机任务状态  结束了 ");
        });
    }


    private async Task StartDrillingAndReport(DrillStartDrillingPayload drillStartDrillingPayload)
    {
        string message = string.Empty;
        int status = 0;
        try
        {

            //选轴
            _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}  选轴 ct时间 发送开始");
            if (!RetractTool())
            {
                message = "选轴的时候先退刀，退刀失败";
                _logger.LogError($"选轴的时候先退刀，退刀失败 ");
                //InteractingDevice.cnc84Command.SetCncComand($"DSP,选轴的时候先退刀，退刀失败");
                return;
            }
            var cncStatus = InteractingDevice.cnc84Command.GetCncStatus();

            var splineStatus = cncStatus?.SpindleStatus;
            if (splineStatus != null)
            {
                splineStatus = splineStatus?.Substring(splineStatus.Length - InteractingDevice.spindleNum);
                char[] arr = splineStatus.ToCharArray();
                Array.Reverse(arr);

                var realSplindleStatus = string.Join("", arr);
                var slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveID"].ToInt();
                var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);
                var rawStatus = string.Join("", Convert.ToString(ready[0], 2).PadLeft(InteractingDevice.spindleNum, '0').Reverse().ToList());
                _logger.LogError($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")} 获取的轴状态{realSplindleStatus}  获取的{rawStatus} 选轴 ct时间 发送结束");
                if (!string.Equals(rawStatus, realSplindleStatus))
                {
                    StartSelectSpline();
                }
            }
            else
            {
                StartSelectSpline();
            }
            _logger.LogDebug($"{DateTime.Now.ToLongTimeString()}  选轴 ct时间 发送结束");

            //压板
            if (InteractingDevice.DeviceDescriptor.Extra["PressBoardOnOff"].ToBool() && !StartPressBoard())
            {
                message = "压板 异常";
                InteractingDevice.cnc84Command.SetCncComand($"DSP,压板 异常");
                _logger.LogError($"压板 异常 ");
                return;
            }

            //蘑菇头
            if (InteractingDevice.DeviceDescriptor.Extra["MushroomOnOff"].ToBool() && !StartOpenMushroomController())
            {
                message = "蘑菇头 异常";
                InteractingDevice.cnc84Command.SetCncComand($"DSP,蘑菇头 异常");
                _logger.LogError($"蘑菇头 异常 ");
                return;
            }

            //开始打板
            StartDrilBoard();

            if (!ValidateStartCommandEnd())
            {
                bool flag = CheckStartStatusAndRestart();
                if (!flag)
                {
                    _logger.LogDebug($"打板未能正常启动 ");
                    message = "没有正常启动";
                    return;
                }
            }
            status = 1;
            message = "完成";
        }
        catch (Exception ee)
        {
            message = $"{ee.Message}";
        }
        finally
        {
            DrillStartDrillingReportPayload drillStartDrillingReportPayload = new DrillStartDrillingReportPayload()
            {
                body = new DrillStartDrillingReportBody()
                {
                    sn = InteractingDevice.DeviceId,
                    taskCode = drillStartDrillingPayload.body.taskCode,
                    status = status,
                    msg = message,
                }
            };
            drillStartDrillingPayload.ModifyHeader();
            _logger.LogInformation($"03-208  开始打板上报: DrillStartDrillingCommand StartDrillingAndReport  上报的 drillStartDrillingReportPayload {JsonSerializer.Serialize(drillStartDrillingReportPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })} ");
            if (status != 1)
            {
                await RemoveTaskCodeRecord(drillStartDrillingPayload.body.taskCode);
            }
            await _mqttClient.PublishStringAsyncEnhance(IotTopic.START_DRILLING_REPORT_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillStartDrillingReportPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);

        }
    }
    public bool RetractTool()
    {
        var toolNum = GetToolNum();
        if (!"T0".Equals(toolNum))
        {
            Thread.Sleep(1000);
            InteractingDevice.cnc84Command.SetCncComand("T");
        }

        if (!"T0".Equals(toolNum) && !ValidateRetractToolEnd())
        {
            return false;
        }
        return true;
    }
    public void StartSelectSpline()
    {
        try
        {
            _logger.LogDebug("开始选择轴");
            var selectSplineMFunction = InteractingDevice.DeviceDescriptor.Extra["SelectSplineMFunction"].ToInt();
            var slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveID"].ToInt();
            var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);
            InteractingDevice.cnc84Command.SetCncComand("SZSA");
            _logger.LogDebug("发送SZSA");
            Thread.Sleep(2000);
            var rawStatus = Convert.ToString(ready[0], 2).PadLeft(InteractingDevice.spindleNum, '0').Reverse().ToList();
            _logger.LogDebug($"开始选择轴的时候 钻机上面板子 {string.Join("", rawStatus)}");
            if (rawStatus.Any(s => s == '0'))
            {
                int index = 0; int command = selectSplineMFunction;
                rawStatus.ForEach(s =>
                {
                    if (s == '0')
                    {
                        _logger.LogDebug($"开始选择轴的时候 发送指令 M{command + index}");
                        InteractingDevice.cnc84Command.SetCncComand($"M{command + index}");
                        Thread.Sleep(3000);
                    }
                    index++;
                });
            }
        }
        catch (Exception ee)
        {
            _logger.LogDebug($"钻机选轴异常：{ee.Message}");
        }
    }
    #region startDrilling
    private bool CheckStartStatusAndRestart()
    {
        try
        {
            bool startFlag = false;

            for (int i = 0; i < 3; i++)
            {
                RestartCnc();
                if (ValidateStartCommandEnd())
                {
                    startFlag = true;
                    break;
                }
            }
            return startFlag;
        }
        catch (Exception ee)
        {
            _logger.LogDebug($"CheckStartStatusAndRestart 异常{ee.Message}");
            return false;
        }
    }

    private void RestartCnc()
    {
        InteractingDevice.cnc84Command.Start();
        _logger.LogDebug($" RestartCnc 发送开始指令 ");
        Thread.Sleep(_xianJinIotOptions.StartedSpan);
    }

    private void StartDrilBoard()
    {
        _logger.LogDebug("开始切换界面");
        Thread.Sleep(2000);
        StartChangeF8();
        Thread.Sleep(2000);
        _logger.LogDebug($" 发送开始指令 ");
        InteractingDevice.cnc84Command.Start();
        Thread.Sleep(_xianJinIotOptions.StartedSpan);
    }

    private void StartChangeF8()
    {
        InteractingDevice.cnc84Command.SetChangePage("WORK_WORK");
    }

    private bool CheckStartStatus()
    {
        try
        {
            //获取打板开始指令
            string endFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["DrilBoardEndFlag"].ToInt(), true);
            _logger.LogDebug($"启动过程中检测： 钻孔结束信号 {DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool()}：{endFlag}");
            string result = "0";
            if (!DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool())
            {
                result = "1";
            }
            var drillHoleStart = result.Equals(endFlag);
            _logger.LogDebug($"启动过程中检测： 钻孔开始信号：{drillHoleStart}");
            return drillHoleStart;
        }
        catch (Exception ee)
        {
            _logger.LogDebug($"启动过程中检测： 钻孔开始信号：{ee.Message}");
            return false;
        }
    }

    private bool ValidateStartCommandEndBySeqFlag()
    {
        try
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                string endFlag = InteractingDevice.cnc84Command.GetSeqFlag(_xianJinIotOptions.HasStartedOnCnc);
                if ("1".Equals(endFlag))
                {
                    stopwatch.Stop();
                    _logger.LogDebug($"发送Start指令 耗时{stopwatch.ElapsedMilliseconds} 毫秒");
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateStartTimeout"].ToLong());
            _logger.LogDebug($"发送Start指令 超过耗时{stopwatch.ElapsedMilliseconds} 毫秒");
        }
        catch (Exception ee)
        {
            Console.WriteLine(ee.Message);
        }
        return false;
    }

    private bool ValidateStartCommandEnd()
    {
        try
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                var seqFlag = CheckStartStatus();
                if (seqFlag)
                {
                    stopwatch.Stop();
                    _logger.LogDebug($"发送Start指令 耗时{stopwatch.ElapsedMilliseconds} 毫秒");
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateStartTimeout"].ToLong());
            _logger.LogDebug($"发送Start指令 超过耗时{stopwatch.ElapsedMilliseconds} 毫秒");
        }
        catch (Exception ee)
        {
            Console.WriteLine(ee.Message);
        }
        return false;
    }
    #endregion


    #region pressBoard
    public bool StartPressBoard()
    {
        InteractingDevice.cnc84Command.SetCncComand($"DSP,压板中");
        var toolNum = GetToolNum();
        if (!"T0".Equals(toolNum))
        {
            Thread.Sleep(1000);
            InteractingDevice.cnc84Command.SetCncComand("T");
        }

        if (!"T0".Equals(toolNum) && !ValidateRetractToolEnd())
        {
            return false;
        }

        Thread.Sleep(5000);
        PressBoard();
        if (!ValidatePressBoardEnd())
        {
            return false;
        }

        return true;
    }

    private string GetToolNum()
    {
        return InteractingDevice.cnc84Command.GetToolParameter().ToolNumber;
    }

    private bool ValidateRetractToolEnd()
    {
        Thread.Sleep(1000);
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var toolNum = string.Empty;
        var commStatus = string.Empty;

        do
        {
            Thread.Sleep(50);
            toolNum = GetToolNum();
            if ("T0".Equals(toolNum) && commStatus != "BUSY:T" && string.IsNullOrEmpty(commStatus))
            {
                stopwatch.Stop();
                return true;
            }
        } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["RetractToolEndTimeout"].ToLong());

        stopwatch.Stop();
        return false;
    }

    private void PressBoard()
    {
        _logger.LogDebug($"发送 压板指令 m102");
        InteractingDevice.cnc84Command.SetCncComand("M102");
    }
    private bool ValidatePressBoardEnd()
    {
        try
        {
            Thread.Sleep(1000);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            do
            {
                var seqFlag = InteractingDevice.cnc84Command.GetSeqFlag(DeviceDescriptor.Extra["PressBoardEndFlagOnCNC84"].ToInt());
                if ("1".Equals(seqFlag, StringComparison.CurrentCultureIgnoreCase))
                {
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidatePressBoardEndTimeout"].ToLong());
        }
        catch (Exception ee)
        {
            Console.WriteLine(ee.Message);
        }

        return false;
    }
    #endregion
    #region mushroom
    public bool StartOpenMushroomController()
    {
        InteractingDevice.cnc84Command.SetCncComand($"DSP,蘑菇头动作中");
        if (InteractingDevice.middleMushroomExist)
        {
            // 获取用户标记
            //string result = InteractingDevice.cnc84Command.GetUserFlag(DeviceDescriptor.Extra["SmallBoardUserFlag"].ToInt()).ToStr();

            string result = "0";
            string p1Str = InteractingDevice.cnc84Command.GetRuntimeValue($"PGMArea(1)");
            string p3Str = InteractingDevice.cnc84Command.GetRuntimeValue($"PGMArea(3)");

            if (!(double.TryParse(p1Str, out double p1) && double.TryParse(p3Str, out double p3)))
            {
                return false;
            }
            var ySize = (p3 - p1) / 1000;

            _logger.LogDebug($"蘑菇头 程序的板长是{ySize} p3:{p3} p1:{p1}  配置小板的最大长度是{DeviceDescriptor.Extra["SmallBoardMaxLength"].ToDouble()} ");//
            if (ySize <= DeviceDescriptor.Extra["SmallBoardMaxLength"].ToDouble())
            {
                result = "1";
            }

            if ("1".Equals(result, StringComparison.CurrentCultureIgnoreCase))
            {
                _logger.LogDebug("有中间蘑菇头  当前板子是小板 中间蘑菇需要打开！");
                if (!StartOpenMiddleMushroom(true)) return false;
            }
            else
            {
                _logger.LogDebug("有中间蘑菇头  当前板子是大板 中间蘑菇不需要打开！");
                if (!StartOpenMiddleMushroom(false)) return false;
            }
        }
        else
        {
            if (!StartOpenMushroom()) return false;
        }

        return true;
    }

    public bool StartOpenMiddleMushroom(bool isOpenMiddle)
    {
        //打开中间 发m106 判断2个标识
        if (isOpenMiddle)
        {
            //判断当前蘑菇头位置
            if (!MushroomFrontBackCloseLocalTion() || !MushroomMiddleCloseLocalTion())
            {
                _logger.LogDebug("当前 前后蘑菇头 或者中间蘑菇头 已经打开，无法进行操作！");
                return false;
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //发送打开蘑菇头指令
            ControlMiddleMushroom("Open");
            Task.Delay(100).GetAwaiter().GetResult();
            //验证蘑菇头是否打开结束
            do
            {
                if (!MushroomFrontBackCloseLocalTion() && !MushroomMiddleCloseLocalTion())
                {
                    _logger.LogDebug("前后中蘑菇头结束，请等待...");
                    stopwatch.Stop();
                    _logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】验证前后中蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateMushroomEndTimeOut"].ToLong());
        }
        else
        {
            //判断当前蘑菇头位置
            if (!MushroomFrontBackCloseLocalTion())
            {
                _logger.LogDebug("当前 前后蘑菇头已经打开，无法进行操作！");
                return false;
            }
            _logger.LogDebug($"大板的时候 是否存在中间蘑菇头 {InteractingDevice.middleMushroomExist}");
            if (InteractingDevice.middleMushroomExist && !MushroomMiddleCloseLocalTion())
            {
                _logger.LogDebug("大板的时候 检查  中间蘑菇头已经打开，无法进行操作！");
                return false;
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //发送打开蘑菇头指令
            ControlFrontBackMushroom("Open");
            Task.Delay(100).GetAwaiter().GetResult();
            //验证蘑菇头是否打开结束
            do
            {
                if (!MushroomFrontBackCloseLocalTion())
                {
                    _logger.LogDebug("前后蘑菇头结束，请等待...");
                    stopwatch.Stop();
                    _logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】验证前后蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateMushroomEndTimeOut"].ToLong());
        }
        return false;
    }

    private bool MushroomFrontBackCloseLocalTion()
    {
        string MushroomCloseLocalTionFlag = $"%S(HSYS55_Output,{DeviceDescriptor.Extra["FrontMushroomCloseLocalTionFlag"]})";
        string result = InteractingDevice.cnc84Command.GetRuntimeString(MushroomCloseLocalTionFlag);
        if ("1:1".Equals(result, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }
        return false;
    }

    private bool MushroomMiddleCloseLocalTion()
    {
        string middleMushroomCloseLocalTionFlag = $"%S(HSYS55_Output,{DeviceDescriptor.Extra["MiddleMushroomCloseLocalTionFlag"]})";
        string result = InteractingDevice.cnc84Command.GetRuntimeString(middleMushroomCloseLocalTionFlag);
        if ("1:1".Equals(result, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        return false;
    }

    public void ControlMiddleMushroom(string str)
    {
        InteractingDevice.cnc84Command.SetCncComand("M106");
        _logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送 开启中间 {str}蘑菇头指令");
    }

    public void ControlFrontBackMushroom(string str)
    {
        InteractingDevice.cnc84Command.SetCncComand("M27");
        _logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送{str}蘑菇头指令");
    }

    private bool StartOpenMushroom()
    {
        if (!MushroomCloseLocalTion())
        {
            return false;
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        ControlMushroom();

        do
        {
            if (!MushroomCloseLocalTion())
            {
                stopwatch.Stop();
                return true;
            }
        } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateMushroomEndTimeOut"].ToLong());

        return false;
    }

    private bool MushroomCloseLocalTion()
    {
        if (DeviceDescriptor.Extra["OutIntFlagFromIntOnOff"].ToBool())
        {
            var result = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["FrontMushroomCloseLocalTionFlag"].ToInt(), true);

            if ("1".Equals(result, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
        }
        else
        {
            var result = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["FrontMushroomCloseLocalTionFlag"].ToInt());

            if ("1:1".Equals(result, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
        }


        return false;
    }

    private void ControlMushroom()
    {
        InteractingDevice.cnc84Command.SetCncComand("M27");
    }
    #endregion
}
