namespace VegaIot.External.AgvEntity.STD;

public class StdCarStatus
{
    public class StdCarModel
    {
        /// <summary>
        /// 机器人编号
        /// </summary>
        public string robotCode { get; set; }

        /// <summary>
        /// 机器人方向 (范围 -180~360 度)
        /// </summary>
        public string robotDir { get; set; }

        /// <summary>
        /// 机器人 IP
        /// </summary>
        public string? robotIp { get; set; }

        /// <summary>
        /// 机器人电量, 范围: 0-100
        /// </summary>
        public string battery { get; set; }

        /// <summary>
        /// 机器人 x 坐标,单位:毫米
        /// </summary>
        public string posX { get; set; }

        /// <summary>
        /// 机器人 y 坐标,单位:毫米
        /// </summary>
        public string posY { get; set; }

        /// <summary>
        /// 机器人所在地图
        /// </summary>
        public string mapCode { get; set; }

        /// <summary>
        /// 机器人当前速度, 单位: mm/s
        /// </summary>
        public string speed { get; set; }

        /// <summary>
        /// 机器人当前状态
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 是否已被排除，被排除后不接受新任务（ 1-排除， 0-正常）
        /// </summary>
        public string exclType { get; set; }

        /// <summary>
        /// 是否暂停 0-否 1-是
        /// </summary>
        public string stop { get; set; }

        /// <summary>
        /// 背货架的编号
        /// </summary>
        public string podCode { get; set; }

        /// <summary>
        /// 背货架的方向
        /// </summary>
        public string podDir { get; set; }

        /// <summary>
        /// 背货架的方向
        /// </summary>
        public List<object> path { get; set; }
    }

    /// <summary>
    /// 小车状态返回值
    /// </summary>
    public class StdCarStateRequest
    {
        public string code { get; set; }

        public string message { get; set; }

        public string reqCode { get; set; }

        public List<StdCarModel>? data { get; set; }
    }

    public class StdCarStateResponseData
    {
        /// <summary>
        /// 机器人编号
        /// </summary>
        public string robotCode { get; set; }

        /// <summary>
        /// 机器人方向 (范围 -180~360 度)
        /// </summary>
        public string robotDir { get; set; }

        /// <summary>
        /// 机器人 IP
        /// </summary>
        public string? robotIp { get; set; }

        /// <summary>
        /// 机器人电量, 范围: 0-100
        /// </summary>
        public string battery { get; set; }

        /// <summary>
        /// 机器人 x 坐标,单位:毫米
        /// </summary>
        public string posX { get; set; }

        /// <summary>
        /// 机器人 y 坐标,单位:毫米
        /// </summary>
        public string posY { get; set; }

        /// <summary>
        /// 机器人所在地图
        /// </summary>
        public string mapCode { get; set; }

        /// <summary>
        /// 机器人当前速度, 单位: mm/s
        /// </summary>
        public string speed { get; set; }

        /// <summary>
        /// 机器人当前状态
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 是否已被排除，被排除后不接受新任务（ 1-排除， 0-正常）
        /// </summary>
        public string exclType { get; set; }

        /// <summary>
        /// 是否暂停 0-否 1-是
        /// </summary>
        public string stop { get; set; }

        /// <summary>
        /// 背货架的编号
        /// </summary>
        public string podCode { get; set; }

        /// <summary>
        /// 背货架的方向
        /// </summary>
        public string podDir { get; set; }

        /// <summary>
        /// 背货架的方向
        /// </summary>
        public List<object> path { get; set; }
    }

    /// <summary>
    /// 查询 AGV 状态 queryAgvStatus
    /// </summary>
    public class StdCarState
    {
        /// <summary>
        /// 请求编码
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public string? reqTime { get; set; } = DateTime.Now.ToString("yyyy-MM-ddHH:mm:ss");

        /// <summary>
        ///客户端编号，如 PDA，HCWMS 等 
        /// </summary>
        public string? clientCode { get; set; }

        /// <summary>
        /// 令牌号
        /// </summary>
        public string? tokenCode { get; set; }

        /// <summary>
        /// 地图编号
        /// </summary>
        public string mapCode { get; set; }
    }


    /// <summary>
    /// 给AGV下任务 genAgvSchedulingTask
    /// </summary>
    public class StdTaskRequest
    {
        /// <summary>
        /// 请求编码
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public string? reqTime { get; set; } = DateTime.Now.ToString("yyyy-MM-ddHH:mm:ss");

        /// <summary>
        ///客户端编号，如 PDA，HCWMS 等 
        /// </summary>
        public string? clientCode { get; set; }

        /// <summary>
        /// 令牌号
        /// </summary>
        public string? tokenCode { get; set; }

        /// <summary>
        /// 任务类型，与在 RCS-2000 端配置的主任务类型编号一致
        /// </summary>
        public string taskTyp { get; set; }

        /// <summary>
        /// 容器类型（叉车/CTU 专用）叉车项目必传
        /// </summary>
        public string? ctnrTyp { get; set; }

        /// <summary>
        /// 容器编号（叉车/CTU 专用）
        /// </summary>
        public string? ctnrCode { get; set; }

        /// <summary>
        /// 容器数量（叉车堆叠专用），默认值1，仅记录堆叠的数量不记录堆叠的每个容器号
        /// </summary>
        public string? ctnrNum { get; set; }

        /// <summary>
        /// 任务模式0-普通 move 1-出库 move 2-入库 move 3-移库 move
        /// </summary>
        public string? taskMode { get; set; }

        /// <summary>
        /// 工作位，一般为机台或工作台位置，与 RCS-2000 端配置的位置名称一 致, 工作位名称为字母\数字\或组合, 不超过 32 位
        /// </summary>
        public string wbCode { get; set; }


        /// <summary>
        /// 位置路径：AGV 关键路径位置集合,与任务类型中模板配置的位置路径一一对应。
        /// </summary>
        public List<StdLocation> positionCodePath { get; set; }

        /// <summary>
        /// 货架编号
        /// </summary>
        public string? podCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? podDir { get; set; }

        /// <summary>
        /// 货架类型, 传空时表示随机找个货 架
        /// 1: 代表不关心货架类型, 找到空货架即可.
        /// -2: 代表从工作位获取关联货架类型, 如果未配置, 只找空货架. 货架类型编号: 只找该货架类型的空货架
        /// </summary>
        public string? podTyp { get; set; }

        /// <summary>
        /// 物料批次或货架上的物料唯一编码,生成任务单时,货架与物料直接绑定 时使用
        /// </summary>
        public string? materialLot { get; set; }

        /// <summary>
        /// 物料类型, 仅移载机器人协议专用必填, 其它车型任务不填
        /// </summary>
        public string? materialType { get; set; }

        /// <summary>
        /// 优先级，从（1~127）级，最大优先级最高
        /// </summary>
        public string? priority { get; set; }

        /// <summary>
        /// 任务单号,选填, 不填系统自动生成，UUID 小于等于 64 位
        /// </summary>
        public string? taskCode { get; set; }

        /// <summary>
        /// AGV 编号
        /// </summary>
        public string? agvCode { get; set; }

        /// <summary>
        /// 组编  CTU 场景下用于按组出库，同组任务优先拼车。 如业务需要任务组间 或组内按顺序出库，则需调用料箱
        ///顺序出库（CTU）接口。潜伏式场景下，通过组号来管理顺序出库的顺序，组号小的优先出库。
        /// </summary>
        public string? groupId { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string? agvTyp { get; set; }

        /// <summary>
        /// 设备类型 区域/策略中挑选货架以及根据物料批次挑选货架时的先进先出规 则支持以下 4 个值，
        /// 1：按照货架到达储位的时间顺序， 先进先出 2：按照货架到达储位的时间顺序，先进后出
        /// 9：按照货架绑定物料批次的时间顺序，先进先出 10：按照货架绑定物料批次的时间顺序，先进后出
        /// </summary>
        public string? positionSelStrategy { get; set; }

        /// <summary>
        /// 自定义字段.JSON 格式
        /// </summary>
        public string? data { get; set; }
    }

    /// <summary>
    /// 给AGV下任务 genAgvSchedulingTask返回值
    /// </summary>
    public class StdTaskResponse
    {
        public string code { get; set; }//0:正常 ⾮0:异常码
        public string Message { get; set; }//提⽰信息.
        public string? data { get; set; }//自定义返回（返回任务单号）
        public string reqCode { get; set; }//  请求编码
    }


    /// <summary>
    /// 查询斯坦德的任务状态  queryTaskStatus
    /// </summary>
    public class QueryStdTaskStatus
    {
        public string reqCode { get; set; }

        /// <summary>
        /// 请求时间戳，格式: “yyyy-MM-dd HH:mm:ss”
        /// </summary>
        public string? reqTime { get; set; }

        /// <summary>
        /// 客户端编号
        /// </summary>
        public string? clientCode { get; set; }

        /// <summary>
        /// 客户端编号
        /// </summary>
        public string? tokenCode { get; set; }

        /// <summary>
        /// 任务单编号数组 任务单编号数组与AGV编号只能传其中之一，批量查询只能使用任务单编号数组
        /// </summary>
        public string? taskCodes { get; set; }

        /// <summary>
        /// AGV 编号任务编号数组与AGV编号只能传其中之一
        /// </summary>
        public string? agvCode { get; set; }
    }

    /// <summary>
    /// 返回查询的斯坦德任务状态返回值
    /// </summary>

    public class QueryStdTaskStatusRes
    {
        public string code { get; set; }

        public string message { get; set; }

        public string reqCode { get; set; }

        public List<StdTaskStatus> data { get; set; }
    }

    public class StdTaskStatus
    {
        public string taskCode { get; set; }

        public string taskType { get; set; }

        public string taskStutas { get; set; }

        public string? agvCode { get; set; }
    }

    public class StdLocation
    {
        public string positionCode { get; set; }
        public string type { get; set; }
    }


}
