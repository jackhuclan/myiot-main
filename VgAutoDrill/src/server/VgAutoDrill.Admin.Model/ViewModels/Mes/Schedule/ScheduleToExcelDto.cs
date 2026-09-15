using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule
{
    public class ScheduleToExcelDto
    {
        [Column("调度Id")]
        public virtual long? Id { get; set; }

        [Column("发起设备")]
        public virtual string? SourceDeviceId { get; set; }

        [Column("调度设备")]
        public virtual string? RequireDeviceId { get; set; }

        [Column("任务编号")]
        public virtual string? TaskId { get; set; }

        [Column("调度任务状态")]
        public virtual string? ScheduledTaskStatus { get; set; }

        [Column("呼叫时间")]
        public virtual DateTime? CreateTime { get; set; }

        [Column("分配时间")]
        public virtual DateTime? AllocateTime { get; set; }

        [Column("开始调度时间")]
        public virtual DateTime? RunningTime { get; set; }

        [Column("调度完成时间")]
        public virtual DateTime? CompletedTime { get; set; }

        [Column("执行失败时间")]
        public virtual DateTime? FailedTime { get; set; }

        [Column("取消计划时间")]
        public virtual DateTime? CanceledTime { get; set; }

        [Column("备注")]
        public virtual string? Remark { get; set; }

        [Column("产品名称")]
        public virtual string? ItemName { get; set; }

        [Column("产品编号")]
        public virtual string? ItemCode { get; set; }

        [Column("工艺路线编号")]
        public virtual string? RouteCode { get; set; }

        [Column("工艺路线名称")]
        public virtual string? RouteName { get; set; }

        [Column("是否紧急")]
        public virtual int? IsUrgent { get; set; }

        [Column("交互方式")]
        public virtual string? InteractionSequence { get; set; }

        [Column("库位编号")]
        public virtual string? SubDeviceCode { get; set; }

        [Column("板料检验是否OK")]
        public virtual string? BarcodeResult { get; set; }

    }
}
