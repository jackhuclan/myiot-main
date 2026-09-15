namespace VgDeviceGateway.Devices.Common
{
    public class Order
    {
        public Order()
        {
            id = 0;
            upper_id = string.Empty;
            appoint_execute_time = string.Empty;
            create_time = string.Empty;
            appoint_vehicle_id = 0;
            done_time = string.Empty;
            start_station_name = string.Empty;
            end_station_name = string.Empty;
            execute_time = string.Empty;
            execute_vehicle_id = 0;
            executing_index = 0;
            mission = new List<Mission>();
            order_state = string.Empty;
            order_type = string.Empty;
            priority = 0;
            reason = string.Empty;
            source = string.Empty;
            user_id = 0;
            template_id = 0;
            eta = 0;
            distance = 0;
            total_distance = 0;
            lockVehicleKey = string.Empty;
            lockStatus = 0;
            vehicle_group = 0;
        }

        /// <summary>
        /// 订单预约的执行时间
        /// </summary>
        public string appoint_execute_time { get; set; }

        /// <summary>
        /// 订单预约的车辆id
        /// </summary>
        public int appoint_vehicle_id { get; set; }

        /// <summary>
        /// 订单生成的时间
        /// </summary>
        public string create_time { get; set; }

        /// <summary>
        /// 剩余距离
        /// </summary>
        public int distance { get; set; }

        /// <summary>
        /// 订单完成的时间
        /// </summary>
        public string done_time { get; set; }

        /// <summary>
        /// 最后一个移动子任务的站点名称
        /// </summary>
        public string end_station_name { get; set; }

        /// <summary>
        /// 剩余时间
        /// </summary>
        public int eta { get; set; }

        /// <summary>
        /// 订单开始执行的时间
        /// </summary>
        public string execute_time { get; set; }

        /// <summary>
        /// 执行任务的车辆ID
        /// </summary>
        public int execute_vehicle_id { get; set; }

        /// <summary>
        /// 正在执行的子任务序号
        /// </summary>
        public int executing_index { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 可选,锁车的状态，在使用了锁车功能时使用，1是锁定状态，0是未锁定状态
        /// </summary>
        public int lockStatus { get; set; }

        /// <summary>
        /// 锁车的key值，在使用了锁车功能时使用
        /// </summary>
        public string lockVehicleKey { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<Mission> mission { get; set; }

        /// <summary>
        /// 订单状态
        /// QUEUEING: 正在排队
        /// CANCELLED: 订单被取消
        /// EXECUTING: 正在执行
        /// FAILED: 订单未能成功执行完
        /// DELETED: 订单被删除
        /// SUCCESS: 订单被成功执行完
        /// HELD: 在执行中手动暂停任务（人主动暂停）
        /// REJECTED: 暂停执行（移除订单池）
        /// HANG: 因为错误导致暂停执行
        /// QUEUED: 调度系统重启后，处于正在排队的订单变为此状态
        /// </summary>
        public string order_state { get; set; }

        /// <summary>
        /// NORMAL: 普通任务
        /// CHARGE: 充电任务或去空闲站点
        /// CMD: ?
        /// </summary>
        public string order_type { get; set; }

        /// <summary>
        /// 任务优先级
        /// </summary>
        public int priority { get; set; }

        /// <summary>
        /// 任务失败原因
        /// </summary>
        public string reason { get; set; }

        /// <summary>
        /// 任务来源
        /// FMS: 调度系统
        /// CALLS: 呼叫器
        /// WMS: 物料管理系统
        /// UI: UI界面
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 第一个移动子任务的站点名称
        /// </summary>
        public string start_station_name { get; set; }

        /// <summary>
        /// 通过订单模版生成的订单模版id
        /// </summary>
        public int template_id { get; set; }

        /// <summary>
        /// 任务总共的距离
        /// </summary>
        public int total_distance { get; set; }

        /// <summary>
        /// 上层系统生成的UUID
        /// </summary>
        public string upper_id { get; set; }

        /// <summary>
        /// 生成订单的用户id
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 指定的车型组id
        /// </summary>
        public int vehicle_group { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public string msg { get; set; }
    }

    public class Mission
    {
        public Mission()
        {
            type = string.Empty;
            destination = 0;
            map_id = 0;
            action_name = string.Empty;
            action_id = 0;
            action_param1 = 0;
            action_param2 = 0;
            execute_time = string.Empty;
            finish_time = string.Empty;
            result_str = string.Empty;
            fail_strategy = string.Empty;
            failValue = string.Empty;
            state = "NA";
        }

        /// <summary>
        /// 动作id
        /// </summary>
        public int action_id { get; set; }

        /// <summary>
        /// 动作模板名
        /// </summary>
        public string action_name { get; set; }

        /// <summary>
        /// 动作参数1
        /// </summary>
        public int action_param1 { get; set; }

        /// <summary>
        /// 动作参数2
        /// </summary>
        public int action_param2 { get; set; }

        /// <summary>
        /// 任务目标站点id
        /// </summary>
        public int destination { get; set; }

        /// <summary>
        /// 子任务开始执行时间,格式"yyyy-MM-dd HH:mm:ss"
        /// </summary>
        public string execute_time { get; set; }

        /// <summary>
        /// 动作任务失败触发的处理类型的值
        /// </summary>
        public string failValue { get; set; }

        /// <summary>
        /// 动作任务失败触发的处理类型
        /// </summary>
        public string fail_strategy { get; set; }

        /// <summary>
        /// 子任务结束时间,格式"yyyy-MM-dd HH:mm:ss"
        /// </summary>
        public string finish_time { get; set; }

        /// <summary>
        /// 地图id
        /// </summary>
        public int map_id { get; set; }

        /// <summary>
        /// 任务返回结果,格式为-> 错误码@任务结果返回值
        /// </summary>
        public string result_str { get; set; }

        /// <summary>
        /// 子任务执行状态 NA: 初始化状态 PROCESSING: 正在执行中
        ///FINISHED: 执行结束 FAILED: 执行失败 CANCELLED: 已取消
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 任务类型 action: 动作任务 move: 移动任务
        /// </summary>
        public string type { get; set; }

        public bool move;
        public bool act;
    }

    public class VehicleInfo
    {
        public VehicleInfo()
        {
            id = 0;
            nickname = string.Empty;
            port = 0;
            ip_addr = string.Empty;
            mac_addr = string.Empty;
            vehicle_type = 0;
            load_state = 0;
            last_online_time = string.Empty;
            is_alive = 0;
            serial_no = string.Empty;
            sys_state = string.Empty;
            location_state = string.Empty;
            execute_order_id = 0;
            pos_x = 0;
            pos_y = 0;
            pos_yaw = 0;
            battery = 0;
            battery_state = string.Empty;
            is_online = 0;
            cur_station_no = 0;
            emergency_state = string.Empty;
            map_name = string.Empty;
            sros_version = string.Empty;
            src_version = string.Empty;
            hardware_version = string.Empty;
            break_switch_status = string.Empty;
            proc_state = string.Empty;
            hardware_state = string.Empty;
            move_state = string.Empty;
            action_state = string.Empty;
            control_mode = 0;
            fault_codes = string.Empty;
            last_error_code = string.Empty;
            action_error_code = string.Empty;
        }

        /// <summary>
        /// 自动生成的数据库ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 车辆识别名，需要保证在系统内唯一
        /// </summary>
        public string nickname { get; set; }

        public int port { get; set; }

        /// <summary>
        /// 车辆IP地址
        /// </summary>
        public string ip_addr { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string mac_addr { get; set; }

        /// <summary>
        /// 无效字段
        /// </summary>
        public int vehicle_type { get; set; }

        /// <summary>
        /// 车辆搭载机构载货状态，数值的每1 bit表示一个货位的载货状态
        /// </summary>
        public int load_state { get; set; }

        /// <summary>
        /// 车辆最近一次在线的时间
        /// </summary>
        public string last_online_time { get; set; }

        /// <summary>
        /// 车辆是否已连接到FMS
        /// </summary>
        public int is_alive { get; set; }

        /// <summary>
        /// UNKNOWN: "连接未建立的状态"
        /// UNAVAILABLE: 连接已经建立,车辆不处于error状态但是也
        /// 不能执行任务
        /// IDLE: 没有执行动作也没有执行移动的状态
        /// EXECUTING: 正在执行动作或者移动
        /// ERROR: 车辆状态出错
        /// CHARGING: 充电状态
        /// PAUSE: 避障状态
        /// </summary>
        public string sys_state { get; set; }

        /// <summary>
        /// 车辆定位状态
        /// LOCATION_STATE_RUNNING: 定位正常
        /// ERROR: 没有定位
        ///  UNKNOWN: 初始化中
        /// </summary>
        public string location_state { get; set; }

        /// <summary>
        /// 正在执行的订单ID
        /// </summary>
        public int execute_order_id { get; set; }

        /// <summary>
        /// 所在地图中x坐标
        /// </summary>
        public int pos_x { get; set; }

        /// <summary>
        /// 所在地图中y坐标
        /// </summary>
        public int pos_y { get; set; }

        /// <summary>
        /// 所在地图中朝向角度
        /// </summary>
        public decimal pos_yaw { get; set; }

        /// <summary>
        /// 车辆剩余电量
        /// </summary>
        public int battery { get; set; }

        /// <summary>
        ///  BATTERY_NA: 状态不可用
        ///  BATTERY_CHARGING: 正在充电
        ///  BATTERY_NO_CHARGING: 未充电
        /// </summary>
        public string battery_state { get; set; }

        /// <summary>
        /// 车辆是否可以被调度系统调度
        ///  1: 可被调度
        ///  0: 不可被调度
        /// </summary>
        public int is_online { get; set; }

        /// <summary>
        /// 车辆所在站点id
        /// </summary>
        public int cur_station_no { get; set; }

        /// <summary>
        /// 车辆急停状态
        ///  STATE_EMERGENCY_NA: 紧急状态不可用
        ///  OK: 不处于急停状态
        ///  CAN_NOT_RECOVER: 不可恢复急停状态(急停开关被拍下去
        ///  CAN_RECOVER: 可恢复急停状态
        /// </summary>
        public string emergency_state { get; set; }

        /// <summary>
        /// 车辆当前加载的地图名称
        /// </summary>
        public string map_name { get; set; }

        /// <summary>
        /// UNMOVABLE: 解抱闸状态
        /// MOVABLE: 抱闸状态
        /// </summary>
        public string break_switch_status { get; set; }

        /// <summary>
        /// sros版本号
        /// </summary>
        public string sros_version { get; set; }

        /// <summary>
        /// src版本号
        /// </summary>
        public string src_version { get; set; }

        /// <summary>
        /// 硬件版本号
        /// </summary>
        public string hardware_version { get; set; }

        /// <summary>
        /// 车辆序列号
        /// </summary>
        public string serial_no { get; set; }

        /// <summary>
        /// 车辆内部状态
        /// UNAVAILABLE: 初始状态
        ///  IDLE: 空闲
        ///  INNER_FORCE_IDLE: 系统强制空闲
        /// USER_FORCE_IDLE: 用户强制空闲
        /// AWAITING_ORDER: 等待分配新任务
        /// PROCESSING_ORDER: 任务正在执行
        /// </summary>
        public string proc_state { get; set; }

        /// <summary>
        /// 硬件状态
        /// H_STATE_ZERO: 硬件默认值
        /// H_STATE_INITIALING: 硬件初始化
        /// H_STATE_OK: 硬件正常
        /// H_STATE_ERROR: 硬件异常
        /// </summary>
        public string hardware_state { get; set; }

        /// <summary>
        /// 动作任务错误码
        /// </summary>
        public string action_error_code { get; set; }

        /// <summary>
        /// 车辆移动状态
        /// MT_NA: "空闲",
        /// MT_WAIT_FOR_START: "等待开始执行",
        /// MT_RUNNING: "执行中",
        /// MT_PAUSED: "暂停中",
        /// MT_FINISHED: "已结束",
        /// MT_IN_CANCEL: "已取消",
        /// MT_WAIT_FOR_ACK: "等待上层系统回复",
        /// MT_WAIT_FOR_CHECKPOINT: "交通管制等待中",
        /// MT_PAUSED_OBSTACLE: "遇到障碍物暂停中"
        /// </summary>
        public string move_state { get; set; }

        /// <summary>
        /// 动作任务执行状态
        /// AT_ZERO: "空闲",
        /// AT_WAIT_FOR_START: "等待开始执行",
        /// AT_RUNNING: "执行中",
        /// AT_PAUSED: "暂停中",
        /// AT_FINISHED: "已结束",
        /// AT_IN_CANCEL: "已取消",
        /// AT_WAIT_FOR_ACK: "等待上层系统回复"
        /// </summary>
        public string action_state { get; set; }

        /// <summary>
        /// 控制模式
        /// 1: 手动控制
        /// 0: 自动控制
        /// </summary>
        public int control_mode { get; set; }

        /// <summary>
        /// 车辆错误码,用英文逗号分割
        /// </summary>
        public string fault_codes { get; set; }

        /// <summary>
        /// 最近一个子任务执行失败的错误码
        /// </summary>
        public string last_error_code { get; set; }
    }

    public class VehiclesRequest
    {
        public VehiclesRequest()
        {
            is_alive = "all";
            is_online = "all";
            is_working = "all";
            page = 0;
            perpage = 0;
        }

        /// <summary>
        ///  all（默认）: 全部车辆
        ///   alive: 在线车辆
        ///    dead: 离线车辆
        /// </summary>
        public string is_alive { get; set; }

        /// <summary>
        /// all（默认）: 全部车辆
        ///  online: 可被调度车辆
        ///  offline:不可被调度车辆
        /// </summary>
        public string is_online { get; set; }

        /// <summary>
        /// all（默认）: 全部车辆
        ///  working: 正在执行任务车辆
        ///   free:空闲车辆
        /// </summary>
        public string is_working { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 每页数据个数
        /// </summary>
        public int perpage { get; set; }
    }

    public class VehiclesResponse
    {
        public VehiclesResponse()
        {
            page = 0;
            perpage = 0;
            total = 0;
            totalPage = 0;
            vehicles = new List<VehicleInfo>();
        }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 每页数据个数
        /// </summary>
        public int perpage { get; set; }

        /// <summary>
        /// 所有数据的个数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 总共页数
        /// </summary>
        public int totalPage { get; set; }

        public List<VehicleInfo> vehicles { get; set; }
    }
}
