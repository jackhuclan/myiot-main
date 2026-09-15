namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary
{
    public class DeviceRecordsSummaryDto : BaseDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 工作时间
        /// </summary>
        public virtual int? WorkTime { get; set; } = 0;

        /// <summary>
        /// 等待时间
        /// </summary>
        public virtual int? WaitTime { get; set; } = 0;

        /// <summary>
        /// 异常时间
        /// </summary>
        public virtual int? ErrorTime { get; set; } = 0;

        /// <summary>
        /// 开机时间
        /// </summary>
        public virtual int? OpenTime { get; set; } = 0;

        /// <summary>
        /// 稼动率
        /// </summary>
        public virtual int? Duty { get; set; } = 0;

        /// <summary>
        /// 理论稼动率
        /// </summary>
        public virtual int? TheoryDuty { get; set; } = 0;

        /// <summary>
        /// 稼动率达成率
        /// </summary>
        public virtual int? DutyRate { get; set; } = 0;



        /// <summary>
        /// 必要时间
        /// </summary>
        public virtual int? NecessaryTime { get; set; } = 0;

        /// <summary>
        /// 结束到开始总时间
        /// </summary>
        public virtual int? EndToStartTime { get; set; } = 0;

        /// <summary>
        /// 清洗夹头总时间
        /// </summary>
        public virtual int? CollectClearTime { get; set; } = 0;

        /// <summary>
        /// 数据日期
        /// </summary>
        public virtual string? DateString { get; set; }

        /// <summary>
        /// 班次(0-白班，1-中班，2-夜班)
        /// </summary>
        public virtual int? Sailings { get; set; } = 0;

        /// <summary>
        /// 工艺路线
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 无任务时长
        /// </summary>
        public virtual int? WithoutTaskTime { get; set; } = 0;

        /// <summary>
        /// 刀具寿命等待时长
        /// </summary>
        public virtual int? ToolLifeExporedTime { get; set; } = 0;
        /// <summary>
        /// 刀具寿命报警次数
        /// </summary>
        public virtual int? ToolLifeExporedCount { get; set; } = 0;

        /// <summary>
        /// 无钻带参数时长
        /// </summary>
        public virtual int? WithoutDrillFileTime { get; set; } = 0;

        /// <summary>
        /// 无生产板料时长
        /// </summary>
        public virtual int? WithoutPanelTime { get; set; } = 0;

        /// <summary>
        /// 设备异常时长
        /// </summary>
        public virtual int? AlarmTime { get; set; } = 0;

        /// <summary>
        /// 设备运行时长
        /// </summary>
        public virtual int? RunTime { get; set; } = 0;

        /// <summary>
        /// Buffer无生料时长
        /// </summary>
        public virtual int? BufferNoBoardTime { get; set; } = 0;

        /// <summary>
        /// Buffer有熟料时长
        /// </summary>
        public virtual int? BufferClinkerExistTime { get; set; } = 0;

        /// <summary>
        /// 设备禁用时长
        /// </summary>
        public virtual int? DeviceDisableTime { get; set; } = 0;

        /// <summary>
        /// Buffer有板到上板完成时长
        /// </summary>
        public virtual int? BufferRawCompleteTime { get; set; } = 0;

        /// <summary>
        /// 钻机有板到开始打板时长
        /// </summary>
        public virtual int? DrillRawExistToRunTime { get; set; } = 0;

        /// <summary>
        /// Buffer自动时长
        /// </summary>
        public virtual int? BufferAutomaticTime { get; set; } = 0;

        /// <summary>
        /// 板方向检测时长
        /// </summary>
        public virtual int? DrillBoardDirectionTime { get; set; } = 0;

        /// <summary>
        /// pin检测时长
        /// </summary>
        public virtual int? DrillTestPinTime { get; set; } = 0;

        /// <summary>
        /// 刀具检测时长
        /// </summary>
        public virtual int? DrillToolEvaluationTime { get; set; } = 0;

        /// <summary>
        /// Buffer手动时长
        /// </summary>
        public virtual int? BufferManualTime { get; set; } = 0;

        /// <summary>
        /// 刀具吸尘报警次数
        /// </summary>
        public virtual int? DrillNoVacuumCount { get; set; } = 0;

        /// <summary>
        /// 刀具吸尘报警标准时长
        /// </summary>
        public virtual int? DrillNoVacuumStandardTime { get; set; } = 0;



        #region 刀具寿命报警更换 --单次耗时（秒）
        // <summary>
        /// 刀具寿命报警更换配置时长 --单次耗时（秒）
        /// </summary>
        public virtual int? ToolLifeExporedChangeStandardTime { get; set; } = 0;
        #endregion

        #region 不切换料号时的换刀时长 --单次耗时（秒）
        /// <summary>
        /// 不切换料号时的换刀配置时长 --单次耗时（秒）
        /// </summary>
        public virtual int? NoSwitchMaterialToolChangeStandardTime { get; set; } = 0;



        /// <summary>
        /// /// <summary>
        /// 不切换料号时的换刀次数
        /// </summary>
        /// </summary>
        public virtual int? NoSwitchMaterialToolChangeCount { get; set; } = 0;
        #endregion

        #region 切换料号时的换刀时长 --单次耗时（秒）
        /// <summary> 
        /// 切换料号时的换刀配置时长 --单次耗时（秒）
        /// </summary>
        public virtual int? SwitchMaterialToolChangeStandardTime { get; set; } = 0;



        /// <summary>
        /// /// <summary>
        /// 不切换料号时的换刀次数
        /// </summary>
        /// </summary>
        public virtual int? SwitchMaterialToolChangeCount { get; set; } = 0;
        #endregion


        #region PIN校正 --按班次平摊（秒）
        /// <summary>
        /// PIN校正标准时长 --按班次平摊（秒）
        /// </summary>
        public virtual int? PINReviseStandardTime { get; set; } = 0;



        /// <summary>
        /// PIN校正次数
        /// </summary>
        public virtual int? PINReviseCount { get; set; } = 0;
        #endregion


        #region 检测摆幅扭力时长 --按班次平摊（秒）
        /// <summary>
        /// 检测摆幅扭力标准时长 --按班次平摊（秒）
        /// </summary>
        public virtual int? DetectSwingTorqueStandardTime { get; set; } = 0;



        /// <summary>
        /// 检测摆幅扭力次数
        /// </summary>
        public virtual int? DetectSwingTorqueCount { get; set; } = 0;
        #endregion

        #region 压力脚更换时长 --单次耗时（秒）
        /// <summary>
        /// 压力脚更换标准时长 --单次耗时（秒）
        /// </summary>
        public virtual int? PressureFootChangeStandardTime { get; set; } = 0;


        /// <summary>
        /// 压力脚更换次数
        /// </summary>
        public virtual int? PressureFootChangeCount { get; set; } = 0;
        #endregion

        #region 多层板，最小层设定
        /// <summary>
        /// 多层板，最小层数设定标准时长
        /// </summary>
        public virtual int? MinMultilayerBoardsStandardValue { get; set; } = 0;


        /// <summary>
        /// 多层板，最小实设定次数
        /// </summary>
        public virtual int? MinMultilayerBoardCount { get; set; } = 0;
        #endregion

        #region 两层板等待首件耗时 --单次耗时（秒）
        /// <summary>
        /// 两层板等待首件标准耗时 --单次耗时（秒）
        /// </summary>
        public virtual int? TwoBoardsWaitFirstResultStandardTime { get; set; } = 0;


        /// <summary>
        /// 两层板等待首件实际次数 --单次耗时（秒）
        /// </summary>
        public virtual int? TwoBoardsWaitFirstResultCount { get; set; } = 0;
        #endregion


        #region 多层板等待首件耗时 --单次耗时（秒）
        /// <summary>
        /// 多层板等待首件标准耗时 --单次耗时（秒）
        /// </summary>
        public virtual int? MultilayerBoardWaitFirstResultStandardTime { get; set; } = 0;


        /// <summary>
        /// 多层板等待首件实际次数
        /// </summary>
        public virtual int? MultilayerBoardWaitFirstResultCount { get; set; } = 0;
        #endregion
    }
}
