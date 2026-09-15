using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Silo
{
    public class SiloQueryReq : Page
    {
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string? Code { get; set; }
        /// <summary>
        /// 运载尺寸
        /// </summary>
        public virtual string? Size { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        public virtual int? FloorCount { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public virtual string? VendorName { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public virtual string? VendorCode { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public virtual string? Location { get; set; }

        /// <summary>
        /// 空料仓
        /// </summary>
        public virtual string? EmptySilo { get; set; }
        /// <summary>
        /// 料仓状态
        /// </summary>
        public virtual SiloStatus? SiloStatus { get; set; }
        /// <summary>
        /// 关联设备类别
        /// </summary>
        public virtual DeviceKind? RelatedDeviceKind { get; set; }
    }
}
