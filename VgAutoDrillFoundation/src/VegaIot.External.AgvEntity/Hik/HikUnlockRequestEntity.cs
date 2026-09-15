namespace VegaIot.External.AgvEntity.Hik;

public class HikUnlockRequestEntity
{
    public string reqCode { get; set; }

    public string robotCode { get; set; }

    public HikUnlockRequestEntity()
    {
        reqCode = string.Empty;
        robotCode = string.Empty;
    }
}
