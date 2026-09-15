namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary
{
    public class AddOrUpdateDeviceRecordsSummaryReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 工作时间
        /// </summary>
        public virtual int? WorkTime { get; set; }

        /// <summary>
        /// 等待时间
        /// </summary>
        public virtual int? WaitTime { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        public virtual int? ErrorTime { get; set; }

        /// <summary>
        /// 开机时间
        /// </summary>
        public virtual int? OpenTime { get; set; }

        /// <summary>
        /// 稼动率
        /// </summary>
        public virtual int? Duty { get; set; }

        /// <summary>
        /// 理论稼动率
        /// </summary>
        public virtual int? TheoryDuty { get; set; }

        /// <summary>
        /// 稼动率达成率
        /// </summary>
        public virtual int? DutyRate { get; set; }

        /// <summary>
        /// 结束到开始总时间
        /// </summary>
        public virtual int? EndToStartTime { get; set; }

        /// <summary>
        /// 清洗夹头总时间
        /// </summary>
        public virtual int? CollectClearTime { get; set; }

        /// <summary>
        /// 数据日期
        /// </summary>
        public virtual string? DateString { get; set; }

        /// <summary>
        /// 班次(0-白班，1-中班，2-夜班)
        /// </summary>
        public virtual int Sailings { get; set; } = 0;

        /// <summary>
        /// 无任务时长
        /// </summary>
        public virtual int? WithoutTaskTime { get; set; }

        /// <summary>
        /// 刀具寿命时长
        /// </summary>
        public virtual int? ToolLifeExporedTime { get; set; }

        /// <summary>
        /// 刀具寿命报警次数
        /// </summary>
        public virtual int? ToolLifeExporedCount { get; set; }

        /// <summary>
        /// 无钻带参数时长
        /// </summary>
        public virtual int? WithoutDrillFileTime { get; set; }

        /// <summary>
        /// 无生产板料时长
        /// </summary>
        public virtual int? WithoutPanelTime { get; set; }

        /// <summary>
        /// 设备异常时长
        /// </summary>
        public virtual int? AlarmTime { get; set; }

        /// <summary>
        /// 设备运行时长
        /// </summary>
        public virtual int? RunTime { get; set; }

        /// <summary>
        /// Buffer无生料时长
        /// </summary>
        public virtual int? BufferNoBoardTime { get; set; }

        /// <summary>
        /// Buffer有熟料时长
        /// </summary>
        public virtual int? BufferClinkerExistTime { get; set; }

        /// <summary>
        /// 设备禁用时长
        /// </summary>
        public virtual int? DeviceDisableTime { get; set; }

        /// <summary>
        /// Buffer有板到上板完成时长
        /// </summary>
        public virtual int? BufferRawCompleteTime { get; set; }

        /// <summary>
        /// 钻机有板到开始打板时长
        /// </summary>
        public virtual int? DrillRawExistToRunTime { get; set; }

        /// <summary>
        /// Buffer自动时长
        /// </summary>
        public virtual int? BufferAutomaticTime { get; set; }

        /// <summary>
        /// 板方向检测时长
        /// </summary>
        public virtual int? DrillBoardDirectionTime { get; set; }

        /// <summary>
        /// pin检测时长
        /// </summary>
        public virtual int? DrillTestPinTime { get; set; }

        /// <summary>
        /// 刀具检测时长
        /// </summary>
        public virtual int? DrillToolEvaluationTime { get; set; }

        /// <summary>
        /// Buffer手动时长
        /// </summary>
        public virtual int? BufferManualTime { get; set; }
    }
}
