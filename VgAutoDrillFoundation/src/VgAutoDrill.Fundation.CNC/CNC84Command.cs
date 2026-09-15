using System.Globalization;
using Microsoft.Extensions.Logging;
using Opc.Ua.Client;
using VgAutoDrill.Fundation.CNC.Model;
using VgAutoDrill.Fundation.CNC.Packet;

namespace VgAutoDrill.Fundation.CNC;

public class CNC84Command : ICNCCommand
{
    private CNCClient cncClient = null;

    private readonly ILogger<CNC84Command> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public CNC84Command(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CNC84Command>();
        _loggerFactory = loggerFactory;
    }

    public void Init(string ip, int port)
    {
        CommFunc.Init(_loggerFactory, ip, port);
        cncClient = CommFunc.GetClient(_loggerFactory);
    }

    public void Init()
    {
        CommFunc.Init(_loggerFactory);
        cncClient = CommFunc.GetClient(_loggerFactory);
    }

    public bool CNCCommandStatus()
    {
        if (cncClient == null)
        {
            return false;
        }
        return cncClient.IsConnect;
    }

    #region REQUEST

    #region UserName

    public string GetUserName()
    {
        string username = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Request = new CNCRequest();
        sMDncPacket.CNC.Request.UserName = new CNCData();
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
        username = receivePacket?.CNC.Request.UserName.ItemValue;
        return username;
    }

    #endregion UserName

    #region CncStatus

    public VgCNCStatus GetCncStatus()
    {
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Request = new CNCRequest();
        sMDncPacket.CNC.Request.CNCStatus = new CNCData();
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);

        string statusPara = receivePacket?.CNC.Request.CNCStatus.ItemValue;

        return ParseStatusPara(statusPara);
    }

    public VgCNCStatus ParseStatusPara(string str)
    {
        if (str == null)
        {
            return null;
        }
        ////   AR00:00:40,AH000003,AP016,ZS00111111,MOSTOP,EC0016,FNF:\1.dr
        VgCNCStatus vgCNCStatus = new VgCNCStatus();
        string[] splitStr = str.Split(',', StringSplitOptions.RemoveEmptyEntries);
        string strRunTime = splitStr.FirstOrDefault(item => item.StartsWith("AR", StringComparison.OrdinalIgnoreCase));
        if (strRunTime != null)
        {
            TimeSpan lTimeSpan = TimeSpan.Parse(strRunTime.Substring(2));
            vgCNCStatus.RunTime = Convert.ToInt32(lTimeSpan.TotalSeconds);
        }
        string strFinHoler = splitStr.FirstOrDefault(item => item.StartsWith("AH", StringComparison.OrdinalIgnoreCase));
        if (strFinHoler != null)
        {
            vgCNCStatus.FinHole = Convert.ToInt32(strFinHoler.Substring(2));
        }

        string strFinPer = splitStr.FirstOrDefault(item => item.StartsWith("AP", StringComparison.OrdinalIgnoreCase));
        if (strFinPer != null)
        {
            vgCNCStatus.FinPer = Convert.ToInt32(strFinPer.Substring(2));
            if (vgCNCStatus.FinPer > 100)
            {
                vgCNCStatus.FinPer = 100;
            }
        }

        string strSpindleStatus = splitStr.FirstOrDefault(item => item.StartsWith("ZS", StringComparison.OrdinalIgnoreCase));
        if (strSpindleStatus != null)
        {
            vgCNCStatus.SpindleStatus = strSpindleStatus.Substring(2);
            vgCNCStatus.SpindleStatusOriginalData = strSpindleStatus.Substring(2);
        }

        string strStatus = splitStr.FirstOrDefault(item => item.StartsWith("MO", StringComparison.OrdinalIgnoreCase));
        if (strSpindleStatus != null)
        {
            vgCNCStatus.Status = strStatus.Substring(2);
        }

        string strProgramName = splitStr.FirstOrDefault(item => item.StartsWith("FN", StringComparison.OrdinalIgnoreCase));
        if (strSpindleStatus != null)
        {
            vgCNCStatus.ProgramName = strProgramName.Substring(2);
        }

        return vgCNCStatus;
    }

    #endregion CncStatus

    #region tool

    public VgCNCTools GetToolParameter()
    {
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Request = new CNCRequest();
        sMDncPacket.CNC.Request.CNCTools = new CNCData();
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
        string toolPara = receivePacket?.CNC.Request.CNCTools.ItemValue;
        return ParseToolParameter(toolPara);
    }

    public void Start()
    {
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.CNCKey = new CNCData()
        {
            ItemValue = "START",
        };
        cncClient.SendCmd(sMDncPacket);
    }

    public void Shutdown()
    {
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.PCCommand = new CNCData()
        {
            ItemValue = "SHUTDOWN",
        };
        cncClient.SendCmd(sMDncPacket);
    }

    public void Stop()
    {
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.CNCKey = new CNCData()
        {
            ItemValue = "STOP",
        };
        cncClient.SendCmd(sMDncPacket);
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
        string strToolType = splitStr.Where(item => { return item.Length == 2; }).FirstOrDefault(item => { return item.StartsWith("E", StringComparison.OrdinalIgnoreCase); });
        if (strToolType != null)
        {
            vgCNCTools.ToolType = int.Parse(strToolType.Substring(1));
        }
        string strToolNumber = splitStr.FirstOrDefault(item => item.StartsWith("T", StringComparison.OrdinalIgnoreCase));
        if (strToolNumber != null)
        {
            vgCNCTools.ToolNumber = strToolNumber;
        }

        string strToolDiameter = splitStr.FirstOrDefault(item => item.StartsWith("D", StringComparison.OrdinalIgnoreCase));
        if (strToolDiameter != null)
        {
            vgCNCTools.ToolDiameter = float.Parse(strToolDiameter.Substring(1));
        }

        string strPresetToolLife = splitStr.FirstOrDefault(item => item.StartsWith("N", StringComparison.OrdinalIgnoreCase));
        if (strPresetToolLife != null)
        {
            vgCNCTools.PresetToolLife = float.Parse(strPresetToolLife.Substring(1));
        }
        string strRemainderToolLife = splitStr.FirstOrDefault(item => item.StartsWith("B", StringComparison.OrdinalIgnoreCase));
        if (strRemainderToolLife != null)
        {
            vgCNCTools.RemainderToolLife = float.Parse(strRemainderToolLife.Substring(1));
        }

        string strToolCount = splitStr.FirstOrDefault(item => item.StartsWith("EN", StringComparison.OrdinalIgnoreCase));
        if (strToolCount != null)
        {
            vgCNCTools.ToolCount = int.Parse(strToolCount.Substring(2));
        }
        string strExecutedToolNum = splitStr.FirstOrDefault(item => item.StartsWith("EU", StringComparison.OrdinalIgnoreCase));
        if (strExecutedToolNum != null)
        {
            vgCNCTools.ExecutedToolNum = int.Parse(strExecutedToolNum.Substring(2));
        }

        string strRequiredToolNum = splitStr.FirstOrDefault(item => item.StartsWith("ER", StringComparison.OrdinalIgnoreCase));
        if (strRequiredToolNum != null)
        {
            vgCNCTools.RequiredToolNum = int.Parse(strRequiredToolNum.Substring(2));
        }

        string strToolLifeCount = splitStr.FirstOrDefault(item => item.StartsWith("NN", StringComparison.OrdinalIgnoreCase));
        if (strToolLifeCount != null)
        {
            vgCNCTools.ToolLifeCount = float.Parse(strToolLifeCount.Substring(2));
        }
        string strUsedLifeCount = splitStr.FirstOrDefault(item => item.StartsWith("NU", StringComparison.OrdinalIgnoreCase));
        if (strUsedLifeCount != null)
        {
            vgCNCTools.UsedLifeCount = float.Parse(strUsedLifeCount.Substring(2));
        }

        string strRemainderLifeCount = splitStr.FirstOrDefault(item => item.StartsWith("NR", StringComparison.OrdinalIgnoreCase));
        if (strRemainderLifeCount != null)
        {
            vgCNCTools.RemainderLifeCount = float.Parse(strUsedLifeCount.Substring(2));
        }
        return vgCNCTools;
    }

    #endregion tool

    #region ScreenSaver

    public VgCNCScreenSaver GetScreenSaver()
    {
        string screenSaver = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Request = new CNCRequest();
        sMDncPacket.CNC.Request.ScreenSaver = new CNCData();
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
        screenSaver = receivePacket?.CNC.Request.ScreenSaver.ItemValue;
        return ParseScreenSaverParameter(screenSaver);
    }

    public VgCNCScreenSaver GetScreenText()
    {
        VgCNCScreenSaver vgCNCScreenSaver = GetScreenSaver();
        return vgCNCScreenSaver;
    }

    ///
    /// 255 65535
    /// [94] 程 序 中 断
    ////Block 15  Step 1  Hole 3  Path 0.000
    ///

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
                    vgCNCScreenSaver.BackColor = Convert.ToInt32(tmpstr[0]).ToString("X6");
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

    #endregion ScreenSaver

    #region CncError

    //05/09/22 16:49:46 NO FILE LOADED * CNC ����ִ����;3300
    //23/05/10 14:21:35 DRILL.SM5 * STOP;3016
    public VgCncError GetCncError()
    {
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Request = new CNCRequest();
        sMDncPacket.CNC.Request.CNCError = new CNCData();
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);

        string statusPara = receivePacket?.CNC.Request.CNCError.ItemValue;

        return ParseCncErrorParameter(statusPara);
    }

    public VgCncError ParseCncErrorParameter(string str)
    {
        if (str == null)
        {
            return null;
        }
        VgCncError vgCncError = new VgCncError();

        vgCncError.ErrorID = GetEventId(str);
        vgCncError.CurrentTime = ParseTime(str);
        vgCncError.DateString = ParseDate(str);
        vgCncError.ProgrameName = GetProgrameName(str);
        vgCncError.ErrorMessage = GetErrorMessage(str);

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
            string[] SplitStr = str.Split(';');
            if (SplitStr.Count() > 1)
            {
                return Convert.ToInt32(SplitStr[SplitStr.Count() - 1]);
            }
        }

        return 0;
    }

    #endregion CncError

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
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Request = new CNCRequest();
        sMDncPacket.CNC.Request.OutFlags = new CNCData();
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
        outflags = receivePacket?.CNC.Request.OutFlags.ItemValue;
        return outflags;
    }

    #endregion OutFlags

    #region ACTProgram

    public string GetACTProgram()
    {
        string actprogram = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Request = new CNCRequest();
        sMDncPacket.CNC.Request.ACTProgram = new CNCData();
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
        actprogram = receivePacket?.CNC.Request.ACTProgram.ItemValue;
        return actprogram;
    }

    #endregion ACTProgram

    #region DiaFileNameWithDialog

    public string GetDiaFileNameWithDialog()
    {
        string diaFileNameWithDialog = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.RuntimeString = new CNCData()
        {
            ItemValue = "%S(DiaFileNameWithDialog,0)"
        };
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
        diaFileNameWithDialog = receivePacket?.CNC.Execute.RuntimeString.Value;
        if (diaFileNameWithDialog != null && diaFileNameWithDialog.StartsWith("1:") && diaFileNameWithDialog.Length > 2)
        {
            diaFileNameWithDialog = diaFileNameWithDialog.Substring(2);
        }
        return diaFileNameWithDialog;
    }

    #endregion DiaFileNameWithDialog

    #region AtpFileName

    public string GetAtpFileName()
    {
        string atpFileNameWithDialog = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.RuntimeString = new CNCData()
        {
            ItemValue = "%S(ATPFileNameWithDialog,0)"
        };
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
        atpFileNameWithDialog = receivePacket?.CNC.Execute.RuntimeString.Value ?? "";
        if (atpFileNameWithDialog.StartsWith("1:") && atpFileNameWithDialog.Length > 2)
        {
            atpFileNameWithDialog = atpFileNameWithDialog.Substring(2);
        }
        return atpFileNameWithDialog;
    }

    #endregion AtpFileName

    #region LoadFile

    public void SetLoadFile(string filePath)
    {
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.Command = new CNCData()
        {
            ItemValue = "CNCCOMMAND=@LF," + filePath
        };
        cncClient.SendCmd(sMDncPacket);
    }

    #endregion LoadFile

    #endregion REQUEST

    #region ADVICE

    #region UserName

    public void RegisterUserNameAdvice(Func<SMDncPacket, Task> callback)
    {
        string username = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.AdviceStart = new CNCAdvice();
        sMDncPacket.CNC.AdviceStart.UserName = new CNCData();
        cncClient.RegisterAdvice(sMDncPacket, callback);
    }

    #endregion UserName

    #region OutFlags

    public void RegisterOutFlags(Func<SMDncPacket, Task> callback)
    {
        string outflags = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.AdviceStart = new CNCAdvice();
        sMDncPacket.CNC.AdviceStart.OutFlags = new CNCData();
        cncClient.RegisterAdvice(sMDncPacket, callback);
    }

    #endregion OutFlags

    #region CncError

    public void RegisterCncError(Func<SMDncPacket, Task> callback)
    {
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.AdviceStart = new CNCAdvice();
        sMDncPacket.CNC.AdviceStart.CNCError = new CNCData();
        cncClient.RegisterAdvice(sMDncPacket, callback);
    }

    #endregion CncError

    #region CncStatus

    public void RegisterCncStatus(Func<SMDncPacket, Task> callback)
    {
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.AdviceStart = new CNCAdvice();
        sMDncPacket.CNC.AdviceStart.CNCStatus = new CNCData();
        cncClient.RegisterAdvice(sMDncPacket, callback);
    }

    #endregion CncStatus

    #endregion ADVICE

    #region CncTools

    public void RegisterCncTools(Func<SMDncPacket, Task> callback)
    {
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.AdviceStart = new CNCAdvice();
        sMDncPacket.CNC.AdviceStart.CNCTools = new CNCData();
        cncClient.RegisterAdvice(sMDncPacket, callback);
    }

    #endregion CncTools

    #region ScreenSaver

    public void RegisterScreenSaver(Func<SMDncPacket, Task> callback)
    {
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.AdviceStart = new CNCAdvice();
        sMDncPacket.CNC.AdviceStart.ScreenSaver = new CNCData();
        cncClient.RegisterAdvice(sMDncPacket, callback);
    }

    #endregion ScreenSaver

    #region UserFlag

    public void SetUserFlag(int flag, int num)
    {
        string username = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.RuntimeValue = new CNCData() { ItemValue = $"SetVal({flag},%UserFlag({num}))" };
        cncClient.SendCmd(sMDncPacket);
    }

    public string GetUserFlag(int num)
    {
        string output = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.RuntimeValue = new CNCData() { ItemValue = $"UserFlag({num})" };
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
        output = receivePacket?.CNC.Execute.RuntimeValue.Value;
        return output;
    }

    public void SetCncComand(string command)
    {
        string username = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.CNCCommand = new CNCData() { ItemValue = command };
        cncClient.SendCmd(sMDncPacket);
    }

    public string GetRuntimeString(string command)
    {
        string output = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.RuntimeString = new CNCData() { ItemValue = command };
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
        output = receivePacket?.CNC.Execute.RuntimeString.Value;
        return output;
    }

    public void SetRuntimeValue(string command)
    {
        string username = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.RuntimeValue = new CNCData() { ItemValue = command };
        cncClient.SendCmd(sMDncPacket);
    }

    #endregion UserFlag

    #region Output

    public string GetOutput(int num, bool flag = false)
    {
        if (flag)
        {
            string output = string.Empty;
            SMDncPacket sMDncPacket = new SMDncPacket();
            sMDncPacket.Value = "1";
            sMDncPacket.CNC.Value = "1";
            sMDncPacket.CNC.Execute = new CNCExecute();
            sMDncPacket.CNC.Execute.RuntimeValue = new CNCData()
            {
                ItemValue = $"HSYS55_Output({num})"
            };
            SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
            output = receivePacket?.CNC.Execute.RuntimeValue.Value;
            return output;
        }
        else
        {
            string output = string.Empty;
            SMDncPacket sMDncPacket = new SMDncPacket();
            sMDncPacket.Value = "1";
            sMDncPacket.CNC.Value = "1";
            sMDncPacket.CNC.Execute = new CNCExecute();
            sMDncPacket.CNC.Execute.RuntimeString = new CNCData()
            {
                ItemValue = $"%S(HSYS55_Output,{num})"
            };
            SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
            output = receivePacket?.CNC.Execute.RuntimeString.Value;
            return output;
        }
    }

    #endregion Output

    public string GetIutput(int num, bool flag = false)
    {
        if (flag)
        {
            string iutput = string.Empty;
            SMDncPacket sMDncPacket = new SMDncPacket();
            sMDncPacket.Value = "1";
            sMDncPacket.CNC.Value = "1";
            sMDncPacket.CNC.Execute = new CNCExecute();
            sMDncPacket.CNC.Execute.RuntimeValue = new CNCData()
            {
                ItemValue = $"HSYS55_Input({num})"
            };
            SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
            iutput = receivePacket?.CNC.Execute.RuntimeValue.Value;
            return iutput;
        }
        else
        {
            string iutput = string.Empty;
            SMDncPacket sMDncPacket = new SMDncPacket();
            sMDncPacket.Value = "1";
            sMDncPacket.CNC.Value = "1";
            sMDncPacket.CNC.Execute = new CNCExecute();
            sMDncPacket.CNC.Execute.RuntimeString = new CNCData()
            {
                ItemValue = $"%S(HSYS55_Input,{num})"
            };
            SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
            iutput = receivePacket?.CNC.Execute.RuntimeString.Value;
            return iutput;
        }
    }

    #region SeqFlag

    public string GetSeqFlag(int num)
    {
        string seq = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.RuntimeValue = new CNCData()
        {
            ItemValue = $"seqFlag({num})"
        };
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
        seq = receivePacket?.CNC.Execute.RuntimeValue.Value;
        return seq;
    }

    #endregion SeqFlag

    public void SetChangePage(string command)
    {
        string username = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.ChangeClient = new CNCData() { ItemValue = command };
        cncClient.SendCmd(sMDncPacket);
    }

    #region PcKey

    public void SetPcKey(string text)
    {
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.PCKey = new CNCData()
        {
            ItemValue = text
        };
        cncClient.SendCmd(sMDncPacket);
    }

    public void StartTwo()
    {
        throw new NotImplementedException();
    }

    public void SetSpindleMask(int maskNum) => throw new NotImplementedException();

    public void WriteCncNode<T>(string url, T value) => throw new NotImplementedException();

    public T ReadCncNode<T>(string url) => throw new NotImplementedException();

    public object[] CallMethodByNodeId(string tagParent, string tag, params object[] args) => throw new NotImplementedException();

    public void AddSubscription(string key, string tag, Action<string, MonitoredItem, MonitoredItemNotificationEventArgs> callback) => throw new NotImplementedException();

    public void RemoveSubscription() => throw new NotImplementedException();

    #endregion PcKey

    public string GetRuntimeValue(string command)
    {
        string iutput = string.Empty;
        SMDncPacket sMDncPacket = new SMDncPacket();
        sMDncPacket.Value = "1";
        sMDncPacket.CNC.Value = "1";
        sMDncPacket.CNC.Execute = new CNCExecute();
        sMDncPacket.CNC.Execute.RuntimeValue = new CNCData()
        {
            ItemValue = command
        };
        SMDncPacket receivePacket = cncClient.SendAndRecive(sMDncPacket);
        iutput = receivePacket?.CNC.Execute.RuntimeValue.Value;
        return iutput;
    }

    //2024-06-24 zhushipeng 为了得到IIoT执行的结果
    public void Abort() => throw new NotImplementedException();

    public object[] SendStart() => throw new NotImplementedException();

    public object[] SendShutdown() => throw new NotImplementedException();

    public object[] SendStop() => throw new NotImplementedException();

    public object[] SendAbort() => throw new NotImplementedException();

    public object[] SendCncComand(string command) => throw new NotImplementedException();

    public object[] SendLoadFile(string filePath, string fileType) => throw new NotImplementedException();
}
