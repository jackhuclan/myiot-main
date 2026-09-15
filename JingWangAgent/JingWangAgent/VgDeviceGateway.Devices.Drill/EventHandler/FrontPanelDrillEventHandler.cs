using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.EventHandler
{
    public class FrontPanelDrillEventHandler : DeviceShare<DefaultDrill>, IDrillEventHandler
    {
        private readonly ILogger<FrontPanelDrillEventHandler> logger;
        public DateTime DrillOprationTime = DateTime.Now;
        public static readonly string WatchFilePath = @"C:\SMWDATA\PROTOCOL\PROTO.PRO";
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
        public bool isRunlastStep = false;

        public FrontPanelDrillEventHandler(ILogger<FrontPanelDrillEventHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
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
        }

        public void AddWatchingEvents()
        {
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

            WatchingProperties.Property("IsLastStep")
            .PostCondition(p => p.NewValue.ToInt() == 1 && p.IsValueChanged)
            .TriggerAlways(() =>
            {
                isRunlastStep = true;
                //顶升降
                var result = InteractingDevice.frontExtendDevice.ControlUpDown(false);
                if (!result)
                {
                    logger.LogDebug($"\r\n顶升也并未全部降下来 \r\n");
                    return;
                }

                logger.LogDebug($"\r\n最后一步完成 执行打板 \r\n");
            });
            WatchingProperties.Property("Drill_WorkStart")
              .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  logger.LogDebug($"\r\n钻机变成工作状态\r\n");
                  await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                  {
                      PayloadPanels.ForEach(panel =>
                      {
                          if (panel.ProductStatus != ProductStatus.EmptySiloBox)
                              panel.ProductStatus = ProductStatus.Drilling;
                      });
                  }));
              });
            WatchingProperties.Property("Drill_WorkEnd")
              .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  try
                  {
                      logger.LogDebug($"\r\n钻机变成非工作状态\r\n");
                      var onWork = (InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["WorkFlag"].ToInt()) == "1:1");
                      logger.LogDebug($"\r\n再次确认钻机的工作状态 {onWork} \r\n");
                      if (!onWork)
                      {
                          await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                          {
                              PayloadPanels.ForEach(panel =>
                              {
                                  if (panel.ProductStatus == ProductStatus.Drilling)
                                  {
                                      panel.ProductStatus = ProductStatus.Finished_DRILL;
                                  }
                              });
                          }));
                          await FirstStep();
                      }
                      else
                      {
                          logger.LogInformation($"---钻机在工作状态不能执行顶升不能开启门\n\r--");
                      }
                  }
                  catch (Exception ee)
                  {
                      logger.LogDebug($"\r\n钻机在Drill_OnNoWork 事件中发生异常{ee.Message}\r\n");
                  }
              });
            WatchingProperties.Properties("AllUnloadAndLoadEnd", "MaterialEnsureFlag", "Drill_WorkEnd", "Drill_AllUpDownDownFlag")
            .When(p =>
            {
                var allUnloadAndLoadEnd = p.Property("AllUnloadAndLoadEnd");
                var materialEnsureFlag = p.Property("MaterialEnsureFlag");
                var drillWorkEnd = p.Property("Drill_WorkEnd");
                var allUpDownDownFlag = p.Property("Drill_AllUpDownDownFlag");

                return (materialEnsureFlag.NewValue.ToInt() == 1) && drillWorkEnd.NewValue.ToBool() && allUpDownDownFlag.NewValue.ToBool() && (allUnloadAndLoadEnd.NewValue.ToInt() == 1) && PayloadPanels.Count(item => item.ProductStatus == ProductStatus.WaitingForDrill) == InteractingDevice.spindleNum;
            }).TriggerAlways(async () =>
            {
                if (isRunlastStep)
                {
                    isRunlastStep = false;
                    await LastStep();
                    logger.LogDebug($"\r\n最后一步完成 执行打板 \r\n");
                }
            });
            //呼叫agv条件
            WatchingProperties.Properties("MaterialEnsureFlag", "Drill_WorkStart", "Drill_AllUpDownUpFlag", "Drill_DoorFlag", "Drill_SplineStatus", "Drill_PanelInfFail", "Drill_ClampOpenFlag")
            .When(p =>
            {
                var materialEnsureFlag = p.Property("MaterialEnsureFlag");
                var drillWorkStart = p.Property("Drill_WorkStart");
                var allUpFlag = p.Property("Drill_AllUpDownUpFlag");
                var doorFlag = p.Property("Drill_DoorFlag");
                var splineStatus = p.Property("Drill_SplineStatus").NewValue.ToStr();
                var panelInfFail = p.Property("Drill_PanelInfFail").NewValue.ToBool();
                var clampOpenFlag = p.Property("Drill_ClampOpenFlag").NewValue.ToBool();

                // 0 无  1 生 2钻 3 熟

                bool existRawPanel = PayloadPanels.Any(s => s.ProductStatus == ProductStatus.WaitingForDrill);
                bool existDrillingPanel = PayloadPanels.Any(s => s.ProductStatus == ProductStatus.Drilling);
                var isExistClinker = PayloadPanels.Any(s => s.ProductStatus == ProductStatus.Finished_DRILL);
                var isRawFall = PayloadPanels.Count(s => s.ProductStatus == ProductStatus.WaitingForDrill) == InteractingDevice.spindleNum;

                logger.LogDebug($"轴使用使用状态{InteractingDevice.SplineNoBrokenStatus} 生料个数{PayloadPanels.Count(s => s.ProductStatus == ProductStatus.WaitingForDrill)}  生料是否满 {isRawFall} 是否存在熟料{isExistClinker}");

                TimeSpan timeSpan = DateTime.Now.Subtract(InteractingDevice.AgvEndTime);
                var agvEndSpanTime = timeSpan.TotalSeconds < InteractingDevice.AgvEndTimeInterval;

                if (agvEndSpanTime)
                {
                    logger.LogDebug($"AGV给钻机上料结束刚结束 不能呼叫 ");
                }

                bool result = (materialEnsureFlag.NewValue.ToInt() == 1) && !drillWorkStart.NewValue.ToBool() && (isExistClinker || (!isRawFall)) && InteractingDevice.allowAllAgv && allUpFlag.NewValue.ToBool() && InteractingDevice.AgvOnWorkDrillStatus == 0 && doorFlag.NewValue.ToBool() && clampOpenFlag
                && DeviceDescriptor.AutoMode && InteractingDevice.MqttClientWrapper.IsConnected;
                if (result)
                {
                    InteractingDevice.CallCondition = $"{DateTime.Now.ToString()}:  满足条件能发起呼叫 轴使用使用状态{InteractingDevice.SplineNoBrokenStatus} 钻机板子状态{splineStatus}  板材信息{string.Join(",", PayloadPanels.Select(s => s.ProductStatus).ToArray())}";
                }
                else
                {
                    List<string> message = new List<string>();
                    if (materialEnsureFlag.NewValue.ToInt() != 1) message.Add("未确认板材信息");
                    if (panelInfFail) message.Add("板材信息设置不正确");
                    if (drillWorkStart.NewValue.ToBool()) message.Add("钻机正在工作不能呼叫");
                    if (!doorFlag.NewValue.ToBool()) message.Add("门没开");
                    if (!InteractingDevice.allowAllAgv) message.Add("呼叫已经成功发起不能再次呼叫");
                    if (!InteractingDevice.MqttClientWrapper.IsConnected) message.Add("Mqtt断开连接 不能呼叫");
                    if (!DeviceDescriptor.AutoMode) message.Add("设备配置是手动 不能呼叫");
                    if (InteractingDevice.AgvOnWorkDrillStatus != 0) message.Add("agv正在给钻机上下料 不能呼叫");
                    if (isRawFall) message.Add("生料已经上满 不能呼叫");
                    if (!allUpFlag.NewValue.ToBool()) message.Add("顶升没有全部升起  不能发起呼叫");
                    if (!clampOpenFlag) message.Add("气夹没开  不能发起呼叫");

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
                         RequestDeviceKind = DeviceKind.CNC84Drill,
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
                                    { "SpindleBehavior", spindleBehaviorInf }
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

        private async Task FirstStep()
        {
            try
            {
                logger.LogDebug($"\r\n开始执行第一步\r\n");
                InteractingDevice.cnc84Command.SetCncComand("M44");
                logger.LogDebug($"\r\n发送蘑菇头指令\r\n");
                var mushroomFlag = InteractingDevice.frontExtendDevice.ControlMushroomInOut(false);
                if (!mushroomFlag)
                {
                    logger.LogDebug($"\r\n顶升 蘑菇头未动作\r\n");
                    InteractingDevice.cnc84Command.SetCncComand($"SDSP, 蘑菇头未动作，请界面开启");
                    return;
                }
                await Task.Delay(1000);
                //顶升
                logger.LogDebug($"\r\n发送顶升指令\r\n");
                var allUpFlag = InteractingDevice.frontExtendDevice.ControlUpDown(true);
                if (!allUpFlag)
                {
                    logger.LogDebug($"\r\n顶升 未全部升起来\r\n");
                    InteractingDevice.cnc84Command.SetCncComand($"SDSP,顶升 未全部升起来，请界面开启");
                    return;
                }
            }
            catch (Exception e)
            {
                logger.LogDebug($"\r\n执行第一步发生异常{e.Message}\r\n");
                logger.LogError($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r --{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} FirstStep 事件错误 {e.Message} \n\r--");
            }
            logger.LogDebug($"\r\n第一步执行结束\r\n");
        }

        public bool ValidateDoorOpen()
        {
            return InteractingDevice.frontExtendDevice.DoorIsOpen();
        }

        private T DeepCopy<T>(T obj)
        {
            var stringObj = JsonSerializer.Serialize(obj);
            return (T)JsonSerializer.Deserialize(stringObj, typeof(T));
        }

        public async Task<bool> LastStep()
        {
            try
            {
                logger.LogDebug($"\r\n开始执行最后一步 \r\n");
                InteractingDevice.cnc84Command.SetPcKey(@"\ESC");
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-发送ESC指令 ");
                await Task.Delay(1000);

                logger.LogDebug($"是否开启选轴功能 ： {selectSplineOnOff} ");
                if (selectSplineOnOff)
                {
                    if (!RetractTool())
                    {
                        logger.LogDebug($"退刀没成功 ");
                        InteractingDevice.cnc84Command.SetCncComand($"SDSP,选轴退刀功能 未正常执行，请手动开启");
                        return false;
                    }
                    StartSelectSpline();
                }
                logger.LogDebug($"是否开启压板功能 ： {pressBoardOnOff} ");
                if (pressBoardOnOff && !StartPressBoard())
                {
                    InteractingDevice.cnc84Command.SetCncComand($"SDSP,压板功能 未正常执行，请手动开启");
                    return false;
                }

                await Task.Delay(2000);
                logger.LogDebug($"是否开启蘑菇头功能 ： {mushroomOnOff} ");
                if (mushroomOnOff && !StartOpenMushroomController())
                {
                    logger.LogDebug($"蘑菇头功能 未正常开启 ");
                    InteractingDevice.cnc84Command.SetCncComand($"SDSP,蘑菇头功能 未正常执行，请手动开启");
                    return false;
                }
                await Task.Delay(2000);

                logger.LogDebug($"是否自动开启打板 ： {drillBoardOnOff} ");
                InteractingDevice.cnc84Command.SetCncComand($"DSP, ");
                if (drillBoardOnOff)
                {
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
                            InteractingDevice.cnc84Command.SetCncComand($"SDSP,正常打板不成功，请手动开启");
                            return false;
                        }
                    }
                    logger.LogDebug($"钻机已开始打板");
                }

                await Task.CompletedTask;

                logger.LogDebug($"\r\n完成最后一步 \r\n");
                return true;
            }
            catch (Exception ee)
            {
                logger.LogError($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r --{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} LastStep 事件错误 {ee.Message} \n\r--");
                logger.LogDebug($"\r\n最后一步发生异常{ee.Message} \r\n");
                return false;
            }
        }

        private (string, string) DefaultBatchCode(string code)
        {
            var drlPath = Path.Combine(DeviceDescriptor.Extra["DrlSearchPath"].ToStr(), code + "." + DeviceDescriptor.Extra["DrlSearchFileExt"].ToStr());
            var diaPath = Path.Combine(DeviceDescriptor.Extra["DiaSearchPath"].ToStr(), code + "." + DeviceDescriptor.Extra["DiaSearchFileExt"].ToStr());
            return (drlPath, diaPath);
        }

        private bool LoadFile(string drilPath, string? diaPath = "")
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
            return true;
        }

        private bool ValidateDrlFile(string drilFilePath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var netProName = string.Empty;
            do
            {
                Thread.Sleep(100);
                netProName = InteractingDevice.cnc84Command.GetCncStatus()?.ProgramName;
                if (drilFilePath.ToLower().Equals(netProName, StringComparison.CurrentCultureIgnoreCase))
                {
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateDrlFileTimeout"].ToLong());
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

        private bool ValidateDiaFileSame(string diaFilePath)
        {
            var diaName = InteractingDevice.cnc84Command.GetRuntimeString("%S(DiaFileNameWithDialog)");
            if ($"1:{diaFilePath}".Equals(diaName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        #region action

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
                //获取打板开始指令
                string endFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["DrilBoardEndFlag"].ToInt());
                logger.LogDebug($"启动过程中检测： 钻孔结束信号：{endFlag}");
                var drillHoleStart = "1:0".Equals(endFlag);
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

        public bool RetractTool()
        {
            InteractingDevice.cnc84Command.SetCncComand($"DSP,退刀中");
            Thread.Sleep(10);
            var toolNum = GetToolNum();
            if (!"T0".Equals(toolNum))
            {
                Thread.Sleep(3000);
                InteractingDevice.cnc84Command.SetCncComand("T");
            }

            if (!"T0".Equals(toolNum) && !ValidateRetractToolEnd())
            {
                return false;
            }
            return true;
        }

        public bool StartPressBoard()
        {
            InteractingDevice.cnc84Command.SetCncComand($"DSP,压板中");
            var toolNum = GetToolNum();
            if (!"T0".Equals(toolNum))
            {
                Thread.Sleep(3000);
                InteractingDevice.cnc84Command.SetCncComand("T");
            }

            if (!"T0".Equals(toolNum) && !ValidateRetractToolEnd())
            {
                return false;
            }

            Thread.Sleep(2000);
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
                Thread.Sleep(2000);
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
            if (InteractingDevice.frontExtendDevice.MushroomState(out bool inFlag, out bool outFlag) && !inFlag)
            {
                return InteractingDevice.frontExtendDevice.ControlMushroomInOut(true);
            }
            return false;
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
            var result = InteractingDevice.cnc84Command.GetOutput(55);

            if ("1:1".Equals(result, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
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
                var ready = InteractingDevice.frontExtendDevice.GetPanelStateString();
                InteractingDevice.cnc84Command.SetCncComand("SZSA");
                logger.LogDebug("发送SZSA");
                Thread.Sleep(2000);
                var rawStatus = ready.ToList();
                logger.LogDebug($"开始选择轴的时候 钻机上面板子 {ready}");
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

       
       public Task LoadFileFromCenter() => throw new NotImplementedException();

        #endregion action
    }
}
