namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Rack
{
    public class RackFullDataByPartition
    {
        public virtual string? WareHouseCode { get; set; }

        public virtual List<RackFullData>? Racks { get; set; }
    }
}
