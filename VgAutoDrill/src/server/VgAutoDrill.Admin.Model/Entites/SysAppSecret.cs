using SqlSugar;
namespace VgAutoDrill.Admin.Model.Entites
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("sys_app_secret")]
    public class SysAppSecret
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 应用Id 
        ///</summary>
        [SugarColumn(ColumnName = "app_id")]
        public string AppId { get; set; }
        /// <summary>
        /// 应用密钥 
        ///</summary>
        [SugarColumn(ColumnName = "app_secret")]
        public string AppSecret { get; set; }
        /// <summary>
        /// 应用Code(唯一值) 
        ///</summary>
        [SugarColumn(ColumnName = "app_code")]
        public string AppCode { get; set; }
        /// <summary>
        /// 应用名 
        ///</summary>
        [SugarColumn(ColumnName = "app_name")]
        public string AppName { get; set; }
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
