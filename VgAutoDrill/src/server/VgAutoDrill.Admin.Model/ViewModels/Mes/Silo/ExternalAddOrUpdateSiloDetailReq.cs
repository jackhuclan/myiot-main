using System.ComponentModel.DataAnnotations;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Rack
{
    public class ExternalAddOrUpdateSiloDetailReq
    {
        // <summary>
        /// 料仓编码
        /// </summary>
        [Required]
        public virtual string? SiloCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        [Required]
        public virtual string? ItemCode { get; set; }
        /// <summary>
        /// 层号
        /// </summary>
        [Required]
        [Range(1, 30, ErrorMessage = "层号超出范围")]
        public virtual int? FloorNum { get; set; }
        /// <summary>
        /// 板料编码
        /// </summary>
        [Required]
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 板料类型 30100-生料 40100-熟料
        /// </summary>
        [Required]
        public virtual string? ProductStatus { get; set; }
        /// <summary>
        /// 每叠块数
        /// </summary>
        [Required]
        public virtual int? Pcs { get; set; }

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
