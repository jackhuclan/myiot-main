// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Opc.Ua;
using Opc.Ua.Server;
using Quartz.Util;

namespace SharpNodeSettings.OpcUaServer;

/// <summary>
/// A node manager for a server that exposes several variables.
/// </summary>
public partial class EmptyNodeManager : CustomNodeManager2
{
    private readonly List<NodeDescription> drilList = new List<NodeDescription>();
    public readonly List<NodeDescription> extradrilList = new List<NodeDescription>();
    private readonly List<NodeDescription> drilExceptionList = new List<NodeDescription>();
    public Dictionary<string, BaseDataVariableState> VgBaseDataVariableState = new Dictionary<string, BaseDataVariableState>();    // 节点管理器
    public string NotUseEqpName = string.Empty;


    public void ChangeNodeValue(string key, object value)
    {
        if (VgBaseDataVariableState.ContainsKey(key))
        {
            BaseDataVariableState baseDataVariableState = VgBaseDataVariableState[key];
            baseDataVariableState.Value = value;
            baseDataVariableState.ClearChangeMasks(SystemContext, false);
        }
    }

    public object GetNodeValue(string key)
    {
        if (VgBaseDataVariableState.ContainsKey(key))
        {
            BaseDataVariableState baseDataVariableState = VgBaseDataVariableState[key];
            return baseDataVariableState.Value;
        }
        return null;
    }

    /// <summary>
    /// Does any initialization required before the address space can be used.
    /// </summary>
    /// <remarks>
    /// The externalReferences is an out parameter that allows the node manager to link to nodes
    /// in other node managers. For example, the 'Objects' node is managed by the CoreNodeManager and
    /// should have a reference to the root folder node(s) exposed by this node manager.
    /// </remarks>
    public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
    {
        InitDrilNode();
        lock (Lock)
        {
            LoadPredefinedNodes(SystemContext, externalReferences);

            IList<IReference> references = null;

            if (!externalReferences.TryGetValue(ObjectIds.ObjectsFolder, out references))
            {
                externalReferences[ObjectIds.ObjectsFolder] = references = new List<IReference>();
            }
            CreateMachineNode("Vega Drill Machines", references, 1, "苏州维嘉科技股份有限公司");
        }
    }

    /// <summary>
    /// 根据
    /// </summary>
    /// <param name="rootName"></param>
    /// <param name="references"></param>
    /// <param name="type"></param>
    /// <param name="Description"></param>
    private void CreateMachineNode(string rootName, IList<IReference> references, int type, string Description)
    {
        //创建根节点【Vega Drill Machines】
        FolderState rootMy = CreateFolder(null, rootName);
        rootMy.AddReference(ReferenceTypes.Organizes, true, ObjectIds.ObjectsFolder);
        references.Add(new NodeStateReference(ReferenceTypes.Organizes, false, rootMy.NodeId));
        rootMy.EventNotifier = EventNotifiers.SubscribeToEvents;
        AddRootNotifier(rootMy);
        //创建设备名称节点【Config.EquipmentID】
        FolderState myFolder;

        if (NotUseEqpName.IsNullOrWhiteSpace())
        {
            myFolder = CreateFolder(rootMy, _eAPClientOptions.EquipmentID);
        }
        else
        {
            myFolder = CreateFolder(rootMy, NotUseEqpName);
        }

        //添加Config.EquipmentID子节点
        CreateDrilSubNode(myFolder);

        AddPredefinedNode(SystemContext, rootMy);
    }

    //根据轴数添加  测量结果
    private void CreateDrilSubNode(FolderState myFolder)
    {
        //添加备名称节点Config.EquipmentID子节点
        var drilList2 = drilList.OrderBy(o => o.NodeName).ToList();
        var drilExceptionList2 = drilExceptionList.OrderBy(o => o.NodeName).ToList();
        foreach (NodeDescription nd in drilList2)
        {
            CreateVariable(myFolder, nd.NodeName, nd.NodeType, nd.ValueRank, nd.NodeValue).Description = nd.NodeDes;
        }
        //把异常单拎出来2024-03-15
        //添加备名称节点Config.EquipmentID子节点【Alarms】
        FolderState exceptFolder = CreateFolder(myFolder, "Alarms");
        foreach (NodeDescription nd in drilExceptionList2)
        {
            CreateVariable(exceptFolder, nd.NodeName, nd.NodeType, nd.ValueRank, nd.NodeValue).Description = nd.NodeDes;
        }
    }

    private BaseDataVariableState<T> CreateVariable<T>(NodeState parent, string name, NodeId dataType, int valueRank, T defaultValue)
    {
        BaseDataVariableState<T> variable = new BaseDataVariableState<T>(parent);

        variable.SymbolicName = name;
        variable.ReferenceTypeId = ReferenceTypes.Organizes;
        variable.TypeDefinitionId = VariableTypeIds.BaseDataVariableType;
        if (parent == null)
        {
            variable.NodeId = new NodeId(name, NamespaceIndex);
        }
        else
        {
            variable.NodeId = new NodeId(parent.NodeId.ToString() + "/" + name);
        }
        variable.BrowseName = new QualifiedName(name, NamespaceIndex);
        variable.DisplayName = new LocalizedText(name);
        variable.WriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description;
        variable.UserWriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description;
        variable.DataType = dataType;
        variable.ValueRank = valueRank;
        variable.AccessLevel = AccessLevels.CurrentReadOrWrite;
        variable.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
        variable.Historizing = false;
        variable.Value = defaultValue;
        variable.StatusCode = StatusCodes.Good;
        variable.Timestamp = DateTime.Now;
        if (parent != null)
        {
            parent.AddChild(variable);
        }
        VgBaseDataVariableState.Add(variable.NodeId.ToString(), variable);
        return variable;
    }

    private void InitDrilNode()
    {
        drilList.Clear();
        drilList.Add(new NodeDescription("MachineName", DataTypeIds.String, "机台编号", _eAPClientOptions.EquipmentID));
        drilList.Add(new NodeDescription("OpcUaVersion", DataTypeIds.String, "版本号", Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? Version.Parse("1.0.0.0").ToString()));
        drilList.Add(new NodeDescription("CurrentTime", DataTypeIds.String, "电脑时间", ""));
        drilList.Add(new NodeDescription("Heartbeat", DataTypeIds.String, "心跳状态", ""));

        // 2025/4/28
        //实时采集
        drilList.Add(new NodeDescription("Status", DataTypeIds.String, "机器状态", ""));
        drilList.Add(new NodeDescription("ScreenText", DataTypeIds.String, "屏幕显示", ""));
        drilList.Add(new NodeDescription("ProgramName", DataTypeIds.String, "程序名", ""));
        drilList.Add(new NodeDescription("SpindleStatus", DataTypeIds.String, "轴状态", ""));
        drilList.Add(new NodeDescription("PgmRunStartTime", DataTypeIds.String, "程序开始时间", ""));
        drilList.Add(new NodeDescription("PgmRunEndTime", DataTypeIds.String, "程序结束时间", ""));
        drilList.Add(new NodeDescription("PgmRunTotalTime", DataTypeIds.String, "程序运行时间", ""));

        //异常采集(实时)
        drilList.Add(new NodeDescription("ExceptCode", DataTypeIds.String, "异常编号", 0));
        drilList.Add(new NodeDescription("ExceptMessage", DataTypeIds.String, "异常信息", ""));
        drilList.Add(new NodeDescription("ExceptStartTime", DataTypeIds.String, "异常开始时间", ""));
        drilList.Add(new NodeDescription("ExceptEndTime", DataTypeIds.String, "异常结束时间", ""));
        drilList.Add(new NodeDescription("ExceptUsingTime", DataTypeIds.String, "异常持续时间", ""));
        //drilList.Add(new NodeDescription("ProgramName", DataTypeIds.String, "程序名", ""));

        drilList.Add(new NodeDescription("BrokenInfo", DataTypeIds.String, "断刀信息", ""));
        drilList.Add(new NodeDescription("BrokenToolId", DataTypeIds.String, "断刀刀号", ""));
        drilList.Add(new NodeDescription("BrokenDia", DataTypeIds.String, "断刀刀径", ""));
        drilList.Add(new NodeDescription("BrokenSpindleId", DataTypeIds.String, "断刀的轴", ""));
        drilList.Add(new NodeDescription("BrokenTime", DataTypeIds.String, "断刀时间", ""));

        drilList.Add(new NodeDescription("CncRunProgress", DataTypeIds.String, "工作进度比", ""));

        //定时采集
        drilList.Add(new NodeDescription("CurPgmFilePath", DataTypeIds.String, "当前drl程序名", ""));
        drilList.Add(new NodeDescription("CurDiaFilePath", DataTypeIds.String, "当前dia程序名", ""));
        drilList.Add(new NodeDescription("CurAtpFilePath", DataTypeIds.String, "当前atp程序名", ""));
        drilList.Add(new NodeDescription("TT", DataTypeIds.String, "刀号", ""));
        drilList.Add(new NodeDescription("TS", DataTypeIds.String, "转速", ""));
        drilList.Add(new NodeDescription("TF", DataTypeIds.String, "进刀速", ""));
        drilList.Add(new NodeDescription("TR", DataTypeIds.String, "退刀速", ""));
        drilList.Add(new NodeDescription("TD", DataTypeIds.String, "刀径", ""));
        drilList.Add(new NodeDescription("TN", DataTypeIds.String, "刀寿命", ""));
        drilList.Add(new NodeDescription("TB", DataTypeIds.String, "刀使用寿命", ""));
        drilList.Add(new NodeDescription("Duty", DataTypeIds.String, "当班稼动率", ""));
        drilList.Add(new NodeDescription("PreDuty", DataTypeIds.String, "上一班次稼动率", ""));
        drilList.Add(new NodeDescription("OPID", DataTypeIds.String, "OPID", ""));
        drilList.Add(new NodeDescription("XYPosition", DataTypeIds.String, "XYPosition", ""));
        drilList.Add(new NodeDescription("ShiftErrorTime", DataTypeIds.String, "班次异常时间", ""));
        drilList.Add(new NodeDescription("ShiftOnlineTime", DataTypeIds.String, "班次已开机时间", ""));
        drilList.Add(new NodeDescription("ShiftWorkingTime", DataTypeIds.String, "班次运行时间", ""));
        drilList.Add(new NodeDescription("ShiftWaitingTime", DataTypeIds.String, "班次待机时间", ""));
        drilList.Add(new NodeDescription("SpindleCount", DataTypeIds.String, "设备轴数", 0));//
        drilList.Add(new NodeDescription("TotalDrillOrRout", DataTypeIds.String, "总孔数/锣程", ""));
        drilList.Add(new NodeDescription("CurDrillOrRout", DataTypeIds.String, "已钻孔数/锣程", ""));
        drilList.Add(new NodeDescription("Block", DataTypeIds.String, "Block", ""));
        drilList.Add(new NodeDescription("Step", DataTypeIds.String, "Step", ""));

        //新增涨缩
        drilList.Add(new NodeDescription("SAX", DataTypeIds.String, "X轴涨缩", ""));
        drilList.Add(new NodeDescription("SAY", DataTypeIds.String, "Y轴涨缩", ""));
        drilList.Add(new NodeDescription("SAZX", DataTypeIds.String, "涨缩中心点X", ""));
        drilList.Add(new NodeDescription("SAZY", DataTypeIds.String, "涨缩中心点Y", ""));
        drilList.Add(new NodeDescription("FinTime", DataTypeIds.String, "当前程式运行时间", ""));
        drilList.Add(new NodeDescription("UserName", DataTypeIds.String, "用户名", ""));
        drilList.Add(new NodeDescription("UserLevel", DataTypeIds.String, "用户级别", ""));
        drilList.Add(new NodeDescription("H", DataTypeIds.String, "Z轴的绝对行进平面", ""));
        drilList.Add(new NodeDescription("Q", DataTypeIds.String, "Z轴行进平面的相对值", ""));
        drilList.Add(new NodeDescription("K", DataTypeIds.String, "Z轴工作平面的相对值", ""));
        drilList.Add(new NodeDescription("Z", DataTypeIds.String, "Z轴的工作平面绝对值", ""));

        //刀测量数据
        drilList.Add(new NodeDescription("DiameterTolChecked", DataTypeIds.String, "直径校验开关", ""));
        drilList.Add(new NodeDescription("DiameterTolNeg", DataTypeIds.String, "直径正偏差设定值", ""));
        drilList.Add(new NodeDescription("DiameterTolPos", DataTypeIds.String, "直径负偏差设定值", ""));
        drilList.Add(new NodeDescription("LengthTolChecked", DataTypeIds.String, "长度偏差校验开关", ""));
        drilList.Add(new NodeDescription("LengthTolNeg", DataTypeIds.String, "长度正偏差设定值", ""));
        drilList.Add(new NodeDescription("LengthTolPos", DataTypeIds.String, "长度负偏差设定值", ""));
        drilList.Add(new NodeDescription("RunoutTolChecked", DataTypeIds.String, "偏摆校验开关", ""));
        drilList.Add(new NodeDescription("RunoutTolNeg", DataTypeIds.String, "偏摆报警值", ""));
        drilList.Add(new NodeDescription("RunoutTolPos", DataTypeIds.String, "偏摆停止值", ""));

        drilList.Add(new NodeDescription("MeasureDDia", DataTypeIds.String, "测刀异常刀径", ""));
        drilList.Add(new NodeDescription("MeasureDTool", DataTypeIds.String, "测刀异常刀号", ""));
        drilList.Add(new NodeDescription("MeasureDSplindle", DataTypeIds.String, "测刀异常轴号", ""));

        for (int i = 0; i < _eAPClientOptions.SpindleCount; i++)
        {
            drilList.Add(new NodeDescription("SpindleEnable" + (i + 1), DataTypeIds.String, "轴状态" + (i + 1), ""));
            drilList.Add(new NodeDescription("TMeasureDia" + (i + 1), DataTypeIds.String, "测刀刀径" + (i + 1), ""));
            drilList.Add(new NodeDescription("TMeasureLen" + (i + 1), DataTypeIds.String, "测刀刀长" + (i + 1), ""));
            drilList.Add(new NodeDescription("TRunout" + (i + 1), DataTypeIds.String, "测刀偏摆" + (i + 1), ""));
            drilList.Add(new NodeDescription("SpindleWorkTimes" + (i + 1), DataTypeIds.String, "轴" + (i + 1) + "运行时间(秒)", ""));
            drilList.Add(new NodeDescription("SpindleWorkTimeStr" + (i + 1), DataTypeIds.String, "轴" + (i + 1) + "运行时间字符串", ""));
        }

        //写  铝片二维码
        drilList.Add(new NodeDescription("WMode", DataTypeIds.Int32, "模式", 0));
        drilList.Add(new NodeDescription("WDiaFileLocation", DataTypeIds.String, "刀直径文件路径", ""));
        drilList.Add(new NodeDescription("WAtpFileLocation", DataTypeIds.String, "刀具文件路径", ""));
        drilList.Add(new NodeDescription("WPgmFileLocation", DataTypeIds.String, "程序文件路径", ""));
        drilList.Add(new NodeDescription("WProgramFileLocation", DataTypeIds.String, "程序文件路径(已弃用)", ""));
        drilList.Add(new NodeDescription("WResult", DataTypeIds.String, "文件加载结果", ""));
        //drilList.Add(new NodeDescription("WLoadDrilSuccess", DataTypeIds.Int32, "加载程式成功", 0));
        //drilList.Add(new NodeDescription("WLoadPinSuccess", DataTypeIds.Int32, "加载PIN成功", 0));
        drilList.Add(new NodeDescription("WOptName", DataTypeIds.String, "批号", ""));
        drilList.Add(new NodeDescription("WFileName", DataTypeIds.String, "料号", ""));

        //额外新增点位
        drilList.AddRange(extradrilList);

        #region 系统自带

        /*drilList.Add(new NodeDescription("Version", DataTypeIds.String, "Version", ""));
        drilList.Add(new NodeDescription("DNCM", DataTypeIds.String, "DNCM", ""));
        drilList.Add(new NodeDescription("DateTime", DataTypeIds.String, "DateTime", ""));
        drilList.Add(new NodeDescription("SysStatus", DataTypeIds.String, "SysStatus", ""));
        drilList.Add(new NodeDescription("CommStatus", DataTypeIds.String, "CommStatus", ""));
        drilList.Add(new NodeDescription("PcStatus", DataTypeIds.String, "PcStatus", ""));
        drilList.Add(new NodeDescription("CNCError", DataTypeIds.String, "CNCError", ""));
        drilList.Add(new NodeDescription("CncStatus", DataTypeIds.String, "CncStatus", ""));
        drilList.Add(new NodeDescription("CncTools", DataTypeIds.String, "CncTools", ""));
        drilList.Add(new NodeDescription("Duty", DataTypeIds.String, "Duty", ""));
        drilList.Add(new NodeDescription("XYPosition", DataTypeIds.String, "XYPosition", ""));
        drilList.Add(new NodeDescription("ScreenSaver", DataTypeIds.String, "ScreenSaver", ""));
        drilList.Add(new NodeDescription("UserName", DataTypeIds.String, "UserName", ""));
        drilList.Add(new NodeDescription("UserLevel", DataTypeIds.String, "UserLevel", ""));
        drilList.Add(new NodeDescription("OPID", DataTypeIds.String, "OPID", ""));
        drilList.Add(new NodeDescription("ActProgram", DataTypeIds.String, "ActProgram", ""));
        drilList.Add(new NodeDescription("NextProgram", DataTypeIds.String, "NextProgram", ""));
        drilList.Add(new NodeDescription("Batch", DataTypeIds.String, "Batch", ""));
        drilList.Add(new NodeDescription("InFlags", DataTypeIds.String, "InFlags", ""));
        drilList.Add(new NodeDescription("OutFlags", DataTypeIds.String, "OutFlags", ""));
        drilList.Add(new NodeDescription("DDESequence", DataTypeIds.String, "DDESequence", ""));
        drilList.Add(new NodeDescription("DDEPC", DataTypeIds.String, "DDEPC", ""));
        drilList.Add(new NodeDescription("FTasten", DataTypeIds.String, "FTasten", ""));*/

        #endregion 系统自带

        //增加的io点
        /*drilList.Add(new NodeDescription("EmergencyStop", DataTypeIds.String, "紧急停止", "0"));
        drilList.Add(new NodeDescription("TableStopExcept", DataTypeIds.String, "桌面停止", "0"));
        drilList.Add(new NodeDescription("PositionStopExcept", DataTypeIds.String, "位置停止", "0"));
        drilList.Add(new NodeDescription("SuctionFilterExcept", DataTypeIds.String, "吸滤装置", "0"));
        drilList.Add(new NodeDescription("CwcTemperatureExcept", DataTypeIds.String, "冷水机水温异常", "0"));
        drilList.Add(new NodeDescription("AirDryerExcept", DataTypeIds.String, "干燥机", "0"));
        drilList.Add(new NodeDescription("SpindleProtectExcept", DataTypeIds.String, "主轴保护", "0"));
        drilList.Add(new NodeDescription("SpindleAirExcept", DataTypeIds.String, "主轴气压", "0"));
        drilList.Add(new NodeDescription("LightBarrierExcept", DataTypeIds.String, "光电栅栏", "0"));
        drilList.Add(new NodeDescription("WaterTemperatureExcept", DataTypeIds.String, "水温异常", "0"));
        drilList.Add(new NodeDescription("WaterFluxCheckExcept", DataTypeIds.String, "水流量异常", "0"));*/
        // 其他点位

        //InitExcept();
    }

    public void InitExcept()
    {
        drilExceptionList.Clear();
        drilExceptionList.Add(new NodeDescription("XYAxesReachesTheTableLimits", DataTypeIds.Int32, "其中一个XY轴达到工作台极限", 0));
        drilExceptionList.Add(new NodeDescription("NumberOfMagazinesInsufficient", DataTypeIds.Int32, "数量不足", 0));
        drilExceptionList.Add(new NodeDescription("ExecutedProgramIncludesM20M06", DataTypeIds.Int32, "执行的部件程序包含M20或M06", 0));
        drilExceptionList.Add(new NodeDescription("ToolLifeExcept", DataTypeIds.Int32, "刀具寿命", 0));
        drilExceptionList.Add(new NodeDescription("SequenceError", DataTypeIds.Int32, "Sequence错误", 0));
        drilExceptionList.Add(new NodeDescription("FrequencyConverterError", DataTypeIds.Int32, "变频器错误", 0));
        drilExceptionList.Add(new NodeDescription("ExecutedProgramIncludesM47", DataTypeIds.Int32, "执行的部件程序包含M47", 0));
        drilExceptionList.Add(new NodeDescription("TheSignalSFUSTPMAActive", DataTypeIds.Int32, "信号SFUSTPMA激活", 0));
        drilExceptionList.Add(new NodeDescription("ExternalDeviceCommunicationError", DataTypeIds.Int32, "与外部设备通信异常", 0));
        drilExceptionList.Add(new NodeDescription("DetectedUndefinedStoppingInput", DataTypeIds.Int32, "CNC检测到未定义的停止输入", 0));
        drilExceptionList.Add(new NodeDescription("StoppingInput", DataTypeIds.Int32, "停止输入", 0));
        drilExceptionList.Add(new NodeDescription("StepByStepExecution", DataTypeIds.Int32, "逐步执行", 0));
        drilExceptionList.Add(new NodeDescription("ToolChangeError", DataTypeIds.Int32, "刀具更换期间发生错误", 0));
        drilExceptionList.Add(new NodeDescription("WorkingAreaExceededExcept", DataTypeIds.Int32, "超出操作范围", 0));
        drilExceptionList.Add(new NodeDescription("ToolNotFound", DataTypeIds.Int32, "找不到刀具", 0));
        drilExceptionList.Add(new NodeDescription("EndOfProgram", DataTypeIds.Int32, "程序结束", 0));
        drilExceptionList.Add(new NodeDescription("UnknownCommand", DataTypeIds.Int32, "未知命令", 0));
        drilExceptionList.Add(new NodeDescription("IncorrectToolInSpindle", DataTypeIds.Int32, "错误的刀具在主轴上", 0));
        drilExceptionList.Add(new NodeDescription("ProgramError", DataTypeIds.Int32, "程序错误", 0));
        drilExceptionList.Add(new NodeDescription("ProgramAborted", DataTypeIds.Int32, "程序中断", 0));
        drilExceptionList.Add(new NodeDescription("InsufficientTools", DataTypeIds.Int32, "刀具不足", 0));

        drilExceptionList.Add(new NodeDescription("GripperNotUp", DataTypeIds.Int32, "机械手未升起", 0));
        drilExceptionList.Add(new NodeDescription("GripperNotDown", DataTypeIds.Int32, "机械手未降下", 0));
        drilExceptionList.Add(new NodeDescription("GripperStillUp", DataTypeIds.Int32, "机械手还处于升起状态", 0));
        drilExceptionList.Add(new NodeDescription("GripperStillDown", DataTypeIds.Int32, "机械手还处于降下状态", 0));
        drilExceptionList.Add(new NodeDescription("TableEnable", DataTypeIds.Int32, "气夹使能", 0));

        drilExceptionList.Add(new NodeDescription("LaserIncorString", DataTypeIds.Int32, "镭射卡连接错误", 0));
        drilExceptionList.Add(new NodeDescription("LaserTableFull", DataTypeIds.Int32, "镭射卡记忆体满", 0));
        drilExceptionList.Add(new NodeDescription("LaserTimeout", DataTypeIds.Int32, "镭射卡超时", 0));
        drilExceptionList.Add(new NodeDescription("LaserCascadingError", DataTypeIds.Int32, "刀检卡通讯错误", 0));
        drilExceptionList.Add(new NodeDescription("LaserToolNMeasured", DataTypeIds.Int32, "镭射，刀具N.量测", 0));

        drilExceptionList.Add(new NodeDescription("LaserStartError", DataTypeIds.Int32, "镭射卡启动错误", 0));
        drilExceptionList.Add(new NodeDescription("LaserTransmissionError", DataTypeIds.Int32, "镭射卡内部传输错误", 0));
        drilExceptionList.Add(new NodeDescription("LaserNoResult", DataTypeIds.Int32, "镭射卡无结果", 0));
        drilExceptionList.Add(new NodeDescription("InvalidDiameter", DataTypeIds.Int32, "无效直径或者无轴位移信号", 0));
        drilExceptionList.Add(new NodeDescription("DiameterError", DataTypeIds.Int32, "刀具直径错误T", 0));

        drilExceptionList.Add(new NodeDescription("RunoutError", DataTypeIds.Int32, "偏摆错误T", 0));
        drilExceptionList.Add(new NodeDescription("RunoutWarning", DataTypeIds.Int32, "偏摆报警T", 0));
        drilExceptionList.Add(new NodeDescription("ToolLong", DataTypeIds.Int32, "刀太长", 0));
        drilExceptionList.Add(new NodeDescription("ToolInSpindle", DataTypeIds.Int32, "刀还在主轴里", 0));
        drilExceptionList.Add(new NodeDescription("LostTool", DataTypeIds.Int32, "刀具丢失_请检查清除刀库", 0));

        drilExceptionList.Add(new NodeDescription("NoValidTool", DataTypeIds.Int32, "刀具无效 T=", 0));
        drilExceptionList.Add(new NodeDescription("CheckTool", DataTypeIds.Int32, "检查刀具T-", 0));
        drilExceptionList.Add(new NodeDescription("CheckZeroErrorTooBig", DataTypeIds.Int32, "校对零点误差过大", 0));
        drilExceptionList.Add(new NodeDescription("CheckZeroUnknownError", DataTypeIds.Int32, "校对零点未知错误", 0));
        drilExceptionList.Add(new NodeDescription("XYScaleError", DataTypeIds.Int32, "X,Y光栅尺错误", 0));

        drilExceptionList.Add(new NodeDescription("XScaleError", DataTypeIds.Int32, "X光栅尺错误", 0));
        drilExceptionList.Add(new NodeDescription("YScaleError", DataTypeIds.Int32, "Y光栅尺错误", 0));
        drilExceptionList.Add(new NodeDescription("XZeroControl", DataTypeIds.Int32, "X零点控制", 0));
        drilExceptionList.Add(new NodeDescription("YZeroControl", DataTypeIds.Int32, "Y零点控制", 0));
        drilExceptionList.Add(new NodeDescription("LenkOrDeptIsOn", DataTypeIds.Int32, "LENK或者DEPT功能开启", 0));

        drilExceptionList.Add(new NodeDescription("WrongPassword", DataTypeIds.Int32, "运行密码", 0));
        drilExceptionList.Add(new NodeDescription("AutoCalibrate", DataTypeIds.Int32, "复位", 0));
        drilExceptionList.Add(new NodeDescription("Buffer1NotEmpty", DataTypeIds.Int32, "刀座1有刀，请取出", 0));
        drilExceptionList.Add(new NodeDescription("Buffer2NotEmpty", DataTypeIds.Int32, "刀座2有刀，请取出", 0));
        drilExceptionList.Add(new NodeDescription("Buffer1IsEmpty", DataTypeIds.Int32, "刀座1无刀", 0));

        drilExceptionList.Add(new NodeDescription("Buffer2IsEmpty", DataTypeIds.Int32, "刀座2无刀", 0));
        drilExceptionList.Add(new NodeDescription("ContactError", DataTypeIds.Int32, "接触钻错误", 0));
        drilExceptionList.Add(new NodeDescription("SplidleNumber", DataTypeIds.Int32, "主轴错误 主轴号=", 0));
        drilExceptionList.Add(new NodeDescription("ToolLengthErrorByTolc", DataTypeIds.Int32, "刀具长度超出TOLC", 0));
        drilExceptionList.Add(new NodeDescription("ToolLengthErrorByTolerance", DataTypeIds.Int32, "刀具长度超出刀具表公差T", 0));

        drilExceptionList.Add(new NodeDescription("CanNotAdjustToolLength", DataTypeIds.Int32, "刀具长度不在自动调整范围内", 0));
        drilExceptionList.Add(new NodeDescription("SutoBrokenTool", DataTypeIds.Int32, "SUTO 断刀", 0));
        drilExceptionList.Add(new NodeDescription("BrokenTool", DataTypeIds.Int32, "断刀T", 0));
        drilExceptionList.Add(new NodeDescription("ReplaceTool", DataTypeIds.Int32, "更换刀具T", 0));
        drilExceptionList.Add(new NodeDescription("KLevelReachedZ", DataTypeIds.Int32, "K平面达到Z:", 0));

        drilExceptionList.Add(new NodeDescription("KLevelUnderZ", DataTypeIds.Int32, "K平面小于Z", 0));
        drilExceptionList.Add(new NodeDescription("SutoNoSurfaceDetected", DataTypeIds.Int32, "SUTO,未检测到板面", 0));
        drilExceptionList.Add(new NodeDescription("SutoSurfaceTooEarly", DataTypeIds.Int32, "SUTO板面接触过早", 0));
        drilExceptionList.Add(new NodeDescription("SplineMore4OutOfWindow", DataTypeIds.Int32, "轴 4次超出窗口精度", 0));
        drilExceptionList.Add(new NodeDescription("TotoOutOfTolerance", DataTypeIds.Int32, "TOTO超出公差", 0));

        drilExceptionList.Add(new NodeDescription("KOverflowMax", DataTypeIds.Int32, "K值超深度钻孔极限值", 0));
        drilExceptionList.Add(new NodeDescription("CbdNoContact", DataTypeIds.Int32, "CBD没有接触钻信号", 0));
        drilExceptionList.Add(new NodeDescription("UnKnownContactError", DataTypeIds.Int32, "未知接触钻错误", 0));
        drilExceptionList.Add(new NodeDescription("PressureFootTouchesOutOfTolerancce", DataTypeIds.Int32, "压力脚接触超公差", 0));
        drilExceptionList.Add(new NodeDescription("ZSPD", DataTypeIds.Int32, "ZSPD,Z 接触钻信号早于XY到位前", 0));

        drilExceptionList.Add(new NodeDescription("NoInversionPointInPeckDrilling", DataTypeIds.Int32, "分段钻时没有翻转点", 0));
        drilExceptionList.Add(new NodeDescription("SutoReachedWithoutContanct", DataTypeIds.Int32, "Z到达中途无接触信号", 0));
        drilExceptionList.Add(new NodeDescription("SutoFirstReachedWithoutContanct", DataTypeIds.Int32, "Z到达第一刀无接触信号", 0));
        drilExceptionList.Add(new NodeDescription("SutoAlternativeSufaceBrokenTool", DataTypeIds.Int32, "SUTO，深度钻孔时板面无接触信号或刀具已断", 0));
        drilExceptionList.Add(new NodeDescription("MinimumSurfaceExceeded", DataTypeIds.Int32, "S最小平面超过Z", 0));

        drilExceptionList.Add(new NodeDescription("DrillDestinationIsBelowTheTablePlance", DataTypeIds.Int32, "目标值达高于当前值", 0));
        drilExceptionList.Add(new NodeDescription("OpticalBrokenDrillDetectionHasBeenActivated", DataTypeIds.Int32, "断刀检测传感器已经到达", 0));
        drilExceptionList.Add(new NodeDescription("DestinationHigherThanCurentPosition", DataTypeIds.Int32, "目标值达高于当前值", 0));
        drilExceptionList.Add(new NodeDescription("SurfaceContactBeforeStartOfDrillStroke", DataTypeIds.Int32, "钻孔之前已有接触钻信号", 0));
        drilExceptionList.Add(new NodeDescription("SwitchOnPreasureFootDoesNotWork", DataTypeIds.Int32, "压力脚开关不工作", 0));

        drilExceptionList.Add(new NodeDescription("MinimumAccelerationTooSmall", DataTypeIds.Int32, "最小加速度过小", 0));
        drilExceptionList.Add(new NodeDescription("KValueNotReachableZ", DataTypeIds.Int32, "K值无法达到Z", 0));
        drilExceptionList.Add(new NodeDescription("KValueHasNotBeenReached", DataTypeIds.Int32, "K值没有达到", 0));
        drilExceptionList.Add(new NodeDescription("ToolIsNotMeasured", DataTypeIds.Int32, "没有量刀", 0));
        drilExceptionList.Add(new NodeDescription("DistanceDrillTipToolSmall", DataTypeIds.Int32, "压力脚-刀尖距离太小", 0));

        drilExceptionList.Add(new NodeDescription("TdifBrokenTool", DataTypeIds.Int32, "TDIF断刀", 0));
        drilExceptionList.Add(new NodeDescription("SutoTdifBrokenTool", DataTypeIds.Int32, "SUTO，深度钻孔断刀", 0));
        drilExceptionList.Add(new NodeDescription("TdifTooLong", DataTypeIds.Int32, "TDIF刀具太长", 0));
        drilExceptionList.Add(new NodeDescription("TotoToolTooLong", DataTypeIds.Int32, "TOTO，接触钻太长", 0));
        drilExceptionList.Add(new NodeDescription("SutoToolTooLong", DataTypeIds.Int32, "SUTO，深度钻刀具太长", 0));

        drilExceptionList.Add(new NodeDescription("PFMustDownBeforeQicMove", DataTypeIds.Int32, "压脚切换时吸屑罩必须放下", 0));
        drilExceptionList.Add(new NodeDescription("QICNotOpen", DataTypeIds.Int32, "QIC没有打开", 0));
        drilExceptionList.Add(new NodeDescription("QICNotClosed", DataTypeIds.Int32, "QIC没有关闭", 0));
        drilExceptionList.Add(new NodeDescription("NoToolInSpindle", DataTypeIds.Int32, "主轴内无刀", 0));
        drilExceptionList.Add(new NodeDescription("ToolIsInJoker", DataTypeIds.Int32, "刀具在中转刀座中", 0));

        drilExceptionList.Add(new NodeDescription("TakeToolFromJoker", DataTypeIds.Int32, "从中转刀库中取刀", 0));
        drilExceptionList.Add(new NodeDescription("CleanCollentManualTool", DataTypeIds.Int32, "夹头清洗（手动）", 0));
        drilExceptionList.Add(new NodeDescription("VacuumNotOpen", DataTypeIds.Int32, "吸尘总阀没有打开", 0));
        drilExceptionList.Add(new NodeDescription("VacuumNotClosed", DataTypeIds.Int32, "吸尘总阀没有关闭", 0));
        drilExceptionList.Add(new NodeDescription("UnKnownMeasSysError", DataTypeIds.Int32, "位置测量系统错误", 0));

        drilExceptionList.Add(new NodeDescription("ContactBeforeZWasMoving", DataTypeIds.Int32, "CBD接触钻信号过早", 0));
        drilExceptionList.Add(new NodeDescription("ToolInLaserWhenMeasStarts", DataTypeIds.Int32, "当刀检测量开启时已有刀具", 0));
        drilExceptionList.Add(new NodeDescription("ToolInLaserWhenMeasEnds", DataTypeIds.Int32, "当刀检测量结束时仍有刀具", 0));
        drilExceptionList.Add(new NodeDescription("MeasSysErrorOfLaser", DataTypeIds.Int32, "镭射卡错误", 0));
        drilExceptionList.Add(new NodeDescription("WrongMeasSysForLaser", DataTypeIds.Int32, "刀检错误", 0));

        drilExceptionList.Add(new NodeDescription("LaserUnknownError", DataTypeIds.Int32, "刀检未知错误", 0));
        drilExceptionList.Add(new NodeDescription("LaserCanNotTurnOn", DataTypeIds.Int32, "刀检打不开", 0));
        drilExceptionList.Add(new NodeDescription("GripperLostTool", DataTypeIds.Int32, "机械手丢刀", 0));
        drilExceptionList.Add(new NodeDescription("CBDNotIn", DataTypeIds.Int32, "蘑菇头没有推进", 0));
        drilExceptionList.Add(new NodeDescription("PinCollectNotClose", DataTypeIds.Int32, "PIN夹未夹紧", 0));

        drilExceptionList.Add(new NodeDescription("CheckZHeightLow10", DataTypeIds.Int32, "Z高度检查小于10", 0));
        drilExceptionList.Add(new NodeDescription("AutoCleanCollet", DataTypeIds.Int32, "自动清洗夹头", 0));
        drilExceptionList.Add(new NodeDescription("QICNotRight", DataTypeIds.Int32, "压脚传感器信号错误", 0));
        drilExceptionList.Add(new NodeDescription("InternalCommunicationError", DataTypeIds.Int32, "主进程期间的内部通信错误", 0));
        drilExceptionList.Add(new NodeDescription("FatalError", DataTypeIds.Int32, "严重错误", 0));

        drilExceptionList.Add(new NodeDescription("NoZAxisSelected", DataTypeIds.Int32, "未选择Z轴", 0));
        drilExceptionList.Add(new NodeDescription("ToolCouldNotBeMeasured", DataTypeIds.Int32, "刀具无法测量", 0));
        drilExceptionList.Add(new NodeDescription("ToolMeasurementError", DataTypeIds.Int32, "刀具测量错误", 0));
        drilExceptionList.Add(new NodeDescription("PositionNotFound", DataTypeIds.Int32, "未找到位置", 0));
        drilExceptionList.Add(new NodeDescription("MappingNotAllowed", DataTypeIds.Int32, "不允许映射", 0));

        drilExceptionList.Add(new NodeDescription("NotAllowed", DataTypeIds.Int32, "不允许", 0));
        drilExceptionList.Add(new NodeDescription("NoToolInSpindleCollet", DataTypeIds.Int32, "主轴夹头中无刀具", 0));
        drilExceptionList.Add(new NodeDescription("WaitForProgram", DataTypeIds.Int32, "等待程序", 0));
        drilExceptionList.Add(new NodeDescription("ProgramCheck", DataTypeIds.Int32, "程序检测", 0));
        drilExceptionList.Add(new NodeDescription("ProgramIsLoaded", DataTypeIds.Int32, "程序加载", 0));

        drilExceptionList.Add(new NodeDescription("LowBatteryPower", DataTypeIds.Int32, "低电池电量", 0));
        drilExceptionList.Add(new NodeDescription("Warning", DataTypeIds.Int32, "警告", 0));
        drilExceptionList.Add(new NodeDescription("EndOfJobLis", DataTypeIds.Int32, "工作清单结束", 0));
        drilExceptionList.Add(new NodeDescription("CanNotBeExecuted", DataTypeIds.Int32, "<开始于…>无法执行", 0));
        drilExceptionList.Add(new NodeDescription("SpotFacingNotAllowed", DataTypeIds.Int32, "不允许点焊", 0));

        drilExceptionList.Add(new NodeDescription("BoardSurface", DataTypeIds.Int32, "板面", 0));
        drilExceptionList.Add(new NodeDescription("MinimumDistanceBetweenDUOAxesUnderRun", DataTypeIds.Int32, "DUO轴之间的最小距离", 0));
        drilExceptionList.Add(new NodeDescription("MaximumDistanceBetweenDUOAxesExceeded", DataTypeIds.Int32, "超过DUO轴之间的最大距离", 0));
        drilExceptionList.Add(new NodeDescription("StartPointNotFound", DataTypeIds.Int32, "未找到起点", 0));
        drilExceptionList.Add(new NodeDescription("NoDataError", DataTypeIds.Int32, "无数据错误", 0));

        drilExceptionList.Add(new NodeDescription("InputError", DataTypeIds.Int32, "输入错误", 0));
        drilExceptionList.Add(new NodeDescription("DriveError", DataTypeIds.Int32, "驱动器错误", 0));
        drilExceptionList.Add(new NodeDescription("FileNotFound", DataTypeIds.Int32, "找不到文件", 0));
        drilExceptionList.Add(new NodeDescription("DataMediumFull", DataTypeIds.Int32, "数据媒体已满", 0));
        drilExceptionList.Add(new NodeDescription("WrongFormatBlock", DataTypeIds.Int32, "格式错误", 0));

        drilExceptionList.Add(new NodeDescription("FileTooLarge", DataTypeIds.Int32, "文件太大", 0));
        drilExceptionList.Add(new NodeDescription("DataMediumWriteProtected", DataTypeIds.Int32, "数据介质写保护", 0));
        drilExceptionList.Add(new NodeDescription("FileAlreadyExists", DataTypeIds.Int32, "文件已存在", 0));
        drilExceptionList.Add(new NodeDescription("FileWriteProtected", DataTypeIds.Int32, "文件写保护", 0));
        drilExceptionList.Add(new NodeDescription("DataTransmissionError", DataTypeIds.Int32, "数据传输错误", 0));

        drilExceptionList.Add(new NodeDescription("DirectoryError", DataTypeIds.Int32, "目录错误", 0));
        drilExceptionList.Add(new NodeDescription("InOutputNorReady", DataTypeIds.Int32, "输入/输出系统未准备就绪", 0));
        drilExceptionList.Add(new NodeDescription("SoftwareFileError", DataTypeIds.Int32, "软件文件错误", 0));
        drilExceptionList.Add(new NodeDescription("BlockTooLong", DataTypeIds.Int32, "块太长", 0));
        drilExceptionList.Add(new NodeDescription("SmallLettersNotAllowed", DataTypeIds.Int32, "不允许使用小写字母", 0));

        drilExceptionList.Add(new NodeDescription("EndOfFile", DataTypeIds.Int32, "文件结束", 0));
        drilExceptionList.Add(new NodeDescription("DiameterNotFoundDataHaveNotBeenTransmitted", DataTypeIds.Int32, "未找到直径：数据尚未传输。", 0));
        drilExceptionList.Add(new NodeDescription("DiameterTableFull", DataTypeIds.Int32, "直径工作台已满", 0));
        drilExceptionList.Add(new NodeDescription("ATPDataCanNotBeTransmittedToolInColle", DataTypeIds.Int32, "无法传输ATP数据夹头中的工具", 0));
        drilExceptionList.Add(new NodeDescription("WrongData", DataTypeIds.Int32, "错误数据", 0));

        drilExceptionList.Add(new NodeDescription("StorageError", DataTypeIds.Int32, "存储错误", 0));
        drilExceptionList.Add(new NodeDescription("CodeInterpreterMissing", DataTypeIds.Int32, "代码解释器缺失", 0));
        drilExceptionList.Add(new NodeDescription("WringFileFormat", DataTypeIds.Int32, "写入文件格式", 0));
        drilExceptionList.Add(new NodeDescription("FileTimeout", DataTypeIds.Int32, "文件超时", 0));
        drilExceptionList.Add(new NodeDescription("ProgramEmpty", DataTypeIds.Int32, "程序为空", 0));

        drilExceptionList.Add(new NodeDescription("CodeInterpreterNotLicensed", DataTypeIds.Int32, "代码解释器未获得许可", 0));
        drilExceptionList.Add(new NodeDescription("FileEmpty", DataTypeIds.Int32, "文件为空", 0));
        drilExceptionList.Add(new NodeDescription("FileDoesNotIncludeEnoughData", DataTypeIds.Int32, "文件没有包含足够的数据", 0));
        drilExceptionList.Add(new NodeDescription("CodeInterpreterMatchMachineType", DataTypeIds.Int32, "代码解释器与机器类型不匹配", 0));
        drilExceptionList.Add(new NodeDescription("FileCouldNotBeWritten", DataTypeIds.Int32, "无法写入电流协议文件", 0));

        drilExceptionList.Add(new NodeDescription("NoEvent", DataTypeIds.Int32, "无事件/无错误", 0));
        drilExceptionList.Add(new NodeDescription("MDError", DataTypeIds.Int32, "伺服错误", 0));
        drilExceptionList.Add(new NodeDescription("MDApplicationError", DataTypeIds.Int32, "应用错误", 0));
        drilExceptionList.Add(new NodeDescription("ErrorDuringToolMeasurement", DataTypeIds.Int32, "刀具测量过程中的错误", 0));
        drilExceptionList.Add(new NodeDescription("DrillStrokeError", DataTypeIds.Int32, "钻孔行程错误", 0));

        drilExceptionList.Add(new NodeDescription("RoutingError", DataTypeIds.Int32, "路由错误", 0));
        drilExceptionList.Add(new NodeDescription("MDMessage", DataTypeIds.Int32, "MD消息", 0));
        drilExceptionList.Add(new NodeDescription("MDServiceError", DataTypeIds.Int32, "MD服务错误", 0));
        drilExceptionList.Add(new NodeDescription("FCError", DataTypeIds.Int32, "FC错误", 0));
        drilExceptionList.Add(new NodeDescription("FCApplicationError", DataTypeIds.Int32, "FC应用程序错误", 0));

        drilExceptionList.Add(new NodeDescription("FCServiceError", DataTypeIds.Int32, "FC服务错误", 0));
        drilExceptionList.Add(new NodeDescription("FCMessage", DataTypeIds.Int32, "FC消息", 0));
        drilExceptionList.Add(new NodeDescription("ErrorInSpotFacingProbeMeasurement", DataTypeIds.Int32, "点对探头测量错误", 0));
        drilExceptionList.Add(new NodeDescription("MDFCApplicationWindowCanNotFound", DataTypeIds.Int32, "找不到MD/FC应用程序窗口", 0));
        drilExceptionList.Add(new NodeDescription("InternalServolinkError", DataTypeIds.Int32, "内部SERVOLINK错误", 0));

        drilExceptionList.Add(new NodeDescription("FatalServolinkError", DataTypeIds.Int32, "严重SERVOLINK错误", 0));
        drilExceptionList.Add(new NodeDescription("GlobalServolinkError", DataTypeIds.Int32, "全局SERVOLINK错误", 0));
    }
}

public class NodeDescription
{
    public string NodeName;
    public NodeId NodeType;
    public string NodeDes;
    public object NodeValue;
    public int ValueRank = ValueRanks.Scalar;

    public NodeDescription(string _NodeName, NodeId _NodeType, string _NodeDes, object _NodeValue)
    {
        this.NodeName = _NodeName;
        this.NodeType = _NodeType;
        this.NodeDes = _NodeDes;
        this.NodeValue = _NodeValue;
        this.ValueRank = ValueRanks.Scalar;
    }

    public NodeDescription(string _NodeName, NodeId _NodeType, string _NodeDes, object _NodeValue, int _ValueRank)
    {
        this.NodeName = _NodeName;
        this.NodeType = _NodeType;
        this.NodeDes = _NodeDes;
        this.NodeValue = _NodeValue;
        this.ValueRank = _ValueRank;
    }
}
