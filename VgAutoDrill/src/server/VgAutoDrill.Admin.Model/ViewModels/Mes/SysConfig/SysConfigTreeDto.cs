namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig
{
    public class SysConfigTreeDto : SysConfigDto
    {
        public virtual List<SysConfigTreeDto>? Children { get; set; }
    }
}
