using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Rack
{
    public class AddOrUpdateSiloDetailReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 料仓编码
        /// </summary>
        public virtual string? SiloCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }
        /// <summary>
        /// 板料编码
        /// </summary>
        public virtual string? PanelCode { get; set; }
        /// <summary>
        ///  起始层
        /// </summary>
        public virtual int? BeginFloorNum { get; set; }
        /// <summary>
        /// 连续层数
        /// </summary>
        public virtual int? FloorCount { get; set; }
        /// <summary>
        /// 板料类型 30100-生料 40100-熟料
        /// </summary>
        public virtual ProductStatus? ProductStatus { get; set; }
        /// <summary>
        /// 每叠块数
        /// </summary>
        public virtual string? Pcs { get; set; }

        /// <summary>
        /// 板宽
        /// </summary>
        public virtual decimal? PanelWidth { get; set; }
        /// <summary>
        /// 板料长度
        /// </summary>
        public virtual decimal? PanelLength { get; set; }
        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        public virtual decimal? PinOffset { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public virtual string? LocationCode { get; set; }
    }
}
