using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace
{
    /// <summary>
    /// 料仓板料追溯详细新增/更新请求
    /// </summary>
    public class AddOrUpdateSiloPanelTraceDetailReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 主表ID（关联t_silo_panel_trace表的id字段）
        /// </summary>
        public virtual long MasterId { get; set; }

        /// <summary>
        /// 位置编码
        /// </summary>
        public virtual string? LocationCode { get; set; }

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
        /// 层数
        /// </summary>
        public virtual int? FloorNum { get; set; }

        /// <summary>
        /// 板料类型 30100-生料 40100-熟料
        /// </summary>
        public virtual ProductStatus ProductStatus { get; set; }

        /// <summary>
        /// 每叠块数
        /// </summary>
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

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public virtual string CreatorName { get; set; } = string.Empty;

        /// <summary>
        /// 修改人姓名
        /// </summary>
        public virtual string ModifierName { get; set; } = string.Empty;
    }
}
