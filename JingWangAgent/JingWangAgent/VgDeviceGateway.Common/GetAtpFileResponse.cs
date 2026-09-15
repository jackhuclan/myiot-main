namespace VgDeviceGateway.Devices.Common;

public class GetAtpFileResponse
{
    public int code { get; set; }
    public string message { get; set; } = string.Empty;
    public GetAtpFileReturnData data { get; set; } = new();
}

public class GetAtpFileReturnData
{
    public string groupNo { get; set; } = string.Empty;
    public string atpFile { get; set; } = string.Empty;
    public bool isNeedLoad { get; set; }
    public string box { get; set; } = string.Empty;
}
