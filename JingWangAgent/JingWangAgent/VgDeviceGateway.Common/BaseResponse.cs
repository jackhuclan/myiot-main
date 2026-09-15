namespace VgDeviceGateway.Devices.Common;

public class BaseResponse
{
    public int Code { get; set; } = -1;

    public string Message { get; set; }

    public string TrancId { get; set; }
}
