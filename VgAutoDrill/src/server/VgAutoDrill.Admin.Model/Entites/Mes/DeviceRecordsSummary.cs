using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 设备记录汇总表
    /// </summary>
    [SugarTable("t_device_records_summary")]
    public class DeviceRecordsSummary : BaseEntity
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [SugarColumn(ColumnName = "device_code")]
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 工作时间
        /// </summary>
        [SugarColumn(ColumnName = "work_time")]
        public virtual int? WorkTime { get; set; }

        /// <summary>
        /// 等待时间
        /// </summary>
        [SugarColumn(ColumnName = "wait_time")]
        public virtual int? WaitTime { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        [SugarColumn(ColumnName = "error_time")]
        public virtual int? ErrorTime { get; set; }

        /// <summary>
        /// 开机时间
        /// </summary>
        [SugarColumn(ColumnName = "open_time")]
        public virtual int? OpenTime { get; set; }

        /// <summary>
        /// 稼动率
        /// </summary>
        [SugarColumn(ColumnName = "duty")]
        public virtual int? Duty { get; set; }

        /// <summary>
        /// 结束到开始总时间
        /// </summary>
        [SugarColumn(ColumnName = "end_to_start_time")]
        public virtual int? EndToStartTime { get; set; }

        /// <summary>
        /// 清洗夹头总时间
        /// </summary>
        [SugarColumn(ColumnName = "collect_clear_time")]
        public virtual int? CollectClearTime { get; set; }

        /// <summary>
        /// 数据日期
        /// </summary>
        [SugarColumn(ColumnName = "date_string")]
        public virtual string? DateString { get; set; }

        /// <summary>
        /// 班次(0-白班，1-中班，2-夜班)
        /// </summary>
        [SugarColumn(ColumnName = "sailings")]
        public virtual int Sailings { get; set; } = 0;

        /// <summary>
        /// 无任务时长
        /// </summary>
        [SugarColumn(ColumnName = "without_task_time")]
        public virtual int? WithoutTaskTime { get; set; }

        /// <summary>
        /// 刀具寿命时长
        /// </summary>
        [SugarColumn(ColumnName = "tool_life_expored_time")]
        public virtual int? ToolLifeExporedTime { get; set; }

        /// <summary>
        /// 刀具寿命报警次数
        /// </summary>
        [SugarColumn(ColumnName = "tool_life_expored_count")]
        public virtual int? ToolLifeExporedCount { get; set; }

        /// <summary>
        /// 无钻带参数时长
        /// </summary>
        [SugarColumn(ColumnName = "without_drill_file_time")]
        public virtual int? WithoutDrillFileTime { get; set; }

        /// <summary>
        /// 无生产板料时长
        /// </summary>
        [SugarColumn(ColumnName = "without_panel_time")]
        public virtual int? WithoutPanelTime { get; set; }

        /// <summary>
        /// 设备异常时长
        /// </summary>
        [SugarColumn(ColumnName = "alarm_time")]
        public virtual int? AlarmTime { get; set; }

        /// <summary>
        /// 设备运行时长
        /// </summary>
        [SugarColumn(ColumnName = "run_time")]
        public virtual int? RunTime { get; set; }

        /// <summary>
        /// Buffer无生料时长
        /// </summary>
        [SugarColumn(ColumnName = "buffer_no_board_time")]
        public virtual int? BufferNoBoardTime { get; set; }

        /// <summary>
        /// Buffer有熟料时长
        /// </summary>
        [SugarColumn(ColumnName = "buffer_clinker_exist_time")]
        public virtual int? BufferClinkerExistTime { get; set; }

        /// <summary>
        /// 设备禁用时长
        /// </summary>
        [SugarColumn(ColumnName = "device_disable_time")]
        public virtual int? DeviceDisableTime { get; set; }

        /// <summary>
        /// Buffer有板到上板完成时长
        /// </summary>
        [SugarColumn(ColumnName = "buffer_raw_complete_time")]
        public virtual int? BufferRawCompleteTime { get; set; }

        /// <summary>
        /// 钻机有板到开始打板时长
        /// </summary>
        [SugarColumn(ColumnName = "drill_raw_exist_to_run_time")]
        public virtual int? DrillRawExistToRunTime { get; set; }

        /// <summary>
        /// Buffer自动时长
        /// </summary>
        [SugarColumn(ColumnName = "buffer_automatic_time")]
        public virtual int? BufferAutomaticTime { get; set; }

        /// <summary>
        /// 板方向检测时长
        /// </summary>
        [SugarColumn(ColumnName = "drill_board_direction_time")]
        public virtual int? DrillBoardDirectionTime { get; set; }

        /// <summary>
        /// pin检测时长
        /// </summary>
        [SugarColumn(ColumnName = "drill_test_pin_time")]
        public virtual int? DrillTestPinTime { get; set; }

        /// <summary>
        /// 刀具检测时长
        /// </summary>
        [SugarColumn(ColumnName = "drill_tool_evaluation_time")]
        public virtual int? DrillToolEvaluationTime { get; set; }

        /// <summary>
        /// 刀具吸尘报警次数
        /// </summary>
        [SugarColumn(ColumnName = "drill_novacuum_count")]
        public virtual int? DrillNoVacuumCount { get; set; }

        /// <summary>
        /// 刀具吸尘报警标准时长
        /// </summary>
        [SugarColumn(ColumnName = "drill_novacuum_config_time")]
        public virtual int? DrillNoVacuumStandardTime { get; set; }




        #region 刀具寿命报警更换 --单次耗时（秒）
        // <summary>
        /// 刀具寿命报警更换标准时长 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "tool_life_expored_change_standard_time")]
        public int? ToolLifeExporedChangeStandardTime { get; set; }




        #endregion

        #region 不切换料号时的换刀时长 --单次耗时（秒）
        /// <summary>
        /// 不切换料号时的换刀标准时长 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "no_switch_material_tool_change_standard_time")]
        public int? NoSwitchMaterialToolChangeStandardTime { get; set; }

        ///// <summary>
        ///// 不切换料号时的换刀实际时长 --单次耗时（秒）
        ///// </summary>
        //[SugarColumn(ColumnName = "no_switch_material_tool_change_real_time")]
        //public int? NoSwitchMaterialToolChangeRealTime { get; set; }

        /// <summary>
        /// /// <summary>
        /// 不切换料号时的换刀次数
        /// </summary>
        /// </summary>
        [SugarColumn(ColumnName = "no_switch_material_tool_change_count")]
        public int? NoSwitchMaterialToolChangeCount { get; set; }
        #endregion

        #region 切换料号时的换刀时长 --单次耗时（秒）
        /// <summary> 
        /// 切换料号时的换刀标准时长 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "switch_material_tool_change_standard_time")]
        public int? SwitchMaterialToolChangeStandardTime { get; set; }

        ///// <summary>
        ///// 不切换料号时的换刀实际时长 --单次耗时（秒）
        ///// </summary>
        //[SugarColumn(ColumnName = "switch_material_tool_change_real_time")]
        //public int? SwitchMaterialToolChangeRealTime { get; set; }

        /// <summary>
        /// /// <summary>
        /// 切换料号时的换刀次数
        /// </summary>
        /// </summary>
        [SugarColumn(ColumnName = "switch_material_tool_change_count")]
        public int? SwitchMaterialToolChangeCount { get; set; }
        #endregion


        #region PIN校正 --按班次平摊（秒）
        /// <summary>
        /// PIN校正标准时长 --按班次平摊（秒）
        /// </summary>
        [SugarColumn(ColumnName = "pin_revise_standard_time")]
        public int? PINReviseStandardTime { get; set; }

        ///// <summary>
        ///// PIN校正实际时长 --按班次平摊（秒）
        ///// </summary>
        //[SugarColumn(ColumnName = "pin_revise_real_time")]
        //public int? PINReviseRealTime { get; set; }

        /// <summary>
        /// PIN校正次数
        /// </summary>
        [SugarColumn(ColumnName = "pin_revise_count")]
        public int? PINReviseCount { get; set; }
        #endregion


        #region 检测摆幅扭力时长 --按班次平摊（秒）
        /// <summary>
        /// 检测摆幅扭力标准时长 --按班次平摊（秒）
        /// </summary>
        [SugarColumn(ColumnName = "defect_swing_torque_standard_time")]
        public int? DetectSwingTorqueStandardTime { get; set; }

        ///// <summary>
        ///// 检测摆幅扭力实际时长 --按班次平摊（秒）
        ///// </summary>
        //[SugarColumn(ColumnName = "defect_swing_torque_real_time")]
        //public int? DetectSwingTorqueRealTime { get; set; }

        /// <summary>
        /// 检测摆幅扭力次数
        /// </summary>
        [SugarColumn(ColumnName = "defect_swing_torque_count")]
        public int? DetectSwingTorqueCount { get; set; }
        #endregion

        #region 压力脚更换时长 --单次耗时（秒）
        /// <summary>
        /// 压力脚更换标准时长 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "pressure_foot_change_standard_time")]
        public int? PressureFootChangeStandardTime { get; set; }

        ///// <summary>
        ///// 压力脚更换实际时长 --单次耗时（秒）
        ///// </summary>
        //[SugarColumn(ColumnName = "pressure_foot_change_real_time")]
        //public int? PressureFootChangeRealTime { get; set; }

        /// <summary>
        /// 压力脚更换次数
        /// </summary>
        [SugarColumn(ColumnName = "pressure_foot_change_count")]
        public int? PressureFootChangeCount { get; set; }
        #endregion

        #region 多层板，最小层设定
        /// <summary>
        /// 多层板，最小层数设定标准层数   3
        /// </summary>
        [SugarColumn(ColumnName = "min_multilayer_boards_standard_value")]
        public int? MinMultilayerBoardsStandardValue { get; set; }

        ///// <summary>
        ///// 多层板，最小实设定实际层数
        ///// </summary>
        //[SugarColumn(ColumnName = "min_multilayer_boards_real_value")]
        //public int? MinMultilayerBoardRealValue { get; set; }

        #endregion

        #region 两层板等待首件耗时 --单次耗时（秒）
        /// <summary>
        /// 两层板等待首件标准耗时 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "two_boards_wait_first_result_standard_time")]
        public int? TwoBoardsWaitFirstResultStandardTime { get; set; }

        ///// <summary>
        ///// 两层板等待首件实际耗时 --单次耗时（秒）
        ///// </summary>
        //[SugarColumn(ColumnName = "two_boards_wait_first_result_real_time")]
        //public int? TwoBoardsWaitFirstResultRealTime { get; set; }

        /// <summary>
        /// 两层板等待首件次数
        /// </summary>
        [SugarColumn(ColumnName = "two_boards_wait_first_result_count")]
        public int? TwoBoardsWaitFirstResultCount { get; set; }
        #endregion


        #region 多层板等待首件结果耗时 --单次耗时（秒）
        /// <summary>
        /// 多层板等待首件结果标准耗时 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "multilayer_boards_wait_first_result_standard_time")]
        public int? MultilayerBoardWaitFirstResultStandardTime { get; set; }

        ///// <summary>
        ///// 多层板等待首件结果实际耗时 --单次耗时（秒）
        ///// </summary>
        //[SugarColumn(ColumnName = "multilayer_boards_wait_first_result_real_time")]
        //public int? MultilayerBoardWaitFirstResultRealTime { get; set; }

        /// <summary>
        /// 多层板等待首件结果次数
        /// </summary>
        [SugarColumn(ColumnName = "multilayer_boards_wait_first_result_count")]
        public int? MultilayerBoardWaitFirstResultCount { get; set; }
        #endregion

        /// <summary>
        /// Buffer手动时长
        /// </summary>
        [SugarColumn(ColumnName = "buffer_manual_time")]
        public virtual int? BufferManualTime { get; set; }

        /// <summary>
        /// 理论稼动率
        /// </summary>
        [SugarColumn(ColumnName = "theory_duty")]
        public virtual int? TheoryDuty { get; set; }

        /// <summary>
        /// 稼动率达成率
        /// </summary>
        [SugarColumn(ColumnName = "duty_rate")]
        public virtual int? DutyRate { get; set; }


        /// <summary>
        /// 必要时间
        /// </summary>
        [SugarColumn(ColumnName = "necessary_time")]
        public virtual int? NecessaryTime { get; set; }
    }

}
