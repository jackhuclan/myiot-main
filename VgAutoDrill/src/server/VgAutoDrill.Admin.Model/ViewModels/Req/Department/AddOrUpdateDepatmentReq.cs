namespace VgAutoDrill.Admin.Model.ViewModels.Req.Department
{
    public class AddOrUpdateDepatmentReq : BaseAddOrUpdateWithTreeDto
    {
        /// <summary>
        /// 部门名称 
        ///</summary>
        public virtual string? DepartmentName { get; set; }

        /// <summary>
        /// 电话/手机 
        ///</summary>
        public virtual string? Telephone { get; set; }
        /// <summary>
        /// 邮箱 
        ///</summary>
        public virtual string? Email { get; set; }
        /// <summary>
        /// QQ 
        ///</summary>
        public virtual string? Qq { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public virtual string? Leader { set; get; }
    }
}
