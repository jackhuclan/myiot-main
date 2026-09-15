namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Route
{
    public class GetRouteListReq : Page
    {
        public virtual string? Code { get; set; }

        public virtual string? Name { get; set; }

        /// <summary>
        /// 审批状态 
        ///</summary>
        public virtual byte? VettingStatus { get; set; }
    }
}
