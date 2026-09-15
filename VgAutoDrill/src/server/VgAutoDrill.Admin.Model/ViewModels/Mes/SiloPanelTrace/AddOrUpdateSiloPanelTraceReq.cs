namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace
{
    /// <summary>
    /// 料仓板料追溯新增/更新请求
    /// </summary>
    public class AddOrUpdateSiloPanelTraceReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public virtual string Code { get; set; } = string.Empty;

        /// <summary>
        /// 料仓号
        /// </summary>
        public virtual string SiloCode { get; set; } = string.Empty;

        /// <summary>
        /// 位置
        /// </summary>
        public virtual string Location { get; set; } = string.Empty;

        /// <summary>
        /// 料仓信息汇总
        /// </summary>
        public virtual string SiloSummary { get; set; } = string.Empty;

        /// <summary>
        /// 生料
        /// </summary>
        public virtual string UndrilledItem { get; set; } = string.Empty;

        /// <summary>
        /// 熟料
        /// </summary>
        public virtual string DrilledItem { get; set; } = string.Empty;

        /// <summary>
        /// 是否存在多个熟料
        /// </summary>
        public virtual bool HasMultipleDrilled { get; set; }

        /// <summary>
        /// 操作主题描述
        /// </summary>
        public virtual string Subject { get; set; } = string.Empty;

        /// <summary>
        /// 调度记录表ID
        /// </summary>
        public virtual long? ScheduleId { get; set; }

        /// <summary>
        /// 运输任务表ID
        /// </summary>
        public virtual long? TransportationTaskId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public virtual string CreatorName { get; set; } = string.Empty;

        /// <summary>
        /// 修改人姓名
        /// </summary>
        public virtual string ModifierName { get; set; } = string.Empty;

        /// <summary>
        /// 告警标识
        /// </summary>
        public virtual bool IsWarning { get; set; }

        /// <summary>
        /// 告警描述
        /// </summary>
        public virtual string? WarningDescription { get; set; }

        /// <summary>
        /// 关联记录ID
        /// </summary>
        public virtual long? ReferenceRecordId { get; set; }

        /// <summary>
        /// 是否需要校验SiloCode(0:不需要校验,1:需要校验)
        /// </summary>
        public virtual bool NeedValidateSiloCode { get; set; }
    }
}
