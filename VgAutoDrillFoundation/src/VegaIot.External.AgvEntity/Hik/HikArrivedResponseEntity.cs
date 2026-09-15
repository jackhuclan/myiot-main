namespace VegaIot.External.AgvEntity.Hik;

public class HikArrivedResponseEntity
{
    //public string AGV_ID { get; set; }

    //public bool Result { get; set; }

    public int code { get; set; }
    public string message { get; set; }
    public string reqCode { get; set; }

    public HikArrivedResponseEntity()
    {
        code = 0;
        message = "成功";
        reqCode = string.Empty;
    }
}
