using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Transportation;

public enum TransportationKind
{
    [Description("")]
    None = 0,
    /// <summary>
    /// 空仓
    /// </summary>
    [Description("空仓")]
    EmptySilo = 1,
    /// <summary>
    /// 生料
    /// </summary>
    [Description("生料")]
    Raw = 2,
    /// <summary>
    /// 熟料
    /// </summary>
    [Description("熟料")]
    Clinker = 3,
    /// <summary>
    /// 首件
    /// </summary>
    [Description("首件")]
    First = 4,
}
