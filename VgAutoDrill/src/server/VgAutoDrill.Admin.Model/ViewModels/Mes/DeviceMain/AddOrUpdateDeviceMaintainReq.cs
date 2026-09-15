namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceMain
{
    public class AddOrUpdateDeviceMaintainReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int? DeviceId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public virtual string? DeviceName { get; set; }

        /// <summary>
        /// 维护时间
        /// </summary>
        public virtual DateTime? MaintainTime { get; set; }

        /// <summary>
        /// 维护人ID
        /// </summary>
        public int? MaintainId { get; set; }

        /// <summary>
        /// 维护人
        /// </summary>
        public virtual string? MaintainPerson { get; set; }

        /// <summary>
        /// 维护状态(-1/0/1)
        /// </summary>
        public virtual string? MaintainStatus { get; set; } = "-1";

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
    }
}