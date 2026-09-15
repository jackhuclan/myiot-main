// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// RGV派送结果报告 EQP报告RGV派送结果给HOST
/// </summary>
[ReplyModelType(typeof(RGVDispatchResultReportReply))]
public class RGVDispatchResultReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    [XmlElement("job_id")]
    public string JobId { get; set; } = string.Empty;

    /// <summary>
    /// 1:完成 2:失败 3:不存在
    /// </summary>
    [XmlElement("result")]
    public string Result { get; set; } = string.Empty;
}
