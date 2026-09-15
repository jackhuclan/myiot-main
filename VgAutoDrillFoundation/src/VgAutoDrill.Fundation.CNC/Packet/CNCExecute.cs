using System.Xml.Serialization;

namespace VgAutoDrill.Fundation.CNC.Packet;

[XmlRoot(ElementName = "EXECUTE")]
public class CNCExecute
{
    #region Value
    private string _value = CommFunc.REQUESTID;
    [XmlAttribute("Value")]
    public string Value2 { get { return null; } set { _value = value; } }
    [XmlAttribute("VALUE")]
    public string Value { get { return _value; } set { _value = value; } }
    #endregion
    #region Progam
    private CNCData _progam;
    [XmlElement("Program")]
    public CNCData Progam2 { get { return null; } set { _progam = value; } }
    [XmlElement("PROGRAM")]
    public CNCData Progam { get { return _progam; } set { _progam = value; } }
    #endregion
    #region ClrNext
    private CNCData _clrNext;
    [XmlElement("ClrNext")]
    public CNCData ClrNext2 { get { return null; } set { _clrNext = value; } }
    [XmlElement("CLRNEXT")]
    public CNCData ClrNext { get { return _clrNext; } set { _clrNext = value; } }
    #endregion
    #region InflgSet
    private CNCData _inflgSet;
    [XmlElement("inflgSet")]
    public CNCData InflgSet2
    {
        get { return null; }
        set { _inflgSet = value; }
    }
    [XmlElement("INFLGSET")]
    public CNCData InflgSet
    {
        get { return _inflgSet; }
        set { _inflgSet = value; }
    }
    #endregion
    #region InflgClr
    private CNCData _inflgClr;
    [XmlElement("InflgClr")]
    public CNCData InflgClr2 { get { return null; } set { _inflgClr = value; } }
    [XmlElement("INFLGCLR")]
    public CNCData InflgClr { get { return _inflgClr; } set { _inflgClr = value; } }
    #endregion
    #region Command
    private CNCData _command;
    [XmlElement("Command")]
    public CNCData Command2 { get { return null; } set { _command = value; } }
    [XmlElement("COMMAND")]
    public CNCData Command { get { return _command; } set { _command = value; } }
    #endregion
    #region CNCCommand
    private CNCData _cncCommand;
    [XmlElement("CNCCOMMAND")]
    public CNCData CNCCommand { get { return _cncCommand; } set { _cncCommand = value; } }
    [XmlElement("CNCCommand")]
    public CNCData CNCCommand2 { get { return null; } set { _cncCommand = value; } }
    #endregion
    #region CNCKey
    private CNCData _cncKey;
    [XmlElement("CNCKEY")]
    public CNCData CNCKey { get { return _cncKey; } set { _cncKey = value; } }
    [XmlElement("CNCKey")]
    public CNCData CNCKey2 { get { return null; } set { _cncKey = value; } }
    #endregion
    #region PCCommand
    private CNCData _pcCommand;
    [XmlElement("PCCOMMAND")]
    public CNCData PCCommand { get { return _pcCommand; } set { _pcCommand = value; } }
    [XmlElement("PCCommand")]
    public CNCData PCCommand2 { get { return null; } set { _pcCommand = value; } }
    #endregion
    #region PCKey
    private CNCData _pcKey;
    [XmlElement("PCKEY")]
    public CNCData PCKey { get { return _pcKey; } set { _pcKey = value; } }
    [XmlElement("PCKey")]
    public CNCData PCKey2 { get { return null; } set { _pcKey = value; } }
    #endregion
    #region ChangeGroup
    private CNCData _changeGroup;
    [XmlElement("CHANGEGROUP")]
    public CNCData ChangeGroup
    {
        get { return _changeGroup; }
        set { _changeGroup = value; }
    }
    [XmlElement("ChangeGroup")]
    public CNCData ChangeGroup2
    {
        get { return null; }
        set { _changeGroup = value; }
    }
    #endregion
    #region ChangeClient
    private CNCData _changeClient;
    [XmlElement("ChangeClient")]
    public CNCData ChangeClient2
    {
        get { return null; }
        set { _changeClient = value; }
    }
    [XmlElement("CHANGECLIENT")]
    public CNCData ChangeClient
    {
        get { return _changeClient; }
        set { _changeClient = value; }
    }
    #endregion
    #region ChangePage
    private CNCData _changePage;
    [XmlElement("CHANGEPAGE")]
    public CNCData ChangePage
    {
        get { return _changePage; }
        set { _changePage = value; }
    }
    [XmlElement("ChangePage")]
    public CNCData ChangePage2
    {
        get { return null; }
        set { _changePage = value; }
    }
    #endregion
    #region StartExe
    private CNCData _startExe;
    [XmlElement("STARTEXE")]
    public CNCData StartEXE
    {
        get { return _startExe; }
        set { _startExe = value; }
    }
    [XmlElement("StartExe")]
    public CNCData StartEXE2
    {
        get { return null; }
        set { _startExe = value; }
    }
    #endregion
    #region RuntimeValue
    private CNCData _runtimeValue;
    [XmlElement("RUNTIMEVALUE")]
    public CNCData RuntimeValue
    {
        get { return _runtimeValue; }
        set { _runtimeValue = value; }
    }
    [XmlElement("RuntimeValue")]
    public CNCData RuntimeValue2
    {
        get { return null; }
        set { _runtimeValue = value; }
    }
    #endregion
    #region RuntimeString
    private CNCData _runtimeString;
    [XmlElement("RUNTIMESTRING")]
    public CNCData RuntimeString
    {
        get { return _runtimeString; }
        set { _runtimeString = value; }
    }
    [XmlElement("RuntimeString")]
    public CNCData RuntimeString2
    {
        get { return null; }
        set { _runtimeString = value; }
    }
    #endregion
    #region RuntimeComm
    private CNCData _runtimeComm;
    [XmlElement("RUNTIMECOMM")]
    public CNCData RuntimeComm
    {
        get { return _runtimeComm; }
        set { _runtimeComm = value; }
    }
    [XmlElement("RuntimeComm")]
    public CNCData RuntimeComm2
    {
        get { return null; }
        set { _runtimeComm = value; }
    }
    #endregion
    #region Load
    private CNCData _load;
    [XmlElement("LOAD")]
    public CNCData Load
    {
        get { return _load; }
        set { _load = value; }
    }
    [XmlElement("Load")]
    public CNCData Load2
    {
        get { return null; }
        set { _load = value; }
    }
    #endregion
    #region Save
    private CNCData _save;
    [XmlElement("SAVE")]
    public CNCData Save
    {
        get { return _save; }
        set { _save = value; }
    }
    [XmlElement("Save")]
    public CNCData Save2
    {
        get { return null; }
        set { _save = value; }
    }
    #endregion
    #region SetHandle
    private CNCData _setHandle;
    [XmlElement("SETHANDLE")]
    public CNCData SetHandle
    {
        get { return _setHandle; }
        set { _setHandle = value; }
    }
    [XmlElement("SetHandle")]
    public CNCData SetHandle2
    {
        get { return null; }
        set { _setHandle = value; }
    }
    #endregion

    public override string ToString()
    {
        return string.Empty;
    }
}

