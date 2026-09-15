using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail
{
    public class MovePanelDetailDto
    {
        /// <summary>
        /// 所在位置编号(DeviceCode/SiloCode)
        /// </summary>
        public virtual string? LocationCode { get; set; }
        /// <summary>
        /// 0生料仓, 1钻机,2熟料仓
        /// </summary>
        public virtual int? Layer { get; set; }
        /// <summary>
        /// 板料所在位置（SplindleIndex-->从1开始/FloorNum-->从0开始）
        /// </summary>
        public virtual int? LocationIndex { get; set; }
        /// <summary>
        /// 关联设备类别
        /// </summary>
        public virtual DeviceKind? RelatedDeviceKind { get; set; }
    }
}
