using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External
{
    public class ExternalAndInnerWorkOrderDto
    {
        /// <summary>
        /// 内部工单名
        /// </summary>
        public virtual string? Name { get; set; }
        /// <summary>
        /// 内部工单编号
        /// </summary>
        public virtual string? Code { get; set; }
        /// <summary>
        /// 工单来源 （客户订单/库存需求）
        /// </summary>
        public virtual string? OrderSource { get; set; }

        /// <summary>
        /// 外部工单号
        /// </summary>
        public virtual string? SourceCode { get; set; }
        /// <summary>
        /// 完工状态(0:DRAFT/1:COMMITED/2:SCHEDULED/3:BEGIN/4:FINISH)
        /// </summary>
        public virtual ManuOrderStatusEnum? ManuOrderStatus { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>
        public virtual int? Status { get; set; }
        /// <summary>
        /// 处理结果描述
        /// </summary>
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 工序组
        /// </summary>
        public virtual string? SpecGroup { get; set; }
    }
}
