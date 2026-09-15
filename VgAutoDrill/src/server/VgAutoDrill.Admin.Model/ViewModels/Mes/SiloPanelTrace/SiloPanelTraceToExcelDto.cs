using Npoi.Mapper.Attributes;
using System;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace
{
    /// <summary>
    /// 料仓板料追溯导出Excel DTO
    /// </summary>
    public class SiloPanelTraceToExcelDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Column("编号")]
        public long Id { get; set; }

        /// <summary>
        /// 料仓号
        /// </summary>
        [Column("料仓号")]
        public string SiloCode { get; set; } = string.Empty;

        /// <summary>
        /// 位置
        /// </summary>
        [Column("位置")]
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// 操作主题描述
        /// </summary>
        [Column("操作主题")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// 生料
        /// </summary>
        [Column("生料")]
        public string? UndrilledItem { get; set; }

        /// <summary>
        /// 熟料
        /// </summary>
        [Column("熟料")]
        public string? DrilledItem { get; set; }

        /// <summary>
        /// 是否存在多个熟料描述
        /// </summary>
        [Column("多个熟料种类")]
        public virtual string? HasMultipleDrilledDescription
        {
            get { return HasMultipleDrilled ? "是" : "否"; }
        }

        /// <summary>
        /// 是否存在多个熟料(不导出)
        /// </summary>
        [Ignore]
        public bool HasMultipleDrilled { get; set; }

        /// <summary>
        /// 调度记录表ID
        /// </summary>
        [Column("调度ID")]
        public string? ScheduleId { get; set; }

        /// <summary>
        /// 运输任务表ID
        /// </summary>
        [Column("运输任务ID")]
        public string? TransportationTaskId { get; set; }

        /// <summary>
        /// 料仓信息汇总
        /// </summary>
        [Column("料仓信息汇总")]
        public string SiloSummary { get; set; } = string.Empty;

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [Column("创建人")]
        public string? CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("创建时间")]
        public string CreateTime { get; set; } = string.Empty;

        /// <summary>
        /// 是否存在告警描述
        /// </summary>
        [Column("是否存在告警")]
        public virtual string? IsWarningDescription
        {
            get { return IsWarning ? "是" : "否"; }
        }

        /// <summary>
        /// 是否存在告警(不导出)
        /// </summary>
        [Ignore]
        public bool IsWarning { get; set; }

        /// <summary>
        /// 告警描述
        /// </summary>
        [Column("告警描述")]
        public string? WarningDescription { get; set; }

        /// <summary>
        /// 修改人姓名（不导出）
        /// </summary>
        [Ignore]
        public string? ModifierName { get; set; }

        /// <summary>
        /// 修改时间（不导出）
        /// </summary>
        [Ignore]
        public string? ModifyTime { get; set; }

        /// <summary>
        /// 状态描述（不导出）
        /// </summary>
        [Ignore]
        public virtual string? StatusDescription
        {
            get { return Status == 1 ? "启用" : "禁用"; }
        }

        /// <summary>
        /// 状态（不导出）
        /// </summary>
        [Ignore]
        public int Status { get; set; }
    }
}