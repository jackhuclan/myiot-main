namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Route
{
    public class AddOrUpdateRouteReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public virtual string? Name { get; set; }

        /// <summary>
        /// 工艺路线说明
        /// </summary>
        public virtual string? RouteDesc { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 审批状态 
        /// 默认值: 0
        ///</summary>
        public virtual byte VettingStatus { get; set; }
    }
}
