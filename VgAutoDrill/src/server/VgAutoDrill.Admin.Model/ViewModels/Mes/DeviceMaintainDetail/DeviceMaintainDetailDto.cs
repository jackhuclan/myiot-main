namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceMaintainDetail
{
    public class DeviceMaintainDetailDto : BaseDto
    {
        /// <summary>
        /// 维护主表ID
        /// </summary>
        public int? MasterId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public int? DeviceId { get; set; }

        /// <summary>
        /// 点检保养项目id
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        /// 点检保养项目编号
        /// </summary>
        public virtual string? SubjectCode { get; set; }

        /// <summary>
        /// 点检保养项目名称
        /// </summary>
        public virtual string? SubjectName { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        public virtual string? MaintainResult { get; set; }

        /// <summary>
        /// 改善措施
        /// </summary>
        public virtual string? BetterSteps { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
    }
}
