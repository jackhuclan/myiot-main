using SqlSugar;
namespace VgAutoDrill.Admin.Model.Entites
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("sys_user_role")]
    public class SysUserRole
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 用户ID 
        ///</summary>
        [SugarColumn(ColumnName = "user_id")]
        public int UserId { get; set; }
        /// <summary>
        /// 角色ID 
        ///</summary>
        [SugarColumn(ColumnName = "role_id")]
        public int RoleId { get; set; }

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
    }
}
