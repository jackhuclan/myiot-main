// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
    public class Rear95DrillPropertyHandler : DeviceShare<DefaultDrill>, IDrillPropertyHandler
    {
        private readonly ILogger<Rear95DrillPropertyHandler> logger;

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
        public readonly bool airlOnOff;
        public readonly bool codeReaderTriggerIsM;
        public readonly bool loadFullMaterialOnOff;
        public readonly bool firstCheckOnOff;

        public Rear95DrillPropertyHandler(ILogger<Rear95DrillPropertyHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            slaveID = (byte)device.DeviceDescriptor.Extra["SlaveID"].ToInt();
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

                    properties["Drill_ConnectionStatus"] = true;
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
                    properties.Add("Drill_State", cncStatus.Status);
                    var splineStatus = cncStatus?.SpindleStatus;
                    if (!DeviceDescriptor.Extra["IsDuo"].ToBool())
                    {
                        splineStatus = cncStatus?.SpindleStatusOriginalData;
                    }
                    if (splineStatus != null)
                    {
                        try
                        {
                            splineStatus = splineStatus?.Substring(splineStatus.Length - InteractingDevice.spindleNum);
                            char[] arr = splineStatus!.ToCharArray();
                            Array.Reverse(arr);

                            properties["Drill_SplineStatus"] = string.Join("", arr);
                        }
                        catch (Exception)
                        {

                        }

                    }
                    else
                    {
                        properties["Drill_SplineStatus"] = Enumerable.Repeat("1", DeviceDescriptor.SpindleNum);
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
                        catch (Exception)
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
                    //properties.Add("Drill_CollectCleanTime", DefaultDrill.CollectCleanTime.ToString("#0"));
                    //properties.Add("Drill_EndToStartTime", StaticEndToStartTime().ToString("#0"));
                    string statusPara = InteractingDevice.cnc84Command.ReadCncNode<String>("ns=4;s=UI/origin/DDETable/CncStatus");

                    var eventIds = statusPara.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    string? eventId = eventIds.FirstOrDefault(item => item.StartsWith("EC", StringComparison.OrdinalIgnoreCase));
                    properties.Add("Drill_EventId", eventId ?? "");
                    var progState = InteractingDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/WorkGroupSettings/WorkGroupInterface/TimeWidget/ProgramState");
                    properties.Add("Drill_ProgramState", progState);
                    logger.LogDebug($"Drill_ProgramState  {progState}");
                    var runState = InteractingDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/AutoData/AutoData.List/AutoData.RunState");
                    properties.Add("Drill_RunState", runState);
                    var runDrillHits = InteractingDevice.cnc84Command.ReadCncNode<UInt32>("ns=4;s=UI/origin/AutoData/AutoData.List/AutoData.RunDrillHits");

                    properties.Add("Drill_RunDrillHits", runDrillHits);
                    var runProductionDuration = InteractingDevice.cnc84Command.ReadCncNode<Double>("ns=4;s=UI/origin/AutoData/AutoData.List/AutoData.RunProductionDuration");
                    TimeSpan runProductionDurationTimeSpan = TimeSpan.FromMilliseconds(runProductionDuration);
                    string runProductionDurationValue = runProductionDurationTimeSpan.TotalMinutes.ToString("#0");
                    properties.Add("Drill_RunProductionDuration", runProductionDurationValue);

                    var runErrorDuration = InteractingDevice.cnc84Command.ReadCncNode<Double>("ns=4;s=UI/origin/AutoData/AutoData.List/AutoData.RunErrorDuration");
                    TimeSpan runErrorDurationTimeSpan = TimeSpan.FromMilliseconds(runErrorDuration);
                    string runErrorDurationValue = runErrorDurationTimeSpan.TotalMinutes.ToString("#0");
                    properties.Add("Drill_RunErrorDuration", runErrorDurationValue);
                }
                else
                {
                    properties["Drill_ConnectionStatus"] = false;
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
            try
            {
                if (InteractingDevice.Engine.DeviceConnector.IsConnected)
                {
                    properties.Add("Buffer_ConnectionStatus", true);
                    if (!initFlag)
                    {
                        initFlag = true;
                        InteractingDevice.modbusIpMaster.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["ScanGunOnOffWritePlc"].ToUshort(), !InteractingDevice.scanGunOnOff);
                        InteractingDevice.modbusIpMaster.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["AirlOnOffWritePlc"].ToUshort(), airlOnOff);
                        InteractingDevice.modbusIpMaster.WriteSingleCoil(slaveID, DeviceDescriptor.Extra["LoadFullMaterialOnOffWritePlc"].ToUshort(), loadFullMaterialOnOff);
                    }
                    //if (InteractingDevice.codeReaderOnOff)
                    //{
                    //    if (codeReaderTriggerIsM)
                    //    {
                    //        var codeReaderTrigger = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, DeviceDescriptor.Extra["CodeReaderTrigger"].ToUshort(), InteractingDevice.spindleNum.ToUshort());

                    //        properties.Add("Buffer_CodeReaderTrigger", string.Join("", codeReaderTrigger.Select(s => s == true ? 1 : 0)));
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
                    // properties.Add("Drill_BoardPositionStatus", ready[0]);
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
                    //properties.Add("Buffer_RawMaterialLayerBoardStatus", InteractingDevice.material[0]);
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
            return properties!;
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
                              .AddProperty("Buffer_ClinkerLayerBoardStatus", "1")
                              .AddProperty("Buffer_ConnectionStatus", false)
                              .AddProperty("Buffer_RawMaterialLayerLoadEnd", false)
                              .AddProperty("Buffer_ClinkerLayerUnloadEnd", false)
                              .AddProperty("Buffer_EnergizeStatus", "0")
                              .AddProperty("Buffer_WarningCode", "0")
                              .AddProperty("Drill_WarningCode", "0")
                              .AddProperty("Buffer_Warning", false)
                              .AddProperty("Buffer_Automatic", false)
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
                              .AddProperty("Buffer_MoRunning", false)
                              .AddProperty("Buffer_AutoMoRunning", false)
                              .AddProperty("ASplineIsWorkFullTable", "")
                              .AddProperty("Buffer_AllUnloadAndLoadEnd", "")

                              .AddProperty("Drill_ShowAllText", "")
                              .AddProperty("Drill_ShiftsStartTime", "")
                              .AddProperty("Drill_LocalTime", "")
                              .AddProperty("Drill_ShiftsTime", "")
                              .AddProperty("Drill_ToolLifeExpiredStartTime", "")
                              .AddProperty("Drill_ToolLifeExpiredEndTime", "")
                              .AddProperty("Drill_ChangeNoBoardTime", "")
                              .AddProperty("Drill_ChangeExistBoardTime", "")
                              .AddProperty("Drill_RunState", "")
                              .AddProperty("Drill_RunStartTime", "")
                              .AddProperty("Drill_RunEndTime", "")
                              .AddProperty("Buffer_RawChangeNoBoardTime", "")
                              .AddProperty("Buffer_RawChangeExistBoardTime", "")
                              .AddProperty("Buffer_AllUnloadAndLoadEndTime", "")
                              .AddProperty("Buffer_ClinkerChangeExistTime", "")
                              .AddProperty("Buffer_ClinkerChangeNoBoardTime", "")
                              .AddProperty("Drill_ErrorStartTime", "")
                              .AddProperty("Drill_ErrorEndTime", "")
                              .AddProperty("Drill_EventId", "")
                              .AddProperty("Drill_State", "")
                              //.AddProperty("Buffer_AutomaticStartTime", "")
                              .AddProperty("Buffer_ManualStartTime", "")
                              //.AddProperty("Buffer_AutomaticEndTime", "")
                              .AddProperty("Buffer_ManualEndTime", "")
                              .AddProperty("Drill_BoardDirectionStartTime", "")
                              .AddProperty("Drill_BoardDirectionEndTime", "")
                              .AddProperty("Drill_TestPinStartTime", "")
                              .AddProperty("Drill_TestPinEndTime", "")
                              .AddProperty("Drill_ToolEvaluationStartTime", "")
                              .AddProperty("Drill_ToolEvaluationEndTime", "")
                              .AddProperty("Drill_BoardPositionStatusStatistics", "-1")
                              .AddProperty("Buffer_ClinkerLayerBoardStatusStatistics", "-1")
                              .AddProperty("Drill_NoVacuumStartTime", "")
                              .AddProperty("Drill_NoVacuumEndTime", "")
                              .AddProperty("Drill_RunDrillHits", "")
                              .AddProperty("Drill_RunProductionDuration", "")
                              .AddProperty("Drill_RunErrorDuration", "")
                              .AddProperty("Buffer_LoadEnd", "")
                              .AddProperty("Buffer_LoadAndUnload", "")
                              .AddProperty("Drill_ProgramState", "")
                              .AddProperty("Buffer_PreControlMushroom", 0)
                              .AddProperty("Drill_LoadFileFromCncUI", false)
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
                screenProperties["Drill_NoError"] = false;
            else
                screenProperties["Drill_NoError"] = true;

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
