using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask
{
    public class GetTaskListReq : Page
    {
        public virtual string? Name { get; set; }

        public virtual string? Code { get; set; }

        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 完工状态(DRAFT/COMMITED/BEGIN/FINISH)
        /// </summary>
        public virtual List<TaskStatusEnum>? TaskStatusList { get; set; }

        public virtual long? WorkOrderId { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        public virtual long? ProcessId { get; set; }

        public virtual string? ProcessCode { get; set; }

        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>

        public int Status { set; get; } = -1;
        /// <summary>
        /// 展示的任务天数
        /// </summary>
        public virtual int? TaskNumber { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 工作站编号
        /// </summary>
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 查询开始时间
        /// </summary>
        public DateTime? QueryStartTime { set; get; }

        /// <summary>
        /// 查询结束时间
        /// </summary>
        public DateTime? QueryEndTime { set; get; }

        /// <summary>
        /// 数据排序方式
        /// </summary>
        public QueryOrderByEnum? QueryOrderBy { get; set; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual List<string>? RouteCodes { get; set; }

        /// <summary>
        /// 是否紧急插单
        /// </summary>
        public virtual int? IsUrgent { get; set; }


        /// <summary>
        /// 自动机器还是手动机器
        /// </summary>
        public virtual bool? IsAuto { get; set; }
    }
}
