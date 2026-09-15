using SqlSugar;
namespace VgAutoDrill.Admin.Model.Entites
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("sys_user")]
    public class SysUser
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 登录名 
        ///</summary>
        [SugarColumn(ColumnName = "user_name")]
        public string UserName { get; set; }
        /// <summary>
        /// 密码 
        ///</summary>
        [SugarColumn(ColumnName = "password")]
        public string Password { get; set; }
        /// <summary>
        /// 密码加盐 
        ///</summary>
        [SugarColumn(ColumnName = "salt")]
        public string Salt { get; set; }
        /// <summary>
        /// 真实姓名 
        ///</summary>
        [SugarColumn(ColumnName = "real_name")]
        public string RealName { get; set; }
        /// <summary>
        /// 部门 
        ///</summary>
        [SugarColumn(ColumnName = "department_id")]
        public int? DepartmentId { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        [SugarColumn(ColumnName = "position_id")]
        public int? PositionId { get; set; }
        /// <summary>
        /// 性别(1:男 0:女) 
        ///</summary>
        [SugarColumn(ColumnName = "sex")]
        public int? Sex { get; set; }
        /// <summary>
        /// 生日 
        ///</summary>
        [SugarColumn(ColumnName = "birthday")]
        public string Birthday { get; set; }
        /// <summary>
        /// 头像 
        ///</summary>
        [SugarColumn(ColumnName = "portrait")]
        public string Portrait { get; set; }
        /// <summary>
        /// 手机 
        ///</summary>
        [SugarColumn(ColumnName = "mobile")]
        public string Mobile { get; set; }
        /// <summary>
        /// 邮箱  
        ///</summary>
        [SugarColumn(ColumnName = "email")]
        public string Email { get; set; }
        /// <summary>
        /// QQ 
        ///</summary>
        [SugarColumn(ColumnName = "qq")]
        public string Qq { get; set; }
        /// <summary>
        /// 微信 
        ///</summary>
        [SugarColumn(ColumnName = "we_chat")]
        public string WeChat { get; set; }
        /// <summary>
        /// 是否超级管理员(1:是 0:否) 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "is_admin")]
        public byte? IsAdmin { get; set; }
        /// <summary>
        /// 备注 
        ///</summary>
        [SugarColumn(ColumnName = "remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 排序 
        ///</summary>
        [SugarColumn(ColumnName = "sort")]
        public int? Sort { get; set; }
        /// <summary>
        /// 否已删除 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "is_deleted")]
        public byte IsDeleted { get; set; }
        /// <summary>
        /// 状态(1:启用;0禁用) 
        /// 默认值: 1
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public int Status { get; set; }
        /// <summary>
        /// 创建人Id 
        ///</summary>
        [SugarColumn(ColumnName = "creator_id")]
        public int? CreatorId { get; set; }
        /// <summary>
        /// 创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间 
        ///</summary>
        [SugarColumn(ColumnName = "modify_time")]
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 修改人Id 
        ///</summary>
        [SugarColumn(ColumnName = "modifier_id")]
        public int? ModifierId { get; set; }
    }
}
