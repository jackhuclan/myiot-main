using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment
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
        /// <summary>
        /// 轴数
        /// </summary>
        public virtual int? SpindleNum { get; set; }

        /// <summary>
        /// 设备类别
        /// </summary>
        public virtual DeviceKind? DeviceKind { get; set; }

        /// <summary>
        /// 工艺路线编码
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 交互位置
        /// </summary>
        public virtual InteractionPosition? InteractionPosition { get; set; }

        /// <summary>
        /// 机器尺寸
        /// </summary>
        public virtual string? MachineSize { get; set; }

        /// <summary>
        /// 自动机器还是手动机器
        /// </summary>
        public virtual bool? IsAuto { get; set; }

        /// <summary>
        /// 是否是DUO机器
        /// </summary>
        public virtual bool? IsDuo { get; set; }

        /// <summary>
        /// 生料库位
        /// </summary>
        public virtual string? RawLocationCode { get; set; }


        /// <summary>
        /// 熟料库位
        /// </summary>
        public virtual string? ClinkerLocationCode { get; set; }

        /// <summary>
        /// 最大板长
        /// </summary>
        public virtual int? MaxBoardLength { get; set; }

    }
}
