namespace VgAutoDrill.Admin.Model.ViewModels.Mes.UnitMeasure
{
    public class GetUnitMeasureListReq : Page
    {
        public virtual long Id { get; set; }
        public virtual string? Code { get; set; }

        public virtual string? Name { get; set; }
    }
}
