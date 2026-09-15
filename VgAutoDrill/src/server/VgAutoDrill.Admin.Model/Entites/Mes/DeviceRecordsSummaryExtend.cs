using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    ///  设备记录汇总扩展表
    /// </summary>
    [SugarTable("t_device_records_summary_extend")]
    public class DeviceRecordsSummaryExtend : BaseEntity
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [SugarColumn(ColumnName = "device_code")]
        public virtual string? DeviceCode { get; set; }

        #region 刀具寿命报警更换 --单次耗时（秒）
        // <summary>
        /// 刀具寿命报警更换标准时长 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "tool_life_expored_change_standard_time")]
        public int? ToolLifeExporedChangeStandardTime { get; set; }

        /// <summary>
        /// 刀具寿命报警更换实际时长 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "tool_life_expored_change_real_time")]
        public int? ToolLifeExporedChangeRealTime { get; set; }
        #endregion

        #region 不切换料号时的换刀时长 --单次耗时（秒）
        /// <summary>
        /// 不切换料号时的换刀标准时长 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "no_switch_material_tool_change_standard_time")]
        public int? NoSwitchMaterialToolChangeStandardTime { get; set; }

        /// <summary>
        /// 不切换料号时的换刀实际时长 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "no_switch_material_tool_change_real_time")]
        public int? NoSwitchMaterialToolChangeRealTime { get; set; }

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

        /// <summary>
        /// 不切换料号时的换刀实际时长 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "switch_material_tool_change_real_time")]
        public int? SwitchMaterialToolChangeRealTime { get; set; }

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

        /// <summary>
        /// PIN校正实际时长 --按班次平摊（秒）
        /// </summary>
        [SugarColumn(ColumnName = "pin_revise_real_time")]
        public int? PINReviseRealTime { get; set; }

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

        /// <summary>
        /// 检测摆幅扭力实际时长 --按班次平摊（秒）
        /// </summary>
        [SugarColumn(ColumnName = "defect_swing_torque_real_time")]
        public int? DetectSwingTorqueRealTime { get; set; }

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

        /// <summary>
        /// 压力脚更换实际时长 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "pressure_foot_change_real_time")]
        public int? PressureFootChangeRealTime { get; set; }

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

        /// <summary>
        /// 多层板，最小实设定实际层数
        /// </summary>
        [SugarColumn(ColumnName = "min_multilayer_boards_real_value")]
        public int? MinMultilayerBoardRealValue { get; set; }

        #endregion

        #region 两层板等待首件耗时 --单次耗时（秒）
        /// <summary>
        /// 两层板等待首件标准耗时 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "two_boards_wait_first_result_standard_time")]
        public int? TwoBoardsWaitFirstResultStandardTime { get; set; }

        /// <summary>
        /// 两层板等待首件实际耗时 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "two_boards_wait_first_result_real_time")]
        public int? TwoBoardsWaitFirstResultRealTime { get; set; }

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

        /// <summary>
        /// 多层板等待首件结果实际耗时 --单次耗时（秒）
        /// </summary>
        [SugarColumn(ColumnName = "multilayer_boards_wait_first_result_real_time")]
        public int? MultilayerBoardWaitFirstResultRealTime { get; set; }

        /// <summary>
        /// 多层板等待首件结果次数
        /// </summary>
        [SugarColumn(ColumnName = "multilayer_boards_wait_first_result_count")]
        public int? MultilayerBoardWaitFirstResultCount { get; set; }
        #endregion

    }
}
