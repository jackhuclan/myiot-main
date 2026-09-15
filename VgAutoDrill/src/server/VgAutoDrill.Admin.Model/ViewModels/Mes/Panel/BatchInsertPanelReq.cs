using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace
{
    public class BatchInsertPanelReq
    {
        /// <summary>
        /// 从第几层开始（起始是0）
        /// </summary>
        public int BeginLayer { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; } = 1;
        /// <summary>
        /// 板料宽度
        /// </summary>
        public float PanelWidth { get; set; }
        /// <summary>
        /// 板料长度
        /// </summary>
        public float PanelLength { get; set; }
        /// <summary>
        /// 板料销钉距中心偏移量
        /// </summary>
        public float PinOffset { get; set; }
        /// <summary>
        /// 成品状态
        /// </summary>
        public ProductStatus ProductStatus { get; set; } = ProductStatus.EmptyPayload;
        /// <summary>
        /// 物料代码
        /// </summary>
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
        /// 单叠数量
        /// </summary>
        public virtual int? Pcs { get; set; }

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