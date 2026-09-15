using SqlSugar;
namespace VgAutoDrill.Admin.Model.Entites
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("sys_menu")]
    public class SysMenu
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 应用Code 
        ///</summary>
        [SugarColumn(ColumnName = "app_code")]
        public string AppCode { get; set; }
        /// <summary>
        /// 菜单名称 
        ///</summary>
        [SugarColumn(ColumnName = "menu_name")]
        public string MenuName { get; set; }
        /// <summary>
        /// 父菜单Id(0表示是根菜单) 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "parent_id")]
        public int ParentId { get; set; }
        /// <summary>
        /// 菜单图标 
        ///</summary>
        [SugarColumn(ColumnName = "menu_icon")]
        public string MenuIcon { get; set; }
        /// <summary>
        /// 路由地址
        ///</summary>
        [SugarColumn(ColumnName = "path")]
        public string Path { get; set; }
        /// <summary>
        /// 菜单Url 
        ///</summary>
        [SugarColumn(ColumnName = "menu_url")]
        public string MenuUrl { get; set; }
        /// <summary>
        /// 菜单类型(1目录 2页面 3按钮) 
        ///</summary>
        [SugarColumn(ColumnName = "menu_type")]
        public int? MenuType { get; set; }
        /// <summary>
        /// 菜单权限标识 
        ///</summary>
        [SugarColumn(ColumnName = "authorize")]
        public string Authorize { get; set; }
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
