namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceMain
{
    public class GetDeviceMaintainListReq : Page
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
        public virtual string? MaintainStatus { get; set; }
    }
}
