using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Drill.Other.Atp;
using VgDeviceGateway.Devices.Drill.Other.Dto;

namespace VgDeviceGateway.Devices.Drill.EventHandler
{
    public class FrontToolDrillEventHandler : DeviceShare<DefaultDrill>, IDrillEventHandler
    {
        private readonly ILogger<FrontToolDrillEventHandler> logger;
        public DateTime DrillOprationTime = DateTime.Now;
        public static readonly string WatchFilePath = @"C:\SMWDATA\PROTOCOL\PROTO.PRO";
        public static readonly string OLDATPFILEDIRECTORY = @"C:\SMWDATA\ATPFILES";
        public readonly byte slaveID;
        private string toolDrillStatus = string.Empty;
        public readonly int spindleNum;
        public readonly int regionSize;
        public bool drillBoardOnOff;

        private static Dictionary<ushort, string> Errors = new Dictionary<ushort, string>()
        {
            {110,"中控下发异常" },
            {200,"打板未能正常启动" },
            {500,"上刀盘不全" },
            {501,"下刀盘不全" },
            {502,"上刀盘区域和传感器变化不一致" },
            {503,"上下刀盘结束传感器未发生变化" },
            {504,"未找到ATP文件夹" },
            {505,"未找到ATP文件" },
            {506,"Dia文件找不到" },
            {507,"解析ATP文件错误" },
            {508,"上下刀盘结束未知错误" },
            {509,"获取移动台面位置超时" },
            {510,"钻机移动台面超时" },
            {511,"换刀线程异常结束" },
            {512,"已移动台面2次  不能再次移动" },
        };

        public FrontToolDrillEventHandler(ILogger<FrontToolDrillEventHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            slaveID = (byte)device.DeviceDescriptor.Extra["SlaveID"].ToInt();
            spindleNum = device.DeviceDescriptor.SpindleNum;
            regionSize = InteractingDevice.DeviceDescriptor.Extra["Region"].ToUshort();
            drillBoardOnOff = device.DeviceDescriptor.Extra["ToolBufferDrillBoardOnOff"].ToBool();
        }

        public void AddWatchingEvents()
        {
            WatchingProperties.Property("Drill_DrillHoleEnd")
            .PostCondition(p => p.IsValueChanged)
            .TriggerAlways(async () =>
            {
                var holeEnd = WatchingProperties.Property("Drill_DrillHoleEnd").NewValue.ToBool();
                logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  钻孔结束信号 {holeEnd} ");
                try
                {
                    if (holeEnd)
                    {
                        try
                        {
                            if (!Directory.Exists(OLDATPFILEDIRECTORY))
                            {
                                return;
                            }

                            var oldAtpFile = GetLastCreateAtpByCnc84();
                            if (string.IsNullOrWhiteSpace(oldAtpFile))
                            {
                                return;
                            }

                            AtpDTO atpDTO = ParseAtp.ParseFromFile(Path.Combine(OLDATPFILEDIRECTORY, oldAtpFile));
                            int[] trayIds = ParseAtp.ParseToolLifetimeByTray(atpDTO.magazineDTOs);
                            if (trayIds.Length == 3)
                            {
                                trayIds = new int[] { 1, 2 };
                            }
                            else if (trayIds.Length == 2)
                            {
                                if (trayIds.Contains(2) && trayIds.Contains(1))
                                {
                                    trayIds = new int[] { 1 };
                                }
                                else if (trayIds.Contains(2) && trayIds.Contains(3))
                                {
                                    trayIds = new int[] { 2 };
                                }
                            }

                            ushort data = TrayIdsToUnshort(trayIds);

                            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveID, DeviceDescriptor.Extra["ToolBufferUnloadTrayPositionWirtePlc"].ToUshort(), Enumerable.Repeat(data, InteractingDevice.spindleNum).ToArray());

                            logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  钻孔信号  结束 解析旧刀盘 {oldAtpFile} 寿命到的刀盘号{string.Join(",", trayIds)}   写到PLC的点位是  二进制是{string.Join("", Convert.ToString(data, 2).PadLeft(16, '0').Reverse())} ");
                            // 上报中控信息
                            //Task.Factory.StartNew(() =>
                            //{
                            //    ReportAtp(oldAtpFile, atpDTO);
                            //});
                        }
                        finally
                        {
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ToolBufferDrillEnd"].ToUshort(), 1);
                        }
                    }
                    else
                    {
                        logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  钻孔信号 开始  ");
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ToolBufferDrillEnd"].ToUshort(), 0);
                        InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveID, DeviceDescriptor.Extra["ToolBufferUnloadTrayPositionWirtePlc"].ToUshort(), Enumerable.Repeat((ushort)0, InteractingDevice.spindleNum).ToArray());
                    }
                }
                catch (Exception ee)
                {
                    await Task.CompletedTask;
                }
                await Task.CompletedTask;
            });

            WatchingProperties.Property("Tool_BufferDrillLoadAndUnLoadTrayEnd")
           .PostCondition(p => p.IsValueChanged && p.NewValue.ToBool())
           .TriggerAlways(async () =>
           {
               try
               {
                   var curDrillToolDrillStatus = GetDeviceToolStatus("ToolBufferDrillStatusOnPlc", out string front3, out string back3);
                   logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  插齿和钻机之间换刀结束  钻机刀盘状态 {toolDrillStatus} ");

                   if (curDrillToolDrillStatus.Equals(toolDrillStatus))
                   {
                       logger.LogDebug($"换刀前 钻机刀状态 {toolDrillStatus}  换刀后钻机刀状态 {curDrillToolDrillStatus}  相同不修改atp  ");
                       WriteWarningToPlc(503);
                       return;
                   }

                   var status = CompareStatusGetRegion(toolDrillStatus, curDrillToolDrillStatus, out List<int> loadRegions, out List<int> unloadRegions);
                   if (status != 0)
                   {
                       logger.LogDebug($"换刀前 钻机刀状态 {toolDrillStatus}  换刀后钻机刀状态 {curDrillToolDrillStatus}  有异常   未全部上成功{status == 1}  未全部下成功{status == -1} ");
                       WriteWarningToPlc((ushort)(status == 1 ? 500 : 501));
                       return;
                   }
                   logger.LogDebug($" 上刀盘的区域{string.Join("", loadRegions)}  下刀盘的区域{string.Join("", unloadRegions)}  ");

                   //if (loadRegions.Count >0)
                   //{
                   //    //比对上生料
                   //    ushort data = TrayIdsToUnshort(loadRegions.ToArray());
                   //    var getLoadRegion = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ToolBufferLoadTrayPosition"].ToUshort(), 1)[0];
                   //    if (data != getLoadRegion)
                   //    {
                   //        logger.LogDebug($"传感器获取的上刀盘的位置区域是 {string.Join("", loadRegions)} 获取的点位   {data} 和赋值的上刀盘位置 {getLoadRegion} 不一致");
                   //        WriteWarningToPlc(502);
                   //        return;
                   //    }
                   //}

                   //更新刀盘信息
                   if (unloadRegions.Count > 0)
                   {
                       for (int j = 0; j < unloadRegions.Count; j++)
                       {
                           ModifyPayloadCutterTraysByLoadAndUnload(unloadRegions[j], 1);
                       }
                   }

                   if (loadRegions.Count > 0)
                   {
                       for (int j = 0; j < loadRegions.Count; j++)
                       {
                           ModifyPayloadCutterTraysByLoadAndUnload(loadRegions[j], 0);
                       }
                   }

                   if (!Directory.Exists(OLDATPFILEDIRECTORY))
                   {
                       WriteWarningToPlc(504);
                       return;
                   }

                   var oldAtpFile = GetLastCreateAtpByCnc84();
                   if (string.IsNullOrWhiteSpace(oldAtpFile))
                   {
                       WriteWarningToPlc(505);
                       return;
                   }
                   string oldAtpFilePath = Path.Combine(OLDATPFILEDIRECTORY, oldAtpFile);
                   string oldDiaFilePath = InteractingDevice.DeviceDescriptor.Extra["DiaFullPath"].ToString();
                   if (!File.Exists(oldDiaFilePath))
                   {
                       WriteWarningToPlc(506);
                       return;
                   }

                   //根据上刀盘的位置 找到对应的的排刀信息
                   string newMagazineInfo = string.Empty;
#if DEBUG
                   // 本地文件
                   newMagazineInfo = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"data/Arrange{loadRegions.ToArray()[0]}.Json"));
#else
                                        //获取中控信息
                    newMagazineInfo = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"data/Arrange{loadRegions.ToArray()[0]}.Json"));
#endif
                   var messageEntity = ParseAtp.ParseToFile(newMagazineInfo, loadRegions.ToArray(), oldAtpFilePath, oldDiaFilePath, unloadRegions.ToArray());

                   //加载atp文件
                   if (!messageEntity.IsSuccessful)
                   {
                       WriteWarningToPlc(507);
                       return;
                   }
                   CNCCommand.SetLoadFile(messageEntity.AtpFile);
                   Thread.Sleep(5000);
                   logger.LogDebug($"是否自动开启打板 ： {drillBoardOnOff} ");

                   if (drillBoardOnOff)
                   {
                       StartDrilBoard();
                       if (!ValidateStartCommandEnd())
                       {
                           bool flag = CheckStartStatusAndRestart();
                           if (!flag)
                           {
                               logger.LogDebug($"打板未能正常启动 ");
                               WriteWarningToPlc(200);
                               return;
                           }
                       }
                       logger.LogDebug($"钻机已开始打板");
                   }
               }
               catch (Exception ee)
               {
                   WriteWarningToPlc(508);
                   logger.LogDebug($"上下刀盘结束处理过程中 ： {ee.Message} ");
                   await Task.CompletedTask;
               }
               await Task.CompletedTask;
           });

            WatchingProperties.Property("Tool_BufferDrillLoadAndUnLoadTrayStart")
           .PostCondition(p => p.IsValueChanged && p.NewValue.ToBool())
           .TriggerAlways(async () =>
           {
               try
               {
                   var allToolDrillStatus = GetDeviceToolStatus("ToolBufferDrillStatusOnPlc", out string front3, out string back3);
                   toolDrillStatus = allToolDrillStatus;
                   logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  插齿和钻机之间换刀开始  钻机刀盘状态 {toolDrillStatus} ");
                   Task.Factory.StartNew(() =>
                   {
                       try
                       {
                           int maxCount = 0;
                           logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  插齿和钻机之间换刀线程 开始 ");
                           bool Flag = true;
                           do
                           {
                               if (maxCount >= 2)
                               {
                                   WriteWarningToPlc(512);
                                   return;
                               }
                               InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["DrillArrivalPositionWriteToPlc"].ToUshort(), 0);
                               int position = ReadNeedMoveToPosition();
                               if (position == -1)
                               {
                                   logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  插齿和钻机之间换刀线程  获取台面移动点位超时 ");
                                   WriteWarningToPlc(509);
                                   return;
                               }
                               maxCount++;
                               InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ReadDrillNeedMoveToPosition"].ToUshort(), 0);
                               logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  插齿和钻机之间换刀线程  台面将要移动到 P{position} 位置");
                               //移动
                               InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["DrillArrivalPositionWriteToPlc"].ToUshort(), 0);
                               InteractingDevice.cnc84Command.SetCncComand($"P{position}");
                               if (!ValidateDrillMoveToPPosition(position))
                               {
                                   WriteWarningToPlc(510);
                                   logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  插齿和钻机之间换刀线程  移动台面到P{position}位超时 ");
                                   return;
                               }
                               InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ReadDrillNeedMoveToPosition"].ToUshort(), 0);
                               InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["DrillArrivalPositionWriteToPlc"].ToUshort(), (ushort)position);
                               Thread.Sleep(DeviceDescriptor.Extra["ToolBufferDoWorkingTimeOut"].ToInt());
                               var toolBufferUnloadAndLoadEnd = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ToolBufferDrillUnLoadingAndLoadingOnPlc"].ToUshort(), 1);
                               Flag = toolBufferUnloadAndLoadEnd[0] == 1;
                           } while (Flag);

                           logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  插齿和钻机之间换刀线程 正常结束 ");
                       }
                       catch (Exception ee)
                       {
                           logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  插齿和钻机之间换刀线程 异常结束 {ee.Message} ");
                           WriteWarningToPlc(511);
                       }
                       finally
                       {
                           InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ReadDrillNeedMoveToPosition"].ToUshort(), 0);
                           InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["DrillArrivalPositionWriteToPlc"].ToUshort(), 0);
                       }
                   });
               }
               catch (Exception)
               {
                   await Task.CompletedTask;
               }
               await Task.CompletedTask;
           });

            WatchingProperties.Properties("Tool_BufferRealNewOldStatus", "Tool_BufferAgvLoadAndUnLoadTray", "Tool_DrillStatus", "Tool_BufferWarningInfoWriteOnPlc", "Tool_BufferDrillLoadAndUnLoadTrayEnd", "Tool_OtherWarning")
           .When(p =>
           {
               var realNewOldStatus = p.Property("Tool_BufferRealNewOldStatus");
               var drillStatus = p.Property("Tool_DrillStatus");
               var agvLoadAndUnLoadTray = p.Property("Tool_BufferAgvLoadAndUnLoadTray");
               var warningInfoWriteOnPlc = p.Property("Tool_BufferWarningInfoWriteOnPlc");
               var bufferDrillLoadAndUnLoadTrayEnd = p.Property("Tool_BufferDrillLoadAndUnLoadTrayEnd");
               var otherWarning = p.Property("Tool_OtherWarning");
               //钻机刀盘状态

               List<int> newTraySpline = HasNewTraySpline(realNewOldStatus.NewValue.ToStr(), out int newCount);
               List<int> oldTraySpline = HasOldTraySpline(realNewOldStatus.NewValue.ToStr(), out int oldCount);
               var rawIsFull = RawToolIsFull(newTraySpline, realNewOldStatus.NewValue.ToStr());

               bool result = !agvLoadAndUnLoadTray.NewValue.ToBool() && (!(rawIsFull == 1) || (oldTraySpline.Count > 0)) && InteractingDevice.allowAllAgv && !warningInfoWriteOnPlc.NewValue.ToBool() && bufferDrillLoadAndUnLoadTrayEnd.NewValue.ToBool() && otherWarning.NewValue.ToInt() == 0
               && InteractingDevice.MqttClientWrapper.IsConnected && DeviceDescriptor.AutoMode;
               if (result)
               {
                   InteractingDevice.CallCondition = $"{DateTime.Now.ToString()}:  满足条件能发起呼叫  板子状态 {ShowSplineStatus(realNewOldStatus.NewValue.ToStr())}";
               }
               else
               {
                   List<string> message = new List<string>();
                   if (agvLoadAndUnLoadTray.NewValue.ToBool()) message.Add("Agv在给插齿上下料,不能呼叫");
                   if (warningInfoWriteOnPlc.NewValue.ToBool()) message.Add("插齿有干涉报警 ，不能发起呼叫");
                   if ((rawIsFull == 1) && (oldTraySpline.Count <= 0)) message.Add("生料已满 也无熟料 不能呼叫");
                   if (!InteractingDevice.MqttClientWrapper.IsConnected) message.Add("Mqtt断开连接 不能呼叫");
                   if (!InteractingDevice.allowAllAgv) message.Add("呼叫已经成功发起不能再次呼叫");
                   if (!DeviceDescriptor.AutoMode) message.Add("设备配置是手动 不能呼叫");
                   if (!bufferDrillLoadAndUnLoadTrayEnd.NewValue.ToBool()) message.Add("插齿和钻机交互未完成 不能呼叫");
                   if (otherWarning.NewValue.ToInt() > 0) message.Add("插齿和钻机 有报警 不能呼叫");
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
                       if (message != null && "Tool buffer 小于呼叫间隔不能呼叫".Equals(message))
                       {
                           InteractingDevice.callTimeResult = $"{DateTime.Now.ToString()}:{message}";
                       }
                       else
                       {
                           InteractingDevice.callResult = $"{DateTime.Now.ToString()}:{message}";
                           InteractingDevice.callTimeResult = "";
                       }

                       logger.LogDebug($"Tool buffer自动呼叫AGV ： {message} ");
                   }
               }
               catch (Exception ee)
               {
                   logger.LogError($"Tool buffer呼叫AGV异常 ： {ee.Message} ");
                   InteractingDevice.callResult = $"{DateTime.Now.ToString()}:{ee.Message}";
                   await Task.CompletedTask;
               }
           });
        }

        private int ReadNeedMoveToPosition()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            do
            {
                Thread.Sleep(100);
                var needMoveToPosition = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ReadDrillNeedMoveToPosition"].ToUshort(), 1);
                if (needMoveToPosition[0] != 0)
                {
                    return needMoveToPosition[0];
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ReadDrillNeedMoveToPositionTimeOut"].ToLong());
            return -1;
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

        private void RestartCnc()
        {
            InteractingDevice.cnc84Command.Start();
            logger.LogDebug($" RestartCnc 发送开始指令 ");
            Thread.Sleep(2000);
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
                } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ToolBufferValidateStartTimeout"].ToLong());
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

        private void StartChangeF8()
        {
            InteractingDevice.cnc84Command.SetChangePage("WORK_WORK");
        }

        private bool ValidateDrillMoveToPPosition(int position)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                Thread.Sleep(100);
                //var yPosition = InteractingDevice.cnc84Command.GetRuntimeString("%S(FIXXY_4)");
                //var yPositionTemp = yPosition.Substring(2, yPosition.Length - 2);
                var yPosition = InteractingDevice.cnc84Command.GetRuntimeValue("AxRealPos(1)");

                var yPositionTemp = yPosition;
                if (position == 1 ? yPositionTemp == DeviceDescriptor.Extra["P1Postion"].ToString() : yPositionTemp == DeviceDescriptor.Extra["P2Postion"].ToString())
                {
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["DrillArrivalPositionWriteToPlcTimeOut"].ToLong());
            return false;
        }

        private List<int> HasOldTraySpline(string realNewOldStatus, out int oldCount)
        {
            oldCount = 0;
            return HasTraySpline(realNewOldStatus, out oldCount, s => s == '2');
        }

        private List<int> HasNewTraySpline(string realNewOldStatus, out int newCount)
        {
            newCount = 0;
            return HasTraySpline(realNewOldStatus, out newCount, s => s == '1');
        }

        private string ShowSplineStatus(string status)
        {
            string newShowStatus = string.Empty;
            for (int i = 0; i < status.Length; i += regionSize)
            {
                var subString = status.Substring(i, regionSize);
                newShowStatus += subString;
                newShowStatus += "-";
            }
            return newShowStatus.Trim('-');
        }

        private List<int> HasTraySpline(string realNewOldStatus, out int count, Func<char, bool> condition)
        {
            count = 0;
            List<int> splines = new List<int>();

            for (int i = 0; i < realNewOldStatus.Length; i += regionSize)
            {
                var subString = realNewOldStatus.Substring(i, regionSize);
                var tmpCount = subString.Count(s => condition.Invoke(s));
                if (tmpCount == 0)
                {
                    continue;
                }
                splines.Add(i / regionSize + 1);
                count += tmpCount;
            }
            return splines;
        }

        public void IniAgvPosition()
        {
            InteractingDevice.spindleAgvPosition = Enumerable.Repeat("null", InteractingDevice.spindleNum).ToArray();
            var tmpSpindeles = DeviceDescriptor.Extra["Spindles"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries);
            Array.Copy(tmpSpindeles, InteractingDevice.spindleAgvPosition, InteractingDevice.spindleAgvPosition.Length);
        }

        public T DeepCopy<T>(T obj)
        {
            var stringObj = JsonSerializer.Serialize(obj);
            return (T)JsonSerializer.Deserialize(stringObj, typeof(T));
        }

        private void WriteWarningToPlc(ushort code)
        {
            try
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, DeviceDescriptor.Extra["ToolBufferOtherWarningInfoWriteOnPlc"].ToUshort(), code);
                logger.LogDebug($"发生错误 错误编码  {code}  错误信息是{Errors[code]}");
            }
            catch (Exception ee)
            {
                logger.LogError($"写错误异常:{ee.Message}");
            }
        }

        public async Task<string> CallAgv()
        {
            try
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(InteractingDevice.CallAgvTime);
                if (timeSpan.TotalSeconds < InteractingDevice.CallAgvTimeInterval)
                {
                    logger.LogDebug("呼叫换刀 小于呼叫间隔不能呼叫");
                    return "呼叫换刀 小于呼叫间隔不能呼叫";
                }

                InteractingDevice.CallAgvTime = DateTime.Now;
                if (!InteractingDevice.allowAllAgv)
                {
                    return "呼叫换刀 agv信号为 false 不能呼叫";
                }
                if (!InteractingDevice.MqttClientWrapper.IsConnected) return "呼叫换刀AGV  Mqtt 连接断开 不上报消息";

                var payloadCutterTrays = DeepCopy(InteractingDevice.PayloadCutterTrays);
                var realNewOldStatus = WatchingProperties.Property("Tool_BufferRealNewOldStatus");
                List<int> newTraySpline = HasNewTraySpline(realNewOldStatus.NewValue.ToStr(), out int newCount);
                List<int> oldTraySpline = HasOldTraySpline(realNewOldStatus.NewValue.ToStr(), out int oldCount);

                var rawIsFull = RawToolIsFull(newTraySpline, realNewOldStatus.NewValue.ToStr());
                bool hasClinker = oldTraySpline.Count > 0;

                var drillStatus = WatchingProperties.Property("Tool_DrillStatus");
                var bufferStatus = WatchingProperties.Property("Tool_BufferStatus");
                List<int> drillExistTrayRegions = DrillExistTrayRegions(drillStatus.NewValue.ToStr());
                List<int> toolBufferExistRawTrayRegions = ToolBufferExistRawTrayRegions(realNewOldStatus.NewValue.ToStr());
                List<int> toolBufferExistClinkerTrayRegions = ToolBufferExistClinkerTrayRegions(realNewOldStatus.NewValue.ToStr());

                var realRegion = ConfirmRegion(drillExistTrayRegions, toolBufferExistRawTrayRegions, toolBufferExistClinkerTrayRegions);
                if (realRegion == -1)
                {
                    return $"区域解析有问题";
                }

                //var spindleBehavior = GetAgvBehavior(realNewOldStatus.NewValue.ToStr(), drillStatus.NewValue.ToStr(), bufferStatus.NewValue.ToStr());
                var spindleBehavior = GetAgvBehaviorByRegion(realRegion, realNewOldStatus.NewValue.ToStr());

                int rawSpindleNum = GetRawSpindleNum(spindleBehavior);

                IniAgvPosition();
                var agvPosition = InteractingDevice.spindleAgvPosition;
                ModifyAgvPosition(agvPosition, spindleBehavior);

                var spindleBehaviorInf = string.Join(",", spindleBehavior);
                var agvPositionInf = string.Join(",", agvPosition);

                ModifyPayloadCutterTrays(payloadCutterTrays, spindleBehavior);

                var result = await DataExporter.DeviceEventReport(
                         new DeviceEventReportRequest()
                         {
                             ProductId = DeviceDescriptor.ProductId,
                             DeviceId = DeviceDescriptor.DeviceId,
                             ClientId = InteractingDevice.ClientId,
                             EventId = Events.REQUEST_AGV_CHANGE_CUTTER,
                             RequestInteractionBehavior = InteractionBehavior.Make(this.DeviceDescriptor.DeviceKind, InteractionBehavior.FRONT_UNLOAD_CUTTER_THEN_LOAD_CUTTER),
                             RequestDeviceKind = DeviceKind.CNC84Drill,
                             RequestMaterialKind = MaterialKind.Cutter,
                             RequestInteractionDirection = InteractionPosition.Front,
                             Params = new Dictionary<string, object?>()
                             {
                                    { "ExistRawNum", newCount },
                                    { "Regions", regionSize },
                                    { "SpindleUseNum", InteractingDevice.spindleNum },
                                    { "RawSpindleNum", rawSpindleNum },
                                    { "SpindleNum", agvPosition.Count(s=>s!="null") },
                                    { "ClinkerSpindleNum", oldTraySpline.Count },
                                    { "InteractivePosition",  DeviceDescriptor.Extra["InteractivePosition"].ToStr () },
                                    { "Spindles", agvPositionInf},
                                    { "SpindleBehavior", spindleBehaviorInf }
                             },
                             PayloadCutterTrays = payloadCutterTrays
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

                return "呼叫换刀 成功";
            }
            catch (Exception ee)
            {
                logger.LogError($"呼叫 换刀 AGV异常 ： {ee.Message} ");
                return $"呼叫换刀 AGV异常 {ee.Message}";
            }
        }

        private int ConfirmRegion(List<int> drillExistTrayRegions, List<int> toolBufferExistRawTrayRegions, List<int> toolBufferExistClinkerTrayRegions)
        {
            var condidateReion = Enumerable.Range(0, regionSize).Except(drillExistTrayRegions).ToList();
            if (condidateReion.Contains(1))
            {
                condidateReion.Remove(1);
            }
            if (toolBufferExistClinkerTrayRegions.Count >= 1)
            {
                for (int i = 0; i < toolBufferExistClinkerTrayRegions.Count; i++)
                {
                    if (condidateReion.Contains(toolBufferExistClinkerTrayRegions[i]))
                    {
                        return toolBufferExistClinkerTrayRegions[i];
                    }
                }
            }
            else if (toolBufferExistRawTrayRegions.Count >= 1)
            {
                for (int i = 0; i < toolBufferExistRawTrayRegions.Count; i++)
                {
                    if (condidateReion.Contains(toolBufferExistRawTrayRegions[i]))
                    {
                        return toolBufferExistRawTrayRegions[i];
                    }
                }
            }
            if (condidateReion.Count > 0)
            {
                return condidateReion[0];
            }

            return -1;
        }

        private List<int> ToolBufferExistClinkerTrayRegions(string realNewOldStatus)
        {
            return ExistTrayRegions(realNewOldStatus, s => s == '2');
        }

        private List<int> ToolBufferExistRawTrayRegions(string realNewOldStatus)
        {
            return ExistTrayRegions(realNewOldStatus, s => s == '1');
        }

        private List<int> DrillExistTrayRegions(string drillStatus)
        {
            return ExistTrayRegions(drillStatus, s => s == '1');
        }

        private List<int> ExistTrayRegions(string status, Func<char, bool> condition)
        {
            List<string> tmp = new List<string>();
            for (int i = 0; i < status.Length; i = i + regionSize)
            {
                tmp.Add(new string(status.Skip(i).Take(regionSize).ToArray()));
            }
            List<int> tmpRegion = new List<int>();
            for (int i = 0; i < regionSize; i++)
            {
                for (int j = 0; j < tmp.Count; j++)
                {
                    if (condition.Invoke(tmp[j][i]) && !tmpRegion.Contains(i))
                    {
                        tmpRegion.Add(i);
                    }
                }
            }

            return tmpRegion;
        }

        private void ModifyPayloadCutterTrays(CutterTrays payloadCutterTrays, int[] spindleBehavior)
        {
            for (int i = 0; i < spindleBehavior.Length; i++)
            {
                if (spindleBehavior[i] == 2 && payloadCutterTrays[InteractingDevice.region * InteractingDevice.spindleNum + i].Status != CutterTrayStatus.Old)
                {
                    payloadCutterTrays[InteractingDevice.region * InteractingDevice.spindleNum + i].Status = CutterTrayStatus.Old;
                }
            }
        }

        //0 上料 1 下料
        private void ModifyPayloadCutterTraysByLoadAndUnload(int region, int modifyType = 0)
        {
            for (int i = 0; i < spindleNum; i++)
            {
                int drillIndex = InteractingDevice.region * i + region - 1;
                int bufferIndex = InteractingDevice.region * InteractingDevice.spindleNum + InteractingDevice.region * i + region - 1;

                int targetIndex = modifyType == 0 ? drillIndex : bufferIndex;
                int sourceIndex = modifyType == 0 ? bufferIndex : drillIndex;

                PayloadCutterTrays[targetIndex].ItemCode = PayloadCutterTrays[sourceIndex].ItemCode;
                PayloadCutterTrays[sourceIndex].ItemCode = string.Empty;

                PayloadCutterTrays[targetIndex].TrayCode = PayloadCutterTrays[sourceIndex].TrayCode;
                PayloadCutterTrays[sourceIndex].TrayCode = string.Empty;

                PayloadCutterTrays[targetIndex].SiloCode = PayloadCutterTrays[sourceIndex].SiloCode;
                PayloadCutterTrays[sourceIndex].SiloCode = string.Empty;

                PayloadCutterTrays[targetIndex].Status = modifyType == 0 ? CutterTrayStatus.New : CutterTrayStatus.Old;
                PayloadCutterTrays[sourceIndex].Status = CutterTrayStatus.NoTray;
            }

            //if (modifyType==1) //1 下料
            //{
            //    for (int i = 0; i < spindleNum; i++)
            //    {
            //        int drillIndex = InteractingDevice.region * i + region - 1;
            //        int bufferIndex = InteractingDevice.region * InteractingDevice.spindleNum + InteractingDevice.region * i + region - 1;

            //        PayloadCutterTrays[bufferIndex].ItemCode = PayloadCutterTrays[drillIndex].ItemCode;
            //        PayloadCutterTrays[drillIndex].ItemCode = string.Empty;

            //        PayloadCutterTrays[bufferIndex].TrayCode = PayloadCutterTrays[drillIndex].TrayCode;
            //        PayloadCutterTrays[drillIndex].TrayCode = string.Empty;

            //        PayloadCutterTrays[bufferIndex].Status = CutterTrayStatus.Old;
            //        PayloadCutterTrays[drillIndex].Status = CutterTrayStatus.NoTray;
            //    }
            //}
            //else if (modifyType == 0) //上料
            //{
            //    for (int i = 0; i < spindleNum; i++)
            //    {
            //        int drillIndex = InteractingDevice.region * i + region - 1;
            //        int bufferIndex = InteractingDevice.region * InteractingDevice.spindleNum + InteractingDevice.region * i + region - 1;

            //        PayloadCutterTrays[drillIndex].ItemCode = PayloadCutterTrays[bufferIndex].ItemCode;
            //        PayloadCutterTrays[bufferIndex].ItemCode = string.Empty;

            //        PayloadCutterTrays[drillIndex].TrayCode = PayloadCutterTrays[bufferIndex].TrayCode;
            //        PayloadCutterTrays[bufferIndex].TrayCode = string.Empty;

            //        PayloadCutterTrays[drillIndex].Status = CutterTrayStatus.New;
            //        PayloadCutterTrays[bufferIndex].Status = CutterTrayStatus.NoTray;
            //    }
            //}
        }

        private int GetRawSpindleNum(int[] spindleBehavior)
        {
            int num = 0;
            for (int i = 0; i < spindleBehavior.Length; i += regionSize)
            {
                var subSpindleBehavior = spindleBehavior.Skip(i).Take(regionSize);
                if (subSpindleBehavior.Contains(0))
                {
                    num++;
                }
            }
            return num;
        }

        private void ModifyAgvPosition(string[] agvPosition, int[] spindleBehavior)
        {
            for (int i = 0; i < spindleBehavior.Length; i += regionSize)
            {
                var subSpindleBehavior = spindleBehavior.Skip(i).Take(regionSize);
                if (subSpindleBehavior.All(s => s == -1))
                {
                    agvPosition[i / regionSize] = "null";
                }
            }
        }

        private int[] ModifySplineBehavior(int[] spindleBehavior, string allToolDrillStatus)
        {
            return spindleBehavior;
        }

        //private int[] GetAgvBehavior(string splineStatus,string drillStatus)
        //{
        //    int[] tmp = Enumerable.Repeat(-1, splineStatus.Length).ToArray();
        //    for (int i = 0; i < splineStatus.Length; i++)
        //    {
        //        if (drillStatus[i]=='1')
        //        {
        //            tmp[i] = splineStatus[i] == '1' ? 1 : splineStatus[i] == '2' ? 2 : -1;
        //        }
        //        else
        //        {
        //            tmp[i] = splineStatus[i] == '0' ? 0 : splineStatus[i] == '2' ? 2 : -1;
        //        }

        //    }
        //    return tmp;
        //}
        private int[] GetAgvBehaviorByRegion(int region, string splineStatus)
        {
            int[] tmp = Enumerable.Repeat(-1, splineStatus.Length).ToArray();
            for (int i = 0; i < splineStatus.Length; i += regionSize)
            {
                int[] tmpSub = Enumerable.Repeat(-1, regionSize).ToArray();
                var subStatus = new string(splineStatus.Skip(i).Take(regionSize).ToArray());
                //012
                var existTool = subStatus.Select((s, i) => new { s, i }).Where(s => s.s != '0').Select(s => s.i);

                if (subStatus[region] == '0')
                {
                    tmpSub[region] = 0;
                }
                else if (subStatus[region] == '2')
                {
                    tmpSub[region] = 2;
                }
                for (int j = 0; j < subStatus.Length; j++)
                {
                    tmp[i + j] = tmpSub[j];
                }
            }

            return tmp;
        }

        private int[] GetAgvBehavior(string splineStatus, string drillStatus, string bufferStatus)
        {
            int[] tmp = Enumerable.Repeat(-1, splineStatus.Length).ToArray();
            for (int i = 0; i < splineStatus.Length; i += regionSize)
            {
                int[] tmpSub = Enumerable.Repeat(-1, regionSize).ToArray();
                var subStatus = new string(splineStatus.Skip(i).Take(regionSize).ToArray());
                //012
                var existTool = subStatus.Select((s, i) => new { s, i }).Where(s => s.s != '0').Select(s => s.i);

                for (int j = 0; j < subStatus.Length; j++)
                {
                    if (j == 1)
                    {
                        tmpSub[j] = -1;
                        continue;
                    }
                    if (subStatus[j] == '0')
                    {
                        if (!(tmpSub.Contains(0) || tmpSub.Contains(2)) && !(existTool.Contains(j + 1) || existTool.Contains(j - 1)))
                        {
                            tmpSub[j] = 0;
                        }
                        else
                        {
                            tmpSub[j] = -1;
                        }
                    }
                    else if (subStatus[j] == '1')
                    {
                        tmpSub[j] = -1;
                    }
                    else if (subStatus[j] == '2')
                    {
                        if (tmpSub.Contains(2))
                        {
                            tmpSub[j] = -1;
                        }
                        else
                        {
                            tmpSub[j] = 2;
                        }
                    }
                }

                for (int j = 0; j < subStatus.Length; j++)
                {
                    tmp[i + j] = tmpSub[j];
                }
            }
            return tmp;
        }

        // -1 异常 0 不满 1 满
        private int RawToolIsFull(List<int> newTraySpline, string realNewOldStatus)
        {
            //所有1号区域 都满
            List<string> tmp = new List<string>();
            for (int i = 0; i < realNewOldStatus.Length; i = i + regionSize)
            {
                tmp.Add(new string(realNewOldStatus.Skip(i).Take(regionSize).ToArray()));
            }
            List<int> tmpRegion = new List<int>();
            for (int i = 0; i < regionSize; i++)
            {
                for (int j = 0; j < tmp.Count; j++)
                {
                    if (tmp[j][i] == '1' && !tmpRegion.Contains(i))
                    {
                        tmpRegion.Add(i);
                    }
                }
            }
            if (tmpRegion.Count == 0)
            {
                return 0;
            }
            else if (tmpRegion.Count > 1 || tmpRegion.Contains(1))
            {
                return -1;
            }
            for (int j = 0; j < tmp.Count; j++)
            {
                if (tmp[j][tmpRegion[0]] != '1')
                {
                    return 0;
                }
            }
            return 1;
        }

        private List<int> CompareStatusGetUnloadRegions(string toolDrillStatus, string allToolDrillStatus)
        {
            return CompareStatusGetRegion(toolDrillStatus, allToolDrillStatus, (oldStatus, newStatus) =>
            {
                return oldStatus.Select((s, i) =>
                {
                    if (s == '1' && newStatus[i] == '0')
                    {
                        return '1';
                    }
                    return '0';
                }).ToString()!;
            }
           );
        }

        private List<int> CompareStatusGetLoadRegions(string toolDrillStatus, string allToolDrillStatus)
        {
            return CompareStatusGetRegion(toolDrillStatus, allToolDrillStatus, (oldStatus, newStatus) =>
            {
                return oldStatus.Select((s, i) =>
                {
                    if (s == '0' && newStatus[i] == '1')
                    {
                        return '1';
                    }
                    return '0';
                }).ToString()!;
            }
           );
        }

        private int CompareStatusGetRegion(string toolDrillStatus, string curToolDrillStatus, out List<int> loadRegion, out List<int> unloadRegion)
        {
            List<string> oldStatus = new List<string>();
            List<string> curStatus = new List<string>();
            List<int> loadRegionTemp = new List<int>();
            List<int> unloadRegionTemp = new List<int>();

            for (int i = 0; i < curToolDrillStatus.Length; i = i + regionSize)
            {
                var subCurString = curToolDrillStatus.Substring(i, regionSize);
                var subOldString = toolDrillStatus.Substring(i, regionSize);
                oldStatus.Add(subOldString);
                curStatus.Add(subCurString);

                for (int j = 0; j < regionSize; j++)
                {
                    if (subCurString[j] == '1' && subOldString[j] == '0' && !loadRegionTemp.Contains(j))
                    {
                        loadRegionTemp.Add(j);
                    }
                    if (subCurString[j] == '0' && subOldString[j] == '1' && !unloadRegionTemp.Contains(j))
                    {
                        unloadRegionTemp.Add(j);
                    }
                }
            }
            loadRegion = loadRegionTemp.Select(s => s + 1).ToList();
            unloadRegion = unloadRegionTemp.Select(s => s + 1).ToList();

            if (loadRegionTemp.Count > 0)
            {
                for (int i = 0; i < loadRegionTemp.Count; i++)
                {
                    if (!curStatus.All(s => s[loadRegionTemp[i]] == '1'))
                    {
                        return 1;
                    }
                }
            }

            if (unloadRegionTemp.Count > 0)
            {
                for (int i = 0; i < unloadRegionTemp.Count; i++)
                {
                    if (!curStatus.All(s => s[unloadRegionTemp[i]] == '0'))
                    {
                        return -1;
                    }
                }
            }
            return 0;
        }

        private List<int> CompareStatusGetRegion(string toolDrillStatus, string allToolDrillStatus, Func<string, string, string> ChangedStatus)
        {
            var changeString = ChangedStatus.Invoke(toolDrillStatus, allToolDrillStatus);
            if (changeString.All(s => s == '0'))
            {
                return new List<int>() { 0 };
            }
            if (changeString.Count(s => s == '1') % spindleNum != 0)
            {
                return new List<int>() { -1 };
            }
            var subString = changeString.Substring(0, regionSize);
            List<int> indexs = GetChangeIndexs(subString);
            bool Flag = true;
            for (int i = 0; i < subString.Length; i = i + regionSize)
            {
                var subOtherString = changeString.Substring(i, regionSize);
                List<int> otherindexs = GetChangeIndexs(subOtherString);
                if (!indexs.SequenceEqual(otherindexs))
                {
                    Flag = false;
                    break;
                }
            }
            if (!Flag)
            {
                return new List<int>() { -2 };
            }
            return indexs.Select(s => s + 1).ToList();
        }

        private static List<int> GetChangeIndexs(string subString)
        {
            List<int> indexs = new List<int>();
            var _ = subString.Select((s, i) =>
            {
                if (s == '1')
                {
                    indexs.Add(i);
                }
                return s;
            });
            return indexs;
        }

        private string GetDeviceToolStatus(string plcPosition, out string front3, out string back3)
        {
            int regionSize = InteractingDevice.DeviceDescriptor.Extra["Region"].ToUshort();
            var frontSize = InteractingDevice.spindleNum / 2;
            var backSize = InteractingDevice.spindleNum / 2;
            if (InteractingDevice.spindleNum % 2 != 0)
            {
                frontSize += 1;
            }
            var toolBuffer = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra[plcPosition].ToUshort(), 2);
            front3 = string.Join("", Convert.ToString(toolBuffer[0], 2).PadLeft(16, '0').Reverse().ToArray()).ToString().Substring(0, frontSize * regionSize);
            back3 = string.Join("", Convert.ToString(toolBuffer[1], 2).PadLeft(16, '0').Reverse().ToArray()).ToString().Substring(0, backSize * regionSize);
            var allToolBuffer = string.Concat(front3, back3);
            return allToolBuffer;
        }

        private static void ReportAtp(string oldAtpFile, AtpDTO atpDTO)
        {
            List<Dictionary<string, string>> arranges = new List<Dictionary<string, string>>();
            foreach (var dto in atpDTO.magazineDTOs)
            {
                if (dto.magazineId < 1)
                    continue;
                Dictionary<string, string> detail = new Dictionary<string, string>();
                {
                    detail.Add("boxCode", "");
                    detail.Add("spindle", "");
                    int boxIndex = 1;
                    if (dto.magazineId > 50)
                    {
                        boxIndex = dto.magazineId % 50 == 0 ? dto.magazineId / 50 : dto.magazineId / 50 + 1;
                    }
                    int side = (dto.magazineId - (boxIndex - 1) * 50) % 51;
                    detail.Add("boxIndex", boxIndex.ToString()); detail.Add("site", side.ToString());
                    detail.Add("pgmdiameter", dto.toolDiameter.ToString());
                    //detail.Add("mocount", "0");
                    detail.Add("life", (dto.toolLife - dto.toolUseLife).ToString());
                }
                arranges.Add(detail);
            }
            Dictionary<string, object> dataS = new Dictionary<string, object>() { { "arranges", arranges } };
            Dictionary<string, object> dic = new Dictionary<string, object>() { { "itemCode", oldAtpFile.Trim() }, { "data", dataS } };

            string jsonData = JsonSerializer.Serialize(dic);
        }

        private ushort TrayIdsToUnshort(int[] trayIds)
        {
            ushort data = 0;
            foreach (int trayId in trayIds)
            {
                data = (ushort)(data | 1 << (trayId - 1));
            }
            return data;
        }

        private string GetLastCreateAtpByCnc84()
        {
            DirectoryInfo atpFolder = new DirectoryInfo(OLDATPFILEDIRECTORY);
            var atpFile = atpFolder.GetFiles().ToList().OrderBy(s => s.CreationTime).LastOrDefault();
            var s = atpFile?.Name;
            return s ?? string.Empty;
        }

        public Task LoadFileFromCenter() => throw new NotImplementedException();
    }
}
