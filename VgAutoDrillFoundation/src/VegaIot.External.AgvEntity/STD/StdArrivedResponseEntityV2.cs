namespace VegaIot.External.AgvEntity.STD;

/// <summary>
/// Code 0:成功，其他值：异常，重新调用
/// </summary>
public class StdArrivedResponseEntityV2
{
    public int Code { get; set; }
    public string Message { get; set; }
    public string? reqCode { get; set; } = string.Empty;

    public StdArrivedResponseEntityV2()
    {
        Code = 0;
        Message = "成功";
    }

}
