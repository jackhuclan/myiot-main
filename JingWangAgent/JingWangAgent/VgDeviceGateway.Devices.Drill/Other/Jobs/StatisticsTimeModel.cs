// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgDeviceGateway.Devices.Drill.Other.Jobs
{
    public class StatisticsTimeModel
    {
        public string Worktime { get; set; }=string.Empty;//工作时间
        public string Waittime { get; set; } = string.Empty;//等待时间
        public string Errortime { get; set; } = string.Empty;//异常时间
        public string Opentime { get; set; } = string.Empty;//开机时间
        public string Duty { get; set; } = string.Empty;//稼动率
        public string EndToStartTime { get; set; } = string.Empty;//结束到开始总时间
        public string CollectClearTime { get; set; } = string.Empty;//清洗夹头总时间
        public string DeviceId { get; set; } = string.Empty;//设备号
        public string DateString { get; set; } = string.Empty;//数据日期

    }
}
