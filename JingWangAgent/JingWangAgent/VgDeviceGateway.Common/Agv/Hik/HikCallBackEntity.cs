namespace VgDeviceGateway.Devices.Common.Agv.Hik;

public class HikCallBackEntity
{
    public HikCallBackEntity()
    {
        //AGV_ID = string.Empty;
        //IsSuccess = false;
        //position = string.Empty;
        //X_Error = string.Empty;
        //Z_Error = string.Empty;
        //Exception = string.Empty;
        //Status = string.Empty;
        //TaskId = string.Empty;
        reqCode = string.Empty;
        reqTime = string.Empty;
        cooX = string.Empty;
        cooY = string.Empty;
        currentPositionCode = string.Empty;
        data = string.Empty;
        mapCode = string.Empty;
        method = string.Empty;
        podCode = string.Empty;
        podDir = string.Empty;
        robotCode = string.Empty;
        taskCode = string.Empty;
        wbCode = string.Empty;
    }

    /// <summary>
    ///  请求编号
    /// </summary>
    public string reqCode { get; set; }

    /// <summary>
    /// 请求时间戳
    /// </summary>
    public string reqTime { get; set; }

    /// <summary>
    /// 地码 X 坐标(mm)
    /// </summary>
    public string? cooX { get; set; }

    /// <summary>
    /// 地码 Y 坐标(mm)
    /// </summary>
    public string? cooY { get; set; }

    /// <summary>
    /// 当前位置编号
    /// </summary>
    public string currentPositionCode { get; set; }

    /// <summary>
    /// 自定义字段
    /// </summary>
    public string? data { get; set; }

    /// <summary>
    /// 地图编号
    /// </summary>
    public string? mapCode { get; set; }

    /// <summary>
    /// 方法名, 可使用任务类型做为方法名
    /// </summary>
    public string method { get; set; }

    /// <summary>
    /// 货架编号：背货架时有值
    /// </summary>
    public string? podCode { get; set; }

    /// <summary>
    /// “180”,”0”,”90”,”-90” 分别 对 应 地 图 的 ” 左 ”,” 右 ”,”上”,”下”：任务完成时有值
    /// </summary>
    public string? podDir { get; set; }

    /// <summary>
    /// AGV 编号
    /// </summary>
    public string robotCode { get; set; }

    /// <summary>
    /// 当前任务单号
    /// </summary>
    public string taskCode { get; set; }

    /// <summary>
    /// 工作位
    /// </summary>
    public string? wbCode { get; set; }
}
