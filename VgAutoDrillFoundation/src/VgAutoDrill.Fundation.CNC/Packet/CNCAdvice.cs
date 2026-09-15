using System.Xml.Serialization;

namespace VgAutoDrill.Fundation.CNC.Packet;

public class CNCAdvice
{
    #region Ignore
    public int AdviceType
    {
        get
        {
            if (_version != null)
                return 1;
            if (DNCM != null)
                return 2;
            if (_dateTime != null)
                return 3;
            if (_sysStatus != null)
                return 4;
            if (_commStatus != null)
                return 5;
            if (_pcStatus != null)
                return 6;
            if (_cncError != null)
                return 7;
            if (_cncStatus != null)
                return 8;
            if (_cncTools != null)
                return 9;
            if (DUTY != null)
                return 10;
            if (_xpPosition != null)
                return 11;
            if (_screenSaver != null)
                return 12;
            if (_userName != null)
                return 13;
            if (_userLevel != null)
                return 14;
            if (OPID != null)
                return 15;
            if (_actProgram != null)
                return 16;
            if (_nextProgam != null)
                return 17;
            if (_batch != null)
                return 18;
            if (_inFlags != null)
                return 19;
            if (_outFlags != null)
                return 20;
            if (_ddeSequence != null)
                return 21;
            if (Flag0 != null)
                return 1000;
            if (Flag1 != null)
                return 1001;
            if (Flag2 != null)
                return 1002;
            if (Flag3 != null)
                return 1003;
            if (Flag4 != null)
                return 1004;
            if (Flag5 != null)
                return 1005;
            if (Flag6 != null)
                return 1006;
            if (Flag7 != null)
                return 1007;
            if (Flag8 != null)
                return 1008;
            if (Flag9 != null)
                return 1009;
            return -1;
        }
    }

    #endregion
    #region Value
    private string _value = CommFunc.REQUESTID;
    [XmlAttribute("Value")]
    public string Value2 { get { return null; } set { _value = value; } }
    [XmlAttribute("VALUE")]
    public string Value { get { return _value; } set { _value = value; } }
    #endregion
    #region Version
    private CNCData _version;
    [XmlElement("Version")]
    public CNCData Version2 { get { return null; } set { _version = value; } }
    [XmlElement("VERSION")]
    public CNCData Version { get { return _version; } set { _version = value; } }
    #endregion
    [XmlElement("DNCM")]
    public CNCData DNCM { get; set; }
    #region DateTime
    private CNCData _dateTime;
    [XmlElement("DateTime")]
    public CNCData DateTime2 { get { return null; } set { _dateTime = value; } }
    [XmlElement("DATETIME")]
    public CNCData DateTime { get { return _dateTime; } set { _dateTime = value; } }
    #endregion
    #region SysStatus
    private CNCData _sysStatus;
    [XmlElement("SysStatus")]
    public CNCData SysStatus2 { get { return null; } set { _sysStatus = value; } }
    [XmlElement("SYSSTATUS")]
    public CNCData SysStatus { get { return _sysStatus; } set { _sysStatus = value; } }
    #endregion
    #region CommStatus
    private CNCData _commStatus;
    [XmlElement("CommStatus")]
    public CNCData CommStatus2 { get { return null; } set { _commStatus = value; } }
    [XmlElement("COMMSTATUS")]
    public CNCData CommStatus { get { return _commStatus; } set { _commStatus = value; } }
    #endregion
    #region PCStatus
    private CNCData _pcStatus;
    [XmlElement("PCStatus")]
    public CNCData PCStatus2 { get { return null; } set { _pcStatus = value; } }
    [XmlElement("PCSTATUS")]
    public CNCData PCStatus { get { return _pcStatus; } set { _pcStatus = value; } }
    #endregion
    #region CNCError
    private CNCData _cncError;
    [XmlElement("CNCError")]
    public CNCData CNCError2 { get { return null; } set { _cncError = value; } }
    [XmlElement("CNCERROR")]
    public CNCData CNCError { get { return _cncError; } set { _cncError = value; } }
    #endregion
    #region CNCStatus
    private CNCData _cncStatus;
    [XmlElement("CNCStatus")]
    public CNCData CNCStatus2 { get { return null; } set { _cncStatus = value; } }
    [XmlElement("CNCSTATUS")]
    public CNCData CNCStatus { get { return _cncStatus; } set { _cncStatus = value; } }
    #endregion
    #region CNCTools
    private CNCData _cncTools;
    [XmlElement("CNCTools")]
    public CNCData CNCTools2 { get { return null; } set { _cncTools = value; } }
    [XmlElement("CNCTOOLS")]
    public CNCData CNCTools { get { return _cncTools; } set { _cncTools = value; } }
    #endregion
    [XmlElement("DUTY")]
    public CNCData DUTY { get; set; }
    #region XYPosition
    private CNCData _xpPosition;
    [XmlElement("XYPosition")]
    public CNCData XYPosition2 { get { return null; } set { _xpPosition = value; } }
    [XmlElement("XYPOSITION")]
    public CNCData XYPosition { get { return _xpPosition; } set { _xpPosition = value; } }
    #endregion
    #region ScreenSaver
    private CNCData _screenSaver;
    [XmlElement("ScreenSaver")]
    public CNCData ScreenSaver2 { get { return null; } set { _screenSaver = value; } }
    [XmlElement("SCREENSAVER")]
    public CNCData ScreenSaver { get { return _screenSaver; } set { _screenSaver = value; } }
    #endregion
    #region UserName
    private CNCData _userName;
    [XmlElement("UserName")]
    public CNCData UserName2 { get { return null; } set { _userName = value; } }
    [XmlElement("USERNAME")]
    public CNCData UserName { get { return _userName; } set { _userName = value; } }
    #endregion
    #region UserLevel
    private CNCData _userLevel;
    [XmlElement("UserLevel")]
    public CNCData UserLevel2 { get { return null; } set { _userLevel = value; } }
    [XmlElement("USERLEVEL")]
    public CNCData UserLevel { get { return _userLevel; } set { _userLevel = value; } }
    #endregion
    [XmlElement("OPID")]
    public CNCData OPID { get; set; }
    #region ACTProgram
    private CNCData _actProgram;
    [XmlElement("ACTProgram")]
    public CNCData ACTProgram2 { get { return null; } set { _actProgram = value; } }
    [XmlElement("ACTPROGRAM")]
    public CNCData ACTProgram { get { return _actProgram; } set { _actProgram = value; } }
    #endregion
    #region NextProgam
    private CNCData _nextProgam;
    [XmlElement("NextProgam")]
    public CNCData NextProgam2 { get { return null; } set { _nextProgam = value; } }
    [XmlElement("NEXTPROGAM")]
    public CNCData NextProgam { get { return _nextProgam; } set { _nextProgam = value; } }
    #endregion
    #region Batch
    private CNCData _batch;
    [XmlElement("Batch")]
    public CNCData Batch2 { get { return null; } set { _batch = value; } }
    [XmlElement("BATCH")]
    public CNCData Batch { get { return _batch; } set { _batch = value; } }
    #endregion
    #region InFlags
    private CNCData _inFlags;
    [XmlElement("InFlags")]
    public CNCData InFlags2 { get { return null; } set { _inFlags = value; } }
    [XmlElement("INFLAGS")]
    public CNCData InFlags { get { return _inFlags; } set { _inFlags = value; } }
    #endregion
    #region OutFlags
    private CNCData _outFlags;
    [XmlElement("OutFlags")]
    public CNCData OutFlags2 { get { return null; } set { _outFlags = value; } }
    [XmlElement("OUTFLAGS")]
    public CNCData OutFlags { get { return _outFlags; } set { _outFlags = value; } }
    #endregion
    #region DDESequence
    private CNCData _ddeSequence;
    [XmlElement("DDESequence")]
    public CNCData DDESequence2 { get { return null; } set { _ddeSequence = value; } }
    [XmlElement("DDESEQUENCE")]
    public CNCData DDESequence { get { return _ddeSequence; } set { _ddeSequence = value; } }
    #endregion
    [XmlElement("DDEPC")]
    public CNCData DDEPC { get; set; }
    [XmlElement("FTASTEN")]
    public CNCData FTASTEN { get; set; }
    [XmlElement("@0")]
    public CNCData Flag0 { get; set; }
    [XmlElement("@1")]
    public CNCData Flag1 { get; set; }
    [XmlElement("@2")]
    public CNCData Flag2 { get; set; }
    [XmlElement("@3")]
    public CNCData Flag3 { get; set; }
    [XmlElement("@4")]
    public CNCData Flag4 { get; set; }
    [XmlElement("@5")]
    public CNCData Flag5 { get; set; }
    [XmlElement("@6")]
    public CNCData Flag6 { get; set; }
    [XmlElement("@7")]
    public CNCData Flag7 { get; set; }
    [XmlElement("@8")]
    public CNCData Flag8 { get; set; }
    [XmlElement("@9")]
    public CNCData Flag9 { get; set; }

    public override string ToString()
    {
        return string.Empty;
    }

    public override int GetHashCode()
    {
        var adviceType = AdviceType;
        switch (adviceType)
        {
            case 1:
                return _version.GetHashCode();
            case 2:
                return DNCM.GetHashCode();
            case 3:
                return _dateTime.GetHashCode();
            case 4:
                return _sysStatus.GetHashCode();
            case 5:
                return _commStatus.GetHashCode();
            case 6:
                return _pcStatus.GetHashCode();
            case 7:
                return _cncError.GetHashCode();
            case 8:
                return _cncStatus.GetHashCode();
            case 9:
                return _cncTools.GetHashCode();
            case 10:
                return DUTY.GetHashCode();
            case 11:
                return _xpPosition.GetHashCode();
            case 12:
                return _screenSaver.GetHashCode();
            case 13:
                return _userName.GetHashCode();
            case 14:
                return _userLevel.GetHashCode();
            case 15:
                return OPID.GetHashCode();
            case 16:
                return _actProgram.GetHashCode();
            case 17:
                return _nextProgam.GetHashCode();
            case 18:
                return _batch.GetHashCode();
            case 19:
                return _inFlags.GetHashCode();
            case 20:
                return _outFlags.GetHashCode();
            case 21:
                return _ddeSequence.GetHashCode();
            case 1000:
                return Flag0.GetHashCode();
            case 1001:
                return Flag1.GetHashCode();
            case 1002:
                return Flag2.GetHashCode();
            case 1003:
                return Flag3.GetHashCode();
            case 1004:
                return Flag4.GetHashCode();
            case 1005:
                return Flag5.GetHashCode();
            case 1006:
                return Flag6.GetHashCode();
            case 1007:
                return Flag7.GetHashCode();
            case 1008:
                return Flag8.GetHashCode();
            case 1009:
                return Flag9.GetHashCode();

            default: break;
        }

        return 0;
    }
}
