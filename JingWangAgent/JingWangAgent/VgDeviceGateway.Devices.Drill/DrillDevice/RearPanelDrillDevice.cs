// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.IO.Ports;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Drill.Other.CodeReader;

namespace VgDeviceGateway.Devices.Drill.DrillDevice
{
    public class RearPanelDrillDevice : DeviceShare<DefaultDrill>, IDrillDevice
    {
        private readonly ILogger<RearPanelDrillDevice> logger;
        public readonly bool pinCheckOnOff;
        public readonly byte slaveID;
        public readonly bool codeReaderTriggerIsM;
        public readonly bool codeReaderLocalConfirmOnOff;
        public readonly bool outIntFlagFromIntOnOff;
        public readonly bool checkDrillHoles = true;
        public readonly bool abCheckOnOff;

        public RearPanelDrillDevice(ILogger<RearPanelDrillDevice> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            slaveID = (byte)device.DeviceDescriptor.Extra["SlaveID"].ToInt();
            this.pinCheckOnOff = device.DeviceDescriptor.Extra["PinCheckOnOff"].ToBool();
            codeReaderTriggerIsM = device.DeviceDescriptor.Extra["CodeReaderTriggerIsM"].ToBool();
            codeReaderLocalConfirmOnOff = device.DeviceDescriptor.Extra["CodeReaderLocalConfirmOnOff"].ToBool();
            outIntFlagFromIntOnOff = DeviceDescriptor.Extra["OutIntFlagFromIntOnOff"].ToBool();
            abCheckOnOff = device.DeviceDescriptor.Extra["ABCheckOnOff"].ToBool();
            if (device.codeReaderOnOff)
            {
                CodeReaderBaseSetting.CodeReaderTriggerBeforeAction += CodeReaderTriggerBefore;
                CodeReaderBaseSetting.CodeReaderReceiveAction += CodeReaderReceive;
            }
        }

        private void CodeReaderReceive(int spindleNum, string codeReaderContent)
        {
            //获取数据
            //更新板材信息
            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  读码器获取的是{spindleNum}轴  内容{codeReaderContent}");
            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  读码器 未修改【 PayloadPanels 】 {JsonSerializer.Serialize(PayloadPanels)} ");
            PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
            {
                InteractingDevice.PayloadPanels[spindleNum].Barcode = codeReaderContent;
            })).GetAwaiter().GetResult();
            PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.DeviceId).GetAwaiter().GetResult();
            if (codeReaderLocalConfirmOnOff) CodeReaderSingleSplindleCheck(spindleNum + 1, 1);
            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  读码器 已修改【 PayloadPanels 】 {JsonSerializer.Serialize(PayloadPanels)} ");
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
                Cnc84Connect();

            var cnc84Flag = InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus();

            var comFlag = true;
            if (InteractingDevice.scanGunOnOff)
            {
                comFlag = ComConnect();
            }

            var codeFlag = true;
            if (InteractingDevice.codeReaderOnOff)
            {
                codeFlag = ScanConnect();
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
            logger.LogDebug($"CompleteScheduleLocal 方法被调用了 调度状态 {ScheduledStatus}：重置{InteractingDevice.allowAllAgv}");
            InteractingDevice.allowAllAgv = true;
            InteractingDevice.TransactionId = string.Empty;
        }

        public void FailScheduleLocal()
        {
            logger.LogDebug($"CancelSchedule 方法被调用了：重置{InteractingDevice.allowAllAgv}");
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
            // Task.Factory.StartNew(WatchDogThread, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(FastCollectDataThread);
            Task.Factory.StartNew(FastCancelLockThread);
            Task.Factory.StartNew(HeartBeatThread);
            if (pinCheckOnOff) Task.Factory.StartNew(FrontPinCheckThread, TaskCreationOptions.LongRunning);
            if (InteractingDevice.codeReaderOnOff) Task.Factory.StartNew(CodeReaderThread, TaskCreationOptions.LongRunning);
            if (abCheckOnOff) Task.Factory.StartNew(DrillHoleCheckThread, TaskCreationOptions.LongRunning);
            return Task.CompletedTask;
        }

        private async Task WatchDogThread()
        {
            try
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
            }
            catch (Exception)
            {
            }

            logger.LogDebug($"WatchDogThread 线程结束");
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

        //监控程式开始和第一次结束过程中轴状态是否有变化
        private async Task DrillHoleCheckThread()
        {
            logger.LogDebug($"DrillHoleCheckThread:开始");
            await Task.Delay(10000);
            bool isPgmFirstEnd = false;
            string strStartSpindleStatus = string.Empty;
            bool isSpindleChanged = false;

            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (InteractingDevice.modbusIpMaster != null && InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus())
                    {
                        var fCall = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["FCallPostion"].ToUshort(), 1)[0];
                        logger.LogDebug($"DrillHoleCheckThread - 从plc获取打板fcall信号： {fCall}");
                        if (fCall)
                        {
                            strStartSpindleStatus = InteractingDevice.cnc84Command.GetCncStatus().SpindleStatus;
                            logger.LogInformation($"DrillHoleCheckThread - 开始打板时轴状态 {strStartSpindleStatus}");

                            while (fCall && !isPgmFirstEnd)
                            {
                                var strHoleData = InteractingDevice.cnc84Command.GetRuntimeString("%S(HFCNTValue)")?.Replace("1:", "");

                                if (strHoleData.Contains(@"_\_"))
                                {
                                    string[] holeData = strHoleData.Split([@"_\_"], StringSplitOptions.None);
                                    string strCurHole = holeData[0].Trim();
                                    string strTotalHole = holeData[1].Trim();
                                    if (strCurHole == strTotalHole)
                                    {
                                        isPgmFirstEnd = true;
                                        logger.LogInformation($"DrillHoleCheckThread - 第一次打板结束，当前孔数 {strCurHole}，总孔数{strTotalHole}，是否需要补孔isSpindleChanged{isSpindleChanged}");
                                    }
                                }
                                else
                                {
                                    logger.LogError($"DrillHoleCheckThread - 采集孔数数据格式错误 - {strHoleData}");
                                }

                                if (fCall && !isPgmFirstEnd && !isSpindleChanged)
                                {
                                    var curSpindleStatus = InteractingDevice.cnc84Command.GetCncStatus().SpindleStatus;
                                    if (strStartSpindleStatus != curSpindleStatus)
                                    {
                                        logger.LogInformation($"DrillHoleCheckThread - 检测到在程式运行期间轴状态发生变化,初始轴状态 {strStartSpindleStatus},当前轴状态 {curSpindleStatus}");
                                        isSpindleChanged = true;

                                        string changeFlag = "1";
                                        logger.LogInformation($"DrillHoleCheckThread - PATCHING_HOLES UserFlag 即将设置成的用户标记：{changeFlag}");
                                        string flag = InteractingDevice.cnc84Command.GetUserFlag(DeviceDescriptor.Extra["PatchHolesUserFlag"].ToInt());  //
                                        logger.LogInformation($"DrillHoleCheckThread - PATCHING_HOLES 获取的cnc84目前用户标记：{flag}");
                                        if (!changeFlag.Equals(flag, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            InteractingDevice.cnc84Command.SetUserFlag(changeFlag == "1" ? 1 : 0, DeviceDescriptor.Extra["PatchHolesUserFlag"].ToInt());
                                            logger.LogInformation($"DrillHoleCheckThread - PATCHING_HOLES 已修改84用户标记：{changeFlag}");
                                        }
                                    }
                                }

                                fCall = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["FCallPostion"].ToUshort(), 1)[0];
                                //logger.LogDebug($"DrillHoleCheckThread - 从plc获取打板fcall信号： {fCall}");
                                if (!fCall)
                                {
                                    logger.LogInformation($"DrillHoleCheckThread - 从plc获取打板结束fcall信号： {fCall}");
                                    isPgmFirstEnd = false;
                                    isSpindleChanged = false;
                                }
                            }
                        }
                        else
                        {
                            isPgmFirstEnd = false;
                            isSpindleChanged = false;
                        }
                    }
                    else
                    {
                        logger.LogError($"DrillHoleCheckThread - modbusIpMaster为空或cnc84Command为空或cnc84Command.CNCCommandStatus()为空,{InteractingDevice.cnc84Command?.CNCCommandStatus()}");
                    }
                }
                catch (Exception e)
                {
                    logger.LogError("DrillHoleCheckThread 线程异常 - " + e.Message);
                    await Task.Delay(6 * 1000);
                }
            } //end of while

            logger.LogDebug($"DrillHoleCheckThread 线程结束");
        }

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
                        //获取轴状态
                        var cncStatus = InteractingDevice.cnc84Command.GetCncStatus();
                        string splineStatusStr = string.Join("", Enumerable.Repeat("1", DeviceDescriptor.SpindleNum).ToArray());
                        var splineStatus = cncStatus?.SpindleStatus;
                        if (splineStatus != null)
                        {
                            splineStatusStr = splineStatus?.Substring(splineStatus.Length - InteractingDevice.spindleNum);
                        }

                        var frontPin = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1)[0];
                        var frontPinStr = Convert.ToString(frontPin, 2).PadLeft(InteractingDevice.spindleNum, '0');
                        var frontPinAlarm = IsAlarm(splineStatusStr, frontPinStr);

                        var rearPin = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["RearPinCheckOnPlc"].ToUshort(), 1)[0];

                        var rearPinStr = Convert.ToString(rearPin, 2).PadLeft(InteractingDevice.spindleNum, '0');
                        var rearPinAlarm = IsAlarm(splineStatusStr, rearPinStr);

                        var cvCheck = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["CVCheckOnPlc"].ToUshort(), 1)[0];

                        var cvCheckStr = Convert.ToString(cvCheck, 2).PadLeft(InteractingDevice.spindleNum, '0');
                        var cvCheckAlarm = IsAlarm(splineStatusStr, cvCheckStr, '1');

                        SetUserFlag(frontPinAlarm, rearPinAlarm, cvCheckAlarm);

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

            void SetUserFlag(bool frontPinAlarm, bool rearPinAlarm, bool cvCheckAlarm)
            {
                //判断是否一致，不一致再修改
                SetSingleUserFlag(frontPinAlarm, "FrontPinCheckUserFlag");
                SetSingleUserFlag(rearPinAlarm, "RearPinCheckUserFlag");
                SetSingleUserFlag(cvCheckAlarm, "CVCheckUserFlag");

                void SetSingleUserFlag(bool dataAlarm, string userFlag)
                {
                    string changeFlag = dataAlarm ? "1" : "0";
                    logger.LogInformation($"{userFlag} 即将设置成的用户标记：{changeFlag}");
                    string flag = InteractingDevice.cnc84Command.GetUserFlag(DeviceDescriptor.Extra[userFlag].ToInt());
                    logger.LogInformation($"{userFlag} 获取的cnc84目前用户标记：{flag}");
                    if (!changeFlag.Equals(flag, StringComparison.CurrentCultureIgnoreCase))
                    {
                        InteractingDevice.cnc84Command.SetUserFlag(dataAlarm ? 1 : 0, DeviceDescriptor.Extra[userFlag].ToInt());
                        logger.LogInformation($"{userFlag} 修改84用户标记：{changeFlag}");
                    }
                }
            }
        }

        private bool IsAlarm(string splineStatusStr, string data, char status = '0')
        {
            logger.LogDebug($"轴状态 :{splineStatusStr} 需要验证的数据 {data}  比对的数据 {status}");
            var modifyData = splineStatusStr.Select((str, index) =>
            {
                if (str == '1' && data[index] == status) return '1';
                else
                    return '0';
            });
            return !string.Join("", Enumerable.Repeat('0', DeviceDescriptor.SpindleNum).ToArray()).Equals(string.Join("", modifyData.ToArray()));
        }

        private async Task HeartBeatThread()
        {
            logger.LogDebug($"HeartBeatThread 开始了");
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
                        logger.LogDebug($"HeartBeatThread plc 连接未建立");
                        await Task.Delay(6 * 1000);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError("HeartBeatThread 线程异常" + e.Message);
                    await Task.Delay(6 * 1000);
                }
            }
            logger.LogDebug($"HeartBeatThread 线程结束");
        }

        private async Task FastCollectDataThread()
        {
            await Task.Delay(2000);
            string oldDrillHoleEndStr = string.Empty;
            int drillHoleEndCount = 0;

            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                var faskDic = new Dictionary<string, object?>();
                try
                {
                    if (InteractingDevice.modbusIpMaster != null)
                    {
                        InteractingDevice.bufferAllUnloadAndLoadEnd = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 1)[0];
                        var unLoadAndLoadEndFlag = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["UnloadAndLoadEndOnPlc"].ToUshort(), 1);
                        faskDic.Add("Buffer_UnLoadAndLoadEndFlag", unLoadAndLoadEndFlag[0]);
                        logger.LogDebug($"buffer给钻机上料结束信号： {unLoadAndLoadEndFlag[0]}");

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

                        faskDic.Add("Buffer_Warning", warnning[0] != 0 || warnning[1] != 0);
                        faskDic.Add("Buffer_CallAgvMessage", InteractingDevice.allowAllAgv);
                        faskDic.Add("MqttConnected", InteractingDevice.MqttClientWrapper?.IsConnected == null ? false : MqttClientWrapper?.IsConnected);
                        faskDic.Add("TranscationId", InteractingDevice.TransactionId);
                        faskDic.Add("NewTranscationId", InteractingDevice.NewTranscationId);
                        var material = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 2);
                        InteractingDevice.material = material;
                        logger.LogDebug($"Buffer上层传感器的内容： {material[0]}");
                        logger.LogDebug($"Buffer下层传感器的内容： {material[1]}");
                        faskDic.Add("Buffer_RawMaterialLayerBoardStatus", material[0]);
                        faskDic.Add("Buffer_ClinkerLayerBoardStatus", material[1]);
                        //在agv对接层 自动 非异常  ready状态
                        if (bufferOnAgvPosition[0] && work[1] == 1 && (warnning[0] == 0 && warnning[1] == 0) && ready[0] == 1 && DeviceDescriptor.AutoMode && InteractingDevice.MqttClientWrapper.IsConnected)
                        {
                            InteractingDevice.isAvailbleForAgv = true;
                        }
                        else
                        {
                            InteractingDevice.isAvailbleForAgv = false;
                        }
                        faskDic.Add("IsAvailbleForAgv", InteractingDevice.isAvailbleForAgv);
                        var boardStatus = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);
                        faskDic.Add("Drill_BoardPositionStatus", boardStatus[0]);
                        logger.LogDebug($"钻机上传感器的内容： {boardStatus[0]}");
                        //Buffer_MoRunning
                        var codeReaderTrigger = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["CodeReaderTrigger"].ToUshort(), InteractingDevice.spindleNum.ToUshort());
                        faskDic.Add("Buffer_MoRunning", codeReaderTrigger.Any(s => s == true));

                        var autoCodeReaderTrigger = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort(), 1)[0];
                        faskDic.Add("Buffer_AutoMoRunning", autoCodeReaderTrigger == 1);
                        var receiveBoardFlag = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReceiveBoardFlag"].ToUshort(), 1);
                        faskDic.Add("Buffer_LoadEnd", receiveBoardFlag[0]);
                        logger.LogDebug($"FastCollectDataThread:D411->Buffer收到生料： {receiveBoardFlag[0]}");

                        var loadAndUnload = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferIsLoadAndUnloadOnPlc"].ToUshort(), 1);
                        faskDic.Add("Buffer_LoadAndUnload", loadAndUnload[0]);
                        logger.LogDebug($"FastCollectDataThread:D505->Buffer既上又下： {loadAndUnload[0]}");
                        if (!DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool())
                        {
                            // logger.LogError("电信号 采集");
                            var fCall = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["FCallPostion"].ToUshort(), 1)[0];
                            logger.LogDebug($"从plc获取打板fcall信号： {fCall}");
                            faskDic.Add("Drill_DrillHoleStart", fCall);
                            faskDic["Drill_DrillHoleEnd"] = !fCall;
                        }

                        await Task.Delay(10);
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
                    logger.LogError("FastCollectDataThread" + e.Message);
                }
                if (DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool())
                {
                    //logger.LogError("非电信号 需要写plc");
                    try
                    {
                        if (InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus())
                        {
                            if (outIntFlagFromIntOnOff)
                            {
                                var bufferLock = InteractingDevice.cnc84Command.GetIutput(DeviceDescriptor.Extra["Cnc84BufferLockOnCnc84"].ToInt()) == "2";
                                faskDic.Add("Drill_BufferLockFlag", bufferLock);
                                var drillHoleEndStr = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["DrilBoardEndFlag"].ToInt(), true);
                                logger.LogDebug($"钻孔获取的实时 int  结束信号：{drillHoleEndStr}");
                                if (drillHoleEndStr == null || !drillHoleEndStr.Equals(oldDrillHoleEndStr))
                                {
                                    oldDrillHoleEndStr = drillHoleEndStr;
                                    drillHoleEndCount = 0;
                                }
                                else
                                {
                                    drillHoleEndCount++;
                                    if (drillHoleEndCount >= 2000)
                                    {
                                        drillHoleEndCount = 3;
                                    }
                                }
                                if (drillHoleEndCount >= 3)
                                {
                                    var drillHoleEnd = drillHoleEndStr == "1";
                                    if (!DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool())
                                    {
                                        drillHoleEnd = !drillHoleEnd;
                                    }
                                    logger.LogDebug($"钻孔结束 int 信号标记：{drillHoleEnd}");
                                    faskDic["Drill_DrillHoleEnd"] = drillHoleEnd;

                                    if (drillHoleEnd)
                                    {
                                        var parkingStationEndFrontFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["P4FrontPostion"].ToInt(), true);
                                        var parkingStationEndBehindFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["P4RearPostion"].ToInt(), true);
                                        logger.LogDebug($"泊车位信号  int 原始信号 前{parkingStationEndFrontFlag} 后 {parkingStationEndBehindFlag}");
                                        var parkingFlag = parkingStationEndFrontFlag == "0" && parkingStationEndBehindFlag == "1";
                                        logger.LogDebug($"泊车位信号int  ：{parkingFlag}");
                                        if (parkingFlag)
                                        {
                                            InteractingDevice.modbusIpMaster?.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["CanUnloadAndLoadFlagWritePlc"].ToUshort(), 1);
                                        }
                                        else
                                        {
                                            InteractingDevice.modbusIpMaster?.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["CanUnloadAndLoadFlagWritePlc"].ToUshort(), 0);
                                        }
                                        faskDic["Drill_ParkingPosition"] = parkingFlag;
                                    }
                                    else
                                    {
                                        InteractingDevice.modbusIpMaster?.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["CanUnloadAndLoadFlagWritePlc"].ToUshort(), 0);
                                        faskDic["Drill_ParkingPosition"] = 0;
                                    }
                                }
                                else
                                {
                                    InteractingDevice.modbusIpMaster?.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["CanUnloadAndLoadFlagWritePlc"].ToUshort(), 0);
                                    faskDic["Drill_ParkingPosition"] = 0;
                                }
                            }
                            else
                            {
                                var bufferLock = InteractingDevice.cnc84Command.GetIutput(DeviceDescriptor.Extra["Cnc84BufferLockOnCnc84"].ToInt()) == "1:2";
                                faskDic.Add("Drill_BufferLockFlag", bufferLock);
                                var drillHoleEndStr = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["DrilBoardEndFlag"].ToInt());
                                logger.LogDebug($"钻孔获取的实时 结束信号：{drillHoleEndStr}");
                                if (drillHoleEndStr == null || !drillHoleEndStr.Equals(oldDrillHoleEndStr))
                                {
                                    oldDrillHoleEndStr = drillHoleEndStr;
                                    drillHoleEndCount = 0;
                                }
                                else
                                {
                                    drillHoleEndCount++;
                                    if (drillHoleEndCount >= 2000)
                                    {
                                        drillHoleEndCount = 3;
                                    }
                                }
                                if (drillHoleEndCount >= 3)
                                {
                                    var drillHoleEnd = drillHoleEndStr == "1:1";
                                    if (!DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool())
                                    {
                                        drillHoleEnd = !drillHoleEnd;
                                    }
                                    logger.LogDebug($"钻孔结束信号标记：{drillHoleEnd}");
                                    faskDic["Drill_DrillHoleEnd"] = drillHoleEnd;

                                    if (drillHoleEnd)
                                    {
                                        var parkingStationEndFrontFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["P4FrontPostion"].ToInt());
                                        var parkingStationEndBehindFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["P4RearPostion"].ToInt());
                                        logger.LogDebug($"泊车位信号 原始信号 前{parkingStationEndFrontFlag} 后 {parkingStationEndBehindFlag}");
                                        var parkingFlag = parkingStationEndFrontFlag == "1:0" && parkingStationEndBehindFlag == "1:1";
                                        logger.LogDebug($"泊车位信号：{parkingFlag}");
                                        if (parkingFlag)
                                        {
                                            InteractingDevice.modbusIpMaster?.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["CanUnloadAndLoadFlagWritePlc"].ToUshort(), 1);
                                        }
                                        else
                                        {
                                            InteractingDevice.modbusIpMaster?.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["CanUnloadAndLoadFlagWritePlc"].ToUshort(), 0);
                                        }
                                        faskDic["Drill_ParkingPosition"] = parkingFlag;
                                    }
                                    else
                                    {
                                        InteractingDevice.modbusIpMaster?.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["CanUnloadAndLoadFlagWritePlc"].ToUshort(), 0);
                                        faskDic["Drill_ParkingPosition"] = 0;
                                    }
                                }
                                else
                                {
                                    InteractingDevice.modbusIpMaster?.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["CanUnloadAndLoadFlagWritePlc"].ToUshort(), 0);
                                    faskDic["Drill_ParkingPosition"] = 0;
                                }
                            }

                            await Task.Delay(10);
                        }
                        else
                        {
                            await Task.Delay(100);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError("FastCollectDataThread" + e.Message);
                        await Task.Delay(6 * 1000);
                    }
                }

                if (faskDic.Count > 0)
                {
                    WatchingProperties.SetValues(faskDic);
                }
            }

            logger.LogDebug($"FastCollectDataThread 线程结束");
        }

        private async Task FastCancelLockThread()
        {
            logger.LogDebug($"FastCancelLockThread 开始了");
            await Task.Delay(2000);
            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus() && InteractingDevice.modbusIpMaster != null)
                    {
                        logger.LogDebug($"outIntFlagFromIntOnOff {outIntFlagFromIntOnOff}");
                        if (outIntFlagFromIntOnOff)
                        {
                            var lockInf = InteractingDevice.cnc84Command.GetIutput(DeviceDescriptor.Extra["Cnc84BufferLockOnCnc84"].ToInt(), true);
                            logger.LogDebug($"CNC84   int   锁机信号状态：{lockInf}");
                            var bufferLock = lockInf == "2";
                            if (bufferLock)
                            {
                                InteractingDevice.EscFlag = false;
                                var bufferOnAgvPosition = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1);
                                logger.LogDebug($"buffer int  是否回到AGV对阶层：{bufferOnAgvPosition[0]}");
                                if (bufferOnAgvPosition[0])
                                {
                                    do
                                    {
                                        InteractingDevice.cnc84Command.SetPcKey(@"\ESC");
                                        logger.LogDebug($"{DateTime.Now.ToLongTimeString()}- int 发送ESC指令 ");
                                        await Task.Delay(1000);
                                        var lockFlag = InteractingDevice.cnc84Command.GetIutput(DeviceDescriptor.Extra["Cnc84BufferLockOnCnc84"].ToInt(), true);
                                        logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-int 发送ESC指令后获取 {lockFlag} ");
                                        if (!"2".Equals(lockFlag))
                                        {
                                            break;
                                        }
                                    } while (true);
                                    InteractingDevice.EscFlag = true;
                                    await Task.Delay(2000);
                                }
                            }
                            else
                            {
                                await Task.Delay(1000);
                            }
                        }
                        else
                        {
                            var lockInf = InteractingDevice.cnc84Command.GetIutput(DeviceDescriptor.Extra["Cnc84BufferLockOnCnc84"].ToInt());
                            logger.LogDebug($"CNC84 锁机信号状态：{lockInf}");
                            var bufferLock = lockInf == "1:2";
                            if (bufferLock)
                            {
                                InteractingDevice.EscFlag = false;
                                var bufferOnAgvPosition = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1);
                                logger.LogDebug($"buffer 是否回到AGV对阶层：{bufferOnAgvPosition[0]}");
                                if (bufferOnAgvPosition[0])
                                {
                                    do
                                    {
                                        InteractingDevice.cnc84Command.SetPcKey(@"\ESC");
                                        logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-发送ESC指令 ");
                                        await Task.Delay(1000);
                                        var lockFlag = InteractingDevice.cnc84Command.GetIutput(DeviceDescriptor.Extra["Cnc84BufferLockOnCnc84"].ToInt());
                                        logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-发送ESC指令后获取 {lockFlag} ");
                                        if (!"1:2".Equals(lockFlag))
                                        {
                                            break;
                                        }
                                    } while (true);
                                    InteractingDevice.EscFlag = true;
                                    await Task.Delay(2000);
                                }
                            }
                            else
                            {
                                await Task.Delay(1000);
                            }
                        }
                    }
                    else
                    {
                        // logger.LogDebug($"esc 线程 连接未建立");
                        await Task.Delay(6 * 1000);
                    }
                }
                catch (Exception)
                {
                    //logger.LogError("esc 线程异常" + e.Message);
                    await Task.Delay(6 * 1000);
                }
            }
            logger.LogDebug($"FastCancelLockThread 线程结束");
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
                    return await Response(ErrorCodes.Sys.FAIL, "CNC84断开链接");
                }

                InteractingDevice.cnc84Command.Shutdown();
                logger.LogDebug("发送关闭cnc84指令");
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

                InteractingDevice.cnc84Command.Stop();
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
                    return await Response(ErrorCodes.Sys.FAIL, "CNC84断开链接");
                }

                StartChangeF8();
                logger.LogDebug("切换到F8界面");
                InteractingDevice.cnc84Command.Start();
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

        private void StartChangeF8()
        {
            InteractingDevice.cnc84Command.SetChangePage("WORK_WORK");
        }

        private void Cnc84Connect()
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

        public Task<DeviceServiceInvokeResponse> SetScannerLot(DeviceServiceInvokeRequest request) => throw new NotImplementedException();

        public Task<DeviceServiceInvokeResponse> LoadAtpFile(DeviceServiceInvokeRequest request) => throw new NotImplementedException();
        public Task<DeviceServiceInvokeResponse> CodeReaderOnOffToCnc(DeviceServiceInvokeRequest request) => throw new NotImplementedException();
    }
}
