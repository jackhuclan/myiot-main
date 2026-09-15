// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

/// <summary>
/// EQP回复关键参数给HOST
/// </summary>
public class TraceDataRequestReply : EapMessage.EapBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    [XmlElement("eqp_id")]
    public string EqpId { get; set; } = string.Empty;

    /// <summary>
    /// 数据采集列表
    /// </summary>
    [XmlElement("trace_data_list")]
    public List<TraceData> TraceDataList { get; set; } = new();

    [XmlRoot("trace_data")]
    public class TraceData
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
