// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// 制程/量测数据报告 EQP上报制程/量测数据给HOST
/// </summary>
[ReplyModelType(typeof(ProcessDataReportReply))]
public class ProcessDataReport : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 批号，板号 *如果以批号上报此栏位填批号 * 如果以板号上报此栏位填板号
    /// </summary>
    [XmlElement("job_id")]
    public string JobId { get; set; } = string.Empty;

    /// <summary>
    /// 数据采集列表
    /// </summary>
    [XmlElement("proc_data_list")]
    public List<ProcData> ProcDataList { get; set; } = new();

    [XmlRoot("proc_data")]
    public class ProcData
    {
        /// <summary>
        /// 数据名称
        /// </summary>
        [XmlElement("data_item")]
        public string DataItem { get; set; } = string.Empty;

        /// <summary>
        /// 数据值
        /// </summary>
        [XmlElement("data_value")]
        public string DataValue { get; set; } = string.Empty;
    }
}
