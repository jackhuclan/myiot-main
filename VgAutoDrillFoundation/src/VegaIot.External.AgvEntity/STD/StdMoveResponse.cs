namespace VegaIot.External.AgvEntity.STD;

public class StdMoveResponse
{
    /// <summary>
    /// 0:成功，其他值：异常，重新调用
    /// </summary>
    public int Code { get; set; } = -1;
    /// <summary>
    /// 异常消息
    /// </summary>
    public string Message { get; set; } = string.Empty;
    public string? ReqCode { get; set; }
    public bool Succ { get; set; } = false;

    public StdMoveOrder? Data { get; set; }
}

public class StdMoveOrder
{
    /// <summary>
    /// 斯坦德 移动任务的order id
    /// </summary>
    public long OrderId { get; set; }

    public string OrderNo { get; set; }
}
