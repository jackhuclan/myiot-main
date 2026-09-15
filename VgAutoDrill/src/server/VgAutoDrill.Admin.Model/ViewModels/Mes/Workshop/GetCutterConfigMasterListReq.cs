namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Workshop
{
    public class GetWorkshopListReq : Page
    {
        public virtual string? Code { get; set; }

        public virtual string? Name { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public virtual string? Charge { get; set; }
    }
}
