namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess
{
    /// <summary>
    /// 根据Item查找工艺路线和工序
    /// </summary>
    public class GetRouteAndProcessByItemReq
    {
        public virtual long? ItemId { get; set; }

        public virtual long? RouteId { get; set; }

        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 工艺路线审批状态 
        ///</summary>
        public virtual byte? VettingStatus { get; set; }
    }
}
