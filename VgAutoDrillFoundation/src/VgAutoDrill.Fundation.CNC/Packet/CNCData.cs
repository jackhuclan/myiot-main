using System.Xml.Serialization;

namespace VgAutoDrill.Fundation.CNC.Packet;

public class CNCData
{
    #region Value
    private string _value;
    [XmlAttribute("Value")]
    public string Value2 { get { return null; } set { _value = value; } }
    [XmlAttribute("VALUE")]
    public string Value { get { return _value; } set { _value = value; } }
    #endregion
    [XmlText]
    public string ItemValue { get; set; } = CommFunc.NULLVALUE;

    public override string ToString()
    {
        return string.Empty;
    }

    public override int GetHashCode()
    {
        return ItemValue?.GetHashCode() ?? 0;
    }
}
