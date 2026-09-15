namespace VgAutoDrill.Fundation.Iot;

public static class Events
{
    public const string REQUEST_AGV_LOAD_PANEL_ONLY = "REQUEST_AGV_LOAD_PANEL_ONLY";
    public const string REQUEST_AGV_UNLOAD_PANEL_ONLY = "REQUEST_AGV_UNLOAD_PANEL_ONLY";
    public const string REQUEST_AGV_LOAD_PANEL_THEN_UNLOAD_PANEL = "REQUEST_AGV_LOAD_PANEL_THEN_UNLOAD_PANEL";
    public const string REQUEST_AGV_UNLOAD_PANEL_THEN_LOAD_PANEL = "REQUEST_AGV_UNLOAD_PANEL_THEN_LOAD_PANEL";
    public const string REQUEST_AGV_LOAD_SILO_ONLY = "REQUEST_AGV_LOAD_SILO_ONLY";
    public const string REQUEST_AGV_UNLOAD_SILO_ONLY = "REQUEST_AGV_UNLOAD_SILO_ONLY";
    public const string REQUEST_AGV_LOAD_SILO_THEN_UNLOAD_SILO = "REQUEST_AGV_LOAD_SILO_THEN_UNLOAD_SILO";
    public const string REQUEST_AGV_UNLOAD_SILO_THEN_LOAD_SILO = "REQUEST_AGV_UNLOAD_SILO_THEN_LOAD_SILO";

    public const string REQUEST_AGV_CHANGE_CUTTER = "REQUEST_AGV_CHANGE_CUTTER";
    public const string SCAN_BOARD = "SCAN_BOARD";
    public const string SCAN_PANEL_CODE = "SCAN_PANEL_CODE";

    /// <summary>
    /// 支持的产品名称，比如drill,agv,pin,unpin,rawbuffer,processedbuffer
    /// </summary>
    public static class Products
    {
        public const string DRILL = "drill";
        public const string AGV = "agv";
        public const string PIN = "pin";
        public const string UNPIN = "unpin";
        public const string RAWSTAGINGDESK = "rawstagingdesk";
        public const string PROCESSEDSTAGINGDESK = "processedstagingdesk";
        public const string GEARSHAPING = "gearshaping";
        public const string SILOSHELVES = "siloshelves";

        public static bool Contains(string productId)
        {
            switch (productId)
            {
                case DRILL:
                case AGV:
                case PIN:
                case UNPIN:
                case RAWSTAGINGDESK:
                case PROCESSEDSTAGINGDESK:
                case GEARSHAPING:
                case SILOSHELVES:
                    return true;
                default:
                    return false;
            }
        }
    }

    public static class Drill
    {
        public const string BUFFER_LACK_RAW_MATERIAL_EVENT = "BUFFER_LACK_RAW_MATERIAL";
        public const string BUFFER_EXIST_CLINKER_EVENT = "BUFFER_EXIST_CLINKER";
        public const string BUFFER_START_LOAD_RAW_MATERIAL_EVENT = "BUFFER_START_LOAD_RAW_MATERIAL";
        public const string BUFFER_END_LOAD_RAW_MATERIAL_EVENT = "BUFFER_END_LOAD_RAW_MATERIAL";
        public const string BUFFER_START_UNLOAD_CLINKER_EVENT = "BUFFER_START_UNLOAD_CLINKER";
        public const string BUFFER_END_UNLOAD_CLINKER_EVENT = "BUFFER_END_UNLOAD_CLINKER";
        public const string DRILL_START_OPEN_MUSHROOM_EVENT = "DRILL_START_OPEN_MUSHROOM";
        public const string DRILL_HALT_EVENT = "DRILL_HALT";
        public const string DRILL_START_LOAD_FILE_EVENT = "DRILL_START_LOAD_FILE";
        public const string DRILL_REQUEST_RECIPE_EVENT = "DRILL_REQUEST_RECIPE";
        public const string DRILL_START_WORK_EVENT = "DRILL_START_WORK";
        public const string DRILL_ON_P1_PARKING_POSITION_EVENT = "DRILL_ON_P1_PARKING_POSITION";
        public const string DRILL_ON_P2_PARKING_POSITION_EVENT = "DRILL_ON_P2_PARKING_POSITION";
        public const string DRILL_OPEN_DOOR_EVENT = "DRILL_OPEN_DOOR_EVENT";
        public const string DRILL_SCAN_CODE_EVENT = "DRILL_SCAN_CODE_EVENT";
        public const string BUFFER_UNCONNECT_EVENT = "BUFFER_UNCONNECT_EVENT";
        public const string DRILL_UNCONNECT_EVENT = "DRILL_UNCONNECT_EVENT";
        public const string DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT = "DRILL_PARAMETER_NOT_CONTAIN_POSITION__EVENT";
        public const string DRILL_PARAMETER_POSITION_FALT_EVENT = "DRILL_PARAMETER_POSITION_FALT_EVENT";
        public const string DRILL_LOADFILE_FALT_EVENT = "DRILL_LOADFILE_FALT_EVENT";
        public const string DRILL_PUT_UP_EXTEND_UNCONNECT_EVENT = "DRILL_PUT_UP_EXTEND_UNCONNECT_EVENT";
        public const string BUFFER_UNLOAD_RAW_MATERIAL_TO_DRIL_EVENT = "BUFFER_UNLOAD_RAW_MATERIAL_TO_DRIL_EVENT";
        public const string BUFFER_ON_AGV_POSITION_EVENT = "BUFFER_ON_AGV_POSITION_EVENT";
        public const string BUFFER_AXIS_LOAD_RAW_MATERIAL_END_EVENT = "BUFFER_AXIS_LOAD_RAW_MATERIAL_END_EVENT";
        public const string BUFFER_AXIS_UNLOAD_CLINKER_END_EVENT = "BUFFER_AXIS_UNLOAD_CLINKER_END_EVENT";
        public const string BUFFER_WARNING_EVENT = "BUFFER_WARNING_EVENT";
        public const string BUFFER_ON_AUTOMATIC_EVENT = "BUFFER_ON_AUTOMATIC_EVENT";
        public const string BUFFER_ON_MANUAL_EVENT = "BUFFER_ON_MANUAL_EVENT";
        public const string BUFFER_RAW_MATERIAL_LAYER_LOAD_END_EVENT = "BUFFER_RAW_MATERIAL_LAYER_LOAD_END_EVENT";
        public const string BUFFER_CLINKER_LAYER_UNLOAD_END_EVENT = "BUFFER_CLINKERL_LAYER_UNLOAD_END_EVENT";
        public const string CHECK_PROGRAM_AND_DIA_EVENT = "CHECK_PROGRAM_AND_DIA";

        public const string BUFFER_LACK_RAW_MATERIAL_EVENT_NAME = "BUFFER缺生料";//呼叫AGV
        public const string BUFFER_EXIST_CLINKER_EVENT_NAME = "BUFFER有熟料";//呼叫AGV
        public const string BUFFER_START_LOAD_RAW_MATERIAL_EVENT_NAME = "BUFFER开始上生料";
        public const string BUFFER_END_LOAD_RAW_MATERIAL_EVENT_NAME = "BUFFER结束上生料";
        public const string BUFFER_START_UNLOAD_CLINKER_EVENT_NAME = "BUFFER开始下熟料";
        public const string BUFFER_END_UNLOAD_CLINKER_EVENT_NAME = "BUFFER结束下熟料";
        public const string DRILL_START_OPEN_MUSHROOM_EVENT_NAME = "钻机打开蘑菇头";
        public const string DRILL_HALT_EVENT_NAME = "钻机急停";
        public const string DRILL_START_LOAD_FILE_EVENT_NAME = "钻机开始加载文件";
        public const string DRILL_REQUEST_RECIPE_EVENT_NAME = "钻机请求配方";
        public const string DRILL_START_WORK_EVENT_NAME = "钻机开始工作";
        public const string DRILL_UNLOAD_PANEL_THEN_LOAD_PANEL_EVENT_NAME = "钻机先下料再上料";
        public const string DRILL_ON_P1_PARKING_POSITION_EVENT_NAME = "钻机到达P1停车位";
        public const string DRILL_ON_P2_PARKING_POSITION_EVENT_NAME = "钻机到达P2停车位";
        public const string DRILL_OPEN_DOOR_EVENT_NAME = "钻机开门";
        public const string DRILL_SCAN_CODE_EVENT_NAME = "钻机扫条码";
        public const string BUFFER_UNCONNECT_EVENT_NAME = "BUFFER连接未建立";
        public const string DRILL_UNCONNECT_EVENT_NAME = "钻机连接未建立";
        public const string DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT_NAME = "钻机上下料方法参数中未包含位置信息";
        public const string DRILL_PARAMETER_POSITION_FALT_EVENT_NAME = "钻机上下料方法参数中传入的位置信息不正确";
        public const string DRILL_LOADFILE_FALT_EVENT_NAME = "钻机加载文件失败";
        public const string DRILL_PUT_UP_EXTEND_UNCONNECT_EVENT_NAME = "钻机提升机构未建立";
        public const string BUFFER_UNLOAD_RAW_MATERIAL_TO_DRIL_EVENT_NAME = "BUFFER下生料到钻机结束信号";
        public const string BUFFER_ON_AGV_POSITION_EVENT_NAME = "BUFFER到换料位信号";
        public const string BUFFER_AXIS_LOAD_RAW_MATERIAL_END_EVENT_NAME = "BUFFER轴上生料结束";
        public const string BUFFER_AXIS_UNLOAD_CLINKER_END_EVENT_NAME = "BUFFER轴下熟料结束";
        public const string BUFFER_WARNING_EVENT_NAME = "BUFFER异常报警";
        public const string BUFFER_ON_AUTOMATIC_EVENT_NAME = "BUFFER自动状态";
        public const string BUFFER_ON_MANUAL_EVENT_NAME = "BUFFER手动状态";
        public const string BUFFER_RAW_MATERIAL_LAYER_LOAD_END_EVENT_NAME = "BUFFER生料层上料结束";
        public const string BUFFER_CLINKER_LAYER_UNLOAD_END_EVENT_NAME = "BUFFER熟料层下料结束";
    }
    public static class Pin
    {
        public const string PIN_REQUEST_LOAD_RAW_MATERIAL_EVENT = "PIN_REQUEST_LOAD_RAW_MATERIAL";//呼叫AGV
        public const string PIN_REQUEST_UNLOAD_PROCESSED_MATERIAL_EVENT = "PIN_REQUEST_UNLOAD_PROCESSED_MATERIAL";//呼叫AGV
        public const string PIN_STEP_ERROR_EVENT = "PIN_STEP_ERROR";

        public const string PIN_REQUEST_LOAD_RAW_MATERIAL_NAME = "叠扳机上料";//呼叫AGV
        public const string PIN_REQUEST_UNLOAD_PROCESSED_MATERIAL_NAME = "叠扳机下料";//呼叫AGV
        public const string PIN_STEP_ERROR_NAME = "叠扳机发生异常";
    }
    public static class UnPin
    {
        public const string UNPIN_REQUEST_LOAD_PROCESSED_MATERIAL_EVENT = "UNPIN_REQUEST_LOAD_PROCESSED_MATERIAL";//呼叫AGV
        public const string UNPIN_REQUEST_UNLOAD_RAW_MATERIAL_EVENT = "UNPIN_REQUEST_UNLOAD_RAW_MATERIAL";//呼叫AGV
        public const string UNPIN_STEP_ERROR_EVENT = "UNPIN_STEP_ERROR";

        public const string UNPIN_REQUEST_LOAD_PROCESSED_MATERIAL_NAME = "拆板机上料";
        public const string UNPIN_REQUEST_UNLOAD_RAW_MATERIAL_NAME = "拆板机下料";
        public const string UNPIN_STEP_ERROR_NAME = "拆板机发生异常";
    }
    public static class AGV
    {
        public const string AGV_CHARGE_EVENT = "AGV_CHARGE";
        public const string AGV_PUT_DOWN_SILO_EVENT = "AGV_PUT_DOWN_SILO";
        public const string AGV_PICK_UP_SILO_EVENT = "AGV_PICK_UP_SILO";
        public const string AGV_MOVE_EVENT = "AGV_MOVE";
        public const string AGV_STATUSREPORT_EVENT = "AGV_STATUSREPORT";
        public const string AGV_MOVE_ISMOVING_EVENT = "AGV_MOVE_ISMOVING";
        public const string AGV_MOVE_ISARRIVED_EVENT = "AGV_MOVE_ISARRIVED";

        public const string AGV_PREPARELOADOK_EVENT = "AGV_PREPARELOADOK";
        public const string AGV_INVOKELOADOK_EVENT = "AGV_INVOKELOADOK";
        public const string AGV_COMPLETELOADOK_EVENT = "AGV_COMPLETELOADOK";
        public const string AGV_PREPAREUNLOADOK_EVENT = "AGV_PREPAREUNLOADOK";
        public const string AGV_INVOKEUNLOADOK_EVENT = "AGV_INVOKEUNLOADOK";
        public const string AGV_COMPLETEUNLOADOK_EVENT = "AGV_COMPLETEUNLOADOK";
        public const string AGV_UPDATE_PLCSILOINFO_EVENT = "AGV_UPDATE_PLCSILOINFO_EVENT";

        public const string AGV_CHARGE_NAME = "AGV充电";
        public const string AGV_PUT_DOWN_SILO_NAME = "AGV下料仓";
        public const string AGV_PICK_UP_SILO_NAME = "AGV上料仓";
        public const string AGV_MOVE_NAME = "AGV移动";
        public const string AGV_STATUSREPORT_NAME = "AGV状态上报";
        public const string AGV_MOVE_ISMOVING_NAME = "AGV移动中···";
        public const string AGV_MOVE_ISARRIVED_NAME = "AGV到达";
        public const string AGV_PREPARELOADOK_NAME = "PREPARELOADOK";
        public const string AGV_INVOKELOADOK_NAME = "INVOKELOADOK";
        public const string AGV_COMPLETELOADOK_NAME = "COMPLETELOADOK";
        public const string AGV_PREPAREUNLOADOK_NAME = "PREPAREUNLOADOK";
        public const string AGV_INVOKEUNLOADOK_NAME = "INVOKEUNLOADOK";
        public const string AGV_COMPLETEUNLOADOK_NAME = "COMPLETEUNLOADOK";
        public const string AGV_UPDATE_PLCSILOINFO_NAME = "更新PLC料仓信息";
    }
    public static class RawBuffer
    {
        public const string RAWBUFFER_REQUEST_LOAD_RAW_MATERIAL = "RAWBUFFER_REQUEST_LOAD_RAW_MATERIAL";
    }
    public static class ProcessedBuffer
    {
        public const string PROCESSEDBUFFER_REQUEST_LOAD_RAW_MATERIAL = "PROCESSEDBUFFER_REQUEST_LOAD_RAW_MATERIAL";
    }
    public static class RawStagingDesk
    {
        public const string RAWSTAGINGDESK_REQUEST_LOAD_RAW_MATERIAL_EVENT = "RAWSTAGINGDESK_REQUEST_LOAD_RAW_MATERIAL";
        public const string RAWSTAGINGDESK_REQUEST_UNLOAD_RAW_MATERIAL_EVENT = "RAWSTAGINGDESK_REQUEST_UNLOAD_RAW_MATERIAL";

        public const string RAWSTAGINGDESK_REQUEST_LOAD_RAW_MATERIAL_NAME = "料仓允许进生料暂存台";//呼叫AGV
        public const string RAWSTAGINGDESK_REQUEST_UNLOAD_RAW_MATERIAL_NAME = "料仓允许出生料暂存台";//呼叫AGV

    }

    public static class ProcessedStagingDesk
    {
        public const string PROCESSEDSTAGINGDESK_REQUEST_LOAD_PROCESSED_MATERIAL_EVENT = "PROCESSEDSTAGINGDESK_REQUEST_LOAD_PROCESSED_MATERIAL";
        public const string PROCESSEDSTAGINGDESK_REQUEST_UNLOAD_PROCESSED_MATERIAL_EVENT = "PROCESSEDSTAGINGDESK_REQUEST_UNLOAD_PROCESSED_MATERIAL";

        public const string PROCESSEDSTAGINGDESK_REQUEST_LOAD_PROCESSED_MATERIAL_NAME = "料仓允许进熟料暂存台";//呼叫AGV
        public const string PROCESSEDSTAGINGDESK_REQUEST_UNLOAD_PROCESSED_MATERIAL_NAME = "料仓允许出熟料暂存台";//呼叫AGV

    }

    public static class SiloShelves
    {
        public const string SHELVES_REQUEST_LOAD_SILO_MATERIALL_EVENT = "SHELVES_REQUEST_LOAD_SILO_MATERIAL";
        public const string SHELVES_REQUEST_UNLOAD_SILO_MATERIAL_EVENT = "SHELVES_REQUEST_UNLOAD_SILO_MATERIAL";

        public const string SHELVES_REQUEST_LOAD_SILO_MATERIAL_NAME = "料仓允许进货架仓";//呼叫AGV
        public const string SHELVES_REQUEST_UNLOAD_SILO_MATERIAL_NAME = "料仓允许出货架仓";//呼叫AGV
    }

}
