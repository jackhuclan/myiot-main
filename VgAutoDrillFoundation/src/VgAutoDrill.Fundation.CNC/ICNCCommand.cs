using Opc.Ua.Client;

using VgAutoDrill.Fundation.CNC.Model;
using VgAutoDrill.Fundation.CNC.Packet;

namespace VgAutoDrill.Fundation.CNC;

public interface ICNCCommand
{
    public void Init();

    public void Init(string ip, int port);

    public bool CNCCommandStatus();

    public string GetUserName();

    public VgCNCStatus GetCncStatus();

    public VgCNCStatus ParseStatusPara(string str);

    public VgCNCTools GetToolParameter();

    public void Start();

    public void StartTwo();

    public void SetSpindleMask(int maskNum);

    public void Shutdown();

    public void Stop();

    public VgCNCTools ParseToolParameter(string? str);

    void WriteCncNode<T>(string url, T value);

    T ReadCncNode<T>(string url);

    object[] CallMethodByNodeId(string tagParent, string tag, params object[] args);

    #region ScreenSaver

    public VgCNCScreenSaver GetScreenSaver();

    public VgCNCScreenSaver GetScreenText();

    ///
    /// 255 65535
    /// [94] 程 序 中 断
    ////Block 15  Step 1  Hole 3  Path 0.000
    ///

    public VgCNCScreenSaver ParseScreenSaverParameter(string str);

    //05/09/22 16:49:46 NO FILE LOADED * CNC ����ִ����;3300
    //23/05/10 14:21:35 DRILL.SM5 * STOP;3016
    public VgCncError GetCncError();

    public VgCncError ParseCncErrorParameter(string str);

    #region OutFlags

    public string GetOutFlags();

    #endregion OutFlags

    #region ACTProgram

    public string GetACTProgram();

    #endregion ACTProgram

    #region DiaFileNameWithDialog

    public string GetDiaFileNameWithDialog();

    #endregion DiaFileNameWithDialog

    #region LoadFile

    public void SetLoadFile(string filePath);

    #endregion LoadFile

    #endregion ScreenSaver

    #region ADVICE

    #region UserName

    public void RegisterUserNameAdvice(Func<SMDncPacket, Task> callback);

    #endregion UserName

    #region OutFlags

    public void RegisterOutFlags(Func<SMDncPacket, Task> callback);

    #endregion OutFlags

    #region CncError

    public void RegisterCncError(Func<SMDncPacket, Task> callback);

    #endregion CncError

    #region CncStatus

    public void RegisterCncStatus(Func<SMDncPacket, Task> callback);

    #endregion CncStatus

    #endregion ADVICE

    #region CncTools

    public void RegisterCncTools(Func<SMDncPacket, Task> callback);

    #endregion CncTools

    #region ScreenSaver

    public void RegisterScreenSaver(Func<SMDncPacket, Task> callback);

    #endregion ScreenSaver

    #region UserFlag

    public void SetUserFlag(int flag, int num);

    public void SetCncComand(string command);

    public string GetRuntimeString(string command);

    public void SetRuntimeValue(string command);

    #endregion UserFlag

    #region Output

    public string GetOutput(int num, bool flag = false);

    #endregion Output

    #region Iutput

    public string GetIutput(int num, bool flag = false);

    #endregion Iutput

    #region SeqFlag

    public string GetSeqFlag(int num);

    #endregion SeqFlag

    #region UserFlag

    public string GetUserFlag(int num);

    #endregion UserFlag

    public string GetAtpFileName();

    public void SetChangePage(string command);

    public void SetPcKey(string text);

    public string GetRuntimeValue(string command);

    public void AddSubscription(string key, string tag, Action<string, MonitoredItem, MonitoredItemNotificationEventArgs> callback);

    public void RemoveSubscription();

    public void Abort();

    //2024-06-24 zhushipeng 为了得到IIoT执行的结果
    public object[] SendStart();

    public object[] SendShutdown();

    public object[] SendStop();

    public object[] SendAbort();

    public object[] SendCncComand(string command);

    public object[] SendLoadFile(string filePath, string fileType);
}
