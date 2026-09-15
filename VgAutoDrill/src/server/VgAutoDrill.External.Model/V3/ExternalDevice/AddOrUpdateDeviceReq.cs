using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.External.Model.V3.ExternalDevice
{
    public class AddOrUpdateDeviceReq : BaseAddOrUpdateWithTreeDto
    {
        /// <summary>
        /// 设备类型ID
        /// </summary>
        public int? DeviceTypeId { get; set; }

        /// <summary>
        /// 设备类型Code
        /// </summary>
        public virtual string? DeviceTypeCode { get; set; }

        /// <summary>
        /// 设备供应商
        /// </summary>
        public int? DeviceVendorId { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public virtual string? DeviceBrand { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public virtual string? DeviceSpec { get; set; }

        /// <summary>
        /// 工位，工作站
        /// </summary>
        public long? WorkStationId { get; set; }

        /// <summary>
        /// 维护周期天
        /// </summary>
        public int? MaintainPeriodDays { get; set; }

        /// <summary>
        /// 设备投产日期
        /// </summary>
        public DateTime? ProductionTime { get; set; }
        /// <summary>
        /// 下次设备维护时间
        /// </summary>
        public DateTime? LastMaintainTime { get; set; }

        /// <summary>
        /// 设备参数
        /// </summary>
        public virtual string? Parameters { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public virtual DeviceStatus? DeviceStatus { get; set; }
    }
}
