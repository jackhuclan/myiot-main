using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule
{
    public class SimpleScheduleDto : BaseDto
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 起点
        /// </summary>
        public virtual string? StartLocation { get; set; }

        /// <summary>
        /// 终点
        /// </summary>
        public virtual string? EndLocation { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public virtual string? Priority { get; set; }
        public virtual string? SourceDeviceId { get; set; }
        /// <summary>
        /// 库位编号
        /// </summary>
        public virtual string? SubDeviceCode { get; set; }
        public virtual string? RequireDeviceId { get; set; }
        public virtual string? TaskId { get; set; }
        /// <summary>
        /// 是否已全部下发任务
        /// </summary>
        public virtual bool IsAllPanelSent { get; set; }

        /// <summary>
        /// 调度任务状态
        /// </summary>
        public virtual ScheduledTaskStatus? ScheduledTaskStatus { get; set; }
        /// <summary>
        /// 交互方式编码
        /// </summary>
        public virtual ushort? RequestInteractionBehavior { get; set; }

        /// <summary>
        /// 交互方式描述
        /// </summary>
        public virtual string? RequestInteractionBehaviorName { get; set; }
        /// <summary>
        /// 是否为先执行
        /// </summary>
        public virtual bool? IsMaster { get; set; }
        public virtual long? MasterScheduleId { get; set; }
        public virtual InteractionSequence? InteractionSequence { get; set; }
        public virtual DeviceKind? RequestDeviceKind { get; set; }
        /// <summary>
        /// 分配时间 
        ///</summary>
        public virtual DateTime? AllocateTime { get; set; }

        /// <summary>
        /// 开始调度时间 
        ///</summary>
        public virtual DateTime? RunningTime { get; set; }

        /// <summary>
        /// 调度完成时间 
        ///</summary>
        public virtual DateTime? CompletedTime { get; set; }

        /// <summary>
        /// 执行失败时间 
        ///</summary>
        public virtual DateTime? FailedTime { get; set; }
    }
}
