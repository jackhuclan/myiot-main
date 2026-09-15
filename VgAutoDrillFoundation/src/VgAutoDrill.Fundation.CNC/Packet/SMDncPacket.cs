using System.Xml.Serialization;

namespace VgAutoDrill.Fundation.CNC.Packet;

[XmlRoot(ElementName = "SMDNCPACKET", Namespace = "")]
public class SMDncPacket
{
    #region Ignore
    [XmlIgnore]
    public int CmdType { get { return CNC.CmdType; } }
    #endregion
    #region Value
    private string _value;
    [XmlAttribute("Value")]
    public string Value2 { get { return null; } set { _value = value; } }
    [XmlAttribute("VALUE")]
    public string Value { get { return _value; } set { _value = value; } }
    #endregion
    [XmlElement("CNC", IsNullable = false)]
    public CNC CNC { get; set; } = new CNC();
    public override string ToString()
    {
        return XmlConventer.Serialize(this);
    }
}
