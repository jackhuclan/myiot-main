namespace VegaIot.External.AgvEntity.STD;

public class QueryAgvStatusResponse
{
    /// <summary>
    ///返回码
    /// </summary>
    public string? code { get; set; }

    public List<QueryAgvStatusData>? data { get; set; }

    public string? interrupt { get; set; }

    /// <summary>
    ///返回消息
    /// </summary>
    public string? message { get; set; }

    public string? msgErrCode { get; set; }

    /// <summary>
    ///请求编号
    /// </summary>
    public string? reqCode { get; set; }
}

public class QueryAgvStatusData
{
    /// <summary>
    ///机器人电量, 范围: 0-100
    /// </summary>
    public string? battery { get; set; }

    /// <summary>
    ///是否已被排除，被排除后不接受新任务（ 1-排除， 0-正常）
    /// </summary>
    public string? exclType { get; set; }

    /// <summary>
    ///机器人所在地图
    /// </summary>
    public string? mapCode { get; set; }

    public string? online { get; set; }

    /// <summary>
    ///执行路径,单位是毫米, 格式 x 轴,y轴,方向
    /// </summary>
    public List<string>? path { get; set; }

    /// <summary>
    ///背货架的编号
    /// </summary>
    public string? podCode { get; set; }

    /// <summary>
    ///背货架的方向
    /// </summary>
    public string? podDir { get; set; }

    /// <summary>
    ///机器人 x 坐标,单位:毫米
    /// </summary>
    public string? posX { get; set; }

    /// <summary>
    ///机器人 y 坐标,单位:毫米
    /// </summary>
    public string posY { get; set; }

    /// <summary>
    ///机器人编号
    /// </summary>
    public string? robotCode { get; set; }

    /// <summary>
    ///机器人方向 (范围 -180~360 度)
    /// </summary>
    public string? robotDir { get; set; }

    /// <summary>
    ///机器人 IP
    /// </summary>
    public string? robotIp { get; set; }

    /// <summary>
    ///机器人当前速度, 单位: mm/s
    /// </summary>
    public string? speed { get; set; }

    /// <summary>
    ///机器人状态
    /// </summary>
    public string? status { get; set; }

    /// <summary>
    ///是否暂停 0-否 1-是
    /// </summary>
    public string? stop { get; set; }

    public int? timestamp { get; set; }
}
