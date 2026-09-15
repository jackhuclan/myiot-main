using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder
{
    public class GetWorkOrderListReq : Page
    {
        public virtual string? Code { get; set; }

        public virtual string? Name { get; set; }

        /// <summary>
        /// 来源类型
        /// 订单或者库存
        /// </summary>
        public virtual string? OrderSource { get; set; }

        public virtual string? SourceCode { get; set; }

        public virtual long? ItemId { get; set; }

        public virtual string? ItemCode { get; set; }

        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string? ClientName { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public virtual string? ClientCode { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        public virtual long? ItemTypeId { get; set; }

        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 工单的状态(DRAFT/COMMITED)
        /// </summary>
        public virtual string? WorkOrderStatus { get; set; }
        /// <summary>
        /// 生产工单的状态(DRAFT/COMMITED/SCHEDULED/BEGIN/FINISH)
        /// </summary>
        public virtual List<ManuOrderStatusEnum>? ManuOrderStatusList { get; set; }

        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { set; get; } = -1;

        /// <summary>
        /// 叠板层数
        /// </summary>
        public virtual int? PanelCount { get; set; }

        /// <summary>
        /// 待排产叠数
        /// </summary>
        public virtual decimal? WadCount { get; set; }

        /// <summary>
        /// 孔数
        /// </summary>
        public virtual decimal? DrillCount { get; set; }

        /// <summary>
        /// 是否增加工单任务
        /// </summary>
        public byte? IsAddWorkOrder { get; set; }

        /// <summary>
        /// 数据排序方式
        /// </summary>
        public QueryOrderByEnum? QueryOrderBy { get; set; }

        /// <summary>
        /// 是否紧急插单
        /// </summary>
        public virtual int? IsUrgent { get; set; }

        /// <summary>
        /// 标记颜色
        /// </summary>
        public virtual string? RemarkColor { get; set; }

        /// <summary>
        /// 层数集合
        /// </summary>
        public virtual List<int>? LayerNumList { get; set; }

        /// <summary>
        /// 是否外部工单导入0/1
        /// </summary>
        public virtual int? IsExternal { get; set; }
    }
}
