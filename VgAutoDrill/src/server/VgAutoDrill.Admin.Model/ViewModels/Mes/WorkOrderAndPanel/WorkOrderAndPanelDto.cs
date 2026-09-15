using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndPanel
{
    public class WorkOrderAndPanelDto : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 工单
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// task
        /// </summary>
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public virtual string? ItemCode { get; set; }
        /// <summary>
        /// 板材编码
        /// </summary>
        public virtual string? PanelCode { get; set; }
        /// <summary>
        /// 片数
        /// </summary>
        public virtual int? Pcs { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public virtual string? BatchCode { get; set; }
        /// <summary>
        /// 产品状态
        /// </summary>
        public virtual ProductStatus ProductStatus { get; set; }
        /// <summary>
        /// 板料位置
        /// </summary>
        public virtual string? BoardLocation { get; set; }
        /// <summary>
        /// 板料宽度
        /// </summary>
        /// <summary>
        /// 板料宽度
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

        /// <summary>
        /// 外部工单
        /// </summary>
        public virtual string? ExternalWorkerOrder { get; set; }

    }
}
