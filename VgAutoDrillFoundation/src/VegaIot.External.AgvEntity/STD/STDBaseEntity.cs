namespace VegaIot.External.AgvEntity.STD;

public abstract class STDBaseEntity
{
    /// <summary>
    /// 请求编号，每个请求都要一个唯一编号， 同一个请求重复提交， 使用同一编号。
    /// </summary>
    public String? reqCode { get; set; } = "";
    /// <summary>
    /// 请求时间截 格式: “yyyy-MM-dd HH:mm:ss”。 
    /// </summary>
    public String reqTime { get; set; } = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    /// <summary>
    /// 客户端编号，如PDA，HCWMS等。
    /// </summary>
    public String clientCode { get; set; } = "VEGA01";
}

public class STDBaseResponse<T>
{
    public Int32 code { get; set; } = -1;

    public String message { get; set; } = String.Empty;

    public String? reqCode { get; set; }

    public Boolean succ { get; set; }

    public T? data { get; set; }
}
