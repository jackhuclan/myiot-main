using System.ComponentModel.DataAnnotations;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External
{
    public class ExternalAddOrUpdatePanelReq
    {
        /// <summary>
        /// 板料编号，
        /// 唯一性要求
        /// </summary>
        [Required]
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 物料代码
        /// </summary>
        [Required]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 板料位置
        /// </summary>
        public virtual string? BoardLocation { get; set; }

        /// <summary>
        /// 产品状态
        /// </summary>
        public virtual ProductStatus? ProductStatus { get; set; }

        /// <summary>
        /// 单叠数量
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
        public float PanelLength { get; set; }
        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        public virtual decimal? PinOffset { get; set; }
    }
}
