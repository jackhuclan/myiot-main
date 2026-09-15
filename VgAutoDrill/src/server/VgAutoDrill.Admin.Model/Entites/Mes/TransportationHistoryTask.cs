using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VgAutoDrill.Admin.Model.CentralModels;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 料仓任务表
    ///</summary>
    [SugarTable("t_transportation_history_task")]
    public class TransportationHistoryTask
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = false)]
        public   long Id { get; set; }

        /// <summary>
        /// 否已删除 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "is_deleted")]
        public  byte IsDeleted { get; set; } = 0;

        /// <summary>
        /// 状态(1:启用;0禁用) 
        /// 默认值: 1
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public  int Status { get; set; } = 1;

        /// <summary>
        /// 创建人Id 
        ///</summary>
        [SugarColumn(ColumnName = "creator_id")]
        public  int? CreatorId { get; set; }
        /// <summary>
        /// 创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public  DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改时间 
        ///</summary>
        [SugarColumn(ColumnName = "modify_time")]
        public  DateTime? ModifyTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改人Id 
        ///</summary>
        [SugarColumn(ColumnName = "modifier_id")]
        public  int? ModifierId { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        /// <summary>
        /// 是否紧急
        /// </summary>
        [SugarColumn(ColumnName = "is_urgent")]
        public virtual int? IsUrgent { get; set; }

        /// <summary>
        /// 交互序列
        /// </summary>
        [SugarColumn(ColumnName = "interaction_sequence")]
        public virtual InteractionSequence? InteractionSequence { get; set; }

        /// <summary>
        /// 调度任务状态
        /// </summary>
        [SugarColumn(ColumnName = "scheduled_task_status")]
        public virtual ScheduledTaskStatus? ScheduledTaskStatus { get; set; }

        /// <summary>
        /// 类型：空仓、生料、熟料、首件
        /// </summary>
        [SugarColumn(ColumnName = "transportation_kind")]
        public virtual TransportationKind? TransportationKind { get; set; }

        /// <summary>
        /// 内部编号
        /// </summary>
        [SugarColumn(ColumnName = "internal_lot_no")]
        public virtual string? InternalLotNo { get; set; }

        /// <summary>
        /// 外部编号
        /// </summary>
        [SugarColumn(ColumnName = "external_lot_no")]
        public virtual string? ExternalLotNo { get; set; }

        /// <summary>
        /// 库房，库位分区编号
        /// </summary>
        [SugarColumn(ColumnName = "warehouse_code")]
        public virtual string? WarehouseCode { get; set; }

        /// <summary>
        /// 料架编号
        /// </summary>
        [SugarColumn(ColumnName = "fork_code")]
        public virtual string? ForkCode { get; set; }

        /// <summary>
        /// 相关钻机追溯
        /// </summary>
        [SugarColumn(ColumnName = "related_drill_trace")]
        public virtual string? RelatedDrillTrace { get; set; }

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
        /// 熟料数量
        /// </summary>
        [SugarColumn(ColumnName = "clinker_count")]
        public virtual int? ClinkerCount { get; set; }

        /// <summary>
        /// 生料数量
        /// </summary>
        /// <returns></returns>
        [SugarColumn(ColumnName = "raw_ount")]
        public virtual int? RawCount { get; set; }

        /// <summary>
        /// 料仓号
        /// </summary>
        [SugarColumn(ColumnName = "silo_code")]
        public string? SiloCode { get; set; }

        [SugarColumn(ColumnName = "related_route")]
        public string? RelatedRoute { get; set; }

        [SugarColumn(ColumnName = "related_schedule_id")]
        public long? RelatedScheduleId { get; set; }

        [SugarColumn(ColumnName = "relate_device_code")]
        public string? RelateDeviceCode { get; set; }

        [SugarColumn(ColumnName = "job_id")]
        public virtual long? JobId { get; set; }

        [SugarColumn(ColumnName = "plan_id")]
        public virtual long? PlanId { get; set; }

        /// <summary>
        /// hk返回信息
        /// </summary>
        [SugarColumn(ColumnName = "hk_response")]
        public virtual string? HkResponse { get; set; }

        [SugarColumn(ColumnName = "master_device_kind")]
        public DeviceKind MasterDeviceKind { get; set; }

        [SugarColumn(ColumnName = "agv_kind")]
        public DeviceKind AgvKind { get; set; }

        [SugarColumn(ColumnName = "allocate_agv")]
        public string? AllocatedAgv { get; set; }

        [SugarColumn(ColumnName = "hk_response_key")]
        public string? HikResponseKey { get; set; }

        [SugarColumn(ColumnName = "fork_location_schedule_id")]
        public string? ForkLocationScheduleId { get; set; }

        /// <summary>
        /// 起始库位
        /// </summary>
        [SugarColumn(ColumnName = "start_location_code")]
        public string? StartLocationCode { get; set; }

        /// <summary>
        /// 起始设备id
        /// </summary>
        [SugarColumn(ColumnName = "start_device_id")]
        public string? StartDeviceId { get; set; }

        /// <summary>
        /// 起始调度id
        /// </summary>
        [SugarColumn(ColumnName = "start_schedule_id")]
        public virtual long? StartScheduleId { get; set; }

        /// <summary>
        /// 终点库位
        /// </summary>
        [SugarColumn(ColumnName = "end_location_code")]
        public string? EndLocationCode { get; set; }

        /// <summary>
        /// 终点设备id
        /// </summary>
        [SugarColumn(ColumnName = "end_device_id")]
        public string? EndDeviceId { get; set; }

        /// <summary>
        /// 终点调度id
        /// </summary>
        [SugarColumn(ColumnName = "end_schedule_id")]
        public virtual long? EndScheduleId { get; set; }

        /// <summary>
        ///设备调度记录id
        /// </summary>
        [SugarColumn(ColumnName = "device_location_schedule_id")]
        public string? DeviceLocationScheduleId { get; set; }

        /// <summary>
        /// 转移行为
        /// </summary>
        [SugarColumn(ColumnName = "transfer_behavior")]
        public virtual int? TransferBehavior { get; set; }

        /// <summary>
        /// 是否手动创建0/1
        /// </summary>
        [SugarColumn(ColumnName = "is_manual")]
        public virtual int? IsManual { get; set; } = 0;

        /// <summary>
        /// 熟料备注
        /// </summary>
        [SugarColumn(ColumnName = "clinker_remark")]
        public virtual string? ClinkerRemark { get; set; }

    }
}
