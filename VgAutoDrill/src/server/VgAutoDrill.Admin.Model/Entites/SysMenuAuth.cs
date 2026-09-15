using SqlSugar;
namespace VgAutoDrill.Admin.Model.Entites
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("sys_menu_auth")]
    public class SysMenuAuth
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 菜单Id 
        ///</summary>
        [SugarColumn(ColumnName = "menu_id")]
        public int MenuId { get; set; }
        /// <summary>
        /// 授权Id(角色Id或者用户Id) 
        ///</summary>
        [SugarColumn(ColumnName = "authorize_id")]
        public int AuthorizeId { get; set; }
        /// <summary>
        /// 授权类型(1角色 2用户) 
        ///</summary>
        [SugarColumn(ColumnName = "authorize_type")]
        public int? AuthorizeType { get; set; }

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
