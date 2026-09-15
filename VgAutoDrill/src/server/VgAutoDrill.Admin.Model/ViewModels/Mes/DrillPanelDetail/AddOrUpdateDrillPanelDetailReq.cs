using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail
{
    public class AddOrUpdateDrillPanelDetailReq : BaseAddOrUpdateDto
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
        /// <summary>
        /// 每叠块数
        /// </summary>
        public virtual int? Pcs { get; set; }
        /// <summary>
        /// 板宽
        /// </summary>
        public virtual decimal? PanelWidth { get; set; }

        /// <summary>
        /// 0生料仓, 1钻机,2熟料仓
        /// </summary>
        public virtual int Layer { get; set; } = 1;

        /// <summary>
        /// 轴
        /// </summary>
        public virtual int? SplindleIndex { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        public virtual decimal? PinOffset { get; set; }

    }
}
