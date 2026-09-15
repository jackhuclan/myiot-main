using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace
{
    /// <summary>
    /// 料仓板料追溯详细查询请求
    /// </summary>
    public class GetSiloPanelTraceDetailListReq : Page
    {
        /// <summary>
        /// 主表ID（关联t_silo_panel_trace表的id字段）
        /// </summary>
        public virtual long? MasterId { get; set; }

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
        /// 板料类型 30100-生料 40100-熟料
        /// </summary>
        public virtual ProductStatus? ProductStatus { get; set; }
    }
}
