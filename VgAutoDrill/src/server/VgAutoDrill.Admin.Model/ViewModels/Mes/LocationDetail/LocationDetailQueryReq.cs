using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.LocationDetail
{
    /// <summary>
    /// 库位明细查询请求
    /// </summary>
    public class LocationDetailQueryReq : Page
    {
        /// <summary>
        /// 库位编号
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 板料
        /// </summary>
        public virtual string? Panel { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

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
    }
}
