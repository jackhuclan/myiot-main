using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder
{
    public class WorkOrderToExcelDto
    {
        [Column("生产工单编码（必填）")]
        public string Code { set; get; }

        [Column("生产工单名称（必填）")]
        public string Name { set; get; }

        [Column("父对象Id(0表示是根对象)（必填）")]
        public long ParentId { get; set; }

        [Column("物料产品类型Id（必填）")]
        public long ItemTypeId { get; set; }

        [Column("产品ID")]
        public int ItemId { get; set; }

        [Column("产品名称")]
        public string ItemName { get; set; }

        [Column("产品编号")]
        public string ItemCode { get; set; }

        [Column("来源类型（订单/库存）")]
        public string OrderSource { get; set; }

        [Column("来源单据")]
        public string SourceCode { get; set; }

        [Column("批次号")]
        public string BatchCode { get; set; }

        [Column("规格型号")]
        public string Specification { get; set; }

        [Column("单位")]
        public string UnitOfMeasure { get; set; }

        [Column("生产数量")]
        public decimal Quantity { get; set; }

        [Column("调整数量")]
        public decimal QuantityChanged { get; set; }

        [Column("已生产数量")]
        public decimal QuantityProduced { get; set; }

        [Column("已排产数量")]
        public decimal QuantityScheduled { get; set; }

        [Column("客户Id")]
        public int ClientId { get; set; }

        [Column("客户名称")]
        public string ClientName { get; set; }

        [Column("客户编号")]
        public string ClientCode { get; set; }

        [Column("需求日期")]
        public DateTime RequestDate { get; set; }

        [Column("叠板层数")]
        public int? PanelCount { get; set; }

        [Column("待排产叠数")]
        public decimal? WadCount { get; set; }

        [Column("孔数")]
        public decimal? DrillCount { get; set; }

        [Column("工艺路线ID")]
        public virtual long? RouteId { get; set; }

        [Column("工艺路线编号")]
        public virtual string? RouteCode { get; set; }

        [Column("工艺路线名称")]
        public virtual string? RouteName { get; set; }

        [Column("建议机台数")]
        public virtual decimal? DispenseMachines { get; set; }
    }
}
