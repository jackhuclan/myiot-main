using SqlSugar;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 调度记录
    ///</summary>
    [SugarTable("t_schedule_his")]
    public class ScheduleHistory
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = false)]
        public virtual long Id { get; set; }

        /// <summary>
        /// 否已删除 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "is_deleted")]
        public virtual byte IsDeleted { get; set; } = 0;

        /// <summary>
        /// 状态(1:启用;0禁用) 
        /// 默认值: 1
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public virtual int Status { get; set; } = 1;

        /// <summary>
        /// 创建人Id 
        ///</summary>
        [SugarColumn(ColumnName = "creator_id")]
        public virtual int? CreatorId { get; set; }
        /// <summary>
        /// 创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改时间 
        ///</summary>
        [SugarColumn(ColumnName = "modify_time")]
        public virtual DateTime? ModifyTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改人Id 
        ///</summary>
        [SugarColumn(ColumnName = "modifier_id")]
        public virtual int? ModifierId { get; set; }

        ///// <summary>
        ///// 任务编号
        ///// </summary>
        //[SugarColumn(ColumnName = "source_schedule_id")]
        //public virtual long? SourceScheduleId { get; set; }


        /// <summary>
        /// 任务编号
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }
        /// <summary>
        /// 起点
        /// </summary>
        [SugarColumn(ColumnName = "start_location")]
        public virtual string? StartLocation { get; set; }
        /// <summary>
        /// 终点
        /// </summary>
        [SugarColumn(ColumnName = "end_location")]
        public virtual string? EndLocation { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        [SugarColumn(ColumnName = "priority")]
        public virtual string? Priority { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "request_json")]
        public virtual string? RequestJson { get; set; }        

        [SugarColumn(ColumnName = "source_device_id")]

        public virtual string? SourceDeviceId { get; set; }

        [SugarColumn(ColumnName = "require_device_id")]

        public virtual string? RequireDeviceId { get; set; }

        [SugarColumn(ColumnName = "task_id")]
        public virtual string? TaskId { get; set; }
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        [SugarColumn(ColumnName = "route_id")]
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        [SugarColumn(ColumnName = "route_code")]
        public virtual string? RouteCode { get; set; }
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        [SugarColumn(ColumnName = "route_name")]
        public virtual string? RouteName { get; set; }
        /// <summary>
        /// 调度任务状态
        /// </summary>
        [SugarColumn(ColumnName = "scheduled_task_status")]
        public virtual ScheduledTaskStatus? ScheduledTaskStatus { get; set; }

        /// <summary>
        /// 分配时间 
        ///</summary>
        [SugarColumn(ColumnName = "allocate_time")]
        public virtual DateTime? AllocateTime { get; set; }

        /// <summary>
        /// 开始调度时间 
        ///</summary>
        [SugarColumn(ColumnName = "running_time")]
        public virtual DateTime? RunningTime { get; set; }

        /// <summary>
        /// 调度完成时间 
        ///</summary>
        [SugarColumn(ColumnName = "completed_time")]
        public virtual DateTime? CompletedTime { get; set; }

        /// <summary>
        /// 执行失败时间 
        ///</summary>
        [SugarColumn(ColumnName = "failed_time")]
        public virtual DateTime? FailedTime { get; set; }

        /// <summary>
        /// 取消计划时间 
        ///</summary>
        [SugarColumn(ColumnName = "canceled_time")]
        public virtual DateTime? CanceledTime { get; set; }

        /// <summary>
        /// 设备事件route key 
        ///</summary>
        [SugarColumn(ColumnName = "routing_key")]
        public virtual string? RoutingKey { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }
        /// <summary>
        /// 变化后的Spindles，
        /// 如null,null,00003,null,null
        /// </summary>
        [SugarColumn(ColumnName = "changed_spindles")]
        public virtual string? ChangedSpindles { get; set; }
        /// <summary>
        /// 调整后的上下料行为，如-1,0,0,-1,0
        /// </summary>

        [SugarColumn(ColumnName = "changed_behavior")]
        public virtual string? ChangedBehavior { get; set; }

        /// <summary>
        /// 是否已全部下发任务
        /// </summary>
        [SugarColumn(ColumnName = "is_all_panel_sent")]
        public virtual bool IsAllPanelSent { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        [SugarColumn(ColumnName = "item_id")]
        public int? ItemId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [SugarColumn(ColumnName = "item_name")]
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 告警编码
        /// </summary>
        [SugarColumn(ColumnName = "warnning_code")]
        public virtual string? WarningCode { get; set; }

        /// <summary>
        /// 告警详细信息
        /// </summary>
        [SugarColumn(ColumnName = "warnning_message")]
        public virtual string? WarningMessage { get; set; }

        /// <summary>
        /// 交互方式编码
        /// </summary>
        [SugarColumn(ColumnName = "request_interaction_behavior")]
        public virtual ushort? RequestInteractionBehavior { get; set; }

        /// <summary>
        /// 交互方式描述
        /// </summary>
        [SugarColumn(ColumnName = "request_interaction_behavior_name")]
        public virtual string? RequestInteractionBehaviorName { get; set; }
        /// <summary>
        /// 累计已上生料
        /// </summary>
        [SugarColumn(ColumnName = "total_raw_count")]
        public virtual int? TotalRawCount { get; set; }

        /// <summary>
        /// 是否紧急
        /// </summary>
        [SugarColumn(ColumnName = "is_urgent")]
        public virtual int? IsUrgent { get; set; }
        /// <summary>
        /// 是否是辅助设备请求
        /// </summary>
        [SugarColumn(ColumnName = "is_auxiliary")]
        public virtual bool? IsAuxiliary { get; set; }
        /// <summary>
        /// 是否为先执行
        /// </summary>
        [SugarColumn(ColumnName = "is_master")]
        public virtual bool? IsMaster { get; set; }
        /// <summary>
        /// 关联的主叫调度记录ID
        /// </summary>
        [SugarColumn(ColumnName = "master_schedule_id")]
        public virtual long? MasterScheduleId { get; set; }
        [SugarColumn(ColumnName = "interaction_sequence")]
        public virtual InteractionSequence? InteractionSequence { get; set; } = Fundation.Iot.Models.InteractionSequence.None;
        [SugarColumn(ColumnName = "request_device_kind")]
        public virtual DeviceKind? RequestDeviceKind { get; set; } = DeviceKind.Unknown;

        /// <summary>
        /// 库位编号
        /// </summary>
        [SugarColumn(ColumnName = "sub_device_code")]
        public virtual string? SubDeviceCode { get; set; }

        /// <summary>
        /// AGV板料信息
        /// </summary>
        [SugarColumn(ColumnName = "agv_payload_panels")]
        public virtual string? AGVPayloadPanels { get; set; }
        /// <summary>
        /// 请求的汇总简略信息
        /// </summary>
        [SugarColumn(ColumnName = "request_summary_info")]
        public virtual string? RequestSummaryInfo { get; set; }
        /// <summary>
        /// 板料检验是否OK
        /// </summary>
        [SugarColumn(ColumnName = "is_barcode_ok")]
        public virtual bool? IsBarcodeOk { get; set; }
        /// <summary>
        /// 取消原因
        /// </summary>
        [SugarColumn(ColumnName = "cancel_reason")]
        public virtual string? CancelReason { get; set; }
    }
}
