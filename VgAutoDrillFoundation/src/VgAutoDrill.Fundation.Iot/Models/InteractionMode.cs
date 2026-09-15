using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

public enum InteractionMode
{
    /// <summary>
    /// 主动，呼叫设备主动和被呼叫设备如agv执行交互动作
    /// </summary>
    [Description("主动，呼叫设备主动和被呼叫设备如agv执行交互动作")]
    Active = 0,
    /// <summary>
    /// 被动，呼叫设备被动等待被呼叫设备如agv发出的指令，然后执行交互动作
    /// </summary>
    [Description("被动，呼叫设备被动等待被呼叫设备如agv发出的指令，然后执行交互动作")]
    Passive = 1,
}
