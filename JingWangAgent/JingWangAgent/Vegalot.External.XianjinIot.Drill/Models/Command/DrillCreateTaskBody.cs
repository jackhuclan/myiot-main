// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Drill.Models.Command
{
    internal class DrillCreateTaskBody : BaseCommandBody
    {
        /// <summary>
        /// 料号编码
        /// </summary>
        public string incodeNumber { get; set; } = string.Empty;

        /// <summary>
        /// 最小孔径
        /// </summary>
        public decimal minPoreSize { get; set; } = 0;

        /// <summary>
        /// 板长
        /// </summary>
        public decimal panelLength { get; set; } = 0;

        /// <summary>
        /// 板宽
        /// </summary>
        public decimal panelWidth { get; set; } = 0;

        /// <summary>
        /// 板厚
        /// </summary>
        public decimal panelThickness { get; set; } = 0;

        /// <summary>
        /// 铜厚
        /// </summary>
        public decimal copperThickness { get; set; } = 0;

        /// <summary>
        /// 执行钻孔任务的轴，根据任务轴进板
        /// </summary>
        public List<int> axleNums { get; set; } = new();

        /// <summary>
        /// 靶孔间距
        /// </summary>
        public decimal targetHoleSpacing { get; set; } = 0;

        /// <summary>
        /// 钻机编码
        /// </summary>
        public string drillingCode { get; set; } = string.Empty;

        /// <summary>
        /// 资料路径
        /// </summary>
        public string filePath { get; set; } = string.Empty;
    }
}
