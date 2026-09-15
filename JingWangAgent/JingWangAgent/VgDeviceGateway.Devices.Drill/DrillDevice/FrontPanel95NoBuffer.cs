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

using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
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
    public class FrontPanel95NoBuffer : DeviceShare<DefaultDrill>, IDrillDevice
    {
        private readonly ILogger<FrontPanel95NoBuffer> logger;
        public readonly bool clearOnOff;
        public readonly bool statisticsOnOff;
        public readonly bool mushroomOnOff;
        public readonly bool codeReaderLocalConfirmOnOff;

        public FrontPanel95NoBuffer(ILogger<FrontPanel95NoBuffer> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;

            this.clearOnOff = device.DeviceDescriptor.Extra["ClearOnOff"].ToBool();
            this.statisticsOnOff = device.DeviceDescriptor.Extra["StatisticsOnOff"].ToBool();
            this.mushroomOnOff = device.DeviceDescriptor.Extra["MushroomOnOff"].ToBool();
            device.mushroomValue = mushroomOnOff;
            codeReaderLocalConfirmOnOff = device.DeviceDescriptor.Extra["CodeReaderLocalConfirmOnOff"].ToBool();
            if (device.codeReaderOnOff)
            {
                // CodeReaderBaseSetting.CodeReaderTriggerBeforeAction += CodeReaderTriggerBefore;
                CodeReaderBaseSetting.CodeReaderReceiveAction += CodeReaderReceive;
                CodeReaderBaseSetting.CodeReaderReceiveAction += CodeInsertToDB;
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
            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  读码器 已修改【 PayloadPanels 】 {JsonSerializer.Serialize(PayloadPanels)} ");
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

        private void CodeReaderTriggerBefore(int splindleNum)
        {
            try
            {
                //清空该轴的读码器验证信息

                PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                {
                    InteractingDevice.PayloadPanels[splindleNum - 1].Barcode = "";
                    InteractingDevice.PayloadPanels[splindleNum - 1].ItemCode = "";
                })).GetAwaiter().GetResult();
            }
            catch (Exception ee)
            {
                logger.LogError($"读码器触发器前动作异常 {ee.Message}");
            }
        }

        public bool ConnectToCncAndOtherDevice()
        {
            if (InteractingDevice.cnc84Command == null || !InteractingDevice.cnc84Command.CNCCommandStatus())
            {
                CNC84Connect();
            }

            var cnc84Flag = InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus();

            var codeFlag = true;
            if (InteractingDevice.codeReaderOnOff && !CodeReaderBaseSetting.scanFlag)
            {
                codeFlag = ScanConnect();
            }

            return cnc84Flag && codeFlag;
        }

        public void CompleteScheduleLocal(ScheduledTaskStatus ScheduledStatus)
        {
            logger.LogDebug($"CompleteScheduleLocal 方法被调用了 调度状态 {ScheduledStatus}：重置{InteractingDevice.allowAllAgv}");
            InteractingDevice.allowAllAgv = true;
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

            if (clearOnOff) ObjectFactory.CreateObject<ClearJobSchedule>().StartClearJob(InteractingDevice, InteractingDevice.cancellationTokenSource.Token);
            if (statisticsOnOff) ObjectFactory.CreateObject<StatisticsJobSchedule>().StartStatisticsJob(InteractingDevice, InteractingDevice.cancellationTokenSource.Token);
            //if (InteractingDevice.codeReaderOnOff) Task.Factory.StartNew(CodeReaderThread, TaskCreationOptions.LongRunning);
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

        private async Task CodeReaderThread()
        {
            logger.LogDebug($"CodeReaderThread:开始了");
            await Task.Delay(10000);
            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    var scanCodeStart = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.ScanCodeStart");
                    //监控cnc95 状态 todo
                    WatchingProperties.SetValues(new Dictionary<string, object?>() { { "Buffer_CodeReaderTrigger", scanCodeStart } });
                    await Task.Delay(6 * 1000);
                }
                catch (Exception e)
                {
                    logger.LogError("CodeReaderThread 线程异常-->" + e.Message);
                    await Task.Delay(6 * 1000);
                }
            }
            logger.LogDebug($"CodeReaderThread 线程结束");
        }

        private async Task FastCollectDataThread()
        {
            logger.LogDebug($"FastCollectDataThread:开始了");
            await Task.Delay(2000);
            while (!InteractingDevice.cancellationTokenSource.IsCancellationRequested)
            {
                var faskDic = new Dictionary<string, object?>();
                try
                {
                    if (InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus())
                    {
                        string showText = InteractingDevice.cnc84Command.ReadCncNode<string>("ns=4;s=UI/normalized/new/ShowText");

                        faskDic.Add("Drill_ShowText", showText);

                        var drillHoleEndOriginal = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["DrilBoardEndFlag"].ToInt());//FCall
                        var str = DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool() ? "结束" : "开始";
                        logger.LogDebug($"FastCollectDataThread:FCall original->打板{str}信号 ：{drillHoleEndOriginal}");
                        var result = "1:1";
                        if (!DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool())
                        {
                            result = "1:0";
                        }
                        var drillHoleEnd = result.Equals(drillHoleEndOriginal);

                        faskDic.Add("Drill_DrillHoleEnd", drillHoleEnd);

                        //程序状态

                        var runState = InteractingDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/AutoData/AutoData.List/AutoData.RunState");
                        logger.LogDebug($"FastCollectDataThread:FCall scanCodeStart->触发读码信号 ：{runState}");
                        faskDic.Add("RunState", runState);

                        if (InteractingDevice.codeReaderOnOff)
                        {
                            var scanCodeStart = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.ScanCodeStart");
                            logger.LogDebug($"FastCollectDataThread:FCall scanCodeStart->触发读码信号 ：{scanCodeStart}");
                            faskDic.Add("Buffer_CodeReaderTrigger", scanCodeStart);
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
                    await Task.Delay(60 * 1000);
                }
                if (faskDic.Count > 0)
                {
                    WatchingProperties.SetValues(faskDic);
                }
            }
            logger.LogDebug($"FastCollectDataThread:线程结束");
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
                InteractingDevice.cnc84Command.Shutdown();
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
                InteractingDevice.cnc84Command.Stop();
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

        //钻带代理是最后执行机构 ，定位问题要从源头找起，首先要确认一下agv是否有相关的数据反馈给中控 再排查中控是否有相关数据下发
        //2024-07-27
        //异常后  解除buffer报警后 buffer会重新发起呼叫 接着上下料  直到中控下发调度完成信号。异常后  不想等中控调度 人员处理，解除buffer报警后  可以点击触摸屏上解除总的上下料信号
        //agv开始操作buffer的时候 这个信号就变成0了  中控不下发 触摸屏不点  这个信号一直是0
        public string CompleteAllEndLocal()
        {
            try
            {
                return $"强制上下料信号 执行结束";
            }
            catch (Exception ee)
            {
                return $"强制上下料结束信号发生异常：{ee.Message}";
            }
        }

        public List<Panel> InitPayloadPanels()
        {
            var panels = Panel.HasSilo.NoPanelForLayerFirst(DeviceDescriptor.DeviceId, InteractingDevice.spindleNum, 1);
            foreach (var item in panels)
            {
                item.Layer = 1;
                item.LocationCode = DeviceDescriptor.DeviceId;
                item.SiloCode = InteractingDevice.DeviceId;
            }
            return panels;
        }
        public async Task<DeviceServiceInvokeResponse> CodeReaderOnOffToCnc(DeviceServiceInvokeRequest request)
        {
            var response = DeviceServiceInvokeResponse.FAIL;
            try
            {
                logger.LogInformation($" CodeReaderOnOffToCnc request:{JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");
                if (request == null || request.Params == null)
                {
                    response.Message = "中控下发的request 为空 ";
                    logger.LogInformation($"CodeReaderOnOffToCnc COMMAND {response.Message}");
                    return response;
                }
                if (!request.Params.ContainsKey("CodeReaderOnOff") || !bool.TryParse(request.Params["CodeReaderOnOff"].ToString(), out bool codeReaderOnOff))
                {
                    response.Message = "中控下发的CodeReaderOnOff 为空";
                    logger.LogInformation($"CodeReaderOnOffToCnc COMMAND {response.Message}");
                    return response;
                }
                InteractingDevice.cnc84Command.WriteCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.ScanCode", codeReaderOnOff);

                return DeviceServiceInvokeResponse.SUCCESS;
            }
            catch (Exception ee)
            {
                response.Message = ee.Message;
                logger.LogError($"SetScannerLot 异常 {ee.Message} ");
                return response;
            }
        }
        public void CodeReaderCheck(int checkResult)
        {
            try
            {
                logger.LogDebug($"CodeReaderCheck 中控下发的验证结果 checkResult{checkResult}   ");
                InteractingDevice.cnc84Command.WriteCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.ScanCodeStart", checkResult != 0);

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
                }
                else
                {
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


    }
}
