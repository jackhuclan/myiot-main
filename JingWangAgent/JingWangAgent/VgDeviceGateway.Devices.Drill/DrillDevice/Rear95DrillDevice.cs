// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/********************************************************************************************
 *                         Suzhou Vega Technology Co., Ltd                                  *
 ********************************************************************************************
 * Copyright    : Copyright(c) Suzhou Vega Technology Co., Ltd                              *
 *                All rights reserved.                                                      *
 *                                                                                          *
 * DateTime       Author          Comment                                                   *
 * 2024.05.01    Li Haiyan        New                                                       *
 * 2024.06.01    Zhu Shipeng      Re-implementation of CNC84                                *
 ********************************************************************************************/

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SqlSugar;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Drill.Other.CodeReader;
using VgDeviceGateway.Devices.Drill.Other.Jobs;

namespace VgDeviceGateway.Devices.Drill.DrillDevice
{
    public class Rear95DrillDevice : DeviceShare<DefaultDrill>, IDrillDevice
    {
        private readonly ILogger<Rear95DrillDevice> logger;
        public readonly byte slaveID;
        public readonly bool pinCheckOnOff;
        public readonly bool clearOnOff;
        public readonly bool statisticsOnOff;
        public readonly bool mushroomOnOff;
        public readonly bool codeReaderTriggerIsM;
        public readonly bool codeReaderLocalConfirmOnOff;
        public readonly bool preControlMushroomOnOff;
        public readonly bool loadFileFromCncUIOnOff;
        public readonly bool boardLengthWriteToPlcOnOff;
        public readonly bool abCheckOnOff;

        public Rear95DrillDevice(ILogger<Rear95DrillDevice> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            this.slaveID = (byte)device.DeviceDescriptor.Extra["SlaveID"].ToInt();
            this.pinCheckOnOff = device.DeviceDescriptor.Extra["PinCheckOnOff"].ToBool();
            this.clearOnOff = device.DeviceDescriptor.Extra["ClearOnOff"].ToBool();
            this.statisticsOnOff = device.DeviceDescriptor.Extra["StatisticsOnOff"].ToBool();
            this.mushroomOnOff = device.DeviceDescriptor.Extra["MushroomOnOff"].ToBool();
            this.loadFileFromCncUIOnOff = device.DeviceDescriptor.Extra["LoadFileFromCncUIOnOff"].ToBool();
            device.mushroomValue = mushroomOnOff;
            codeReaderTriggerIsM = device.DeviceDescriptor.Extra["CodeReaderTriggerIsM"].ToBool();
            codeReaderLocalConfirmOnOff = device.DeviceDescriptor.Extra["CodeReaderLocalConfirmOnOff"].ToBool();
            preControlMushroomOnOff = device.DeviceDescriptor.Extra["PreControlMushroomOnOff"].ToBool();
            boardLengthWriteToPlcOnOff = device.DeviceDescriptor.Extra["BoardLengthWriteToPlcOnOff"].ToBool();
            abCheckOnOff = device.DeviceDescriptor.Extra["ABCheckOnOff"].ToBool();
            if (device.codeReaderOnOff)
            {
                CodeReaderBaseSetting.CodeReaderTriggerBeforeAction = CodeReaderTriggerBefore;
                CodeReaderBaseSetting.CodeReaderReceiveAction += CodeReaderReceive;
                CodeReaderBaseSetting.CodeReaderReceiveAction += CodeInsertToDB;
            }
        }

        private void CodeInsertToDB(int spindleNum, string codeReaderContent)
        {
            var IsPanelSNToDB = DeviceDescriptor.Extra["IsPanelSNToDB"].ToBool();
            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  读码器获取的是{spindleNum + 1}轴  内容{codeReaderContent},是否写入数据库：{IsPanelSNToDB.ToString()}");
            if (IsPanelSNToDB)
            {
                string info = $"spindleNum:{spindleNum + 1},Lot:{codeReaderContent},SN:{codeReaderContent},deviceId:{InteractingDevice.DeviceId}";
                logger.LogWarning($"CodeInsertToDB：{DateTime.Now.ToShortTimeString()}  给数据库写入的数据为: {info} ,准备写入DB");
                if (InteractingDevice.ApplicationServices.TryGetService<IDrillLoadPanelSNToDB>(out IDrillLoadPanelSNToDB fileLoadResult))
                {
                    logger.LogWarning($"CodeInsertToDB：写入DB");
                    fileLoadResult?.LoadPanelSNToDB(info);
                }
            }
        }

        private void CodeReaderReceive(int spindleNum, string codeReaderContent)
        {
            //获取数据
            //更新板材信息
            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  读码器获取的是{spindleNum}轴  内容{codeReaderContent}");

            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  读码器 未修改【 PayloadPanels 】 {JsonSerializer.Serialize(PayloadPanels)} ");
            PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Factory.StartNew((obj) =>
           {
               InteractingDevice.PayloadPanels[(int)obj].Barcode = codeReaderContent;
           }, spindleNum)).GetAwaiter().GetResult();

            PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.DeviceId).GetAwaiter().GetResult();
            if (codeReaderLocalConfirmOnOff) CodeReaderSingleSplindleCheck(spindleNum, 1);
            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  读码器 已修改【 PayloadPanels 】 {JsonSerializer.Serialize(PayloadPanels)} ");
        }

        private void CodeReaderTriggerBefore(int splindleNum)
        {
            try
            {
                //清空该轴的读码器验证信息
                //
                var checkOld = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferCodeReaderCheckOnPlc"].ToUshort(), 1)[0];
                logger.LogDebug($"获取的读码器原始数据{checkOld} {Convert.ToString(checkOld, 2).PadLeft(DeviceDescriptor.SpindleNum.ToInt(), '0')}");

                var checkNew = checkOld & ~(1 << splindleNum);
                logger.LogDebug($"修改的读码器数据{checkNew} {Convert.ToString(checkNew, 2).PadLeft(DeviceDescriptor.SpindleNum.ToInt(), '0')}");

                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["BufferCodeReaderCheckOnPlc"].ToUshort(), (ushort)checkNew);
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra[$"Spline{splindleNum + 1}CodeReaderCodeNum"].ToUshort(), 0);
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra[$"Spline{splindleNum + 1}CodeReaderCodeStartPosition"].ToUshort(), new ushort[10]);
            }
            catch (Exception ee)
            {
                logger.LogError($"读码器触发器前动作异常 {ee.Message}");
            }
        }

        public bool ConnectToCncAndOtherDevice()
        {
            if (!InteractingDevice.plcConnentFlag)
            {
                InteractingDevice.plcConnentFlag = PlcConnect();
            }
            if (InteractingDevice.cnc84Command == null || !InteractingDevice.cnc84Command.CNCCommandStatus())
            {
                CNC84Connect();
            }

            var cnc84Flag = InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus();

            var comFlag = true;
            if (InteractingDevice.scanGunOnOff)
            {
                comFlag = ComConnect();
            }

            var codeFlag = true;
            if (InteractingDevice.codeReaderOnOff)
            {
                if (!CodeReaderBaseSetting.scanFlag)
                {
                    codeFlag = ScanConnect();
                }

            }

            return cnc84Flag && InteractingDevice.plcConnentFlag;
        }

        public void CompleteScheduleLocal(ScheduledTaskStatus ScheduledStatus)
        {
            logger.LogDebug($"CompleteScheduleLocal 方法被调用了 调度状态 {ScheduledStatus}：重置{InteractingDevice.allowAllAgv}");
            InteractingDevice.allowAllAgv = true;
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 1);
            InteractingDevice.TransactionId = string.Empty;
        }

        public void CanceledScheduleLocal(ScheduledTaskStatus ScheduledStatus)
        {
            logger.LogDebug($"CanceledScheduleLocal 方法被调用了 调度状态 {ScheduledStatus}：重置{InteractingDevice.allowAllAgv}");
            InteractingDevice.allowAllAgv = true;
            InteractingDevice.TransactionId = string.Empty;
        }

        public void FailScheduleLocal()
        {
            logger.LogDebug($"FailScheduleLocal 方法被调用了：重置{InteractingDevice.allowAllAgv}");
            InteractingDevice.TransactionId = string.Empty;
            InteractingDevice.allowAllAgv = true;
            try
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["WarningInfoWriteOnPlc"].ToUshort(), 110);
                logger.LogDebug($"钻机给buffer报警编号：110");
            }
            catch (Exception)
            {
                logger.LogDebug($"中控给钻机下发异常");
            }
        }

        public Task InitializeLocal()
        {
            Task.Factory.StartNew(WatchDogThread, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(FastCollectDataThread);
            Task.Factory.StartNew(FastCancelLockThread);
            Task.Factory.StartNew(HeartBeatThread, TaskCreationOptions.LongRunning);
            // Task.Factory.StartNew(PlcErrorCheckThread, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(TimeStatisticThread, TaskCreationOptions.LongRunning);
            if (pinCheckOnOff) Task.Factory.StartNew(FrontPinCheckThread, TaskCreationOptions.LongRunning);
            if (clearOnOff) ObjectFactory.CreateObject<ClearJobSchedule>().StartClearJob(InteractingDevice, InteractingDevice.cancellationTokenSource.Token);
            if (statisticsOnOff) ObjectFactory.CreateObject<StatisticsJobSchedule>().StartStatisticsJob(InteractingDevice, InteractingDevice.cancellationTokenSource.Token);
            if (InteractingDevice.codeReaderOnOff) Task.Factory.StartNew(CodeReaderThread, TaskCreationOptions.LongRunning);
            if (abCheckOnOff) Task.Factory.StartNew(ABCheckThread, TaskCreationOptions.LongRunning);
            return Task.CompletedTask;
        }

        private async Task WatchDogThread()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表
            foreach (Process process in processes)
            {
                //如果实例已经存在则忽略当前进程
                if (process.Id != current.Id)
                {
                    //保证要打开的进程同已经存在的进程来自同一文件路径
                    if (process.MainModule.FileName.Equals(current.MainModule.FileName))
                    {
                        //已经存在的进程
                        Environment.Exit(Environment.ExitCode);
                        Process.GetCurrentProcess().Kill();
                        return;
                    }
                    else
                    {
                        process.Kill();
                        process.WaitForExit(3000);
                    }
                }
            }
            logger.LogDebug($"WatchDogThread:开始了");
            await Task.Delay(1000);
            string filePath = DeviceDescriptor.Extra["WatchDogAppPath"].ToStr();
            string processName = Path.GetFileNameWithoutExtension(filePath);
            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        logger.LogDebug($"WatchDogAppPath {filePath} 文件不存在");
                        await Task.Delay(60 * 1000);
                        continue;
                    }
                    Process[] myproc = Process.GetProcessesByName(processName);
                    if (myproc.Length == 0)
                    {
                        logger.LogDebug("检测到看护程序已退出，开始重新激活程序,程序路径:{0}", filePath);
                        ProcessStartInfo info = new ProcessStartInfo
                        {
                            WorkingDirectory = Path.GetDirectoryName(filePath),
                            FileName = filePath,
                            UseShellExecute = true
                        };
                        Process.Start(info);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError("WatchDogThread 线程异常->" + e.Message);
                    await Task.Delay(6 * 1000);
                }
                await Task.Delay(10000);
            }
            logger.LogDebug($"WatchDogThread 线程结束");
        }

        private async Task HeartBeatThread()
        {
            logger.LogDebug($"HeartbeatThread:开始了");
            await Task.Delay(10000);
            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (InteractingDevice.modbusIpMaster != null && InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus())
                    {
                        int data = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["HeartBeatOnPlc"].ToUshort(), 1)[0];
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["HeartBeatOnPlc"].ToUshort(), (ushort)(data == 1 ? 0 : 1));
                        await Task.Delay(DeviceDescriptor.Extra["HeartBeatInterval"].ToInt());
                    }
                    else
                    {
                        logger.LogDebug($"HeartBeatThread Plc 连接未建立");
                        await Task.Delay(6 * 1000);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError("HeartbeatThread 线程异常->D500->" + e.Message);
                    await Task.Delay(6 * 1000);
                }
            }
            logger.LogDebug($"HeartBeatThread 线程结束");
        }

        /// <summary>
        /// 钻机异常写入寄存器地址->109  Plc异常写入寄存器地址->110
        /// </summary>
        /// <returns></returns>
        private async Task PlcErrorCheckThread()
        {
            logger.LogDebug($"PlcErrorCheckThread:开始了");
            await Task.Delay(10000);
            int oldData = 0;
            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(1000);
                try
                {
                    if (InteractingDevice.modbusIpMaster != null && InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus())
                    {
                        int data = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 110, 1)[0];
                        if (oldData == data)
                        {
                            continue;
                        }
                        oldData = data;
                        if (data == 0)
                        {
                            PlcError[-1] = "无故障";
                            InteractingDevice.cnc84Command.SetCncComand($"DSP,");
                            continue;
                        }

                        string msg = PlcError[data];
                        if (msg != PlcError[-1])
                        {
                            PlcError[-1] = msg;
                            logger.LogWarning("PlcErrorCheckThread 内容" + msg);
                            InteractingDevice.cnc84Command.SetCncComand($"DSP,Please_Check_Buffer_Error!");
                            //InteractingDevice.cnc84Command.SetCncComand($"SDSP,{msg}");
                            //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 110, 0);//按【故障复位】清除
                        }
                    }
                    else
                    {
                        logger.LogDebug($"PlcErrorCheckThread Plc 连接未建立");
                        await Task.Delay(6 * 1000);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError("PlcErrorCheckThread->" + e.Message);
                    await Task.Delay(6 * 1000);
                }
            }
            logger.LogDebug($"FrontPinCheckThread 线程结束");
        }

        public static ConcurrentDictionary<int, string> PlcError = new ConcurrentDictionary<int, string>()
        {
            [-1] = "无故障",
            [0] = "无故障",
            [1] = "轴1前销钉检测异常",
            [2] = "轴2前销钉检测异常",
            [3] = "轴3前销钉检测异常",
            [4] = "轴4前销钉检测异常",
            [5] = "轴5前销钉检测异常",
            [6] = "轴6前销钉检测异常",
            [7] = "轴1后销钉检测异常",
            [8] = "轴2后销钉检测异常",
            [9] = "轴3后销钉检测异常",
            [10] = "轴4后销钉检测异常",
            [11] = "轴5后销钉检测异常",
            [12] = "轴6后销钉检测异常",
            [13] = "工位1卡料",
            [14] = "工位2卡料",
            [15] = "工位3卡料",
            [16] = "工位4卡料",
            [17] = "工位5卡料",
            [18] = "工位6卡料",
            [19] = "工位1皮带气缸异常",
            [20] = "工位2皮带气缸异常",
            [21] = "工位3皮带气缸异常",
            [22] = "工位4皮带气缸异常",
            [23] = "工位5皮带气缸异常",
            [24] = "工位6皮带气缸异常",
            [25] = "机构移动至位置3故障",
            [26] = "机构移动至位置2故障",
            [27] = "机构移动至位置1故障",
            [28] = "下层成品1 Load异常",
            [29] = "下层成品2 Load异常",
            [30] = "下层成品3 Load异常",
            [31] = "下层成品4 Load异常",
            [32] = "下层成品5 Load异常",
            [33] = "下层成品6 Unload异常",
            [34] = "上层原料1 Unload异常",
            [35] = "上层原料2 Unload异常",
            [36] = "上层原料3 Unload异常",
            [37] = "上层原料4 Unload异常",
            [38] = "上层原料5 Unload异常",
            [39] = "上层原料6 Unload异常",
            [40] = "钻机急停",
            [41] = "B/F 急停",
            [42] = "B/F 光栅",
            [43] = "轴1 读码失败",
            [44] = "轴2 读码失败",
            [45] = "轴3 读码失败",
            [46] = "轴4 读码失败",
            [47] = "轴5 读码失败",
            [48] = "轴6 读码失败",
            [49] = "伺服连接失败",
            [50] = "运程模块连接失败",
            [51] = "轴1 板材超出最大尺寸",
            [52] = "轴2 板材超出最大尺寸",
            [53] = "轴3 板材超出最大尺寸",
            [54] = "轴4 板材超出最大尺寸",
            [55] = "轴5 板材超出最大尺寸",
            [56] = "轴6 板材超出最大尺寸",
            [57] = "伺服未回原点",
            [58] = "伺服未使能",
            [59] = "负限位报警",
            [60] = "正限位报警",
            [61] = "程序FB执行错误",
            [62] = "程序轴报错",
            [63] = "伺服有故障",
            [64] = "扫码枪未配置",
            [65] = "扫码枪断开",
            [66] = "cnc84未配置",
            [67] = "cnc84断开",
        };

        /// <summary>
        /// 2024-07-13
        /// zhushipeng
        /// 前销钉检测
        /// M线圈
        /// D寄存器
        /// B0=12288
        /// W2.4=B24=B0+24 W2.5=B25=B0+25
        /// W0.0-12288 W0.1-12289 W0.2-12290 W0.3-12291 W0.4-12292 W0.5-12293
        /// </summary>
        /// <returns></returns>
        private async Task FrontPinCheckThread()
        {
            logger.LogDebug($"FrontPinCheckThread:开始了");
            await Task.Delay(10000);
            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (InteractingDevice.modbusIpMaster != null && InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus())
                    {
                        var frontPin = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1)[0];
                        frontPin = Change1To0(frontPin);

                        var rearPin = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["RearPinCheckOnPlc"].ToUshort(), 1)[0];

                        rearPin = Change1To0(rearPin);

                        var cvCheck = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["CVCheckOnPlc"].ToUshort(), 1)[0];

                        Int64 result = (((cvCheck << 6) | rearPin) << 6 | frontPin);
                        var cnc95Result = InteractingDevice.cnc84Command.ReadCncNode<Int64>("ns=4;s=UI/normalized/custom/Customer.pinsensor");
                        logger.LogDebug($"FrontPinCheckThread  计算的结果: {result} -- {Convert.ToString(result, 2)}  获取的旗标 {cnc95Result}  ");
                        //var obj = InteractingDevice.cnc84Command.SendCncComand("VALU,CUSTOMER24");
                        //logger.LogDebug($"FrontPinCheckThread  VALU,CUSTOMER24: {string.Join(",",obj)}  ");
                        if (cnc95Result != result)
                            InteractingDevice.cnc84Command.WriteCncNode<Int64>("ns=4;s=UI/normalized/custom/Customer.pinsensor", result);

                        await Task.Delay(InteractingDevice.DeviceDescriptor.Extra["PinCheckInterval"].ToInt());
                    }
                    else
                    {
                        logger.LogDebug($"FrontPinCheckThread Plc 连接未建立");
                        await Task.Delay(6 * 1000);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError("FrontPinCheckThread 线程异常->W0.0工位前销钉检测->" + e.Message);
                    await Task.Delay(6 * 1000);
                }
            }
            logger.LogDebug($"FrontPinCheckThread 线程结束");
        }
        long writeToCNCValue = 0;
        private async Task ABCheckThread()
        {
            logger.LogDebug($"ABCheckThread:开始了");
            await Task.Delay(10000);
            var originDateTime = DateTime.Parse("0001/01/01 08:00:00");
            DateTime? startDateTime = null;
            DateTime? endDateTime = null;
            bool[]? oldworkStations = null;
            int boardStatus = 0;

            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(InteractingDevice.DeviceDescriptor.Extra["ABCheckInterval"].ToInt());
                try
                {

                    if (InteractingDevice.modbusIpMaster != null && InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus())
                    {
                        // 1.判断程序是否开始ns=4;s=UI/origin/AutoData/AutoData.List/AutoData.RunStartTime

                        var startTime = InteractingDevice.cnc84Command.ReadCncNode<DateTime>("ns=4;s=UI/origin/AutoData/AutoData.List/AutoData.RunStartTime").AddHours(8);
                        var endTime = InteractingDevice.cnc84Command.ReadCncNode<DateTime>("ns=4;s=UI/origin/AutoData/AutoData.List/AutoData.RunEndTime").AddHours(8);
                        var boardStatusTemp = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);
                        logger.LogDebug($"系统中获取的开始时间 {startTime}  和结束时间 {endTime} 钻机上传感器的内容： {boardStatusTemp[0]}");
                        var errors = InteractingDevice.cnc84Command.ReadCncNode<string[]>("ns=4;s=UI/origin/DDETable/CncErrorTable/CncError");
                        if (startTime == originDateTime || ((startTime != originDateTime) && endTime != originDateTime && endTime.Subtract(startTime).Ticks >= 0))
                        {
                            startDateTime = null;
                            endDateTime = null;
                            oldworkStations = null;
                            logger.LogDebug($"程序被清除了 或者程序已经结束   ");
                            writeToCNCValue = 0;
                            continue;
                        }

                        if (startTime != startDateTime)
                        {
                            startDateTime = startTime;
                            boardStatus = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1)[0];
                            logger.LogDebug($"ABCheckThread 程序开始 {startDateTime}  开始的时候钻机板子状态 {boardStatus}   ");
                            endDateTime = null;
                            oldworkStations = null;
                            writeToCNCValue = 0;

                        }
                        logger.LogDebug($"ABCheckThread 开始时间 {startDateTime}  结束时间 {endDateTime}   ");
                        if (endDateTime != null)
                        {
                            continue;
                        }
                        var end = errors.Where(s => !string.IsNullOrEmpty(s) && s.Contains("AB check finish"))
                                      .Where(line => DateTime.ParseExact(line.Substring(0, 19), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture) >= startDateTime)
                                       .OrderByDescending(line => DateTime.ParseExact(
                                        line.Substring(0, 19), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture))
                                       .FirstOrDefault();
                        if (end != null)
                        {
                            endDateTime = DateTime.ParseExact(end.Substring(0, 19), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            logger.LogDebug($"ABCheckThread 结束时间 {endDateTime}   ");
                            return;
                        }

                        //获取轴状态

                        var workStations = InteractingDevice.cnc84Command.ReadCncNode<bool[]>("ns=4;s=UI/origin/WorkGroupSettings/WorkGroupInterface/WorkStations/WORKSTATION.SELECTION");
                        logger.LogDebug($"ABCheckThread 轴状态 {string.Join(",", workStations)}  钻机板子状态 {boardStatus} ");
                        var splindleNumberDataValue = InteractingDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/Parameter/General/AxisConfiguration/WS.Number");

                        if (oldworkStations == null)
                        {
                            oldworkStations = workStations;
                            //首次获取数据
                            //和前销钉比较判断要不要写
                            ReadAndWriteABcheck(boardStatus, workStations, splindleNumberDataValue);
                        }
                        if (workStations.SequenceEqual(oldworkStations))
                        {
                            //比对现在和iiot是否一致 不一致也要写
                            var abcheckResult = InteractingDevice.cnc84Command.ReadCncNode<Int64>("ns=4;s=UI/normalized/custom/Customer.ABCheck");
                            logger.LogDebug($"ABCheckThread 轴没变化和iiot不一致  {Convert.ToString(abcheckResult, 2)}  获取的旗标 {abcheckResult}  ");
                            if (writeToCNCValue != abcheckResult && writeToCNCValue != 0)
                            {
                                logger.LogDebug($"ABCheckThread 轴没变化和iiot不一致  新写入的 {Convert.ToString(writeToCNCValue, 2)}  获取的旗标 {writeToCNCValue}  ");
                                InteractingDevice.cnc84Command.WriteCncNode<Int64>("ns=4;s=UI/normalized/custom/Customer.ABCheck", writeToCNCValue);
                            }
                            continue;
                        }
                        logger.LogDebug($"变化了的轴状态 {string.Join("", workStations)}");
                        oldworkStations = workStations;
                        ReadAndWriteABcheck(boardStatus, workStations, splindleNumberDataValue);

                    }
                    else
                    {
                        logger.LogDebug($"ABCheckThread  连接未建立");
                        await Task.Delay(6 * 1000);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError("ABCheckThread 线程异常->" + e.Message);
                    await Task.Delay(6 * 1000);
                }
            }
            logger.LogDebug($"ABCheckThread 线程结束");
        }

        private void ReadAndWriteABcheck(int boardStatus, bool[] workStations, int splindleNumberDataValue)
        {
            if (workStations.All(s => s == true))
            {
                return;
            }
            var tableStatus = ComputerCurrentTableStatus(workStations, splindleNumberDataValue);
            if (tableStatus == boardStatus)
            {
                return;
            }
            // 判断轴开 台面关闭了
            List<int> closeTableNum = new List<int>();
            for (int i = 0; i < InteractingDevice.spindleNum; i++)
            {
                // 左移 i 位，然后与 1 进行按位与运算
                int bit = (boardStatus >> i) & 1;
                int bitT = (tableStatus >> i) & 1;
                logger.LogDebug($"第 {i} 位: 板子是{bit} 台面是{bitT}");
                if (bit == 1 && bitT == 0)
                {
                    closeTableNum.Add(i);
                }
            }
            var abcheckResult = InteractingDevice.cnc84Command.ReadCncNode<Int64>("ns=4;s=UI/normalized/custom/Customer.ABCheck");
            logger.LogDebug($"ABCheckThread {Convert.ToString(abcheckResult, 2)}  获取的旗标 {abcheckResult}  ");
            //整合
            foreach (int bitIndex in closeTableNum)
            {
                abcheckResult |= 1 << bitIndex;
            }
            var finalResult = abcheckResult;
            writeToCNCValue = finalResult;
            logger.LogDebug($"ABCheckThread 新写入的{Convert.ToString(finalResult, 2)}  获取的旗标 {finalResult}  ");
            InteractingDevice.cnc84Command.WriteCncNode<Int64>("ns=4;s=UI/normalized/custom/Customer.ABCheck", finalResult);
        }

        private int ComputerCurrentTableStatus(bool[] workStations, int splindleNumberDataValue)
        {
            var result = 0;
            var spindleNum = InteractingDevice.spindleNum;
            if (DeviceDescriptor.Extra["IsDuo"].ToBool())
            {
                spindleNum = spindleNum * 2;
                if (splindleNumberDataValue != 0)
                {
                    spindleNum = splindleNumberDataValue;
                }
                for (int i = 0; i < spindleNum; i = i + 2)
                {
                    if (workStations[i] || workStations[i + 1])
                    {
                        result += (1 << i / 2);
                    }


                }
            }
            else
            {
                if (splindleNumberDataValue != 0)
                {
                    spindleNum = splindleNumberDataValue;
                }
                for (int i = 0; i < spindleNum; i++)
                {
                    if (workStations[i])
                    {
                        result += (i << i);
                    }

                }

            }
            return result;
        }

        private async Task CodeReaderThread()
        {
            logger.LogDebug($"CodeReaderThread:开始了");
            await Task.Delay(10000);
            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (InteractingDevice.modbusIpMaster != null)
                    {
                        if (InteractingDevice.codeReaderOnOff)
                        {
                            if (codeReaderTriggerIsM)
                            {
                                var codeReaderTrigger = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["CodeReaderTrigger"].ToUshort(), InteractingDevice.spindleNum.ToUshort());
                                WatchingProperties.SetValues(new Dictionary<string, object?>() { { "Buffer_CodeReaderTrigger", string.Join("", codeReaderTrigger.Select(s => s == true ? 1 : 0)) } });
                            }
                            else
                            {
                                var codeReaderTrigger = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["CodeReaderTriggerD"].ToUshort(), 1);
                                WatchingProperties.SetValues(new Dictionary<string, object?>() { { "Buffer_CodeReaderTrigger", codeReaderTrigger } });
                            }
                        }

                        await Task.Delay(InteractingDevice.DeviceDescriptor.Extra["CodeReaderTriggerInterval"].ToInt());
                    }
                    else
                    {
                        logger.LogDebug($"CodeReaderThread Plc 连接未建立");
                        await Task.Delay(6 * 1000);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError("CodeReaderThread 线程异常-->" + e.Message);
                    await Task.Delay(6 * 1000);
                }
            }
            logger.LogDebug($"CodeReaderThread 线程结束");
        }

        //
        private ushort Change1To0(int data)
        {
            logger.LogDebug($"转化前的数据 :{Convert.ToString(data, 2).PadLeft(6, '0')}");
            string binaryString = new string(Enumerable.Repeat('1', InteractingDevice.spindleNum).ToArray()); // 二进制字符串
            ushort number;
            // 将二进制字符串转换为整数
            number = Convert.ToUInt16(binaryString, 2);
            data = number - data;
            logger.LogDebug($"转化后的数据 :{Convert.ToString(data, 2).PadLeft(6, '0')}");
            return (ushort)data;
        }

        private bool initPlc = true;
        private bool initCnc = true;

        private async Task FastCollectDataThread()
        {
            logger.LogDebug($"FastCollectDataThread:开始了");
            await Task.Delay(2000);
            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                var faskDic = new Dictionary<string, object?>();
                try
                {
                    if (InteractingDevice.modbusIpMaster != null)
                    {
                        InteractingDevice.bufferAllUnloadAndLoadEnd = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 1)[0];
                        var unLoadAndLoadEndFlag = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["UnloadAndLoadEndOnPlc"].ToUshort(), 1);//M21上料结束
                        faskDic.Add("Buffer_UnLoadAndLoadEndFlag", unLoadAndLoadEndFlag[0]);
                        logger.LogDebug($"FastCollectDataThread:M21->Buffer给钻机上料结束信号： {unLoadAndLoadEndFlag[0]}");

                        var bufferOnAgvPosition = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1);
                        faskDic.Add("Buffer_OnAgvPosition", bufferOnAgvPosition[0]);

                        var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferReadyFlag"].ToUshort(), 2);
                        faskDic.Add("Buffer_IsReady", ready[0] == 1);

                        var work = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferStatusFlag"].ToUshort(), 4);
                        faskDic.Add("Buffer_Automatic", work[1] == 1);

                        var agvOnWorkToBuffer = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferAgvUnLoadingAndLoading"].ToUshort(), 1);
                        InteractingDevice.AgvIsWorkOnBuffer = agvOnWorkToBuffer[0] == 1;
                        faskDic.Add("Buffer_AgvOnWorkBuffer", agvOnWorkToBuffer[0] == 1);

                        var warnning = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["WarningInfoWriteOnPlc"].ToUshort(), 2);
                        faskDic.Add("Drill_WarningCode", warnning[0]);
                        faskDic.Add("Buffer_WarningCode", warnning[1]);

                        faskDic.Add("Buffer_Warning", warnning[0] == 110 || warnning[1] != 0);
                        faskDic.Add("Buffer_CallAgvMessage", InteractingDevice.allowAllAgv);
                        faskDic.Add("MqttConnected", InteractingDevice.MqttClientWrapper?.IsConnected == null ? false : MqttClientWrapper?.IsConnected);
                        faskDic.Add("TranscationId", InteractingDevice.TransactionId);
                        faskDic.Add("NewTranscationId", InteractingDevice.NewTranscationId);
                        var material = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 2);
                        faskDic.Add("Buffer_RawMaterialLayerBoardStatus", material[0]);
                        InteractingDevice.material = material;
                        logger.LogDebug($"Buffer上层传感器的内容： {material[0]}");
                        logger.LogDebug($"Buffer下层传感器的内容： {material[1]}");
                        faskDic.Add("Buffer_ClinkerLayerBoardStatus", material[1]);
                        faskDic.Add("Buffer_ClinkerLayerBoardStatusStatistics", material[1]);
                        //在agv对接层 自动 非异常  ready状态
                        if (bufferOnAgvPosition[0] && work[1] == 1 && (warnning[0] != 110 && warnning[1] == 0) && ready[0] == 1 && DeviceDescriptor.AutoMode && InteractingDevice.MqttClientWrapper.IsConnected)
                        {
                            InteractingDevice.isAvailbleForAgv = true;
                        }
                        else
                        {
                            InteractingDevice.isAvailbleForAgv = false;
                        }
                        faskDic.Add("IsAvailbleForAgv", InteractingDevice.isAvailbleForAgv);
                        var boardStatus = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);
                        logger.LogDebug($"钻机上传感器的内容： {boardStatus[0]}");
                        faskDic.Add("Drill_BoardPositionStatus", boardStatus[0]);
                        faskDic.Add("Drill_BoardPositionStatusStatistics", boardStatus[0]);
                        await Task.Delay(200);
                        faskDic.Add("Drill_CollectCleanTime", DefaultDrill.CollectCleanTime.ToString("#0"));

                        var codeReaderTrigger = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["CodeReaderTrigger"].ToUshort(), InteractingDevice.spindleNum.ToUshort());
                        faskDic.Add("Buffer_MoRunning", codeReaderTrigger.Any(s => s == true));

                        var autoCodeReaderTrigger = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort(), 1)[0];
                        faskDic.Add("Buffer_AutoMoRunning", autoCodeReaderTrigger == 1);

                        if (initPlc)
                        {
                            initPlc = false;
                            if (material[0] == 0) WatchingProperties.Property("Buffer_RawChangeNoBoardTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            if (material[1] != 0) WatchingProperties.Property("Buffer_ClinkerChangeExistTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            if (boardStatus[0] == 0) WatchingProperties.Property("Drill_ChangeNoBoardTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            if (work[1] != 1) WatchingProperties.Property("Buffer_ManualStartTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        var receiveBoardFlag = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReceiveBoardFlag"].ToUshort(), 1);
                        faskDic.Add("Buffer_LoadEnd", receiveBoardFlag[0]);
                        logger.LogDebug($"FastCollectDataThread:D411->Buffer收到生料： {receiveBoardFlag[0]}");

                        var loadAndUnload = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferIsLoadAndUnloadOnPlc"].ToUshort(), 1);
                        faskDic.Add("Buffer_LoadAndUnload", loadAndUnload[0]);
                        logger.LogDebug($"FastCollectDataThread:D505->Buffer既上又下： {loadAndUnload[0]}");

                        if (preControlMushroomOnOff)
                        {
                            var triggerPreControlMushroom = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["TriggerPreControlMushroom"].ToUshort(), 1);
                            faskDic.Add("Buffer_PreControlMushroom", triggerPreControlMushroom[0]);
                            logger.LogDebug($"FastCollectDataThread:触发提前动蘑菇头： {triggerPreControlMushroom[0]}");
                        }
                        var fCall = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["FCallPostion"].ToUshort(), 1)[0];
                        logger.LogDebug($"从plc获取打板fcall信号： {fCall}");
                        faskDic.Add("Drill_DrillHoleStart", fCall);
                        faskDic.Add("Drill_DrillHoleEnd", !fCall);
                    }
                    else
                    {
                        InteractingDevice.isAvailbleForAgv = false;
                        faskDic["IsAvailbleForAgv"] = InteractingDevice.isAvailbleForAgv;
                        await Task.Delay(6 * 1000);
                    }
                }
                catch (Exception e)
                {
                    InteractingDevice.isAvailbleForAgv = false;
                    faskDic["IsAvailbleForAgv"] = InteractingDevice.isAvailbleForAgv;
                    logger.LogError("FastCollectDataThread:Plc->" + e.Message);
                }

                try
                {
                    if (InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus() && InteractingDevice.modbusIpMaster != null)
                    {
                        try
                        {
                            logger.LogDebug($"boardLengthWriteToPlcOnOff 读当前程序的板长信息写到plc的开关 ：{boardLengthWriteToPlcOnOff}");
                            if (boardLengthWriteToPlcOnOff)
                            {
                                // 获取板长  写到plc
                                var boardlength = InteractingDevice.cnc84Command.ReadCncNode<double>("ns=4;s=UI/normalized/custom/Customer.boardlength");
                                logger.LogDebug($"boardLengthWriteToPlcOnOff 从cnc95 中获取的 板长是 {boardlength}");
                                boardlength = Math.Floor(boardlength);
                                ushort u = Convert.ToUInt16(boardlength);
                                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["CurrentProgramBoardLengthWriteToPlc"].ToUshort(), u);
                                logger.LogDebug($"boardLengthWriteToPlcOnOff 写到plc寄存器的值是 {u}");
                            }
                        }
                        catch (Exception ee)
                        {
                            logger.LogError($"写当前程序的板长信息写到plc的出现异常 {ee.Message}");

                        }

                        string showText = InteractingDevice.cnc84Command.ReadCncNode<string>("ns=4;s=UI/normalized/new/ShowText");
                        string blockText = InteractingDevice.cnc84Command.ReadCncNode<string>("ns=4;s=UI/origin/RosiInfo/BlockText");

                        faskDic.Add("Drill_ShowText", showText);
                        faskDic.Add("Drill_ShowAllText", $"{blockText} {showText}");

                        faskDic.Add("Drill_LocalTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        //var bufferLock = InteractingDevice.cnc84Command.GetIutput(DeviceDescriptor.Extra["Cnc84BufferLockOnCnc84"].ToInt()) == "1:2";//75 锁机是 1:2
                        var bufferLock = InteractingDevice.modbusIpMaster?.ReadCoils(slaveID, DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1)[0];
                        faskDic.Add("Drill_BufferLockFlag", bufferLock);

                        var parkingStationEndFrontFlag = InteractingDevice.modbusIpMaster!.ReadCoils(slaveID, DeviceDescriptor.Extra["P4FrontPostion"].ToUshort(), 1)[0] ? "1:1" : "1:0";//12313 W2.5
                        await Task.Delay(100);
                        var parkingStationEndBehindFlag = InteractingDevice.modbusIpMaster!.ReadCoils(slaveID, DeviceDescriptor.Extra["P4RearPostion"].ToUshort(), 1)[0] ? "1:1" : "1:0";//12312 W2.4

                        var parkingFlag = parkingStationEndFrontFlag == "1:1" && parkingStationEndBehindFlag == "1:0"; //12312 W2.4前不亮 12313 W2.5后亮

                        if (parkingFlag)
                        {
                            logger.LogInformation($"Fast……Thread:P4泊车位信号M181->1后W2.4不亮->{parkingStationEndBehindFlag} 前W2.5亮->{parkingStationEndFrontFlag} 泊车位信号->{parkingFlag}");
                            InteractingDevice.modbusIpMaster?.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["CanUnloadAndLoadFlagWritePlc"].ToUshort(), 1);//181
                            //logger.LogInformation($"FastCollectDataThread:泊车位信号M181->1");
                        }
                        else
                        {
                            logger.LogDebug($"Fast……Thread:P4泊车位信号M181->0后W2.4不亮->{parkingStationEndBehindFlag} 前W2.5亮->{parkingStationEndFrontFlag} 泊车位信号->{parkingFlag}");
                            InteractingDevice.modbusIpMaster?.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["CanUnloadAndLoadFlagWritePlc"].ToUshort(), 0);
                            //logger.LogDebug($"FastCollectDataThread:泊车位信号M181->0");
                        }
                        // ///打板结束信号
                        /*
                         * public string GetOutput(int num)
                            {
                                if (num == 58)
                                {
                                    Boolean input = opcUaClient.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.FCall");
                                    if (input)
                                    {
                                        return "1:1";
                                    }
                                    else
                                    {
                                        return "1:0";
                                    }
                                }
                            }
                        */
                        await Task.Delay(100);

                        {

                        }

                        {
                            //var drillHoleEndOriginal = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["DrilBoardEndFlag"].ToInt());//FCall
                            //var str = DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool() ? "结束" : "开始";
                            //logger.LogDebug($"FastCollectDataThread:FCall original->打板{str}信号 ：{drillHoleEndOriginal}");
                            //var result = "1:1";
                            //if (!DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool())
                            //{
                            //    result = "1:0";
                            //}
                            //var drillHoleEnd = result.Equals(drillHoleEndOriginal);

                            //faskDic.Add("Drill_DrillHoleEnd", drillHoleEnd);
                        }

                        faskDic.Add("Drill_ParkingPosition", parkingFlag);

                        try
                        {
                            if (loadFileFromCncUIOnOff)
                            {
                                var LoadFileFromCncUIFlag = InteractingDevice.cnc84Command.ReadCncNode<bool>($"ns=4;s=UI/normalized/custom/{DeviceDescriptor.Extra["LoadFileFromCncUIFlag"]}");
                                faskDic.Add("Drill_LoadFileFromCncUI", LoadFileFromCncUIFlag);
                            }
                        }
                        catch (Exception)
                        {


                        }

                        //添加班次信息
                        try
                        {
                            var shiftsStartTime = InteractingDevice.cnc84Command.ReadCncNode<DateTime[]>("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.StartTime");
                            if (shiftsStartTime.Length > 0)
                            {
                                faskDic.Add("Drill_ShiftsStartTime", shiftsStartTime[0].AddHours(8).ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                            var Drill_ShiftsTime = InteractingDevice.cnc84Command.ReadCncNode<double>("ns=4;s=UI/origin/ProductionTimes/MDE.ShiftsTable/MDE.ShiftsTime");

                            faskDic.Add("Drill_ShiftsTime", (Drill_ShiftsTime / 60 / 1000).ToString());
                            string dateString = "0001/01/01 08:00:00";
                            var originDateTime = DateTime.Parse(dateString);
                            var runStartTime = InteractingDevice.cnc84Command.ReadCncNode<DateTime>("ns=4;s=UI/origin/AutoData/AutoData.List/AutoData.RunStartTime");
                            if (runStartTime.AddHours(8) != originDateTime)
                            {
                                faskDic.Add("Drill_RunStartTime", runStartTime.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss"));
                            }

                            var runEndTime = InteractingDevice.cnc84Command.ReadCncNode<DateTime>("ns=4;s=UI/origin/AutoData/AutoData.List/AutoData.RunEndTime");
                            if (runEndTime.AddHours(8) != originDateTime)
                            {
                                faskDic.Add("Drill_RunEndTime", runEndTime.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                            faskDic.Add("Buffer_AllUnloadAndLoadEnd", InteractingDevice.bufferAllUnloadAndLoadEnd);
                        }
                        catch (Exception)
                        {
                        }

                        await Task.Delay(100);
                    }
                    else
                    {
                        await Task.Delay(2000);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError("FastCollectDataThread:" + e.Message);
                    await Task.Delay(6 * 1000);
                }
                if (faskDic.Count > 0)
                {
                    WatchingProperties.SetValues(faskDic);
                }
            }
            logger.LogDebug($"FastCollectDataThread:线程结束");
        }

        private DateTime StringToDateTime(string dateTimeString)
        {
            try
            {
                IFormatProvider culture = new CultureInfo("fr-FR", true);
                return DateTime.Parse(dateTimeString, culture, DateTimeStyles.NoCurrentDateDefault);
            }
            catch (FormatException)
            {
                Console.WriteLine("日期时间格式不正确！");
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生错误：" + ex.Message);
            }
            return default;
        }

        private void UpdatePropTime(string[] allData, string content, string prop)
        {
            var allTime = allData.Where(s => s.Contains(content)).Select(s =>
            {
                var content = s.Split("*");

                return StringToDateTime(content[0]);
            });
            if (allTime.Count() != 0)
            {
                var time = allTime.Last();
                WatchingProperties.Property(prop).SetValue(time.ToString("yyyy-MM-dd HH:mm:ss"));
                logger.LogDebug($"{content} ： {prop} 更新 时间{time.ToString("yyyy-MM-dd HH:mm:ss")} ");
            }
        }

        private async Task TimeStatisticThread()
        {
            logger.LogDebug($"TimeStatisticThread:开始了");
            await Task.Delay(2000);
            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus())
                    {
                        string showText = InteractingDevice.cnc84Command.ReadCncNode<string>("ns=4;s=UI/normalized/new/ShowText");
                        logger.LogDebug($"TimeStatisticThread ShowText ：{showText}");
                        WatchingProperties.Property("Drill_ShowText").SetValue(showText);

                        var error = InteractingDevice.cnc84Command.ReadCncNode<string[]>("ns=4;s=UI/origin/DDETable/CncErrorTable/CncError");

                        UpdatePropTime(error, "Tool life begin", "Drill_ToolLifeExpiredStartTime");
                        UpdatePropTime(error, "Tool life end", "Drill_ToolLifeExpiredEndTime");
                        UpdatePropTime(error, "Collet clean begin", "Drill_CollectCleanBegin");
                        UpdatePropTime(error, "Collet clean end", "Drill_CollectCleanEnd");
                        UpdatePropTime(error, "test pin begin", "Drill_TestPinStartTime");
                        UpdatePropTime(error, "test pin end", "Drill_TestPinEndTime");
                        UpdatePropTime(error, "No Vacuum begin", "Drill_NoVacuumStartTime");
                        UpdatePropTime(error, "No Vacuum end", "Drill_NoVacuumEndTime");

                        #region newUpdatePayloadpanel

                        //var bufferOnAgvPosition = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1);
                        //var isOnAgvPosition = bufferOnAgvPosition[0];
                        //if (oldIsOnAgvPosition != isOnAgvPosition)
                        //{
                        //    if (isOnAgvPosition) //在agv对接层
                        //    {
                        //        //更新板材
                        //        var material = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 2);
                        //        var bufferClinkerStatus = material[1];
                        //        var bufferRawStatus = material[0];

                        //        var boardDrillStatus = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);
                        //        var drillBoard = boardDrillStatus[0];

                        //        logger.LogError($"TimeStatisticThread update PayloadPanels  新的Buffer上层 {bufferRawStatus}  钻机{drillBoard}  新的BUFFER下层 {bufferClinkerStatus} ");
                        //        bool load = false;
                        //        bool unload = false;
                        //        load = (oldBufferRawStatus != 0 && bufferRawStatus == 0 && drillBoard != 0);
                        //        unload = (oldBufferClinkerStatus == 0 && bufferClinkerStatus != 0);

                        //        logger.LogError($"TimeStatisticThread update PayloadPanels  buffer上生料到钻机{load}  钻机上否下熟料到buffer {unload} ");

                        //        if (unload)
                        //        {
                        //            try
                        //            {
                        //                logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  下熟料 未修改 PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                        //                logger.LogDebug($"{DateTime.Now.ToShortTimeString()} Transid 修改前  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");
                        //                InteractingDevice.OldTransactionId = InteractingDevice.DrillTransactionId;
                        //                InteractingDevice.DrillTransactionId = "";
                        //                logger.LogDebug($"{DateTime.Now.ToShortTimeString()} Transid 修改后  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");
                        //                await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                        //                {
                        //                    for (int i = 2 * InteractingDevice.spindleNum; i < InteractingDevice.PayloadPanels.Count; i++)
                        //                    {
                        //                        InteractingDevice.PayloadPanels[i] = InteractingDevice.PayloadPanels[i - InteractingDevice.spindleNum];
                        //                        InteractingDevice.PayloadPanels[i]!.Layer = 2;
                        //                        if (InteractingDevice.PayloadPanels[i].ProductStatus == ProductStatus.Drilling)
                        //                        {
                        //                            InteractingDevice.PayloadPanels[i]!.ProductStatus = ProductStatus.Finished_DRILL;
                        //                        }
                        //                        var panel = Panel.HasSilo.NoPanelForSingleSpindle("", InteractingDevice.PayloadPanels[i].Position, 1, 1)[0];
                        //                        panel.LocationCode = InteractingDevice.DeviceId;
                        //                        panel.SiloCode = InteractingDevice.DeviceId;
                        //                        InteractingDevice.PayloadPanels[i - InteractingDevice.spindleNum] = panel;
                        //                    }
                        //                }));

                        //                logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  下熟料 已修改 PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                        //                await InteractingDevice.PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.DeviceId);
                        //            }
                        //            catch (Exception)
                        //            {
                        //                await Task.CompletedTask;
                        //            }

                        //        }
                        //        if (load)
                        //        {
                        //            try
                        //            {
                        //                ////控制板子转化
                        //                logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Transid 修改前  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");
                        //                InteractingDevice.DrillTransactionId = InteractingDevice.NewTranscationId;
                        //                InteractingDevice.NewTranscationId = "";
                        //                logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Transid 修改后  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");

                        //                logger.LogWarning($"{DateTime.Now.ToShortTimeString()} buffer上层给钻机上料  未修改【 PayloadPanels 】  {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                        //                string itemCode = string.Empty;

                        //                await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                        //                {
                        //                    for (int i = InteractingDevice.spindleNum; i < 2 * InteractingDevice.spindleNum; i++)
                        //                    {
                        //                        InteractingDevice.PayloadPanels[i] = InteractingDevice.PayloadPanels[i - InteractingDevice.spindleNum];
                        //                        if (!string.IsNullOrEmpty(InteractingDevice.PayloadPanels[i].ItemCode) && !string.IsNullOrEmpty(itemCode))
                        //                        {
                        //                            itemCode = InteractingDevice.PayloadPanels[i].ItemCode;
                        //                        }
                        //                        InteractingDevice.PayloadPanels[i]!.Layer = 1;
                        //                        if (InteractingDevice.PayloadPanels[i]!.ProductStatus == ProductStatus.WaitingForDrill)
                        //                        {
                        //                            InteractingDevice.PayloadPanels[i]!.ProductStatus = ProductStatus.Drilling;
                        //                        }
                        //                        var panel = Panel.HasSilo.NoPanelForSingleSpindle("", InteractingDevice.PayloadPanels[i].Position, 0, 1)[0];
                        //                        panel.LocationCode = InteractingDevice.DeviceId;
                        //                        panel.SiloCode = InteractingDevice.DeviceId;
                        //                        InteractingDevice.PayloadPanels[i - InteractingDevice.spindleNum] = panel;
                        //                    }
                        //                }));

                        //                logger.LogWarning($"{DateTime.Now.ToShortTimeString()}   buffer上层给钻机上料 已修改【 PayloadPanels 】 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                        //            }
                        //            catch (Exception)
                        //            {
                        //                await Task.CompletedTask;
                        //            }

                        //        }

                        //        oldBufferClinkerStatus = bufferClinkerStatus;
                        //        oldBufferRawStatus = bufferRawStatus;

                        //    }
                        //    else //不在agv对接层
                        //    {
                        //        //更新itemcode 获取原来钻机数据 buffer熟料层数据
                        //        InteractingDevice.ItemCode = InteractingDevice.PayloadPanels.Take(InteractingDevice.spindleNum).ToArray().Select(s => s.ItemCode).FirstOrDefault();

                        //        var material = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 2);
                        //        oldBufferClinkerStatus = material[1];
                        //        oldBufferRawStatus = material[0];

                        //        logger.LogError($"TimeStatisticThread update PayloadPanels 旧的Buffer上层{oldBufferRawStatus}   旧的BUFFER下层 {oldBufferClinkerStatus} ");

                        //    }
                        //    oldIsOnAgvPosition = isOnAgvPosition;

                        //}

                        #endregion newUpdatePayloadpanel

                        await Task.Delay(1000);
                    }
                    else
                    {
                        logger.LogDebug($"TimeStatisticThread:连接未建立");
                        await Task.Delay(6 * 1000);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError("TimeStatisticThread" + e.Message);
                    await Task.Delay(6 * 1000);
                }
            }
            logger.LogDebug($"FastCancelLockThread:线程结束");
        }

        /// <summary>
        /// 2024-05-29
        /// M0 1:Buffer在AGV对阶层，Buffer在安全位
        /// M0 0:Buffer上下料 95电信号锁机 Buffer不在安全位
        /// Buffer重新回到AGV对阶层【M0=1】，发送ESC取消报警【Buffer不在安全位】
        /// 2024-09-02
        /// 根据95界面 BUFFER未在安全位置锁机信号 再加 buffer是否回到安全位置
        /// </summary>
        /// <returns></returns>

        private async Task FastCancelLockThread()
        {
            logger.LogDebug($"FastCancelLockThread:开始了");
            await Task.Delay(2000);
            int count = -1;

            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus() && InteractingDevice.modbusIpMaster != null)
                    {
                        count = -1;
                        string blockText = InteractingDevice.cnc84Command.ReadCncNode<string>("ns=4;s=UI/origin/RosiInfo/BlockText");
                        logger.LogDebug($"判断是否有锁机信号: {blockText} ");
                        if (blockText.ToUpper().Contains("BUFFER未在安全位置") || blockText.ToUpper().Contains("ATRUN"))
                        {

                            InteractingDevice.EscFlag = false;
                            var bufferOnAgvPosition = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1)[0];//M0,
                            logger.LogDebug($"FastCancelLockThread:CNC95 锁机信号状态->BufferOnAgvPositionFlag->M0->：{bufferOnAgvPosition}");//M0 1:Buffer回到AGV对阶层
                            if (bufferOnAgvPosition)
                            {
                                do
                                {
                                    InteractingDevice.cnc84Command.Abort();
                                    await Task.Delay(200);

                                    InteractingDevice.cnc84Command.SetCncComand($"DSP,IIoT_Send_ESC_RPC_ABORT");
                                    logger.LogDebug($"FastCancelLockThread:发送ESC指令 ");
                                    await Task.Delay(200);
                                    string screentText = InteractingDevice.cnc84Command.ReadCncNode<string>("ns=4;s=UI/origin/RosiInfo/BlockText");
                                    logger.LogDebug($"发送ESC指令后获取: {screentText} ");
                                    //if (screentText.ToUpper() != DeviceDescriptor.Extra["LockMessage"].ToStr())
                                    if (!screentText.ToUpper().Contains("BUFFER未在安全位置") && !screentText.ToUpper().Contains("ATRUN"))
                                    {
                                        logger.LogDebug($"FastCancelLockThread:发送ESC指令成功, 已取消[BUFFER未在安全位置] ,退出do-while.");
                                        await Task.Delay(500);
                                        InteractingDevice.cnc84Command.SetCncComand($"DSP,BUFFER_NOT_IN_SAFE_LOCATION_CANCELED");
                                        break;
                                    }
                                } while (true);
                                InteractingDevice.EscFlag = true;
                                await Task.Delay(200);
                            }
                        }
                        else
                        {
                            await Task.Delay(1000);
                        }
                    }
                    else
                    {
                        count++;
                        if (count % 10 == 0)
                        {
                            logger.LogDebug($"FastCancelLockThread:ESC 线程 连接未建立");
                        }
                        await Task.Delay(6 * 1000);
                    }
                }
                catch (Exception e)
                {
                    count++;
                    if (count % 10 == 0)
                    {
                        logger.LogError("FastCancelLockThread:ESC 线程异常" + e.Message);
                    }
                    await Task.Delay(6 * 1000);
                }
            }
            logger.LogDebug($"FastCancelLockThread:线程结束");
        }

        private static string DeleteSpecialCharacter(string str)
        {
            str = str.Replace(@"\", "");
            str = str.Replace(@"/", "");
            str = str.Replace(@":", "");
            str = str.Replace(@"*", "");
            str = str.Replace(@"?", "");
            str = str.Replace(@"<", "");
            str = str.Replace(@">", "");
            str = str.Replace(@"|", "");
            return str;
        }

        public async Task<DeviceServiceInvokeResponse> ShutdownLocal()
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to Shutdown!");
            try
            {
                if (!InteractingDevice.cnc84Command.CNCCommandStatus())
                {
                    return await Response(ErrorCodes.Sys.FAIL, "CNC95断开链接");
                }
                //Use the command object RPC_SHUTDOWN to Close CNC and return to windows.
                //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_SHUTDOWN");
                //InteractingDevice.cnc84Command.Shutdown();
                InteractingDevice.cnc84Command.SetCncComand($"DSP,IIoT_Send_Start_RPC_SHUTDOWN");
                logger.LogDebug("发送关闭CNC95指令");
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} finish Shutdown!");

                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return await Response(ErrorCodes.Sys.FAIL, e.Message);
            }
        }

        public async Task<DeviceServiceInvokeResponse> StandbyLocal()
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to Standby!");
            try
            {
                if (!InteractingDevice.cnc84Command.CNCCommandStatus())
                {
                    return await Response(ErrorCodes.Sys.FAIL, "CNC84断开链接");
                }
                //Use the command object RPC_STOP to stop the execution. The Stop button is pressed.
                //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_STOP");
                //InteractingDevice.cnc84Command.Stop();
                InteractingDevice.cnc84Command.SetCncComand($"DSP,IIoT_Send_Start_RPC_STOP");
                logger.LogDebug("发送暂停指令");
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} finish Standby!");

                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return await Response(ErrorCodes.Sys.FAIL, e.Message);
            }
        }

        public async Task<DeviceServiceInvokeResponse> ScheduleTaskLocal()
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to ScheduleTask!");
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} finish ScheduleTask!");
            return await Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> WorkLocal()
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to Work!");
            try
            {
                if (!InteractingDevice.cnc84Command.CNCCommandStatus())
                {
                    return await Response(ErrorCodes.Sys.FAIL, "CNC95断开链接");
                }

                StartChangeF8();
                logger.LogDebug("切换到F8界面");
                //Use the command object RPC_START to start the execution (again). The Start key is pressed.
                //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_START");
                InteractingDevice.cnc84Command.Start();
                InteractingDevice.cnc84Command.SetCncComand($"DSP,IIoT_Send_Start_RPC_START");
                logger.LogDebug("发送打板指令");
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} finish Work!");

                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return await Response(ErrorCodes.Sys.FAIL, e.Message);
            }
        }

        /// <summary>
        /// //模拟按下F8键
        /// keybd_event(vbKeyF8, 0, 0, 0);
        /// //松开按键F8
        /// keybd_event(vbKeyF8, 0, 2, 0);
        /// </summary>
        private void StartChangeF8()
        {
            InteractingDevice.cnc84Command.SetChangePage("WORK_WORK");
            InteractingDevice.cnc84Command.SetCncComand($"DSP,keybd_event_vbKeyF8");
        }

        private void CNC84Connect()
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}   begin to Cnc84Connect!");
            InteractingDevice.cnc84Command = InteractingDevice.cNC84CommandWrapper.CreateCNCCommand();
            InteractingDevice.cnc84Command.Init(DeviceDescriptor.Extra["CNC84Ip"].ToStr(), DeviceDescriptor.Extra["CNC84Port"].ToInt());
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}   finish Cnc84Connect!");
        }

        private bool ComConnect()
        {
            try
            {
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}   begin to ComConnect!");
                DefaultDrill.port.PortName = DeviceDescriptor.Extra["ScanCom"].ToStr();
                DefaultDrill.port.DataBits = 8;
                DefaultDrill.port.BaudRate = 9600;//设置波特率
                DefaultDrill.port.StopBits = StopBits.One;
                DefaultDrill.port.Parity = Parity.None;
                DefaultDrill.port.DataReceived += Port_DataReceived;
                if (DefaultDrill.port.IsOpen)
                {
                    DefaultDrill.port.Close();
                }
                Task.Delay(1000);
                DefaultDrill.port.Open();
                return DefaultDrill.port.IsOpen;
            }
            catch (Exception e)
            {
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}  ComConnect {e.Message}!");
            }
            return false;
        }

        private bool ScanConnect()
        {
            try
            {
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}   begin to ScanConnect!");
                InteractingDevice.codeReaderBaseSetting = InteractingDevice.ObjectFactory.CreateObject<CodeReaderBaseSetting>(InteractingDevice);
                var flag = InteractingDevice.codeReaderBaseSetting.StartScanConnect();
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}   连接读码器服务器成功  {flag}!");
                return flag;
            }
            catch (Exception e)
            {
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}  ComConnect {e.Message}!");
            }
            return false;
        }

        private bool PlcConnect()
        {
            try
            {
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}   begin to PlcConnect!");
                InteractingDevice.modbusIpMaster = InteractingDevice.modbusIpMasterWrapper.CreateIp(DeviceDescriptor.Extra["Ip"].ToStr(), DeviceDescriptor.Extra["Port"].ToInt());
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}   finish  PlcConnect!");
                return true;
            }
            catch (Exception e)
            {
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}  PlcConnect {e.Message}!");
            }
            return false;
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            logger.LogDebug("触发扫码枪");
            byte[] RecData = new byte[1024];
            DefaultDrill.port.Read(RecData, 0, 1024);
            var str = System.Text.Encoding.UTF8.GetString(RecData).Split('\r', ';')[0];
            if (str.Length > 20)
            {
                str = str.Substring(0, 20);
            }
            DefaultDrill.port.DiscardInBuffer();
            logger.LogDebug("获取的二维码信息：" + str);

            str = DeleteSpecialCharacter(str);
            WatchingProperties.Property("Drill_CodeMessage").SetValue(str);
        }

        //钻带代理是最后执行机构 ，定位问题要从源头找起，首先要确认一下agv是否有相关的数据反馈给中控 再排查中控是否有相关数据下发
        //2024-07-27
        //异常后  解除buffer报警后 buffer会重新发起呼叫 接着上下料  直到中控下发调度完成信号。异常后  不想等中控调度 人员处理，解除buffer报警后  可以点击触摸屏上解除总的上下料信号
        //agv开始操作buffer的时候 这个信号就变成0了  中控不下发 触摸屏不点  这个信号一直是0
        public string CompleteAllEndLocal()
        {
            try
            {
                var end = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 200, 3);//200
                if (end[0] > 1 || end[2] > 1)
                {
                    return $"强制上下料信号 不能执行";
                }
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 1);
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["BufferAgvUnLoadingAndLoading"].ToUshort(), 0);
                return $"强制上下料信号 执行结束";
            }
            catch (Exception ee)
            {
                return $"强制上下料结束信号发生异常：{ee.Message}";
            }
        }

        public List<Panel> InitPayloadPanels()
        {
            var panels = Panel.HasSilo.NoPanelForLayerFirst(DeviceDescriptor.DeviceId, InteractingDevice.spindleNum, 3);
            foreach (var item in panels)
            {
                item.LocationCode = DeviceDescriptor.DeviceId;
                item.SiloCode = InteractingDevice.DeviceId;
            }
            return panels;
        }

        public void CodeReaderCheck(int checkResult)
        {
            try
            {
                //清空该轴的读码器验证信息
                //
                var checkOld = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferCodeReaderCheckOnPlc"].ToUshort(), 1)[0];
                logger.LogDebug($"获取的读码器 写确认信号 原始数据{checkOld} {Convert.ToString(checkOld, 2).PadLeft(DeviceDescriptor.SpindleNum.ToInt(), '0')}");

                var checkNew = checkResult;
                logger.LogDebug($"修改的读码器 写确认信号  数据{checkNew} {Convert.ToString(checkNew, 2).PadLeft(DeviceDescriptor.SpindleNum.ToInt(), '0')}");

                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["BufferCodeReaderCheckOnPlc"].ToUshort(), (ushort)checkNew);
            }
            catch (Exception ee)
            {
                logger.LogError($"读码器触发器 写确认信号 异常 {ee.Message}");
            }
        }

        public void CodeReaderSingleSplindleCheck(int splindleNum, int checkResult)
        {
            try
            {
                if (checkResult == 0)
                {
                    var checkOld = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferCodeReaderCheckOnPlc"].ToUshort(), 1)[0];
                    logger.LogDebug($" checkResult==0 获取的读码器 写确认信号 原始数据{checkOld} {Convert.ToString(checkOld, 2).PadLeft(DeviceDescriptor.SpindleNum.ToInt(), '0')}");

                    var checkNew = checkOld & ~(1 << splindleNum - 1);
                    logger.LogDebug($"checkResult==0 修改的读码器 写确认信号  数据{checkNew} {Convert.ToString(checkNew, 2).PadLeft(DeviceDescriptor.SpindleNum.ToInt(), '0')}");

                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["BufferCodeReaderCheckOnPlc"].ToUshort(), (ushort)checkNew);
                }
                else
                {
                    //
                    var checkOld = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferCodeReaderCheckOnPlc"].ToUshort(), 1)[0];
                    logger.LogDebug($"checkResult==1 获取的读码器 写确认信号 原始数据{checkOld} {Convert.ToString(checkOld, 2).PadLeft(DeviceDescriptor.SpindleNum.ToInt(), '0')}");

                    var checkNew = checkOld | (1 << (splindleNum - 1));
                    logger.LogDebug($"checkResult==1 修改的读码器 写确认信号  数据{checkNew} {Convert.ToString(checkNew, 2).PadLeft(DeviceDescriptor.SpindleNum.ToInt(), '0')}");

                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["BufferCodeReaderCheckOnPlc"].ToUshort(), (ushort)checkNew);
                }
            }
            catch (Exception ee)
            {
                logger.LogError($"读码器触发器 写确认信号 异常 {ee.Message}");
            }
        }

        public async Task<DeviceServiceInvokeResponse> SetScannerLot(DeviceServiceInvokeRequest request)
        {
            var response = DeviceServiceInvokeResponse.FAIL;
            try
            {
                var loadDrill = ObjectFactory.CreateObject<Load95DrlFileCommand>(InteractingDevice);
                return await loadDrill.SetScannerLot(request);
            }
            catch (Exception ee)
            {
                response.Message = ee.Message;
                logger.LogError($"SetScannerLot 异常 {ee.Message} ");
                return response;
            }
        }

        public async Task<DeviceServiceInvokeResponse> LoadAtpFile(DeviceServiceInvokeRequest request)
        {
            var response = DeviceServiceInvokeResponse.FAIL;
            try
            {
                var loadatp = ObjectFactory.CreateObject<Load95AtpCommand>(InteractingDevice);
                return await loadatp.LoadAtpFile(request);
            }
            catch (Exception ee)
            {
                response.Message = ee.Message;
                logger.LogError($"LoadAtpFile 异常 {ee.Message} ");
                return response;
            }
        }

        public Task<DeviceServiceInvokeResponse> CodeReaderOnOffToCnc(DeviceServiceInvokeRequest request) => throw new NotImplementedException();
    }
}
