namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Client
{
    public class GetClientListReq : Page
    {
        public virtual string? Code { get; set; }

        public virtual string? Name { get; set; }

        public virtual int? Status { get; set; }
    }
}
