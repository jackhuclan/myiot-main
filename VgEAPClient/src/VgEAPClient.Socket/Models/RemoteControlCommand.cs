// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// HOST发送远程控制指令给EQP
/// </summary>
[ReplyModelType(typeof(RemoteControlCommandReply))]
public class RemoteControlCommand : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 设备端口ID，如果没端口，此栏位为空
    /// L01-上料口#1
    /// L02-上料口#2
    /// L03- NG工位
    /// L04- 陪镀位
    /// ...
    /// U01-下料口#1
    /// U02-下料口#2
    /// U03-下料口#3
    /// U04- NG位
    /// U05- 收板机陪镀板工位
    /// U06- 检修NG位
    /// </summary>
    [XmlElement("port_id")]
    public string PortId { get; set; } = string.Empty;

    /// <summary>
    /// 1:Start 通知机台开始投板
    /// 2:Stop 通知机台停止板
    /// 3:Pause 通知机台暂停
    /// 4:Resume 通知机台复机
    /// 5:Open Buffer 通知机台启动暂存
    /// 6:Close Buffer 通知机台关闭暂存
    /// 7:Unload Carrier 通知机台下料
    /// 8:Panel OK 通知Panel正常
    /// 9:Panel NG通知Panel异常
    /// 10:AGVTransferComplete AGV派送完成
    /// 11: Inspect OK 首件OK
    /// 12: Inspect NG 首件NG
    /// 13:通知Panel为陪镀板
    /// 14:通知Panel为检修异常板
    /// 15：Continue：继续送料（开料线）
    /// </summary>
    [XmlElement("remote_command")]
    public string RemoteCommand { get; set; } = string.Empty;
}
