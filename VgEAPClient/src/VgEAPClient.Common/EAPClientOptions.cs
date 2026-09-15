// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common;

using System.Net;
using VgAutoDrill.Fundation.Iot.Configuration;

public class EAPClientOptions
{
    public string Language { get; set; } = "default";
    public string MainFormTitle { get; set; } = string.Empty;
    public string UserID { get; set; } = string.Empty;
    public bool IsLogin { get; set; } = true;
    public string EquipmentID { get; set; } = string.Empty;
    public string LineCode { get; set; } = string.Empty;
    public string HttpListenPort { get; set; } = string.Empty;
    public string OperationMode { get; set; } = string.Empty;
    public string CIMMode { get; set; } = string.Empty;
    public string HttpPostUrl { get; set; } = string.Empty;
    public string AuthorizationCode { get; set; } = string.Empty;
    public int ReportIntervalMin { get; set; } = 0;
    public string PortNum { get; set; } = string.Empty;
    public string ModbusAddr { get; set; } = string.Empty;
    public bool IsDebug { get; set; } = false;

    public string CustomCfg { get; set; } = "";//非标项目中的额外配置

    public bool IsMsgBoxBrokenToolInfo { get; set; } = false;

    public MsgBoxBrokenToolInfo MsgBoxBrokenToolInfoSet { get; set; } = new();

    public ushort CNC84Port { get; set; } = 33822;
    public EAPCommunicationOptions Communication { get; set; } = new();
#if DEBUG
    public string CNC84Server { get; set; } = "192.168.102.155";  //"192.168.200.128";//
#else
    public string CNC84Server { get; set; } = "127.0.0.1";
#endif

    public bool CNC84GetJobInfoByAutoList { get; set; } = false;

    /// <summary>
    ///  DNC OpcUaServer 服务地址
    /// </summary>
    public string OpcUaServer { get; set; } = string.Empty;

    public int T { get; set; } = 0;
    public bool IsPM { get; set; } = false;
    public int SpindleCount { get; set; } = 6;
#if DEBUG
    public string OPCDAServer { get; set; } = "opc.tcp://127.0.0.1:16664";
#else
    public  string OPCDAServer { get; set; } = "opc.tcp://127.0.0.1:16664";
#endif
    public string OPCUAPwd { get; set; } = "worker";
    public string OPCUAUser { get; set; } = "service";
    public string selectedDiaFile { get; set; } = "";

    public DeviceDescriptor DeviceDescriptor { get; set; } = new();

    /// <summary>
    /// 鉴权字段
    /// </summary>
    public string AuthString { get; set; } = string.Empty;

    /// <summary>
    /// 是否可以发送本地监听IP和Port
    /// </summary>
    public bool bIsFirstSendIPPort = true;

    public IPEndPoint? iPLocal;

    public string GetArrangeDataAddr { get; set; } = string.Empty;

    /// <summary>
    /// ATP 文件目录
    /// </summary>
    public string ATPFileDir { get; set; } = @"C:\Users\Public\SIEB-MEYER\AtpFiles";

    public string ATPFileSuffix { get; set; } = ".atp";
    public string ATPFile { get; set; } = @"C:\Users\Public\SIEB-MEYER\AtpFiles\vega.atp";
    public string DIAFileSuffix { get; set; } = ".dia";
    public string DIAFilePath { get; set; } = @"C:\Users\Public\SIEB-MEYER\DiameterTables";
    public string DIAFile { get; set; } = @"C:\Users\Public\SIEB-MEYER\DiameterTables\vega.dia";

    /// <summary>
    /// 自动加载cnc刀具文件
    /// </summary>
    public bool AutoLoadCNCFile { get; set; } = false;

    /// <summary>
    /// 本地加载程式配置-Everything 搜索路径
    /// </summary>
    public string RecipeSearchPath { get; set; } = "E:\\test\\";

    /// <summary>
    /// 预加载文件间隔(秒)
    /// </summary>
    public int RecipeSearchPathTimeFresh { get; set; } = 0;
    /// <summary>
    /// 文件名额外匹配正则
    /// </summary>
    public string RecipeSearchNameRegex { get; set; } = "";

    /// <summary>
    /// 是否匿名访问网络路径
    /// </summary>
    public bool IsAnonymous { get; set; } = false;
    /// <summary>
    /// 网络路径访问用户名
    /// </summary>
    public string NetPathUser { get; set; } = string.Empty;
    /// <summary>
    /// 网络路径访问密码
    /// </summary>
    public string NetPathPwd { get; set; } = string.Empty;

    /// <summary>
    /// 本地加载程式配置-是否加载DIA文件
    /// </summary>
    public bool IsLoadDiaFile { get; set; } = true;

    /// <summary>
    /// 本地加载程式配置-搜索DIA文件后缀
    /// </summary>

    public string DiaSearchSuffix { get; set; } = ".dia";
    public string AtpSearchSuffix { get; set; } = ".atp";
    public bool IsDirectLoadFile { get; set; } = false;

    /// <summary>
    /// 运行开机自动启动
    /// </summary>
    public bool EnableAutoStartApp { get; set; } = true;

    /// <summary>
    /// 记录采集的数据在UI界面上
    /// </summary>
    public bool EnableLogCollectionDataOnUI { get; set; } = true;

    /// <summary>
    /// 是否采集电表数据
    /// </summary>
    public bool EnableAmmeter { get; set; } = false;

    /// <summary>
    /// 电表的型号.默认为
    /// </summary>
    public string AmmeterModel { get; set; } = "DTSSU6606VCRF";

    /// <summary>
    /// 电表用的端口名
    /// </summary>
    public string AmmeterPortName { get; set; } = "COM4";

    /// <summary>
    /// 波特率
    /// </summary>
    public int AmmeterBaudRate { get; set; } = 9600;

    //和BUFF协作的模式.    0:无 ；  1:半自动（BUFF会自动进料出料， 但需要人工在CNC界面操作才会 开始加工）； 
    public int BuffUseMode { get; set; } = 0;


    /// <summary>
    /// 是否将程式配置同步加载到其它机台编号
    /// </summary>
    public bool IsCollaborative { get; set; } = false;

    /// <summary>
    /// 其它机台编号列表
    /// </summary>
    public List<string> OthEquipmentIDs { get; set; } = [];

    /// <summary>
    /// 维护项目配置，每一配置的格式(注意这里的时长单位都以小时计) : 项目名1,维护间隔1,提醒提前量1;项目名2,维护间隔2,提醒提前量2;
    /// 例子:     导轨保养,960,48;PIN校准,600,36;
    /// </summary>
    public string MaintenanceCfg { get; set; } = "";

    /// <summary>
    /// 是否弹窗提示文件载入成功或失败
    /// </summary>
    public bool IsLoadFileMsgBoxShow { get; set; } = true;

    /// <summary>
    /// 报警状态是否可以加载文件
    /// </summary>
    public bool IsAlarmLoadFile { get; set; } = true;

    /// <summary>
    /// RecipeList 程序文件 参数名(支持正则)
    /// </summary>
    public string RecipeListPgmRegex { get; set; } = "^FilePath";

    /// <summary>
    /// RecipeList 程序文件 参数值(0=获取全路径;1=只获取文件名,去指定目录查询)
    /// </summary>
    public int RecipeListPgmGetType { get; set; } = 1;

    /// <summary>
    /// 程序文件 指定目录查询是否模糊查询
    /// </summary>
    public bool IsBlurSerachRecipeListPgm { get; set; } = true;

    /// <summary>
    /// RecipeList 参数文件 参数名(支持正则)
    /// </summary>
    public string RecipeListDiaRegex { get; set; } = "^ParamPath";

    /// <summary>
    /// RecipeList 参数文件 参数值(0=获取全路径;1=只获取文件名,去指定目录查询)
    /// </summary>
    public int RecipeListDiaGetType { get; set; } = 0;

    /// <summary>
    /// 参数文件 指定目录查询是否模糊查询
    /// </summary>
    public bool IsBlurSerachRecipeListDia { get; set; } = true;

    /// <summary>
    /// RecipeList 刀盘文件 参数名(支持正则)
    /// </summary>
    public string RecipeListAtpRegex { get; set; } = "^AtpPath";

    /// <summary>
    /// RecipeList 刀盘文件 参数值(0=获取全路径;1=只获取文件名,去指定目录查询)
    /// </summary>
    public int RecipeListAtpGetType { get; set; } = 0;

    /// <summary>
    /// 刀盘文件 指定目录查询是否模糊查询
    /// </summary>
    public bool IsBlurSerachRecipeListAtp { get; set; } = true;

    public bool IsIncludeEqpType { get; set; } = false;
    /// <summary>
    /// 设备类型(DRL=钻机;RUT=锣机)
    /// </summary>
    public string EqpType { get; set; } = "DRL";
}

public class MsgBoxBrokenToolInfo
{
    /// <summary>
    /// 批次号最小输入长度 0为不限制
    /// </summary>
    public int ItemIdMinLength { get; set; } = 0;

    /// <summary>
    /// 断刃刃长选项
    /// </summary>
    public List<string> BrokenLengthItems { get; set; } = new List<string>();

    /// <summary>
    /// 断刃原因选项
    /// </summary>
    public List<string> BrokenReasonItems { get; set; } = new List<string>();
}
