namespace VegaIot.External.AgvEntity.STD;

public class StdCarResponse
{
    /// <summary>
    /// 0:成功，其他值：异常，重新调用
    /// </summary>
    public int code { get; set; } = -1;

    /// <summary>
    /// 异常消息
    /// </summary>
    public string msg { get; set; } = string.Empty;

    public string reqCode { get; set; } = string.Empty;

    public bool succ { get; set; }

    public StdCarModel? data { get; set; }
}
