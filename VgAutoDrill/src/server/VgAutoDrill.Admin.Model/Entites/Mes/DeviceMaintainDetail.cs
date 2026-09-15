using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 设备维护明细
    /// </summary>
    [SugarTable("t_device_maintain_detail")]
    public class DeviceMaintainDetail : BaseEntity
    {
        /// <summary>
        /// 维护主表ID
        /// </summary>
        [SugarColumn(ColumnName = "master_id")]
        public int? MasterId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        [SugarColumn(ColumnName = "device_id")]
        public int? DeviceId { get; set; }

        /// <summary>
        /// 点检保养项目id
        /// </summary>
        [SugarColumn(ColumnName = "subject_id")]
        public int? SubjectId { get; set; }

        /// <summary>
        /// 点检保养项目编号
        /// </summary>
        [SugarColumn(ColumnName = "subject_code")]
        public virtual string? SubjectCode { get; set; }

        /// <summary>
        /// 点检保养项目名称
        /// </summary>
        [SugarColumn(ColumnName = "subject_name")]
        public virtual string? SubjectName { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        [SugarColumn(ColumnName = "maintain_result")]
        public virtual string? MaintainResult { get; set; }

        /// <summary>
        /// 改善措施
        /// </summary>
        [SugarColumn(ColumnName = "better_steps")]
        public virtual string? BetterSteps { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }
    }
}
