using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary
{
    public class DeviceRecordsSummaryToExcelDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [Column("设备编号")]
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        [Column("工艺路线")]
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 班次
        /// </summary>
        [Column("班次")]
        public virtual string? Sailings { get; set; }

        /// <summary>
        /// 实际稼动率
        /// </summary>
        [Column("实际稼动率")]
        public virtual int? Duty { get; set; }

        ///// <summary>
        ///// 理论稼动率
        ///// </summary>
        //[Column("理论稼动率")]
        //public virtual int? TheoryDuty { get; set; }

        ///// <summary>
        ///// 稼动率达成率
        ///// </summary>
        //[Column("稼动率达成率")]
        //public virtual int? DutyRate { get; set; }

        /// <summary>
        /// 开机时间
        /// </summary>
        [Column("开机时长")]
        public virtual int? OpenTime { get; set; }

        /// <summary>
        /// 工作时间
        /// </summary>
        [Column("工作时长")]
        public virtual int? WorkTime { get; set; }

        /// <summary>
        /// 等待时间
        /// </summary>
        [Column("等待时长")]
        public virtual int? WaitTime { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        [Column("异常时长")]
        public virtual int? ErrorTime { get; set; }

        /// <summary>
        /// 未排产时长
        /// </summary>
        [Column("未排产时长")]
        public virtual int? WithoutTaskTime { get; set; }

        /// <summary>
        /// 未设置自动化时长
        /// </summary>
        [Column("未设置自动化时长")]
        public virtual int? DeviceDisableTime { get; set; }

        /// <summary>
        /// 手动时长
        /// </summary>
        [Column("手动时长")]
        public virtual int? BufferManualTime { get; set; }

        /// <summary>
        /// 刀具检测时长
        /// </summary>
        [Column("刀检时长")]
        public virtual int? DrillToolEvaluationTime { get; set; }

        /// <summary>
        /// 刀具寿命到时长
        /// </summary>
        [Column("刀具寿命")]
        public virtual int? ToolLifeExporedTime { get; set; }

        /// <summary>
        /// 清洗夹头总时长
        /// </summary>
        [Column("清洗夹头总时长")]
        public virtual int? CollectClearTime { get; set; }

        /// <summary>
        /// 无钻带参数时长
        /// </summary>
        [Column("无钻带参数时长")]
        public virtual int? WithoutDrillFileTime { get; set; }

        /// <summary>
        /// 数据日期
        /// </summary>
        [Column("数据日期")]
        public virtual string? DateString { get; set; }

        /// <summary>
        /// 中控记录时间
        /// </summary>
        [Column("中控记录时间")]
        public virtual string? MesRecordDate { get; set; }

    }
}
