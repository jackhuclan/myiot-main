// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/*********************************************************************************************
 *                         Suzhou Vega Technology Co., Ltd                                   *
 *********************************************************************************************
 * Copyright    : Copyright(c) Suzhou Vega Technology Co., Ltd                               *
 *                All rights reserved.                                                       *
 *                                                                                           *
 * DateTime       Author          Comment                                                    *
 * 2024.05.01    Li Haiyan        New                                                        *
 * 2024.06.01    Zhu Shipeng      Re-implementation of CNC84                                 *
 * 2024.06.16    Zhu Shipeng      Refactor the message to show                               *
 * 2024.06.18    Zhu Shipeng      Upgrade to NuGet 2.6.18.1                                  *
 *********************************************************************************************/

using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Drill.Other.CodeReader;

namespace VgDeviceGateway.Devices.Drill.EventHandler
{
    public class FrontPanel95NoBufferEventHandler : DeviceShare<DefaultDrill>, IDrillEventHandler
    {
        private readonly ILogger<FrontPanel95NoBufferEventHandler> logger;
        public DateTime DrillOprationTime = DateTime.Now;
        public static readonly string WatchFilePath = @"C:\SMWDATA\PROTOCOL\PROTO.PRO";
        public readonly bool codeReaderOnOff;
        public bool mushroomOnOff;
        public bool pressBoardOnOff;

        //public bool checkBoardDirectionOnOff;//检查板方向，开旗标让CNC95的脚本去做
        public bool selectSplineOnOff;

        public readonly int selectSplineMFunction;
        public bool drillBoardOnOff;
        public bool checkProgramAndDiaOnOff;
        public bool loadFileFromCentreOnOff;
        public bool checkToolLifeOnOff;
        public bool cnc84DiaFileOnOff;
        public bool existWhiteSpaceConfirm;
        public readonly bool codeReaderTriggerIsM;
        public readonly bool firstCheckOnOff;
        public readonly bool statisticsOnOff;
        public readonly string drillFullTableAppendContent;
        public readonly string drillHalfTableAppendContent;
        public readonly bool drillChangeAfterPathOnOff;
        public readonly bool aSplineIsWorkFullTable;
        public readonly bool validateFileAppendOnOff;
        public readonly bool existFourMushroom;

        public FrontPanel95NoBufferEventHandler(ILogger<FrontPanel95NoBufferEventHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            codeReaderOnOff = device.DeviceDescriptor.Extra["CodeReaderOnOff"].ToBool();
            mushroomOnOff = device.DeviceDescriptor.Extra["MushroomOnOff"].ToBool();
            drillBoardOnOff = device.DeviceDescriptor.Extra["DrillBoardOnOff"].ToBool();
            //checkBoardDirectionOnOff = device.DeviceDescriptor.Extra["CheckBoardDirectionOnOff"].ToBool();//检查板方向，开旗标让CNC95的脚本去做
            pressBoardOnOff = device.DeviceDescriptor.Extra["PressBoardOnOff"].ToBool();
            selectSplineOnOff = device.DeviceDescriptor.Extra["SelectSplineOnOff"].ToBool();
            loadFileFromCentreOnOff = device.DeviceDescriptor.Extra["LoadFileFromCentreOnOff"].ToBool();
            checkProgramAndDiaOnOff = device.DeviceDescriptor.Extra["CheckProgramAndDiaOnOff"].ToBool();
            checkToolLifeOnOff = device.DeviceDescriptor.Extra["CheckToolLifeOnOff"].ToBool();
            cnc84DiaFileOnOff = device.DeviceDescriptor.Extra["Cnc84DiaFileOnOff"].ToBool();
            existWhiteSpaceConfirm = device.DeviceDescriptor.Extra["ExistWhiteSpaceConfirm"].ToBool();
            selectSplineMFunction = device.DeviceDescriptor.Extra["SelectSplineMFunction"].ToInt();
            codeReaderTriggerIsM = device.DeviceDescriptor.Extra["CodeReaderTriggerIsM"].ToBool();
            firstCheckOnOff = device.DeviceDescriptor.Extra["FirstCheckOnOff"].ToBool();
            statisticsOnOff = device.DeviceDescriptor.Extra["StatisticsOnOff"].ToBool();
            drillFullTableAppendContent = device.DeviceDescriptor.Extra["DrillFullTableAppendContent"].ToStr().ToLower();
            drillHalfTableAppendContent = device.DeviceDescriptor.Extra["DrillHalfTableAppendContent"].ToStr().ToLower();
            drillChangeAfterPathOnOff = device.DeviceDescriptor.Extra["DrillChangeAfterPathOnOff"].ToBool();
            aSplineIsWorkFullTable = device.DeviceDescriptor.Extra["ASplineIsWorkFullTable"].ToBool();
            validateFileAppendOnOff = device.DeviceDescriptor.Extra["ValidateFileAppendOnOff"].ToBool();
            existFourMushroom = device.DeviceDescriptor.Extra["ExistFourMushroom"].ToBool();

        }

        private async Task ChangeIsFirstProperty(bool isFirst)
        {
            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  首件检测 未修改【 PayloadPanels 】 {JsonSerializer.Serialize(PayloadPanels)} ");
            await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
             {
                 for (int i = InteractingDevice.spindleNum; i < 2 * InteractingDevice.spindleNum; i++)
                 {
                     if (InteractingDevice.PayloadPanels[i]!.ProductStatus == ProductStatus.Drilling)
                     {
                         InteractingDevice.PayloadPanels[i].IsFirst = isFirst;
                     }
                     else
                     {
                         InteractingDevice.PayloadPanels[i].IsFirst = false;
                     }
                 }
             }));

            await PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.DeviceId);
            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  首件检测 已修改【 PayloadPanels 】 {JsonSerializer.Serialize(PayloadPanels)} ");
        }

        private void OpenOrCloseWarningDialog(bool isOpen = true)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other\\WindowsFormsApp1.exe");
                string processName = Path.GetFileNameWithoutExtension(filePath);
                Process[] myproc = Process.GetProcessesByName(processName);
                if (myproc.Length != 0)
                {
                    myproc.ToList().ForEach(s => s.Kill());
                }
                if (isOpen)
                {
                    logger.LogDebug("弹窗提示退出，开始重新激活程序,程序路径:{0}", filePath);
                    ProcessStartInfo info = new ProcessStartInfo
                    {
                        WorkingDirectory = Path.GetDirectoryName(filePath),
                        FileName = filePath,
                        UseShellExecute = true
                    };
                    Process.Start(info);
                }
            }
            catch (Exception ee)
            {
                logger.LogError("弹窗提示 异常:{0}", ee.Message);
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
        public void AddWatchingEvents()
        {
            //
            WatchingProperties.Property("Drill_Tody")
                 .PostCondition(p => p.IsValueChanged)
                 .TriggerAlways(() =>
                 {
                     DefaultDrill.CollectCleanTime = 0;
                     var startTime = DateTime.Now;
                     DefaultDrill.CollectCleanStartTime = DateTime.Now;
                     DefaultDrill.CollectCleanEndTime = DateTime.Now;
                 });
            WatchingProperties.Property("Drill_ShowText")
                  .PostCondition(p => p.IsValueChanged && !string.IsNullOrWhiteSpace(p.NewValue.ToStr()))
                  .TriggerAlways(async () =>
                  {
                      var showText = WatchingProperties.Property("Drill_ShowText");
                      logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Drill_ShowText   变化旧的程序 {showText.OldValue.ToStr()}  新的程序 {showText.NewValue.ToStr()} ");
                      if (showText.NewValue.ToStr().Contains("Collet clean begin"))
                      {
                          var startTime = DateTime.Now;
                          DefaultDrill.CollectCleanStartTime = startTime;
                          DefaultDrill.CollectCleanEndTime = startTime;
                          WatchingProperties.Property("Drill_CollectCleanBegin").SetValue(startTime.ToString());
                          WatchingProperties.Property("Drill_CollectCleanEnd").SetValue(startTime.ToString());
                      }
                      else if (showText.NewValue.ToStr().Contains("Collet clean end"))
                      {
                          try
                          {
                              var startTime = DefaultDrill.CollectCleanStartTime;
                              var endTime = DateTime.Now;
                              DefaultDrill.CollectCleanEndTime = endTime;
                              WatchingProperties.Property("Drill_CollectCleanEnd").SetValue(endTime.ToString());
                              DefaultDrill.CollectCleanTime += (float)((endTime - startTime).TotalMinutes);
                          }
                          catch (Exception e)
                          {
                              logger.LogError($"{DateTime.Now.ToShortTimeString()}  CollectCleanTime 异常  {e.Message} ");


                          }

                      }
                  });


            ////WatchingProperties.Property("Drill_CurrentProgramFile")
            ////  .PostCondition(p => p.IsValueChanged && !string.IsNullOrWhiteSpace(p.NewValue.ToStr()))
            ////  .TriggerAlways(async () =>
            ////  {
            ////      var currentProgramFile = WatchingProperties.Property("Drill_CurrentProgramFile");
            ////      logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  首件检测  开启{firstCheckOnOff} 程序发生变化旧的程序 {currentProgramFile.OldValue.ToStr()}  新的程序 {currentProgramFile.NewValue.ToStr()} ");

            ////      if (firstCheckOnOff)
            ////      {
            ////          logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  首件检测  检测结果清空 置上首件检测 ");
            ////          InteractingDevice.cnc84Command.WriteCncNode<bool>($"ns=4;s=UI/normalized/custom/{InteractingDevice.DeviceDescriptor.Extra["FirstCheckResult"].ToStr()}", false);
            ////          InteractingDevice.cnc84Command.WriteCncNode<bool>($"ns=4;s=UI/normalized/custom/{InteractingDevice.DeviceDescriptor.Extra["FirstCheckFlag"].ToStr()}", true);
            ////      }
            ////  });

            if (firstCheckOnOff)
            {
                WatchingProperties.Property("Drill_FirstCheckResult")
                .PostCondition(p => p.IsValueChanged && p.NewValue.ToBool())
                .TriggerAlways(async () =>
                 {
                     try
                     {
                         var firstCheckResult = WatchingProperties.Property("Drill_FirstCheckResult").NewValue.ToBool();
                         logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  首件检测  结果 {firstCheckResult} ");

                         if (DeviceDescriptor.AutoMode)
                         {
                             //更新payloadpanel；
                             await ChangeIsFirstProperty(firstCheckResult);
                         }

                         InteractingDevice.cnc84Command.WriteCncNode<bool>($"ns=4;s=UI/normalized/custom/{InteractingDevice.DeviceDescriptor.Extra["FirstCheckFlag"].ToStr()}", false);
                         InteractingDevice.cnc84Command.WriteCncNode<bool>($"ns=4;s=UI/normalized/custom/{InteractingDevice.DeviceDescriptor.Extra["FirstCheckResult"].ToStr()}", false);
                     }
                     catch (Exception ee)
                     {
                         logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  首件检测  异常{ee.Message} ");
                     }
                 });
            }

            WatchingProperties.Property("Buffer_CodeReaderTrigger")
                .PostCondition(p => p.IsValueChanged && p.NewValue.ToBool())
                .TriggerAlways(async () =>
                {
                    if (InteractingDevice.codeReaderOnOff)
                    {

                        CodeReaderBaseSetting.receiveDataInf=new string[InteractingDevice.spindleNum];
                        PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                        {
                            for (int i = 0; i < InteractingDevice.PayloadPanels.Count; i++)
                            {
                                var panel = Panel.HasSilo.NoPanelForSingleSpindle("", i + 1, 1, 1)[0];
                                panel.LocationCode = InteractingDevice.DeviceId;
                                panel.SiloCode = InteractingDevice.DeviceId;
                                InteractingDevice.PayloadPanels[i] = panel;
                            }
                        })).GetAwaiter().GetResult();

                        for (int i = 0; i < InteractingDevice.spindleNum; i++)
                        {
                            _ = Task.Factory.StartNew((index) =>
                            {
                                try
                                {
                                    int codeIndex = (int)index;
                                    logger.LogInformation($"轴{codeIndex + 1} 触发读码 线程开始");
                                    InteractingDevice.codeReaderBaseSetting.SendScanMesssageToSignal(codeIndex, "1", true);
                                    logger.LogInformation($"轴{codeIndex + 1} 触发读码 线程结束");
                                }
                                catch (Exception ee)
                                {

                                    logger.LogError($"触发读码 异常 {ee.Message}");
                                }
                              
                            }, i);
                        }
                        _ = Task.Factory.StartNew(() =>
                        {
                           var getAllBarcode= WaitUntil(() => CodeReaderBaseSetting.receiveDataInf.All(s => !string.IsNullOrEmpty(s)), TimeSpan.FromMilliseconds(InteractingDevice.DeviceDescriptor.Extra["CodeReaderTimeout"].ToInt()));
                            logger.LogInformation($"是否获取全部的二维码信息 {getAllBarcode}");
                        });
                    }
                });

            //WatchingProperties.Property("Buffer_CallAgvMessage")
            //.PostCondition(p => !p.NewValue.ToBool())
            //.TriggerAlways(async () =>
            //{
            //    bool resetCallAgvFlag = await QueryScheduleResetCallAgv();
            //    logger.LogDebug($"QueryScheduleResetCallAgv 方法被调用了：重置 {resetCallAgvFlag} 信号");
            //    if (resetCallAgvFlag)
            //    {
            //        logger.LogDebug($"QueryScheduleResetCallAgv 方法被调用了：重置 InteractingDevice.allowAllAgv 信号");
            //        InteractingDevice.allowAllAgv = true;
            //    }
            //});

            WatchingProperties.Property("NewTranscationId")
            .PostCondition(p => !p.NewValue.ToBool() && p.IsValueChanged)
            .TriggerAlways(() =>
            {
                logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  NewTranscationId  发生变化前 {WatchingProperties.Property("NewTranscationId").OldValue}  发生变化后{WatchingProperties.Property("NewTranscationId").NewValue}");
            });
            WatchingProperties.Property("TranscationId")
              .PostCondition(p => !p.NewValue.ToBool() && p.IsValueChanged)
              .TriggerAlways(() =>
              {
                  var transcationIdProperties = WatchingProperties.Property("TranscationId");
                  var newValue = transcationIdProperties.NewValue.ToStr();
                  var oldValue = transcationIdProperties.OldValue.ToStr();
                  var newValueContentArray = newValue.Split("----", StringSplitOptions.RemoveEmptyEntries);
                  var oldValueContentArray = oldValue.Split("----", StringSplitOptions.RemoveEmptyEntries);
                  if (newValueContentArray.Length == 2)
                  {
                      var newContent = newValueContentArray[1];
                      if (string.IsNullOrWhiteSpace(oldValue))
                      {
                          if (newContent.Contains($"没有安排生产任务"))
                          {
                              OpenOrCloseWarningDialog();
                              return;
                          }
                      }
                      else if (oldValueContentArray.Length == 2)
                      {
                          var oldContent = oldValueContentArray[1];
                          if (!string.Equals(newContent, oldContent) && newContent.Contains($"没有安排生产任务"))
                          {
                              OpenOrCloseWarningDialog();
                              return;
                          }
                      }
                      if (newContent.Contains("呼叫成功"))
                      {
                          OpenOrCloseWarningDialog(false);
                      }
                  }

                  logger.LogDebug($"{DateTime.Now.ToShortTimeString()} 呼叫agv TranscationId  发生变化前 {WatchingProperties.Property("TranscationId").OldValue}  发生变化后{WatchingProperties.Property("TranscationId").NewValue}");
                  //
              });
            WatchingProperties.Property("MqttConnected")
               .PostCondition(p => !p.NewValue.ToBool() && p.IsValueChanged)
               .TriggerAlways(() =>
               {
                   logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  mqtt连接断开重置呼叫按钮 ");
                   InteractingDevice.allowAllAgv = true;
               });

            WatchingProperties.Property("Drill_Halt")
                .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    if (DeviceDescriptor.AutoMode)
                    {
                        await DataExporter.DeviceEventReport(
                        new DeviceEventReportRequest()
                        {
                            ProductId = DeviceDescriptor.ProductId,
                            DeviceId = DeviceDescriptor.DeviceId,
                            ClientId = InteractingDevice.ClientId,
                            EventId = Events.Drill.DRILL_HALT_EVENT,
                            EventName = Events.Drill.DRILL_HALT_EVENT_NAME,
                            RequestDeviceKind = DeviceKind.CNC95Drill,
                            PayloadPanels = InteractingDevice.PayloadPanels
                        });
                    }
                });

            //钻孔打板结束信号
            WatchingProperties.Property("Drill_DrillHoleEnd")
                .PostCondition(p => p.IsValueChanged)
                .TriggerAlways(async () =>
                {

                    var holeEnd = WatchingProperties.Property("Drill_DrillHoleEnd").NewValue.ToBool();
                    logger.LogWarning($"Drill_DrillHoleEnd->钻孔结束信号-发生变化-> {holeEnd} ");
                    try
                    {
                        if (holeEnd)
                        {   //打板结束没有退板直接上板了  手动上的时候要用力气撞一下销钉  要不然 传感器感应不到板子
                            //下料信号是啥时候给plc的:plc自己判断的  钻机代理只给p4泊车位和钻孔结束信号
                            //判断钻机是否有板子
                            // 打板结束 更新板材信息 finishDrill

                            InteractingDevice.OldTransactionId = InteractingDevice.DrillTransactionId;
                            InteractingDevice.DrillTransactionId = "";
                            await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                            {
                                foreach (var item in InteractingDevice.PayloadPanels)
                                {
                                    if (item.ProductStatus == ProductStatus.Drilling)
                                    {
                                        item.ProductStatus = ProductStatus.Finished_DRILL;
                                    }
                                }
                            }));


                            if (DeviceDescriptor.AutoMode && !string.IsNullOrWhiteSpace(InteractingDevice.OldTransactionId))
                            {
                                var result = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<FinishTaskRequest, FinishTaskResponse>(InteractingDevice.CentralWebOptions.FinishTask,
                                 new FinishTaskRequest()
                                 {
                                     ProductId = DeviceDescriptor.ProductId,
                                     DeviceId = DeviceDescriptor.DeviceId,
                                     TraceId = InteractingDevice.OldTransactionId,
                                 });
                                logger.LogDebug($"上报TransID结束信号返回值：  {JsonSerializer.Serialize(result)}  ");
                            }
                        }
                        else
                        {
                            // // 打板开始 更新板材信息 drilling
                            InteractingDevice.DrillTransactionId = InteractingDevice.NewTranscationId;
                            InteractingDevice.NewTranscationId = "";

                            await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                            {
                                foreach (var item in InteractingDevice.PayloadPanels)
                                {
                                    if (item.ProductStatus == ProductStatus.WaitingForDrill)
                                    {
                                        item.ProductStatus = ProductStatus.Drilling;
                                    }
                                }
                            }));

                        }
                    }
                    catch (Exception)
                    {
                        await Task.CompletedTask;
                    }
                    await Task.CompletedTask;
                });

            WatchingProperties.Property("Drill_RealCodeMessage")
                .PostCondition(p => p.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    try
                    {
                        InteractingDevice.LoadFileFlag = false;
                        InteractingDevice.cnc84Command.SetCncComand("NOBSIZ");
                        var codeInfo = WatchingProperties.Property("Drill_RealCodeMessage").NewValue.ToStr();
                        logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  机器加载的条码{codeInfo}");
                        var drillPath = string.Empty;
                        var diaPath = string.Empty;
                        var Cnc84DiaFileOnOff = DeviceDescriptor.Extra["Cnc84DiaFileOnOff"].ToBool();
                        if (DeviceDescriptor.AutoMode)
                        {
                            var response = await DataExporter.DeviceEventReport(
                                new DeviceEventReportRequest()
                                {
                                    ProductId = DeviceDescriptor.ProductId,
                                    DeviceId = DeviceDescriptor.DeviceId,
                                    ClientId = InteractingDevice.ClientId,
                                    EventId = Events.Drill.DRILL_REQUEST_RECIPE_EVENT,
                                    EventName = Events.Drill.DRILL_REQUEST_RECIPE_EVENT_NAME,
                                    RequestDeviceKind = DeviceKind.CNC95Drill,
                                    Params = new Dictionary<string, object?>() { { "IncodeNumber", codeInfo } },
                                    PayloadPanels = InteractingDevice.PayloadPanels
                                });
                            if (response == null)
                            {
                                logger.LogDebug(response?.Message);

                                return;
                            }
                            else if (response?.Code != ErrorCodes.Sys.SUCCESS)
                            {
                                logger.LogDebug(response?.Message);

                                return;
                            }

                            if (Cnc84DiaFileOnOff)
                            {
                                if (response.Params == null || !response.Params.ContainsKey("DrlPath") || !response.Params.ContainsKey("DiaPath"))
                                {
                                    logger.LogError("未下发配方路径");

                                    return;
                                }
                            }
                            else
                            {
                                if (response.Params == null || !response.Params.ContainsKey("DrlPath"))
                                {
                                    logger.LogError("未下发配方路径");

                                    return;
                                }
                            }
                            //加载文件
                            drillPath = response.Params["DrlPath"].ToStr();
                            diaPath = response.Params["DiaPath"].ToStr();
                        }
                        else
                        {
                            var basePath = Path.Combine(AppContext.BaseDirectory, "data");
                            drillPath = Path.Combine(basePath, codeInfo + $".{DeviceDescriptor.Extra["DrlSearchFileExt"]}");
                            if (Cnc84DiaFileOnOff)
                            {
                                diaPath = Path.Combine(basePath, codeInfo + $".{DeviceDescriptor.Extra["DiaSearchFileExt"]}");
                            }
                        }
                        if (!CheckDrlDiaFile(drillPath, diaPath))
                        {
                            logger.LogError("配方路径文件不存在");
                            return;
                        }
                        if (!LoadFile(drillPath, diaPath))
                        {
                            return;
                        }
                        else
                        {
                            var boardLengthInfOnPlc = WatchingProperties.Property("Buffer_BoardLengthInfOnPlc").NewValue.ToFloat();
                            //获取参数
                            var realLength = GetBSIZLength(boardLengthInfOnPlc);
                            InteractingDevice.cnc84Command.SetCncComand($"BSIZ{realLength.ToString("0.000")}");

                            //设置小板用户标记
                            if (InteractingDevice.middleMushroomExist)
                            {
                                double smallBoardMaxLength = DeviceDescriptor.Extra["SmallBoardMaxLength"].ToDouble();

                                if (realLength < smallBoardMaxLength)
                                {   //opcUaClient.WriteNode<Boolean>("ns=4;s=UI/normalized/custom/UF.SmallBoard", true);
                                    InteractingDevice.cnc84Command.SetUserFlag(1, DeviceDescriptor.Extra["SmallBoardUserFlag"].ToInt());
                                    logger.LogDebug($"设置小板用户标记 ");
                                }
                                else
                                {   //opcUaClient.WriteNode<Boolean>("ns=4;s=UI/normalized/custom/UF.SmallBoard", false);
                                    InteractingDevice.cnc84Command.SetUserFlag(0, DeviceDescriptor.Extra["SmallBoardUserFlag"].ToInt());
                                    logger.LogDebug($"取消小板用户标记 ");
                                }
                            }
                            logger.LogDebug($"扫码枪验证文件后加载文件 程序成功加载标识");
                        }
                        InteractingDevice.LoadFileFlag = true;
                    }
                    catch (Exception)
                    {
                        await Task.CompletedTask;
                    }

                    await Task.CompletedTask;
                });

            WatchingProperties.Property("Drill_CodeMessage")
                .PostCondition(p => p.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    try
                    {
                        var holeEnd = WatchingProperties.Property("Drill_DrillHoleEnd").NewValue.ToBool();
                        var upPosition = WatchingProperties.Property("Buffer_DrillLockFlag").NewValue.ToBool();
                        var tmpCode = WatchingProperties.Property("Drill_CodeMessage").NewValue.ToStr();
                        var realCodeMessage = WatchingProperties.Property("Drill_RealCodeMessage").NewValue.ToStr();
                        logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  外部系统传入的条码 {tmpCode} 钻机结束信号{holeEnd} buffer在AGV对接层 {upPosition} 机器的条码是{realCodeMessage} ");
                        if (holeEnd && upPosition && !realCodeMessage.Equals(tmpCode))
                        {
                            WatchingProperties.Property("Drill_RealCodeMessage").SetValue(tmpCode);
                        }
                        else
                        {
                            logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  条码不满足需求 不能使用");
                            ushort errrorCode = 0;
                            if (!holeEnd) errrorCode = 303;
                            if (!upPosition) errrorCode = 304;
                            if (realCodeMessage.Equals(tmpCode)) errrorCode = 305;

                        }
                        if (DeviceDescriptor.AutoMode)
                        {
                            await DataExporter.DeviceEventReport(
                            new DeviceEventReportRequest()
                            {
                                ProductId = DeviceDescriptor.ProductId,
                                DeviceId = DeviceDescriptor.DeviceId,
                                ClientId = InteractingDevice.ClientId,
                                EventId = Events.Drill.DRILL_SCAN_CODE_EVENT,
                                EventName = Events.Drill.DRILL_SCAN_CODE_EVENT_NAME,
                                RequestDeviceKind = DeviceKind.CNC95Drill,
                                Params = new Dictionary<string, object?>()
                                {
                                    { "Drill_RealCodeMessage",  InteractingDevice.CodeMessage},
                                }
                            });
                        }
                    }
                    catch (Exception)
                    {
                        await Task.CompletedTask;
                    }
                    //发送取消事件
                    await Task.CompletedTask;
                });




            //RunState ==2  //Drill_DrillHoleEnd 结束
            WatchingProperties.Properties("RunState", "Drill_DrillHoleEnd")
              .When(p =>
              {
                  var runState = p.Property("RunState");
                  var holeEnd = p.Property("Drill_DrillHoleEnd");

                  bool result = (runState.NewValue.ToInt() == 2) && holeEnd.NewValue.ToBool() && InteractingDevice.allowAllAgv && DeviceDescriptor.AutoMode && InteractingDevice.MqttClientWrapper.IsConnected;
                  if (result)
                  {
                      InteractingDevice.CallCondition = $"{DateTime.Now.ToString()}:  满足条件能发起呼叫 生料个数{PayloadPanels.Count(s => s.ProductStatus == ProductStatus.WaitingForDrill)}  熟料个数 {PayloadPanels.Count(s => s.ProductStatus == ProductStatus.Finished_DRILL)}";
                  }
                  else
                  {
                      List<string> message = new List<string>();

                      if (!InteractingDevice.allowAllAgv) message.Add("呼叫已经成功发起不能再次呼叫");
                      if (!(runState.NewValue.ToInt() == 2)) message.Add("runState 不是2 不能再次呼叫");
                      if (!holeEnd.NewValue.ToBool()) message.Add("钻孔没结束 下不能呼叫");
                      if (!DeviceDescriptor.AutoMode) message.Add("设备配置是手动 不能呼叫");
                      if (!InteractingDevice.MqttClientWrapper.IsConnected) message.Add("Mqtt断开连接 不能呼叫");
                      InteractingDevice.CallCondition = $"{DateTime.Now.ToString()}: 不满足条件不能发起呼叫： {string.Join(",", message)} ";
                  }

                  return result;
              })
              .TriggerAlways(async () =>
              {
                  try
                  {
                      if (DeviceDescriptor.AutoMode)
                      {
                          string message = await CallAgv();
                          if (message != null && "小于呼叫间隔不能呼叫".Equals(message))
                          {
                              InteractingDevice.callTimeResult = $"{DateTime.Now.ToString()}:{message}";
                          }
                          else
                          {
                              InteractingDevice.callResult = $"{DateTime.Now.ToString()}:{message}";
                              InteractingDevice.callTimeResult = "";
                          }

                          logger.LogDebug($"buffer自动呼叫AGV ： {message} ");
                      }
                  }
                  catch (Exception ee)
                  {
                      logger.LogError($"buffer呼叫AGV异常 ： {ee.Message} ");
                      InteractingDevice.callResult = $"{DateTime.Now.ToString()}:{ee.Message}";
                      await Task.CompletedTask;
                  }
              });


       
        }
        public static bool WaitUntil(
        Func<bool> condition,
        TimeSpan timeout,
        TimeSpan checkInterval = default)
        {
            if (checkInterval == default) checkInterval = TimeSpan.FromMilliseconds(100);

            var sw = System.Diagnostics.Stopwatch.StartNew();
            while (!condition())
            {
                if (sw.Elapsed >= timeout) return false;
                Thread.Sleep(checkInterval);
            }
            return true;
        }

        [DllImport("user32.dll")]
        public static extern void keybd_event(int bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);

        //用来遍历所有窗口
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        //获取窗口Text
        [DllImport("user32.dll")]
        private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        //获取窗口类名
        [DllImport("user32.dll")]
        private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        public struct WindowInfo
        {
            public IntPtr hWnd;
            public string szWindowName;
            public string szClassName;
        }

        public WindowInfo[] GetAllDesktopWindows()
        {
            //用来保存窗口对象 列表
            List<WindowInfo> wndList = new List<WindowInfo>();

            //enum all desktop windows
            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);

                //get hwnd
                wnd.hWnd = hWnd;

                //get window name
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();

                //get window class
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();

                //add it into list
                wndList.Add(wnd);
                return true;
            }, 0);

            return wndList.ToArray();
        }

        private bool OpenCNCWindow()
        {
            string windowTitle = "TSMCNCFrame"; // 替换为要查找的窗口标题
            var allWindow = GetAllDesktopWindows();
            foreach (var window in allWindow)
            {
                Console.WriteLine($"{window.hWnd}--- {window.szWindowName} ==== {window.szClassName}");
            }
            var item1 = allWindow.Where(s => windowTitle.Contains(s.szClassName));
            if (item1 == null || item1.ToList().Count <= 0)
            {
                Console.WriteLine("未找到指定的窗口");
                return false;
            }
            foreach (var item2 in item1.ToList())
            {
                Console.WriteLine($"查询到的界面 {item2.hWnd}--- {item2.szWindowName} ==== {item2.szClassName}");
            }

            IntPtr hWnd = item1.ToList()[0].hWnd;
            IntPtr hWnd1 = GetForegroundWindow();
            if (hWnd1 != hWnd)
            {
                ShowWindow(hWnd, 3);
                Thread.Sleep(1000);
                SetForegroundWindow(hWnd);
                Thread.Sleep(1000);
                // PostMessage(hWnd, 'y', 0, 0);
                //keybd_event(vbKeyY, 0, 0, 0);

                // SendKeys.Send("Y");
                //keybd_event(vbKeyY, 0, 2, 0);
                return true;
            }
            return true;
        }

    
      
   
        private void RestartCnc()
        {
            InteractingDevice.cnc84Command.Start();
            InteractingDevice.cnc84Command.SetCncComand($"DSP,IIoT_Send_Start_RPC_START");
            logger.LogDebug($" RestartCnc 发送开始指令 ");
            Thread.Sleep(2000);
            InteractingDevice.cnc84Command.SetCncComand($"DSP,");
            Thread.Sleep(1000);
        }

        private bool CheckStartStatus()
        {
            try
            {
                //FCall FCall才是打板结束/ATRUN决定了能不能开始加工
                //获取打板开始指令Boolean input = opcUaClient.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.FCall");
                string endFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["DrilBoardEndFlag"].ToInt());
                string result = "1:0";
                if (!DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool())
                {
                    result = "1:1";
                }

                InteractingDevice.cnc84Command.SetCncComand($"DSP,Read_Customer_FCall_ORIGINAL_" + endFlag);
                logger.LogDebug($"启动过程中检测： 钻孔结束信号 {DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool()}：{endFlag}");
                var drillHoleStart = result.Equals(endFlag);
                logger.LogDebug($"启动过程中检测： 钻孔开始信号：{drillHoleStart}");
                return drillHoleStart;
            }
            catch (Exception ee)
            {
                logger.LogDebug($"启动过程中检测： 钻孔开始信号：{ee.Message}");
                return false;
            }
        }

        private bool CheckStartStatusAndRestart()
        {
            try
            {
                if (CheckStartStatus())
                {
                    return true;
                }
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
                logger.LogDebug($"CheckStartStatusAndRestart 异常{ee.Message}");
                return false;
            }
        }

        public bool StartPressBoard()
        {
            var toolNum = GetToolNum();
            if (!"T0".Equals(toolNum))
            {
                Thread.Sleep(3000);
                //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", command);
                InteractingDevice.cnc84Command.SetCncComand("T");
                Thread.Sleep(1000);
                InteractingDevice.cnc84Command.SetCncComand($"DSP,CNC_Command_T");
            }

            if (!"T0".Equals(toolNum) && !ValidateRetractToolEnd())
            {
                return false;
            }

            Thread.Sleep(100);
            PressBoard();
            Thread.Sleep(300);
            if (!ValidatePressBoardEnd())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// opcUaClient.ReadNode<String>("ns=4;s=UI/origin/ToolParameter/ToolCncToolsTable/CncTools");
        /// splitStr.FirstOrDefault(item => item.StartsWith("T", StringComparison.OrdinalIgnoreCase));
        /// </summary>
        /// <returns>ToolNumber</returns>
        private string GetToolNum()
        {
            return InteractingDevice.cnc84Command.GetToolParameter().ToolNumber;
        }

        private bool ValidateRetractToolEnd()
        {
            Thread.Sleep(2000);
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
                    Thread.Sleep(2000);
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["RetractToolEndTimeout"].ToLong());

            stopwatch.Stop();
            return false;
        }

        private void PressBoard()
        {
            //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERTWOPINspindleselect");
            //SCRP,scCncCUSTOMERTWOPINspindleselect
            InteractingDevice.cnc84Command.SetCncComand($"DSP,Press_Board_M44");
            Thread.Sleep(300);
            InteractingDevice.cnc84Command.SendCncComand("SCRP,scCncCUSTOMERTWOPINspindleselect");//M102,压板 CNC84 M102;CNC95 M44
            Thread.Sleep(30000);//等待25秒
            InteractingDevice.cnc84Command.SetCncComand($"DSP,");
            Thread.Sleep(300);
        }

    
        private bool ValidatePressBoardEnd()
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                do
                {
                    //opcUaClient.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/SF.BoardOver");//压板结束
                    var seqFlag = InteractingDevice.cnc84Command.GetSeqFlag(DeviceDescriptor.Extra["PressBoardEndFlagOnCNC84"].ToInt());
                    Thread.Sleep(300);
                    InteractingDevice.cnc84Command.SetCncComand($"DSP,Read_SF_Press_Board_Over_" + seqFlag);
                    Thread.Sleep(2000);
                    if ("1".Equals(seqFlag, StringComparison.CurrentCultureIgnoreCase))
                    {
                        stopwatch.Stop();
                        return true;
                    }
                } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidatePressBoardEndTimeout"].ToLong());//90秒
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
                        logger.LogDebug($"发送Start指令 耗时{stopwatch.ElapsedMilliseconds} 毫秒");
                        return true;
                    }
                } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateStartTimeout"].ToLong());
                logger.LogDebug($"发送Start指令 超过耗时{stopwatch.ElapsedMilliseconds} 毫秒");
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return false;
        }

        public void StartDrillBoard()
        {
            logger.LogWarning("StartDrilling开始切换界面");
            StartChangeF8("StartDrilling");
            logger.LogWarning($" 发送开始指令 ");
            InteractingDevice.cnc84Command.SetCncComand($"DSP,Auto_Start_Drilling_The_Board");
            Thread.Sleep(300);
            InteractingDevice.cnc84Command.Start();
            Thread.Sleep(9000);//等待9秒，让机器运行一下再去判断是否启动。
            InteractingDevice.cnc84Command.SetCncComand($"DSP,Drilling");
            int i = 1;
            Int32 colorNum = InteractingDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/RosiInfo/BlockBackgroundColor");
            while (colorNum != 7925624)
            {
                InteractingDevice.cnc84Command.StartTwo();
                Thread.Sleep(2000);
                colorNum = InteractingDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/RosiInfo/BlockBackgroundColor");
                logger.LogWarning($"开始打板调用第{i}次");
                i++;
                if (i == 3)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// //模拟按下F8键
        /// keybd_event(vbKeyF8, 0, 0, 0);
        /// //松开按键F8
        /// keybd_event(vbKeyF8, 0, 2, 0);
        /// </summary>
        private void StartChangeF8(string sMemo)
        {
            InteractingDevice.cnc84Command.SetCncComand($"DSP,keybd_event_F8_" + sMemo);
            Thread.Sleep(1000);
            InteractingDevice.cnc84Command.SetChangePage("WORK_WORK");
            Thread.Sleep(1000);
            InteractingDevice.cnc84Command.SetCncComand($"DSP,");
        }
        public bool StartOpenMushroomControllerNew()
        {
            return existFourMushroom ? OpenFourMushroomController() : StartOpenMushroomController();
        }

        public bool OpenFourMushroomController()
        {
            //验证初始状态
            if (ValidateInitMushroomState())
            {
                //调脚本
                int boardSize = SendScript();
                logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 蘑菇头 判定的板子 1小板 2中板 3大板 实际是 {boardSize}");
                return ValidateMushroomEndState(boardSize);
            }

            //验证结束
            return false;
        }
        private bool ValidateInitMushroomState()
        {
            var z1State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1");
            var z2State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2");
            var z3State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z3");

            logger.LogWarning($"机器【{DeviceDescriptor.DeviceName} 】蘑菇头 初始 z1状态{z1State}   z2状态{z2State} z3状态{z3State}");
            if (!z1State || !z2State || !z3State)
            {
                return false;
            }
            return true;
        }

        private int SendScript()
        {
            var smallBoard = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/UF.SmallBoard");
            var middleBoard = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/UF.MiddleBoard");
            logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】验证蘑菇头   获取小板用户标记 {smallBoard}  获取中板用户标记 {middleBoard}");
            if (smallBoard && !middleBoard)
            {
                //小板
                ControlMiddleMushroom("Open");
                return 1;
            }
            else if (!smallBoard && middleBoard)
            {
                //中板
                ControlThirdMushroom("Open");
                return 2;
            }
            else
            {
                //大板
                ControlFrontBackMushroom("Open");
                return 3;
            }
        }
        private bool ValidateMushroomEndState(int boardSize)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                Thread.Sleep(1000);
                var z1State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1");
                var z2State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2");
                var z3State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z3");
                logger.LogWarning($"机器【{DeviceDescriptor.DeviceName} 】验证蘑菇头 z1状态{z1State}   z2状态{z2State} z3状态{z3State}");
                if (boardSize == 3 && !z1State && z2State && z3State)
                {
                    logger.LogWarning("大板 前后蘑菇头结束，请等待...");
                    stopwatch.Stop();
                    logger.LogWarning($"机器【{DeviceDescriptor.DeviceName} 】大板验证 前后蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
                    return true;
                }
                else if (boardSize == 2 && !z1State && z2State && !z3State)
                {
                    logger.LogWarning("中板 前后 中后蘑菇头结束，请等待...");
                    stopwatch.Stop();
                    logger.LogWarning($"机器【{DeviceDescriptor.DeviceName} 】中板验证 前后 中后蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
                    return true;
                }
                else if (boardSize == 1 && !z1State && !z2State && z3State)
                {
                    logger.LogWarning("小板 前后 中前蘑菇头结束，请等待...");
                    stopwatch.Stop();
                    logger.LogWarning($"机器【{DeviceDescriptor.DeviceName} 】小板验证 前后 中前蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
                    return true;
                }

            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateMushroomEndTimeOut"].ToLong());//90秒
            return false;
        }
        public bool StartOpenMushroomController()
        {
            if (InteractingDevice.middleMushroomExist)
            {
                //获取用户标记
                //opcUaClient.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/UF.SmallBoard");//小板
                string result = InteractingDevice.cnc84Command.GetUserFlag(DeviceDescriptor.Extra["SmallBoardUserFlag"].ToInt()).ToStr();
                if ("1".Equals(result, StringComparison.CurrentCultureIgnoreCase))
                {
                    logger.LogWarning("有中间蘑菇头  当前板子是小板 中间蘑菇需要打开！");
                    if (!StartOpenMiddleMushroom(true)) return false;
                }
                else
                {
                    logger.LogWarning("有中间蘑菇头  当前板子是大板 中间蘑菇不需要打开！");
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
                //判断当前蘑菇头位置true未锁，false升起锁定 CbdOpen_Z2=Middle CbdOpen_Z1=FrontBack
                if ("前后蘑菇头已开" == MushroomFrontBackCloseLocaltion() || "中间蘑菇头已开" == MushroomMiddleCloseLocaltion())
                {
                    InteractingDevice.cnc84Command.SetCncComand($"DSP,SMALL_Z1_Z2_OPENED_RETRUN_FALSE");
                    logger.LogWarning("当前 前后蘑菇头 或者中间蘑菇头 已经打开，无法进行操作！");
                    return false;
                }
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                do
                {
                    //发送打开蘑菇头指令
                    ControlMiddleMushroom("Open");
                    Thread.Sleep(1000);
                    //验证蘑菇头是否打开结束CbdOpen_Z1FrontBack=CbdOpen_Z2Middle=false=已开
                    if ("前后蘑菇头已开" == MushroomFrontBackCloseLocaltion() && "中间蘑菇头已开" == MushroomMiddleCloseLocaltion())
                    {
                        logger.LogWarning("前后中蘑菇头结束，请等待...");
                        stopwatch.Stop();
                        logger.LogWarning($"机器【{DeviceDescriptor.DeviceName} 】验证前后中蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
                        return true;
                    }
                } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateMushroomEndTimeOut"].ToLong());//90秒
            }
            else
            {
                //判断当前蘑菇头位置true未锁，false升起锁定 CbdOpen_Z2=Middle CbdOpen_Z1=FrontBack
                if ("前后蘑菇头已开" == MushroomFrontBackCloseLocaltion())
                {
                    InteractingDevice.cnc84Command.SetCncComand($"DSP,BIG_Z1_Opened_Return_False");
                    logger.LogWarning("当前 前后蘑菇头已经打开，无法进行操作！");
                    return false;
                }
                logger.LogWarning($"大板的时候 是否存在中间蘑菇头 {InteractingDevice.middleMushroomExist}");
                if (InteractingDevice.middleMushroomExist && "中间蘑菇头已开" == MushroomMiddleCloseLocaltion())
                {
                    InteractingDevice.cnc84Command.SetCncComand($"DSP,BIG_Z2_Opened_Return_False");
                    logger.LogWarning("大板的时候 检查  中间蘑菇头已经打开，无法进行操作！");
                    return false;
                }
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                do
                {
                    //发送打开蘑菇头指令
                    ControlFrontBackMushroom("Open");
                    Thread.Sleep(1000);
                    //验证蘑菇头是否打开结束CbdOpen_Z2Middle=true=未开 CbdOpen_Z1FrontBack=false=已开
                    if ("前后蘑菇头已开" == MushroomFrontBackCloseLocaltion() && "中间蘑菇头未开" == MushroomMiddleCloseLocaltion())
                    {
                        logger.LogWarning("前后蘑菇头结束，请等待...");
                        stopwatch.Stop();
                        logger.LogWarning($"机器【{DeviceDescriptor.DeviceName} 】验证前后蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
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

        private string MushroomFrontBackCloseLocaltion()
        {
            string MushroomCloseLocalTionFlag = $"%S(HSYS55_Output,{DeviceDescriptor.Extra["FrontMushroomCloseLocalTionFlag"]})";//64
            //GetOutput(64)
            //opcUaClient.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1");
            string result = InteractingDevice.cnc84Command.GetRuntimeString(MushroomCloseLocalTionFlag);
            if ("1:1".Equals(result, StringComparison.CurrentCultureIgnoreCase))
            {
                return "前后蘑菇头未开";
            }
            return "前后蘑菇头已开";
        }

        private string MushroomMiddleCloseLocaltion()
        {
            string middleMushroomCloseLocalTionFlag = $"%S(HSYS55_Output,{DeviceDescriptor.Extra["MiddleMushroomCloseLocalTionFlag"]})";//55
            //GetOutput(55)
            //opcUaClient.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2");
            string result = InteractingDevice.cnc84Command.GetRuntimeString(middleMushroomCloseLocalTionFlag);
            if ("1:1".Equals(result, StringComparison.CurrentCultureIgnoreCase))
            {
                return "中间蘑菇头未开";
            }

            return "中间蘑菇头已开";
        }

        public void ControlMiddleMushroom(string str)
        {
            //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
            InteractingDevice.cnc84Command.SetCncComand($"DSP,OPEN_MIDDLE_MUSHROOM_M38");
            Thread.Sleep(300);
            InteractingDevice.cnc84Command.SetCncComand("SCRP,scCncCUSTOMERCbdClampOpenClosesmall");//M38,小板
            Thread.Sleep(5000);
            InteractingDevice.cnc84Command.SetCncComand($"DSP,");
            logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送 开启中间 {str}蘑菇头指令->SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
        }
        public void ControlThirdMushroom(string str)
        {
            //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
            InteractingDevice.cnc84Command.SetCncComand($"DSP,OPEN_MIDDLE_MUSHROOM_M35");
            Thread.Sleep(300);
            InteractingDevice.cnc84Command.SetCncComand("SCRP,scCncCUSTOMERCbdClampClosemiddle");//M35,第3组 中间板子
            Thread.Sleep(5000);
            InteractingDevice.cnc84Command.SetCncComand($"DSP,");
            logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送 开启中间 {str}蘑菇头指令->SCRP,scCncCUSTOMERCbdClampClosemiddle");//todo
        }

        public void ControlFrontBackMushroom(string str)
        {
            //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scKbiCbdClampOpenClose");
            InteractingDevice.cnc84Command.SetCncComand($"DSP,OPEN_FRONT_BACK_MASHROOM_M41");
            Thread.Sleep(300);
            InteractingDevice.cnc84Command.SetCncComand("SCRP,scKbiCbdClampOpenClose");//M41,大板,开前后蘑菇头 大板
            Thread.Sleep(5000);
            InteractingDevice.cnc84Command.SetCncComand($"DSP,");
            logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送{str}蘑菇头指令->SCRP,scKbiCbdClampOpenClose");
        }

        private bool StartOpenMushroom()
        {
            if ("前后蘑菇头已开" == MushroomFrontBackCloseLocaltion())
            {
                return false;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            ControlMushroom();

            do
            {
                if ("前后蘑菇头已开" == MushroomFrontBackCloseLocaltion())
                {
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateMushroomEndTimeOut"].ToLong());

            return false;
        }

        private bool MushroomCloseLocaltion()
        {
            //opcUaClient.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2");
            InteractingDevice.cnc84Command.SetCncComand($"DSP,Customer_CbdOpen_Z2");
            Thread.Sleep(1000);
            var result = InteractingDevice.cnc84Command.GetOutput(55);
            if ("1:1".Equals(result, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private void ControlMushroom()
        {
            //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scKbiCbdClampOpenClose");
            InteractingDevice.cnc84Command.SetCncComand($"DSP,M41_OPEN_FRONT_BACK_MASHROOM");
            Thread.Sleep(1000);
            InteractingDevice.cnc84Command.SetCncComand("SCRP,scKbiCbdClampOpenClose");//M27,大板,开前后蘑菇头,M41 CNC84 M27;CNC95 M41
        }

        public T DeepCopy<T>(T obj)
        {
            var stringObj = JsonSerializer.Serialize(obj);
            return (T)JsonSerializer.Deserialize(stringObj, typeof(T))!;
        }

        public async Task<string> CallAgv()
        {
            try
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(InteractingDevice.CallAgvTime);
                if (timeSpan.TotalSeconds < InteractingDevice.CallAgvTimeInterval)
                {
                    logger.LogDebug("小于呼叫间隔不能呼叫");
                    return "小于呼叫间隔不能呼叫";
                }

                InteractingDevice.CallAgvTime = DateTime.Now;
                if (!InteractingDevice.allowAllAgv)
                {
                    return "呼叫agv信号为 false 不能呼叫";
                }

                if (!InteractingDevice.MqttClientWrapper.IsConnected) return "呼叫AGV  Mqtt 连接断开 不上报消息";

                var payloadPanelsTemp = DeepCopy(InteractingDevice.PayloadPanels);
                int ExistRawNum = payloadPanelsTemp.Count(c => c.ProductStatus == ProductStatus.WaitingForDrill);
                var splineStatus = InteractingDevice.SplineNoBrokenStatus;
                int SpindleUseNum = splineStatus.Count(c => c == '1');

                var isExistClinker = payloadPanelsTemp.Any(c => c.ProductStatus == ProductStatus.Finished_DRILL);
                var materialStatus = string.Join("", PayloadPanels.Select(s => s.ProductStatus == ProductStatus.WaitingForDrill ? "1" : s.ProductStatus == ProductStatus.Finished_DRILL ? "2" : "0"));

                logger.LogDebug($"轴使用状态{splineStatus}  板材状态  {materialStatus} ");
                //交互行为
                var rawMaterialStatusCount = materialStatus.Count(c => c == '1');
                var clinkerMaterialStatusCount = materialStatus.Count(c => c == '2');
                var agvOperateSplineCount = InteractingDevice.spindleNum;
                IniAgvPosition();
                var agvPosition = InteractingDevice.spindleAgvPosition;
                var spindleBehavior = new int[splineStatus.Length];
                for (int i = 0; i < splineStatus.Length; i++)
                {
                    if (splineStatus[i] == '0' && materialStatus[i] == '0')
                    {
                        agvPosition[i] = "null";
                        spindleBehavior[i] = -1;
                        agvOperateSplineCount--;
                    }
                    else if (splineStatus[i] == '0' && materialStatus[i] == '1')
                    {
                        rawMaterialStatusCount--;
                        clinkerMaterialStatusCount++;
                        spindleBehavior[i] = 1;
                    }
                    else if (splineStatus[i] == '0' && materialStatus[i] == '2')
                    {
                        spindleBehavior[i] = 1;
                    }
                    else if (splineStatus[i] == '1' && materialStatus[i] == '0')
                    {
                        spindleBehavior[i] = 0;
                    }
                    else if (splineStatus[i] == '1' && materialStatus[i] == '1')
                    {
                        agvPosition[i] = "null";
                        spindleBehavior[i] = -1;
                        agvOperateSplineCount--;
                    }
                    else if (splineStatus[i] == '1' && materialStatus[i] == '2')
                    {
                        spindleBehavior[i] = 2;
                    }
                }
                var spindleBehaviorInf = string.Join(",", spindleBehavior);
                var agvPositionInf = string.Join(",", agvPosition);
                var result = await DataExporter.DeviceEventReport(
                     new DeviceEventReportRequest()
                     {
                         ProductId = DeviceDescriptor.ProductId,
                         DeviceId = DeviceDescriptor.DeviceId,
                         ClientId = InteractingDevice.ClientId,
                         EventId = Events.REQUEST_AGV_LOAD_PANEL_THEN_UNLOAD_PANEL,
                         RequestInputProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_2,
                         RequestOutputProductStatus = ProductStatus.Finished_DRILL,
                         RequestInteractionBehavior = InteractionBehavior.Make(this.DeviceDescriptor.DeviceKind, InteractionBehavior.FRONT_UNLOAD_PANEL_THEN_LOAD_PANEL),
                         EventName = Events.Drill.BUFFER_EXIST_CLINKER_EVENT_NAME,
                         RequestDeviceKind = DeviceKind.CNC95Drill,
                         RequestMaterialKind = MaterialKind.Panel,
                         RequestInteractionDirection = InteractionPosition.Front,
                         Params = new Dictionary<string, object?>()
                         {
                                    { "ExistRawNum", ExistRawNum },
                                    { "SpindleUseNum", SpindleUseNum },
                                    { "RawSpindleNum", rawMaterialStatusCount },
                                    { "SpindleNum", agvOperateSplineCount },
                                    { "ClinkerSpindleNum", clinkerMaterialStatusCount },
                                    { "InteractivePosition",  DeviceDescriptor.Extra["InteractivePosition"].ToStr () },
                                    { "Spindles", agvPositionInf},
                                    { "SpindleBehavior", spindleBehaviorInf },
                                    { "RegulatePos", DeviceDescriptor.Extra["RegulatePos"].ToStr() }
                         },
                         PayloadPanels = InteractingDevice.PayloadPanels,
                     });
                if (result == null)
                {
                    InteractingDevice.TransactionId = $"{DateTime.Now.ToString()}  ----  和服务器断开连接，呼叫失败";
                    return $"{DateTime.Now.ToString()}  ----  和服务器断开连接，呼叫失败";
                }
                else
                {
                    if (result.Code == ErrorCodes.Sys.SUCCESS)
                    {
                        InteractingDevice.NewTranscationIdTemp = result.TraceId;
                        InteractingDevice.allowAllAgv = false;
                        InteractingDevice.TransactionId = $"{DateTime.Now.ToString()}  ----呼叫成功   {result.TraceId}";
                    }
                    else if (result.Code == ErrorCodes.Sys.DUPLICATE_SERVICE_INVOKED_CODE)
                    {
                        InteractingDevice.TransactionId = $"{DateTime.Now.ToString()}  ----重复呼叫  {result.Message}";
                    }
                    else
                    {
                        InteractingDevice.TransactionId = $"{DateTime.Now.ToString()}  ----呼叫失败  {result.Message}";
                    }
                    logger.LogDebug($"上报信息结果：  {JsonSerializer.Serialize(result)}  ");
                    InteractingDevice.NewTranscationId = InteractingDevice.NewTranscationIdTemp;
                    return $"呼叫AGV  生料的个数{rawMaterialStatusCount}  下熟料个数{clinkerMaterialStatusCount} AGV位置：{agvPositionInf}   轴上下料信息：  {spindleBehaviorInf}   结束";
                }
            }
            catch (Exception ee)
            {
                logger.LogError($"呼叫AGV异常 ： {ee.Message} ");
                return $"呼叫AGV异常 {ee.Message}";
            }

        }

        public void IniAgvPosition()
        {
            InteractingDevice.spindleAgvPosition = Enumerable.Repeat("null", InteractingDevice.spindleNum).ToArray();
            var tmpSpindeles = DeviceDescriptor.Extra["Spindles"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries);
            Array.Copy(tmpSpindeles, InteractingDevice.spindleAgvPosition, InteractingDevice.spindleAgvPosition.Length);
        }

        public bool CheckDrlDiaFile(string drlFilePath, string diaFilePath)
        {
            string DiaFilePath_tmp = diaFilePath;
            string DrlFilePath_tmp = drlFilePath;
            logger.LogDebug("直径表" + DiaFilePath_tmp);
            logger.LogDebug("钻带程序路径" + DrlFilePath_tmp);

            if (!File.Exists(DrlFilePath_tmp))
            {
                logger.LogDebug("请检查是否有生产程序文件!");
                return false;
            }

            if (!string.IsNullOrEmpty(DiaFilePath_tmp) && !File.Exists(DiaFilePath_tmp))
            {

                logger.LogDebug("请检查是否有直径程序文件!");
                return false;
            }
            return true;
        }



        public double GetBSIZLength(double length)
        {
            if (length == 0)
            {
                length = DeviceDescriptor.Extra["DefalutBoardLength"].ToDouble();
                logger.LogDebug($"未设置板长 ，设置为默认压板长度{length}");
            }
            return length;
        }

        public bool LoadFile(string drilPath, string? diaPath = "")
        {
            if (!string.IsNullOrWhiteSpace(diaPath) && !ValidateDiaFileSame(diaPath))
            {
                InteractingDevice.cnc84Command.SendLoadFile(diaPath, "DIAMETERTABLE");
                if (!ValidateDiaFile(diaPath))
                {
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(drilPath) && !ValidateDrlFileSame(drilPath))
            {
                InteractingDevice.cnc84Command.SetCncComand("CM@@@");
                if (!ValidateCmDrlFile())
                {
                    return false;
                }

                InteractingDevice.cnc84Command.SendLoadFile(drilPath, "PROGRAM");
                //SendLoadFile(drilPath, "PROGRAM");
                if (!ValidateDrlFile(drilPath))
                {
                    return false;
                }
                //验证是否重新加载
                if (!ValidateDrlImg())
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidateDrlImg()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            do
            {
                Thread.Sleep(1000);
                string blockText = InteractingDevice.cnc84Command.ReadCncNode<string>("ns=4;s=UI/origin/RosiInfo/BlockText");
                logger.LogDebug($"ValidateDrlImg  {blockText}");
                if (!string.IsNullOrEmpty(blockText) && (blockText.Contains("等待开始") || blockText.Contains("Waiting for start")))
                {
                    logger.LogDebug($"ValidateDrlImg  验证结束");
                    stopwatch.Stop();
                    return true;
                }

            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateCmFileTimeout"].ToLong());
            return false;
        }

        public object[] SendLoadFile(string filePath, string fileType)
        {
            string[] parameters = { filePath, fileType };
            object[] dataValue = InteractingDevice.cnc84Command!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_LOAD", parameters);
            return dataValue;
        }

        private bool ValidateCmDrlFile()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var netProName = string.Empty;
            do
            {
                Thread.Sleep(1000);
                netProName = InteractingDevice.cnc84Command.GetACTProgram();
                if (string.IsNullOrEmpty(netProName) || "[NULL]".Equals(netProName))
                {
                    stopwatch.Stop();
                    return true;
                }
                InteractingDevice.cnc84Command.SetCncComand("CM@@@");
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateCmFileTimeout"].ToLong());
            return false;
        }

        private bool ValidateDiaFileSame(string diaFilePath)
        {
            //opcUaClient.ReadNode<String>("ns=4;s=UI/origin/FilesAndPaths/ToolFiles/DiameterGroup/DiameterTable");
            var diaName = InteractingDevice.cnc84Command.GetRuntimeString("%S(DiaFileNameWithDialog)");
            if (string.IsNullOrEmpty(diaName))
            {
                return false;
            }
            if ($"1:{diaFilePath.Replace("\\", "").Replace("/", "").ToLower()}".Equals(diaName.Replace("\\", "").Replace("/", "").ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        private bool ValidateDrlFileSame(string drlFilePath)
        {
            var drlName = InteractingDevice.cnc84Command.GetACTProgram();
            if (string.IsNullOrEmpty(drlName))
            {
                return false;
            }
            if ($"{drlFilePath.Replace("\\", "").Replace("/", "").ToLower()}".Equals(drlName.Replace("\\", "").Replace("/", "").ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        private bool ValidateDiaFile(string diaFilePath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var diaName = string.Empty;
            do
            {
                Thread.Sleep(50);
                //opcUaClient.ReadNode<String>("ns=4;s=UI/origin/FilesAndPaths/ToolFiles/DiameterGroup/DiameterTable");
                diaName = InteractingDevice.cnc84Command.GetRuntimeString("%S(DiaFileNameWithDialog)");
                if (string.IsNullOrEmpty(diaName))
                {
                    continue;
                }
                if ($"1:{diaFilePath.Replace("\\", "").Replace("/", "").ToLower()}".Equals(diaName.Replace("\\", "").Replace("/", "").ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateDiaFileTimeout"].ToLong());
            stopwatch.Stop();
            return false;
        }

        private bool ValidateDrlFile(string drilFilePath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var netProName = string.Empty;
            do
            {
                Thread.Sleep(100);
                netProName = InteractingDevice.cnc84Command.GetACTProgram();
                if (string.IsNullOrEmpty(netProName))
                {
                    continue;
                }
                var modifyDrilFilePath = drilFilePath.Replace("\\", "").Replace("/", "").ToLower();
                var modifyNextProName = netProName.Replace("\\", "").Replace("/", "").ToLower();
                logger.LogInformation($"校验程序名 modifyDrilFilePath:{modifyDrilFilePath} modifyNextProName{modifyNextProName}");

                if (modifyDrilFilePath.Equals(modifyNextProName, StringComparison.CurrentCultureIgnoreCase))
                {
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateDrlFileTimeout"].ToLong());
            return false;
        }

        public Task LoadFileFromCenter() => throw new NotImplementedException();
    }
}
