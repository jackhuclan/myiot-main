using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Rack
{
    public class RackQueryReq : Page
    {
        /// <summary>
        /// 所属分区编号
        /// </summary>
        public virtual string? WareHouseCode { get; set; }

        /// <summary>
        /// 所属分区id
        /// </summary>
        public virtual int? WareHouseId { get; set; }

        /// <summary>
        /// 料架编号
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 是否关联料仓
        /// </summary>
        public virtual Byte? IsHaveSilo { get; set; } = 0x0;

        /// <summary>
        /// 料仓编号
        /// </summary>
        public virtual string? SiloCode { get; set; }
        /// <summary>
        /// 设备类别
        /// </summary>
        public virtual List<DeviceKind>? DeviceKinds { get; set; }
        /// <summary>
        /// 位置码
        /// </summary>
        public virtual string? PositionCode { get; set; }
    }
}
