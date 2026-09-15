using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement
{
    public class AddOrUpdateScheduleReq : BaseAddOrUpdateDto
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

        /// <summary>
        /// 
        /// </summary>
        public virtual string? RequestJson { get; set; }        

        public virtual string? SourceDeviceId { get; set; }
        public virtual string? RequireDeviceId { get; set; }
        public virtual string? TaskId { get; set; }

        /// <summary>
        /// 调度任务状态
        /// </summary>
        public virtual ScheduledTaskStatus? ScheduledTaskStatus { get; set; }
        /// <summary>
        /// 是否已全部下发任务
        /// </summary>
        public virtual bool IsAllPanelSent { get; set; }
        /// <summary>
        /// 设备事件route key 
        ///</summary>
        public virtual string? RoutingKey { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
        public virtual string? ChangedSpindles { get; set; }
        public virtual string? ChangedBehavior { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int? ItemId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 告警编码
        /// </summary>
        public virtual string? WarningCode { get; set; }

        /// <summary>
        /// 告警详细信息
        /// </summary>
        public virtual string? WarningMessage { get; set; }

        /// <summary>
        /// 交互方式编码
        /// </summary>
        public virtual ushort? RequestInteractionBehavior { get; set; }

        /// <summary>
        /// 交互方式描述
        /// </summary>
        public virtual string? RequestInteractionBehaviorName { get; set; }
        /// <summary>
        /// 累计已上生料
        /// </summary>        
        public virtual int? TotalRawCount { get; set; }
        /// <summary>
        /// 是否紧急
        /// </summary>
        public virtual int? IsUrgent { get; set; }
        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? RouteCode { get; set; }
        /// <summary>
        /// 是否是辅助设备请求
        /// </summary>
        public virtual bool? IsAuxiliary { get; set; }
        /// <summary>
        /// 是否为先执行
        /// </summary>
        public virtual bool? IsMaster { get; set; }
        public virtual long? MasterScheduleId { get; set; }
        public virtual InteractionSequence? InteractionSequence { get; set; }
        public virtual DeviceKind? RequestDeviceKind { get; set; }

        /// <summary>
        /// 库位编号
        /// </summary>
        public virtual string? SubDeviceCode { get; set; }

        /// <summary>
        /// AGV板料信息
        /// </summary>
        public virtual string? AGVPayloadPanels { get; set; }
        /// <summary>
        /// 请求的汇总简略信息
        /// </summary>
        public virtual string? RequestSummaryInfo { get; set; }
        public virtual bool? IsBarcodeOk { get; set; }
        public virtual string? CancelReason { get; set; }

        /// <summary>
        /// 库位Panel信息
        /// </summary>
        public virtual List<ScheduleLocationPanel>? LocationPanels { get; set; }
    }
}