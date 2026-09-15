namespace VgAutoDrill.Fundation.Iot;

public class ErrorCodes
{
    public static class Sys
    {
        public const string SUCCESS = "SUCCESS";
        public const string FAIL = "FAIL";
        public const string TAKE_LOCK_FAILED = "获取锁失败";

        public const string UNKNOWN_PRODUCT_CODE = "UNKNOWN_PRODUCT";
        public const string UNKNOWN_PRODUCT_MESSAGE = "不支持的设备类型";

        public const string OFFLINE_CODE = "OFFLINE_CODE";
        public const string OFFLINE_MESSAGE = "设备已离线";

        public const string DUPLICATE_SERVICE_INVOKED_CODE = "DUPLICATE_SERVICE_INVOKED_CODE";
        public const string DUPLICATE_SERVICE_INVOKED_MESSAGE = "重复呼叫";

        public const string WRONG_DEVICE_CODE = "WRONG_DEVICE_CODE";
        public const string WRONG_DEVICE_MESSAGE = "找不到设备";

        public const string MISSING_TRACE_ID_CODE = "MISSING_TRACE_ID_CODE";
        public const string MISSING_TRACE_ID_MESSAGE = "missing trace id";

        public const string WRONG_SERVICE_CODE = "WRONG_SERVICE_CODE";
        public const string WRONG_SERVICE_MESSAGE = "错误的SERVICE ID";

        public const string WRONG_EVENT_CODE = "WRONG_EVENT_CODE";
        public const string WRONG_EVENT_MESSAGE = "错误的EVENT ID";

        public const string PLC_UNCONNECT_CODE = "PLC_UNCONNECT_CODE";
        public const string PLC_UNCONNECT_MESSAGE = "PLC未连接";

        public const string EXCEPTION_CODE = "EXCEPTION_CODE";
        public const string EXCEPTION_MESSAGE = "";

        public const string UNKONW_DEVICE_PROPERTY_CODE = "UNKONW_DEVICE_PROPERTY_CODE";
        public const string UNKONW_DEVICE_PROPERTY_MESSAGE = "不支持的设备属性";

        public const string HTTPREQUEST_TIMEOUT_CODE = "HTTPREQUEST_TIMEOUT_CODE";
        public const string HTTPREQUEST_TIMEOUT_MESSAGE = "API调用超时";

        public const string NOT_FIND_DEVICE_CODE = "NOT_FIND_DEVICE_CODE";
        public const string NOT_FIND_DEVICE_MESSAGE = "未找到设备";

        public const string WEBAPI_RETURNNULL_CODE = "WEBAPI_RETURNNULL_CODE";
        public const string WEBAPI_RETURNNULL_MESSAGE = "WEBAPI返回值为NULL";

        public const string DUPLICATE_EVENT_SCHEDULE_REQUEST_CODE = "DUPLICATE_EVENT_SCHEDULE_REQUEST_CODE";
        public const string DUPLICATE_EVENT_SCHEDULE_REQUEST_MESSAGE = "重复的请求调度事件";

        //当前已有生料[{existRawNum}]，已经满足任务安排的数量[{task.NowWadCount}]！不需要再发起叫料！
        public const string UNNEED_CALL_RAW_PANEL = "UNNEED_CALL_RAW_PANEL";

        public const string WRONG_CONFIG_ID = "WRONG_CONFIG_ID";
        public const string WRONG_CONFIG_ID_MESSAGE = "错误的 ConfigId";

        public const string WRONG_SERVICE_PARAM = "WRONG_SERVICE_PARAM";
        public const string WRONG_SERVICE_PARAM_MESSAGE = "错误的服务参数{0}";

        public const string WRONG_UNSUPPORTED_SERVICE = "WRONG_UNSUPPORTED_SERVICE";
        public const string WRONG_UNSUPPORTED_MESSAGE = "不支持此服务{0}";

        public const string UNKNOWN_CODE = "UNKNOWN_CODE";
        public const string UNKNOWN_MESSAGE = "未知的错误";
    }

    public static class AGV
    {
        public const string AGV_PLC_ERRORCODE_1_MESSAGE = "提升伺服故障";
        public const string AGV_PLC_ERRORCODE_2_MESSAGE = "提升轴错误";
        public const string AGV_PLC_ERRORCODE_3_MESSAGE = "提升伺服FB块执行错误";
        public const string AGV_PLC_ERRORCODE_4_MESSAGE = "提升伺服正限位错误";
        public const string AGV_PLC_ERRORCODE_5_MESSAGE = "提升伺服负限位错误";
        public const string AGV_PLC_ERRORCODE_6_MESSAGE = "提升伺服没有使能ok错误";
        public const string AGV_PLC_ERRORCODE_7_MESSAGE = "提升伺服没有回原点ok错误";
        public const string AGV_PLC_ERRORCODE_8_MESSAGE = "提升伺服到位延时报警-自动上生料";
        public const string AGV_PLC_ERRORCODE_9_MESSAGE = "提升伺服到位延时报警-自动下熟料";
        public const string AGV_PLC_ERRORCODE_11_MESSAGE = "送料驱动故障";
        public const string AGV_PLC_ERRORCODE_12_MESSAGE = "送料轴错误";
        public const string AGV_PLC_ERRORCODE_13_MESSAGE = "送料步进FB块执行错误";
        public const string AGV_PLC_ERRORCODE_14_MESSAGE = "送料步进正限位错误";
        public const string AGV_PLC_ERRORCODE_15_MESSAGE = "送料步进负限位错误";
        public const string AGV_PLC_ERRORCODE_16_MESSAGE = "送料步进没有使能ok错误";
        public const string AGV_PLC_ERRORCODE_17_MESSAGE = "送料步进没有回原点ok错误";
        public const string AGV_PLC_ERRORCODE_21_MESSAGE = "推进驱动故障";
        public const string AGV_PLC_ERRORCODE_22_MESSAGE = "推进轴错误";
        public const string AGV_PLC_ERRORCODE_23_MESSAGE = "推进步进FB块执行错误";
        public const string AGV_PLC_ERRORCODE_24_MESSAGE = "推进步进正限位错误";
        public const string AGV_PLC_ERRORCODE_25_MESSAGE = "推进步进负限位错误";
        public const string AGV_PLC_ERRORCODE_26_MESSAGE = "推进步进没有使能ok错误";
        public const string AGV_PLC_ERRORCODE_27_MESSAGE = "推进步进没有回原点ok错误";
        public const string AGV_PLC_ERRORCODE_28_MESSAGE = "推进步进到位延时报警—自动上生料";
        public const string AGV_PLC_ERRORCODE_30_MESSAGE = "推进步进手动定位去零点位置超时报警";
        public const string AGV_PLC_ERRORCODE_31_MESSAGE = "推进步进绝对定位去零点位置超时报警";
        public const string AGV_PLC_ERRORCODE_32_MESSAGE = "推进步进回原点超时报警";
        public const string AGV_PLC_ERRORCODE_40_MESSAGE = "设备急停信号异常";
        public const string AGV_PLC_ERRORCODE_43_MESSAGE = "中控控制设备急停";
        public const string AGV_PLC_ERRORCODE_44_MESSAGE = "自动模式下提升伺服不在使能状态";
        public const string AGV_PLC_ERRORCODE_45_MESSAGE = "自动模式下送料步进不在使能状态";
        public const string AGV_PLC_ERRORCODE_46_MESSAGE = "自动模式下推进步进不在使能状态";
        public const string AGV_PLC_ERRORCODE_47_MESSAGE = "代理通讯异常";
        public const string AGV_PLC_ERRORCODE_48_MESSAGE = "上料超时";
        public const string AGV_PLC_ERRORCODE_49_MESSAGE = "下料超时";
        public const string AGV_PLC_ERRORCODE_50_MESSAGE = "轴通讯异常";
        public const string AGV_PLC_ERRORCODE_51_MESSAGE = "半自动上生料超时";
        public const string AGV_PLC_ERRORCODE_52_MESSAGE = "半自动下熟料超时";
        public const string AGV_PLC_ERRORCODE_53_MESSAGE = "上生料超时报警";
        public const string AGV_PLC_ERRORCODE_54_MESSAGE = "下熟料超时报警";
        public const string AGV_PLC_ERRORCODE_55_MESSAGE = "下熟料流程超过所设定时间报警";
        public const string AGV_PLC_ERRORCODE_56_MESSAGE = "上生料流程超过所设定时间报警";
        public const string AGV_PLC_ERRORCODE_58_MESSAGE = "推进步进下熟料到位超时";
        public const string AGV_PLC_ERRORCODE_57_MESSAGE = "初始化超时报警";
        public const string AGV_PLC_ERRORCODE_60_MESSAGE = "扫码器通讯异常";
        public const string AGV_PLC_ERRORCODE_61_MESSAGE = "扫码器TCP数据发送异常";
        public const string AGV_PLC_ERRORCODE_62_MESSAGE = "扫码器TCP数据接收异常";
        public const string AGV_PLC_ERRORCODE_63_MESSAGE = "AGV提升机构此时不应有料仓";
        public const string AGV_PLC_ERRORCODE_64_MESSAGE = "自动取料仓时未识别到料仓二维码";
        public const string AGV_PLC_ERRORCODE_65_MESSAGE = "自动取料仓时读取的二维码不匹配";
        public const string AGV_PLC_ERRORCODE_66_MESSAGE = "AGV提升机构上没有料仓";
        public const string AGV_PLC_ERRORCODE_67_MESSAGE = "检侧到有料，提升机构不可移动";

        public const string AGV_Work_FinishedWork_Code = "AGV_Work_FinishedWork";
        public const string AGV_Work_FinishedWork_MESSAGE = "结束当前任务";
    }
}
