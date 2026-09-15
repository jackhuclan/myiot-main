// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgDeviceGateway.Devices.Common
{
    public class MaterialDistributionNotify
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// 子批次号
        /// </summary>
        public string? SonContainerName { get; set; }

        /// <summary>
        /// 工序组编码
        /// </summary>
        public string SpecGroup { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 任务类型:1 – 物料下机;  2 – 首检下机；其他类型待定
        /// </summary>
        public string TaskType { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }
    }

    public class MaterialDistributionNotifyResponse
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
}
