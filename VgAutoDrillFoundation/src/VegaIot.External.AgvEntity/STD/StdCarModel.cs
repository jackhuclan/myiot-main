namespace VegaIot.External.AgvEntity.STD;

public class StdCarModel
{
    /// <summary>
    ///车辆类型
    /// </summary>
    public string? vehicleType { get; set; }

    /// <summary>
    ///车辆IP地址
    /// </summary>
    public string? ip { get; set; }

    /// <summary>
    ///车名
    /// </summary>
    public string? vehicleName { get; set; }

    /// <summary>
    ///车辆编号
    /// </summary>
    public string? vehicleKey { get; set; }

    /// <summary>
    /// 当前站点位置
    /// </summary>
    public string? nowStation { get; set; }

    /// <summary>
    ///上一个站点位置
    /// </summary>
    public string? beforeStation { get; set; }

    /// <summary>
    ///通行点
    /// </summary>
    public string? passStation { get; set; }

    /// <summary>
    ///路径终点
    /// </summary>
    public string? endStation { get; set; }

    /// <summary>
    ///是否到达终点
    /// </summary>
    public bool? arrivedStation { get; set; }

    /// <summary>
    ///电量百分比
    /// </summary>

    public float power { get; set; }

    /// <summary>
    ///是否低电量
    /// </summary>
    public string? isLower { get; set; }

    /// <summary>
    ///是否可调度
    /// </summary>
    public bool? dispatch { get; set; }

    /// <summary>
    ///任务id
    /// </summary>
    public string? orderId { get; set; }

    /// <summary>
    ///车辆状态
    /// </summary>
    public string? agvStates { get; set; }

    /// <summary>
    ///车辆版本
    /// </summary>
    public string? agvVersion { get; set; }

    /// <summary>
    ///管制占用路径
    /// </summary>
    public string? controlPath { get; set; }

    /// <summary>
    ///路径信息
    /// </summary>
    public string? pathMsg { get; set; }

    /// <summary>
    ///下一个站点
    /// </summary>
    public string? nextStation { get; set; }

    /// <summary>
    ///当前路径角度
    /// </summary>
    public string? angle { get; set; }

    /// <summary>
    ///车辆当前x左标
    /// </summary>
    public float x { get; set; }

    /// <summary>
    ///车辆当前y左标
    /// </summary>
    public float y { get; set; }

    /// <summary>
    ///车辆当前角度(弧度)
    /// </summary>
    public double radian { get; set; }

    /// <summary>
    ///剩余运行时间
    /// </summary>
    public string? remainingTime { get; set; }

    /// <summary>
    ///配置默认充电电量范围
    /// </summary>
    public string? chargingRange { get; set; }

    /// <summary>
    ///车辆所在地图
    /// </summary>
    public string? mapId { get; set; }

    /// <summary>
    ///车辆当前速度
    /// </summary>
    public float speed { get; set; }

    /// <summary>
    ///是否暂停
    /// </summary>
    public bool? pause { get; set; }
}
