using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Rack
{
    public class RackFullQueryReq
    {
        /// <summary>
        /// 所属分区编号
        /// </summary>
        public virtual string? WareHouseCode { get; set; }

        /// <summary>
        /// 料架编号
        /// </summary>
        public virtual string? Code { get; set; }

        public virtual List<string>? RouteCodes { get; set; }

        /// <summary>
        /// 料仓编号
        /// </summary>
        public virtual string? SiloCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 设备类别
        /// </summary>
        public virtual List<DeviceKind>? DeviceKinds { get; set; }
    }
}
