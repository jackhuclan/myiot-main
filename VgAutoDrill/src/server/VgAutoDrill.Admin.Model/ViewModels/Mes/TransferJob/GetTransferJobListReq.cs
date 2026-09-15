using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob
{
    public class GetTransferJobListReq : Page
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
        public virtual List<ScheduledTaskStatus>? ScheduledTaskStatusList { get; set; }

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

        public virtual string? SiloCode { get; set; }

        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 相关钻机追溯
        /// </summary>
        public virtual string? RelatedDrillTrace { get; set; }

        public virtual int? TimeOutSecondes { get; set; }

        public virtual long? JobId { get; set; }

        public virtual long? PlanId { get; set; }

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
        /// 转移行为
        /// </summary>
        public virtual int? TransferBehavior { get; set; }

        /// <summary>
        /// hik反馈信息
        /// </summary>
        public virtual string? HkResponse { get; set; }

        /// <summary>
        /// 是否手动创建0/1
        /// </summary>
        public virtual int? IsManual { get; set; }

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

        /// <summary>
        /// 创建时间-(呼叫)开始时间
        /// </summary>
        public DateTime? StartTime { set; get; }

        /// <summary>
        /// 创建时间-（呼叫）结束时间
        /// </summary>
        public DateTime? EndTime { set; get; }
    }
}
