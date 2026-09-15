namespace VgDeviceGateway.Devices.Common.Agv.Hik;

public class HikUnlockResponseEntity
{
    public HikUnlockResponseEntity()
    {
        code = string.Empty;
        data = string.Empty;
        interrupt = false;
        message = string.Empty;
        reqCode = string.Empty;
        taskTypInfo = string.Empty;
    }


    public string code { get; set; }//0:正常 ⾮0:异常码

    public string? data { get; set; }
    public bool interrupt { get; set; }
    public string message { get; set; }//提⽰信息.

    public string reqCode { get; set; }//  请求编码
    public string taskTypInfo { get; set; }//  请求编码
}
