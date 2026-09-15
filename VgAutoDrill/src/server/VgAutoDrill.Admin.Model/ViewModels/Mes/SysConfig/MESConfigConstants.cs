namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig
{
    /// <summary>
    /// Mes系统配置项
    /// </summary>
    public class MESConfigConstants
    {
        /// <summary>
        /// 中控系统是否在维护
        /// </summary>
        public const string CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN = "CentralControlSystemIsMaintain";

        /// <summary>
        /// 中转区是否在维护
        /// </summary>
        public const string TRANSFER_LOCATION_IS_MAINTAIN = "TransferLocationIsMaintain";

        /// <summary>
        /// 中控系统已上报调度的超时设定(秒)
        /// </summary>
        public const string CENTRAL_CONTROL_SYSTEM_CREATED_TIMEOUT = "CentralControlSystemCreatedTimeout";

        /// <summary>
        /// 中控系统已分配的调度的超时设定(秒)
        /// </summary>
        public const string CENTRAL_CONTROL_SYSTEM_ALLOCATED_TIMEOUT = "CentralControlSystemAllocatedTimeout";

        /// <summary>
        /// AGV设备状态
        /// </summary>
        public const string AGV_DEVICE_NOW_STATUS = "AGVDeviceNowStatus";

        /// <summary>
        /// 是否显示告警等及时信息
        /// </summary>
        public const string IS_SHOW_TIMELY_INFORMATION = "IsShowTimelyInformation";

        /// <summary>
        /// AGV最近调度记录条数
        /// </summary>
        public const string AGV_LATEST_SCHEDULE_COUNT = "AGVLatestScheduleCount";

        /// <summary>
        /// 钻孔任务轴数
        /// </summary>
        public const string DRILL_TASK_SHAFT_COUNT = "DrillTaskShaftCount";

        /// <summary>
        /// 在线钻机待做任务显示个数
        /// </summary>
        public const string DRILL_DEVICE_TASK_SHOW_COUNT = "DrillDeviceTaskShowCount";

        /// <summary>
        /// 在线AGV板料信息及最近调度记录显示个数
        /// </summary>
        public const string AGV_DEVICE_SILO_INFO_SHOW_COUNT = "AGVDeviceSiloInfoShowCount";

        /// <summary>
        /// 外部工单轮询处理间隔时间（秒）
        /// </summary>
        public const string EXTERNAL_WORKORDER_INTERVAL = "ExternalWorkOrderInterval";

        /// <summary>
        /// 外部工单导入指定层数的数据
        /// </summary>
        public const string QUERY_LAYER_NUM_LIST = "QueryLayerNumList";

        /// <summary>
        /// 是否校验料仓被使用
        /// </summary>
        public const string IS_CHECK_SILO_USED = "IsCheckSiloUsed";

        /// <summary>
        /// 加载调度记录几天之内的数据
        /// </summary>
        public const string LOAD_SCHEDULE_TASK_SETTINGSDAYS_DATA = "LoadScheduleTaskSettingDaysData";

        /// <summary>
        /// 是否开启库存信息查询
        /// </summary>
        public const string ENABLE_KINWONG_STOCK_INFO_QUERY = "EnableKinwongStockInfoQuery";

        /// <summary>
        /// 库存信息查询地址
        /// </summary>
        public const string URL_KINWONG_STOCK_INFO_QUERY = "URLKinwongStockInfoQuery";

        /// <summary>
        /// 库存信息查询区域或策略编码
        /// </summary>
        public const string STOCK_INFO_QUERY_TARGET_POSAREA_CODE = "StockInfoQueryTargetPosAreaCode";

        /// <summary>
        /// 库存信息查询目标区域
        /// </summary>
        public const string STOCK_INFO_QUERY_TARGET_POSAREA = "StockInfoQueryTargetPosArea";

        /// <summary>
        /// 孔数
        /// </summary>
        public const string DRILL_COUNT = "DrillCount";

        /// <summary>
        /// 单趟耗时 单位min
        /// </summary>
        public const string SINGLE_TRIP_TIME = "SingleTripTime";

        /// <summary>
        /// 生成钻孔任务的方式
        /// </summary>
        public const string PRODUCE_TASK_TYPE = "ProduceTaskType";

        /// <summary>
        /// 是否刷新库存
        /// </summary>
        public const string REFRESH_STOCK = "RefreshStock";

        /// <summary>
        /// 是否自动审批钻孔任务
        /// </summary>
        public const string AUTO_VETTING_DRILL_TASK = "AutoVettingDrillTask";

        /// <summary>
        /// 导入的数据默认状态为:可用
        /// </summary>
        public const string IMPORT_STATUS = "ImportStatus";

        /// <summary>
        /// 导入外部工单，是否直接生成排产任务
        /// </summary>
        public const string DIRECT_BUILD_TASK = "DirectBuildTask";

        /// <summary>
        /// 钻机交互位置
        /// </summary>
        public const string DEFAULT_INTERACTION_POSITION = "DefaultInteractionPosition";

        /// <summary>
        /// 是否开启获取钻带参数
        /// </summary>
        public const string JINGWANG_TRANSFER_DRILL_FILE_ENABLE = "JingWangTransferDrillFileEnable";

        /// <summary>
        /// 获取钻带参数请求路径
        /// </summary>
        public const string JINGWANG_TRANSFER_DRILL_FILE_URL = "JingWangTransferDrillFileUrl";

        /// <summary>
        /// 是否开启导入WIP外部工单
        /// </summary>
        public const string JIANGXI_KINWONG_WIP_ENABLE = "JiangXiKinWongWIPEnable";

        /// <summary>
        /// 导入WIP外部工单请求路径
        /// </summary>
        public const string JIANGXI_KINWONG_WIP_URL = "JiangXiKinWongWIPUrl";

        /// <summary>
        /// 是否开启SetMoveInLot
        /// </summary>
        public const string JINGWANG_SET_MOVE_IN_LOT_ENABLE = "JingWangSetMoveInLotEnable";

        /// <summary>
        /// SetMoveInLot请求路径
        /// </summary>
        public const string JINGWANG_SET_MOVE_IN_LOT_URL = "JingWangSetMoveInLotUrl";

        /// <summary>
        /// 是否开启SetMoveOutLot
        /// </summary>
        public const string JINGWANG_SET_MOVE_OUT_LOT_ENABLE = "JingWangSetMoveOutLotEnable";

        /// <summary>
        /// SetMoveOutLot请求路径
        /// </summary>
        public const string JINGWANG_SET_MOVE_OUT_LOT_URL = "JingWangSetMoveOutLotUrl";

        /// <summary>
        /// 是否开启SetTrackInLot
        /// </summary>
        public const string JINGWANG_SET_TRACK_IN_LOT_ENABLE = "JingWangSetTrackInLotEnable";

        /// <summary>
        /// SetTrackInLot请求路径
        /// </summary>
        public const string JINGWANG_SET_TRACK_IN_LOT_URL = "JingWangSetTrackInLotUrl";

        /// <summary>
        /// 是否开启SetTrackOutLot
        /// </summary>
        public const string JINGWANG_SET_TRACK_OUT_LOT_ENABLE = "JingWangSetTrackOutLotEnable";

        /// <summary>
        /// SetTrackOutLot请求路径
        /// </summary>
        public const string JINGWANG_SET_TRACK_OUT_LOT_URL = "JingWangSetTrackOutLotUrl";

        /// <summary>
        /// 获取料仓实时板料信息请求路径
        /// </summary>
        public const string CENTRAL_GET_RACK_PANELS_URL = "CentralGetRackPanelsUrl";

        /// <summary>
        /// 下发设备指令请求路径
        /// </summary>
        public const string CENTRAL_ALLOTS_DEVICE_COMMAND = "CentralAllotsDeviceCommand";

        /// <summary>
        /// 下发板料数据到设备请求路径
        /// </summary>
        public const string CENTRAL_ALLOTS_PANEL_DATA = "CentralAllotsPanelData";

        /// <summary>
        /// 是否开启移动料仓
        /// </summary>
        public const string JINGWANG_GEN_AGV_SCHEDULING_TASK_ENABLE = "JingWangGenAgvSchedulingTaskEnable";

        /// <summary>
        /// 移动料仓请求路径
        /// </summary>
        public const string JINGWANG_GEN_AGV_SCHEDULING_TASK_URL = "JingWangGenAgvSchedulingTaskUrl";

        /// <summary>
        /// 移动料仓：呼叫站点
        /// </summary>
        public const string JINGWANG_GEN_AGV_SCHEDULING_TASK_WBCODE = "JingWangGenAgvSchedulingTask_wbCode";

        /// <summary>
        /// 移动料仓：任务类型
        /// </summary>
        public const string JINGWANG_GEN_AGV_SCHEDULING_TASK_TASKTYP = "JingWangGenAgvSchedulingTask_taskTyp";

        /// <summary>
        /// 获取料仓数据请求路径
        /// </summary>
        public const string CENTRAL_GET_SIMPLE_LOCATIONS = "CentralGetSimpleLocations";

        /// <summary>
        /// 是否显示非WIP外部工单
        /// </summary>
        public const string ENABLE_NOT_WIP_DATA = "EnableNotWIPData";

        /// <summary>
        /// 是否过滤接口无法识别的传入区域
        /// </summary>
        public const string STOCK_INFO_QUERY_WITHOUT_AREA_ENABLE = "StockInfoQueryWithoutAreaEnable";

        /// <summary>
        /// 开始上料请求路径
        /// </summary>
        public const string XIANJIN_SET_BEGIN_LOAD_PANEL = "XianJinSetBeginLoadPanel";

        /// <summary>
        /// 下熟料请求路径
        /// </summary>
        public const string XIANJIN_UNDER_CLINKER_PANEL = "XianJinUnderClinkerPanel";

        /// <summary>
        /// 下熟料结束请求路径
        /// </summary>
        public const string XIANJIN_SET_COMPLETE_UNDER_CLINKER_PANEL = "XianJinSetCompleteUnderClinkerPanel";

        /// <summary>
        /// 上传mes转仓接口请求路径
        /// </summary>
        public const string JINGWANG_KW_AGV_STOCK_IN_URL = "JingWangkwAGVStockInUrl";

        /// <summary>
        /// 上传mes暂停该lot请求路径
        /// </summary>
        public const string JINGWANG_HOLD_LOT_URL = "JingWangHoldLotUrl";

        /// <summary>
        /// 下发海康任务(崇达)
        /// </summary>
        public const string CHONGDA_GEN_AGV_SCHEDULING_TASK_URL = "ChongDaGenAgvSchedulingTaskUrl";

        /// <summary>
        /// 查询海康AGV状态(崇达)
        /// </summary>
        public const string CHONGDA_QUERY_AGV_STATUS_URL = "ChongDaQueryAgvStatusUrl";

        /// <summary>
        /// 地图位置信息同步(崇达)
        /// </summary>
        public const string CHONGDA_SYNC_MAP_DATAS_URL = "ChongDaSyncMapDatasUrl";

        /// <summary>
        /// 查询任务状态(崇达)
        /// </summary>
        public const string CHONGDA_QUERY_TASK_STATUS_URL = "ChongDaQueryTaskStatusUrl";

        /// <summary>
        /// 继续执行任务(崇达)
        /// </summary>
        public const string CHONGDA_CONTINUE_TASK_URL = "ChongDaContinueTaskUrl";

        /// <summary>
        /// 取消任务(崇达)
        /// </summary>
        public const string CHONGDA_CANCEL_TASK_URL = "ChongDaCancelTaskUrl";

        /// <summary>
        /// 监控任务完成超时时间(单位分钟)
        /// </summary>
        public const string MONITOR_TASK_COMPLETE_TIMEOUT = "MonitorTaskCompleteTimeOut";

        /// <summary>
        /// 监控任务完成超时时间(单位分钟)
        /// </summary>
        public const string MONITOR_TASK_FAIL_TIMEOUT = "MonitorTaskFailTimeOut";

        /// <summary>
        /// 海康接口任务类型模板
        /// </summary>
        public const string HIK_TASK_TYP = "HiktaskTyp";

        /// <summary>
        /// 班次
        /// </summary>
        public const string SAILINGS = "Sailings";

        /// <summary>
        /// 人工录入工单是否开启获取钻带参数
        /// </summary>
        public const string JINGWANG_TRANSFER_DRILL_FILE_ENABLE_WORKORDER = "JingWangTransferDrillFileEnable_WorkOrder";

        /// <summary>
        /// 添加工单，校验物料数量是否超过自动工单的数量
        /// </summary>
        public const string JINGWANG_VERIFY_ITEM_NUM_WORKORDER = "JingWangVerifyItemNum_WorkOrder";

        /// <summary>
        /// 库存信息钻孔区域编码
        /// </summary>
        public const string STOCK_INFO_QUERY_DRILLAREA_CODE = "StockInfoQueryDrillAreaCode";

        /// <summary>
        /// 稼动率汇总表数据，保留时长（单位：月）
        /// </summary>
        public const string RATE_RECORD_SUMMARY_RETAIN_TIME = "RateRecordSummaryRetainTime";

        /// <summary>
        /// 稼动率明细数据，保留时长（单位：月）
        /// </summary>
        public const string RATE_FACTOR_RETAIN_TIME = "RateFactorRetainTime";

        /// <summary>
        /// 博敏接口
        /// </summary>
        public const string BOMIN_DRILL_RECIPES = "BominDrillRecipes";

        /// <summary>
        /// 是否开启自动配刀
        /// </summary>
        public const string ENABLE_AUTO_CUTTER = "EnableAutoCutter";

        /// <summary>
        /// 是否多机型
        /// </summary>
        public const string HAVE_MULTI_MACHINES = "HaveMultiMachines";

        /// <summary>
        /// 机型相关配置
        /// </summary>
        public const string MACHINE_TYPE_CONFIG = "MachineTypeConfig";

        /// <summary>
        /// 手动呼叫agv上料
        /// </summary>
        public const string UP_RAW_MATERIAL = "UploadRawMaterial";

        /// <summary>
        /// 手动呼叫agv退空盘
        /// </summary>
        public const string UNLOAD_EMPTY_FORK = "UnloadEmptyFork";

        /// <summary>
        /// 上机解绑/下机绑定
        /// </summary>
        public const string BIND_AND_UN_BIND = "BindAndUnbindAnd";

        /// <summary>
        /// 手动呼叫agv叫空盘
        /// </summary>
        public const string UPLOAD_EMPTY_FORK = "UploadEmptyFork";

        /// <summary>
        /// 手动呼叫agv下料
        /// </summary>
        public const string UNLOAD_CLINKER = "UnloadClinker";


        /// <summary>
        /// agv运行状态
        /// </summary>
        public const string AGV_RUN_STATUS = "Agv_Run_Status";


        /// <summary>
        /// 是否启用独立分区设置
        /// </summary>
        public const string ENABLE_STAND_ALONE_PARTITION_SETTINGS = "EnableStandAlonePartitionSettings";

        /// <summary>
        /// 是否启用通知第三方系统板料与钻机加工关系
        /// </summary>
        public const string ENABLE_NOTICE_EXTERNAL_SYSTEM_PANELS_DATA = "EnableNoticeExternalSystemPanelsData";

        /// <summary>
        /// 通知第三方系统板料与钻机加工关系的接口地址
        /// </summary>
        public const string URL_NOTICE_EXTERNAL_SYSTEM_PANELS_DATA = "URLNoticeExternalSystemPanelsData";

        /// <summary>
        /// 迁移历史调度数据开关
        /// </summary>

        public const string TRANSFER_SCHEDULE_HISTORY_DATA_SWITCH = "TransferScheduleHistoryDataSwitch";

        /// <summary>
        /// 获取配刀组计划apt文件路径url
        /// </summary>
        public const string CUTTER_GROUP_APT_FILE_URL = "CutterGroupAptFileUrl";
        /// <summary>
        /// 迁移料仓历史任务数据开关
        /// </summary>
        public const string TRANSSFER_TRANSPORTATION_HISTORY_SWITCH = "TransferTransportationHistorySwitch";
        /// <summary>
        /// 是否需要检查刀盒码
        /// </summary>
        public const string NEED_CHECK_BOXCODES = "NeedCheckBoxCodes";
    }
}