using SqlSugar;
using System;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 料仓板料追溯表
    /// </summary>
    [SugarTable("t_silo_panel_trace")]
    public class SiloPanelTrace : BaseEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        [SugarColumn(ColumnName = "code", Length = 50, IsNullable = false)]
        public virtual string Code { get; set; } = string.Empty;

        /// <summary>
        /// 料仓号
        /// </summary>
        [SugarColumn(ColumnName = "silo_code", Length = 50, IsNullable = false)]
        public virtual string SiloCode { get; set; } = string.Empty;

        /// <summary>
        /// 位置
        /// </summary>
        [SugarColumn(ColumnName = "location", Length = 100, IsNullable = false)]
        public virtual string Location { get; set; }

        /// <summary>
        /// 料仓信息汇总
        /// </summary>
        [SugarColumn(ColumnName = "silo_summary", ColumnDataType = "TEXT", IsNullable = false)]
        public virtual string SiloSummary { get; set; }

        /// <summary>
        /// 生料
        /// </summary>
        [SugarColumn(ColumnName = "undrilled_item", Length = 500, IsNullable = true)]
        public virtual string? UndrilledItem { get; set; }

        /// <summary>
        /// 熟料
        /// </summary>
        [SugarColumn(ColumnName = "drilled_item", Length = 500, IsNullable = true)]
        public virtual string? DrilledItem { get; set; }

        /// <summary>
        /// 是否存在多个熟料
        /// </summary>
        [SugarColumn(ColumnName = "has_multiple_drilled", IsNullable = false)]
        public virtual bool HasMultipleDrilled { get; set; }

        /// <summary>
        /// 操作主题描述
        /// </summary>
        [SugarColumn(ColumnName = "subject", Length = 500, IsNullable = false)]
        public virtual string Subject { get; set; }

        /// <summary>
        /// 调度记录表ID
        /// </summary>
        [SugarColumn(ColumnName = "schedule_id", IsNullable = true)]
        public virtual long? ScheduleId { get; set; }

        /// <summary>
        /// 运输任务表ID
        /// </summary>
        [SugarColumn(ColumnName = "transportation_task_id", IsNullable = true)]
        public virtual long? TransportationTaskId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [SugarColumn(ColumnName = "creator_name", Length = 100, IsNullable = true)]
        public virtual string? CreatorName { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        [SugarColumn(ColumnName = "modifier_name", Length = 100, IsNullable = true)]
        public virtual string? ModifierName { get; set; }

        /// <summary>
        /// 告警标识(0:无告警,1:有告警)
        /// </summary>
        [SugarColumn(ColumnName = "is_warning")]
        public virtual bool IsWarning { get; set; } = false;

        /// <summary>
        /// 告警描述
        /// </summary>
        [SugarColumn(ColumnName = "warning_description", Length = 500, IsNullable = true)]
        public virtual string? WarningDescription { get; set; }

        /// <summary>
        /// 关联记录ID
        /// </summary>
        [SugarColumn(ColumnName = "reference_record_id", IsNullable = true)]
        public virtual long? ReferenceRecordId { get; set; }

        /// <summary>
        /// 是否需要校验SiloCode(0:不需要校验,1:需要校验)
        /// </summary>
        [SugarColumn(ColumnName = "need_validate_silocode", IsNullable = false)]
        public virtual bool NeedValidateSiloCode { get; set; } = false;
    }
}
