using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Rack
{
    public class SiloInfo
    {
        /// <summary>
        /// 料仓编号
        /// </summary>
        public virtual string? SiloCode { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public string? Location { get; set; }
        /// <summary>
        /// 空层
        /// </summary>
        public string? EmptySilo { get; set; }
        /// <summary>
        /// 尺寸
        /// </summary>
        public string? Size { get; set; }
        /// <summary>
        /// 层数
        /// </summary>

        public int? FloorCount { get; set; }
        /// <summary>
        /// 料仓状态
        /// </summary>
        public virtual SiloStatus? SiloStatus { get; set; }
        /// <summary>
        /// 关联设备类别
        /// </summary>
        public virtual DeviceKind? RelatedDeviceKind { get; set; }
        /// <summary>
        /// 料仓载料信息
        /// </summary>
        public List<ExternalSiloDetailDto> SiloDetails { get; set; } = new List<ExternalSiloDetailDto>();
    }
}
