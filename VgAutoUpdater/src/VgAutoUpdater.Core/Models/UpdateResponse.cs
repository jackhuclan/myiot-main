namespace VgAutoUpdater.Core.Models;

public class UpdateResponse
{
    public string Code { get; set; }
    public string Message { get; set; }
    public object? Data { get; set; }

    private UpdateResponse(string code, string message) : this(code, message, null) { }
    private UpdateResponse(string code, string message, object data)
    {
        Code = code;
        Message = message;
        Data = data;
    }

    public static UpdateResponse Ok(string message = "")
    {
        return new UpdateResponse("200", message);
    }

    public static UpdateResponse Ok(object data)
    {
        return new UpdateResponse("200", "", data);
    }

    public static UpdateResponse Ok(string code, string message)
    {
        return new UpdateResponse(code, message);
    }

    public static UpdateResponse Fail(string message = "")
    {
        return new UpdateResponse("500", message);
    }

    public static UpdateResponse Fail(string code, string message)
    {
        return new UpdateResponse(code, message);
    }
}
