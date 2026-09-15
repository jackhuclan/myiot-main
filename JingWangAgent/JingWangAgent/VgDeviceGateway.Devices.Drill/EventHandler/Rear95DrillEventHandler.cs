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
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Drill.EventHandler
{
    public class Rear95DrillEventHandler : DeviceShare<DefaultDrill>, IDrillEventHandler
    {
        private readonly ILogger<Rear95DrillEventHandler> logger;
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
        public readonly byte slaveID;
        public readonly bool codeReaderTriggerIsM;
        public readonly bool firstCheckOnOff;
        public readonly bool statisticsOnOff;
        public readonly string drillFullTableAppendContent;
        public readonly string drillHalfTableAppendContent;
        public readonly bool drillChangeAfterPathOnOff;
        public readonly bool aSplineIsWorkFullTable;
        public readonly bool validateFileAppendOnOff;
        public readonly bool existFourMushroom;
        public readonly bool payloadPanelOptimizeOnOff;
        public static volatile int RunNum = 0;
        public static volatile int bufferToDrillNum = 0;
        public static volatile int drillToBufferNum = 0;
        private string _strGroupNoFromGetAtp = string.Empty;

        public Rear95DrillEventHandler(ILogger<Rear95DrillEventHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
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
            slaveID = (byte)device.DeviceDescriptor.Extra["SlaveID"].ToInt();
            codeReaderTriggerIsM = device.DeviceDescriptor.Extra["CodeReaderTriggerIsM"].ToBool();
            firstCheckOnOff = device.DeviceDescriptor.Extra["FirstCheckOnOff"].ToBool();
            statisticsOnOff = device.DeviceDescriptor.Extra["StatisticsOnOff"].ToBool();
            drillFullTableAppendContent = device.DeviceDescriptor.Extra["DrillFullTableAppendContent"].ToStr().ToLower();
            drillHalfTableAppendContent = device.DeviceDescriptor.Extra["DrillHalfTableAppendContent"].ToStr().ToLower();
            drillChangeAfterPathOnOff = device.DeviceDescriptor.Extra["DrillChangeAfterPathOnOff"].ToBool();
            aSplineIsWorkFullTable = device.DeviceDescriptor.Extra["ASplineIsWorkFullTable"].ToBool();
            validateFileAppendOnOff = device.DeviceDescriptor.Extra["ValidateFileAppendOnOff"].ToBool();
            existFourMushroom = device.DeviceDescriptor.Extra["ExistFourMushroom"].ToBool();
            payloadPanelOptimizeOnOff = device.DeviceDescriptor.Extra["PayloadPanelOptimizeOnOff"].ToBool();
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
                      logger.LogDebug($"统计时间 {DateTime.Now.ToShortTimeString()} Drill_ShowText   变化旧的程序 {showText.OldValue.ToStr()}  新的程序 {showText.NewValue.ToStr()} ");
                      var data = showText.NewValue.ToStr();

                      if (data.Contains("Board Direction begin"))
                      {
                          WatchingProperties.Property("Drill_BoardDirectionStartTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                      }
                      else if (data.Contains("Board Direction end"))
                      {
                          WatchingProperties.Property("Drill_BoardDirectionEndTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                      }
                      else if (data.Contains("Tool Evaluation begin"))
                      {
                          WatchingProperties.Property("Drill_ToolEvaluationStartTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                      }
                      else if (data.Contains("Tool Evaluation end"))
                      {
                          WatchingProperties.Property("Drill_ToolEvaluationEndTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                      }
                  });

            WatchingProperties.Property("Drill_CurrentProgramFile")
              .PostCondition(p => p.IsValueChanged && !string.IsNullOrWhiteSpace(p.NewValue.ToStr()))
              .TriggerAlways(async () =>
              {
                  RunNum = 0;
                  var currentProgramFile = WatchingProperties.Property("Drill_CurrentProgramFile");
                  logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  首件检测  开启{firstCheckOnOff} 程序发生变化旧的程序 {currentProgramFile.OldValue.ToStr()}  新的程序 {currentProgramFile.NewValue.ToStr()} ");

                  if (firstCheckOnOff)
                  {
                      logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  首件检测  检测结果清空 置上首件检测 ");
                      InteractingDevice.cnc84Command.WriteCncNode<bool>($"ns=4;s=UI/normalized/custom/{InteractingDevice.DeviceDescriptor.Extra["FirstCheckResult"].ToStr()}", false);
                      InteractingDevice.cnc84Command.WriteCncNode<bool>($"ns=4;s=UI/normalized/custom/{InteractingDevice.DeviceDescriptor.Extra["FirstCheckFlag"].ToStr()}", true);
                  }
              });

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

            if (codeReaderTriggerIsM)
            {
                WatchingProperties.Property("Buffer_CodeReaderTrigger")
               .PostCondition(p => p.IsValueChanged)
               .TriggerAlways(async () =>
               {
                   if (InteractingDevice.codeReaderOnOff)
                   {
                       var triggerStr = WatchingProperties.Property("Buffer_CodeReaderTrigger").NewValue.ToStr();
                       logger.LogDebug($"Buffer_CodeReaderTrigger：触发读码 信号 {triggerStr}");

                       for (int i = 0; i < triggerStr.Length; i++)
                       {
                           if (triggerStr[i] == '1')
                           {
                               Task.Factory.StartNew((index) =>
                               {
                                   int codeIndex = (int)index;
                                   logger.LogInformation($"轴{codeIndex + 1} 触发读码 线程开始");
                                   InteractingDevice.codeReaderBaseSetting.SendScanMesssageToSignal(codeIndex, "1", true);
                                   Task.Delay(3000).Wait();
                                   do
                                   {
                                       ushort curTrigger = InteractingDevice.modbusIpMaster.ReadInputRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["CodeReaderTrigger"].ToUshort(), 1)[0];
                                       if (((curTrigger & (1 << codeIndex)) == (1 << codeIndex)) && (string.IsNullOrEmpty(InteractingDevice.codeReaderBaseSetting.receiveData[codeIndex.ToStr()])))
                                       {
                                           InteractingDevice.codeReaderBaseSetting.SendScanMesssageToSignal(codeIndex, "1");
                                           Task.Delay(3000).Wait();
                                       }
                                       else
                                       {
                                           break;
                                       }
                                   } while (true);
                                   logger.LogInformation($"轴{codeIndex + 1} 触发读码 线程结束");
                               }, i);
                           }
                       }
                   }
               });
            }
            else
            {
                WatchingProperties.Property("Buffer_CodeReaderTrigger")
               .PostCondition(p => p.IsValueChanged && p.NewValue.ToInt() != 0)
               .TriggerAlways(async () =>
               {
                   if (InteractingDevice.codeReaderOnOff)
                   {
                       var trigger = WatchingProperties.Property("Buffer_CodeReaderTrigger").NewValue.ToInt();
                       var triggerStr = Convert.ToString(trigger, 2).PadLeft(InteractingDevice.spindleNum, '0').ToArray().Reverse().ToStr();
                       logger.LogDebug($"Buffer_CodeReaderTrigger：触发读码 信号 {triggerStr}");

                       for (int i = 0; i < triggerStr.Length; i++)
                       {
                           if (triggerStr[i] == '1')
                           {
                               Task.Factory.StartNew((index) =>
                               {
                                   int codeIndex = (int)index;
                                   logger.LogInformation($"轴{codeIndex + 1} 触发读码 线程开始");
                                   InteractingDevice.codeReaderBaseSetting.SendScanMesssageToSignal(codeIndex, "1");
                                   Task.Delay(3000).Wait();
                                   do
                                   {
                                       ushort curTrigger = InteractingDevice.modbusIpMaster.ReadInputRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["CodeReaderTrigger"].ToUshort(), 1)[0];
                                       if (((curTrigger & (1 << codeIndex)) == (1 << codeIndex)) && (string.IsNullOrEmpty(InteractingDevice.codeReaderBaseSetting.receiveData[codeIndex.ToStr()])))
                                       {
                                           InteractingDevice.codeReaderBaseSetting.SendScanMesssageToSignal(codeIndex, "1");
                                           Task.Delay(3000).Wait();
                                       }
                                       else
                                       {
                                           break;
                                       }
                                   } while (true);
                                   logger.LogInformation($"轴{codeIndex + 1} 触发读码 线程结束");
                               }, i);
                           }
                       }
                   }
               });
            }

            WatchingProperties.Property("Buffer_CallAgvMessage")
            .PostCondition(p => !p.NewValue.ToBool())
            .TriggerAlways(async () =>
            {
                bool resetCallAgvFlag = await QueryScheduleResetCallAgv();
                logger.LogDebug($"QueryScheduleResetCallAgv 方法被调用了：重置 {resetCallAgvFlag} 信号");
                if (resetCallAgvFlag)
                {
                    logger.LogDebug($"QueryScheduleResetCallAgv 方法被调用了：重置 InteractingDevice.allowAllAgv 信号");
                    InteractingDevice.allowAllAgv = true;
                }
            });

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
            WatchingProperties.Property("Buffer_AgvOnWorkBuffer")
              .PostCondition(p => !p.NewValue.ToBool() && p.IsValueChanged)
              .TriggerAlways(() =>
              {
                  InteractingDevice.AgvEndTime = DateTime.Now;
                  try
                  {
                      var material = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 2);
                      var rawMaterialLayerBoardStatus = material[0];
                      if (rawMaterialLayerBoardStatus != 0)
                      {
                          InteractingDevice.NewTranscationId = InteractingDevice.NewTranscationIdTemp;
                      }
                  }
                  catch (Exception ee)
                  {
                      logger.LogDebug($"NewTranscationId 赋值异常 {ee.Message}");
                  }

                  InteractingDevice.NewTranscationIdTemp = "";
              });

            #region PayloadPanels

            //钻机上板子状态从0变成非0 表示buffer上层给钻机上料
            WatchingProperties.Property("Drill_BoardPositionStatus")
              .PostCondition(p => p.IsValueChanged && p.OldValue.ToInt() == 0 && p.NewValue.ToInt() > 0)
              .TriggerAlways(async () =>
              {
                  if (payloadPanelOptimizeOnOff)
                  {
                      try
                      {
                          if (!InteractingDevice.DeviceDescriptor.AutoMode) return;
                          var boardPositionStatus = WatchingProperties.Property("Drill_BoardPositionStatus");
                          logger.LogError($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatus  上生料到钻机 钻机上板子状态从{boardPositionStatus.OldValue.ToInt()}变成 {boardPositionStatus.NewValue.ToInt()} ");
                          var rawLayerBoardStatus = WatchingProperties.Property("Buffer_RawMaterialLayerBoardStatus");
                          logger.LogError($"{DateTime.Now.ToShortTimeString()}生料层板子状态  {rawLayerBoardStatus}");
                          var holeStart = WatchingProperties.Property("Drill_DrillHoleStart").NewValue.ToBool();
                          logger.LogError($"Drill_BoardPositionStatus ->程序状态---> {holeStart} ");
                          var screenText = WatchingProperties.Property("Drill_ScreenText");
                          logger.LogError($"{DateTime.Now.ToShortTimeString()}  Drill_BoardPositionStatus  当前屏幕字段  {screenText.NewValue.ToStr()}");
                          var bufferRawPanels = InteractingDevice.PayloadPanels.Take(InteractingDevice.spindleNum);
                          logger.LogError($"{DateTime.Now.ToShortTimeString()}  Drill_BoardPositionStatus  板料信息  {JsonSerializer.Serialize(bufferRawPanels)}");


                          ////控制板子转化
                          logger.LogError($"{DateTime.Now.ToShortTimeString()} Transid 修改前  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");
                          InteractingDevice.DrillTransactionId = InteractingDevice.NewTranscationId;
                          //InteractingDevice.NewTranscationId = "";
                          logger.LogError($"{DateTime.Now.ToShortTimeString()} Transid 修改后  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");

                          logger.LogError($"{DateTime.Now.ToShortTimeString()} buffer上层给钻机上料  未修改【 PayloadPanels 】  {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                          string itemCode = string.Empty;
                          //判断一下当前的agv给buffer上料的panel中的taskcode 是否一致 不一致不流转

                          if (bufferRawPanels.All(s => string.IsNullOrEmpty(s.ItemCode)) || rawLayerBoardStatus.NewValue.ToInt() != 0)
                          {
                              logger.LogError($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatus 板材信息 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} 上层没有板料信息  buffer上层板子的状态{rawLayerBoardStatus.NewValue.ToInt()} 不流转");
                              return;
                          }

                          await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                          {
                              for (int i = InteractingDevice.spindleNum; i < 2 * InteractingDevice.spindleNum; i++)
                              {
                                  InteractingDevice.PayloadPanels[i] = InteractingDevice.PayloadPanels[i - InteractingDevice.spindleNum];
                                  if (!string.IsNullOrEmpty(InteractingDevice.PayloadPanels[i].ItemCode) && !string.IsNullOrEmpty(itemCode))
                                  {
                                      itemCode = InteractingDevice.PayloadPanels[i].ItemCode;
                                  }
                                  InteractingDevice.PayloadPanels[i]!.Layer = 1;
                                  if (InteractingDevice.PayloadPanels[i]!.ProductStatus == ProductStatus.WaitingForDrill)
                                  {
                                      InteractingDevice.PayloadPanels[i]!.ProductStatus = ProductStatus.Drilling;
                                  }
                                  var panel = Panel.HasSilo.NoPanelForSingleSpindle("", InteractingDevice.PayloadPanels[i].Position, 0, 1)[0];
                                  panel.LocationCode = InteractingDevice.DeviceId;
                                  panel.SiloCode = InteractingDevice.DeviceId;
                                  InteractingDevice.PayloadPanels[i - InteractingDevice.spindleNum] = panel;
                              }
                          }));

                          logger.LogError($"{DateTime.Now.ToShortTimeString()}   buffer上层给钻机上料 已修改【 PayloadPanels 】 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                      }
                      catch (Exception)
                      {
                          await Task.CompletedTask;
                      }
                      await Task.CompletedTask;
                  }
                  else
                  {
                      try
                      {
                          var bufferRawPanels = InteractingDevice.PayloadPanels.Take(InteractingDevice.spindleNum);
                          logger.LogError($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatus  生料层板材信息 {JsonSerializer.Serialize(bufferRawPanels)}");
                          var rawLayerBoardStatus = WatchingProperties.Property("Buffer_RawMaterialLayerBoardStatus");
                          logger.LogError($"{DateTime.Now.ToShortTimeString()}  Drill_BoardPositionStatus  生料层板子状态  {rawLayerBoardStatus.NewValue.ToInt()}");
                          var screenText = WatchingProperties.Property("Drill_ScreenText");
                          logger.LogError($"{DateTime.Now.ToShortTimeString()}  Drill_BoardPositionStatus  当前屏幕字段  {screenText.NewValue.ToStr()}");

                          var boardPositionStatus = WatchingProperties.Property("Drill_BoardPositionStatus");
                          logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatus  上生料到钻机 钻机上板子状态从{boardPositionStatus.OldValue.ToInt()}变成 {boardPositionStatus.NewValue.ToInt()} ");
                          if (!InteractingDevice.DeviceDescriptor.AutoMode) return;
                          logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatus  板材从buffer流转到钻机的次数 {bufferToDrillNum} ");
                          logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatus  是否开了根据程序状态判断生料板材流转 {InteractingDevice.DeviceDescriptor.Extra["RawChangeByProgramStateOnOff"].ToBool()}");

                          if (InteractingDevice.DeviceDescriptor.Extra["RawChangeByProgramStateOnOff"].ToBool())
                          {
                              try
                              {
                                  var progState = InteractingDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/WorkGroupSettings/WorkGroupInterface/TimeWidget/ProgramState");
                                  logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatus  progState {progState} ");

                                  if (new int[] { 2, 3, 4 }.Contains(progState))
                                  {
                                      logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatus 程序在加工过程中 不能流转生料板材  ");
                                      return;
                                  }
                              }
                              catch (Exception ee)
                              {
                                  logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatus  根据程序状态 生料转板材异常 {ee.Message} ");
                              }
                          }

                          if (bufferToDrillNum >= 1)
                          {
                              logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatus  不流转板材信息 ");
                              return;
                          }
                          bufferToDrillNum++;
                          logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatus  从buffer流转到钻机的测试修改板材  {bufferToDrillNum} ");

                          ////控制板子转化
                          logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Transid 修改前  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");
                          InteractingDevice.DrillTransactionId = InteractingDevice.NewTranscationId;
                          //InteractingDevice.NewTranscationId = "";
                          logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Transid 修改后  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");

                          logger.LogWarning($"{DateTime.Now.ToShortTimeString()} buffer上层给钻机上料  未修改【 PayloadPanels 】  {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                          string itemCode = string.Empty;

                          await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                          {
                              for (int i = InteractingDevice.spindleNum; i < 2 * InteractingDevice.spindleNum; i++)
                              {
                                  InteractingDevice.PayloadPanels[i] = InteractingDevice.PayloadPanels[i - InteractingDevice.spindleNum];
                                  if (!string.IsNullOrEmpty(InteractingDevice.PayloadPanels[i].ItemCode) && !string.IsNullOrEmpty(itemCode))
                                  {
                                      itemCode = InteractingDevice.PayloadPanels[i].ItemCode;
                                  }
                                  InteractingDevice.PayloadPanels[i]!.Layer = 1;
                                  if (InteractingDevice.PayloadPanels[i]!.ProductStatus == ProductStatus.WaitingForDrill)
                                  {
                                      InteractingDevice.PayloadPanels[i]!.ProductStatus = ProductStatus.Drilling;
                                  }
                                  var panel = Panel.HasSilo.NoPanelForSingleSpindle("", InteractingDevice.PayloadPanels[i].Position, 0, 1)[0];
                                  panel.LocationCode = InteractingDevice.DeviceId;
                                  panel.SiloCode = InteractingDevice.DeviceId;
                                  InteractingDevice.PayloadPanels[i - InteractingDevice.spindleNum] = panel;
                              }
                          }));

                          logger.LogWarning($"{DateTime.Now.ToShortTimeString()}   buffer上层给钻机上料 已修改【 PayloadPanels 】 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                      }
                      catch (Exception)
                      {
                          await Task.CompletedTask;
                      }
                      await Task.CompletedTask;
                  }
              });

            //buffer 下层板子状态从0变成非0 表示钻机下熟料给buffer下层
            WatchingProperties.Property("Buffer_ClinkerLayerBoardStatus")
               .PostCondition(p => p.IsValueChanged && p.OldValue.ToInt() == 0 && p.NewValue.ToInt() > 0)
               .TriggerAlways(async () =>
               {
                   if (payloadPanelOptimizeOnOff)
                   {
                       try
                       {
                           if (!InteractingDevice.DeviceDescriptor.AutoMode) return;

                           var clinkerLayerBoardStatus = WatchingProperties.Property("Buffer_ClinkerLayerBoardStatus");
                           logger.LogError($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus 下熟料到buffer   下层板子状态从{clinkerLayerBoardStatus.OldValue.ToInt()}变成 {clinkerLayerBoardStatus.NewValue.ToInt()} ");
                           //判断一下buffer上层有板料  buffer上层是空
                           var drillPanels = InteractingDevice.PayloadPanels.Skip(InteractingDevice.spindleNum).Take(InteractingDevice.spindleNum);
                           logger.LogError($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus 钻机上面的板材信息   {JsonSerializer.Serialize(drillPanels)} ");

                           var boardPositionStatus = WatchingProperties.Property("Drill_BoardPositionStatus");
                           logger.LogError($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus  钻机上板子的状态 {boardPositionStatus.NewValue.ToInt()} ");
                           var holeStart = WatchingProperties.Property("Drill_DrillHoleStart").NewValue.ToBool();
                           logger.LogError($"Buffer_ClinkerLayerBoardStatus ->程序状态---> {holeStart} ");
                           var screenText = WatchingProperties.Property("Drill_ScreenText");
                           logger.LogError($"{DateTime.Now.ToShortTimeString()}  Buffer_ClinkerLayerBoardStatus  当前屏幕字段  {screenText.NewValue.ToStr()}");


                           logger.LogError($"{DateTime.Now.ToShortTimeString()}  下熟料 未修改【 PayloadPanels 】 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                           logger.LogError($"{DateTime.Now.ToShortTimeString()} 下熟料  Transid 修改前  {InteractingDevice.NewTranscationId}  钻机 {InteractingDevice.DrillTransactionId} 熟料{InteractingDevice.OldTransactionId}");
                           InteractingDevice.OldTransactionId = InteractingDevice.DrillTransactionId;
                           //InteractingDevice.DrillTransactionId = "";
                           logger.LogError($"{DateTime.Now.ToShortTimeString()} 下熟料 Transid 修改后  {InteractingDevice.NewTranscationId}  钻机 {InteractingDevice.DrillTransactionId} 熟料{InteractingDevice.OldTransactionId}");

                           if (drillPanels.All(s => string.IsNullOrEmpty(s.ItemCode)))
                           {
                               logger.LogError($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus 板材信息 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} 钻机没有板料信息  钻机传感器{boardPositionStatus.NewValue.ToInt()} 不流转");
                               return;
                           }

                           await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                           {
                               for (int i = 2 * InteractingDevice.spindleNum; i < InteractingDevice.PayloadPanels.Count; i++)
                               {
                                   InteractingDevice.PayloadPanels[i] = InteractingDevice.PayloadPanels[i - InteractingDevice.spindleNum];
                                   InteractingDevice.PayloadPanels[i]!.Layer = 2;
                                   if (InteractingDevice.PayloadPanels[i].ProductStatus == ProductStatus.Drilling)
                                   {
                                       InteractingDevice.PayloadPanels[i]!.ProductStatus = ProductStatus.Finished_DRILL;
                                   }
                                   var panel = Panel.HasSilo.NoPanelForSingleSpindle("", InteractingDevice.PayloadPanels[i].Position, 1, 1)[0];
                                   panel.LocationCode = InteractingDevice.DeviceId;
                                   panel.SiloCode = InteractingDevice.DeviceId;
                                   InteractingDevice.PayloadPanels[i - InteractingDevice.spindleNum] = panel;
                               }
                           }));

                           logger.LogError($"{DateTime.Now.ToShortTimeString()}  下熟料  已修改【 PayloadPanels 】 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                       }
                       catch (Exception)
                       {
                           await Task.CompletedTask;
                       }
                       await Task.CompletedTask;
                   }
                   else
                   {
                       if (!InteractingDevice.DeviceDescriptor.AutoMode) return;
                       try
                       {
                           var drillPanels = InteractingDevice.PayloadPanels.Skip(InteractingDevice.spindleNum).Take(InteractingDevice.spindleNum);
                           logger.LogError($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus  钻机层板材信息 {JsonSerializer.Serialize(drillPanels)}");
                           var drillPositionStatus = WatchingProperties.Property("Drill_BoardPositionStatus");
                           logger.LogError($"{DateTime.Now.ToShortTimeString()}  Buffer_ClinkerLayerBoardStatus  钻机板子状态  {drillPositionStatus.NewValue.ToInt()}");
                           var screenText = WatchingProperties.Property("Drill_ScreenText");
                           logger.LogError($"{DateTime.Now.ToShortTimeString()}  Buffer_ClinkerLayerBoardStatus  当前屏幕字段  {screenText.NewValue.ToStr()}");

                           var clinkerLayerBoardStatus = WatchingProperties.Property("Buffer_ClinkerLayerBoardStatus");
                           logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus  下熟料到buffer buffer上层传感器状态{clinkerLayerBoardStatus.OldValue.ToInt()}变成 {clinkerLayerBoardStatus.NewValue.ToInt()} ");
                           logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus  下熟料板材从钻机流转到buffer的次数 {drillToBufferNum} ");
                           logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus  是否开了根据程序状态判断板材流转 {InteractingDevice.DeviceDescriptor.Extra["ClinkerChangeByProgramStateOnOff"].ToBool()}");

                           if (InteractingDevice.DeviceDescriptor.Extra["ClinkerChangeByProgramStateOnOff"].ToBool())
                           {
                               try
                               {
                                   var progState = InteractingDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/WorkGroupSettings/WorkGroupInterface/TimeWidget/ProgramState");
                                   logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus  progState {progState} ");

                                   if (new int[] { 2, 3, 4 }.Contains(progState))
                                   {
                                       logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus 程序在加工过程中 不能流转熟料板材  ");
                                       return;
                                   }
                               }
                               catch (Exception ee)
                               {
                                   logger.LogWarning($"{DateTime.Now.ToShortTimeString()} ClinkerChangeByProgramStateOnOff  根据程序状态 流转熟料板材异常 {ee.Message} ");
                               }
                           }

                           if (drillToBufferNum >= 1)
                           {
                               logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus  下熟料不流转板材信息 ");
                               return;
                           }
                           drillToBufferNum++;
                           logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus  下熟料从钻机流转到Buffer修改板材次数  {bufferToDrillNum} ");

                           logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  下熟料 未修改 PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                           logger.LogDebug($"{DateTime.Now.ToShortTimeString()} Transid 修改前  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");
                           InteractingDevice.OldTransactionId = InteractingDevice.DrillTransactionId;
                           //InteractingDevice.DrillTransactionId = "";
                           logger.LogDebug($"{DateTime.Now.ToShortTimeString()} Transid 修改后  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");
                           await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                           {
                               for (int i = 2 * InteractingDevice.spindleNum; i < InteractingDevice.PayloadPanels.Count; i++)
                               {
                                   InteractingDevice.PayloadPanels[i] = InteractingDevice.PayloadPanels[i - InteractingDevice.spindleNum];
                                   InteractingDevice.PayloadPanels[i]!.Layer = 2;
                                   if (InteractingDevice.PayloadPanels[i].ProductStatus == ProductStatus.Drilling)
                                   {
                                       InteractingDevice.PayloadPanels[i]!.ProductStatus = ProductStatus.Finished_DRILL;
                                   }
                                   var panel = Panel.HasSilo.NoPanelForSingleSpindle("", InteractingDevice.PayloadPanels[i].Position, 1, 1)[0];
                                   panel.LocationCode = InteractingDevice.DeviceId;
                                   panel.SiloCode = InteractingDevice.DeviceId;
                                   InteractingDevice.PayloadPanels[i - InteractingDevice.spindleNum] = panel;
                               }
                           }));

                           logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  下熟料 已修改 PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                           await InteractingDevice.PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.DeviceId);
                       }
                       catch (Exception)
                       {
                           await Task.CompletedTask;
                       }
                       await Task.CompletedTask;
                   }

               });

            #endregion PayloadPanels
            //Drill_DrillHoleEnd
            WatchingProperties.Property("Drill_DrillHoleStart")
                .PostCondition(p => p.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    var holeStart = WatchingProperties.Property("Drill_DrillHoleStart").NewValue.ToBool();
                    logger.LogWarning($"Drill_DrillHoleStart->钻孔开始信号-发生变化-> {holeStart} ");
                    try
                    {
                        logger.LogDebug($"FinishTask:上报 任务完成/开始");
                        if (!holeStart)
                        {   //打板结束没有退板直接上板了  手动上的时候要用力气撞一下销钉  要不然 传感器感应不到板子
                            //下料信号是啥时候给plc的:plc自己判断的  钻机代理只给p4泊车位和钻孔结束信号
                            //判断钻机是否有板子
                            logger.LogDebug($"FinishTask:上报 任务完成： 返回： 打板结束 end ");
                            var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);//116-1
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["BufferLoadToDrillFlag"].ToUshort(), 0);//406-1
                            logger.LogDebug($"{DateTime.Now.ToShortTimeString()} 打板结束信号 钻机结束 buffer给agv上料信号 ");
                            await Task.Delay(200);
                            InteractingDevice.modbusIpMaster.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["Cnc84WorkEndWritePlc"].ToUshort(), ready[0] != 0);//打板结束M23-1
                            logger.LogDebug($"{DateTime.Now.ToShortTimeString()} 打板结束信号 钻机是否存在板子 {ready[0] != 0} ");

                            var res = InteractingDevice.OldTransactionId == null ? "OldTransactionI为空" : InteractingDevice.OldTransactionId;
                            logger.LogDebug($"{DateTime.Now.ToShortTimeString()} ,DeviceDescriptor.AutoMode= {DeviceDescriptor.AutoMode},InteractingDevice.OldTransactionId={res} ");

                            if (DeviceDescriptor.AutoMode && !string.IsNullOrWhiteSpace(InteractingDevice.OldTransactionId))
                            {
                                var panel = InteractingDevice.PayloadPanels.Skip(InteractingDevice.spindleNum).Take(InteractingDevice.spindleNum).FirstOrDefault(s => !string.IsNullOrEmpty(s.TaskCode));
                                var TaskCode = panel == null ? $"" : panel.TaskCode;
                                var req = new FinishTaskRequest()
                                {
                                    ProductId = DeviceDescriptor.ProductId,
                                    DeviceId = DeviceDescriptor.DeviceId,
                                    TraceId = InteractingDevice.OldTransactionId,
                                    Params = new Dictionary<string, object?>()
                                    {
                                        {"TaskCode" ,TaskCode}
                                    }
                                };
                                logger.LogDebug($"FinishTask:上报 任务完成： 请求参数：  {JsonSerializer.Serialize(req)}  ");
                                var result = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<FinishTaskRequest, FinishTaskResponse>(InteractingDevice.CentralWebOptions.FinishTask, req);
                                logger.LogDebug($"FinishTask:上报 任务完成： 返回：  {JsonSerializer.Serialize(result)}  ");
                            }
                        }
                        else
                        {
                            InteractingDevice.modbusIpMaster?.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["Cnc84WorkEndWritePlc"].ToUshort(), false);
                            var res = InteractingDevice.DrillTransactionId == null ? "DrillTransactionId" : InteractingDevice.DrillTransactionId;
                            logger.LogDebug($"{DateTime.Now.ToShortTimeString()} ,DeviceDescriptor.AutoMode= {DeviceDescriptor.AutoMode},InteractingDevice.DrillTransactionId={res} ");

                            if (DeviceDescriptor.AutoMode && !string.IsNullOrWhiteSpace(InteractingDevice.DrillTransactionId))
                            {
                                logger.LogDebug($"FinishTask:上报 任务完成： 返回： 打板开始 start  ");

                                await ReportStartInf();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        await Task.CompletedTask;
                    }
                    await Task.CompletedTask;
                });
            #region old Drill_DrillHoleEnd
            //钻孔打板结束信号
            //WatchingProperties.Property("Drill_DrillHoleEnd")
            //    .PostCondition(p => p.IsValueChanged)
            //    .TriggerAlways(async () =>
            //    {
            //        var holeEnd = WatchingProperties.Property("Drill_DrillHoleEnd").NewValue.ToBool();
            //        logger.LogWarning($"Drill_DrillHoleEnd->钻孔结束信号-发生变化-> {holeEnd} ");
            //        try
            //        {
            //            logger.LogDebug($"FinishTask:上报 任务完成/开始");
            //            if (holeEnd)
            //            {   //打板结束没有退板直接上板了  手动上的时候要用力气撞一下销钉  要不然 传感器感应不到板子
            //                //下料信号是啥时候给plc的:plc自己判断的  钻机代理只给p4泊车位和钻孔结束信号
            //                //判断钻机是否有板子
            //                logger.LogDebug($"FinishTask:上报 任务完成： 返回： 打板结束 end ");
            //                var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);//116-1
            //                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["BufferLoadToDrillFlag"].ToUshort(), 0);//406-1
            //                logger.LogDebug($"{DateTime.Now.ToShortTimeString()} 打板结束信号 钻机结束 buffer给agv上料信号 ");
            //                await Task.Delay(200);
            //                InteractingDevice.modbusIpMaster.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["Cnc84WorkEndWritePlc"].ToUshort(), ready[0] != 0);//打板结束M23-1
            //                logger.LogDebug($"{DateTime.Now.ToShortTimeString()} 打板结束信号 钻机是否存在板子 {ready[0] != 0} ");

            //                var res = InteractingDevice.OldTransactionId == null ? "OldTransactionI为空" : InteractingDevice.OldTransactionId;
            //                logger.LogDebug($"{DateTime.Now.ToShortTimeString()} ,DeviceDescriptor.AutoMode= {DeviceDescriptor.AutoMode},InteractingDevice.OldTransactionId={res} ");

            //                if (DeviceDescriptor.AutoMode && !string.IsNullOrWhiteSpace(InteractingDevice.OldTransactionId))
            //                {
            //                    var req = new FinishTaskRequest()
            //                    {
            //                        ProductId = DeviceDescriptor.ProductId,
            //                        DeviceId = DeviceDescriptor.DeviceId,
            //                        TraceId = InteractingDevice.OldTransactionId,
            //                    };
            //                    logger.LogDebug($"FinishTask:上报 任务完成： 请求参数：  {JsonSerializer.Serialize(req)}  ");
            //                    var result = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<FinishTaskRequest, FinishTaskResponse>(InteractingDevice.CentralWebOptions.FinishTask, req);
            //                    logger.LogDebug($"FinishTask:上报 任务完成： 返回：  {JsonSerializer.Serialize(result)}  ");
            //                }
            //            }
            //            else
            //            {
            //                InteractingDevice.modbusIpMaster?.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["Cnc84WorkEndWritePlc"].ToUshort(), false);
            //                var res = InteractingDevice.DrillTransactionId == null ? "DrillTransactionId" : InteractingDevice.DrillTransactionId;
            //                logger.LogDebug($"{DateTime.Now.ToShortTimeString()} ,DeviceDescriptor.AutoMode= {DeviceDescriptor.AutoMode},InteractingDevice.DrillTransactionId={res} ");

            //                if (DeviceDescriptor.AutoMode && !string.IsNullOrWhiteSpace(InteractingDevice.DrillTransactionId))
            //                {
            //                    logger.LogDebug($"FinishTask:上报 任务完成： 返回： 打板开始 start  ");

            //                    await ReportStartInf();
            //                }
            //            }
            //        }
            //        catch (Exception)
            //        {
            //            await Task.CompletedTask;
            //        }
            //        await Task.CompletedTask;
            //    });
            #endregion
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
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["ScanGunInfoNumOnPlc"].ToUshort(), codeInfo.Length.ToUshort());
                        InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveID, DeviceDescriptor.Extra["ScanGunInfoStartPositionOnPlc"].ToUshort(), new ushort[20]);
                        InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveID, DeviceDescriptor.Extra["ScanGunInfoStartPositionOnPlc"].ToUshort(), codeInfo.Select(x => (ushort)x).ToArray());
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
                                WriteWarningToPlc(306);
                                return;
                            }
                            else if (response?.Code != ErrorCodes.Sys.SUCCESS)
                            {
                                logger.LogDebug(response?.Message);
                                WriteWarningToPlc(307);
                                return;
                            }

                            if (Cnc84DiaFileOnOff)
                            {
                                if (response.Params == null || !response.Params.ContainsKey("DrlPath") || !response.Params.ContainsKey("DiaPath"))
                                {
                                    logger.LogError("未下发配方路径");
                                    WriteWarningToPlc(308);
                                    return;
                                }
                            }
                            else
                            {
                                if (response.Params == null || !response.Params.ContainsKey("DrlPath"))
                                {
                                    logger.LogError("未下发配方路径");
                                    WriteWarningToPlc(308);
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
                        //if (!CheckDrlDiaFile(drillPath, diaPath))
                        //{
                        //    logger.LogError("配方路径文件不存在");
                        //    return;
                        //}
                        if (!LoadFile(drillPath, diaPath))
                        {
                            WriteWarningToPlc(11);
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
                            WriteWarningToPlc(errrorCode);
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

            WatchingProperties.Property("Buffer_RawMaterialLayerLoadEnd")
              .PostCondition(p => p.IsValueChanged && p.NewValue.ToBool())
              .TriggerAlways(async () =>
              {
                  logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  buffer 生料层上料完成  ");
                  if (DeviceDescriptor.AutoMode)
                  {
                      await DataExporter.DeviceEventReport(
                           new DeviceEventReportRequest()
                           {
                               ProductId = DeviceDescriptor.ProductId,
                               DeviceId = DeviceDescriptor.DeviceId,
                               ClientId = InteractingDevice.ClientId,
                               EventId = Events.Drill.BUFFER_RAW_MATERIAL_LAYER_LOAD_END_EVENT,
                               EventName = Events.Drill.BUFFER_RAW_MATERIAL_LAYER_LOAD_END_EVENT_NAME,
                               RequestDeviceKind = DeviceKind.CNC95Drill,
                           });
                  }
              });

            WatchingProperties.Property("Buffer_ClinkerLayerUnloadEnd")
             .PostCondition(p => p.IsValueChanged && p.NewValue.ToBool())
             .TriggerAlways(async () =>
             {
                 logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  buffer 熟料层下料完成  ");
                 if (DeviceDescriptor.AutoMode)
                 {
                     await DataExporter.DeviceEventReport(
                          new DeviceEventReportRequest()
                          {
                              ProductId = DeviceDescriptor.ProductId,
                              DeviceId = DeviceDescriptor.DeviceId,
                              ClientId = InteractingDevice.ClientId,
                              EventId = Events.Drill.BUFFER_CLINKER_LAYER_UNLOAD_END_EVENT,
                              EventName = Events.Drill.BUFFER_CLINKER_LAYER_UNLOAD_END_EVENT_NAME,
                              RequestDeviceKind = DeviceKind.CNC95Drill,
                          });
                 }
             });

            WatchingProperties.Property("Buffer_Automatic")
               .PostCondition(p => p.IsValueChanged)
               .TriggerAlways(async () =>
               {
                   var bufferAutomatic = WatchingProperties.Property("Buffer_Automatic").NewValue.ToBool();
                   if (bufferAutomatic == false)
                   {
                       WatchingProperties.Property("Buffer_ManualStartTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                   }
                   else
                   {
                       if (string.IsNullOrEmpty(WatchingProperties.Property("Buffer_ManualStartTime").NewValue.ToString()))
                       {
                           WatchingProperties.Property("Buffer_ManualStartTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                       }

                       WatchingProperties.Property("Buffer_ManualEndTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                   }

                   logger.LogDebug($"统计时间  {DateTime.Now.ToShortTimeString()}  buffer 自动状态 {bufferAutomatic}  ");

                   if (DeviceDescriptor.AutoMode)
                   {
                       if (bufferAutomatic)
                       {
                           await DataExporter.DeviceEventReport(
                            new DeviceEventReportRequest()
                            {
                                ProductId = DeviceDescriptor.ProductId,
                                DeviceId = DeviceDescriptor.DeviceId,
                                ClientId = InteractingDevice.ClientId,
                                EventId = Events.Drill.BUFFER_ON_AUTOMATIC_EVENT,
                                EventName = Events.Drill.BUFFER_ON_AUTOMATIC_EVENT_NAME,
                                RequestDeviceKind = DeviceKind.CNC95Drill,
                            });
                       }
                       else
                       {
                           await DataExporter.DeviceEventReport(
                               new DeviceEventReportRequest()
                               {
                                   ProductId = DeviceDescriptor.ProductId,
                                   DeviceId = DeviceDescriptor.DeviceId,
                                   ClientId = InteractingDevice.ClientId,
                                   EventId = Events.Drill.BUFFER_ON_MANUAL_EVENT,
                                   EventName = Events.Drill.BUFFER_ON_MANUAL_EVENT_NAME,
                                   RequestDeviceKind = DeviceKind.CNC95Drill,
                               });
                       }
                   }
               });

            WatchingProperties.Properties("Buffer_WarningCode", "Drill_WarningCode")
                 .When(p =>
                 {
                     var bufferWarningCode = p.Property("Buffer_WarningCode");
                     var drillWarningCode = p.Property("Drill_WarningCode");
                     return (bufferWarningCode.NewValue.ToInt() != 0 || drillWarningCode.NewValue.ToInt() != 0) && (bufferWarningCode.IsValueChanged || drillWarningCode.IsValueChanged);
                 })
               .TriggerAlways(async () =>
               {
                   var bufferWarningCode = WatchingProperties.Property("Buffer_WarningCode").NewValue.ToInt();
                   var drillWarningCode = WatchingProperties.Property("Drill_WarningCode").NewValue.ToInt();
                   logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-buffer 异常代码:{bufferWarningCode} 钻机异常代码:{drillWarningCode}");

                   if (!InteractingDevice.threeColorLightsExistsOnBuffer && !InteractingDevice.plcAlarmNotNotifyDrillErrorIds.Contains(bufferWarningCode))
                   {
                       StartThreeColorLights();
                   }
                   if (DeviceDescriptor.AutoMode)
                   {
                       await DataExporter.DeviceEventReport(
                            new DeviceEventReportRequest()
                            {
                                ProductId = DeviceDescriptor.ProductId,
                                DeviceId = DeviceDescriptor.DeviceId,
                                ClientId = InteractingDevice.ClientId,
                                EventId = Events.Drill.BUFFER_WARNING_EVENT,
                                EventName = Events.Drill.BUFFER_WARNING_EVENT_NAME,
                                RequestDeviceKind = DeviceKind.CNC95Drill,
                                PayloadPanels = InteractingDevice.PayloadPanels,
                            });
                   }
               });

            WatchingProperties.Properties("Buffer_EndUnloadClinker", "Buffer_LoadOrUnloadAxis")
                 .When(p =>
                 {
                     var bufferEndUnloadClinker = p.Property("Buffer_EndUnloadClinker");
                     return bufferEndUnloadClinker.NewValue.ToBool() && bufferEndUnloadClinker.IsValueChanged;
                 })
               .TriggerAlways(async () =>
               {
                   var bufferEndUnloadClinker = WatchingProperties.Property("Buffer_EndUnloadClinker").NewValue.ToBool();
                   var axis = WatchingProperties.Property("Buffer_LoadOrUnloadAxis").NewValue.ToBool();
                   logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-buffer 轴：{axis}下熟料： {bufferEndUnloadClinker}");
                   if (DeviceDescriptor.AutoMode)
                   {
                       await DataExporter.DeviceEventReport(
                            new DeviceEventReportRequest()
                            {
                                ProductId = DeviceDescriptor.ProductId,
                                DeviceId = DeviceDescriptor.DeviceId,
                                ClientId = InteractingDevice.ClientId,
                                EventId = Events.Drill.BUFFER_AXIS_UNLOAD_CLINKER_END_EVENT,
                                EventName = Events.Drill.BUFFER_AXIS_UNLOAD_CLINKER_END_EVENT_NAME,
                                RequestDeviceKind = DeviceKind.CNC95Drill,
                                Params = new Dictionary<string, object?>()
                                {
                                { "Axis", axis },
                                },
                                PayloadPanels = InteractingDevice.PayloadPanels,
                            });
                   }
               });

            WatchingProperties.Properties("Buffer_EndLoadRawMaterial", "Buffer_LoadOrUnloadAxis")
                 .When(p =>
                 {
                     var bufferEndLoadRawMaterial = p.Property("Buffer_EndLoadRawMaterial");
                     return bufferEndLoadRawMaterial.NewValue.ToBool() && bufferEndLoadRawMaterial.IsValueChanged;
                 })
               .TriggerAlways(async () =>
               {
                   var bufferEndLoadRawMaterial = WatchingProperties.Property("Buffer_EndLoadRawMaterial").NewValue.ToBool();
                   var axis = WatchingProperties.Property("Buffer_LoadOrUnloadAxis").NewValue.ToBool();
                   logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-buffer 轴：{axis}接收到生料 {bufferEndLoadRawMaterial}");
                   if (DeviceDescriptor.AutoMode)
                   {
                       await DataExporter.DeviceEventReport(
                        new DeviceEventReportRequest()
                        {
                            ProductId = DeviceDescriptor.ProductId,
                            DeviceId = DeviceDescriptor.DeviceId,
                            ClientId = InteractingDevice.ClientId,
                            EventId = Events.Drill.BUFFER_AXIS_LOAD_RAW_MATERIAL_END_EVENT,
                            EventName = Events.Drill.BUFFER_AXIS_LOAD_RAW_MATERIAL_END_EVENT_NAME,
                            RequestDeviceKind = DeviceKind.CNC95Drill,
                            Params = new Dictionary<string, object?>()
                            {
                            { "Axis", axis },
                            },
                            PayloadPanels = InteractingDevice.PayloadPanels,
                        });
                   }
               });

            WatchingProperties.Property("Buffer_OnAgvPosition")
                 .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
               .TriggerAlways(async () =>
               {
                   logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-buffer到达agv对接层");
                   if (DeviceDescriptor.AutoMode)
                   {
                       await DataExporter.DeviceEventReport(
                            new DeviceEventReportRequest()
                            {
                                ProductId = DeviceDescriptor.ProductId,
                                DeviceId = DeviceDescriptor.DeviceId,
                                ClientId = InteractingDevice.ClientId,
                                EventId = Events.Drill.BUFFER_ON_AGV_POSITION_EVENT_NAME,
                                EventName = Events.Drill.BUFFER_ON_AGV_POSITION_EVENT_NAME,
                                RequestDeviceKind = DeviceKind.CNC95Drill,
                            });
                   }
               });
            WatchingProperties.Properties("Buffer_OnAgvPosition", "Buffer_AgvOnWorkBuffer", "Buffer_RawMaterialLayerBoardStatus", "Buffer_ClinkerLayerBoardStatus")
                .When(p =>
                {
                    var onAgvPosition = p.Property("Buffer_OnAgvPosition");
                    var automatic = p.Property("Buffer_Automatic");
                    var agvOnWorkBuffer = p.Property("Buffer_AgvOnWorkBuffer");
                    var rawMaterialLayerBoardStatus = p.Property("Buffer_RawMaterialLayerBoardStatus");

                    var clinkerLayerBoardStatus = p.Property("Buffer_ClinkerLayerBoardStatus");
                    var clinerIsZero = clinkerLayerBoardStatus.NewValue.ToInt() == 0;
                    var rawFallNum = Convert.ToInt32(new string(Enumerable.Repeat('1', InteractingDevice.spindleNum).ToArray()), 2);
                    var rawIsFall = rawMaterialLayerBoardStatus.NewValue.ToInt() == rawFallNum;
                    return onAgvPosition.NewValue.ToBool() && automatic.NewValue.ToBool() && !agvOnWorkBuffer.NewValue.ToBool() && clinerIsZero && rawIsFall && InteractingDevice.bufferAllUnloadAndLoadEnd != 1;
                })
                .TriggerAlways(async () =>
                {
                    try
                    {
                        //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 1);
                        //logger.LogDebug($"钻机代理  无熟料 生料已满 解除总的上下料信号  ");
                    }
                    catch (Exception)
                    {
                        //logger.LogError($"钻机代理  解除总的上下料信号 ： {ee.Message} ");

                        await Task.CompletedTask;
                    }
                });

            WatchingProperties.Properties("Buffer_OnAgvPosition", "Buffer_IsReady", "Buffer_Automatic", "Buffer_AgvOnWorkBuffer", "Buffer_Warning", "Buffer_RawMaterialLayerBoardStatus", "Buffer_ClinkerLayerBoardStatus")
              .When(p =>
              {
                  var onAgvPosition = p.Property("Buffer_OnAgvPosition");
                  var isReady = p.Property("Buffer_IsReady");
                  var automatic = p.Property("Buffer_Automatic");
                  var agvOnWorkBuffer = p.Property("Buffer_AgvOnWorkBuffer");
                  var bufferWarning = p.Property("Buffer_Warning");
                  var rawMaterialLayerBoardStatus = p.Property("Buffer_RawMaterialLayerBoardStatus");

                  var clinkerLayerBoardStatus = p.Property("Buffer_ClinkerLayerBoardStatus");
                  var clinerIsZero = clinkerLayerBoardStatus.NewValue.ToInt() == 0;
                  var rawFallNum = Convert.ToInt32(new string(Enumerable.Repeat('1', InteractingDevice.spindleNum).ToArray()), 2);
                  var rawIsFall = rawMaterialLayerBoardStatus.NewValue.ToInt() == rawFallNum;

                  TimeSpan timeSpan = DateTime.Now.Subtract(InteractingDevice.AgvEndTime);
                  var agvEndSpanTime = timeSpan.TotalSeconds < InteractingDevice.AgvEndTimeInterval;
                  if (agvEndSpanTime)
                  {
                      logger.LogDebug($"AGV给钻机上料结束刚结束 不能呼叫 生料层{rawMaterialLayerBoardStatus.NewValue.ToInt()}  熟料层 {clinkerLayerBoardStatus.NewValue.ToInt()}");
                  }

                  bool result = onAgvPosition.NewValue.ToBool() && isReady.NewValue.ToBool() && automatic.NewValue.ToBool() && !agvOnWorkBuffer.NewValue.ToBool() && InteractingDevice.allowAllAgv && !agvEndSpanTime && !bufferWarning.NewValue.ToBool() && DeviceDescriptor.AutoMode && InteractingDevice.MqttClientWrapper.IsConnected && (!(clinerIsZero && rawIsFall));
                  if (result)
                  {
                      InteractingDevice.CallCondition = $"{DateTime.Now.ToString()}:  满足条件能发起呼叫 生料层{rawMaterialLayerBoardStatus.NewValue.ToInt()}  熟料层 {clinkerLayerBoardStatus.NewValue.ToInt()}";
                  }
                  else
                  {
                      List<string> message = new List<string>();
                      if (!onAgvPosition.NewValue.ToBool()) message.Add("不在AGV对接层");
                      if (!isReady.NewValue.ToBool()) message.Add("buffer 不是ready状态");
                      if (!automatic.NewValue.ToBool()) message.Add("buffer 不是自动状态");
                      if (agvOnWorkBuffer.NewValue.ToBool()) message.Add("Agv在给Buffer上下料,不能呼叫");
                      if (!InteractingDevice.allowAllAgv) message.Add("呼叫已经成功发起不能再次呼叫");
                      if (agvEndSpanTime) message.Add("agv刚给buffer上料结束不能再次呼叫");
                      if (bufferWarning.NewValue.ToBool()) message.Add("Buffer在报警状态下不能呼叫");
                      if (!DeviceDescriptor.AutoMode) message.Add("设备配置是手动 不能呼叫");
                      if (!InteractingDevice.MqttClientWrapper.IsConnected) message.Add("Mqtt断开连接 不能呼叫");
                      if ((clinerIsZero && rawIsFall)) message.Add("生料已满 也无熟料 不能呼叫");
                      InteractingDevice.CallCondition = $"{DateTime.Now.ToString()}: 不满足条件不能发起呼叫： {string.Join(",", message)} ";
                      try
                      {
                          if (DeviceDescriptor.AutoMode)
                              DataExporter.DeviceAlarmReport(new DeviceAlarmReportRequest()
                              {
                                  ProductId = InteractingDevice.ProductId,
                                  DeviceId = InteractingDevice.DeviceId,
                                  RequestDeviceKind = InteractingDevice.DeviceDescriptor.DeviceKind,
                                  AlarmCode = "",
                                  AlarmContent = string.Join(",", message),
                                  AlarmTime = DateTime.Now,
                                  AlarmLevel = AlarmLevel.Information,
                                  AlarmKind = AlarmKind.Unknown
                              });
                      }
                      catch (Exception)
                      {
                      }
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
            //==2024-05-29================================================================================================================================================================================================================
            //M21 buffer给钻机上料结束后续操作，选轴，压板，开启蘑菇头，开始打板
            WatchingProperties.Property("Buffer_UnLoadAndLoadEndFlag")
              .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  int errorNum = 0;
                  try
                  {
                      TimeSpan timeSpan = DateTime.Now.Subtract(DrillOprationTime);
                      if (timeSpan.TotalSeconds < InteractingDevice.CallAgvTimeInterval)
                      {
                          logger.LogWarning("Drill_DrillHoleEnd 钻机和buffer整个上下料结束 重复进来");
                          return;
                      }
                      if (DeviceDescriptor.Extra["IsOneScript"].ToBool())
                      {
                          errorNum = await CallOneScript();
                          return;
                      }
                      else if (DeviceDescriptor.Extra["OptimizeActions"].ToBool())
                      {
                          errorNum = await OptimizeActions();
                          return;
                      }
                      InteractingDevice.isOnDoingScript = true;
                      SendMessageToShow(0);
                      InteractingDevice.cnc84Command.SetCncComand($"DSP,New_Loop_CNC_Buffer_Load_UnLoad_Begin");
                      await Task.Delay(2000);
                      DrillOprationTime = DateTime.Now;
                      logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  钻机和buffer整个上下料结束");

                      InteractingDevice.cnc84Command.SetCncComand($"DSP,1/8_Waiting_Alarm_Not_In_Safe_Location_Cancel");
                      SendMessageToShow(1);

                      #region todo

                      var escTimeout = DeviceDescriptor.Extra["ValidateEscCommandTimeOut"].ToInt();    //等待ESC,持续2分钟。
                      while (escTimeout > 0 && !InteractingDevice.EscFlag)
                      {
                          await Task.Delay(10);
                          escTimeout--;
                      }

                      if (!InteractingDevice.EscFlag)
                      {
                          WriteWarningToPlc(19);
                          logger.LogWarning($"钻机给buffer报警编号：19");                                  //等待ESC,持续2分钟,超时给PLC报19代码
                          errorNum = 1;
                          return;
                      }

                      InteractingDevice.cnc84Command.SetCncComand($"DSP,2/8_Spindle_Select");
                      SendMessageToShow(2);
                      await Task.Delay(2000);
                      logger.LogWarning($"是否开启选轴功能 ： {selectSplineOnOff} ");                      //SelectSplineOnOff 是否开启选轴功能
                      if (selectSplineOnOff)
                      {
                          StartSelectSpline();
                      }
                      await Task.Delay(200);

                      InteractingDevice.cnc84Command.SetCncComand($"DSP,3/8_Request_Central_Control_Drl_File");
                      SendMessageToShow(3);
                      await Task.Delay(2000);

                      #endregion todo

                      logger.LogWarning($"是否向中控要加载程序和参数 ： {loadFileFromCentreOnOff}  自动模式 {DeviceDescriptor.AutoMode} ");
                      if (DeviceDescriptor.AutoMode && loadFileFromCentreOnOff)
                      {
                          string result = "0";
                          try
                          {
                              var panel = InteractingDevice.PayloadPanels.Skip(InteractingDevice.spindleNum).Take(InteractingDevice.spindleNum).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s.ItemCode));
                              var itemCode = string.Empty; if (panel != null) { itemCode = panel.ItemCode; }
                              if (string.IsNullOrEmpty(itemCode))
                              {
                                  logger.LogError($"itemCode 是空 不能向中控发起呼叫获取工艺分组");
                                  InteractingDevice.cnc84Command.SetCncComand($"DSP,ITEMCODE_IS_NULL");
                                  result = "2";
                                  WriteWarningToPlc(7);
                                  return;
                              }
                              var url = string.Format(DeviceDescriptor.Extra["GetGroupCode"].ToStr(), DeviceDescriptor.DeviceId, itemCode);
                              var group = await HttpRequestInvoker.GetFromJsonAsync<Dictionary<string, object?>>(url);
                              if (group == null || group.Count == 0)
                              {
                                  logger.LogError($"向中控请求获取工艺分组 {url} 异常");
                                  InteractingDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_GET_GETGROUPCODE_URL");
                                  result = "3";
                                  WriteWarningToPlc(7);
                                  return;
                              }
                              logger.LogDebug($"向中控请求{url}的返回值为{JsonSerializer.Serialize(group)}");
                              if (!group.ContainsKey("SpecGroup") || string.IsNullOrEmpty(group["SpecGroup"].ToString()))
                              {
                                  logger.LogError($"向中控请求{url} 返回值中 不包括 SpecGroup 或工艺分组为空");
                                  InteractingDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_NOT_CONTAINSKEY_SPECGROUP");
                                  result = "4";
                                  WriteWarningToPlc(7);
                                  return;
                              }
                              var groupStr = group["SpecGroup"].ToString();
                              logger.LogDebug($"向中控请求的工艺分组为{groupStr}");
                              InteractingDevice.cnc84Command.SetCncComand($"DSP,Get_data_from_database");
                              DrillInfo drillInf;
                              try
                              {
                                  drillInf = InteractingDevice.DrillFilePathLocator.GetFilePath($"{itemCode};{groupStr}");
                              }
                              catch (Exception ee)
                              {
                                  if (!ee.Message.StartsWith("EAP_MATERIAL_DATA"))
                                  {
                                      InteractingDevice.cnc84Command.SetCncComand($"DSP,3/8_DATABASE_EXCEPTION");
                                  }
                                  else
                                  {
                                      InteractingDevice.cnc84Command.SetCncComand($"DSP,{ee.Message}");
                                  }
                                  logger.LogError($"向中控请求的工艺分组为{groupStr} 和数据库交互失败 {ee.Message}");
                                  result = "5";
                                  WriteWarningToPlc(7);
                                  return;
                              }
                              logger.LogError($"数据库交互获取数据 {JsonSerializer.Serialize(drillInf)}");
                              if (string.IsNullOrWhiteSpace(drillInf.DrlPath))
                              {
                                  logger.LogError("和数据库交互获取钻带文件不存在");
                                  InteractingDevice.cnc84Command.SetCncComand($"DSP, DRILL_PATH_NOT_FIND");
                                  result = "6";
                                  WriteWarningToPlc(7);
                                  return;
                              }
                              InteractingDevice.cnc84Command.SetCncComand($"DSP,Get_data_from_center");
                              //向中控要转化后的路径
                              var drillPathUrl = string.Format(DeviceDescriptor.Extra["GetAfterDrillPath"].ToStr(), DeviceDescriptor.DeviceId, drillInf.DrlPath, itemCode);
                              var drillPathResult = await HttpRequestInvoker.GetFromJsonAsync<Dictionary<string, object?>>(drillPathUrl);
                              if (drillPathResult == null)
                              {
                                  logger.LogError($"向中控请求获取转化后钻带程序 {drillPathUrl} 异常");
                                  InteractingDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_GET_AFTER_DRILL_PATH_URL");
                                  result = "7";
                                  WriteWarningToPlc(7);
                                  return;
                              }
                              logger.LogError($"向中控请求转化后{drillPathUrl}的返回值为{JsonSerializer.Serialize(drillPathResult)}");
                              if (!drillPathResult.ContainsKey("AfterDrillPath") || string.IsNullOrEmpty(drillPathResult["AfterDrillPath"].ToString()))
                              {
                                  logger.LogError($"向中控请求{drillPathUrl} 返回值中 不包括 AfterDrillPath 或 AfterDrillPath为空");
                                  InteractingDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_NOT_CONTAINSKEY_AFTERDRILLPATH");
                                  result = "8";
                                  WriteWarningToPlc(7);
                                  return;
                              }

                              string afterDrillPath = drillPathResult["AfterDrillPath"].ToStr();
                              logger.LogDebug($"是否转化 AfterDrillPath开关 {drillChangeAfterPathOnOff},A轴是否工作整个台面 {aSplineIsWorkFullTable} 中控下发AfterDrillPath路径{afterDrillPath}");
                              if (drillChangeAfterPathOnOff)
                              {
                                  string tempafterDrillPath = ModifyAfterPath(afterDrillPath);
                                  afterDrillPath = tempafterDrillPath;
                              }
                              string filename = Path.GetFileNameWithoutExtension(afterDrillPath).ToLower();
                              if (validateFileAppendOnOff && !ValidateFileAppendContent(filename))
                              {
                                  logger.LogError($"配方路径文件不是该机台用的钻带");
                                  InteractingDevice.cnc84Command.SetCncComand($"DSP, DRILL_PATH_NOT_MATCH_TABLE_SIZE");
                                  result = "9";
                                  WriteWarningToPlc(7);
                                  return;
                              }
                              string drillPath = afterDrillPath;
                              string diaPath = drillInf.DiaPath;
                              string atpPath = string.Empty;
                              //if (!CheckDrlDiaFile(drillPath, diaPath))
                              //{
                              //    logger.LogError("配方路径文件不存在");
                              //    InteractingDevice.cnc84Command.SetCncComand($"DSP, DRILL_OR_DIA_PATH_NOT_FIND");
                              //    result = "10";
                              //    WriteWarningToPlc(7);
                              //    return;
                              //}
                              if (DeviceDescriptor.Extra["IsGetAtpFromCentre"].ToBool())
                              {
                                  var strGetAtpUrl = string.Format(DeviceDescriptor.Extra["GetAtpFromCentreUrl"].ToStr(), DeviceDescriptor.DeviceId);
                                  logger.LogDebug($"调用请求ATPFile接口, {strGetAtpUrl}");
                                  var getAtpFileResponse = await HttpRequestInvoker.GetFromJsonAsync<GetAtpFileResponse>(strGetAtpUrl);
                                  logger.LogDebug($"调用请求ATPFile接口, {strGetAtpUrl}，返回, {JsonSerializer.Serialize(getAtpFileResponse)}");
                                  if (getAtpFileResponse == null)
                                  {
                                      logger.LogError($"向中控请求{strGetAtpUrl} 返回值为空");
                                      throw new Exception($"向中控请求{strGetAtpUrl} 返回值为空");
                                  }
                                  else if (getAtpFileResponse.code != 0)
                                  {
                                      logger.LogError($"请求{strGetAtpUrl} 返回值code != 0，原因 {getAtpFileResponse?.message}");
                                      throw new Exception($"请求{strGetAtpUrl} 返回值code != 0，原因 {getAtpFileResponse?.message}");
                                  }
                                  else if (getAtpFileResponse.data == null)
                                  {
                                      logger.LogError($"{strGetAtpUrl} 返回值data或isNeedLoad为空");
                                      throw new Exception($"{strGetAtpUrl} 返回值data为空");
                                  }
                                  else if (getAtpFileResponse.data.isNeedLoad)
                                  {
                                      if (string.IsNullOrWhiteSpace(getAtpFileResponse.data.atpFile) || string.IsNullOrWhiteSpace(getAtpFileResponse.data.groupNo))
                                      {
                                          logger.LogError($"{strGetAtpUrl} 返回值中atp文件路径或groupNo为空, isNeedLoad={getAtpFileResponse.data.isNeedLoad}");
                                          throw new Exception($"{strGetAtpUrl} 返回值中atp文件路径或groupNo为空, isNeedLoad={getAtpFileResponse.data.isNeedLoad}");
                                      }

                                      atpPath = getAtpFileResponse.data.atpFile;
                                      _strGroupNoFromGetAtp = getAtpFileResponse.data.groupNo;
                                  }
                              }

                              if (!LoadFileToCNC84(drillPath, diaPath, "", atpPath))
                              {
                                  logger.LogDebug($"加载程序到CNC84 失败");
                                  InteractingDevice.cnc84Command.SetCncComand($"DSP, LOAD_DRILL_FAIL");
                                  result = "11";
                                  WriteWarningToPlc(7);
                                  return;
                              }
                              result = "1";
                          }
                          catch (Exception ee)
                          {
                              logger.LogDebug($"和中控交互异常{ee.Message}");
                              InteractingDevice.cnc84Command.SetCncComand($"DSP, LOAD_FILE_FROM_CENTRE_FAIL");
                              result = "12";
                              WriteWarningToPlc(7);
                              return;
                          }
                          finally
                          {
                              if (result != "1")
                              {
                                  result = "2";
                                  errorNum = 2;
                              }
                              //通知eap
                              if (InteractingDevice.ApplicationServices.TryGetService<IDrillFileLoadResult>(out IDrillFileLoadResult fileLoadResult))
                              {
                                  fileLoadResult?.WriteFileLoadResult(result);
                              }
                          }
                      }

                      InteractingDevice.cnc84Command.SetCncComand($"DSP,4/8_Press_The_Board");
                      SendMessageToShow(4);
                      await Task.Delay(2000);
                      logger.LogWarning($"是否开启压板功能 ： {pressBoardOnOff} ");                        //PressBoardOnOff 是否开启压板功能
                      if (pressBoardOnOff && !StartPressBoard())
                      {
                          WriteWarningToPlc(12);
                          logger.LogDebug($"钻机给buffer报警编号：12");
                          errorNum = 3;
                          return;
                      }

                      if (!InteractingDevice.scanGunOnOff)                                              //ScanGunOnOff 扫码枪启用
                      {
                          /*CNC95编写脚本绑定evAfterProgramAnalysis事件来给大小板标志位置位
                          string isSmallBoard = "0";
                          try
                          {
                              string bSizContent = File.ReadLines(WatchFilePath).Where(x => x.Contains(" BSIZ")).Last();
                              string[] arr = Regex.Split(bSizContent, " BSIZ");
                              if (arr.Length == 2 && arr[1].ToDouble() < DeviceDescriptor.Extra["SmallBoardMaxLength"].ToDouble())
                              {
                                  isSmallBoard = "1";
                                  logger.LogDebug($"读取的板长{arr[1].ToDouble()} 默认的 最小板的最大长度为{DeviceDescriptor.Extra["SmallBoardMaxLength"].ToDouble()}");
                              }
                          }
                          catch (Exception)
                          {
                              WriteWarningToPlc(29);
                              logger.LogDebug($"钻机给buffer报警编号：29");
                              logger.LogDebug("读取命令文件错误");
                          }
                          //SetUserFlag(int flag, int num)
                          //opcUaClient.WriteNode<Boolean>("ns=4;s=UI/normalized/custom/UF.SmallBoard", false);
                          InteractingDevice.cnc84Command.SetUserFlag(isSmallBoard.ToInt(), DeviceDescriptor.Extra["SmallBoardUserFlag"].ToInt());
                          logger.LogDebug($"设置小板用户标记 {isSmallBoard} ");
                          */
                      }

                      InteractingDevice.cnc84Command.SetCncComand($"DSP,5/8_Open_The_Mushroom");
                      SendMessageToShow(5);
                      await Task.Delay(2000);

                      logger.LogWarning($"是否开启蘑菇头功能 ： 配置{mushroomOnOff} 实际{InteractingDevice.mushroomValue} ");                         //MushroomOnOff 是否开启蘑菇头功能
                      if (InteractingDevice.mushroomValue && !StartOpenMushroomControllerNew())
                      {
                          logger.LogWarning($"开始选蘑菇头");
                          WriteWarningToPlc(9);
                          logger.LogWarning($"钻机给buffer报警编号：9");
                          errorNum = 4;
                          return;
                      }

                      InteractingDevice.cnc84Command.SetCncComand($"DSP,6/8_Check_Program_Drl_Dia");
                      SendMessageToShow(6);
                      await Task.Delay(2000);

                      logger.LogWarning($"是否检查程序和钻带功能 ： {checkProgramAndDiaOnOff}  自动模式 {DeviceDescriptor.AutoMode} ");

                      if (DeviceDescriptor.AutoMode && checkProgramAndDiaOnOff)                         //CheckProgramAndDiaOnOff 是否检查程序和钻带功能
                      {
                          var programPath = WatchingProperties.Property("Drill_CurrentProgramFile").NewValue.ToStr();
                          var diaPath = WatchingProperties.Property("Drill_CurrentParameterFile").NewValue.ToStr();

                          logger.LogWarning($"检查程序和钻带 ：钻带 {programPath}  参数 {diaPath} ");
                          if (DeviceDescriptor.AutoMode)
                          {
                              var result = await DataExporter.DeviceEventReport(
                             new DeviceEventReportRequest()
                             {
                                 ProductId = DeviceDescriptor.ProductId,
                                 DeviceId = DeviceDescriptor.DeviceId,
                                 ClientId = InteractingDevice.ClientId,
                                 EventId = Events.Drill.CHECK_PROGRAM_AND_DIA_EVENT,
                                 EventName = "检查钻带参数和钻带文件请求",
                                 RequestDeviceKind = DeviceKind.CNC95Drill,
                                 Params = new Dictionary<string, object?>()
                                 {
                                   { "ProgramPath", programPath },
                                   { "DiaPath", diaPath },
                                 },
                                 PayloadPanels = InteractingDevice.PayloadPanels,
                             });

                              if (result == null)
                              {
                                  logger.LogWarning($"检查程序和钻带 和中控服务器断开连接了 ");
                                  WriteWarningToPlc(300);
                              }
                              else if (result.Code != ErrorCodes.Sys.SUCCESS)
                              {
                                  logger.LogWarning($"中控反馈的结果 不匹配 不能进行打板 ");
                                  WriteWarningToPlc(301);
                                  return;
                              }
                          }
                      }

                      InteractingDevice.cnc84Command.SetCncComand($"DSP,7/8_Check_Tool_Life");
                      SendMessageToShow(7);
                      await Task.Delay(2000);

                      logger.LogWarning($"是否检查刀具寿命功能 ： {checkToolLifeOnOff} ");
                      if (checkToolLifeOnOff && !CheckToolLife())                                       //CheckToolLifeOnOff 是否检查刀具寿命功能
                      {
                          logger.LogWarning($"检查刀具寿命功能 ： 不满足当趟需求 ");
                          WriteWarningToPlc(302);
                          return;
                      }
                      InteractingDevice.cnc84Command.SetCncComand($"DSP,8/8_Drilling");
                      SendMessageToShow(8);
                      await Task.Delay(2000);

                      logger.LogWarning($"是否自动开启打板 ： {drillBoardOnOff} ");

                      if (drillBoardOnOff)                                                              //DrillBoardOnOff 是否自动开启打板
                      {
                          /*scCncOEMBoardDirectionCheck//检查板方向，开旗标让CNC95的脚本去做
                           *
                           * int i= showDialog(DIALOG_QUESTION,"Please check the direction of the board!","Tip");
                           *
                           * Customer.BoardDirectionCheck=i ;
                           *
                           * return;
                           *
                           * IIoT->Customer.BoardDirectionCheck->Int64
                          */
                          /*
                          if (checkBoardDirectionOnOff)//2024-06-08zhushipeng 开启确认大小板方向功能（金禄、联锦成），开旗标让CNC95的脚本去做
                          {
                              InteractingDevice.cnc84Command.SetCncComand($"SCRP,scCncOEMBoardDirectionCheck");
                              while (true)
                              {
                                  Int64 boardDirectionCheck = InteractingDevice.cnc84Command.ReadCncNode<Int64>("ns=4;s=UI/normalized/custom/Customer.BoardDirectionCheck");
                                  //OK_BUTTON       1024        The button “OK” was used.
                                  //YES_BUTTON      16384       The button “Yes” was used.
                                  //NO_BUTTON       65536       The button “No” was used
                                  //CANCEL_BUTTON   4194304     The button “Cancel” was used.
                                  if (16384 == boardDirectionCheck)//YES_BUTTON
                                  {
                                      break;
                                  }
                                  if (65536 == boardDirectionCheck)//NO_BUTTON
                                  {
                                      break;
                                  }
                                  await Task.Delay(2000);
                              }
                              InteractingDevice.cnc84Command.WriteCncNode<Int64>("ns=4;s=UI/normalized/custom/Customer.BoardDirectionCheck", 0);
                          }
                          */
                          StartDrillBoard();
                          if (!ValidateStartCommandEnd())
                          {
                              bool flag = CheckStartStatusAndRestart();
                              if (!flag)
                              {
                                  logger.LogWarning($"打板未能正常启动 ");
                                  WriteWarningToPlc(200);
                                  logger.LogWarning($"钻机给buffer报警编号：200");
                                  return;
                              }
                          }
                          logger.LogWarning($"钻机已开始打板");
                          InteractingDevice.cnc84Command.SetCncComand($"DSP,Drilling");
                      }
                      else
                      {
                          InteractingDevice.cnc84Command.SetCncComand($"SDSP,Manual_Start_Drilling_The_Board");
                      }
                      await Task.CompletedTask;
                  }
                  catch (Exception ee)
                  {
                      errorNum = 5;
                      logger.LogError($"buffer给钻机上完板子后续动作中发生异常 ： {ee.Message} ");
                      try
                      {
                          Thread.Sleep(500);
                          InteractingDevice.cnc84Command.SetCncComand($"DSP,Exception_Manual_Start_Drilling_The_Board");
                      }
                      catch (Exception e)
                      {
                          logger.LogError($"DSP,Exception_Manual_Start_Drilling_The_Board异常 ： {e.Message} ");
                      }

                      await Task.CompletedTask;
                  }
                  finally
                  {
                      SendMessageToShow(9, false);
                      InteractingDevice.isOnDoingScript = false;
                      logger.LogError($"buffer给钻机上完板子后续动作异常动作编码 ： {errorNum} ");
                      if (errorNum != 0)
                      {
                          try
                          {
                              InteractingDevice.cnc84Command.SetCncComand($"SDSP,{GetErrorMessage(errorNum)}");
                          }
                          catch (Exception)
                          {
                              logger.LogError($"buffer给钻机上完板子后续动作异常动作SDSP 错误");
                          }
                      }
                  }
              });

            //2025-02-13 //统计时间
            //钻机上的板子从0边成无的时间
            WatchingProperties.Property("Drill_BoardPositionStatusStatistics")
              .PostCondition(p => p.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  try
                  {
                      var boardPositionStatus = WatchingProperties.Property("Drill_BoardPositionStatusStatistics");
                      logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatusStatistics  上生料到钻机 钻机上板子状态从{boardPositionStatus.OldValue.ToInt()}变成 {boardPositionStatus.NewValue.ToInt()} ");
                      var oldValue = boardPositionStatus.OldValue.ToInt();
                      var NewValue = boardPositionStatus.NewValue.ToInt();
                      if (oldValue != -1)
                      {
                          if (NewValue == 0)
                          {
                              logger.LogDebug($"统计时间  更新的属性名字  Drill_ChangeNoBoardTime");
                              WatchingProperties.Property("Drill_ChangeNoBoardTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                          }
                          else if (oldValue == 0 && NewValue != 0)
                          {
                              logger.LogDebug($"统计时间 更新的属性名字  Drill_ChangeExistBoardTime");
                              WatchingProperties.Property("Drill_ChangeExistBoardTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                          }
                      }
                  }
                  catch (Exception)
                  {
                      await Task.CompletedTask;
                  }
                  await Task.CompletedTask;
              });
            //buffer 熟料层板材信息
            WatchingProperties.Property("Buffer_ClinkerLayerBoardStatusStatistics")
            .PostCondition(p => p.IsValueChanged)
            .TriggerAlways(async () =>
            {
                try
                {
                    var clinkerLayerBoardStatus = WatchingProperties.Property("Buffer_ClinkerLayerBoardStatusStatistics");
                    logger.LogDebug($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatusStatistics  {clinkerLayerBoardStatus.OldValue.ToInt()} 变成 {clinkerLayerBoardStatus.NewValue.ToInt()} ");
                    var oldValue = clinkerLayerBoardStatus.OldValue.ToInt();
                    var NewValue = clinkerLayerBoardStatus.NewValue.ToInt();
                    if (oldValue == 0 && NewValue != 0)
                    {
                        logger.LogDebug($"统计时间 熟料变成有板子  ");
                        WatchingProperties.Property("Buffer_ClinkerChangeExistTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else if (oldValue != 0 && NewValue == 0)
                    {
                        logger.LogDebug($"统计时间 熟料变成无板子  ");
                        WatchingProperties.Property("Buffer_ClinkerChangeNoBoardTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
                catch (Exception)
                {
                    await Task.CompletedTask;
                }
                await Task.CompletedTask;
            });
            //buffer 生料层板材信息

            WatchingProperties.Property("Buffer_RawMaterialLayerBoardStatus")
           .PostCondition(p => p.IsValueChanged)
           .TriggerAlways(async () =>
           {
               try
               {
                   var rawMaterialLayerBoardStatus = WatchingProperties.Property("Buffer_RawMaterialLayerBoardStatus");
                   logger.LogDebug($"{DateTime.Now.ToShortTimeString()} rawMaterialLayerBoardStatus  {rawMaterialLayerBoardStatus.OldValue.ToInt()} 变成 {rawMaterialLayerBoardStatus.NewValue.ToInt()} ");
                   var oldValue = rawMaterialLayerBoardStatus.OldValue.ToInt();
                   var NewValue = rawMaterialLayerBoardStatus.NewValue.ToInt();
                   if (oldValue != 0 && NewValue == 0)
                   {
                       logger.LogDebug($"统计时间 生料变成无板子");
                       WatchingProperties.Property("Buffer_RawChangeNoBoardTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                   }
                   if (oldValue == 0 && NewValue != 0)
                   {
                       logger.LogDebug($"统计时间 生料变成无板子");
                       WatchingProperties.Property("Buffer_RawChangeExistBoardTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                   }
               }
               catch (Exception)
               {
                   await Task.CompletedTask;
               }
               await Task.CompletedTask;
           });
            //Buffer_AllUnloadAndLoadEnd 整个上下料时间
            WatchingProperties.Property("Buffer_AllUnloadAndLoadEnd")
          .PostCondition(p => p.IsValueChanged)
          .TriggerAlways(async () =>
          {
              try
              {
                  var allUnloadAndLoadEnd = WatchingProperties.Property("Buffer_AllUnloadAndLoadEnd");
                  if (string.IsNullOrWhiteSpace(allUnloadAndLoadEnd.OldValue.ToStr()))
                  {
                      return;
                  }
                  logger.LogDebug($"{DateTime.Now.ToShortTimeString()} allUnloadAndLoadEnd  {allUnloadAndLoadEnd.OldValue.ToInt()} 变成 {allUnloadAndLoadEnd.NewValue.ToInt()} ");
                  var oldValue = allUnloadAndLoadEnd.OldValue.ToInt();
                  var NewValue = allUnloadAndLoadEnd.NewValue.ToInt();
                  if (oldValue != 0 && NewValue == 1)
                  {
                      logger.LogDebug($"统计时间 生料变成无板子");
                      WatchingProperties.Property("Buffer_RawChangeNoBoardTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                  }
                  if (oldValue == 0 && NewValue != 0)
                  {
                      logger.LogDebug($"统计时间 生料变成无板子");
                      WatchingProperties.Property("Buffer_RawChangeExistBoardTime").SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                  }
              }
              catch (Exception)
              {
                  await Task.CompletedTask;
              }
              await Task.CompletedTask;
          });

            WatchingProperties.Property("Drill_RunState")
             .PostCondition(p => p.IsValueChanged && p.NewValue.ToInt() == 3)
              .TriggerAlways(async () =>
               {
                   logger.LogInformation($"{DateTime.Now.ToShortTimeString()}  程序开始 清空文本提示");
                   try
                   {
                       InteractingDevice.cnc84Command.SetCncComand($"DSP,");
                   }
                   catch (Exception ee)
                   {
                       logger.LogDebug($"{DateTime.Now.ToShortTimeString()} 清空异常  {ee} ");
                   }
               });

            WatchingProperties.Property("Drill_NoError")
             .PostCondition(p => p.IsValueChanged)
             .TriggerAlways(async () =>
             {
                 var drillNoError = WatchingProperties.Property("Drill_NoError").NewValue.ToBool();
                 var prop = drillNoError ? "Drill_ErrorEndTime" : "Drill_ErrorStartTime";
                 logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  钻机是否报警 {!drillNoError} 更新属性名 {prop} ");
                 WatchingProperties.Property(prop).SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
             });

            WatchingProperties.Property("Drill_ProgramState")
             .PostCondition(p => p.IsValueChanged)
             .TriggerAlways(() =>
             {
                 try
                 {
                     var programStatePro = WatchingProperties.Property("Drill_ProgramState");
                     var programState = programStatePro.NewValue.ToInt();
                     logger.LogDebug($"{DateTime.Now} Drill_ProgramState  旧值{programStatePro.OldValue.ToInt()}   新值{programState}  ");

                     if (programState == 5)
                     {
                         logger.LogDebug($"{DateTime.Now} Drill_ProgramState  {programState} 进入程序结束交互数据  ");
                         // 1.钻机itemcode
                         var panel = InteractingDevice.PayloadPanels.Where(s => s.Layer == 1).FirstOrDefault(s => !string.IsNullOrEmpty(s.ItemCode));
                         var ItemCode = panel == null ? $"null" : panel.ItemCode;
                         //本趟钻孔数量
                         var runDrillHits = WatchingProperties.Property("Drill_RunDrillHits").NewValue.ToInt();
                         //本趟钻孔时间
                         var unProductionDuration = WatchingProperties.Property("Drill_RunProductionDuration").NewValue.ToDouble();
                         //本趟报警时间
                         var runErrorDuration = WatchingProperties.Property("Drill_RunErrorDuration").NewValue.ToDouble();
                         //生产趟数
                         string content = string.Join(",", ItemCode, runDrillHits, unProductionDuration, runErrorDuration, ++RunNum);
                         logger.LogDebug($"Drill_ProgramState    单趟运行数据 ： {content} ");
                         if (InteractingDevice.ApplicationServices.TryGetService<IDrillWriteRunInformation>(out IDrillWriteRunInformation? runInformation))
                         {
                             runInformation?.WriteRunInformation(content);
                         }
                     }
                     else if (programState == 2)
                     {
                         bufferToDrillNum = 0;
                         drillToBufferNum = 0;
                         logger.LogDebug($"Drill_ProgramState {programState}   修改buffer给钻机上料板材流转 bufferToDrillNum{bufferToDrillNum} drillToBufferNum {drillToBufferNum}");
                     }
                 }
                 catch (Exception ee)
                 {
                     logger.LogDebug($"Drill_ProgramState    异常： {ee.Message} ");
                 }
             });

            //
            WatchingProperties.Property("Buffer_PreControlMushroom")
           .PostCondition(p => p.IsValueChanged && p.NewValue.ToInt() == 1)
           .TriggerAlways(() =>
           {
               ushort result = 0;
               try
               {
                   //触发动作

                   if (StartPreOpenMushroomControllerNew())
                   {
                       logger.LogDebug($"Buffer_PreControlMushroom 结束  验证成功修改result");
                       result = 1;
                   }
               }
               catch (Exception ee)
               {
                   logger.LogDebug($"Buffer_PreControlMushroom    异常： {ee.Message} ");
                   result = 2;
               }
               finally
               {
                   logger.LogDebug($"Buffer_PreControlMushroom 结束  写到plc中的数据 {result}");
                   try
                   {
                       InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["PreControlMushroomResult"].ToUshort(), result);//406-1
                   }
                   catch (Exception)
                   {
                   }
               }
           });

            //Drill_LoadFileFromCncUI 监控cnc95界面
            WatchingProperties.Property("Drill_LoadFileFromCncUI")
         .PostCondition(p => p.IsValueChanged && p.NewValue.ToBool())
         .TriggerAlways(async () =>
         {
             logger.LogDebug($"Drill_LoadFileFromCncUI 开始了");
             try
             {
                 // 添加相关的判断 是都能加载程序
                 var progState = InteractingDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/WorkGroupSettings/WorkGroupInterface/TimeWidget/ProgramState");
                 logger.LogDebug($"提前动蘑菇头 Drill_ProgramState  {progState}");
                 if (new int[] { 2, 3, 4 }.Contains(progState))
                 {
                     InteractingDevice.cnc84Command.SetCncComand($"DSP,should_not_allow_load_file");
                     return;
                 }
                 var result = await LoadScriptFromCenter();
                 logger.LogDebug($"Drill_LoadFileFromCncUI 结束  加载文件失败 {result == 2}");
             }
             catch (Exception ee)
             {
                 logger.LogDebug($"Drill_LoadFileFromCncUI    异常： {ee.Message} ");
             }
             finally
             {
                 try
                 {
                     InteractingDevice.cnc84Command.WriteCncNode<bool>($"ns=4;s=UI/normalized/custom/{DeviceDescriptor.Extra["LoadFileFromCncUIFlag"]}", false);
                     logger.LogDebug($"Drill_LoadFileFromCncUI   清空iiot点位 完成");
                 }
                 catch (Exception ee)
                 {
                     logger.LogDebug($"Drill_LoadFileFromCncUI   清空iiot点位： {ee.Message} ");
                 }
                 logger.LogDebug($"Drill_LoadFileFromCncUI 结束  ");
             }
         });
        }

        private async Task<int> OptimizeActions()
        {
            var errorNum = 0;
            SendMessageToShow(0);
            InteractingDevice.cnc84Command.SetCncComand($"DSP,进入自动循环");
            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  钻机和buffer整个上下料结束");
            await Task.Delay(1000);
            SendMessageToShow(1);
            InteractingDevice.cnc84Command.SetCncComand($"DSP,取消buffe不在安全位置报警");
            await Task.Delay(1000);
            if (!ValidateEscEnd())
            {
                WriteWarningToPlc(19);
                logger.LogWarning($"钻机给buffer报警编号：19");                                  //等待ESC,持续2分钟,超时给PLC报19代码
                errorNum = 1;
                return errorNum;
            }

            if (selectSplineOnOff)
            {
                SendMessageToShow(2);
                InteractingDevice.cnc84Command.SetCncComand($"DSP,选轴中");
                await Task.Delay(1000);
                logger.LogWarning($"是否开启选轴功能 ： {selectSplineOnOff} ");                      //SelectSplineOnOff 是否开启选轴功能
                                                                                            //退刀
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
                    return 7;
                }

                Thread.Sleep(1000);

                var cncStatus = InteractingDevice.cnc84Command.GetCncStatus();
                var splineStatus = cncStatus?.SpindleStatus;
                if (splineStatus != null)
                {
                    splineStatus = splineStatus?.Substring(splineStatus.Length - InteractingDevice.spindleNum);
                    char[] arr = splineStatus.ToCharArray();
                    Array.Reverse(arr);

                    var realSplindleStatus = string.Join("", arr);
                    var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);
                    var rawStatus = string.Join("", Convert.ToString(ready[0], 2).PadLeft(InteractingDevice.spindleNum, '0').Reverse().ToList());
                    logger.LogError($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")} 获取的轴状态{realSplindleStatus}  获取的{rawStatus} 选轴 ct时间 发送结束");
                    if (!string.Equals(rawStatus, realSplindleStatus))
                    {
                        StartSelectSpline();
                    }
                }
                else
                {
                    StartSelectSpline();
                }
            }

            if (DeviceDescriptor.AutoMode && loadFileFromCentreOnOff)
            {
                var result = await LoadScriptFromCenter();
                if (result == 2)
                {
                    errorNum = 2;
                    return errorNum;
                }
            }

            logger.LogWarning($"是否开启压板功能 ： {pressBoardOnOff} ");                        //PressBoardOnOff 是否开启压板功能
            if (pressBoardOnOff)
            {
                InteractingDevice.cnc84Command.SetCncComand($"DSP,压板中");
                SendMessageToShow(4);
                await Task.Delay(1000);
                if (!StartPressBoard())
                {
                    WriteWarningToPlc(12);
                    logger.LogDebug($"钻机给buffer报警编号：12");
                    errorNum = 3;
                    return errorNum;
                }
            }

            logger.LogWarning($"是否开启蘑菇头功能 ： 配置{mushroomOnOff} 实际{InteractingDevice.mushroomValue} ");                         //MushroomOnOff 是否开启蘑菇头功能
            if (InteractingDevice.mushroomValue)
            {
                InteractingDevice.cnc84Command.SetCncComand($"DSP,蘑菇头动作中");
                SendMessageToShow(5);
                await Task.Delay(1000);
                if (!StartOpenMushroomControllerNew())
                {
                    logger.LogWarning($"开始选蘑菇头");
                    WriteWarningToPlc(9);
                    logger.LogWarning($"钻机给buffer报警编号：9");
                    errorNum = 4;
                    return errorNum;
                }
            }

            if (drillBoardOnOff)                                                              //DrillBoardOnOff 是否自动开启打板
            {
                InteractingDevice.cnc84Command.SetCncComand($"DSP,打板");
                SendMessageToShow(8);
                await Task.Delay(500);

                logger.LogWarning($"是否自动开启打板 ： {drillBoardOnOff} ");
                StartDrillBoard();
                if (!ValidateStartCommandEnd())
                {
                    bool flag = CheckStartStatusAndRestart();
                    if (!flag)
                    {
                        logger.LogWarning($"打板未能正常启动 ");
                        WriteWarningToPlc(200);
                        logger.LogWarning($"钻机给buffer报警编号：200");
                        errorNum = 5;
                        return errorNum;
                    }
                }
                logger.LogWarning($"钻机已开始打板");
                InteractingDevice.cnc84Command.SetCncComand($"DSP,加工中");
            }
            else
            {
                await Task.Delay(1000);
                InteractingDevice.cnc84Command.SetCncComand($"DSP,");
                await Task.Delay(1000);
                InteractingDevice.cnc84Command.SetCncComand($"SDSP,手动开始打板");
            }

            return errorNum;
        }

        private async Task<int> LoadScriptFromCenter()
        {
            logger.LogWarning($"是否向中控要加载程序和参数 cnc95ui ： {loadFileFromCentreOnOff}  自动模式 {DeviceDescriptor.AutoMode} ");
            InteractingDevice.cnc84Command.SetCncComand($"DSP,load_file_begin");
            SendMessageToShow(3);
            await Task.Delay(200);

            string result = "0";
            try
            {
                var panel = InteractingDevice.PayloadPanels.Skip(InteractingDevice.spindleNum).Take(InteractingDevice.spindleNum).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s.ItemCode));
                var itemCode = string.Empty; if (panel != null) { itemCode = panel.ItemCode; }
                if (string.IsNullOrEmpty(itemCode))
                {
                    logger.LogError($"itemCode 是空 不能向中控发起呼叫获取工艺分组 cnc95ui");
                    InteractingDevice.cnc84Command.SetCncComand($"DSP,ITEMCODE_IS_NULL");
                    result = "2";
                    WriteWarningToPlc(7);
                    return 2;
                }

                var url = string.Format(DeviceDescriptor.Extra["GetGroupCode"].ToStr(), DeviceDescriptor.DeviceId, itemCode);
                var group = await HttpRequestInvoker.GetFromJsonAsync<Dictionary<string, object?>>(url);
                if (group == null || group.Count == 0)
                {
                    logger.LogError($"向中控请求获取工艺分组 {url} 异常 cnc95ui");
                    InteractingDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_GET_GETGROUPCODE_URL");
                    result = "3";
                    WriteWarningToPlc(7);
                    return 2;
                }
                logger.LogDebug($"向中控请求{url}的返回值为{JsonSerializer.Serialize(group)} cnc95ui ");
                if (!group.ContainsKey("SpecGroup") || string.IsNullOrEmpty(group["SpecGroup"].ToString()))
                {
                    logger.LogError($"向中控请求{url} 返回值中 不包括 SpecGroup 或工艺分组为空 cnc95ui");
                    InteractingDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_NOT_CONTAINSKEY_SPECGROUP");
                    result = "4";
                    WriteWarningToPlc(7);
                    return 2;
                }
                var groupStr = group["SpecGroup"].ToString();
                logger.LogDebug($"向中控请求的工艺分组为{groupStr} cnc95ui");
                InteractingDevice.cnc84Command.SetCncComand($"DSP,Get_data_from_database");
                DrillInfo drillInf;
                try
                {
                    drillInf = InteractingDevice.DrillFilePathLocator.GetFilePath($"{itemCode};{groupStr}");
                }
                catch (Exception ee)
                {
                    if (!ee.Message.StartsWith("EAP_MATERIAL_DATA"))
                    {
                        InteractingDevice.cnc84Command.SetCncComand($"DSP,3/8_DATABASE_EXCEPTION");
                    }
                    else
                    {
                        InteractingDevice.cnc84Command.SetCncComand($"DSP,{ee.Message}");
                    }
                    logger.LogError($"向中控请求的工艺分组为{groupStr} 和数据库交互失败 {ee.Message} cnc95ui");
                    result = "5";
                    WriteWarningToPlc(7);
                    return 2;
                }
                logger.LogError($"数据库交互获取数据 {JsonSerializer.Serialize(drillInf)} cnc95ui");
                if (string.IsNullOrWhiteSpace(drillInf.DrlPath))
                {
                    logger.LogError("和数据库交互获取钻带文件不存在 cnc95ui");
                    InteractingDevice.cnc84Command.SetCncComand($"DSP, DRILL_PATH_NOT_FIND");
                    result = "6";
                    WriteWarningToPlc(7);
                    return 2;
                }
                InteractingDevice.cnc84Command.SetCncComand($"DSP,Get_data_from_center");
                //向中控要转化后的路径
                var drillPathUrl = string.Format(DeviceDescriptor.Extra["GetAfterDrillPath"].ToStr(), DeviceDescriptor.DeviceId, drillInf.DrlPath, itemCode);
                var drillPathResult = await HttpRequestInvoker.GetFromJsonAsync<Dictionary<string, object?>>(drillPathUrl);
                if (drillPathResult == null)
                {
                    logger.LogError($"向中控请求获取转化后钻带程序 {drillPathUrl} 异常 cnc95ui");
                    InteractingDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_GET_AFTER_DRILL_PATH_URL");
                    result = "7";
                    WriteWarningToPlc(7);
                    return 2;
                }
                logger.LogError($"向中控请求转化后{drillPathUrl}的返回值为{JsonSerializer.Serialize(drillPathResult)} cnc95ui");
                if (!drillPathResult.ContainsKey("AfterDrillPath") || string.IsNullOrEmpty(drillPathResult["AfterDrillPath"].ToString()))
                {
                    logger.LogError($"向中控请求{drillPathUrl} 返回值中 不包括 AfterDrillPath 或 AfterDrillPath为空  cnc95ui");
                    InteractingDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_NOT_CONTAINSKEY_AFTERDRILLPATH");
                    result = "8";
                    WriteWarningToPlc(7);
                    return 2;
                }

                string afterDrillPath = drillPathResult["AfterDrillPath"].ToStr();
                logger.LogDebug($"是否转化 AfterDrillPath开关 {drillChangeAfterPathOnOff},A轴是否工作整个台面 {aSplineIsWorkFullTable} 中控下发AfterDrillPath路径{afterDrillPath} cnc95ui");
                if (drillChangeAfterPathOnOff)
                {
                    string tempafterDrillPath = ModifyAfterPath(afterDrillPath);
                    afterDrillPath = tempafterDrillPath;
                }
                string filename = Path.GetFileNameWithoutExtension(afterDrillPath).ToLower();
                if (validateFileAppendOnOff && !ValidateFileAppendContent(filename))
                {
                    logger.LogError($"配方路径文件不是该机台用的钻带 cnc95ui");
                    InteractingDevice.cnc84Command.SetCncComand($"DSP, DRILL_PATH_NOT_MATCH_TABLE_SIZE");
                    result = "9";
                    WriteWarningToPlc(7);
                    return 2;
                }
                string drillPath = afterDrillPath;
                string diaPath = drillInf.DiaPath;

                if (!LoadFileToCNC84(drillPath, diaPath))
                {
                    logger.LogDebug($"加载程序到CNC84 失败 cnc95ui");
                    InteractingDevice.cnc84Command.SetCncComand($"DSP, LOAD_DRILL_FAIL");
                    result = "11";
                    WriteWarningToPlc(7);
                    return 2;
                }
                result = "1";
                return 0;
            }
            catch (Exception ee)
            {
                logger.LogDebug($"和中控交互异常{ee.Message} cnc95ui");
                InteractingDevice.cnc84Command.SetCncComand($"DSP, LOAD_FILE_FROM_CENTRE_FAIL");
                result = "12";
                WriteWarningToPlc(7);
                return 2;
            }
            finally
            {
                if (result != "1")
                {
                    result = "2";
                }
                //通知eap
                if (InteractingDevice.ApplicationServices.TryGetService<IDrillFileLoadResult>(out IDrillFileLoadResult fileLoadResult))
                {
                    fileLoadResult?.WriteFileLoadResult(result);
                }
            }
        }

        private async Task<int> CallOneScript()
        {
            var errorNum = 0;
            InteractingDevice.cnc84Command.SetCncComand($"DSP,New_Loop_CNC_Buffer_Load_UnLoad_Begin");
            await Task.Delay(1000);
            DrillOprationTime = DateTime.Now;
            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  钻机和buffer整个上下料结束");
            if (!ValidateEscEnd())
            {
                WriteWarningToPlc(19);
                logger.LogWarning($"钻机给buffer报警编号：19");                                  //等待ESC,持续2分钟,超时给PLC报19代码
                errorNum = 1;
                return errorNum;
            }
            Int64 drillBoardStatus = GetDrillBoardStatus();
            logger.LogDebug($"CallOneScript 给钻机的轴状态是{drillBoardStatus}");
            InteractingDevice.cnc84Command.WriteCncNode<Int64>("ns=4;s=UI/normalized/custom/STATIONALLOPENCLOSE", drillBoardStatus);
            Thread.Sleep(2000);
            //检查状态
            //调用脚本
            if (!ValidateScriptsAutoRunStart())
            {
                Thread.Sleep(1000);
                InteractingDevice.cnc84Command.SetCncComand($"DSP,scKbiAutoRun_Error");
                errorNum = 6;
            }

            return errorNum;
        }

        private bool ValidateScriptsAutoRunStart()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                logger.LogDebug("ValidateScriptsAutoRunStart 调用 SCRP,scKbiAutoRun 开始");
                InteractingDevice.cnc84Command.SendCncComand("SCRP,scKbiAutoRun");
                Thread.Sleep(1000);
                var autoRunEnd = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.AutoRunEnd");
                logger.LogDebug($"ValidateScriptsAutoRunStart  检查Customer.AutoRunEnd 结束状态{autoRunEnd}  开始状态{!autoRunEnd}");
                if (!autoRunEnd) return true;
                Thread.Sleep(1000);
                autoRunEnd = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.AutoRunEnd");
                logger.LogDebug($"ValidateScriptsAutoRunStart  二次判断检查Customer.AutoRunEnd 结束状态{autoRunEnd}  开始状态{!autoRunEnd}");
                if (!autoRunEnd) return true;
            } while (stopwatch.ElapsedMilliseconds < 7000);
            return false;
        }

        private bool ValidateFileAppendContent(string filename)
        {
            if (aSplineIsWorkFullTable)
            {
                return filename.EndsWith(drillFullTableAppendContent);
            }
            else
            {
                var content = drillHalfTableAppendContent.Split(",", StringSplitOptions.RemoveEmptyEntries);
                return content.Any(s => filename.EndsWith(s));
            }
        }

        private string ModifyAfterPath(string afterDrillPath)
        {
            string directoryName = Path.GetDirectoryName(afterDrillPath);
            string fileName = Path.GetFileNameWithoutExtension(afterDrillPath);
            string extension = Path.GetExtension(afterDrillPath);
            var appendContent = aSplineIsWorkFullTable ? "N" : "O";
            var tempafterDrillPath = Path.Combine(directoryName, $"{fileName}{appendContent}{extension}");
            logger.LogDebug($"转化后的路径修追加全台面文件类型  {tempafterDrillPath}");
            if (!aSplineIsWorkFullTable && !File.Exists(tempafterDrillPath))
            {
                tempafterDrillPath = afterDrillPath;
                logger.LogDebug($"2/3台面 o后缀文件不存在 更换成原来下发的  {tempafterDrillPath}");
            }
            logger.LogDebug($"最总使用的文件是 ：  {tempafterDrillPath}");
            return tempafterDrillPath;
        }

        [DllImport("user32.dll")]
        public static extern void keybd_event(int bVk, byte bScan, int dwFlags, int dwExtraInfo);

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

        private bool GetDrillInfFromCentre(string incodeNumber, string itemCode, out string drillPath, out string diaPath, out string panelLength)
        {
            drillPath = string.Empty;
            diaPath = string.Empty;
            panelLength = string.Empty;
            if (DeviceDescriptor.AutoMode)
            {
                var response = DataExporter.DeviceEventReport(
                          new DeviceEventReportRequest()
                          {
                              ProductId = DeviceDescriptor.ProductId,
                              DeviceId = DeviceDescriptor.DeviceId,
                              ClientId = InteractingDevice.ClientId,
                              EventId = Events.Drill.DRILL_REQUEST_RECIPE_EVENT,
                              EventName = Events.Drill.DRILL_REQUEST_RECIPE_EVENT_NAME,
                              RequestDeviceKind = DeviceKind.CNC84Drill,
                              Params = new Dictionary<string, object?>() { { "IncodeNumber", incodeNumber }, { "ItemCode", itemCode } },
                              PayloadPanels = InteractingDevice.PayloadPanels
                          }).GetAwaiter().GetResult();
                if (response == null)
                {
                    logger.LogDebug(response?.Message);
                    WriteWarningToPlc(306);
                    return false;
                }
                else if (response?.Code != ErrorCodes.Sys.SUCCESS)
                {
                    logger.LogDebug(response?.Message);
                    WriteWarningToPlc(307);
                    return false;
                }

                if (cnc84DiaFileOnOff)
                {
                    if (response.Params == null || !response.Params.ContainsKey("DrlPath") || !response.Params.ContainsKey("DiaPath") || !response.Params.ContainsKey("PanelLength"))
                    {
                        logger.LogError("未下发配方路径");
                        WriteWarningToPlc(308);
                        return false;
                    }
                    diaPath = response.Params["DiaPath"].ToStr();
                }
                else
                {
                    if (response.Params == null || !response.Params.ContainsKey("DrlPath") || !response.Params.ContainsKey("PanelLength"))
                    {
                        logger.LogError("未下发配方路径");
                        WriteWarningToPlc(308);
                        return false;
                    }
                }
                drillPath = response.Params["DrlPath"].ToStr();
                panelLength = response.Params["PanelLength"].ToStr();

                return true;
            }
            return false;
        }

        //private void LoadProgram(string filePath, OpcUaClient opcuaClient)
        //{
        //    try
        //    {
        //        opcuaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", new object[] { "DSP," });
        //        opcuaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", new object[] { "DSP,THE_PART_PROGRAM_CLEARING_IN_PROGRESS" });
        //        Thread.Sleep(1000);
        //        object[] objs = { "CM@@@" };
        //        object[] res = opcuaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", objs);
        //        opcuaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", new object[] { "DSP," });
        //        Thread.Sleep(1000);
        //        opcuaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", new object[] { "DSP,THE_PART_PROGRAM_IS_LOADING" });
        //        Thread.Sleep(2000);
        //        object[] objects = { filePath, "PROGRAM" };
        //        //object[] objects = { filePath, "DIAMETERTABLE" };
        //        object[] results = opcuaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_LOAD", objects);
        //        if (results != null)
        //        {
        //            if (results.Length > 0)
        //            {
        //                log.Information("RPC_LOAD:" + filePath + "结果:" + results[0].ToString());
        //                if (results[0].ToString() == "success" || results[0].ToString() == "true")
        //                {
        //                    //count表的数量修改为0
        //                    Update_PC_DATA_COUNT(true);
        //                    Upload_PC_SIGNAL_STATUS("MATERIALOK", "1");
        //                    opcuaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", new object[] { "DSP," });
        //                    Thread.Sleep(1000);
        //                    opcuaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", new object[] { "DSP,THE_PART_PROGRAM_LOAD_SUCCESS" });
        //                    //MessageBox.Show("下发成功，请至CNC95确认下发钻带程序。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    // dgvFiles.Rows.RemoveAt(e.RowIndex);
        //                    dgvFiles.Rows.Clear();
        //                    opcuaClient.Disconnect();
        //                }
        //                else
        //                {
        //                    MATERIALOKErr = "连接CNC失败，pgm加载失败";
        //                    Upload_PC_SIGNAL_STATUS("MATERIALOK", "2");
        //                    opcuaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", new object[] { "DSP," });
        //                    Thread.Sleep(1000);
        //                    opcuaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", new object[] { "DSP,THE_PART_PROGRAM_LOAD_FAIL" });
        //                    MessageBox.Show("钻带程序下发失败，请检查CNC95状态是否正确？", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //            }
        //            else
        //            {
        //                MATERIALOKErr = "连接CNC失败，pgm加载失败";
        //                Upload_PC_SIGNAL_STATUS("MATERIALOK", "2");
        //                MessageBox.Show("调用方法(LoadProgram)异常", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //        else
        //        {
        //            MATERIALOKErr = "连接CNC失败，pgm加载失败";
        //            Upload_PC_SIGNAL_STATUS("MATERIALOK", "2");
        //            MessageBox.Show("CNC95  Connect Error", "(LoadProgram)错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string err = ex.Message;
        //    }

        //}
        private bool LoadFileToCNC84(string drillPath, string diaPath = "", string panelLength = "", string atpPath = "")
        {
            InteractingDevice.cnc84Command.SetCncComand($"DSP, LOADING_FILE");
            Thread.Sleep(1000);
            InteractingDevice.LoadFileFlag = false;
            if (!LoadFile(drillPath, diaPath, atpPath))
            {
                WriteWarningToPlc(11);
                return false;
            }
            //else
            //{
            //    //获取板材标记
            //    //获取参数
            //    var realLength = GetBSIZLength(Convert.ToDouble(panelLength));
            //    InteractingDevice.cnc84Command.SetCncComand($"BSIZ{realLength.ToString("0.000")}");

            //    //设置小板用户标记
            //    if (InteractingDevice.middleMushroomExist)
            //    {
            //        double smallBoardMaxLength = DeviceDescriptor.Extra["SmallBoardMaxLength"].ToDouble();

            //        if (realLength < smallBoardMaxLength)
            //        {
            //            InteractingDevice.cnc84Command.SetUserFlag(1, DeviceDescriptor.Extra["SmallBoardUserFlag"].ToInt());
            //            logger.LogDebug($"设置小板用户标记 ");
            //        }
            //        else
            //        {
            //            InteractingDevice.cnc84Command.SetUserFlag(0, DeviceDescriptor.Extra["SmallBoardUserFlag"].ToInt());
            //            logger.LogDebug($"取消小板用户标记 ");
            //        }
            //    }
            //    logger.LogDebug($"扫码枪验证文件后加载文件 程序成功加载标识");

            //}
            InteractingDevice.LoadFileFlag = true;
            return true;
        }

        private bool CheckToolLife()
        {
            int zeroCount = 0;
            for (int i = 0; i < 300; i++)
            {
                if (zeroCount >= 5)
                {
                    break;
                }
                var toolD = InteractingDevice.cnc84Command.GetRuntimeValue($"TParD({i})");
                if (string.IsNullOrEmpty(toolD) || toolD == "0")
                {
                    zeroCount++;
                    continue;
                }
                zeroCount = 0;
                var toolMatch = InteractingDevice.cnc84Command.GetRuntimeValue($"JOBS_SUFFICIENT({i})");
                if (!string.IsNullOrEmpty(toolMatch) && toolMatch == "0")
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> QueryScheduleResetCallAgv()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InteractingDevice.NewTranscationIdTemp))
                {
                    return true;
                }
                if (DeviceDescriptor.AutoMode)
                {
                    var response = await HttpRequestInvoker.GetFromJsonAsync<ScheduledTaskStatus>(string.Format(InteractingDevice.QueryScheduleUrl, InteractingDevice.NewTranscationIdTemp));
                    logger.LogDebug($"QuerySchedule, {JsonSerializer.Serialize(response)}");
                    if (response == ScheduledTaskStatus.Completed || response == ScheduledTaskStatus.Failed || response == ScheduledTaskStatus.Canceled)
                    {
                        return true;
                    }
                    return false;
                }
                else //单机模式直接返回true
                    return true;
            }
            catch (Exception ex)
            {
                logger.LogDebug($"QuerySchedule 异常, {JsonSerializer.Serialize(ex.Message)}");
                return true;
            }
        }

        private async Task ReportStartInf()
        {
            var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);
            var count = Convert.ToString(ready[0], 2).PadLeft(InteractingDevice.spindleNum, '0').Count(c => c == '1');
            logger.LogDebug($"ReportStartInf： 钻机上板子情况  {ready[0]} 个数{count}  ");

            var res = InteractingDevice.DrillTransactionId == null ? "DrillTransactionId为空" : InteractingDevice.DrillTransactionId;
            logger.LogDebug($"{DateTime.Now.ToShortTimeString()} ,DeviceDescriptor.AutoMode= {DeviceDescriptor.AutoMode},InteractingDevice.DrillTransactionId={res} ");

            if (DeviceDescriptor.AutoMode)
            {

                var panel = InteractingDevice.PayloadPanels.Skip(InteractingDevice.spindleNum).Take(InteractingDevice.spindleNum).FirstOrDefault(s => !string.IsNullOrEmpty(s.TaskCode));
                var TaskCode = panel == null ? $"" : panel.TaskCode;
                logger.LogDebug($"ReportStartInf：  TaskCode {TaskCode}  ");
                var req = new BeginTaskRequest()
                {
                    ProductId = DeviceDescriptor.ProductId,
                    DeviceId = DeviceDescriptor.DeviceId,
                    TraceId = InteractingDevice.DrillTransactionId,
                    RealCount = count,
                    Params = new Dictionary<string, object?>()
                    {
                        { "TaskCode",TaskCode}
                    }
                };

                logger.LogDebug($"BeginTask:上报 任务开始： 请求参数：  {JsonSerializer.Serialize(req)}  ");
                var result = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<BeginTaskRequest, BeginTaskResponse>(InteractingDevice.CentralWebOptions.BeginTask, req);
                logger.LogDebug($"BeginTask:上报 任务开始 返回值：  {JsonSerializer.Serialize(result)}  ");
            }
        }

        /// <summary>
        /// Use the command object RPC_START to start the execution (again). The Start key is pressed.
        /// opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_START");
        /// </summary>
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

            Thread.Sleep(1000);
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

        private void RetractTool()
        {
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
            Thread.Sleep(15000);//等待25秒
                                //InteractingDevice.cnc84Command.SetCncComand($"DSP,");
                                // Thread.Sleep(300);
        }

        private void StartThreeColorLights()
        {
            return;
            //还未实现三色灯开启
            //
            //InteractingDevice.cnc84Command.SetCncComand("M113");
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
                    Thread.Sleep(200);
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
            // InteractingDevice.cnc84Command.SetCncComand($"DSP,");
        }

        public bool StartOpenMushroomControllerNew()
        {
            return existFourMushroom ? OpenFourMushroomController() : StartOpenMushroomController();
        }

        public bool StartPreOpenMushroomControllerNew()
        {
            var boardStatus = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);
            logger.LogDebug($"提前动蘑菇头  钻机上传感器的内容： {boardStatus[0]}");
            //获取板子状态
            if (boardStatus[0] != 0) return false;
            var progState = InteractingDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/WorkGroupSettings/WorkGroupInterface/TimeWidget/ProgramState");
            logger.LogDebug($"提前动蘑菇头 Drill_ProgramState  {progState}");
            if (new int[] { 2, 3, 4 }.Contains(progState)) return false;

            return existFourMushroom ? PreOpenFourMushroomController() : PreStartOpenMushroomController();
        }

        public bool OpenFourMushroomController()
        {
            //验证初始状态
            if (ValidateInitMushroomState())
            {
                //判断板子
                int boardSize = GetBoardSize();
                logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 蘑菇头 判定的板子 1小板 2中板 3大板 实际是 {boardSize}");
                switch (boardSize)
                {
                    case 1:
                        return ValidateMushroomEndSmallState();
                    case 2:
                        return ValidateMushroomEndMiddleState();
                    default:
                        return ValidateMushroomEndBigState();
                }
            }
            return false;
        }

        public void ControlMiddleMushroomNew(string str)
        {
            //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
            InteractingDevice.cnc84Command.SetCncComand($"DSP,OPEN_MIDDLE_MUSHROOM_M");
            Thread.Sleep(1000);
            InteractingDevice.cnc84Command.SetCncComand("SCRP,scCncCUSTOMERCbdClampOpenClosesmall");//M38,小板
            Thread.Sleep(5000);
            //InteractingDevice.cnc84Command.SetCncComand($"DSP,");
            logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送 开启中间 {str}蘑菇头指令->SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
        }

        private int GetBoardSize()
        {
            var smallBoard = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/UF.SmallBoard");
            var middleBoard = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/UF.MiddleBoard");
            logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】验证蘑菇头   获取小板用户标记 {smallBoard}  获取中板用户标记 {middleBoard}");
            if (smallBoard && !middleBoard)
            {
                return 1;
            }
            else if (!smallBoard && middleBoard)
            {
                return 2;
            }
            return 3;
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
        private bool ValidateMushroomEndBigState()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scKbiCbdClampOpenClose");
                InteractingDevice.cnc84Command.SetCncComand($"DSP,OPEN_FRONT_BACK_MASHROOM_M");
                Thread.Sleep(1000);
                InteractingDevice.cnc84Command.SetCncComand("SCRP,scKbiCbdClampClose");//M41,大板,开前后蘑菇头 大板
                Thread.Sleep(5000);
                // InteractingDevice.cnc84Command.SetCncComand($"DSP,");
                logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送 大板蘑菇头指令->SCRP,scKbiCbdClampClose");
                var z1State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1");
                var z2State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2");
                var z3State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z3");
                logger.LogWarning($"机器【{DeviceDescriptor.DeviceName} 】验证蘑菇头 z1状态{z1State}   z2状态{z2State} z3状态{z3State}");
                if (!z1State && z2State && z3State)
                {
                    logger.LogWarning("大板 前后蘑菇头结束，请等待...");
                    stopwatch.Stop();
                    logger.LogWarning($"机器【{DeviceDescriptor.DeviceName} 】大板验证 前后蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateMushroomEndTimeOut"].ToLong());//90秒
            return false;
        }
        private bool ValidateMushroomEndMiddleState()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
                InteractingDevice.cnc84Command.SetCncComand($"DSP,OPEN_MIDDLE_MUSHROOM_M");
                Thread.Sleep(1000);
                InteractingDevice.cnc84Command.SetCncComand("SCRP,scCncCUSTOMERCbdClampClosemiddle");//M35,第3组 中间板子
                Thread.Sleep(5000);
                //InteractingDevice.cnc84Command.SetCncComand($"DSP,");
                logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送 开启中间 中蘑菇头指令->SCRP,scCncCUSTOMERCbdClampClosemiddle");//todo
                var z1State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1");
                var z2State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2");
                var z3State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z3");
                logger.LogWarning($"机器【{DeviceDescriptor.DeviceName} 】验证蘑菇头 z1状态{z1State}   z2状态{z2State} z3状态{z3State}");
                if (!z1State && z2State && !z3State)
                {
                    logger.LogWarning("中板 前后 中后蘑菇头结束，请等待...");
                    stopwatch.Stop();
                    logger.LogWarning($"机器【{DeviceDescriptor.DeviceName} 】中板验证 前后 中后蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
                    return true;
                }

            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateMushroomEndTimeOut"].ToLong());//90秒
            return false;
        }
        private bool ValidateMushroomEndSmallState()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
                InteractingDevice.cnc84Command.SetCncComand($"DSP,OPEN_SMALL_MUSHROOM_M");
                Thread.Sleep(1000);
                InteractingDevice.cnc84Command.SetCncComand("SCRP,scCncCUSTOMERCbdClampClosesmall");//M38,小板
                Thread.Sleep(5000);
                logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送 开启中间 小板蘑菇头指令->SCRP,scCncCUSTOMERCbdClampClosesmall");
                var z1State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1");
                var z2State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2");
                var z3State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z3");
                logger.LogWarning($"机器【{DeviceDescriptor.DeviceName} 】验证蘑菇头 z1状态{z1State}   z2状态{z2State} z3状态{z3State}");
                if (!z1State && !z2State && z3State)
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
            InteractingDevice.cnc84Command.SetCncComand($"DSP,OPEN_MIDDLE_MUSHROOM_M");
            Thread.Sleep(1000);
            InteractingDevice.cnc84Command.SetCncComand("SCRP,scCncCUSTOMERCbdClampOpenClosesmall");//M38,小板
            Thread.Sleep(5000);
            //InteractingDevice.cnc84Command.SetCncComand($"DSP,");
            logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送 开启中间 {str}蘑菇头指令->SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
        }

        public void ControlThirdMushroom(string str)
        {
            //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
            InteractingDevice.cnc84Command.SetCncComand($"DSP,OPEN_MIDDLE_MUSHROOM_M");
            Thread.Sleep(1000);
            InteractingDevice.cnc84Command.SetCncComand("SCRP,scCncCUSTOMERCbdClampClosemiddle");//M35,第3组 中间板子
            Thread.Sleep(5000);
            //InteractingDevice.cnc84Command.SetCncComand($"DSP,");
            logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送 开启中间 {str}蘑菇头指令->SCRP,scCncCUSTOMERCbdClampClosemiddle");//todo
        }

        public void ControlFrontBackMushroom(string str)
        {
            //opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scKbiCbdClampOpenClose");
            InteractingDevice.cnc84Command.SetCncComand($"DSP,OPEN_FRONT_BACK_MASHROOM_M");
            Thread.Sleep(1000);
            InteractingDevice.cnc84Command.SetCncComand("SCRP,scKbiCbdClampOpenClose");//M41,大板,开前后蘑菇头 大板
            Thread.Sleep(5000);
            // InteractingDevice.cnc84Command.SetCncComand($"DSP,");
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
            InteractingDevice.cnc84Command.SetCncComand($"DSP,OPEN_FRONT_BACK_MASHROOM");
            Thread.Sleep(1000);
            InteractingDevice.cnc84Command.SetCncComand("SCRP,scKbiCbdClampOpenClose");//M27,大板,开前后蘑菇头,M41 CNC84 M27;CNC95 M41
        }

        private bool ValidateEscEnd()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                Thread.Sleep(100);
                string blockText = InteractingDevice.cnc84Command.ReadCncNode<string>("ns=4;s=UI/origin/RosiInfo/BlockText");
                logger.LogDebug($"判断是否有锁机信号: {blockText} ");
                if (!blockText.ToUpper().Contains("BUFFER未在安全位置") && !blockText.ToUpper().Contains("ATRUN"))
                {
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateEscCommandTimeOut"].ToLong() * 10);//90秒
            return false;
        }

        private Int64 GetDrillBoardStatus()
        {
            var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);//116
            Int64 drillBoardStatus = ready[0];
            logger.LogWarning($"原始掩码decimalNum:{drillBoardStatus}");
            if (DeviceDescriptor.Extra["IsDuo"].ToBool())
            {
                var rawStatus = Convert.ToString(ready[0], 2).PadLeft(6, '0').ToList();//十进制整数转换为二进制字符串：int decimalValue = 6轴63,5轴31,4轴17,3轴7,2轴3,1轴1;string binaryString = Convert.ToString(decimalValue, 2);
                logger.LogWarning($"读到板状态 rawStatus:{ready[0]}");
                string statusStr = string.Join("", rawStatus);
                string result = "";

                foreach (char digit in statusStr)
                {
                    result += new string(digit, 2);
                }
                Int64 decimalNum = Convert.ToInt64(result, 2);                          //二进制字符串转换为十进制整数：string binaryString = "111111";int decimalValue = Convert.ToInt32(binaryString, 2);
                logger.LogWarning($" Duo 掩码decimalNum:{decimalNum}");
                drillBoardStatus = decimalNum;
            }

            return drillBoardStatus;
        }

        public void StartSelectSpline()
        {
            try
            {
                logger.LogWarning("StartSelectSpline开始切换界面");
                Thread.Sleep(1000);

                StartChangeF8("StartSelectSpline");
                logger.LogWarning("开始选择轴");
                var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);//116
                if (!DeviceDescriptor.Extra["IsDuo"].ToBool())
                {
                    InteractingDevice.cnc84Command.WriteCncNode<Int64>("ns=4;s=UI/normalized/custom/STATIONALLOPENCLOSE", ready[0]);
                }
                else
                {
                    var rawStatus = Convert.ToString(ready[0], 2).PadLeft(6, '0').ToList();//十进制整数转换为二进制字符串：int decimalValue = 6轴63,5轴31,4轴17,3轴7,2轴3,1轴1;string binaryString = Convert.ToString(decimalValue, 2);
                    logger.LogWarning($"读到板状态 rawStatus:{ready[0]}");
                    string statusStr = string.Join("", rawStatus);
                    string result = "";

                    foreach (char digit in statusStr)
                    {
                        result += new string(digit, 2);
                    }
                    Int64 decimalNum = Convert.ToInt64(result, 2);                          //二进制字符串转换为十进制整数：string binaryString = "111111";int decimalValue = Convert.ToInt32(binaryString, 2);
                    logger.LogWarning($"掩码decimalNum:{decimalNum}");

                    InteractingDevice.cnc84Command.WriteCncNode<Int64>("ns=4;s=UI/normalized/custom/STATIONALLOPENCLOSE", decimalNum);
                    Thread.Sleep(1000);

                    InteractingDevice.cnc84Command.SetCncComand($"DSP,Start_Select_Spindle_" + decimalNum.ToString());
                }

                Thread.Sleep(1000);

                logger.LogWarning("调用选轴脚本");
                InteractingDevice.cnc84Command.SetCncComand($"DSP,SPINDLE_SELECT");
                Thread.Sleep(1000);
                InteractingDevice.cnc84Command.SetCncComand($"SCRP,scCncCUSTOMERSTATIONALLOPENCLOSE");

                Thread.Sleep(2000);
                logger.LogWarning("选轴脚本调用完成");
                //InteractingDevice.cnc84Command.SetCncComand($"DSP,");
            }
            catch (Exception ee)
            {
                logger.LogDebug($"钻机选轴异常：{ee.Message}" + ee);
            }
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
                var work = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferStatusFlag"].ToUshort(), 4);
                if (work[1] != 1)
                {
                    return "手动状态不能呼叫AGV";
                }
                if (!InteractingDevice.MqttClientWrapper.IsConnected) return "呼叫AGV  Mqtt 连接断开 不上报消息";
                var payloadPanelsTemp = DeepCopy(InteractingDevice.PayloadPanels);

                var material = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 2);
                var rawMaterialLayerBoardStatus = material[0];
                var clinkerLayerBoardStatus = material[1];
                var panel = payloadPanelsTemp.Skip(InteractingDevice.spindleNum * 2).Take(InteractingDevice.spindleNum).FirstOrDefault(s => !string.IsNullOrEmpty(s.ItemCode));
                var ItemCode = panel == null ? $"11111{InteractingDevice.DeviceId}" : panel.ItemCode;
                //熟料层 状态
                var removeclinker = Convert.ToString(clinkerLayerBoardStatus, 2).PadLeft(InteractingDevice.spindleNum, '0').Reverse().ToArray();
                for (int i = 0; i < removeclinker.Length; i++)
                {
                    if (removeclinker[i] == '0')
                    {
                        var panelSingle = Panel.HasSilo.NoPanelForSingleSpindle("", i + 1, 2, 1)[0];
                        panelSingle.LocationCode = InteractingDevice.DeviceId;
                        panelSingle.SiloCode = InteractingDevice.DeviceId;
                        payloadPanelsTemp[InteractingDevice.spindleNum * 2 + i] = panelSingle;
                    }
                }

                var splineStatus = InteractingDevice.SplineNoBrokenStatus;
                var rawStatus = Convert.ToString(rawMaterialLayerBoardStatus, 2).PadLeft(InteractingDevice.spindleNum, '0').Reverse().ToArray();
                int ExistRawNum = rawStatus.Count(c => c == '1');
                int SpindleUseNum = splineStatus.Count(c => c == '1');
                bool rawFull = true;
                for (int i = 0; i < splineStatus.Length; i++)
                {
                    if (splineStatus[i] == '1' && rawStatus[i] != '1')
                    {
                        rawFull = false;
                        break;
                    }
                }
                logger.LogDebug($"轴未坏状态{splineStatus} 生料轴状态  {string.Join(",", rawStatus)} ");

                if (clinkerLayerBoardStatus == 0)
                {
                    InteractingDevice.isCallAgvUnload = false;
                    //无熟料
                    if (rawFull)
                    {
                        logger.LogDebug($"生料满 不呼叫  ");
                        return "生料满 不呼叫";
                    }
                    else
                    {
                        // 只上生料
                        logger.LogDebug("只上料");
                        var count = splineStatus.Count(c => c == '1');
                        IniAgvPosition();
                        var agvPosition = InteractingDevice.spindleAgvPosition;
                        var spindleBehavior = new int[splineStatus.Length];
                        for (int i = 0; i < splineStatus.Length; i++)
                        {
                            if (splineStatus[i] == '0')
                            {
                                agvPosition[i] = "null";
                                spindleBehavior[i] = -1;
                            }
                            else
                            {
                                if (rawStatus[i] == '1')
                                {
                                    agvPosition[i] = "null";
                                    spindleBehavior[i] = -1;
                                    count--;
                                }
                                else
                                {
                                    spindleBehavior[i] = 0;
                                }
                            }
                        }

                        var spindleBehaviorInf = string.Join(",", spindleBehavior);
                        var agvPositionInf = string.Join(",", agvPosition);
                        logger.LogDebug($"只上料-----上生料个数 {count}   AGV位置：{agvPositionInf}   轴上下料信息：  {spindleBehaviorInf}  ");
                        //只上料
                        logger.LogDebug($"只上料----- 上报信息PayloadPanels：  {JsonSerializer.Serialize(payloadPanelsTemp)}  ");
                        if (DeviceDescriptor.AutoMode)
                        {
                            var result = await DataExporter.DeviceEventReport(
                          new DeviceEventReportRequest()
                          {
                              ProductId = DeviceDescriptor.ProductId,
                              DeviceId = DeviceDescriptor.DeviceId,
                              ClientId = InteractingDevice.ClientId,
                              EventId = Events.REQUEST_AGV_LOAD_PANEL_THEN_UNLOAD_PANEL,
                              RequestInputProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1,
                              RequestOutputProductStatus = ProductStatus.Finished_DRILL,
                              RequestInteractionBehavior = InteractionBehavior.Make(this.DeviceDescriptor.DeviceKind, InteractionBehavior.REAR_LOAD_PANEL_ONLY),
                              EventName = Events.Drill.BUFFER_EXIST_CLINKER_EVENT_NAME,
                              RequestDeviceKind = DeviceKind.CNC95Drill,
                              RequestMaterialKind = MaterialKind.Panel,
                              RequestInteractionDirection = InteractionPosition.Rear,
                              Params = new Dictionary<string, object?>()
                              {
                                    { "ExistRawNum", ExistRawNum },
                                    { "SpindleUseNum", SpindleUseNum },
                                    { "RawSpindleNum", count },
                                    { "SpindleNum", count },
                                    { "ClinkerSpindleNum", 0 },
                                    { "InteractivePosition",  DeviceDescriptor.Extra["InteractivePosition"].ToStr () },
                                    { "Spindles", agvPositionInf},
                                    { "SpindleBehavior", spindleBehaviorInf },
                                    { "RegulatePos", DeviceDescriptor.Extra["RegulatePos"].ToStr() }
                              },
                              PayloadPanels = payloadPanelsTemp,
                          });
                            if (result == null)
                            {
                                InteractingDevice.TransactionId = $"{DateTime.Now.ToString()}  ----  和服务器断开连接，呼叫失败";
                            }
                            else
                            {
                                if (result.Code == ErrorCodes.Sys.SUCCESS)
                                {
                                    InteractingDevice.NewTranscationIdTemp = result.TraceId!;
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
                            }

                            logger.LogDebug($"上报信息结果：  {JsonSerializer.Serialize(result)}  ");
                        }
                        return $"呼叫AGV   只上料  上生料个数 {count}   AGV位置：{agvPositionInf}   轴上下料信息：  {spindleBehaviorInf}    结束";
                    }
                }
                else
                {
                    //有熟料
                    if (rawFull)
                    {
                        // 只下熟料
                        logger.LogDebug("只下料");
                        IniAgvPosition();
                        var status = clinkerLayerBoardStatus;
                        var count = Convert.ToString(status, 2).PadLeft(InteractingDevice.spindleNum, '0').Count(c => c == '1');
                        var agvPosition = InteractingDevice.spindleAgvPosition;
                        var clinkerStatus = Convert.ToString(status, 2).PadLeft(InteractingDevice.spindleNum, '0').Reverse().ToArray();
                        var spindleBehavior = new int[splineStatus.Length];
                        var flag = false;
                        for (int i = 0; i < clinkerStatus.Length; i++)
                        {
                            if (clinkerStatus[i] == '0')
                            {
                                agvPosition[i] = "null";
                                spindleBehavior[i] = -1;
                            }
                            else
                            {
                                spindleBehavior[i] = 1;
                                if (payloadPanelsTemp[i + 2 * DeviceDescriptor.SpindleNum].IsNull())
                                {
                                    // flag = true;
                                    //break;
                                    payloadPanelsTemp[i + 2 * DeviceDescriptor.SpindleNum] = new Panel
                                    {
                                        PanelCode = "mockinf" + Guid.NewGuid(),
                                        ItemCode = ItemCode,
                                        ProductStatus = ProductStatus.Finished_DRILL,
                                        SiloCode = $"Silo01",
                                        Layer = 2,
                                        Position = i + 1,
                                        BatchCode = "",
                                        LotId = "",
                                        PanelWidth = 622f,
                                        PinOffset = 3.2f,
                                    };
                                }
                                else
                                {
                                    if (payloadPanelsTemp[i + 2 * DeviceDescriptor.SpindleNum].ProductStatus != ProductStatus.Finished_DRILL)
                                    {
                                        payloadPanelsTemp[i + 2 * DeviceDescriptor.SpindleNum] = new Panel
                                        {
                                            PanelCode = "mockinf" + Guid.NewGuid(),
                                            ItemCode = ItemCode,
                                            ProductStatus = ProductStatus.Finished_DRILL,
                                            SiloCode = $"Silo01",
                                            Layer = 2,
                                            Position = i + 1,
                                            BatchCode = "",
                                            LotId = "",
                                            PanelWidth = 622f,
                                            PinOffset = 3.2f,
                                        };
                                        //flag = true;
                                        //break;
                                    }
                                }
                            }
                        }
                        if (flag)
                        {
                            return $"只下料  操作失败 请加载料仓    结束";
                        }
                        var spindleBehaviorInf = string.Join(",", spindleBehavior);
                        var agvPositionInf = string.Join(",", agvPosition);
                        InteractingDevice.isCallAgvUnload = true;
                        logger.LogDebug($"只下料----- AGV位置：{agvPositionInf}   轴上下料信息：  {spindleBehaviorInf}   呼叫AGV只下料{InteractingDevice.isCallAgvUnload}");
                        logger.LogDebug($"只下料----- 上报信息PayloadPanels：  {JsonSerializer.Serialize(payloadPanelsTemp)}  ");
                        if (DeviceDescriptor.AutoMode)
                        {
                            var result = await DataExporter.DeviceEventReport(
                             new DeviceEventReportRequest()
                             {
                                 ProductId = DeviceDescriptor.ProductId,
                                 DeviceId = DeviceDescriptor.DeviceId,
                                 ClientId = InteractingDevice.ClientId,
                                 EventId = Events.REQUEST_AGV_LOAD_PANEL_THEN_UNLOAD_PANEL,
                                 RequestInteractionBehavior = InteractionBehavior.Make(this.DeviceDescriptor.DeviceKind, InteractionBehavior.REAR_UNLOAD_PANEL_ONLY),
                                 RequestInputProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1,
                                 RequestOutputProductStatus = ProductStatus.Finished_DRILL,
                                 EventName = Events.Drill.BUFFER_EXIST_CLINKER_EVENT_NAME,
                                 RequestDeviceKind = DeviceKind.CNC95Drill,
                                 RequestMaterialKind = MaterialKind.Panel,
                                 RequestInteractionDirection = InteractionPosition.Rear,
                                 Params = new Dictionary<string, object?>()
                                 {
                                       { "ExistRawNum", ExistRawNum },
                                       { "SpindleUseNum", SpindleUseNum },
                                        { "SpindleNum", count },
                                        { "RawSpindleNum", 0 },
                                        { "ClinkerSpindleNum", count },
                                        { "InteractivePosition",  DeviceDescriptor.Extra["InteractivePosition"].ToStr () },
                                        { "Spindles", agvPositionInf },
                                        { "SpindleBehavior", spindleBehaviorInf },
                                        { "RegulatePos", DeviceDescriptor.Extra["RegulatePos"].ToStr() }
                                 },
                                 PayloadPanels = payloadPanelsTemp,
                             });

                            if (result.Code == ErrorCodes.Sys.DUPLICATE_SERVICE_INVOKED_CODE)
                            {
                                InteractingDevice.TransactionId = $"{DateTime.Now.ToString()}  ----  和服务器断开连接，呼叫失败";
                            }
                            else
                            {
                                if (result.Code == ErrorCodes.Sys.SUCCESS)
                                {
                                    InteractingDevice.NewTranscationIdTemp = result.TraceId!;
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
                            }

                            logger.LogDebug($"上报信息结果：  {JsonSerializer.Serialize(result)}  ");
                        }
                        return $"呼叫AGV   只下料   AGV位置：{agvPositionInf}   轴上下料信息：  {spindleBehaviorInf}   结束";
                    }
                    else
                    {
                        //既上生料也下熟料
                        //先上料再下料
                        InteractingDevice.isCallAgvUnload = false;
                        logger.LogDebug("先上料再下料");
                        var count = splineStatus.Count(c => c == '1');
                        var clinkerLayerCount = Convert.ToString(clinkerLayerBoardStatus, 2).PadLeft(InteractingDevice.spindleNum, '0').Reverse().Count(c => c == '1');
                        InteractingDevice.IniAgvPosition();
                        var agvPosition = InteractingDevice.spindleAgvPosition;
                        var status = clinkerLayerBoardStatus;
                        var clinkerStatus = Convert.ToString(status, 2).PadLeft(InteractingDevice.spindleNum, '0').Reverse().ToArray();
                        var spindleBehavior = new int[splineStatus.Length];
                        var flag = false;
                        var rawRealCount = 0;
                        for (int i = 0; i < splineStatus.Length; i++)
                        {
                            if (splineStatus[i] == '0' && clinkerStatus[i] == '0')
                            {
                                agvPosition[i] = "null";
                                spindleBehavior[i] = -1;
                            }
                            else if (splineStatus[i] == '0' && clinkerStatus[i] == '1')
                            {
                                spindleBehavior[i] = 1;
                                if (payloadPanelsTemp[i + 2 * DeviceDescriptor.SpindleNum].IsNull())
                                {
                                    payloadPanelsTemp[i + 2 * DeviceDescriptor.SpindleNum] = new Panel
                                    {
                                        PanelCode = "mockinf" + Guid.NewGuid(),
                                        ItemCode = ItemCode,
                                        ProductStatus = ProductStatus.Finished_DRILL,
                                        SiloCode = $"Silo01",
                                        Layer = 2,
                                        Position = i + 1,
                                        BatchCode = "",
                                        LotId = "",
                                        PanelWidth = 622f,
                                        PinOffset = 3.2f,
                                    };
                                    //flag = true;
                                    //break;
                                }
                                else
                                {
                                    if (payloadPanelsTemp[i + 2 * DeviceDescriptor.SpindleNum].ProductStatus != ProductStatus.Finished_DRILL)
                                    {
                                        payloadPanelsTemp[i + 2 * DeviceDescriptor.SpindleNum] = new Panel
                                        {
                                            PanelCode = "mockinf" + Guid.NewGuid(),
                                            ItemCode = ItemCode,
                                            ProductStatus = ProductStatus.Finished_DRILL,
                                            SiloCode = $"Silo01",
                                            Layer = 2,
                                            Position = i + 1,
                                            BatchCode = "",
                                            LotId = "",
                                            PanelWidth = 622f,
                                            PinOffset = 3.2f,
                                        };
                                        //flag = true;
                                        //break;
                                    }
                                }
                            }
                            else if (splineStatus[i] == '1' && clinkerStatus[i] == '1')
                            {
                                if (rawStatus[i] == '1')
                                {
                                    spindleBehavior[i] = 1;
                                }
                                else
                                {
                                    rawRealCount++;
                                    spindleBehavior[i] = 2;
                                }
                                if (payloadPanelsTemp[i + 2 * DeviceDescriptor.SpindleNum].IsNull())
                                {
                                    payloadPanelsTemp[i + 2 * DeviceDescriptor.SpindleNum] = new Panel
                                    {
                                        PanelCode = "mockinf" + Guid.NewGuid(),
                                        ItemCode = ItemCode,
                                        ProductStatus = ProductStatus.Finished_DRILL,
                                        SiloCode = $"Silo01",
                                        Layer = 2,
                                        Position = i + 1,
                                        BatchCode = "",
                                        LotId = "",
                                        PanelWidth = 622f,
                                        PinOffset = 3.2f,
                                    };
                                    //flag = true;
                                    //break;
                                }
                                else
                                {
                                    if (payloadPanelsTemp[i + 2 * DeviceDescriptor.SpindleNum].ProductStatus != ProductStatus.Finished_DRILL)
                                    {
                                        payloadPanelsTemp[i + 2 * DeviceDescriptor.SpindleNum] = new Panel
                                        {
                                            PanelCode = "mockinf" + Guid.NewGuid(),
                                            ItemCode = ItemCode,
                                            ProductStatus = ProductStatus.Finished_DRILL,
                                            SiloCode = $"Silo01",
                                            Layer = 2,
                                            Position = i + 1,
                                            BatchCode = "",
                                            LotId = "",
                                            PanelWidth = 622f,
                                            PinOffset = 3.2f,
                                        };
                                        //flag = true;
                                        //break;
                                    }
                                }
                            }
                            else if (splineStatus[i] == '1' && clinkerStatus[i] == '0')
                            {
                                if (rawStatus[i] == '1')
                                {
                                    spindleBehavior[i] = -1;
                                }
                                else
                                {
                                    rawRealCount++;
                                    spindleBehavior[i] = 0;
                                }
                            }
                        }

                        if (flag)
                        {
                            return $"先上料再下料  操作失败 请加载料仓    结束";
                        }
                        var spindleBehaviorInf = string.Join(",", spindleBehavior);
                        var agvPositionInf = string.Join(",", agvPosition);
                        logger.LogDebug($"先上料再下料----上生料的个数{rawRealCount}-  AGV位置：{agvPositionInf}   轴上下料信息：  {spindleBehaviorInf}  ");
                        logger.LogDebug($"上报信息PayloadPanels：  {JsonSerializer.Serialize(payloadPanelsTemp)}  ");
                        if (DeviceDescriptor.AutoMode)
                        {
                            var result = await DataExporter.DeviceEventReport(
                          new DeviceEventReportRequest()
                          {
                              ProductId = DeviceDescriptor.ProductId,
                              DeviceId = DeviceDescriptor.DeviceId,
                              ClientId = InteractingDevice.ClientId,
                              EventId = Events.REQUEST_AGV_LOAD_PANEL_THEN_UNLOAD_PANEL,
                              RequestInputProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1,
                              RequestOutputProductStatus = ProductStatus.Finished_DRILL,
                              RequestInteractionBehavior = InteractionBehavior.Make(this.DeviceDescriptor.DeviceKind, InteractionBehavior.REAR_LOAD_PANEL_THEN_UNLOAD_PANEL),
                              EventName = Events.Drill.BUFFER_EXIST_CLINKER_EVENT_NAME,
                              RequestDeviceKind = DeviceKind.CNC95Drill,
                              RequestMaterialKind = MaterialKind.Panel,
                              RequestInteractionDirection = InteractionPosition.Rear,
                              Params = new Dictionary<string, object?>()
                              {
                                    { "ExistRawNum", ExistRawNum },
                                    { "SpindleUseNum", SpindleUseNum },
                                    { "SpindleNum", count },
                                    { "RawSpindleNum", rawRealCount },
                                    { "ClinkerSpindleNum", clinkerLayerCount },
                                    //{ "ClinkerSpindleStatus",  clinkerStatusInf },
                                    { "InteractivePosition",  DeviceDescriptor.Extra["InteractivePosition"].ToStr() },
                                    { "Spindles",  agvPositionInf },
                                    { "SpindleBehavior", spindleBehaviorInf },
                                    { "RegulatePos", DeviceDescriptor.Extra["RegulatePos"].ToStr() }
                              },
                              PayloadPanels = payloadPanelsTemp
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
                                    InteractingDevice.NewTranscationIdTemp = result.TraceId!;
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
                            }
                            logger.LogDebug($"上报信息结果：  {JsonSerializer.Serialize(result)}  ");
                        }
                        return $"呼叫AGV   先上料再下料 上生料的个数{rawRealCount}  AGV位置：{agvPositionInf}   轴上下料信息：  {spindleBehaviorInf}   结束";
                    }
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
                WriteWarningToPlc(7);
                logger.LogDebug("请检查是否有生产程序文件!");
                return false;
            }

            if (!string.IsNullOrEmpty(DiaFilePath_tmp) && !File.Exists(DiaFilePath_tmp))
            {
                WriteWarningToPlc(7);
                logger.LogDebug("请检查是否有直径程序文件!");
                return false;
            }
            return true;
        }

        private void WriteWarningToPlc(ushort code)
        {
            try
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["WarningInfoWriteOnPlc"].ToUshort(), code);
                if (!InteractingDevice.threeColorLightsExistsOnBuffer)
                {
                    StartThreeColorLights();
                }
            }
            catch (Exception ee)
            {
                logger.LogError($"写错误异常:{ee.Message}");
            }
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
            if (!string.IsNullOrWhiteSpace(drilPath) && !ValidateDrlFileSame(drilPath))
            {
                InteractingDevice.cnc84Command.SetCncComand("CM@@@");
                if (!ValidateCmDrlFile())
                {
                    logger.LogError(" 加载文件 发送CM@@@ 执行失败");
                    return false;
                }
                Thread.Sleep(1000);
                InteractingDevice.cnc84Command.SetCncComand("CD@@@");
                if (!ValidateCdFile())
                {
                    logger.LogError(" 加载文件  发送CD@@@ 执行失败");
                    return false;
                }
                Thread.Sleep(1000);
                //加载dia文件
                //加载drl文件
                InteractingDevice.cnc84Command.SendLoadFile(diaPath, "DIAMETERTABLE");
                Thread.Sleep(3000);
                if (!ValidateDiaFile(diaPath))
                {
                    logger.LogError(" 加载文件  验证加载dia文件失败");
                    return false;
                }
                Thread.Sleep(3000);
                InteractingDevice.cnc84Command.SendLoadFile(drilPath, "PROGRAM");
                Thread.Sleep(3000);
                //SendLoadFile(drilPath, "PROGRAM");
                if (!ValidateDrlFile(drilPath))
                {
                    logger.LogError(" 加载文件  验证加载pro文件失败");
                    return false;
                }
                //验证是否重新加载
                if (!ValidateDrlImg())
                {
                    logger.LogError(" 加载文件  解析程序失败");
                    return false;
                }
            }

            #region same

            //else
            //{
            //    if (!string.IsNullOrWhiteSpace(diaPath) && !ValidateDiaFileSame(diaPath))
            //    {
            //        InteractingDevice.cnc84Command.SendLoadFile(diaPath, "DIAMETERTABLE");
            //        if (!ValidateDiaFile(diaPath))
            //        {
            //            return false;
            //        }
            //    }
            //    if (!string.IsNullOrWhiteSpace(drilPath) && !ValidateDrlFileSame(drilPath))
            //    {
            //        InteractingDevice.cnc84Command.SendLoadFile(drilPath, "PROGRAM");
            //        //SendLoadFile(drilPath, "PROGRAM");
            //        Thread.Sleep(2000);
            //        if (!ValidateDrlFile(drilPath))
            //        {
            //            return false;
            //        }
            //        //验证是否重新加载
            //        if (!ValidateDrlImg())
            //        {
            //            return false;
            //        }
            //    }

            //}

            #endregion same

            return true;
        }

        private void FailLoadAtpFileAsync()
        {
            Task.Run(async () =>
            {
                try
                {
                    if (DeviceDescriptor.Extra["IsGetAtpFromCentre"].ToBool())
                    {
                        string strGroupNo = string.Empty;
                        string strUpdateCutterUrl = DeviceDescriptor.Extra["UpdateCutterGroupStatus"].ToStr();
                        if (string.IsNullOrWhiteSpace(_strGroupNoFromGetAtp))
                        {
                            logger.LogError($"向中控请求 GetAfterDrillPath 返回值中 为空或不包括 strGroupNo 或 strGroupNo 为空");
                        }
                        else
                        {
                            strGroupNo = _strGroupNoFromGetAtp;
                            _strGroupNoFromGetAtp = string.Empty;
                        }
                        UpdateCutterGroupRequest updateCutterGroupRequest = new UpdateCutterGroupRequest() { groupNo = strGroupNo, groupStatus = (int)CUTTERGROUPSTATUS.Lock };
                        logger.LogDebug($"调用ATP加载结果接口, {strUpdateCutterUrl}");
                        var updateCutterResponse = await HttpRequestInvoker.PostAsJsonAsync<UpdateCutterGroupRequest, UpdateCutterGroupResponse>(strUpdateCutterUrl, updateCutterGroupRequest);
                        logger.LogDebug($"调用ATP加载结果接口, {strUpdateCutterUrl}，返回, {JsonSerializer.Serialize(updateCutterResponse)}");
                        if (updateCutterResponse == null)
                        {
                            logger.LogError($"{strUpdateCutterUrl} 返回空");
                        }
                        else
                        {
                            logger.LogDebug($"{strUpdateCutterUrl} 返回 code={updateCutterResponse.code}, message={updateCutterResponse.message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
            });
        }

        public bool LoadAtpFile(string atpPath)
        {
            logger.LogDebug($"LoadAtpFile执行加载刀具文件");
            bool IsLoadSuccess = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(atpPath) && DeviceDescriptor.Extra["IsGetAtpFromCentre"].ToBool())
                {
                    InteractingDevice.cnc84Command.SetCncComand("CA@@@");
                    if (!ValidateCaFile())
                    {
                        logger.LogError(" 加载文件 发送CA@@@ 执行失败");
                        return false;
                    }
                    Thread.Sleep(2000);

                    InteractingDevice.cnc84Command.SendLoadFile(atpPath, "ATP");
                    Thread.Sleep(3000);
                    if (!ValidateAtpFile(atpPath))
                    {
                        logger.LogError(" 加载文件  验证加载atp文件失败");
                        return false;
                    }
                    else
                    {
                        logger.LogDebug($"加载ATP文件{atpPath}成功");
                        IsLoadSuccess = true;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message + " 加载文件 发送CA@@@ 执行异常");
            }
            finally
            {
                if (!IsLoadSuccess)
                {
                    FailLoadAtpFileAsync();
                }
            }
            return false;
        }

        public bool LoadFile(string drilPath, string diaPath, string atpPath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(atpPath))
                {
                    if (!LoadAtpFile(atpPath))
                    {
                        return false;
                    }
                }

                return LoadFile(drilPath, diaPath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return false;
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
                if (string.IsNullOrEmpty(netProName) || "[NULL]".Equals(netProName) || "????".Equals(netProName))
                {
                    stopwatch.Stop();
                    logger.LogError($"CM@@@ 用时 {stopwatch.ElapsedMilliseconds} Millisecond");
                    return true;
                }
                InteractingDevice.cnc84Command.SetCncComand("CM@@@");
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateDiaFileTimeout"].ToLong());
            return false;
        }

        private bool ValidateCdFile()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var netProName = string.Empty;
            do
            {
                Thread.Sleep(1000);
                netProName = InteractingDevice.cnc84Command.GetDiaFileNameWithDialog();
                if (string.IsNullOrEmpty(netProName) || "[NULL]".Equals(netProName))
                {
                    stopwatch.Stop();
                    logger.LogError($"CD@@@ 用时 {stopwatch.ElapsedMilliseconds} Millisecond");
                    return true;
                }
                InteractingDevice.cnc84Command.SetCncComand("CD@@@");
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateDiaFileTimeout"].ToLong());
            return false;
        }

        private bool ValidateCaFile()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var netProName = string.Empty;
            do
            {
                Thread.Sleep(1000);
                netProName = InteractingDevice.cnc84Command.GetAtpFileName();
                if (string.IsNullOrEmpty(netProName) || "[NULL]".Equals(netProName))
                {
                    stopwatch.Stop();
                    logger.LogError($"CA@@@ 用时 {stopwatch.ElapsedMilliseconds} Millisecond");
                    return true;
                }
                InteractingDevice.cnc84Command.SetCncComand("CA@@@");
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateDiaFileTimeout"].ToLong());
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
                    logger.LogError($"ValidateDiaFile 用时 {stopwatch.ElapsedMilliseconds} Millisecond");
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateDiaFileTimeout"].ToLong());
            stopwatch.Stop();
            return false;
        }

        private bool ValidateAtpFile(string atpFilePath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var diaName = string.Empty;
            do
            {
                Thread.Sleep(50);
                //opcUaClient.ReadNode<String>("ns=4;s=UI/origin/FilesAndPaths/ToolFiles/DiameterGroup/DiameterTable");
                diaName = InteractingDevice.cnc84Command.GetAtpFileName();
                if (string.IsNullOrEmpty(diaName))
                {
                    continue;
                }
                if ($"{atpFilePath.Replace("\\", "").Replace("/", "").ToLower()}".Equals(diaName.Replace("\\", "").Replace("/", "").ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    logger.LogError($"ValidateDiaFile 用时 {stopwatch.ElapsedMilliseconds} Millisecond");
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
                var modifyDrilFilePath = drilFilePath.Trim().Replace("\\", "").Replace("/", "").ToLower().Trim();
                var modifyNextProName = netProName.Trim().Replace("\\", "").Replace("/", "").ToLower().Trim();
                logger.LogInformation($"校验程序名 modifyDrilFilePath:{modifyDrilFilePath} modifyNextProName{modifyNextProName}");
                if (modifyNextProName.Equals(modifyDrilFilePath, StringComparison.CurrentCultureIgnoreCase))
                {
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateDrlFileTimeout"].ToLong());
            return false;
        }

        public async Task LoadFileFromCenter()
        {
            int errorNum = 0;
            try
            {
                InteractingDevice.isOnDoingScript = true;
                SendMessageToShow(0);
                InteractingDevice.cnc84Command.SetCncComand($"DSP,New_Loop_CNC_Buffer_Load_UnLoad_Begin");
                await Task.Delay(2000);

                DrillOprationTime = DateTime.Now;
                logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  钻机和buffer整个上下料结束");
                InteractingDevice.cnc84Command.SetCncComand($"DSP,3/8_Load_File_From_Center");
                SendMessageToShow(3);
                logger.LogWarning($"是否向中控要加载程序和参数 ： {loadFileFromCentreOnOff}  自动模式 {DeviceDescriptor.AutoMode} ");
                if (DeviceDescriptor.AutoMode && loadFileFromCentreOnOff)
                {
                    string result = "0";
                    try
                    {
                        var panel = InteractingDevice.PayloadPanels.Skip(InteractingDevice.spindleNum).Take(InteractingDevice.spindleNum).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s.ItemCode));
                        var itemCode = string.Empty; if (panel != null) { itemCode = panel.ItemCode; }
                        if (string.IsNullOrEmpty(itemCode))
                        {
                            logger.LogError($"itemCode 是空 不能向中控发起呼叫获取工艺分组");
                            InteractingDevice.cnc84Command.SetCncComand($"DSP,ITEMCODE_IS_NULL");
                            result = "2";
                            WriteWarningToPlc(7);
                            return;
                        }
                        var url = string.Format(DeviceDescriptor.Extra["GetGroupCode"].ToStr(), DeviceDescriptor.DeviceId, itemCode);
                        var group = await HttpRequestInvoker.GetFromJsonAsync<Dictionary<string, object?>>(url);
                        if (group == null || group.Count == 0)
                        {
                            logger.LogError($"向中控请求获取工艺分组 {url} 异常");
                            InteractingDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_GET_GETGROUPCODE_URL");
                            result = "3";
                            WriteWarningToPlc(7);
                            return;
                        }
                        logger.LogDebug($"向中控请求{url}的返回值为{JsonSerializer.Serialize(group)}");
                        if (!group.ContainsKey("SpecGroup") || string.IsNullOrEmpty(group["SpecGroup"].ToString()))
                        {
                            logger.LogError($"向中控请求{url} 返回值中 不包括 SpecGroup 或工艺分组为空");
                            InteractingDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_NOT_CONTAINSKEY_SPECGROUP");
                            result = "4";
                            WriteWarningToPlc(7);
                            return;
                        }
                        var groupStr = group["SpecGroup"].ToString();
                        logger.LogDebug($"向中控请求的工艺分组为{groupStr}");
                        InteractingDevice.cnc84Command.SetCncComand($"DSP,Get_data_from_database");
                        DrillInfo drillInf;
                        try
                        {
                            drillInf = InteractingDevice.DrillFilePathLocator.GetFilePath($"{itemCode};{groupStr}");
                        }
                        catch (Exception ee)
                        {
                            if (!ee.Message.StartsWith("EAP_MATERIAL_DATA"))
                            {
                                InteractingDevice.cnc84Command.SetCncComand($"DSP,3/8_DATABASE_EXCEPTION");
                            }
                            else
                            {
                                InteractingDevice.cnc84Command.SetCncComand($"DSP,{ee.Message}");
                            }
                            logger.LogError($"向中控请求的工艺分组为{groupStr} 和数据库交互失败 {ee.Message}");
                            result = "5";
                            WriteWarningToPlc(7);
                            return;
                        }
                        logger.LogError($"数据库交互获取数据 {JsonSerializer.Serialize(drillInf)}");
                        if (string.IsNullOrWhiteSpace(drillInf.DrlPath))
                        {
                            logger.LogError("和数据库交互获取钻带文件不存在");
                            InteractingDevice.cnc84Command.SetCncComand($"DSP, DRILL_PATH_NOT_FIND");
                            result = "6";
                            WriteWarningToPlc(7);
                            return;
                        }
                        //向中控要转化后的路径
                        var drillPathUrl = string.Format(DeviceDescriptor.Extra["GetAfterDrillPath"].ToStr(), DeviceDescriptor.DeviceId, drillInf.DrlPath, itemCode);
                        var drillPathResult = await HttpRequestInvoker.GetFromJsonAsync<Dictionary<string, object?>>(drillPathUrl);
                        if (drillPathResult == null)
                        {
                            logger.LogError($"向中控请求获取转化后钻带程序 {drillPathUrl} 异常");
                            InteractingDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_GET_AFTER_DRILL_PATH_URL");
                            result = "7";
                            WriteWarningToPlc(7);
                            return;
                        }
                        logger.LogError($"向中控请求转化后{drillPathUrl}的返回值为{JsonSerializer.Serialize(drillPathResult)}");
                        if (!drillPathResult.ContainsKey("AfterDrillPath") || string.IsNullOrEmpty(drillPathResult["AfterDrillPath"].ToString()))
                        {
                            logger.LogError($"向中控请求{drillPathUrl} 返回值中 不包括 AfterDrillPath 或 AfterDrillPath为空");
                            InteractingDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_NOT_CONTAINSKEY_AFTERDRILLPATH");
                            result = "8";
                            WriteWarningToPlc(7);
                            return;
                        }

                        string afterDrillPath = drillPathResult["AfterDrillPath"].ToStr();
                        logger.LogDebug($"是否转化 AfterDrillPath开关 {drillChangeAfterPathOnOff},A轴是否工作整个台面 {aSplineIsWorkFullTable} 中控下发AfterDrillPath路径{afterDrillPath}");
                        if (drillChangeAfterPathOnOff)
                        {
                            string tempafterDrillPath = ModifyAfterPath(afterDrillPath);
                            afterDrillPath = tempafterDrillPath;
                        }
                        string filename = Path.GetFileNameWithoutExtension(afterDrillPath).ToLower();
                        if (validateFileAppendOnOff && !ValidateFileAppendContent(filename))
                        {
                            logger.LogError($"配方路径文件不是该机台用的钻带");
                            InteractingDevice.cnc84Command.SetCncComand($"DSP, DRILL_PATH_NOT_MATCH_TABLE_SIZE");
                            result = "9";
                            WriteWarningToPlc(7);
                            return;
                        }
                        string drillPath = afterDrillPath;
                        string diaPath = drillInf.DiaPath;
                        //if (!CheckDrlDiaFile(drillPath, diaPath))
                        //{
                        //    logger.LogError("配方路径文件不存在");
                        //    InteractingDevice.cnc84Command.SetCncComand($"DSP, DRILL_OR_DIA_PATH_NOT_FIND");
                        //    result = "10";
                        //    WriteWarningToPlc(7);
                        //    return;
                        //}

                        if (!LoadFileToCNC84(drillPath, diaPath))
                        {
                            logger.LogDebug($"加载程序到CNC84 失败");
                            InteractingDevice.cnc84Command.SetCncComand($"DSP, LOAD_DRILL_FAIL");
                            result = "11";
                            WriteWarningToPlc(7);
                            return;
                        }
                        result = "1";
                    }
                    catch (Exception ee)
                    {
                        logger.LogDebug($"和中控交互异常{ee.Message}");
                        InteractingDevice.cnc84Command.SetCncComand($"DSP, LOAD_FILE_FROM_CENTRE_FAIL");
                        result = "12";
                        WriteWarningToPlc(7);
                        return;
                    }
                    finally
                    {
                        if (result != "1")
                        {
                            result = "2";
                            errorNum = 2;
                        }
                        //通知eap
                        if (InteractingDevice.ApplicationServices.TryGetService<IDrillFileLoadResult>(out IDrillFileLoadResult fileLoadResult))
                        {
                            fileLoadResult?.WriteFileLoadResult(result);
                        }
                    }
                }
                InteractingDevice.cnc84Command.SetCncComand($"DSP,4/8_Press_The_Board");
                SendMessageToShow(4);
                await Task.Delay(2000);
                logger.LogWarning($"是否开启压板功能 ： {pressBoardOnOff} ");                        //PressBoardOnOff 是否开启压板功能
                if (pressBoardOnOff && !StartPressBoard())
                {
                    WriteWarningToPlc(12);
                    logger.LogDebug($"钻机给buffer报警编号：12");
                    errorNum = 3;
                    return;
                }

                if (!InteractingDevice.scanGunOnOff)                                              //ScanGunOnOff 扫码枪启用
                {
                    /*CNC95编写脚本绑定evAfterProgramAnalysis事件来给大小板标志位置位
                    string isSmallBoard = "0";
                    try
                    {
                        string bSizContent = File.ReadLines(WatchFilePath).Where(x => x.Contains(" BSIZ")).Last();
                        string[] arr = Regex.Split(bSizContent, " BSIZ");
                        if (arr.Length == 2 && arr[1].ToDouble() < DeviceDescriptor.Extra["SmallBoardMaxLength"].ToDouble())
                        {
                            isSmallBoard = "1";
                            logger.LogDebug($"读取的板长{arr[1].ToDouble()} 默认的 最小板的最大长度为{DeviceDescriptor.Extra["SmallBoardMaxLength"].ToDouble()}");
                        }
                    }
                    catch (Exception)
                    {
                        WriteWarningToPlc(29);
                        logger.LogDebug($"钻机给buffer报警编号：29");
                        logger.LogDebug("读取命令文件错误");
                    }
                    //SetUserFlag(int flag, int num)
                    //opcUaClient.WriteNode<Boolean>("ns=4;s=UI/normalized/custom/UF.SmallBoard", false);
                    InteractingDevice.cnc84Command.SetUserFlag(isSmallBoard.ToInt(), DeviceDescriptor.Extra["SmallBoardUserFlag"].ToInt());
                    logger.LogDebug($"设置小板用户标记 {isSmallBoard} ");
                    */
                }
                InteractingDevice.cnc84Command.SetCncComand($"DSP,5/8_Open_The_Mushroom");
                SendMessageToShow(5);
                await Task.Delay(2000);

                logger.LogWarning($"是否开启蘑菇头功能 ： 配置{mushroomOnOff} 实际{InteractingDevice.mushroomValue} ");                         //MushroomOnOff 是否开启蘑菇头功能
                if (InteractingDevice.mushroomValue && !StartOpenMushroomControllerNew())
                {
                    logger.LogWarning($"开始选蘑菇头");
                    WriteWarningToPlc(9);
                    logger.LogWarning($"钻机给buffer报警编号：9");
                    errorNum = 4;
                    return;
                }
                InteractingDevice.cnc84Command.SetCncComand($"DSP,6/8_Check_Program_Drl_Dia");
                SendMessageToShow(6);
                await Task.Delay(2000);

                logger.LogWarning($"是否检查程序和钻带功能 ： {checkProgramAndDiaOnOff}  自动模式 {DeviceDescriptor.AutoMode} ");

                if (DeviceDescriptor.AutoMode && checkProgramAndDiaOnOff)                         //CheckProgramAndDiaOnOff 是否检查程序和钻带功能
                {
                    var programPath = WatchingProperties.Property("Drill_CurrentProgramFile").NewValue.ToStr();
                    var diaPath = WatchingProperties.Property("Drill_CurrentParameterFile").NewValue.ToStr();

                    logger.LogWarning($"检查程序和钻带 ：钻带 {programPath}  参数 {diaPath} ");
                    if (DeviceDescriptor.AutoMode)
                    {
                        var result = await DataExporter.DeviceEventReport(
                       new DeviceEventReportRequest()
                       {
                           ProductId = DeviceDescriptor.ProductId,
                           DeviceId = DeviceDescriptor.DeviceId,
                           ClientId = InteractingDevice.ClientId,
                           EventId = Events.Drill.CHECK_PROGRAM_AND_DIA_EVENT,
                           EventName = "检查钻带参数和钻带文件请求",
                           RequestDeviceKind = DeviceKind.CNC95Drill,
                           Params = new Dictionary<string, object?>()
                           {
                                   { "ProgramPath", programPath },
                                   { "DiaPath", diaPath },
                           },
                           PayloadPanels = InteractingDevice.PayloadPanels,
                       });

                        if (result == null)
                        {
                            logger.LogWarning($"检查程序和钻带 和中控服务器断开连接了 ");
                            WriteWarningToPlc(300);
                        }
                        else if (result.Code != ErrorCodes.Sys.SUCCESS)
                        {
                            logger.LogWarning($"中控反馈的结果 不匹配 不能进行打板 ");
                            WriteWarningToPlc(301);
                            return;
                        }
                    }
                }
                InteractingDevice.cnc84Command.SetCncComand($"DSP,7/8_Check_Tool_Life");
                SendMessageToShow(7);
                await Task.Delay(2000);

                logger.LogWarning($"是否检查刀具寿命功能 ： {checkToolLifeOnOff} ");
                if (checkToolLifeOnOff && !CheckToolLife())                                       //CheckToolLifeOnOff 是否检查刀具寿命功能
                {
                    logger.LogWarning($"检查刀具寿命功能 ： 不满足当趟需求 ");
                    WriteWarningToPlc(302);
                    return;
                }
                InteractingDevice.cnc84Command.SetCncComand($"DSP,8/8_Drilling");
                SendMessageToShow(8);
                await Task.Delay(2000);

                logger.LogWarning($"是否自动开启打板 ： {drillBoardOnOff} ");

                if (drillBoardOnOff)                                                              //DrillBoardOnOff 是否自动开启打板
                {
                    /*scCncOEMBoardDirectionCheck//检查板方向，开旗标让CNC95的脚本去做
                     *
                     * int i= showDialog(DIALOG_QUESTION,"Please check the direction of the board!","Tip");
                     *
                     * Customer.BoardDirectionCheck=i ;
                     *
                     * return;
                     *
                     * IIoT->Customer.BoardDirectionCheck->Int64
                    */
                    /*
                    if (checkBoardDirectionOnOff)//2024-06-08zhushipeng 开启确认大小板方向功能（金禄、联锦成），开旗标让CNC95的脚本去做
                    {
                        InteractingDevice.cnc84Command.SetCncComand($"SCRP,scCncOEMBoardDirectionCheck");
                        while (true)
                        {
                            Int64 boardDirectionCheck = InteractingDevice.cnc84Command.ReadCncNode<Int64>("ns=4;s=UI/normalized/custom/Customer.BoardDirectionCheck");
                            //OK_BUTTON       1024        The button “OK” was used.
                            //YES_BUTTON      16384       The button “Yes” was used.
                            //NO_BUTTON       65536       The button “No” was used
                            //CANCEL_BUTTON   4194304     The button “Cancel” was used.
                            if (16384 == boardDirectionCheck)//YES_BUTTON
                            {
                                break;
                            }
                            if (65536 == boardDirectionCheck)//NO_BUTTON
                            {
                                break;
                            }
                            await Task.Delay(2000);
                        }
                        InteractingDevice.cnc84Command.WriteCncNode<Int64>("ns=4;s=UI/normalized/custom/Customer.BoardDirectionCheck", 0);
                    }
                    */
                    StartDrillBoard();
                    if (!ValidateStartCommandEnd())
                    {
                        bool flag = CheckStartStatusAndRestart();
                        if (!flag)
                        {
                            logger.LogWarning($"打板未能正常启动 ");
                            WriteWarningToPlc(200);
                            logger.LogWarning($"钻机给buffer报警编号：200");
                            errorNum = 5;
                            return;
                        }
                    }
                    logger.LogWarning($"钻机已开始打板");
                    InteractingDevice.cnc84Command.SetCncComand($"DSP,Drilling");
                }
                else
                {
                    InteractingDevice.cnc84Command.SetCncComand($"SDSP,Manual_Start_Drilling_The_Board");
                }
                await Task.CompletedTask;
            }
            catch (Exception ee)
            {
                logger.LogError($"buffer给钻机上完板子后续动作中发生异常 ： {ee.Message} ");
                errorNum = 6;
                try
                {
                    InteractingDevice.cnc84Command.SetCncComand($"DSP,Exception_Manual_Start_Drilling_The_Board");
                }
                catch (Exception e)
                {
                    logger.LogError($"DSP,Exception_Manual_Start_Drilling_The_Board异常 ： {e.Message} ");
                }
                await Task.CompletedTask;
            }
            finally
            {
                InteractingDevice.isOnDoingScript = false;
                SendMessageToShow(9, false);
                logger.LogError($"buffer给钻机上完板子后续动作异常动作编码 ： {errorNum} ");
                if (errorNum != 0)
                {
                    try
                    {
                        Thread.Sleep(500);
                        InteractingDevice.cnc84Command.SetCncComand($"SDSP,{GetErrorMessage(errorNum)}");
                    }
                    catch (Exception)
                    {
                        logger.LogError($"buffer给钻机上完板子后续动作异常动作SDSP 错误");
                    }
                }
            }
        }

        public string GetErrorMessage(int num)
        {
            switch (num)
            {
                case 1:
                    return "1_Send_Esc_Error";

                case 2:
                    return "2_Load_File_Error";

                case 3:
                    return "3_Press_Board__Error";

                case 4:
                    return "4_Mushroom_Error";

                case 5:
                    return "5_Unknown_ERROR";

                case 6:
                    return "6_One_Script_Error";

                case 7:
                    return "7_Select_Spindle_Error";
            }

            return "";
        }

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, int msg, int wParam, int lParam);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        public const int USER = 0X0400;
        public const int WM_OPEN = USER + 101;
        public const int WM_CLOSE = USER + 102;

        private Task SendMessageToShow(int eventId, bool isOpen = true)
        {
            return Task.Factory.StartNew(() =>
               {
                   try
                   {
                       try
                       {
                           string serverIp = "127.0.0.1"; // Server IP address
                           int port = DeviceDescriptor.Extra["ShowDialogPort"].ToInt();

                           Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                           try
                           {
                               clientSocket.Connect(serverIp, port);
                               logger.LogInformation("SendMessageToShow  Connected to server.");

                               string message = eventId.ToStr();
                               byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                               clientSocket.Send(messageBytes);
                               logger.LogInformation("SendMessageToShow  Sent message to server: " + message);
                           }
                           catch (Exception ex)
                           {
                               logger.LogInformation("Exception: " + ex.Message);
                           }
                           finally
                           {
                               clientSocket.Close();
                           }
                       }
                       catch (Exception ee)
                       {
                           logger.LogError($"发送提示窗体异常： {ee.Message} ");
                       }
                   }
                   catch (Exception ee)
                   {
                       logger.LogError($"发送提示窗体异常： {ee.Message} ");
                   }
               });
        }

        public bool PreStartOpenMushroomController()
        {
            var z1State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1");
            if (InteractingDevice.middleMushroomExist)
            {
                var z2State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2");
                logger.LogDebug($"提前动蘑菇头状态   初始状态  z1State {z1State}  z2State {z2State}");
                if (z1State && z2State) return true;
                //判断  动作
                logger.LogDebug($"提前动蘑菇头状态  第一次调用蘑菇头脚本->SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
                ControlMiddleMushroom("OPEN");
                logger.LogDebug($"提前动蘑菇头状态  第一次调用蘑菇头脚本->结束");
                if ((!z1State && !z2State) || (!z1State && z2State))
                {
                    logger.LogDebug($"提前动蘑菇头状态  调一次后进入验证状态");
                    return ValidateMiddleMushroom();
                }
                else
                {
                    logger.LogDebug($"提前动蘑菇头状态 进入二次判断分支");
                    ValidateFirstMiddleMushroom(z1State, z2State);
                    z1State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1");
                    z2State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2");
                    logger.LogDebug($"提前动蘑菇头状态 再次验证状态 z1State {z1State}  z2State {z2State}");
                    if (z1State && z2State) return true;
                    Thread.Sleep(1000);
                    logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 第二次调用蘑菇头脚本->SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
                    ControlMiddleMushroom("OPEN");
                    return ValidateMiddleMushroom();
                }
            }
            else
            {
                if (z1State) return true;
                //动作
                ControlFrontBackMushroom("OPEN");
                //验证
                return ValidateFrontBackMushroom();
            }
        }

        public bool PreOpenFourMushroomController()
        {
            //验证初始状态
            return ValidateInitMushroomState();
        }

        public bool ValidateFrontBackMushroom()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                Thread.Sleep(1000);
                var z1State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1");
                logger.LogDebug($"提前动蘑菇头 ValidateFrontBackMushroom z1State{z1State}");
                if (z1State)
                {
                    logger.LogDebug("提前动蘑菇头 前后蘑菇头结束，请等待...");
                    stopwatch.Stop();
                    logger.LogDebug($"提前动蘑菇头 机器【{DeviceDescriptor.DeviceName} 】验证前后蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateMushroomEndTimeOut"].ToLong());
            return false;
        }

        public bool ValidateFirstMiddleMushroom(bool z1State, bool z2State)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                //ns=4;s=UI/Enums/CommStatusPicklist_PICKLIST
                Thread.Sleep(1000);
                var z1StateCur = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1");
                var z2StateCur = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2");
                logger.LogDebug($"ValidateFirstMiddleMushroom 原z1State {z1State} 当前z1State{z1StateCur} 原z2State {z2State} 当前z2State{z2StateCur}");
                if (z1State != z1StateCur || z2State != z2StateCur)
                {
                    logger.LogDebug("蘑菇头状态发生变化 第一次执行结束");
                    stopwatch.Stop();
                    logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】蘑菇头状态发生变化：{stopwatch.ElapsedMilliseconds} 毫秒");
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < 6000);
            return false;
        }

        public bool ValidateMiddleMushroom()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                Thread.Sleep(1000);
                //验证蘑菇头是否打开结束CbdOpen_Z1FrontBack=CbdOpen_Z2Middle=false=已开
                var z1State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1");
                var z2State = InteractingDevice.cnc84Command.ReadCncNode<bool>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2");
                logger.LogDebug($"ValidateMiddleMushroom  z1State {z1State}  z2State {z2State}");
                if (z1State && z2State)
                {
                    logger.LogDebug("ValidateMiddleMushroom 前后中蘑菇头结束，请等待...");
                    stopwatch.Stop();
                    logger.LogDebug($" ValidateMiddleMushroom机器【{DeviceDescriptor.DeviceName} 】验证前后中蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateMushroomEndTimeOut"].ToLong());//90秒
            logger.LogDebug($"ValidateMiddleMushroom 机器【{DeviceDescriptor.DeviceName} 】验证前后中蘑菇头超时 用时：{stopwatch.ElapsedMilliseconds} 毫秒");
            return false;
        }
    }
}
