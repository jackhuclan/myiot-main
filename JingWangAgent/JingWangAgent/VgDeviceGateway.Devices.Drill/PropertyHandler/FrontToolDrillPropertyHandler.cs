using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.PropertyHandler
{
    public class FrontToolDrillPropertyHandler : DeviceShare<DefaultDrill>, IDrillPropertyHandler
    {
        private readonly ILogger<FrontToolDrillPropertyHandler> logger;
        public readonly byte slaveID;
        public readonly int regionSize;

        public FrontToolDrillPropertyHandler(ILogger<FrontToolDrillPropertyHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            regionSize = InteractingDevice.DeviceDescriptor.Extra["Region"].ToUshort();
        }

        public Dictionary<string, object?> DrillInformation()
        {
            var properties = new Dictionary<string, object>();
            try
            {
                if (InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus())
                {
                    properties.Add("Drill_ConnectionStatus", true);

                    var parking1StationEndFrontFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["ToolBufferP1FrontPostion"].ToInt());
                    var parking1StationEndBehindFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["ToolBufferP1RearPostion"].ToInt());
                    var parking1Flag = parking1StationEndFrontFlag == "1:0" && parking1StationEndBehindFlag == "1:1";
                    properties.Add("Drill_Parking1Position", parking1Flag);

                    var parking2StationEndFrontFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["ToolBufferP2FrontPostion"].ToInt());
                    var parking2StationEndBehindFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["ToolBufferP2RearPostion"].ToInt());
                    var parking2Flag = parking2StationEndFrontFlag == "1:0" && parking2StationEndBehindFlag == "1:1";
                    properties.Add("Drill_Parking2Position", parking2Flag);

                    //var yPosition = InteractingDevice.cnc84Command.GetRuntimeString("%S(FIXXY_4)");
                    var yPosition = InteractingDevice.cnc84Command.GetRuntimeValue("AxRealPos(1)");
                    if (!string.IsNullOrEmpty(yPosition))
                    {
                        //var yPositionTemp = yPosition.Substring(2, yPosition.Length - 2);
                        var yPositionTemp = yPosition;
                        var p2Position = (ushort)(yPositionTemp == DeviceDescriptor.Extra["P2Postion"].ToStr() ? 1 : 0);
                        logger.LogDebug($"钻孔是否在P2位置：{p2Position}");
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["DrillOnP2Position"].ToUshort(), p2Position);

                        properties.Add("Drill_YPosition", yPositionTemp);
                    }
                }
                else
                {
                    properties.Add("Drill_ConnectionStatus", false);
                    InteractingDevice.Engine.DeviceConnector.IsConnected = false;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return properties;
        }

        public Dictionary<string, object?> OtherInformation()
        {
            var properties = new Dictionary<string, object>();
            try
            {
                if (InteractingDevice.Engine.DeviceConnector.IsConnected)
                {
                    properties.Add("Buffer_ConnectionStatus", true);

                    properties.Add("MqttConnected", InteractingDevice.MqttClientWrapper?.IsConnected == null ? false : MqttClientWrapper?.IsConnected);

                    var toolFCall = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["ToolFCallPostion"].ToUshort(), 1)[0];
                    logger.LogDebug($"钻孔结束信号：{!toolFCall}");

                    properties.Add("Drill_DrillHoleEnd", !toolFCall);

                    var otherWarning = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ToolBufferOtherWarningInfoWriteOnPlc"].ToUshort(), 1)[0];
                    properties.Add("Tool_OtherWarning", otherWarning);

                    InteractingDevice.bufferAllUnloadAndLoadEnd = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ToolBufferAgvAllUnloadAndLoadEndOnPlc"].ToUshort(), 1)[0];

                    var allToolDrillStatus = GetDeviceToolStatus("ToolBufferDrillStatusOnPlc", out string front3, out string back3);
                    properties.Add("Tool_DrillFront3Status", front3);
                    properties.Add("Tool_DrillBack3Status", back3);
                    properties.Add("Tool_DrillStatus", allToolDrillStatus);

                    var allToolBufferStatus = GetDeviceToolStatus("ToolBufferStatusOnPlc", out string front3ToolBuffer, out string back3ToolBuffer);
                    properties.Add("Tool_BufferFront3Status", front3);
                    properties.Add("Tool_BufferBack3Status", back3);
                    properties.Add("Tool_BufferStatus", allToolBufferStatus);

                    var waringFlag = IsWarningWriteOnPlc(allToolDrillStatus, allToolBufferStatus);
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ToolBufferWarningInfoWriteOnPlc"].ToUshort(), (ushort)(waringFlag ? 1 : 0));
                    var warningInfo = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ToolBufferWarningInfoWriteOnPlc"].ToUshort(), 1);
                    properties.Add("Tool_BufferWarningInfoWriteOnPlc", warningInfo[0] != 0);

                    var allToolBufferNewOld = GetDeviceToolStatus("ToolBufferNewOldOnPlc", out string front3ToolBufferNewOld, out string back3ToolBufferNewOld);
                    properties.Add("Tool_BufferFront3NewOldStatus", front3ToolBufferNewOld);
                    properties.Add("Tool_BufferBack3NewOldStatus", back3ToolBufferNewOld);
                    properties.Add("Tool_BufferNewOldStatus", allToolBufferNewOld);
                    //var allToolBufferRealNewOld = string.Join("", allToolBufferNewOld.Select((s, i) => allToolBufferStatus[i] == '0' ? '0' : s));
                    var allToolBufferRealNewOld = allToolBufferNewOld.Select((s, i) =>
                    {
                        if (allToolBufferStatus[i] == '0')
                        {
                            return 0;
                        }
                        else if (s == '0')
                        {
                            return 1;
                        }
                        else if (s == '1')
                        {
                            return 2;
                        }
                        return s;
                    });
                    properties.Add("Tool_BufferRealNewOldStatus", string.Join("", allToolBufferRealNewOld));

                    var toolBufferUnloadAndLoadEnd = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ToolBufferDrillUnLoadingAndLoadingOnPlc"].ToUshort(), 1);
                    properties.Add("Tool_BufferDrillLoadAndUnLoadTrayStart", toolBufferUnloadAndLoadEnd[0] == 1);
                    properties.Add("Tool_BufferDrillLoadAndUnLoadTrayEnd", toolBufferUnloadAndLoadEnd[0] == 0);

                    var toolBufferAgvLoadAndUnLoadTray = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ToolBufferAgvLoadAndUnLoadTray"].ToUshort(), 1);
                    properties.Add("Tool_BufferAgvLoadAndUnLoadTray", toolBufferAgvLoadAndUnLoadTray[0] == 1);
                }
                else
                {
                    properties.Add("Buffer_ConnectionStatus", false);
                    InteractingDevice.Engine.DeviceConnector.IsConnected = false;
                }
            }
            catch (Exception e)
            {
                InteractingDevice.plcConnentFlag = false;
                logger.LogError(e, e.Message);
                InteractingDevice.Engine.DeviceConnector.IsConnected = false;
            }
            return properties;
        }

        private List<int> DrillExistTrayRegions(string drillStatus)
        {
            return ExistTrayRegions(drillStatus, s => s == '1');
        }

        private List<int> BufferExistTrayRegions(string bufferStatus)
        {
            return ExistTrayRegions(bufferStatus, s => s == '1');
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

        private bool IsWarningWriteOnPlc(string drillStatus, string toolBufferStatus)
        {
            List<int> drillRegions = DrillExistTrayRegions(drillStatus);
            List<int> bufferRegions = BufferExistTrayRegions(toolBufferStatus);
            // 2号区域问题
            if (drillRegions.Contains(1) || drillRegions.Contains(1))
            {
                return true;
            }

            if (drillRegions.Intersect(bufferRegions).Count() > 0)
            {
                return true;
            }

            for (int i = 0; i < drillStatus.Length; i++)
            {
                if (drillStatus[i] == '1' && toolBufferStatus[i] == '1')
                {
                    return true;
                }
            }

            //干涉造成的影响
            for (int i = 0; i < toolBufferStatus.Length; i += regionSize)
            {
                var subSpindleBehavior = toolBufferStatus.Skip(i).Take(regionSize).ToArray();
                var subDrillStatus = drillStatus.Skip(i).Take(regionSize).ToArray();
                if (subDrillStatus[1] == '1')
                {
                    return true;
                }
                if (subDrillStatus.Count(s => s == '1') >= 2)
                {
                    return true;
                }

                //2号区域坏掉
                if (subSpindleBehavior[1] != '0')
                {
                    return true;
                }

                if (subSpindleBehavior.Count(s => s == '1') >= 2)
                {
                    return true;
                }
                else if (subSpindleBehavior.Count(s => s == '1') == 2)
                {
                    var subString = new string(subSpindleBehavior);
                    var index = subString.IndexOf("0");
                    if (index == 0 || index == 2)
                    {
                        return true;
                    }
                }
            }

            return false;
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

        public void AddWatchingProperties()
        {
            WatchingProperties.AddProperty("Drill_ConnectionStatus", false)
                             .AddProperty("Buffer_ConnectionStatus", false)
                             .AddProperty("Drill_DrillHoleEnd", false)
                             .AddProperty("Tool_DrillStatus", "")
                             .AddProperty("Tool_DrillFront3Status", "")
                             .AddProperty("Tool_DrillBack3Status", "")
                             .AddProperty("Tool_BufferStatus", "")
                             .AddProperty("Tool_BufferFront3Status", "")
                             .AddProperty("Tool_BufferBack3Status", "")
                             .AddProperty("Tool_BufferNewOldStatus", "")
                             .AddProperty("Tool_BufferRealNewOldStatus", "")
                             .AddProperty("Tool_BufferFront3NewOldStatus", "")
                             .AddProperty("Tool_BufferBack3NewOldStatus", "")
                             .AddProperty("Tool_BufferLoadTrayPosition", 0)
                             .AddProperty("Tool_BufferDrillLoadAndUnLoadTrayStart", false)
                             .AddProperty("Tool_BufferAgvLoadAndUnLoadTray", true)
                             .AddProperty("MqttConnected", false)
                             .AddProperty("Tool_BufferWarningInfoWriteOnPlc", 0)
                             .AddProperty("Drill_Parking1Position", 0)
                             .AddProperty("Drill_Parking2Position", 0)
                             .AddProperty("Drill_YPosition", "")
                             .AddProperty("Tool_OtherWarning", 0)
                             .AddProperty("Tool_BufferDrillLoadAndUnLoadTrayEnd", true);
        }
    }
}
