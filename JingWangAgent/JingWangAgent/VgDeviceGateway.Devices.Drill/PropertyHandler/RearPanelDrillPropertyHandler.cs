// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.CNC.Model;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.PropertyHandler
{
    public class RearPanelDrillPropertyHandler : DeviceShare<DefaultDrill>, IDrillPropertyHandler
    {
        private readonly ILogger<RearPanelDrillPropertyHandler> logger;

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
        public readonly byte slaveID;
        public readonly bool scanGunOnOff;

        //public readonly bool noLoadMaterialOnOff;
        public readonly bool airlOnOff;

        public readonly bool codeReaderTriggerIsM;
        public readonly bool loadFullMaterialOnOff;

        public RearPanelDrillPropertyHandler(ILogger<RearPanelDrillPropertyHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            slaveID = (byte)device.DeviceDescriptor.Extra["SlaveID"].ToInt();
            scanGunOnOff = device.DeviceDescriptor.Extra["ScanGunOnOff"].ToBool();
            airlOnOff = device.DeviceDescriptor.Extra["AirlOnOff"].ToBool();
            codeReaderTriggerIsM = device.DeviceDescriptor.Extra["CodeReaderTriggerIsM"].ToBool();
            loadFullMaterialOnOff = device.DeviceDescriptor.Extra["LoadFullMaterialOnOff"].ToBool();
        }

        public Dictionary<string, object?> DrillInformation()
        {
            var properties = new Dictionary<string, object>();
            try
            {
                if (InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus())
                {
                    properties["Drill_ConnectionStatus"] = true;

                    properties.Add("Drill_FrontMushroom", InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["FrontMushroomCloseLocalTionFlag"].ToInt()) == "1:1");
                    if (InteractingDevice.middleMushroomExist)
                    {
                        properties.Add("Drill_MiddleMushroom", InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["MiddleMushroomCloseLocalTionFlag"].ToInt()) == "1:1");
                    }
                    var cncStatus = InteractingDevice.cnc84Command.GetCncStatus();

                    var splineStatus = cncStatus?.SpindleStatus;
                    if (splineStatus != null)
                    {
                        properties.Add("Drill_State", cncStatus.Status);
                        splineStatus = splineStatus?.Substring(splineStatus.Length - InteractingDevice.spindleNum);
                        char[] arr = splineStatus.ToCharArray();
                        Array.Reverse(arr);
                        properties["Drill_SplineStatus"] = string.Join("", arr);
                    }
                    else
                    {
                        properties["Drill_SplineStatus"] = Enumerable.Repeat("1", DeviceDescriptor.SpindleNum);
                    }
                    if (cncStatus != null)
                    {
                        properties.Add("Drill_Percentage", cncStatus.FinPer);
                    }

                    var subProperties = DrillScreenText();
                    foreach (var item in subProperties)
                    {
                        properties.Add(item.Key, item.Value);
                    }

                    try
                    {
                        var programPath = InteractingDevice.cnc84Command.GetACTProgram();
                        var parameterPath = InteractingDevice.cnc84Command.GetDiaFileNameWithDialog();
                        var atpPath = InteractingDevice.cnc84Command.GetAtpFileName();
                        properties.Add("Drill_CurrentProgramFile", programPath);
                        properties.Add("Drill_CurrentParameterFile", parameterPath);
                        properties.Add("Drill_CurrentAtpFile", atpPath);
                        var opentime = InteractingDevice.cnc84Command.GetRuntimeValue("PC_HisBdeTimes(1262)");
                        var worktime = InteractingDevice.cnc84Command.GetRuntimeValue("PC_HisBdeTimes(1363)");
                        var waittime = InteractingDevice.cnc84Command.GetRuntimeValue("PC_HisBdeTimes(1464)");
                        var errortime = InteractingDevice.cnc84Command.GetRuntimeValue("PC_HisBdeTimes(1565)");
                        var duty = InteractingDevice.cnc84Command.GetRuntimeValue("PC_HisBdeData(1867)");

                        properties.Add("Drill_CurrentOpentime", opentime);
                        properties.Add("Drill_CurrentWorktime", worktime);
                        properties.Add("Drill_CurrentWaittime", waittime);
                        properties.Add("Drill_CurrentErrortime", errortime);
                        properties.Add("Drill_CurrentDuty", duty);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    properties["Drill_ConnectionStatus"] = false;
                    InteractingDevice.Engine.DeviceConnector.IsConnected = false;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return properties;
        }

        private bool initFlag = false;

        public Dictionary<string, object?> OtherInformation()
        {
            var properties = new Dictionary<string, object>();
            try
            {
                if (InteractingDevice.Engine.DeviceConnector.IsConnected)
                {
                    properties["Buffer_ConnectionStatus"] = true;
                    if (!initFlag)
                    {
                        initFlag = true;
                        InteractingDevice.modbusIpMaster.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["ScanGunOnOffWritePlc"].ToUshort(), !InteractingDevice.scanGunOnOff);
                        //InteractingDevice.modbusIpMaster.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["NoLoadMaterialOnOffWritePlc"].ToUshort(), InteractingDevice.noLoadMaterialOnOff);
                        InteractingDevice.modbusIpMaster.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["AirlOnOffWritePlc"].ToUshort(), airlOnOff);
                        InteractingDevice.modbusIpMaster.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["LoadFullMaterialOnOffWritePlc"].ToUshort(), loadFullMaterialOnOff);
                    }
                    //if (InteractingDevice.codeReaderOnOff)
                    //{
                    //    if (codeReaderTriggerIsM)
                    //    {
                    //        var codeReaderTrigger = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["CodeReaderTrigger"].ToUshort(), InteractingDevice.spindleNum.ToUshort());

                    //        properties.Add("Buffer_CodeReaderTrigger", string.Join("", codeReaderTrigger.Select(s=>s==true?1:0)));
                    //    }
                    //    else
                    //    {
                    //        var codeReaderTrigger = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["CodeReaderTriggerD"].ToUshort(), 1);
                    //        properties.Add("Buffer_CodeReaderTrigger", codeReaderTrigger);
                    //    }

                    //}

                    var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferReadyFlag"].ToUshort(), 2);
                    properties.Add("Buffer_IsReady", ready[0] == 1);
                    properties.Add("Buffer_EndLoadRawMaterial", ready[1] == 1);

                    var work = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferStatusFlag"].ToUshort(), 4);
                    properties.Add("Buffer_EnergizeStatus", work[0] == 1);
                    properties.Add("Buffer_Automatic", work[1] == 1);
                    properties.Add("Buffer_NoError", work[2] == 0);
                    properties.Add("Drill_Halt", work[3] == 1);

                    ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["ExistBoardOnDrill"].ToUshort(), 1);
                    //properties.Add("Drill_BoardPositionStatus", ready[0]);
                    properties.Add("Drill_ExistBoard", ready[0] != 0);

                    var actionFlag = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort(), 5);
                    properties.Add("Buffer_StartLoadRawMaterial", actionFlag[0] == 1);
                    properties.Add("Buffer_StartUnloadClinker", actionFlag[1] == 1);
                    properties.Add("Buffer_EndUnloadClinker", actionFlag[2] == 1);
                    properties.Add("Buffer_LoadAndUnloadEnd", actionFlag[3] == 1);
                    properties.Add("Buffer_LoadOrUnloadAxis", actionFlag[4]);

                    var bufferOnAgvPosition = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1);
                    properties.Add("Buffer_OnAgvPosition", bufferOnAgvPosition[0]);

                    InteractingDevice.existRawPanel = InteractingDevice.material[0] != 0;
                    //properties.Add("Buffer_RawMaterialLayerBoardStatus",InteractingDevice. material[0]);
                    //properties.Add("Buffer_ClinkerLayerBoardStatus", InteractingDevice.material[1]);

                    AssembleInteractionSequence(InteractingDevice.material);

                    properties.Add("SuggestPanelInteractionSequence", InteractingDevice.suggestInteractionSequence);

                    var boardLengthInfOnPlc = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra["BoardLengthInfOnPlc"].ToUshort(), 2);
                    properties.Add("Buffer_BoardLengthInfOnPlc", UshortArrayToFloat(boardLengthInfOnPlc));

                    InteractingDevice.loadMaterialVale = !InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["NoLoadMaterialOnOffWritePlc"].ToUshort(), 1)[0];
                }
                else
                {
                    InteractingDevice.suggestInteractionSequence = InteractionSequence.None;
                    properties["SuggestPanelInteractionSequence"] = InteractingDevice.suggestInteractionSequence;
                    properties["Buffer_ConnectionStatus"] = false;
                    InteractingDevice.Engine.DeviceConnector.IsConnected = false;
                }
            }
            catch (Exception e)
            {
                InteractingDevice.plcConnentFlag = false;
                InteractingDevice.suggestInteractionSequence = InteractionSequence.None;
                properties["SuggestPanelInteractionSequence"] = InteractingDevice.suggestInteractionSequence;
                logger.LogError(e, e.Message);
                InteractingDevice.Engine.DeviceConnector.IsConnected = false;
            }
            return properties;
        }

        private void AssembleInteractionSequence(ushort[] material)
        {
            int rawState = material[0];
            int clinkerState = material[1];
            int allRawCount = Convert.ToInt32(string.Join("", Enumerable.Repeat("1", InteractingDevice.spindleNum).ToArray()), 2);

            if (rawState == allRawCount) //生料满
            {
                if (clinkerState == 0)
                {
                    InteractingDevice.suggestInteractionSequence = InteractionSequence.None;
                }
                else
                {
                    InteractingDevice.suggestInteractionSequence = InteractionSequence.UnloadOnly;
                }
            }
            else //生料不满
            {
                if (clinkerState == 0)
                {
                    InteractingDevice.suggestInteractionSequence = InteractionSequence.LoadOnly;
                }
                else
                {
                    InteractingDevice.suggestInteractionSequence = InteractionSequence.LoadThenUnload;
                }
            }
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
                              .AddProperty("Buffer_RawMaterialLayerBoardStatus", "")
                              .AddProperty("Buffer_ClinkerLayerBoardStatus", 1)
                              .AddProperty("Buffer_ConnectionStatus", false)
                              .AddProperty("Buffer_RawMaterialLayerLoadEnd", false)
                              .AddProperty("Buffer_ClinkerLayerUnloadEnd", false)
                              .AddProperty("Buffer_EnergizeStatus", "0")
                              .AddProperty("Buffer_WarningCode", "0")
                              .AddProperty("Drill_WarningCode", "0")
                              .AddProperty("Buffer_Warning", false)
                              .AddProperty("Buffer_Automatic", true)
                              .AddProperty("Buffer_NoError", "0")
                              .AddProperty("Drill_NoError", "0")
                              .AddProperty("Drill_Halt", false)
                              .AddProperty("Buffer_UnLoadAndLoadEndFlag", false)
                              .AddProperty("Buffer_OnAgvPosition", true)
                              .AddProperty("Buffer_IsReady", true)
                              .AddProperty("Drill_ExistBoard", "0")
                              .AddProperty("Drill_DrillHoleEnd", "")
                              .AddProperty("Drill_DrillHoleStart", true)
                              .AddProperty("Drill_PinEnd", "0")
                              .AddProperty("Buffer_Position", "")
                              .AddProperty("Buffer_LowerPosition", false)
                              .AddProperty("Drill_CodeMessage", "")
                              .AddProperty("Drill_SplineStatus", string.Join("", Enumerable.Repeat("1", InteractingDevice.spindleNum).ToArray()))
                              .AddProperty("Drill_RealCodeMessage", "")
                              .AddProperty("Drill_PressBoardEnd", "0")
                              .AddProperty("Buffer_StartLoadRawMaterial", "0")
                              .AddProperty("Buffer_StartUnloadClinker", "0")
                              .AddProperty("Buffer_LoadOrUnloadAxis", "0")
                              .AddProperty("Buffer_EndLoadRawMaterial", "0")
                              .AddProperty("Buffer_EndUnloadClinker", "0")
                              .AddProperty("Buffer_DrillLockFlag", false)
                              .AddProperty("Buffer_BoardLengthInfOnPlc", DeviceDescriptor.Extra["DefalutBoardLength"].ToFloat())
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
                              .AddProperty("Buffer_AgvOnWorkBuffer", false)
                              .AddProperty("Buffer_CallAgvMessage", true)
                              .AddProperty("Buffer_CallAgvMessage_Reason", "")
                              .AddProperty("Buffer_LoadToDrillFlag", 0)
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
                              .AddProperty("Buffer_MoRunning", false)
                              .AddProperty("Buffer_AutoMoRunning", false)
                              .AddProperty("Drill_CurrentDuty", "")
                              .AddProperty("Drill_Percentage", "")
                              .AddProperty("Buffer_LoadEnd", "")
                              .AddProperty("Buffer_LoadAndUnload", "")
                              .AddProperty("Drill_State", "")
                              ;
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
