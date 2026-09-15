// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Drill.Models.Report;
internal class DrillDeviceInfoReportBody : ReportBaseBodyEntity
{
    /// <summary>
    /// 设备状态 0-异常，1-空闲，3-作业中，4-暂停
    /// </summary>
    public int deviceStatus { get; set; }

    /// <summary>
    /// 设备编码
    /// </summary>
    public string deviceCode { get; set; } = string.Empty;
    /// <summary>
    /// 设备位置信息(仅AGV 需要)
    /// </summary>
    public string location { get; set; } = string.Empty;

    /// <summary>
    /// 设备电量(仅AGV需要）
    /// </summary>
    public float quantityOfElectricity { get; set; } = 0.0f;


    /// <summary>
    /// 任务进度
    /// </summary>
    public int taskSchedule { get; set; }


}
