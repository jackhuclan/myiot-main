using System.Text.Json.Serialization;

namespace VgDeviceGateway.Devices.Common.Agv.Hik;

public class HikCarStatus
{
    public class HikCarModel
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
    public class HikCarStateRequest
    {
        public string code { get; set; }

        public string message { get; set; }

        public string reqCode { get; set; }

        public List<HikCarModel>? data { get; set; }
    }

    public class HikCarStateResponseData
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
    public class HikCarState
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
    public class HikTaskRequest
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
        public object[] positionCodePath { get; set; }

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
    public class HikTaskResponse
    {
        public string code { get; set; }//0:正常 ⾮0:异常码
        public string Message { get; set; }//提⽰信息.
        public string? data { get; set; }//自定义返回（返回任务单号）
        public string reqCode { get; set; }//  请求编码
    }

    /// <summary>
    /// 查询海康的任务状态  queryTaskStatus
    /// </summary>
    public class QueryHikTaskStatus
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
    /// 返回查询的海康任务状态返回值
    /// </summary>

    public class QueryHikTaskStatusRes
    {
        public string code { get; set; }

        public string message { get; set; }

        public string reqCode { get; set; }

        public List<HikTaskStatus> data { get; set; }
    }

    public class HikTaskStatus
    {
        public string taskCode { get; set; }

        public string taskType { get; set; }

        public string taskStutas { get; set; }

        public string? agvCode { get; set; }
    }

    public class HikLocation
    {
        public string positionCode { get; set; }
        public string type { get; set; }
    }

    /// <summary>
    /// 继续下一个任务
    /// </summary>
    public class HikContinueTask
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
        /// 工作位
        /// </summary>
        public string? wbCode { get; set; }

        /// <summary>
        /// 货架号
        /// </summary>
        public string? podCode { get; set; }

        /// <summary>
        /// AGV 编号
        /// </summary>
        public string? agvCode { get; set; }

        /// <summary>
        /// 任务单号
        /// </summary>
        public string? taskCode { get; set; }

        /// <summary>
        /// 下一个子任务的序列，指定第几个子任务开始执行
        /// </summary>
        public string? taskSeq { get; set; }

        /// <summary>
        /// 下一个位置信息，在任务类型中配置外部设置时需要传入，否则不需要设置
        /// </summary>
        public object? nextPositionCode { get; set; }
    }

    /// <summary>
    /// 取消任务
    /// </summary>
    public class HikCancelTask
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
        /// 取消类型0 表示：取消后货架直接放地上, 1 表示：AGV 仍然背着货架， 根据
        /// </summary>
        public string? forceCancel { get; set; }

        /// <summary>
        /// forcecancel=1 时有意义，回库区域编号，如果为空，采用货架配置的库区
        /// </summary>
        public string? matterArea { get; set; }

        /// <summary>
        /// 取消该 AGV 正在执行的任务单
        /// </summary>
        public string? agvCode { get; set; }

        /// <summary>
        /// 任务单编号, 取消该任务单
        /// </summary>
        public string? taskCode { get; set; }
    }

    /// <summary>
    /// 继续/取消任务 返回的消息
    /// </summary>
    public class HikCancelOrContinueReq
    {
        public string Code { get; set; }
        public string message { get; set; }
        public string reqCode { get; set; }
    }

    /// <summary>
    /// 查询区域中的库存信息
    /// </summary>
    public class StockInfoQuery
    {
        /// <summary>
        /// 请求编号
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 请求时间截
        /// </summary>
        public string? reqTime { get; set; }

        /// <summary>
        /// 客户端编号
        /// </summary>
        public string? clientCode { get; set; }

        /// <summary>
        /// 目标区域，支持区域编号或者策略编号，格式：区域编号${04},策略编号${02}
        /// </summary>
        public string targetPosArea { get; set; }
    }

    /// <summary>
    /// 查询区域中的库存信息回复的信息
    /// </summary>
    public class StockInfoQueryRes
    {
        public string code { get; set; }
        public string message { get; set; }

        public string reqCode { get; set; }

        public System.Text.Json.JsonDocument data { get; set; }
    }

    /// <summary>
    /// 查询区域中的库存信息回复的信息
    /// </summary>
    public class StockInfoQueryResData
    {
        /// <summary>
        /// 物料号
        /// </summary>
        public string? product { get; set; }

        /// <summary>
        /// 托盘号
        /// </summary>
        public string? podCode { get; set; }

        /// <summary>
        /// 所在位置
        /// </summary>
        public string? address { get; set; }

        /// <summary>
        /// 所在位置区域
        /// </summary>
        public string? addressName { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string? lot { get; set; }

        /// <summary>
        /// 本托盘数量
        /// </summary>
        public decimal? qty { get; set; }

        /// <summary>
        /// 系统数量
        /// </summary>
        public decimal? sysQty { get; set; }

        /// <summary>
        /// 差数
        /// </summary>
        public decimal? difQty { get; set; }
    }

    /// <summary>
    /// 查询区域中的库存信息返回值
    /// </summary>
    public class ResMsg
    {
        public string code { get; set; }
        public string message { get; set; }
        public string reqCode { get; set; }

        //任务号
        public string data { get; set; }
    }

    /// <summary>
    /// 给地上爬的机器人指派任务的返回消息
    /// </summary>
    public class HikAgvCallBack
    {
        public string reqCode { get; set; }

        public string reqTime { get; set; }

        public decimal? cooX { get; set; }

        public decimal? cooY { get; set; }

        public string currentPositionCode { get; set; }

        public string? mapCode { get; set; }

        public string? mapDataCode { get; set; }

        public string? stgBinCode { get; set; }

        public string method { get; set; }

        public string? podCode { get; set; }

        public string? podDir { get; set; }

        public string? materialLot { get; set; }

        public string? materialType { get; set; }

        public string robotCode { get; set; }

        public string taskCode { get; set; }

        public string? wbCode { get; set; }

        public string? ctnrCode { get; set; }

        public string? ctnrType { get; set; }

        public string? roadWayCode { get; set; }

        public string? seq { get; set; }

        public string? eqpCode { get; set; }

        /// <summary>
        /// data
        /// </summary>
        //public MaterialData data { get; set; }
        public object? data { get; set; }
        [JsonIgnore]
        public MaterialData materialdData { get; set; } = new MaterialData();

        public HikAgvCallBack()
        {
            reqCode = string.Empty;
            reqTime = string.Empty;
            cooX = 0;
            cooY = 0;
            currentPositionCode = string.Empty;
            data = data;
            mapCode = string.Empty;
            mapDataCode = string.Empty;
            stgBinCode = string.Empty;
            method = string.Empty;
            podCode = string.Empty;
            podDir = string.Empty;
            materialLot = string.Empty;
            materialType = string.Empty;
            robotCode = string.Empty;
            taskCode = string.Empty;
            wbCode = string.Empty;
            ctnrCode = string.Empty;
            ctnrType = string.Empty;
            roadWayCode = string.Empty;
            seq = string.Empty;
            eqpCode = string.Empty;
        }
    }

    public class MaterialData
    {
        /// <summary>
        /// 真实搬运任务单号
        /// </summary>
        public string? realTaskCode { get; set; }

        /// <summary>
        /// 运输工序 下工序
        /// </summary>
        public string? carryPro { get; set; }

        /// <summary>
        /// 批次组号(母lot)
        /// </summary>
        public string? materialGroupCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string materialLot { get; set; }

        /// <summary>
        /// 物料号
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 载具类型Q8(大车)，Q6(小车)
        /// </summary>
        public string? carrierTyp { get; set; }

        /// <summary>
        /// 批次系统数量
        /// </summary>
        public string? sysLotNum { get; set; }

        /// <summary>
        /// 批次本托盘数量
        /// </summary>
        public string podLotNum { get; set; }

        /// <summary>
        /// 批次状态（只能传OK/NG）
        /// </summary>
        public string? lotStatus { get; set; }

        /// <summary>
        /// 是否包含报废板子（0-否 1–是）
        /// </summary>
        public string? includeScrap { get; set; }

        /// <summary>
        /// 托盘状态：0-空托盘 1-正常物料 2-异常物料
        /// </summary>
        public string? podStatus { get; set; }

        public MaterialData()
        {
            realTaskCode = string.Empty;

            carryPro = string.Empty;

            materialGroupCode = string.Empty;

            materialLot = string.Empty;

            materialCode = string.Empty;

            carrierTyp = string.Empty;

            sysLotNum = string.Empty;

            podLotNum = string.Empty;

            lotStatus = string.Empty;

            includeScrap = string.Empty;

            podStatus = string.Empty;
        }
    }

    /// <summary>
    /// 返回给HikAGVcallback的消息
    /// </summary>
    public class HikAgvCallBackRes
    {
        public string resultCode { get; set; }
        public string resultMsg { get; set; }
    }

    public class HikArrivedEntity
    {
        public string reqCode { get; set; }

        public string reqTime { get; set; }

        public decimal? cooX { get; set; }

        public decimal? cooY { get; set; }

        public string currentPositionCode { get; set; }

        public string? mapCode { get; set; }

        public string? mapDataCode { get; set; }

        public string? stgBinCode { get; set; }

        public string method { get; set; }

        public string? podCode { get; set; }

        public string? podDir { get; set; }

        public string? materialLot { get; set; }

        public string? materialType { get; set; }

        public string robotCode { get; set; }

        public string taskCode { get; set; }

        public string? wbCode { get; set; }

        public string? ctnrCode { get; set; }

        public string? ctnrType { get; set; }

        public string? roadWayCode { get; set; }

        public string? seq { get; set; }

        public string? eqpCode { get; set; }

        public object? data { get; set; }
        [JsonIgnore]
        public MaterialData materialdData { get; set; } = new MaterialData();

        public HikArrivedEntity()
        {
            reqCode = string.Empty;
            reqTime = string.Empty;
            cooX = 0;
            cooY = 0;
            currentPositionCode = string.Empty;
            data = data;
            mapCode = string.Empty;
            mapDataCode = string.Empty;
            stgBinCode = string.Empty;
            method = string.Empty;
            podCode = string.Empty;
            podDir = string.Empty;
            materialLot = string.Empty;
            materialType = string.Empty;
            robotCode = string.Empty;
            taskCode = string.Empty;
            wbCode = string.Empty;
            ctnrCode = string.Empty;
            ctnrType = string.Empty;
            roadWayCode = string.Empty;
            seq = string.Empty;
            eqpCode = string.Empty;
        }
    }

    public class SentTaskToHikAgv
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
        public object[] userCallCodePath { get; set; }

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

    public class BindLineSideStock
    {
        /// <summary>
        /// 请求编码
        /// </summary>
        public string reqCode { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public string? reqTime { get; set; }

        /// <summary>
        ///客户端编号，如 PDA，HCWMS 等
        /// </summary>
        public string? clientCode { get; set; }

        /// <summary>
        /// 货架编号
        /// </summary>
        public string podCode { get; set; }

        /// <summary>
        /// 绑定标识：1-绑定 0-解绑
        /// </summary>
        public string indBind { get; set; }

        /// <summary>
        /// 地图呼叫站点名
        /// </summary>
        public string mapDataCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string materialLot { get; set; }

        /// <summary>
        /// 批次组号(母lot)
        /// </summary>
        public string? materialGroupCode { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 批次系统数量
        /// </summary>
        public string? sysLotNum { get; set; }

        /// <summary>
        /// 批次本托盘数量
        /// </summary>
        public string? podLotNum { get; set; }

        /// <summary>
        /// 批次状态  NG/OK
        /// </summary>
        public string? lotStatus { get; set; }

        /// <summary>
        /// 是否包含报废板（0-否 1–是）
        /// </summary>
        public string? includeScrap { get; set; }
    }

    public class BindLineSideStockRes
    {
        public string code { get; set; }

        public string message { get; set; }
        public string reqCode { get; set; }
    }
}

public class QueryTaskStatus
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
    /// 任务号
    /// </summary>
    public string[]? taskCodes { get; set; }

    /// <summary>
    /// AGV code
    /// </summary>
    public string? agvCode { get; set; }
}

public class QueryTaskStatusResponse
{
    public string code { get; set; }

    public string message { get; set; }
    public string reqCode { get; set; }

    //public List<QueryTaskStatusData> data { get; set; }
    public object? data { get; set; }

    [JsonIgnore]
    public List<QueryTaskStatusData> QueryResult { get; set; } = new List<QueryTaskStatusData>();
}

public class QueryTaskStatusData
{
    public string taskCode { get; set; }

    public string taskTyp { get; set; }

    public string taskStatus { get; set; }

    public string? agvCode { get; set; }
}
