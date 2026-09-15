using Npoi.Mapper.Attributes;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob
{
    public class TransportationTaskToExcelDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Column("ID")]
        public virtual string? Id { get; set; }

        /// <summary>
        /// 库房，库位分区编号
        /// </summary>
        [Column("分区")]
        public virtual string? WarehouseCode { get; set; }

        /// <summary>
        /// 料架编号
        /// </summary>
        [Column("库位")]
        public virtual string? ForkCode { get; set; }

        /// <summary>
        /// 交互序列
        /// </summary>
        [Ignore]
        public virtual InteractionSequence? InteractionSequence { get; set; }

        /// <summary>
        /// 交互序列
        /// </summary>
        [Column("交互")]
        public virtual string? InteractionSequenceDescription
        {
            get { return InteractionSequence == null ? "" : InteractionSequence!.GetDescription(); }
        }

        [Ignore]
        public virtual ScheduledTaskStatus? ScheduledTaskStatus { get; set; }

        /// <summary>
        /// 调度任务状态描述
        /// </summary>
        [Column("调度状态")]
        public virtual string? ScheduledTaskStatusDescription
        {
            get { return ScheduledTaskStatus == null ? "" : ScheduledTaskStatus!.GetDescription(); }
        }

        /// <summary>
        /// 类型：空仓、生料、熟料、首件
        /// </summary>
        [Ignore]
        public virtual TransportationKind? TransportationKind { get; set; }

        /// <summary>
        /// 类型：空仓、生料、熟料、首件
        /// </summary>
        [Column("料仓类型")]
        public virtual string? TransportationKindDescription
        {
            get { return TransportationKind == null ? "" : TransportationKind!.GetDescription(); }
        }

        /// <summary>
        /// 是否手动创建0/1
        /// </summary>
        [Ignore]
        public virtual int? IsManual { get; set; } = 0;

        /// <summary>
        /// 是否手动创建0/1 描述
        /// </summary>
        [Column("手动创建")]
        public virtual string? IsManualDescription
        {
            get { return IsManual == null ? "" : (IsManual == 0 ? "否" : "是"); }
        }

        /// <summary>
        /// 创建时间 
        ///</summary>
        [Ignore]
        public virtual DateTime? CreateTime { get; set; }

        // <summary>
        /// 创建时间 
        ///</summary>
        [Column("创建时间")]
        public virtual string? CreateTimeDescription
        {
            get { return CreateTime == null ? "" : CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"); }
        }
        /// <summary>
        /// 等待时长
        ///</summary>
        [Column("等待时长(分钟)")]
        public virtual int? WaitingTime
        {
            get
            {
                switch ((int)ScheduledTaskStatus)
                {
                    case 4:
                        return (RunningTime == null || CreateTime == null) ? 0 : (int)(RunningTime - CreateTime).Value.TotalMinutes;
                    case -2:
                        return (CanceledTime == null || CreateTime == null) ? 0 : (int)(CanceledTime - CreateTime).Value.TotalMinutes;
                    case -1:
                        return (FailedTime == null || CreateTime == null) ? 0 : (int)(FailedTime - CreateTime).Value.TotalMinutes;
                    default:
                        return (CreateTime == null) ? 0 : (int)(DateTime.Now - CreateTime).Value.TotalMinutes;
                }
            }
        }


        /// <summary>
        /// 内部编号
        /// </summary>
        [Column("内部编号")]
        public virtual string? InternalLotNo { get; set; }

        /// <summary>
        /// 料仓号
        /// </summary>
        [Column("料仓")]
        public string? SiloCode { get; set; }


        /// <summary>
        /// hk返回信息
        /// </summary>
        [Column("HK反馈信息")]
        public virtual string? HkResponse { get; set; }

        /// <summary>
        /// 相关钻机追溯
        /// </summary>
        [Column("备注")]
        public virtual string? RelatedDrillTrace { get; set; }

        /// <summary>
        /// 是否紧急
        /// </summary>
        [Ignore]
        public virtual int? IsUrgent { get; set; }

        /// <summary>
        /// 是否紧急
        /// </summary>
        [Column("紧急")]
        public virtual string? IsUrgentDescription
        {
            get { return IsUrgent == null ? "" : IsUrgent == 0 ? "否" : "是"; }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //[Column("工艺路线")]
        //public virtual string? RelatedRoute { get; set; }

        /// <summary>
        /// 熟料数量
        /// </summary>
        [Column("熟料")]
        public virtual int? ClinkerCount { get; set; }

        /// <summary>
        /// 生料数量
        /// </summary>
        /// <returns></returns>
        [Column("生料")]
        public virtual int? RawCount { get; set; }

        /// <summary>
        /// 外部编号
        /// </summary>
        [Column("外部编号")]
        public virtual string? ExternalLotNo { get; set; }

        /// <summary>
        /// 开始调度时间
        ///</summary>
        [Ignore]
        public virtual DateTime? RunningTime { get; set; }

        /// <summary>
        /// 开始调度时间
        ///</summary>
        [Column("开始调度")]
        public virtual string? RunningTimeDescription
        {
            get { return RunningTime == null ? "" : RunningTime.Value.ToString("yyyy-MM-dd HH:mm:ss"); }
        }
        /// <summary>
        /// 调度完成时间
        ///</summary>
        [Ignore]
        public virtual DateTime? CompletedTime { get; set; }

        /// <summary>
        /// 调度完成时间
        ///</summary>
        [Column("调度完成")]
        public virtual string? CompletedTimeDescription
        {
            get { return CompletedTime == null ? "" : CompletedTime.Value.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        /// <summary>
        /// 上料时长
        /// </summary>
        [Column("上料时长")]
        public virtual int? LoadMaterialTime
        {
            get
            {
                return (CompletedTime == null || RunningTime == null) ? 0 : (int)(CompletedTime - RunningTime).Value.TotalMinutes;
            }
        }

        /// <summary>
        /// 执行失败时间
        ///</summary>
        [Ignore]
        public virtual DateTime? FailedTime { get; set; }

        /// <summary>
        /// 执行失败时间
        ///</summary>
        [Column("执行失败")]
        public virtual string? FailedTimeDescription
        {
            get { return FailedTime == null ? "" : FailedTime.Value.ToString("yyyy-MM-dd HH:mm:ss"); }
        }
        /// <summary>
        /// 取消计划时间
        ///</summary>
        [Ignore]
        public virtual DateTime? CanceledTime { get; set; }

        /// <summary>
        /// 执行失败时间
        ///</summary>
        [Column("取消计划")]
        public virtual string? CanceledTimeDescription
        {
            get { return CanceledTime == null ? "" : CanceledTime.Value.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

    }
}
