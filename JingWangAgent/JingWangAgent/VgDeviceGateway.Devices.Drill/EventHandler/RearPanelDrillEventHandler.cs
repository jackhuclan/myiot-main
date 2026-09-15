// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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

namespace VgDeviceGateway.Devices.Drill.EventHandler
{
    public class RearPanelDrillEventHandler : DeviceShare<DefaultDrill>, IDrillEventHandler
    {
        private readonly ILogger<RearPanelDrillEventHandler> logger;
        public DateTime DrillOprationTime = DateTime.Now;
        public static readonly string WatchFilePath = @"C:\SMWDATA\PROTOCOL\PROTO.PRO";
        public static readonly string WeedOutFilePath = @"C:\SMWDATA\PROTOCOL\OEMPROTO.PRO";
        public readonly bool codeReaderOnOff;
        public bool mushroomOnOff;
        public bool pressBoardOnOff;
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
        public readonly bool outIntFlagFromIntOnOff;

        public RearPanelDrillEventHandler(ILogger<RearPanelDrillEventHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            codeReaderOnOff = device.DeviceDescriptor.Extra["CodeReaderOnOff"].ToBool();
            mushroomOnOff = device.DeviceDescriptor.Extra["MushroomOnOff"].ToBool();
            drillBoardOnOff = device.DeviceDescriptor.Extra["DrillBoardOnOff"].ToBool();
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
            outIntFlagFromIntOnOff = DeviceDescriptor.Extra["OutIntFlagFromIntOnOff"].ToBool();
        }

        public void AddWatchingEvents()
        {
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
                               _ = Task.Factory.StartNew((index) =>
                               {
                                   int codeIndex = (int)index;
                                   logger.LogInformation($"轴{codeIndex + 1} 触发读码 线程开始");
                                   InteractingDevice.codeReaderBaseSetting.SendScanMesssageToSignal(codeIndex, "1", true);
                                   Task.Delay(3000).Wait();
                                   do
                                   {
                                       bool curTrigger = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, (ushort)(InteractingDevice.DeviceDescriptor.Extra["CodeReaderTrigger"].ToUshort() + codeIndex), 1)[0];
                                       if (curTrigger)
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
                               _ = Task.Factory.StartNew((index) =>
                                {
                                    int codeIndex = (int)index;
                                    logger.LogInformation($"轴{codeIndex + 1} 触发读码 线程开始");
                                    InteractingDevice.codeReaderBaseSetting.SendScanMesssageToSignal(codeIndex, "1");
                                    Task.Delay(3000).Wait();
                                    do
                                    {
                                        ushort curTrigger = InteractingDevice.modbusIpMaster.ReadInputRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["CodeReaderTriggerD"].ToUshort(), 1)[0];
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
                            RequestDeviceKind = DeviceKind.CNC84Drill,
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
            //钻机上板子状态从0变成非0 表示buffer上层给钻机上料
            WatchingProperties.Property("Drill_BoardPositionStatus")
              .PostCondition(p => p.IsValueChanged && p.OldValue.ToInt() == 0 && p.NewValue.ToInt() > 0)
              .TriggerAlways(async () =>
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
              });
            //buffer 下层板子状态从0变成非0 表示钻机下熟料给buffer下层
            WatchingProperties.Property("Buffer_ClinkerLayerBoardStatus")
               .PostCondition(p => p.IsValueChanged && p.OldValue.ToInt() == 0 && p.NewValue.ToInt() > 0)
               .TriggerAlways(async () =>
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

                       if (drillPanels.All(s => string.IsNullOrEmpty(s.ItemCode)) || boardPositionStatus.NewValue.ToInt() != 0)
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
               });

            if (!DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool())
            {
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
                             var req = new FinishTaskRequest()
                             {
                                 ProductId = DeviceDescriptor.ProductId,
                                 DeviceId = DeviceDescriptor.DeviceId,
                                 TraceId = InteractingDevice.OldTransactionId,
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
            }
            else
            {
                WatchingProperties.Property("Drill_DrillHoleEnd")
                .PostCondition(p => p.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    var holeEnd = WatchingProperties.Property("Drill_DrillHoleEnd").NewValue.ToBool();
                    logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  钻孔结束信号 {holeEnd} ");
                    try
                    {
                        logger.LogInformation($"Drill_DrillHoleEnd 发生变化  InteractingDevice.ItemCode 清空");
                        InteractingDevice.ItemCode = string.Empty;
                        if (holeEnd)
                        {
                            //判断钻机是否有板子
                            var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["BufferLoadToDrillFlag"].ToUshort(), 0);
                            logger.LogDebug($"{DateTime.Now.ToShortTimeString()} 打板结束信号 钻机结束 buffer给agv上料信号 ");
                            InteractingDevice.modbusIpMaster.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["Cnc84WorkEndWritePlc"].ToUshort(), ready[0] != 0);
                            logger.LogDebug($"{DateTime.Now.ToShortTimeString()} 打板结束信号 钻机是否存在板子 {ready[0] != 0} ");

                            if (DeviceDescriptor.AutoMode && !string.IsNullOrWhiteSpace(InteractingDevice.OldTransactionId))
                            {
                                var req = new FinishTaskRequest()
                                {
                                    ProductId = DeviceDescriptor.ProductId,
                                    DeviceId = DeviceDescriptor.DeviceId,
                                    TraceId = InteractingDevice.OldTransactionId,
                                };
                                logger.LogDebug($"FinishTask:上报 任务完成： 请求参数：  {JsonSerializer.Serialize(req)}  ");
                                var result = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<FinishTaskRequest, FinishTaskResponse>(InteractingDevice.CentralWebOptions.FinishTask, req);
                                logger.LogDebug($"FinishTask:上报 任务完成： 返回：  {JsonSerializer.Serialize(result)}  ");
                            }
                        }
                        else
                        {
                            InteractingDevice.modbusIpMaster?.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["Cnc84WorkEndWritePlc"].ToUshort(), false);
                            if (DeviceDescriptor.AutoMode && !string.IsNullOrWhiteSpace(InteractingDevice.DrillTransactionId))
                            {
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
            }


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
                                    RequestDeviceKind = DeviceKind.CNC84Drill,
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

                        if (!CheckDrlDiaFile(drillPath, diaPath))
                        {
                            logger.LogError("配方路径文件不存在");
                            return;
                        }

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
                                {
                                    InteractingDevice.cnc84Command.SetUserFlag(1, DeviceDescriptor.Extra["SmallBoardUserFlag"].ToInt());
                                    logger.LogDebug($"设置小板用户标记 ");
                                }
                                else
                                {
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
                                RequestDeviceKind = DeviceKind.CNC84Drill,
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
                               RequestDeviceKind = DeviceKind.CNC84Drill,
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
                              RequestDeviceKind = DeviceKind.CNC84Drill,
                          });
                 }
             });

            WatchingProperties.Property("Buffer_Automatic")
               .PostCondition(p => p.IsValueChanged)
               .TriggerAlways(async () =>
               {
                   var bufferAutomatic = WatchingProperties.Property("Buffer_Automatic").NewValue.ToBool();
                   logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  buffer 自动状态 {bufferAutomatic}  ");
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
                                RequestDeviceKind = DeviceKind.CNC84Drill,
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
                                   RequestDeviceKind = DeviceKind.CNC84Drill,
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
                                RequestDeviceKind = DeviceKind.CNC84Drill,
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
                                RequestDeviceKind = DeviceKind.CNC84Drill,
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
                            RequestDeviceKind = DeviceKind.CNC84Drill,
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
                                RequestDeviceKind = DeviceKind.CNC84Drill,
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
                    catch (Exception ee)
                    {
                        logger.LogError($"钻机代理  解除总的上下料信号 ： {ee.Message} ");

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
                          logger.LogDebug("Drill_DrillHoleEnd 钻机和buffer整个上下料结束 重复进来");
                          return;
                      }
                      // 备份获取中控下发的itemcode
                      var centerLoadItemCode = InteractingDevice.ItemCode;
                      InteractingDevice.ItemCode = string.Empty;
                      logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}  备份中控下发的最新的 centerLoadItemCode {centerLoadItemCode}   InteractingDevice.ItemCode:{InteractingDevice.ItemCode} ");
                      DrillOprationTime = DateTime.Now;
                      logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}  钻机和buffer整个上下料结束");
                      var panel = InteractingDevice.PayloadPanels.Skip(InteractingDevice.spindleNum).Take(InteractingDevice.spindleNum).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s.ItemCode));
                      var itemCode = string.Empty; if (panel != null) { itemCode = panel.ItemCode; }
                      var escTimeout = DeviceDescriptor.Extra["ValidateEscCommandTimeOut"].ToInt();
                      logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}  esc ct时间 发送开始");
                      InteractingDevice.cnc84Command.SetCncComand($"DSP,取消报警中");
                      //errorNum
                      while (escTimeout > 0 && !InteractingDevice.EscFlag)
                      {
                          await Task.Delay(10);
                          escTimeout--;
                      }
                      logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}  esc ct时间 发送结束");
                      if (!InteractingDevice.EscFlag)
                      {
                          errorNum = 1;
                          logger.LogWarning($"钻机给buffer报警编号：19");
                          WriteWarningToPlc(19);
                          return;
                      }
                      logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}  选轴 ct时间 发送开始");
                      logger.LogDebug($"是否开启选轴功能 ： {selectSplineOnOff} ");
                      if (selectSplineOnOff)
                      {
                          InteractingDevice.cnc84Command.SetCncComand($"DSP,选轴中");
                          if (!RetractTool())
                          {
                              errorNum = 2;
                              logger.LogError($"选轴的时候先退刀，退刀失败 ");
                              InteractingDevice.cnc84Command.SetCncComand($"DSP,选轴的时候先退刀，退刀失败");
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
                      logger.LogDebug($"{DateTime.Now.ToLongTimeString()}  选轴 ct时间 发送结束");
                      logger.LogDebug($"是否向中控要加载程序和参数 ： {loadFileFromCentreOnOff}  自动模式 {DeviceDescriptor.AutoMode} ");
                      if (DeviceDescriptor.AutoMode && loadFileFromCentreOnOff)
                      {
                          int loadError = 0;
                          try
                          {
                              if ((DeviceDescriptor.Extra["AgvOperationTypes"].ToString() == "8"))
                              {
                                  if (string.IsNullOrWhiteSpace(itemCode))
                                  {
                                      logger.LogError($"itemCode 为空 中控下发的 备份 centerLoadItemCode {centerLoadItemCode}");
                                      itemCode = centerLoadItemCode;
                                  }

                                  if (string.IsNullOrWhiteSpace(itemCode))
                                  {
                                      logger.LogError("itemCode 未空不能和ftp服务交互");
                                      WriteWarningToPlc(11);
                                      InteractingDevice.cnc84Command.SetCncComand($"DSP,itemCode 为空 异常 ,请手动加载钻带 ");
                                      loadError = 1;
                                      return;
                                  }
                                  InteractingDevice.ItemCode = string.Empty;
                                  logger.LogError($"单次加载后 InteractingDevice.ItemCode {InteractingDevice.ItemCode} 使用完清空");
                                  DrillInfo drillInf;
                                  try
                                  {
                                      drillInf = InteractingDevice.DrillFilePathLocator.GetFilePath($"{itemCode}");
                                  }
                                  catch (Exception ee)
                                  {
                                      InteractingDevice.cnc84Command.SetCncComand($"DSP,{ee.Message} 异常 ,请手动加载钻带");
                                      loadError = 2;
                                      return;
                                  }
                                  string drillPath = drillInf.DrlPath;
                                  string diaPath = drillInf.DiaPath;

                                  if (!CheckDrlDiaFile(drillPath, diaPath))
                                  {
                                      loadError = 3;
                                      logger.LogError("配方路径文件不存在");
                                      InteractingDevice.cnc84Command.SetCncComand($"DSP,钻带文件不存在 异常 ,请手动加载");
                                      return;
                                  }

                                  if (!LoadFileNoBsizToCNC84(drillPath, diaPath))
                                  {
                                      loadError = 4;
                                      logger.LogError($"加载程序到CNC84 失败");
                                      InteractingDevice.cnc84Command.SetCncComand($"DSP,加载程序到CNC84 异常 手动加载");
                                      return;
                                  }
                              }
                              else
                              {
                                  InteractingDevice.cnc84Command.SetCncComand($"DSP,中控交互加载钻带中");
                                  if (string.IsNullOrWhiteSpace(itemCode))
                                  {
                                      logger.LogError("itemCode 未空不能和中控服务交互");
                                      WriteWarningToPlc(7);
                                      InteractingDevice.cnc84Command.SetCncComand($"DSP,itemCode 为空 异常 ,请手动加载钻带 ");
                                      loadError = 1;
                                      return;
                                  }

                                  DrillInfo drillInf;
                                  try
                                  {
                                      drillInf = InteractingDevice.DrillFilePathLocator.GetFilePath($"{itemCode};{DeviceDescriptor.DeviceId}");
                                  }
                                  catch (Exception ee)
                                  {
                                      Thread.Sleep(1000);
                                      InteractingDevice.cnc84Command.SetCncComand($"DSP,{ee.Message}");
                                      logger.LogError($"DrillFilePathLocator 异常 {ee.Message}");
                                      WriteWarningToPlc(7);
                                      return;
                                  }

                                  // 获取 diaFilePath 的值
                                  string drillPath = drillInf.DrlPath;
                                  string diaPath = drillInf.DiaPath;

                                  if (!LoadFileToCNC84(drillPath, diaPath))
                                  {
                                      loadError = 6;
                                      logger.LogDebug($"加载程序到CNC84 失败 异常");
                                      InteractingDevice.cnc84Command.SetCncComand($"DSP,加载程序到CNC84 失败 异常");
                                      return;
                                  }
                              }
                          }
                          catch (Exception ee)
                          {
                              Thread.Sleep(1000);
                              logger.LogDebug($"加载文件异常{ee.Message}");
                              InteractingDevice.cnc84Command.SetCncComand($"DSP, 加载文件失败");
                              loadError = 7;
                              WriteWarningToPlc(7);
                              return;
                          }
                          finally
                          {
                              if (loadError != 0)
                              {
                                  errorNum = 3;
                              }
                          }
                      }
                      logger.LogDebug($"是否开启压板功能 ： {pressBoardOnOff} ");
                      logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}  压板 ct时间 发送开始");
                      if (pressBoardOnOff && !StartPressBoard())
                      {
                          errorNum = 4;
                          InteractingDevice.cnc84Command.SetCncComand($"DSP,压板 异常");
                          WriteWarningToPlc(12);
                          return;
                      }
                      logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}  压板 ct时间 发送结束");

                      #region old bsiz

                      //if (!InteractingDevice.scanGunOnOff && mushroomOnOff && InteractingDevice.middleMushroomExist)
                      //{
                      //    string isSmallBoard = "0";
                      //    try
                      //    {
                      //        var bSizContent = File.ReadLines(WatchFilePath).Where(x => x.Contains(" BSIZ")).LastOrDefault();
                      //        if (bSizContent == null)
                      //        {
                      //            var direct = Path.GetDirectoryName(WatchFilePath);
                      //            DirectoryInfo directoryInfo = new DirectoryInfo(direct);
                      //            FileInfo[] files = directoryInfo.GetFiles("*.pro").Where(fn => fn.FullName != WatchFilePath).Where(fn => fn.FullName != WeedOutFilePath).OrderByDescending(f => f.CreationTime).ToArray();

                      //            for (int i = 0; i < files.Length; i++)
                      //            {
                      //                bSizContent = File.ReadLines(files[i].FullName).Where(x => x.Contains(" BSIZ")).LastOrDefault();
                      //                if (bSizContent != null)
                      //                {
                      //                    break;
                      //                }
                      //            }

                      //        }
                      //        if (bSizContent == null)
                      //        {
                      //            throw new Exception("文件中没找到 板子长度");
                      //        }

                      //        string[] arr = Regex.Split(bSizContent, " BSIZ");
                      //        if (arr.Length == 2 && arr[1].ToDouble() < DeviceDescriptor.Extra["SmallBoardMaxLength"].ToDouble())
                      //        {
                      //            isSmallBoard = "1";
                      //            logger.LogDebug($"读取的板长{arr[1].ToDouble()} 默认的 最小板的最大长度为{DeviceDescriptor.Extra["SmallBoardMaxLength"].ToDouble()}");
                      //        }

                      //    }
                      //    catch (Exception ee)
                      //    {
                      //        WriteWarningToPlc(29);
                      //        logger.LogError($"获取板长 读取命令文件错误 {ee.Message}");

                      //    }
                      //    InteractingDevice.cnc84Command.SetUserFlag(isSmallBoard.ToInt(), DeviceDescriptor.Extra["SmallBoardUserFlag"].ToInt());
                      //    logger.LogDebug($"设置小板用户标记 {isSmallBoard} ");
                      //}

                      #endregion old bsiz

                      logger.LogDebug($"是否开启蘑菇头功能 ： {mushroomOnOff} ");
                      logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}  蘑菇头 ct时间 发送开始");
                      if (mushroomOnOff && !StartOpenMushroomController())
                      {
                          errorNum = 5;
                          InteractingDevice.cnc84Command.SetCncComand($"DSP,蘑菇头 异常");
                          WriteWarningToPlc(9);
                          return;
                      }
                      logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}  蘑菇头 ct时间 发送结束");
                      logger.LogDebug($"是否检查程序和钻带功能 ： {checkProgramAndDiaOnOff}  自动模式 {DeviceDescriptor.AutoMode} ");

                      if (DeviceDescriptor.AutoMode && checkProgramAndDiaOnOff)
                      {
                          var programPath = WatchingProperties.Property("Drill_CurrentProgramFile").NewValue.ToStr();
                          var diaPath = WatchingProperties.Property("Drill_CurrentParameterFile").NewValue.ToStr();

                          logger.LogDebug($"检查程序和钻带 ：钻带 {programPath}  参数 {diaPath} ");
                          var result = await DataExporter.DeviceEventReport(
                             new DeviceEventReportRequest()
                             {
                                 ProductId = DeviceDescriptor.ProductId,
                                 DeviceId = DeviceDescriptor.DeviceId,
                                 ClientId = InteractingDevice.ClientId,
                                 EventId = Events.Drill.CHECK_PROGRAM_AND_DIA_EVENT,
                                 EventName = "检查钻带参数和钻带文件请求",
                                 RequestDeviceKind = DeviceKind.CNC84Drill,
                                 Params = new Dictionary<string, object?>()
                                 {
                                   { "ProgramPath", programPath },
                                   { "DiaPath", diaPath },
                                 },
                                 PayloadPanels = InteractingDevice.PayloadPanels,
                             });

                          if (result == null)
                          {
                              logger.LogDebug($"检查程序和钻带 和中控服务器断开连接了 ");
                              WriteWarningToPlc(300);
                              errorNum = 6;
                              return;
                          }
                          else if (result.Code != ErrorCodes.Sys.SUCCESS)
                          {
                              logger.LogDebug($"中控反馈的结果 不匹配 不能进行打板 ");
                              WriteWarningToPlc(301);
                              errorNum = 6;
                              return;
                          }
                      }

                      logger.LogDebug($"是否检查刀具寿命功能 ： {checkToolLifeOnOff} ");
                      if (checkToolLifeOnOff && !CheckToolLife())
                      {
                          logger.LogDebug($"检查刀具寿命功能 ： 不满足当趟需求 ");
                          WriteWarningToPlc(302);
                          errorNum = 7;
                          return;
                      }

                      logger.LogDebug($"是否自动开启打板 ： {drillBoardOnOff} ");
                      logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}  自动打板 ct时间 发送开始");
                      if (drillBoardOnOff)
                      {
                          InteractingDevice.cnc84Command.SetCncComand($"DSP,自动打板中");
                          if (existWhiteSpaceConfirm) OpenCNCWindow();
                          StartDrilBoard();
                          if (existWhiteSpaceConfirm && IsShowConfirmMessage())
                          {
                              keybd_event('Y', 0, 0, 0);
                              keybd_event('Y', 0, 2, 0);
                          }
                          Thread.Sleep(1000);
                          if (!ValidateStartCommandEnd())
                          {
                              bool flag = CheckStartStatusAndRestart();
                              if (!flag)
                              {
                                  logger.LogDebug($"打板未能正常启动 ");
                                  WriteWarningToPlc(200);
                                  errorNum = 8;
                                  return;
                              }
                          }
                          InteractingDevice.cnc84Command.SetCncComand($"DSP,");
                          logger.LogDebug($"钻机已开始打板");
                      }
                      else
                      {
                          InteractingDevice.cnc84Command.SetCncComand($"SDSP,请手动开启打板");
                      }
                      logger.LogDebug($"{DateTime.Now.ToLongTimeString()}  自动打板 ct时间 发送结束");
                      await Task.CompletedTask;
                  }
                  catch (Exception ee)
                  {
                      errorNum = 9;
                      InteractingDevice.cnc84Command.SetCncComand($"DSP,未知 异常");
                      logger.LogError($"buffer给钻机上完板子后续动作中发生异常 ： {ee.Message} ");
                      await Task.CompletedTask;
                  }
                  finally
                  {
                      logger.LogInformation($"buffer给钻机上完板子后续动作异常动作编码 ： {errorNum} ");
                      if (errorNum != 0)
                      {
                          try
                          {
                              Thread.Sleep(1000);
                              InteractingDevice.cnc84Command.SetCncComand($"SDSP,{GetErrorMessage(errorNum)}");
                          }
                          catch (Exception)
                          {
                              logger.LogError($"buffer给钻机上完板子后续动作异常动作SDSP 错误");
                          }

                          SendMessageToShow(errorNum);
                      }
                  }
              });
        }

        #region SendMessage

        public string GetErrorMessage(int num)
        {
            switch (num)
            {
                case 1:
                    return "1_发送esc异常";

                case 2:
                    return "2_选轴异常";

                case 3:
                    return "3_加载文件异常";

                case 4:
                    return "4_压板异常";

                case 5:
                    return "5_蘑菇头异常";

                case 6:
                    return "6_二次检查配方异常";

                case 7:
                    return "7_刀具寿命异常";

                case 8:
                    return "8_启动打板异常";

                case 9:
                    return "9_未知异常";
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
            });
        }

        #endregion SendMessage

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

        private bool GetDrillInfFromCentreByHttp(string itemCode)
        {
            // "GetDrillRecipes": "http://192.168.102.178:8001/v1/central/device/GetDrillRecipes?deviceId={0}&lot={1}",

            return true;
        }

        private bool LoadFileNoBsizToCNC84(string drillPath, string diaPath = "")
        {
            InteractingDevice.LoadFileFlag = false;
            if (!LoadFile(drillPath, diaPath))
            {
                WriteWarningToPlc(11);
                return false;
            }
            InteractingDevice.LoadFileFlag = true;
            return true;
        }

        private bool LoadFileToCNC84(string drillPath, string diaPath = "")
        {
            InteractingDevice.LoadFileFlag = false;
            if (!LoadFileCommon(drillPath, diaPath))
            {
                WriteWarningToPlc(11);
                return false;
            }
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
                var response = await HttpRequestInvoker.GetFromJsonAsync<ScheduledTaskStatus>(string.Format(InteractingDevice.QueryScheduleUrl, InteractingDevice.NewTranscationIdTemp));
                logger.LogDebug($"QuerySchedule, {JsonSerializer.Serialize(response)}");
                if (response == ScheduledTaskStatus.Completed || response == ScheduledTaskStatus.Failed || response == ScheduledTaskStatus.Canceled)
                {
                    return true;
                }
                return false;
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
            if (DeviceDescriptor.AutoMode)
            {
                var req = new BeginTaskRequest()
                {
                    ProductId = DeviceDescriptor.ProductId,
                    DeviceId = DeviceDescriptor.DeviceId,
                    TraceId = InteractingDevice.DrillTransactionId,
                    RealCount = count
                };
                logger.LogDebug($"BeginTask:上报 任务开始： 请求参数：  {JsonSerializer.Serialize(req)}  ");
                var result = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<BeginTaskRequest, BeginTaskResponse>(InteractingDevice.CentralWebOptions.BeginTask, req);
                logger.LogDebug($"BeginTask:上报 任务开始 返回值：  {JsonSerializer.Serialize(result)}  ");
            }
        }

        private void RestartCnc()
        {
            InteractingDevice.cnc84Command.Start();
            logger.LogDebug($" RestartCnc 发送开始指令 ");
            Thread.Sleep(2000);
        }

        private bool CheckStartStatus()
        {
            try
            {
                if (outIntFlagFromIntOnOff)
                {
                    //获取打板开始指令
                    string endFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["DrilBoardEndFlag"].ToInt(), true);
                    logger.LogDebug($"启动过程中检测： int  钻孔结束信号 {DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool()}：{endFlag}");
                    string result = "0";
                    if (!DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool())
                    {
                        result = "1";
                    }
                    var drillHoleStart = result.Equals(endFlag);
                    logger.LogDebug($"启动过程中检测：int 钻孔开始信号：{drillHoleStart}");
                    return drillHoleStart;
                }
                else
                {
                    //获取打板开始指令
                    string endFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["DrilBoardEndFlag"].ToInt());
                    logger.LogDebug($"启动过程中检测： 钻孔结束信号 {DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool()}：{endFlag}");
                    string result = "1:0";
                    if (!DeviceDescriptor.Extra["IsDrilBoardEndFlag"].ToBool())
                    {
                        result = "1:1";
                    }
                    var drillHoleStart = result.Equals(endFlag);
                    logger.LogDebug($"启动过程中检测： 钻孔开始信号：{drillHoleStart}");
                    return drillHoleStart;
                }

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
                    if (!existWhiteSpaceConfirm || !IsShowConfirmMessage())
                    {
                        RestartCnc();
                    }
                    else
                    {
                        keybd_event('Y', 0, 0, 0);
                        keybd_event('Y', 0, 2, 0);
                    }

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
            logger.LogDebug($"发送 压板指令 m102");
            InteractingDevice.cnc84Command.SetCncComand("M102");
        }

        private void StartThreeColorLights()
        {
            InteractingDevice.cnc84Command.SetCncComand("M113");
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

        public void StartDrilBoard()
        {
            logger.LogDebug("开始切换界面");
            Thread.Sleep(2000);
            StartChangeF8();
            Thread.Sleep(2000);
            logger.LogDebug($" 发送开始指令 ");
            InteractingDevice.cnc84Command.Start();
            Thread.Sleep(2000);
        }

        public bool IsShowConfirmMessage()
        {
            var vgCNCScreenSaver = InteractingDevice.cnc84Command.GetScreenText();
            if (vgCNCScreenSaver != null && vgCNCScreenSaver.ScreenText != null)
            {
                return vgCNCScreenSaver.ScreenText.Contains($"[{DeviceDescriptor.Extra["WhiteSpaceConfirmIndex"].ToStr()}]");
            }

            return false;
        }

        private void StartChangeF8()
        {
            InteractingDevice.cnc84Command.SetChangePage("WORK_WORK");
        }

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

                logger.LogDebug($"蘑菇头 程序的板长是{ySize} p3:{p3} p1:{p1}  配置小板的最大长度是{DeviceDescriptor.Extra["SmallBoardMaxLength"].ToDouble()} ");//
                if (ySize <= DeviceDescriptor.Extra["SmallBoardMaxLength"].ToDouble())
                {
                    result = "1";
                }

                if ("1".Equals(result, StringComparison.CurrentCultureIgnoreCase))
                {
                    logger.LogDebug("有中间蘑菇头  当前板子是小板 中间蘑菇需要打开！");
                    if (!StartOpenMiddleMushroom(true)) return false;
                }
                else
                {
                    logger.LogDebug("有中间蘑菇头  当前板子是大板 中间蘑菇不需要打开！");
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
                    logger.LogDebug("当前 前后蘑菇头 或者中间蘑菇头 已经打开，无法进行操作！");
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
                        logger.LogDebug("前后中蘑菇头结束，请等待...");
                        stopwatch.Stop();
                        logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】验证前后中蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
                        return true;
                    }
                } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateMushroomEndTimeOut"].ToLong());
            }
            else
            {
                //判断当前蘑菇头位置
                if (!MushroomFrontBackCloseLocalTion())
                {
                    logger.LogDebug("当前 前后蘑菇头已经打开，无法进行操作！");
                    return false;
                }
                logger.LogDebug($"大板的时候 是否存在中间蘑菇头 {InteractingDevice.middleMushroomExist}");
                if (InteractingDevice.middleMushroomExist && !MushroomMiddleCloseLocalTion())
                {
                    logger.LogDebug("大板的时候 检查  中间蘑菇头已经打开，无法进行操作！");
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
                        logger.LogDebug("前后蘑菇头结束，请等待...");
                        stopwatch.Stop();
                        logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】验证前后蘑菇头用时：{stopwatch.ElapsedMilliseconds} 毫秒");
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
            logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送 开启中间 {str}蘑菇头指令");
        }

        public void ControlFrontBackMushroom(string str)
        {
            InteractingDevice.cnc84Command.SetCncComand("M27");
            logger.LogDebug($"机器【{DeviceDescriptor.DeviceName} 】 发送{str}蘑菇头指令");
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
            if (outIntFlagFromIntOnOff)
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

        public void StartSelectSpline()
        {
            try
            {
                logger.LogDebug("开始选择轴");
                var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);
                InteractingDevice.cnc84Command.SetCncComand("SZSA");
                logger.LogDebug("发送SZSA");
                Thread.Sleep(2000);
                var rawStatus = Convert.ToString(ready[0], 2).PadLeft(InteractingDevice.spindleNum, '0').Reverse().ToList();
                logger.LogDebug($"开始选择轴的时候 钻机上面板子 {string.Join("", rawStatus)}");
                if (rawStatus.Any(s => s == '0'))
                {
                    int index = 0; int command = selectSplineMFunction;
                    rawStatus.ForEach(s =>
                    {
                        if (s == '0')
                        {
                            logger.LogDebug($"开始选择轴的时候 发送指令 M{command + index}");
                            InteractingDevice.cnc84Command.SetCncComand($"M{command + index}");
                            Thread.Sleep(3000);
                        }
                        index++;
                    });
                }
            }
            catch (Exception ee)
            {
                logger.LogDebug($"钻机选轴异常：{ee.Message}");
            }
        }

        public T DeepCopy<T>(T obj)
        {
            var stringObj = JsonSerializer.Serialize(obj);
            return (T)JsonSerializer.Deserialize(stringObj, typeof(T));
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
                var panel = payloadPanelsTemp.Skip(InteractingDevice.spindleNum * 2).Take(InteractingDevice.spindleNum).FirstOrDefault(s => !string.IsNullOrEmpty(s.ItemCode));
                var ItemCode = panel == null ? "222222" : panel.ItemCode;
                logger.LogError($"可能补的板材信息   {ItemCode} ");
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
                              RequestDeviceKind = DeviceKind.CNC84Drill,
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
                                    { "RegulatePos", DeviceDescriptor.Extra["RegulatePos"].ToStr() },
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
                        }

                        logger.LogDebug($"上报信息结果：  {JsonSerializer.Serialize(result)}  ");
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
                        //获取ItemCode

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
                                 RequestDeviceKind = DeviceKind.CNC84Drill,
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
                                        { "RegulatePos", DeviceDescriptor.Extra["RegulatePos"].ToStr() },
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
                        }

                        logger.LogDebug($"上报信息结果：  {JsonSerializer.Serialize(result)}  ");
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
                              RequestDeviceKind = DeviceKind.CNC84Drill,
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
                                    { "RegulatePos", DeviceDescriptor.Extra["RegulatePos"].ToStr() },
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
                        }
                        logger.LogDebug($"上报信息结果：  {JsonSerializer.Serialize(result)}  ");
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
            var Cnc84DiaFileOnOff = DeviceDescriptor.Extra["Cnc84DiaFileOnOff"].ToBool();

            if (!File.Exists(DrlFilePath_tmp))
            {
                WriteWarningToPlc(7);
                logger.LogDebug("请检查是否有生产程序文件!");
                return false;
            }

            if (Cnc84DiaFileOnOff && !File.Exists(DiaFilePath_tmp))
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

        public bool LoadFileCommon(string drilPath, string? diaPath = "")
        {
            if (!string.IsNullOrWhiteSpace(diaPath) && !ValidateDiaFileSame(diaPath))
            {
                InteractingDevice.cnc84Command.SetLoadFile(diaPath);
                if (!ValidateDiaFile(diaPath))
                {
                    return false;
                }
            }
            //判断是否相同 不同加载
            if (!string.IsNullOrWhiteSpace(drilPath) && !ValidateDrlFileSame(drilPath))
            {
                InteractingDevice.cnc84Command.SetLoadFile(drilPath);
                if (!ValidateDrlFile(drilPath))
                {
                    return false;
                }
                logger.LogDebug($"ValidateDrlFile  成功");
                if (!ValidateDrlImg())
                {
                    return false;
                }
                logger.LogDebug($"ValidateDrlImg  成功");
            }

            return true;
        }

        public bool LoadFile(string drilPath, string? diaPath = "")
        {
            InteractingDevice.cnc84Command.SetCncComand("CM@@@");
            if (!ValidateCmDrlFile())
            {
                return false;
            }
            if (!string.IsNullOrWhiteSpace(diaPath) && !ValidateDiaFileSame(diaPath))
            {
                InteractingDevice.cnc84Command.SetLoadFile(diaPath);
                if (!ValidateDiaFile(diaPath))
                {
                    return false;
                }
            }
            InteractingDevice.cnc84Command.SetLoadFile(drilPath);
            if (!ValidateDrlFile(drilPath))
            {
                return false;
            }
            logger.LogDebug($"ValidateDrlFile  成功");
            if (!ValidateDrlImg())
            {
                return false;
            }
            logger.LogDebug($"ValidateDrlImg  成功");
            return true;
        }

        private bool ValidateDrlImg()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            do
            {
                Thread.Sleep(1000);
                var screenSaver = InteractingDevice.cnc84Command.GetScreenSaver();
                logger.LogDebug($"ValidateDrlImg  {screenSaver?.ScreenText}");
                if (!string.IsNullOrEmpty(screenSaver?.ScreenText) && screenSaver.ScreenText.Contains("[2000]"))
                {
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateCmFileTimeout"].ToLong());
            return false;
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

        private bool ValidateDiaFileSame(string diaFilePath)
        {
            var diaName = InteractingDevice.cnc84Command.GetRuntimeString("%S(DiaFileNameWithDialog)");
            if ($"1:{diaFilePath}".Equals(diaName, StringComparison.OrdinalIgnoreCase))
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
                diaName = InteractingDevice.cnc84Command.GetRuntimeString("%S(DiaFileNameWithDialog)");
                if ($"1:{diaFilePath}".Equals(diaName, StringComparison.OrdinalIgnoreCase))
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
                netProName = InteractingDevice.cnc84Command.GetCncStatus()?.ProgramName.ToLower();
                if (drilFilePath.ToLower().Equals(netProName, StringComparison.CurrentCultureIgnoreCase))
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
