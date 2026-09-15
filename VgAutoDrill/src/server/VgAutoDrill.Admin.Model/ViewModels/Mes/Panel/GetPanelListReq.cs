using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace
{
    public class GetPanelListReq : Page
    {
        public virtual string? ItemCode { set; get; }

        public virtual string? BatchCode { set; get; }

        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 产品状态
        /// </summary>
        public virtual ProductStatus? ProductStatus { get; set; }

        /// <summary>
        /// 产品状态集合（0-空位，1-空仓，20000-生料，40000-熟料）
        /// </summary>
        public virtual List<ProductStatus>? ProductStatusList { get; set; }

        /// <summary>
        /// 状态
        /// </summary>

        public virtual int Status { set; get; } = -1;

        public virtual string? LocationCode { set; get; }
    }
}
