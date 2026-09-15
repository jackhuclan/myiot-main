namespace VegaIot.External.AgvEntity.Hik;

public class QueryTaskStatusResponse
{
    public string? code { get; set; }

    public string? message { get; set; }

    public string? reqCode { get; set; }

    public string? msgErrCode { get; set; }

    public bool? interrupt { get; set; }

    public List<QueryTaskStatusData> data { get; set; } = new List<QueryTaskStatusData>();
}

public class QueryTaskStatusData
{
    public string? taskCode { get; set; }

    //任务状态：1-已创建，2-正在执行，5-取消完成，9-已结束, 10-被打断
    public string? taskStatus { get; set; }

    public string? agvCode { get; set; }

    public string? taskTyp { get; set; }
}
