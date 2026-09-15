// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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

namespace VgDeviceGateway.Devices.Drill.EventHandler;

public class RearPanelIotDrillEventHandler : DeviceShare<DefaultDrill>, IDrillEventHandler
{
    private readonly ILogger<RearPanelIotDrillEventHandler> logger;
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

    public RearPanelIotDrillEventHandler(ILogger<RearPanelIotDrillEventHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
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
                           Task.Factory.StartNew((index) =>
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
                           Task.Factory.StartNew((index) =>
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
                  var boardPositionStatus = WatchingProperties.Property("Drill_BoardPositionStatus");
                  logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Drill_BoardPositionStatus  上生料到钻机 钻机上板子状态从{boardPositionStatus.OldValue.ToInt()}变成 {boardPositionStatus.NewValue.ToInt()} ");
                  if (!InteractingDevice.DeviceDescriptor.AutoMode) return;
                  ////控制板子转化
                  logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Transid 修改前  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");
                  InteractingDevice.DrillTransactionId = InteractingDevice.NewTranscationId;
                  InteractingDevice.NewTranscationId = "";
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
                   logger.LogWarning($"{DateTime.Now.ToShortTimeString()} Buffer_ClinkerLayerBoardStatus 下熟料到buffer   下层板子状态从{clinkerLayerBoardStatus.OldValue.ToInt()}变成 {clinkerLayerBoardStatus.NewValue.ToInt()} ");
                   logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  下熟料 未修改【 PayloadPanels 】 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
                   logger.LogWarning($"{DateTime.Now.ToShortTimeString()} 下熟料  Transid 修改前  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");
                   InteractingDevice.OldTransactionId = InteractingDevice.DrillTransactionId;
                   InteractingDevice.DrillTransactionId = "";
                   logger.LogWarning($"{DateTime.Now.ToShortTimeString()} 下熟料 Transid 修改后  {InteractingDevice.NewTranscationId}  {InteractingDevice.DrillTransactionId} {InteractingDevice.OldTransactionId}");
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

                   logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  下熟料  已修改【 PayloadPanels 】 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)} ");
               }
               catch (Exception)
               {
                   await Task.CompletedTask;
               }
               await Task.CompletedTask;
           });
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
                        InteractingDevice.modbusIpMaster?.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["Cnc84WorkEndWritePlc"].ToUshort(), false);
                        try
                        {
                            InteractingDevice.cnc84Command.SendCncComand("DSP,");
                        }
                        catch (Exception)
                        {


                        }


                    }
                }
                catch (Exception)
                {
                    await Task.CompletedTask;
                }
                await Task.CompletedTask;
            });

        WatchingProperties.Property("Buffer_Automatic")
           .PostCondition(p => p.IsValueChanged)
           .TriggerAlways(async () =>
           {
               var bufferAutomatic = WatchingProperties.Property("Buffer_Automatic").NewValue.ToBool();
               logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  buffer 自动状态 {bufferAutomatic}  ");

           });




        WatchingProperties.Property("Buffer_OnAgvPosition")
             .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
           .TriggerAlways(async () =>
           {
               logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-buffer到达agv对接层");

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
              TimeSpan timeSpan = DateTime.Now.Subtract(DrillOprationTime);
              if (timeSpan.TotalSeconds < InteractingDevice.CallAgvTimeInterval)
              {
                  logger.LogDebug("Drill_DrillHoleEnd 钻机和buffer整个上下料结束 重复进来");
                  return;
              }
              string mes = string.Empty;
              int status = 0;
              try
              {

                  logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}  钻机和buffer整个上下料结束 开始");

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
                      logger.LogWarning($"钻机给buffer报警编号：19");
                      mes = "解除钻机报警异常";
                      return;
                  }

                  logger.LogDebug($"{DateTime.Now.ToLongTimeString()}  钻机和buffer整个上下料结束 结束");
                  status = 1;
                  mes = "完成";
                  await Task.CompletedTask;
              }
              catch (Exception ee)
              {

                  InteractingDevice.cnc84Command.SetCncComand($"DSP,未知 异常");
                  mes = "buffer给钻机上完板子后续动作中发生异常";
                  logger.LogError($"buffer给钻机上完板子后续动作中发生异常 ： {ee.Message} ");
                  await Task.CompletedTask;
              }
              finally
              {
                  //logger.LogInformation($"buffer给钻机上完板子后续动作异常动作编码 ： {errorNum} ");
                  // 接口上报buffer将板材推送到钻机上面
                  if (InteractingDevice.ApplicationServices.TryGetService<IDrillLoadPanelToDrillComplete>(out IDrillLoadPanelToDrillComplete loadPanelToDrillComplete))
                  {
                      string result = string.Join(",", status, mes);
                      loadPanelToDrillComplete?.PanelToDrillComplete(result);
                  }
              }
          });
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

    public Task LoadFileFromCenter() => throw new NotImplementedException();
}
