namespace VgAutoDrill.Admin.Model.ViewModels.Res.User
{
    public class UserInfoDto
    {
        /// <summary>
        /// Id
        /// </summary>
        //[JsonProperty(PropertyName = "userId")]
        public int Id { set; get; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Portrait { get; set; } = "";
        /// <summary>
        /// 部门ID
        /// </summary>
        public int? DepartmentId { set; get; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { set; get; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { set; get; }
        /// <summary>
        /// 性别(1:男 0:女) 
        ///</summary>
        public int? Sex { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { set; get; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }
    }
}
