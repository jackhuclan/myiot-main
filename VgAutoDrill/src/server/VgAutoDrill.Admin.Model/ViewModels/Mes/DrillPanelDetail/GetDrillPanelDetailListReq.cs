using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail
{
    public class GetDrillPanelDetailListReq : Page
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public virtual string? DeviceCode { get; set; }
        /// <summary> 
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }
        /// <summary>
        /// 板料编码
        /// </summary>
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 该层板料类型:（38000-等待钻孔, 39000-正在钻孔,40000-钻机完成加工）
        /// </summary>
        public virtual ProductStatus? ProductStatus { get; set; }
    }
}
