namespace VgAutoDrill.Admin.Model.ViewModels.Res.Department
{
    public class DepartmentDto
    {
        /// <summary>
        ///  主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 父对象Id(0表示是根对象) 
        ///</summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 状态(1:启用;0禁用) 
        /// 默认值: 1
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 电话/手机 
        ///</summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 邮箱 
        ///</summary>
        public string Email { get; set; }
        /// <summary>
        /// QQ 
        ///</summary>
        public string Qq { get; set; }
        /// <summary>
        /// 负责人
        ///</summary>
        public string Leader { get; set; }
    }
}
