using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.LocationDetail
{
    /// <summary>
    /// 库位明细DTO
    /// </summary>
    public class LocationDetailDto : BaseEntity
    {
        /// <summary>
        /// 库位编号
        /// </summary>
        public virtual string Code { get; set; } = string.Empty;

        /// <summary>
        /// 板料
        /// </summary>
        public virtual string Panel { get; set; } = string.Empty;

        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// 层数
        /// </summary>
        public virtual int? FloorNum { get; set; }

        /// <summary>
        /// 板料编码
        /// </summary>
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 该层板料类型:（0-空仓, 30100-生料,40100-熟料）
        /// </summary>
        public virtual ProductStatus? ProductStatus { get; set; }
        /// <summary>
        /// 产品状态描述
        /// </summary>
        public virtual string? ProductStatusDesc { get; set; }
        /// <summary>
        /// 每叠片数
        /// </summary>
        public virtual int? Pcs { get; set; }

        /// <summary>
        /// 板料宽度
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
        /// 创建人姓名
        /// </summary>
        public virtual string? CreatorName { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        public virtual string? ModifierName { get; set; }
    }
}
