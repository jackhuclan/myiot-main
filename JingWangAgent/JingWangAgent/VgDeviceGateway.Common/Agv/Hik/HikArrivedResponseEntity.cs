namespace VgDeviceGateway.Devices.Common.Agv.Hik;

public class HikArrivedResponseEntity
{
    //public string AGV_ID { get; set; }

    //public bool Result { get; set; }

    public string code { get; set; }
    public string message { get; set; }
    public string reqCode { get; set; }

    public HikArrivedResponseEntity()
    {
        code = string.Empty;
        message = string.Empty;
        reqCode = string.Empty;
    }
}
