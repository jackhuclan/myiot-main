using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceMain
{
    public class DeviceMaintainToExcelDto
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        [Column("设备编码")]
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 点检保养项目编码
        /// </summary>
        [Column("点检保养项目编码")]
        public virtual string? SubJectCode { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        [Column("检验结果")]
        public virtual string? MaintainResult { get; set; }

        /// <summary>
        /// 改善措施
        /// </summary>
        [Column("改善措施")]
        public virtual string? BetterSteps { get; set; }

        /// <summary>
        /// 维护时间
        /// </summary>
        [Column("维护时间")]
        public virtual DateTime? MaintainTime { get; set; }

        /// <summary>
        /// 维护人
        /// </summary>
        [Column("维护人")]
        public virtual string? MaintainPerson { get; set; }

        /// <summary>
        /// 维护状态(-1/0/1)
        /// </summary>
        [Column("维护状态")]
        public virtual string? MaintainStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("备注")]
        public virtual string? Remark { get; set; }
    }
}
