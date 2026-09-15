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

using System.Text.RegularExpressions;

using Microsoft.Extensions.Logging;

using VgAutoDrill.Fundation.CNC.Model;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.PropertyHandler
{
    public class FrontPanel95NoBufferPropertyHandler : DeviceShare<DefaultDrill>, IDrillPropertyHandler
    {
        private readonly ILogger<FrontPanel95NoBufferPropertyHandler> logger;

        public readonly ISet<int> sequenceError = new HashSet<int>() {
            1501, 1502, 1503, 1504, 1505, 1507, 1508, 1509, 1510, 1511,
            1512, 1513, 1514, 1515, 1517, 1518, 1519, 1520, 1521, 1522,
            1523, 1524, 1525, 1526, 1528, 1530, 1531, 1532, 1533, 1534,
            1535, 1536, 1537, 1538, 1539, 1540, 1544, 1545, 1546, 1547,
            1548, 1550, 1551, 1552, 1553, 1554, 1555, 1556, 1557, 1559,
            1560, 1561, 1562, 1563, 1564, 1565, 1566, 1567, 1568, 1569,
            1570, 1571, 1572, 1573, 1574, 1575, 1576, 1577, 1578, 1579,
            1580, 1581, 1582, 1583, 1584, 1585, 1587, 1588, 1590, 1595,
            1596, 1598, 1599, 1600, 1603, 1604, 1605, 1606, 1607, 1608,
            1609, 1610, 1611, 1612, 1613, 1615, 1616, 1617, 1625, 1626,
            1627, 1628, 1629, 1630, 1637 };

        public readonly ISet<int> sequenceAlarm = new HashSet<int>() { 1042, 1102 };
        public readonly bool scanGunOnOff;
        public readonly bool airlOnOff;
        public readonly bool codeReaderTriggerIsM;
        public readonly bool loadFullMaterialOnOff;
        public readonly bool firstCheckOnOff;

        public FrontPanel95NoBufferPropertyHandler(ILogger<FrontPanel95NoBufferPropertyHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            scanGunOnOff = device.DeviceDescriptor.Extra["ScanGunOnOff"].ToBool();
            airlOnOff = device.DeviceDescriptor.Extra["AirlOnOff"].ToBool();
            codeReaderTriggerIsM = device.DeviceDescriptor.Extra["CodeReaderTriggerIsM"].ToBool();
            loadFullMaterialOnOff = device.DeviceDescriptor.Extra["LoadFullMaterialOnOff"].ToBool();
            firstCheckOnOff = device.DeviceDescriptor.Extra["FirstCheckOnOff"].ToBool();
        }

        public Dictionary<string, object?> DrillInformation()
        {
            var properties = new Dictionary<string, object>();
            try
            {
                if (InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus())
                {

                    properties.Add("Drill_Tody", DateTime.Today);
                    properties.Add("ASplineIsWorkFullTable", InteractingDevice.DeviceDescriptor.Extra["ASplineIsWorkFullTable"].ToBool());

                    properties.Add("Drill_ConnectionStatus", true);
                    //opcUaClient.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1")?"1:1":"1:0"
                    properties.Add("Drill_FrontMushroom", InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["FrontMushroomCloseLocalTionFlag"].ToInt()) == "1:1");//64
                    Task.Delay(200);
                    if (InteractingDevice.middleMushroomExist)
                    {
                        //opcUaClient.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2")?"1:1":"1:0"
                        properties.Add("Drill_MiddleMushroom", InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["MiddleMushroomCloseLocalTionFlag"].ToInt()) == "1:1");//55
                        Task.Delay(200);
                    }
                    /*
                    //opcUaClient.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.FCall")?"1:1":"1:0"
                    var data = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["DrilBoardEndFlag"].ToInt());
                    Task.Delay(200);
                    var drillHoleEnd = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["DrilBoardEndFlag"].ToInt()) == "1:1";
                    properties.Add("Drill_DrillHoleEnd", drillHoleEnd);
                    Task.Delay(200);

                    //CNC95 P4直接接入PLC P4FrontPostion 12313  P4RearPostion 12312
                    //var parkingStationEndFrontFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["P4FrontPostion"].ToInt());//12313
                    //var parkingStationEndBehindFlag = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["P4RearPostion"].ToInt());//12312
                    var parkingStationEndFrontFlag = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["P4FrontPostion"].ToUshort(), 1)[0] ? "1:1" : "1:0";//12313 W2.5
                    Task.Delay(200);
                    var parkingStationEndBehindFlag = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["P4RearPostion"].ToUshort(), 1)[0] ? "1:1" : "1:0";//12312 W2.4
                    var parkingFlag = parkingStationEndFrontFlag == "1:1" && parkingStationEndBehindFlag == "1:0";//12312 W2.4前不亮 12313 W2.5后亮
                    //logger.LogDebug($"DrillInformation:后W2.4不亮->{parkingStationEndBehindFlag} 前W2.5亮->{parkingStationEndFrontFlag} properties.Add->Drill_ParkingPosition->{parkingFlag}");
                    properties.Add("Drill_ParkingPosition", parkingFlag);
                    Task.Delay(200);

                    //opcUaClient.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/SF.BoardOver");
                    var pressBoardEndFlag = InteractingDevice.cnc84Command.GetSeqFlag(DeviceDescriptor.Extra["PressBoardEndFlagOnCNC84"].ToInt());//68
                    properties.Add("Drill_PressBoardEnd", pressBoardEndFlag == "1");
                    Task.Delay(200);
                    */
                    //opcUaClient.ReadNode<String>("ns=4;s=UI/origin/DDETable/CncStatus");
                    var cncStatus = InteractingDevice.cnc84Command.GetCncStatus();
                    var splineStatus = cncStatus?.SpindleStatus;
                    if (splineStatus != null)
                    {
                        splineStatus = splineStatus?.Substring(splineStatus.Length - InteractingDevice.spindleNum);
                        char[] arr = splineStatus!.ToCharArray();
                        Array.Reverse(arr);
                        properties.Add("Drill_SplineStatus", string.Join("", arr));
                    }
                    else
                    {
                        properties.Add("Drill_SplineStatus", Enumerable.Repeat("1", DeviceDescriptor.SpindleNum));
                    }
                    if (cncStatus != null)
                    {
                        properties.Add("Drill_Percentage", cncStatus.FinPer);
                    }
                    Task.Delay(200);

                    var subProperties = DrillScreenText();
                    foreach (var item in subProperties)
                    {
                        properties.Add(item.Key, item.Value);
                    }
                    //添加首件检测
                    if (firstCheckOnOff)
                    {
                        try
                        {
                            bool firstCheckFlag = InteractingDevice.cnc84Command.ReadCncNode<bool>($"ns=4;s=UI/normalized/custom/{InteractingDevice.DeviceDescriptor.Extra["FirstCheckFlag"].ToStr()}");
                            properties.Add("Drill_FirstCheckFlag", firstCheckFlag);

                            bool firstCheckResult = InteractingDevice.cnc84Command.ReadCncNode<bool>($"ns=4;s=UI/normalized/custom/{InteractingDevice.DeviceDescriptor.Extra["FirstCheckResult"].ToStr()}");
                            properties.Add("Drill_FirstCheckResult", firstCheckResult);

                        }
                        catch (Exception ee)
                        {


                        }

                    }
                    try
                    {
                        var programPath = InteractingDevice.cnc84Command.GetACTProgram();
                        Task.Delay(200);
                        var parameterPath = InteractingDevice.cnc84Command.GetDiaFileNameWithDialog();
                        Task.Delay(200);
                        var atpPath = InteractingDevice.cnc84Command.GetAtpFileName();
                        properties.Add("Drill_CurrentProgramFile", programPath);
                        properties.Add("Drill_CurrentParameterFile", parameterPath);
                        properties.Add("Drill_CurrentAtpFile", atpPath);
                        Task.Delay(200);
                        //2024-06-24
                        Double[] dataValueUtilisation = InteractingDevice.cnc84Command.ReadCncNode<Double[]>("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Utilisation");//当班嫁动率 OK
                        Double[] dataValueWorkingTime = InteractingDevice.cnc84Command.ReadCncNode<Double[]>("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.WorkingTime");//OK
                        Double[] dataValueWaitingTime = InteractingDevice.cnc84Command.ReadCncNode<Double[]>("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.WaitingTime");//OK
                        Double[] dataValueErrorTime = InteractingDevice.cnc84Command.ReadCncNode<Double[]>("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.ErrorTime");//OK
                        Double[] dataValueActiveTime = InteractingDevice.cnc84Command.ReadCncNode<Double[]>("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.ActiveTime");//OK
                        TimeSpan timeSpan = TimeSpan.FromMilliseconds(dataValueWorkingTime[0]);
                        var worktime = timeSpan.TotalMinutes.ToString("#0");
                        timeSpan = TimeSpan.FromMilliseconds(dataValueWaitingTime[0]);
                        var waittime = timeSpan.TotalMinutes.ToString("#0");
                        timeSpan = TimeSpan.FromMilliseconds(dataValueErrorTime[0]);
                        var errortime = timeSpan.TotalMinutes.ToString("#0");
                        timeSpan = TimeSpan.FromMilliseconds(dataValueActiveTime[0]);
                        var opentime = timeSpan.TotalMinutes.ToString("#0");

                        var duty = Math.Round(dataValueUtilisation[0], 0).ToString();

                        properties.Add("Drill_CurrentOpentime", opentime);
                        properties.Add("Drill_CurrentWorktime", worktime);
                        properties.Add("Drill_CurrentWaittime", waittime);
                        properties.Add("Drill_CurrentErrortime", errortime);
                        properties.Add("Drill_CurrentDuty", duty);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, ex.Message);
                    }

                    //
                    properties.Add("Drill_CollectCleanTime", DefaultDrill.CollectCleanTime.ToString("#0"));
                    properties.Add("Drill_EndToStartTime", StaticEndToStartTime().ToString("#0"));
                }
                else
                {
                    properties.Add("Drill_ConnectionStatus", false);
                    Task.Delay(2000);
                    InteractingDevice.Engine.DeviceConnector.IsConnected = false;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return properties!;
        }

        private bool initFlag = false;


        public double StaticEndToStartTime()
        {
            double endToStartTime = 0;
            try
            {
                string dateString = "0001/01/01 08:00:00";
                var originDateTime = DateTime.Parse(dateString);
                DateTime now = DateTime.Now;
                DateTime today = DateTime.Today;

                DateTime[] dataValueRunStartTime = InteractingDevice.cnc84Command.ReadCncNode<DateTime[]>("ns=4;s=UI/origin/AutoList/AutoList.List/AutoList.RunStartTime");//程序开始时间
                DateTime[] dataValueRunEndTime = InteractingDevice.cnc84Command.ReadCncNode<DateTime[]>("ns=4;s=UI/origin/AutoList/AutoList.List/AutoList.RunEndTime");//程序结束时间
                dataValueRunStartTime = dataValueRunStartTime.Select(s => s.AddHours(8)).ToArray();
                dataValueRunEndTime = dataValueRunEndTime.Select(s => s.AddHours(8)).ToArray();
                if (dataValueRunStartTime.All(s => s == originDateTime))
                {
                    return endToStartTime += now.Subtract(today).TotalMinutes;

                }

                var lastRunStartTime = dataValueRunStartTime.Max();
                var lastRunStartTimeIndex = Array.FindIndex(dataValueRunStartTime, s => s == dataValueRunStartTime.Max());
                var lastRunEndTime = dataValueRunEndTime.Max();
                var lastRunEndTimeIndex = Array.FindIndex(dataValueRunEndTime, s => s == dataValueRunEndTime.Max());

                int index = Array.FindIndex(dataValueRunStartTime, d => d.Subtract(today).Ticks >= 0);
                if (index == -1)
                {
                    if (lastRunEndTimeIndex == lastRunStartTimeIndex)
                    {
                        if (dataValueRunEndTime[lastRunEndTimeIndex].Subtract(today).Ticks >= 0)
                        {
                            return endToStartTime += now.Subtract(dataValueRunEndTime[lastRunEndTimeIndex]).TotalMinutes;
                        }
                        else
                        {
                            return endToStartTime += now.Subtract(today).TotalMinutes;
                        }

                    }
                    else
                    {
                        return 0;
                    }


                }
                if (index - 1 >= 0)
                {
                    index = index - 1;
                }

                dataValueRunStartTime = dataValueRunStartTime.Skip(index).ToArray();
                dataValueRunEndTime = dataValueRunEndTime.Skip(index).ToArray();

                lastRunEndTime = dataValueRunEndTime.Max();
                lastRunStartTime = dataValueRunStartTime.Max();
                if (dataValueRunStartTime[0].Subtract(today).Ticks > 0)
                {
                    endToStartTime += dataValueRunStartTime[0].Subtract(today).TotalMinutes;
                    if (dataValueRunStartTime[0] == lastRunStartTime && dataValueRunEndTime[0] != originDateTime && dataValueRunEndTime[0] == lastRunEndTime)
                    {
                        endToStartTime += now.Subtract(dataValueRunEndTime[0]).TotalMinutes;
                    }
                }

                for (global::System.Int32 i = 1; i < dataValueRunStartTime.Length; i++)
                {
                    if (dataValueRunStartTime[i] == originDateTime)
                    {
                        continue;
                    }

                    if (dataValueRunEndTime[i - 1].Subtract(today).Ticks < 0)
                    {
                        endToStartTime += dataValueRunStartTime[i].Subtract(today).TotalMinutes;
                    }
                    else
                    {
                        endToStartTime += dataValueRunStartTime[i].Subtract(dataValueRunEndTime[i - 1]).TotalMinutes;

                    }
                    if (dataValueRunStartTime[i] == lastRunStartTime && (dataValueRunEndTime[i] == lastRunEndTime) && lastRunEndTime != originDateTime)
                    {
                        endToStartTime += now.Subtract(dataValueRunEndTime[i]).TotalMinutes;
                    }

                }
                logger.LogInformation($"StatisticsJob   endToStartTime:{endToStartTime}");

            }
            catch (Exception e)
            {

                logger.LogError($"StatisticsJob {e.Message}");
            }
            return endToStartTime;


        }

        public Dictionary<string, object?> OtherInformation()
        {
            var properties = new Dictionary<string, object>();
            return properties!;
        }

      

        public void AddWatchingProperties()
        {
            WatchingProperties.AddProperty("Drill_ConnectionStatus", false)
                              .AddProperty("Drill_ScreenText", "")
                              .AddProperty("Drill_FrontMushroom", "")
                              .AddProperty("Drill_MiddleMushroom", "")
                              .AddProperty("Drill_ParkingPosition", "")
                              .AddProperty("DrillBuffer_LoadAndUnloadEnd", 0)
                              .AddProperty("Drill_BoardPositionStatus", "-1")
                              .AddProperty("Drill_WarningCode", "0")
                              .AddProperty("Drill_NoError", "0")
                              .AddProperty("Drill_Halt", false)
                              .AddProperty("Drill_DrillHoleEnd", false)
                              .AddProperty("Drill_DrillHoleStart", true)
                              .AddProperty("Drill_CodeMessage", "")
                              .AddProperty("Drill_SplineStatus", string.Join("", Enumerable.Repeat("1", InteractingDevice.spindleNum).ToArray()))
                              .AddProperty("Drill_RealCodeMessage", "")
                              .AddProperty("Drill_PressBoardEnd", "0")
                              .AddProperty("Drill_EscEnd", false)
                              .AddProperty("PrepareLoadOk", false)
                              .AddProperty("InvokeLoadOk", false)
                              .AddProperty("CompleteLoadOk", false)
                              .AddProperty("PrepareUnloadOk", false)
                              .AddProperty("InvokeUnloadOk", false)
                              .AddProperty("CompleteUnload", false)
                              .AddProperty("CompleteAllLoadOrUnloadAction", false)
                              .AddProperty("Drill_CurrentProgramFile", "")
                              .AddProperty("Drill_CurrentParameterFile", "")
                              .AddProperty("Drill_BufferLockFlag", false)
                              .AddProperty("Drill_CurrentAtpFile", "")
                              .AddProperty("AutoFlag", false)
                              .AddProperty("MqttConnected", false)
                              .AddProperty("TranscationId", "")
                              .AddProperty("NewTranscationId", "")
                              .AddProperty("Drill_CurrentOpentime", 0)
                              .AddProperty("Drill_CurrentWorktime", 0)
                              .AddProperty("Drill_CurrentWaittime", 0)
                              .AddProperty("Drill_CurrentErrortime", 0)
                              .AddProperty("SuggestPanelInteractionSequence", InteractionSequence.None)
                              .AddProperty("IsAvailbleForAgv", false)
                              .AddProperty("Buffer_CodeReaderTrigger", 0)
                              .AddProperty("Drill_CurrentDuty", "")
                              .AddProperty("Drill_Percentage", "")
                              .AddProperty("Drill_FirstCheckFlag", false)
                              .AddProperty("Drill_FirstCheckResult", false)
                              .AddProperty("Drill_ShowText", "")
                              .AddProperty("Drill_CollectCleanBegin", "")
                              .AddProperty("Drill_CollectCleanEnd", "")
                              .AddProperty("Drill_CollectCleanTime", 0)
                              .AddProperty("Drill_Tody", DateTime.Today.AddDays(-1))
                              .AddProperty("Drill_EndToStartTime", "0") 
                              .AddProperty("ASplineIsWorkFullTable", "")
                              .AddProperty("RunState", -1)

                               ;
        }

        private Dictionary<string, object> DrillScreenText()
        {
            var screenProperties = new Dictionary<string, object>();
            //var vgCNCScreenSaver = InteractingDevice.cnc84Command.GetScreenText();
            //var isErrorOnCNC84 = CNC84Error(vgCNCScreenSaver);
            //string screenText = vgCNCScreenSaver?.ScreenText ?? "";
            //Double ProgramErrorTime, ProgramRunTime, ProgramWaitTime, ProgressInPercent;

            Double ProgressInPercent = InteractingDevice.cnc84Command.ReadCncNode<Double>("ns=4;s=UI/origin/WorkGroupSettings/WorkGroupInterface/TimeWidget/ProgressInPercent");

            InteractingDevice.percentage = (int)ProgressInPercent * 100;
            // BlockBackgroundColor红色:5066239BlockText:未找到T1刀具BlockTextColor:4340265
            String ScreenText = InteractingDevice.cnc84Command.ReadCncNode<String>("ns=4;s=UI/origin/RosiInfo/BlockText");
            Int32 BlockBackgroundColor = InteractingDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/RosiInfo/BlockBackgroundColor");
            Int32 BlockTextColor = InteractingDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/RosiInfo/BlockTextColor");
            screenProperties.Add("Drill_ScreenText", ScreenText);
            if (BlockBackgroundColor == 5066239 && BlockTextColor == 4340265)
                screenProperties.Add("Drill_NoError", true);
            else
                screenProperties.Add("Drill_NoError", false);
            return screenProperties;
        }

        [Obsolete("not in use", true)]
        private bool CNC84Error(VgCNCScreenSaver vgCNCScreenSaver)
        {
            if (vgCNCScreenSaver == null) { return false; }
            var color = vgCNCScreenSaver.BackColor;
            var message = vgCNCScreenSaver.ScreenText;
            switch (color)
            {
                case "5066239"://eg.BlockBackgroundColor红色:5066239BlockText:未找到T1刀具BlockTextColor:4340265
                    {
                        var messageMath = Regex.Match(message, "\\[([\\d]*)\\]");

                        if (messageMath.Success)
                        {
                            var errorId = int.Parse(messageMath.Groups[1].Value);
                            return errorId != 48;
                        }
                        break;
                    }
                case "FF0000":
                    {
                        var messageMath = Regex.Match(message, "\\[([\\d]*)\\]");

                        if (messageMath.Success)
                        {
                            var errorId = int.Parse(messageMath.Groups[1].Value);
                            return sequenceError.Contains(errorId);
                        }
                        break;
                    }
                case "8000":
                    {
                        var messageMath = Regex.Match(message, "\\[([\\d]*)\\]");

                        if (messageMath.Success)
                        {
                            var errorId = int.Parse(messageMath.Groups[1].Value);
                            return sequenceAlarm.Contains(errorId);
                        }
                        break;
                    }
                default: break;
            }
            return false;
        }

        private float UshortArrayToFloat(ushort[] ushortArray)
        {
            float result = 0.0f;
            if (ushortArray.Length == 2)
            {
                byte[] floatBytes = new byte[ushortArray.Length * 2];
                for (int i = 0; i < ushortArray.Length; i++)
                {
                    byte[] bytes = BitConverter.GetBytes(ushortArray[i]);
                    Array.Copy(bytes, 0, floatBytes, i * 2, 2);
                }
                float floatValue = BitConverter.ToSingle(floatBytes, 0);
                return floatValue;
            }
            return result;
        }
    }
}
