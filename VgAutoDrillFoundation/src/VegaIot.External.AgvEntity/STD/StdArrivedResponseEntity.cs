using System.Text.Json.Serialization;

namespace VegaIot.External.AgvEntity.STD;

/// <summary>
/// Code 0:成功，其他值：异常，重新调用
/// </summary>
public class StdArrivedResponseEntity
{
    public int Code { get; set; } = -1;
    public string Message { get; set; }

    public StdArrivedResponseEntity(int code)
    {
        Code = code;
    }

    [JsonConstructor]
    public StdArrivedResponseEntity(int code, string message)
    {
        Code = code;
        Message = message;
    }
}
