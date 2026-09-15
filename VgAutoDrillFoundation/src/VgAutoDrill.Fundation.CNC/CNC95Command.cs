using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Opc.Ua.Client;
using OpcUaHelper;
using VgAutoDrill.Fundation.CNC.Model;
using VgAutoDrill.Fundation.CNC.Packet;

namespace VgAutoDrill.Fundation.CNC;

public class CNC95Command : ICNCCommand
{
    private OpcUaClient? opcUaClient = null;
    private readonly ILogger<CNC95Command> _logger;

    public CNC95Command(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CNC95Command>();
    }

    public async void Init(string ip, int port)
    {
        opcUaClient = new OpcUaClient();
        //设置用户名连接
        opcUaClient!.UserIdentity = new UserIdentity("service", "worker");
        try
        {
            await opcUaClient!.ConnectServer($"opc.tcp://{ip}:{port}");
            if (opcUaClient!.Connected == false)
            {
                _logger.LogInformation("OPC UA Server 连接失败");
            }
            else//连接成功就开始采集OPC UA Server的数据
            {
                _logger.LogInformation("OPC UA Server 连接成功");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("OPC UA Server 连接失败\r\n" + ex.Message);
        }
    }

    public async void Init()
    {
        opcUaClient = new OpcUaClient();
        //设置用户名连接
        opcUaClient!.UserIdentity = new UserIdentity("service", "worker");
        try
        {
            await opcUaClient!.ConnectServer("opc.tcp://127.0.0.1:16664");
            if (opcUaClient!.Connected == false)
            {
                _logger.LogInformation("OPC UA Server 连接失败");
            }
            else//连接成功就开始采集OPC UA Server的数据
            {
                _logger.LogInformation("OPC UA Server 连接成功");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("OPC UA Server 连接失败\r\n" + ex.Message);
        }
    }

    public bool CNCCommandStatus()
    {
        if (opcUaClient == null)
        {
            return false;
        }
        return opcUaClient!.Connected;
    }

    #region REQUEST

    #region UserName

    public string GetUserName()
    {
        string username = string.Empty;
        username = opcUaClient!.ReadNode<String>("ns=4;s=UI/origin/User/User.CurrentUser/UserName");
        return username;
    }

    #endregion UserName

    #region CncStatus

    public VgCNCStatus GetCncStatus()
    {
        string statusPara = opcUaClient!.ReadNode<String>("ns=4;s=UI/origin/DDETable/CncStatus");
        return ParseStatusPara(statusPara);
    }

    public VgCNCStatus ParseStatusPara(string str)
    {
        if (str == null)
        {
            return null;
        }
        ////   AR00:00:40,AP016,ZS00111111,MOSTOP,EC0000000000,FNF:\1.dr
        VgCNCStatus vgCNCStatus = new VgCNCStatus();
        string[] splitStr = str.Split(',', StringSplitOptions.RemoveEmptyEntries);
        string? strRunTime = splitStr.FirstOrDefault(item => item.StartsWith("AR", StringComparison.OrdinalIgnoreCase));
        if (strRunTime != null)
        {
            TimeSpan lTimeSpan = TimeSpan.Parse(strRunTime.Substring(2));
            vgCNCStatus.RunTime = Convert.ToInt32(lTimeSpan.TotalSeconds);
        }
        string? strFinHoler = splitStr.FirstOrDefault(item => item.StartsWith("AH", StringComparison.OrdinalIgnoreCase));
        if (strFinHoler != null)
        {
            vgCNCStatus.FinHole = Convert.ToInt32(strFinHoler.Substring(2));
        }

        string? strFinPer = splitStr.FirstOrDefault(item => item.StartsWith("AP", StringComparison.OrdinalIgnoreCase));
        if (strFinPer != null)
        {
            vgCNCStatus.FinPer = Convert.ToInt32(strFinPer.Substring(2));
            if (vgCNCStatus.FinPer > 100)
            {
                vgCNCStatus.FinPer = 100;
            }
        }

        string? strSpindleStatus = splitStr.FirstOrDefault(item => item.StartsWith("ZS", StringComparison.OrdinalIgnoreCase));
        if (strSpindleStatus != null)
        {
            vgCNCStatus.SpindleStatus = spindlestatus(strSpindleStatus.Substring(2));
            vgCNCStatus.SpindleStatusOriginalData = strSpindleStatus.Substring(2);
        }

        string? strStatus = splitStr.FirstOrDefault(item => item.StartsWith("MO", StringComparison.OrdinalIgnoreCase));
        if (strSpindleStatus != null)
        {
            vgCNCStatus.Status = strStatus!.Substring(2);
        }

        string? strProgramName = splitStr.FirstOrDefault(item => item.StartsWith("FN", StringComparison.OrdinalIgnoreCase));
        if (strSpindleStatus != null)
        {
            vgCNCStatus.ProgramName = strProgramName!.Substring(2);
        }
        return vgCNCStatus;
    }

    /// <summary>
    /// 将16轴转换为8轴
    /// </summary>
    /// <param name="binaryString"></param>
    /// <returns></returns>
    public string spindlestatus(string binaryString)
    {
        string strSpindleStatus = string.Empty;
        int chunkSize = 2;
        int numChunks = (int)Math.Ceiling((double)binaryString.Length / chunkSize);
        string[] binaryArray = new string[numChunks];

        for (int i = 0; i < numChunks; i++)
        {
            int startIndex = i * chunkSize;
            int length = Math.Min(chunkSize, binaryString.Length - startIndex);
            binaryArray[i] = binaryString.Substring(startIndex, length);
        }

        foreach (string chunk in binaryArray)
        {
            if (chunk == "00" || chunk == "01" || chunk == "10")
            {
                strSpindleStatus += "0";
            }
            else if (chunk == "11")
            {
                strSpindleStatus += "1";
            }
        }
        return strSpindleStatus;
    }

    #endregion CncStatus

    #region tool

    public VgCNCTools GetToolParameter()
    {
        string toolPara = opcUaClient!.ReadNode<String>("ns=4;s=UI/origin/ToolParameter/ToolCncToolsTable/CncTools");
        return ParseToolParameter(toolPara);
    }

    /// <summary>
    /// Use the command object RPC_START to start the execution (again). The Start key is pressed.
    /// </summary>
    public object[] SendStart()
    {
        object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_START");
        return dataValue;
    }

    public void Start()
    {
        object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_START");
    }

    /// <summary>
    /// Use the command object RPC_SHUTDOWN to Close CNC and return to windows.
    /// </summary>
    public object[] SendShutdown()
    {
        object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_SHUTDOWN");
        return dataValue;
    }

    public void Shutdown()
    {
        object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_SHUTDOWN");
    }

    /// <summary>
    /// Use the command object RPC_STOP to stop the execution. The Stop button is pressed.
    /// </summary>
    public object[] SendStop()
    {
        object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_STOP");
        return dataValue;
    }

    public void Stop()
    {
        object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_STOP");
    }

    /// <summary>
    /// ESC Key
    /// Use the command object RPC_ABORT to abort an execution. The Escape key is pressed.
    /// </summary>
    public object[] SendAbort()
    {
        object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_ABORT");
        return dataValue;
    }

    public void Abort()
    {
        object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_ABORT");
    }

    public VgCNCTools ParseToolParameter(string? str)
    {
        //T1,D0.150,N1500,B150,EN2,EU0,ER1,NN3000,NU150,NR15
        //T0,D0,N0,B0,EN0,EU0,ER0,NN0,NU0,NR0
        //T0,D0,E?,N0,B0,EN0,EU0,ER0,NN0,NU0,NR0
        //T1,D0.150,E0,N1500,B150,EN2,EU0,ER1,NN3000,NU150,NR18
        if (str == null)
        {
            return new VgCNCTools();
        }
        VgCNCTools vgCNCTools = new VgCNCTools();
        string[] splitStr = str.Split(',');
        string? strToolType = splitStr.Where(item => { return item.Length == 2; }).FirstOrDefault(item => { return item.StartsWith("E", StringComparison.OrdinalIgnoreCase); });
        if (strToolType != null)
        {
            vgCNCTools.ToolType = int.Parse(strToolType.Substring(1));
        }
        string? strToolNumber = splitStr.FirstOrDefault(item => item.StartsWith("T", StringComparison.OrdinalIgnoreCase));
        if (strToolNumber != null)
        {
            vgCNCTools.ToolNumber = strToolNumber;
        }

        string? strToolDiameter = splitStr.FirstOrDefault(item => item.StartsWith("D", StringComparison.OrdinalIgnoreCase));
        if (strToolDiameter != null)
        {
            vgCNCTools.ToolDiameter = float.Parse(strToolDiameter.Substring(1));
        }

        string? strPresetToolLife = splitStr.FirstOrDefault(item => item.StartsWith("N", StringComparison.OrdinalIgnoreCase));
        if (strPresetToolLife != null)
        {
            vgCNCTools.PresetToolLife = float.Parse(strPresetToolLife.Substring(1));
        }
        string? strRemainderToolLife = splitStr.FirstOrDefault(item => item.StartsWith("B", StringComparison.OrdinalIgnoreCase));
        if (strRemainderToolLife != null)
        {
            vgCNCTools.RemainderToolLife = float.Parse(strRemainderToolLife.Substring(1));
        }

        string? strToolCount = splitStr.FirstOrDefault(item => item.StartsWith("EN", StringComparison.OrdinalIgnoreCase));
        if (strToolCount != null)
        {
            vgCNCTools.ToolCount = int.Parse(strToolCount.Substring(2));
        }
        string? strExecutedToolNum = splitStr.FirstOrDefault(item => item.StartsWith("EU", StringComparison.OrdinalIgnoreCase));
        if (strExecutedToolNum != null)
        {
            vgCNCTools.ExecutedToolNum = int.Parse(strExecutedToolNum.Substring(2));
        }

        string? strRequiredToolNum = splitStr.FirstOrDefault(item => item.StartsWith("ER", StringComparison.OrdinalIgnoreCase));
        if (strRequiredToolNum != null)
        {
            vgCNCTools.RequiredToolNum = int.Parse(strRequiredToolNum.Substring(2));
        }

        string? strToolLifeCount = splitStr.FirstOrDefault(item => item.StartsWith("NN", StringComparison.OrdinalIgnoreCase));
        if (strToolLifeCount != null)
        {
            vgCNCTools.ToolLifeCount = float.Parse(strToolLifeCount.Substring(2));
        }
        string? strUsedLifeCount = splitStr.FirstOrDefault(item => item.StartsWith("NU", StringComparison.OrdinalIgnoreCase));
        if (strUsedLifeCount != null)
        {
            vgCNCTools.UsedLifeCount = float.Parse(strUsedLifeCount.Substring(2));
        }

        string? strRemainderLifeCount = splitStr.FirstOrDefault(item => item.StartsWith("NR", StringComparison.OrdinalIgnoreCase));
        if (strRemainderLifeCount != null)
        {
            vgCNCTools.RemainderLifeCount = float.Parse(strUsedLifeCount!.Substring(2));
        }
        return vgCNCTools;
    }

    #endregion tool

    #region ScreenSaver

    public VgCNCScreenSaver GetScreenSaver()
    {
        string screenSaver = string.Empty;
        screenSaver = opcUaClient!.ReadNode<Int32>("ns=4;s=UI/origin/RosiInfo/BlockBackgroundColor").ToString() + " " + opcUaClient!.ReadNode<Int32>("ns=4;s=UI/origin/RosiInfo/BlockTextColor").ToString() +
            "\n" + opcUaClient!.ReadNode<String>("ns=4;s=UI/origin/RosiInfo/BlockText");
        return ParseScreenSaverParameter(screenSaver);
    }

    public VgCNCScreenSaver GetScreenText()
    {
        VgCNCScreenSaver vgCNCScreenSaver = GetScreenSaver();
        return vgCNCScreenSaver;
    }

    /// <summary>
    /// 255 65535
    /// [94] 程 序 中 断
    /// Block 15  Step 1  Hole 3  Path 0.000
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public VgCNCScreenSaver ParseScreenSaverParameter(string str)
    {
        ///////////16711680 16777215[1546] TOOL LENGTH ERROR BY TOLC T=1 Z1:S/Z2:S D1.100
        ///////32768 16777215 4%4%
        if (str == null)
        {
            return null;
        }
        VgCNCScreenSaver vgCNCScreenSaver = new VgCNCScreenSaver();
        string[] SplitStr = str.Split('\n');
        if (SplitStr.Count() > 0)
        {
            string[] tmpstr = SplitStr[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tmpstr.Count() == 2)
            {
                try
                {
                    //vgCNCScreenSaver.BackColor = Convert.ToInt32(tmpstr[0]).ToString("X6");
                    //vgCNCScreenSaver.TextColor = Convert.ToInt32(tmpstr[1]).ToString("X6");
                    vgCNCScreenSaver.BackColor = ParseScreenSaverColor(tmpstr[0]);
                    vgCNCScreenSaver.TextColor = Convert.ToInt32(tmpstr[1]).ToString("X6");
                }
                catch (Exception)
                {
                    //ServiceLogger.LogError(e.ToString());
                    // ServiceLogger.LogError(str);
                    Console.WriteLine("GetScreenSaverPara 函数发生异常");
                }
            }
        }
        for (int i = 1; i < SplitStr.Count(); i++)
        {
            vgCNCScreenSaver.ScreenText += SplitStr[i] + " ";
        }
        return vgCNCScreenSaver;
    }

    private string ParseScreenSaverColor(string str)
    {// BlockBackgroundColor红色:5066239BlockText:未找到T1刀具BlockTextColor:4340265
        if ("5066239" == str)//error
        {
            return "0000FF";
        }
        if ("16711680" == str)//seq
        {
            return "FF0000";
        }
        if ("32768" == str)//run
        {
            return "8000";
        }
        return "";
    }

    #endregion ScreenSaver

    #region CncError===================================================

    public VgCncError GetCncError()
    {
        DataValue dataValue = opcUaClient!.ReadNode(new NodeId("ns=4;s=UI/origin/User/User.CurrentUser/UserName"));
        string statusPara = string.Empty;
        foreach (var s in (string[])dataValue.Value)
        {
            if (s.Trim() != "")
                statusPara = s;
        }
        return ParseCncErrorParameter(statusPara);
    }

    public VgCncError ParseCncErrorParameter(string str)
    {
        if (str == null)
        {
            return null;
        }
        string[] SplitStr = str.Split('*');
        VgCncError vgCncError = new VgCncError();

        vgCncError.ErrorID = GetEventId(SplitStr[2].Trim());
        vgCncError.CurrentTime = ParseTime(SplitStr[0].Trim());
        vgCncError.DateString = ParseDate(SplitStr[0].Trim());
        vgCncError.ProgrameName = opcUaClient!.ReadNode<String>("ns=4;s=UI/origin/RPCTable/RPCDataTable/RPC_DATA_ACTPROGRAM");
        vgCncError.ErrorMessage = SplitStr[3].Trim();

        return vgCncError;
    }

    private string GetErrorMessage(string str)
    {
        if (str != null)
        {
            string[] SplitStr = str.Split(new char[] { ';', '*' }, StringSplitOptions.RemoveEmptyEntries);
            if (SplitStr.Count() > 1)
            {
                return SplitStr[SplitStr.Count() - 2];
            }
        }

        return "";
    }

    private int GetEventId(string str)
    {
        if (str != null)
        {
            return Convert.ToInt32(str.Trim());
        }
        return 0;
    }

    #endregion CncError===================================================

    #region time

    private string ParseDate(string str)
    {
        string[] SplitStr = str.Split(' ');
        if (SplitStr.Length >= 2)
        {
            string textStr = SplitStr[0];
            return textStr;
        }
        return DateTime.Now.ToString("dd/MM/yy");
    }

    private DateTime ParseTime(string str)
    {
        string[] SplitStr = str.Split(' ');
        if (SplitStr.Length >= 2)
        {
            string TextStr = SplitStr[0] + " " + SplitStr[1];
            if (!string.IsNullOrEmpty(TextStr))
            {
                IFormatProvider culture = new CultureInfo("fr-FR", true);
                return DateTime.Parse(TextStr, culture, DateTimeStyles.NoCurrentDateDefault);
            }
        }
        return DateTime.Now;
    }

    private DateTime ParseTime(string date, string time)
    {
        string TextStr = date + " " + time;
        IFormatProvider culture = new CultureInfo("fr-FR", true);
        return DateTime.Parse(TextStr, culture, DateTimeStyles.NoCurrentDateDefault);
    }

    private string GetProgrameName(string str)
    {
        if (str != null)
        {
            string[] splitStr = str.Split('*', StringSplitOptions.RemoveEmptyEntries);
            if (splitStr.Count() >= 1)
            {
                string[] splitStrTemp = splitStr[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (splitStrTemp.Count() >= 1)
                {
                    return splitStrTemp[splitStrTemp.Count() - 1];
                }
            }
        }

        return "";
    }

    #endregion time

    #region OutFlags

    public string GetOutFlags()
    {
        string outflags = string.Empty;
        outflags = opcUaClient!.ReadNode<String>("ns=4;s=UI/origin/DDETable/LoaderOutputFlags");
        return outflags;
    }

    #endregion OutFlags

    #region ACTProgram

    public string GetACTProgram()
    {
        string actprogram = string.Empty;
        actprogram = opcUaClient!.ReadNode<String>("ns=4;s=UI/origin/FilesAndPaths/ProgramFiles/ProgramGroup/Program");
        return actprogram;
    }

    #endregion ACTProgram

    #region DiaFileNameWithDialog====================

    public string GetDiaFileNameWithDialog()
    {
        string diaFileNameWithDialog = string.Empty;
        diaFileNameWithDialog = opcUaClient!.ReadNode<String>("ns=4;s=UI/origin/FilesAndPaths/ToolFiles/DiameterGroup/DiameterTable");
        return diaFileNameWithDialog;
    }

    #endregion DiaFileNameWithDialog====================

    #region AtpFileName================

    public string GetAtpFileName()
    {
        string atpFileNameWithDialog = string.Empty;
        atpFileNameWithDialog = opcUaClient!.ReadNode<String>("ns=4;s=UI/origin/FilesAndPaths/ToolFiles/ATPGroup/ATP");
        return atpFileNameWithDialog;
    }

    #endregion AtpFileName================

    #region SendLoadFile

    /// <summary>
    /// Use the command object RPC_LOAD to load a file.
    /// ▶ PROGRAM = part program
    /// ▶ SUBPROGARM = subprogram
    /// ▶ ATP = ATP file
    /// ▶ DIAMETERTABLE = diameter file
    /// ▶ INDEXTABLE = index file
    /// ▶ INFO = Info file
    /// ▶ TEXT = CNC command file
    /// ▶ MACHINECONFIGURATION = configuration file
    /// </summary>
    /// <param name="filePath"></param>
    [Obsolete("use two parameters method instead", true)]
    public object[] SendLoadFile(string filePath)
    {
        //object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_PROGRAM", filePath);
        string fileExtension = Path.GetExtension(filePath).TrimStart('.');
        int result_drl = string.Compare(fileExtension, "drl", StringComparison.OrdinalIgnoreCase);
        if (result_drl == 0)
        {
            string[] parameters = { filePath, "PROGRAM" };
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_LOAD", parameters);
            return dataValue;
        }
        int result_dia = string.Compare(fileExtension, "dia", StringComparison.OrdinalIgnoreCase);
        if (result_dia == 0)
        {
            string[] parameters = { filePath, "DIAMETERTABLE" };
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_LOAD", parameters);
            return dataValue;
        }
        return new object[] { };
    }

    /// <summary>
    /// Use the command object RPC_LOAD to load a file.
    /// ▶ PROGRAM = part program
    /// ▶ SUBPROGARM = subprogram
    /// ▶ ATP = ATP file
    /// ▶ DIAMETERTABLE = diameter file
    /// ▶ INDEXTABLE = index file
    /// ▶ INFO = Info file
    /// ▶ TEXT = CNC command file
    /// ▶ MACHINECONFIGURATION = configuration file
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileType">PROGRAM,DIAMETERTABLE</param>
    /// <returns></returns>
    public object[] SendLoadFile(string filePath, string fileType)
    {
        string[] parameters = { filePath, fileType };
        object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_LOAD", parameters);
        return dataValue;
    }

    public void SetLoadFile(string filePath)
    {
        //object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_PROGRAM", filePath);
        string fileExtension = Path.GetExtension(filePath).TrimStart('.');
        int result_drl = string.Compare(fileExtension, "drl", StringComparison.OrdinalIgnoreCase);
        if (result_drl == 0)
        {
            string[] parameters = { filePath, "PROGRAM" };
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_LOAD", parameters);
        }
        int result_dia = string.Compare(fileExtension, "dia", StringComparison.OrdinalIgnoreCase);
        if (result_dia == 0)
        {
            string[] parameters = { filePath, "DIAMETERTABLE" };
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_LOAD", parameters);
        }
    }

    #endregion SendLoadFile

    #endregion REQUEST

    #region ADVICE

    #region UserName

    public void RegisterUserNameAdvice(Func<SMDncPacket, Task> callback) => throw new NotImplementedException();

    #endregion UserName

    #region OutFlags

    public void RegisterOutFlags(Func<SMDncPacket, Task> callback) => throw new NotImplementedException();

    #endregion OutFlags

    #region CncError

    public void RegisterCncError(Func<SMDncPacket, Task> callback) => throw new NotImplementedException();

    #endregion CncError

    #region CncStatus

    public void RegisterCncStatus(Func<SMDncPacket, Task> callback) => throw new NotImplementedException();

    #endregion CncStatus

    #endregion ADVICE

    #region CncTools

    public void RegisterCncTools(Func<SMDncPacket, Task> callback) => throw new NotImplementedException();

    #endregion CncTools

    #region ScreenSaver

    public void RegisterScreenSaver(Func<SMDncPacket, Task> callback) => throw new NotImplementedException();

    #endregion ScreenSaver

    #region UserFlag

    public void SetUserFlag(int flag, int num)
    {
        if (flag == 0)
        {
            opcUaClient!.WriteNode<Boolean>("ns=4;s=UI/normalized/custom/UF.SmallBoard", false);
        }
        if (flag == 1)
        {
            opcUaClient!.WriteNode<Boolean>("ns=4;s=UI/normalized/custom/UF.SmallBoard", true);
        }
    }

    public void SetSpindleMask(int maskNum)
    {
        opcUaClient!.WriteNode<int>("ns=4;s=UI/normalized/custom/STATIONALLOPENCLOSE", maskNum);
    }

    public void WriteCncNode<T>(string url, T value)
    {
        opcUaClient!.WriteNode<T>(url, value);
    }

    public T ReadCncNode<T>(string url)
    {
        return opcUaClient!.ReadNode<T>(url);
    }

    public object[] CallMethodByNodeId(string tagParent, string tag, params object[] args)
    {
        object[] dataValue = opcUaClient!.CallMethodByNodeId(tagParent, tag, args);
        return dataValue;
    }

    public void AddSubscription(string key, string tag, Action<string, MonitoredItem, MonitoredItemNotificationEventArgs> callback)
    {
        opcUaClient!.AddSubscription(key, tag, callback);
    }

    public void RemoveSubscription()
    {
        opcUaClient!.RemoveAllSubscription();
    }

    public string GetUserFlag(int num)
    {
        if (num == 59)
        {
            Boolean usr = opcUaClient!.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/UF.SmallBoard");//小板
            if (usr)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        return "0";
    }

    /// <summary>
    /// Use the command object RPC_CNCCOMMAND to execute a CNC command in the CNC.
    /// </summary>
    /// <param name="command"></param>
    public object[] SendCncComand(string command)
    {
        /*
        if (command == "SZSA")
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERstationALLON");
        }
        */
        if (command == "M27")//M27,大板,Z1_false,Z2_true	开前后蘑菇头,M41
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scKbiCbdClampOpenClose");
            return dataValue;
        }
        else if (command == "M102")//M102,压板	95调用脚本,M44
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERTWOPINspindleselect");
            return dataValue;
        }
        else if (command == "M106")//M106,中板,Z1_false,Z2_flase	开前中蘑菇头,M38
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
            return dataValue;
        }
        /*
        else if (command == "M107")
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERstation1OFF");
        }
        else if (command == "M108")
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERstation2OFF");
        }
        else if (command == "M109")
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERstation3OFF");
        }
        else if (command == "M110")
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERstation4OFF");
        }
        else if (command == "M111")
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERstation5OFF");
        }
        */
        else
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", command);
            return dataValue;
        }
    }

    public void SetCncComand(string command)
    {
        /*
        if (command == "SZSA")
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERstationALLON");
        }
        */
        if (command == "M27")//M27,大板,Z1_false,Z2_true	开前后蘑菇头,M41
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scKbiCbdClampOpenClose");
        }
        else if (command == "M102")//M102,压板	95调用脚本,M44
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERTWOPINspindleselect");
        }
        else if (command == "M106")//M106,中板,Z1_false,Z2_flase	开前中蘑菇头,M38
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERCbdClampOpenClosesmall");
        }
        /*
        else if (command == "M107")
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERstation1OFF");
        }
        else if (command == "M108")
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERstation2OFF");
        }
        else if (command == "M109")
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERstation3OFF");
        }
        else if (command == "M110")
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERstation4OFF");
        }
        else if (command == "M111")
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scCncCUSTOMERstation5OFF");
        }
        */
        else
        {
            object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", command);
        }
    }

    public string GetRuntimeString(string command)
    {
        if (command == "%S(DiaFileNameWithDialog)")
        {
            return "1:" + GetDiaFileNameWithDialog();
        }
        if (command == "%S(HSYS55_Output,64)")
        {
            return GetOutput(64);
        }
        if (command == "%S(HSYS55_Output,55)")
        {
            return GetOutput(55);
        }
        return "Vega";
    }

    public void SetRuntimeValue(string command) => throw new NotImplementedException();

    #endregion UserFlag

    #region Output

    public string GetOutput(int num, bool flag = false)
    {
        if (num == 55)
        {
            Boolean input = opcUaClient!.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z2");//middle
            if (input)
            {
                return "1:1";
            }
            else
            {
                return "1:0";
            }
        }
        if (num == 58)
        {
            Boolean input = opcUaClient!.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.FCall");//钻孔结束信号，复位置位，钻孔结束置位
            if (input)
            {
                return "1:1";
            }
            else
            {
                return "1:0";
            }
        }
        if (num == 64)
        {
            Boolean input = opcUaClient!.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.CbdOpen_Z1");//front back
            if (input)
            {
                return "1:1";
            }
            else
            {
                return "1:0";
            }
        }
        /*
        if (num == 85)
        {
            Boolean input = opcUaClient!.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.OutArea1_1");//读PLC 12312 P4 Front
            if (input)
            {
                return "1:1";
            }
            else
            {
                return "1:0";
            }
        }
        if (num == 86)
        {
            Boolean input = opcUaClient!.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.OutArea2_1");//读PLC 12313 P4 Rear
            if (input)
            {
                return "1:1";
            }
            else
            {
                return "1:0";
            }
        }
        */
        return "1:0";
    }

    #endregion Output

    public String GetIutput(int num, bool flag = false) => throw new NotImplementedException();

    /*
    public string GetIutput(int num)//ReadCoils读PLC【0】的BufferOnAgvPositionFlag点位
    {
        if (num == 75)
        {
            Boolean input = opcUaClient!.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/Customer.ATRUN");
            if (input)
            {
                return "1:2";
            }
            else
            {
                return "1:0";
            }
        }
        return "1:0";
    }
    */

    #region SeqFlag

    public string GetSeqFlag(int num)
    {
        if (num == 68)
        {
            Boolean boardOver = opcUaClient!.ReadNode<Boolean>("ns=4;s=UI/normalized/custom/SF.BoardOver");//压板结束
            if (boardOver)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        return "0";
    }

    #endregion SeqFlag

    public void SetChangePage(string command)
    {
        if (command.Equals("WORK_WORK"))
        {
            //模拟按下F8键
            keybd_event(vbKeyF8, 0, 0, 0);
            //松开按键F8
            keybd_event(vbKeyF8, 0, 2, 0);
        }
    }

    #region PcKey

    public void SetPcKey(string text)
    {
        if (text.Equals(@"\ESC"))
        {
            //模拟按下esc键
            keybd_event(vbKeyEscape, 0, 0, 0);
            //松开按键esc
            keybd_event(vbKeyEscape, 0, 2, 0);
            //object[] dataValue = opcUaClient!.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", $"SCRP,scOEMEscape");
        }
    }

    public void StartTwo()
    {
        //模拟按下空格键
        keybd_event(vbKeySpace, 0, 0, 0);
        //松开按键空格键
        keybd_event(vbKeySpace, 0, 2, 0);
    }

    #endregion PcKey

    #region bVk参数 常量定义

    public const byte vbKeyLButton = 0x1;    // 鼠标左键
    public const byte vbKeyRButton = 0x2;    // 鼠标右键
    public const byte vbKeyCancel = 0x3;     // CANCEL 键
    public const byte vbKeyMButton = 0x4;    // 鼠标中键
    public const byte vbKeyBack = 0x8;       // BACKSPACE 键
    public const byte vbKeyTab = 0x9;        // TAB 键
    public const byte vbKeyClear = 0xC;      // CLEAR 键
    public const byte vbKeyReturn = 0xD;     // ENTER 键
    public const byte vbKeyShift = 0x10;     // SHIFT 键
    public const byte vbKeyControl = 0x11;   // CTRL 键
    public const byte vbKeyAlt = 18;         // Alt 键  (键码18)
    public const byte vbKeyMenu = 0x12;      // MENU 键
    public const byte vbKeyPause = 0x13;     // PAUSE 键
    public const byte vbKeyCapital = 0x14;   // CAPS LOCK 键
    public const byte vbKeyEscape = 0x1B;    // ESC 键
    public const byte vbKeySpace = 0x20;     // SPACEBAR 键
    public const byte vbKeyPageUp = 0x21;    // PAGE UP 键
    public const byte vbKeyEnd = 0x23;       // End 键
    public const byte vbKeyHome = 0x24;      // HOME 键
    public const byte vbKeyLeft = 0x25;      // LEFT ARROW 键
    public const byte vbKeyUp = 0x26;        // UP ARROW 键
    public const byte vbKeyRight = 0x27;     // RIGHT ARROW 键
    public const byte vbKeyDown = 0x28;      // DOWN ARROW 键
    public const byte vbKeySelect = 0x29;    // Select 键
    public const byte vbKeyPrint = 0x2A;     // PRINT SCREEN 键
    public const byte vbKeyExecute = 0x2B;   // EXECUTE 键
    public const byte vbKeySnapshot = 0x2C;  // SNAPSHOT 键
    public const byte vbKeyDelete = 0x2E;    // Delete 键
    public const byte vbKeyHelp = 0x2F;      // HELP 键
    public const byte vbKeyNumlock = 0x90;   // NUM LOCK 键

    //常用键 字母键A到Z
    public const byte vbKeyA = 65;

    public const byte vbKeyB = 66;
    public const byte vbKeyC = 67;
    public const byte vbKeyD = 68;
    public const byte vbKeyE = 69;
    public const byte vbKeyF = 70;
    public const byte vbKeyG = 71;
    public const byte vbKeyH = 72;
    public const byte vbKeyI = 73;
    public const byte vbKeyJ = 74;
    public const byte vbKeyK = 75;
    public const byte vbKeyL = 76;
    public const byte vbKeyM = 77;
    public const byte vbKeyN = 78;
    public const byte vbKeyO = 79;
    public const byte vbKeyP = 80;
    public const byte vbKeyQ = 81;
    public const byte vbKeyR = 82;
    public const byte vbKeyS = 83;
    public const byte vbKeyT = 84;
    public const byte vbKeyU = 85;
    public const byte vbKeyV = 86;
    public const byte vbKeyW = 87;
    public const byte vbKeyX = 88;
    public const byte vbKeyY = 89;
    public const byte vbKeyZ = 90;

    //数字键盘0到9
    public const byte vbKey0 = 48;    // 0 键

    public const byte vbKey1 = 49;    // 1 键
    public const byte vbKey2 = 50;    // 2 键
    public const byte vbKey3 = 51;    // 3 键
    public const byte vbKey4 = 52;    // 4 键
    public const byte vbKey5 = 53;    // 5 键
    public const byte vbKey6 = 54;    // 6 键
    public const byte vbKey7 = 55;    // 7 键
    public const byte vbKey8 = 56;    // 8 键
    public const byte vbKey9 = 57;    // 9 键

    public const byte vbKeyNumpad0 = 0x60;    //0 键
    public const byte vbKeyNumpad1 = 0x61;    //1 键
    public const byte vbKeyNumpad2 = 0x62;    //2 键
    public const byte vbKeyNumpad3 = 0x63;    //3 键
    public const byte vbKeyNumpad4 = 0x64;    //4 键
    public const byte vbKeyNumpad5 = 0x65;    //5 键
    public const byte vbKeyNumpad6 = 0x66;    //6 键
    public const byte vbKeyNumpad7 = 0x67;    //7 键
    public const byte vbKeyNumpad8 = 0x68;    //8 键
    public const byte vbKeyNumpad9 = 0x69;    //9 键
    public const byte vbKeyMultiply = 0x6A;   // MULTIPLICATIONSIGN(*)键
    public const byte vbKeyAdd = 0x6B;        // PLUS SIGN(+) 键
    public const byte vbKeySeparator = 0x6C;  // ENTER 键
    public const byte vbKeySubtract = 0x6D;   // MINUS SIGN(-) 键
    public const byte vbKeyDecimal = 0x6E;    // DECIMAL POINT(.) 键
    public const byte vbKeyDivide = 0x6F;     // DIVISION SIGN(/) 键

    //F1到F12按键
    public const byte vbKeyF1 = 0x70;   //F1 键

    public const byte vbKeyF2 = 0x71;   //F2 键
    public const byte vbKeyF3 = 0x72;   //F3 键
    public const byte vbKeyF4 = 0x73;   //F4 键
    public const byte vbKeyF5 = 0x74;   //F5 键
    public const byte vbKeyF6 = 0x75;   //F6 键
    public const byte vbKeyF7 = 0x76;   //F7 键
    public const byte vbKeyF8 = 0x77;   //F8 键
    public const byte vbKeyF9 = 0x78;   //F9 键
    public const byte vbKeyF10 = 0x79;  //F10 键
    public const byte vbKeyF11 = 0x7A;  //F11 键
    public const byte vbKeyF12 = 0x7B;  //F12 键

    #endregion bVk参数 常量定义

    #region 引用win32api方法

    /// <summary>
    /// 导入模拟键盘的方法
    /// </summary>
    /// <param name="bVk" >按键的虚拟键值</param>
    /// <param name= "bScan" >扫描码，一般不用设置，用0代替就行</param>
    /// <param name= "dwFlags" >选项标志：0：表示按下，2：表示松开</param>
    /// <param name= "dwExtraInfo">一般设置为0</param>
    [DllImport("user32.dll")]
    public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

    public string GetRuntimeValue(string command) => throw new NotImplementedException();

    #endregion 引用win32api方法
}
