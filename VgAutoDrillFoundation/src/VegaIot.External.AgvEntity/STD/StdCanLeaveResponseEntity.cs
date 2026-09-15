using System.Text.Json.Serialization;

namespace VegaIot.External.AgvEntity.STD;

/// <summary>
/// Code 0:成功，其他值：异常，重新调用
/// </summary>
public class StdCanLeaveResponseEntity
{
    /// <summary>
    /// 0:成功，其他值：异常，重新调用
    /// </summary>
    public int Code { get; set; } = -1;

    /// <summary>
    /// 异常消息
    /// </summary>
    public bool Result { get; set; } = false;

    public StdCanLeaveResponseEntity(int code)
    {
        Code = code;
    }

    [JsonConstructor]
    public StdCanLeaveResponseEntity(int code, bool result)
    {
        Code = code;
        Result = result;
    }
}
