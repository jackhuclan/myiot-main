using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder
{
    public class GetMOTaskReq
    {
        /// <summary>
        /// 生产工单Id
        /// </summary>
        public virtual int? WorkOrderId { get; set; }
        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// stationcode
        /// </summary>
        public virtual string? WorkStationCode { get; set; }


        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 工单名称
        /// </summary>

        public virtual string? WorkOrderName { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 展示的任务天数
        /// </summary>
        public virtual int? TaskNumber { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public virtual string? ProcessCode { get; set; }
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? RouteCode { get; set; }
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public virtual string? RouteName { get; set; }

        /// <summary>
        /// 工艺路线编码集合
        /// </summary>
        public virtual List<string>? RouteCodeList { get; set; }

        public virtual ManuOrderStatusEnum? manuOrderStatus { get; set; }


        /// <summary>
        /// 是否紧急插单
        /// </summary>
        public virtual int? IsUrgent { get; set; }

        /// <summary>
        /// 任务总数
        /// </summary>
        public virtual int? TaskCount { get; set; }

        /// <summary>
        /// 任务已提交
        /// </summary>
        public virtual int? TaskCommit { get; set; }

        /// <summary>
        /// 任务已提交
        /// </summary>
        public virtual int? TaskDraft { get; set; }

        // <summary>
        /// 派送中
        /// </summary>
        public virtual int? TaskSending { get; set; }
        /// <summary>
        /// Buffer就位
        /// </summary>
        public virtual int? TaskBuffered { get; set; }
        /// <summary>
        /// 已开始
        /// 数值从2变更为40
        /// </summary>
        public virtual int? TaskBegin { get; set; }
        /// <summary>
        /// 已完成
        /// 数值从3变更为50
        /// </summary>
        public virtual int? TaskFinish { get; set; }

        /// <summary>
        /// 标记颜色
        /// </summary>
        public virtual string? RemarkColor { get; set; }

        /// <summary>
        /// 是否查询历史为完成工单
        /// </summary>
        public virtual bool? IsHistory { get; set; }

        /// <summary>
        /// 是否显示已经完工的工单数据
        /// </summary>
        public virtual bool? IsFinish { get; set; }

    }
}
