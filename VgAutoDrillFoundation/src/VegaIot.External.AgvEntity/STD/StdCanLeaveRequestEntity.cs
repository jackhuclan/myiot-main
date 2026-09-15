namespace VegaIot.External.AgvEntity.STD;

public class StdCanLeaveRequestEntity
{
    public long OrderId { get; set; } = -1;
    public string VehicleId { get; set; } = string.Empty;

    public string VehicleName { get; set; } = string.Empty;

    public string PositionCode { get; set; } = string.Empty;

    public string PositionName { get; set; } = string.Empty;

    public string OrderNo { get; set; } = string.Empty;
}
