using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob
{
    public class TransferJobDto : BaseDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 是否紧急
        /// </summary>
        public virtual int? IsUrgent { get; set; }

        /// <summary>
        /// 交互序列
        /// </summary>
        public virtual InteractionSequence? InteractionSequence { get; set; }

        /// <summary>
        /// 调度任务状态
        /// </summary>
        public virtual ScheduledTaskStatus? ScheduledTaskStatus { get; set; }

        /// <summary>
        /// 类型：空仓、生料、熟料、首件
        /// </summary>
        public virtual TransportationKind? TransportationKind { get; set; }

        /// <summary>
        /// 内部编号
        /// </summary>
        public virtual string? InternalLotNo { get; set; }

        /// <summary>
        /// 外部编号
        /// </summary>
        public virtual string? ExternalLotNo { get; set; }

        /// <summary>
        /// 库房，库位分区编号
        /// </summary>
        public virtual string? WarehouseCode { get; set; }

        /// <summary>
        /// 料架编号
        /// </summary>
        public virtual string? ForkCode { get; set; }

        /// <summary>
        /// 相关钻机追溯
        /// </summary>
        public virtual string? RelatedDrillTrace { get; set; }

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

        /// <summary>
        /// 取消计划时间
        ///</summary>
        public virtual DateTime? CanceledTime { get; set; }

        /// <summary>
        /// 熟料数量
        /// </summary>
        public virtual int? ClinkerCount { get; set; }

        /// <summary>
        /// 生料数量
        /// </summary>
        /// <returns></returns>
        public virtual int? RawCount { get; set; }

        /// <summary>
        /// 料仓号
        /// </summary>
        public string? SiloCode { get; set; }

        public string? RelatedRoute { get; set; }
        public long? RelatedScheduleId { get; set; }
        public string? RelateDeviceCode { get; set; }
        public virtual long? JobId { get; set; }
        public virtual long? PlanId { get; set; }
        public virtual string? HkResponse { get; set; }
        public DeviceKind MasterDeviceKind { get; set; }
        public DeviceKind AgvKind { get; set; }
        public string AllocatedAgv { get; set; } = string.Empty;
        public string? HikResponseKey { get; set; }
        public string? ForkLocationScheduleId { get; set; }

        /// <summary>
        /// 起始库位
        /// </summary>
        public string? StartLocationCode { get; set; }

        /// <summary>
        /// 起始设备id
        /// </summary>
        public string? StartDeviceId { get; set; }

        /// <summary>
        /// 起始调度id
        /// </summary>
        public virtual long? StartScheduleId { get; set; }       

        /// <summary>
        /// 终点库位
        /// </summary>
        public string? EndLocationCode { get; set; }

        /// <summary>
        /// 终点设备id
        /// </summary>
        public string? EndDeviceId { get; set; }

        /// <summary>
        /// 终点调度id
        /// </summary>
        public virtual long? EndScheduleId { get; set; }
        /// <summary>
        /// 起始调度
        /// </summary>
        public virtual string? StartSchedule { get; set; }
        /// <summary>
        /// 终点调度
        /// </summary>
        public virtual string? EndSchedule { get; set; }
        public string? DeviceLocationScheduleId { get; set; }

        /// <summary>
        /// 转移行为
        /// </summary>
        public virtual int? TransferBehavior { get; set; }

        /// <summary>
        /// 是否手动创建0/1
        /// </summary>
        public virtual int? IsManual { get; set; } = 0;

        /// <summary>
        /// 熟料备注
        /// </summary>
        public virtual string? ClinkerRemark { get; set; }

        /// <summary>
        ///  代理执行任务/中控控制任务
        ///  中控自动运行的任务:true,代理自动运行的任务:false
        /// </summary>
        public virtual bool? IsCssControlled { get; set; }


        /// <summary>
        /// 工序组
        /// </summary>
        public virtual string? SpecGroup { get; set; }

        /// <summary>
        /// 叠数
        /// </summary>
        public virtual int? PanelCount { get; set; }


        /// <summary>
        /// PCS总数
        /// </summary>
        public virtual int? TotalPcs { get; set; }

        public virtual string? PositionCodes { get; set; }
    }
}