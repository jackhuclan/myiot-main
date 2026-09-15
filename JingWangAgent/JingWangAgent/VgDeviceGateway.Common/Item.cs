// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgDeviceGateway.Devices.Common
{
    public class Item
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 板料长度
        /// </summary>
        public float PanelLength { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string? IncodeNumber { get; set; }
        /// <summary>
        /// 叠板层数，
        /// 从单元定义表获取，如果未定义，默认为1
        /// </summary>
        public decimal? PanelCount { get; set; }

        /// <summary>
        /// 钻带文件路径
        /// </summary>
        public virtual string? DrillFilePath { get; set; }

        /// <summary>
        /// 板料宽度
        /// </summary>
        public float PanelWidth { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public virtual string? UnitOfMeasure { get; set; } = "Panel";

        /// <summary>
        /// 产品大类编码
        /// </summary>
        public virtual string? ProductCategoryCode { get; set; } = "P01";

        /// <summary>
        /// 物料1产品2
        /// Item Or Product
        /// </summary>
        public virtual int? ItemOrProduct { get; set; } = 2;
    }
}
