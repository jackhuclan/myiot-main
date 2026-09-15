using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.CNC.Model;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.PropertyHandler
{
    public class FrontPanelDrillPropertyHandler : DeviceShare<DefaultDrill>, IDrillPropertyHandler
    {
        private readonly ILogger<FrontPanelDrillPropertyHandler> logger;

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

        public FrontPanelDrillPropertyHandler(ILogger<FrontPanelDrillPropertyHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public Dictionary<string, object?> DrillInformation()
        {
            var properties = new Dictionary<string, object>();
            properties.Add("AllUnloadAndLoadEnd", InteractingDevice.bufferAllUnloadAndLoadEnd);
            properties.Add("IsLastStep", InteractingDevice.IsLastStep);
            try
            {
                if (InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus())
                {
                    properties.Add("MaterialEnsureFlag", InteractingDevice.MaterialEnsureFlag);
                    properties.Add("MqttConnected", MqttClientWrapper?.IsConnected == null ? false : MqttClientWrapper?.IsConnected);
                    properties.Add("Drill_ConnectionStatus", true);
                    var workEndStr = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["DrilBoardEndFlag"].ToInt());//== "1:1"
                    Thread.Sleep(10);
                    var workStartStr = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["WorkFlag"].ToInt());

                    //if(!string.IsNullOrEmpty(workEndStr))
                    //{
                    //    propties.Add("Drill_WorkEnd", workEndStr == "1:1");
                    //}

                    if (!string.IsNullOrEmpty(workStartStr))
                    {
                        properties.Add("Drill_WorkStart", workStartStr == "1:1");
                        properties.Add("Drill_WorkEnd", workStartStr == "1:0");
                    }

                    Thread.Sleep(10);
                    var airClose = (InteractingDevice.cnc84Command.GetIutput(DeviceDescriptor.Extra["AirCloseFlag"].ToInt()) == "1:2");
                    properties.Add("Drill_SpindleAirError", airClose);
                    Thread.Sleep(10);
                    var str = InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["ClampOpenFlag"].ToInt());
                    if (!string.IsNullOrEmpty(str))
                    {
                        var clampOpenFlag = (str == "1:1");
                        properties.Add("Drill_ClampOpenFlag", clampOpenFlag);
                    }

                    //var splineStatus = InteractingDevice.cnc84Command.GetCncStatus()?.SpindleStatus;
                    //if (splineStatus != null)
                    //{
                    //    splineStatus = splineStatus?.Substring(splineStatus.Length - InteractingDevice.spindleNum);
                    //    char[] arr = splineStatus.ToCharArray();
                    //    Array.Reverse(arr);
                    //    properties.Add("Drill_SplineStatus", string.Join("", arr));
                    //}
                    //else
                    //{
                    //    properties.Add("Drill_SplineStatus", Enumerable.Repeat("1", DeviceDescriptor.SpindleNum));
                    //}

                    //var subProperties = DrillScreenText();
                    //foreach (var item in subProperties)
                    //{
                    //    properties.Add(item.Key, item.Value);
                    //}

                    //try
                    //{
                    //    var programPath = InteractingDevice.cnc84Command.GetACTProgram();
                    //    var parameterPath = InteractingDevice.cnc84Command.GetDiaFileNameWithDialog();
                    //    var atpPath = InteractingDevice.cnc84Command.GetAtpFileName();
                    //    properties.Add("Drill_CurrentProgramFile", programPath);
                    //    properties.Add("Drill_CurrentParameterFile", parameterPath);
                    //    properties.Add("Drill_CurrentAtpFile", atpPath);
                    //    var opentime = InteractingDevice.cnc84Command.GetRuntimeValue("PC_HisBdeTimes(1262)");
                    //    var worktime = InteractingDevice.cnc84Command.GetRuntimeValue("PC_HisBdeTimes(1363)");
                    //    var waittime = InteractingDevice.cnc84Command.GetRuntimeValue("PC_HisBdeTimes(1464)");
                    //    var errortime = InteractingDevice.cnc84Command.GetRuntimeValue("PC_HisBdeTimes(1565)");
                    //    var duty = InteractingDevice.cnc84Command.GetRuntimeValue("PC_HisBdeData(1867)");

                    //    properties.Add("Drill_CurrentOpentime", opentime);
                    //    properties.Add("Drill_CurrentWorktime", worktime);
                    //    properties.Add("Drill_CurrentWaittime", waittime);
                    //    properties.Add("Drill_CurrentErrortime", errortime);
                    //    properties.Add("Drill_CurrentDuty", duty);
                    //}
                    //catch (Exception)
                    //{
                    //}
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
                if (InteractingDevice.frontExtendDevice.IsConnected)
                {
                    properties.Add("ExtendDevice_ConnectionStatus", true);
                    var datas = InteractingDevice.frontExtendDevice.ReadCoilsAll();
                    if (datas != null)
                    {
                        properties.Add("Drill_DoorFlag", datas.DoorState);
                        for (int i = 0; i < datas.frontSplindleStates.Length; i++)
                        {
                            var splindleInf = datas.frontSplindleStates[i];
                            properties.Add($"Drill_Spline{i + 1}BigAirFlag", splindleInf.BigAirState);
                            properties.Add($"Drill_Spline{i + 1}SmallAirFlag", splindleInf.SmallAirState);
                            properties.Add($"Drill_Spline{i + 1}UpDownFlag", splindleInf.UpDownState);
                            properties.Add($"Drill_Spline{i + 1}PanelFlag", splindleInf.PanelState);
                        }
                        properties.Add($"Drill_AllUpDownUpFlag", datas.frontSplindleStates.All(s => s.UpDownState));
                        properties.Add($"Drill_AllUpDownDownFlag", datas.frontSplindleStates.All(s => !s.UpDownState));
                    }

                    var panel = InteractingDevice.frontExtendDevice.GetPanelStateString();
                    if (!string.IsNullOrEmpty(panel))
                    {
                        properties.Add($"Drill_SplineStatus", panel);
                        bool panelInfFail = false;
                        StringBuilder panelStatusSb = new StringBuilder();
                        for (int i = 0; i < panel.Length; i++)
                        {
                            if (panel[i] == '1')
                            {
                                switch (PayloadPanels[i].ProductStatus)
                                {
                                    case ProductStatus.EmptySiloBox:
                                        panelStatusSb.Append("0");
                                        panelInfFail = true;
                                        break;

                                    case ProductStatus.WaitingForDrill:
                                        panelStatusSb.Append("1");
                                        break;

                                    case ProductStatus.Drilling:
                                        panelStatusSb.Append("2");
                                        break;

                                    case ProductStatus.Finished_DRILL:
                                        panelStatusSb.Append("3");
                                        break;

                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                if (PayloadPanels[i].ProductStatus != ProductStatus.EmptySiloBox)
                                {
                                    panelInfFail = true;
                                }
                                panelStatusSb.Append("0");
                            }
                        }
                        var panelStatus = panelStatusSb.ToStr();

                        bool existRawPanel = panelStatus.Contains("1");
                        bool existDrillingPanel = panelStatus.Contains("2");
                        var isExistClinker = panelStatus.Contains("3");
                        if (!panelInfFail)
                        {
                            panelInfFail = (existDrillingPanel && isExistClinker) || (existDrillingPanel && existRawPanel);
                        }
                        properties.Add($"Drill_PanelInfFail", panelInfFail);
                    }
                    bool result = InteractingDevice.frontExtendDevice.MushroomState(out bool inFlag, out bool outFlag);
                    if (result)
                    {
                        properties.Add($"Drill_MushroomInFlag", inFlag);
                        properties.Add($"Drill_MushroomOutFlag", outFlag);
                    }
                }
                else
                {
                    properties.Add("ExtendDevice_ConnectionStatus", false);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                InteractingDevice.Engine.DeviceConnector.IsConnected = false;
            }
            return properties;
        }

        private Dictionary<string, object> DrillScreenText()
        {
            var screenProperties = new Dictionary<string, object>();
            var vgCNCScreenSaver = InteractingDevice.cnc84Command.GetScreenText();
            var isErrorOnCNC84 = CNC84Error(vgCNCScreenSaver);
            string screenText = vgCNCScreenSaver?.ScreenText ?? "";
            if (screenText.Contains("%") && int.TryParse(screenText.Split("%")[1], out int percentage))
            {
                InteractingDevice.percentage = percentage;
            }

            screenProperties.Add("Drill_ScreenText", screenText);
            screenProperties.Add("Drill_NoError", !isErrorOnCNC84);

            return screenProperties;
        }

        private bool CNC84Error(VgCNCScreenSaver vgCNCScreenSaver)
        {
            if (vgCNCScreenSaver == null) { return false; }
            var color = vgCNCScreenSaver.BackColor;
            var message = vgCNCScreenSaver.ScreenText;
            switch (color)
            {
                case "0000FF":
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

        public void AddWatchingProperties()
        {
            WatchingProperties.AddProperty("Drill_ConnectionStatus", false)
                             .AddProperty("Drill_ScreenText", "")
                             .AddProperty("Drill_WorkStart", true)
                             .AddProperty("Drill_WorkEnd", false)
                             .AddProperty("Drill_NoError", true)
                             .AddProperty("Drill_SpindleAirError", true)
                             .AddProperty("DrillDoorOpenAndAirOpenStatus", false)
                             .AddProperty("Drill_SplineStatus", "")
                             .AddProperty("PrepareLoadOk", false)
                             .AddProperty("InvokeLoadOk", false)
                             .AddProperty("CompleteLoadOk", false)
                             .AddProperty("PrepareUnloadOk", false)
                             .AddProperty("InvokeUnloadOk", false)
                             .AddProperty("CompleteUnload", false)
                             .AddProperty("CompleteAllLoadOrUnloadAction", false)
                             .AddProperty("Drill_CurrentProgramFile", "")
                             .AddProperty("Drill_CurrentParameterFile", "")
                             .AddProperty("Drill_CurrentAtpFile", "")
                             .AddProperty("MqttConnected", false)
                             .AddProperty("TranscationId", "")
                             .AddProperty("NewTranscationId", "")
                             .AddProperty("IsLastStep", false)
                             .AddProperty("Drill_CurrentOpentime", 0)
                             .AddProperty("Drill_CurrentWorktime", 0)
                             .AddProperty("Drill_CurrentWaittime", 0)
                             .AddProperty("Drill_CurrentErrortime", 0)
                             .AddProperty("MaterialEnsureFlag", 0)//板材信息
                             .AddProperty("Drill_Spline1BigAirFlag", "")//轴1大气夹
                             .AddProperty("Drill_Spline2BigAirFlag", "")
                             .AddProperty("Drill_Spline3BigAirFlag", "")
                             .AddProperty("Drill_Spline4BigAirFlag", "")
                             .AddProperty("Drill_Spline5BigAirFlag", "")
                             .AddProperty("Drill_Spline6BigAirFlag", "")
                             .AddProperty("Drill_Spline1SmallAirFlag", "")//轴1小气夹
                             .AddProperty("Drill_Spline2SmallAirFlag", "")
                             .AddProperty("Drill_Spline3SmallAirFlag", "")
                             .AddProperty("Drill_Spline4SmallAirFlag", "")
                             .AddProperty("Drill_Spline5SmallAirFlag", "")
                             .AddProperty("Drill_Spline6SmallAirFlag", "")
                             .AddProperty("Drill_Spline1UpDownFlag", "")//轴1顶升状态
                             .AddProperty("Drill_Spline2UpDownFlag", "")
                             .AddProperty("Drill_Spline3UpDownFlag", "")
                             .AddProperty("Drill_Spline4UpDownFlag", "")
                             .AddProperty("Drill_Spline5UpDownFlag", "")
                             .AddProperty("Drill_Spline6UpDownFlag", "")
                             .AddProperty("Drill_Spline1PanelFlag", "")//轴1板子状态
                             .AddProperty("Drill_Spline2PanelFlag", "")
                             .AddProperty("Drill_Spline3PanelFlag", "")
                             .AddProperty("Drill_Spline4PanelFlag", "")
                             .AddProperty("Drill_Spline5PanelFlag", "")
                             .AddProperty("Drill_Spline6PanelFlag", "")
                             .AddProperty("Drill_AllUpDownUpFlag", false)
                             .AddProperty("Drill_AllUpDownDownFlag", false)
                             .AddProperty("Buffer_CallAgvMessage", true)
                             .AddProperty("ExtendDevice_ConnectionStatus", true)
                             .AddProperty("Drill_DoorFlag", false)
                             .AddProperty("Drill_MushroomInFlag", "")
                             .AddProperty("Drill_MushroomOutFlag", "")
                             .AddProperty("Drill_PanelInfFail", false)
                             .AddProperty("Drill_ClampOpenFlag", false)
                             .AddProperty("AllUnloadAndLoadEnd", 0)
                             .AddProperty("Drill_CurrentDuty", "");
        }
    }
}
