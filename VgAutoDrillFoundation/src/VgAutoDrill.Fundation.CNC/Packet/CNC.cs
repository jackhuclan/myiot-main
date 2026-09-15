using System.Xml.Serialization;

namespace VgAutoDrill.Fundation.CNC.Packet;

[XmlRoot(IsNullable = false, ElementName = "CNC")]
public class CNC
{
    #region Ignore
    [XmlIgnore]
    public int CmdType
    {
        get
        {
            if (_request != null)
            {
                return 1;
            }
            if (_execute != null)
            {
                return 2;
            }
            if (_advice != null)
            {
                return 3;
            }
            if (_adviceStart != null)
            {
                return 4;
            }
            if (_adviceStop != null)
            {
                return 5;
            }
            return 0;
        }
    }
    #endregion
    #region Value
    private string _value;
    [XmlAttribute("Value")]
    public string Value2 { get { return null; } set { _value = value; } }
    [XmlAttribute("VALUE")]
    public string Value { get { return _value; } set { _value = value; } }
    #endregion
    #region Request
    private CNCRequest _request;
    [XmlElement("Request")]
    public CNCRequest Request2 { get { return null; } set { _request = value; } }
    [XmlElement("REQUEST")]
    public CNCRequest Request { get { return _request; } set { _request = value; } }
    #endregion
    #region Execute
    private CNCExecute _execute;
    [XmlElement("Execute")]
    public CNCExecute Execute2 { get { return null; } set { _execute = value; } }
    [XmlElement("EXECUTE")]
    public CNCExecute Execute { get { return _execute; } set { _execute = value; } }
    #endregion
    #region Advice
    private CNCAdvice _advice;
    [XmlElement("Advise")]
    public CNCAdvice Advice2 { get { return null; } set { _advice = value; } }
    [XmlElement("ADVISE")]
    public CNCAdvice Advice { get { return _advice; } set { _advice = value; } }
    #endregion
    #region AdviceStart
    private CNCAdvice _adviceStart;
    [XmlElement("AdviseStart")]
    public CNCAdvice AdviceStart2 { get { return null; } set { _adviceStart = value; } }
    [XmlElement("ADVISESTART")]
    public CNCAdvice AdviceStart { get { return _adviceStart; } set { _adviceStart = value; } }
    #endregion
    #region AdviceStop
    private CNCAdvice _adviceStop;
    [XmlElement("AdviseStop")]
    public CNCAdvice AdviceStop2 { get { return null; } set { _adviceStop = value; } }
    [XmlElement("ADVISESTOP")]
    public CNCAdvice AdviceStop { get { return _adviceStop; } set { _adviceStop = value; } }
    #endregion

    public override string ToString()
    {
        return string.Empty;
    }
}
