using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.LocationPanel
{
    public class AddOrUpdateLocationPanelReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 板料编号
        /// </summary>
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 物料代码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public virtual string? BatchCode { get; set; }
        /// <summary>
        /// 板料长度
        /// </summary>
        public float PanelLength { get; set; }
        /// <summary>
        /// 板料位置
        /// </summary>
        public virtual string? BoardLocation { get; set; }

        /// <summary>
        /// 产品状态
        /// </summary>
        public virtual ProductStatus ProductStatus { get; set; }

        /// <summary>
        /// 单叠数量
        /// </summary>
        public virtual int? Pcs { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public virtual long? StationId { get; set; }

        /// <summary>
        ///父类ID
        /// </summary>
        public virtual long? ParentId { get; set; }
        /// <summary>
        /// 板宽
        /// </summary>
        public virtual decimal? PanelWidth { get; set; }

        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        public virtual decimal? PinOffset { get; set; }

        /// <summary>
        /// 库位号
        /// </summary>
        public virtual string? LocationCode { get; set; }

        /// <summary>
        /// 料仓号
        /// </summary>
        public virtual string? SiloCode { get; set; }
    }
}
