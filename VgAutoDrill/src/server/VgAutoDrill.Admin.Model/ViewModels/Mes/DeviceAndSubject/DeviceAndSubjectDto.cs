namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndSubject
{
    public class DeviceAndSubjectDto : BaseDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int? DeviceId { get; set; }

        /// <summary>
        /// 点检保养项目id
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        /// 点检保养项目编码
        /// </summary>
        public virtual string? SubjectCode { get; set; }

        /// <summary>
        /// 点检保养项目名称
        /// </summary>
        public virtual string? SubjectName { get; set; }

        /// <summary>
        /// 点检保养项目标准
        /// </summary>
        public virtual string? Standard { get; set; }

    }
}
