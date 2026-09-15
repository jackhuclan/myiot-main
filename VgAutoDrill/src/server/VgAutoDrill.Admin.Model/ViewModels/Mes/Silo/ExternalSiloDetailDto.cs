using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Silo
{
    public class ExternalSiloDetailDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }
        /// <summary>
        /// 板料编码
        /// </summary>
        public virtual string? PanelCode { get; set; }
        /// <summary>
        /// 层号
        /// </summary>
        public virtual int? FloorNum { get; set; }

        /// <summary>
        /// 板料类型 30100-生料 40100-熟料
        /// </summary>
        public virtual ProductStatus ProductStatus { get; set; }
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
    }
}
