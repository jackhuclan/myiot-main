using SqlSugar;
namespace VgAutoDrill.Admin.Model.Entites
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("sys_dic_data")]
    public class SysDicData
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 名称 
        ///</summary>
        [SugarColumn(ColumnName = "dic_name")]
        public string DicName { get; set; }
        /// <summary>
        /// 字典Key 
        ///</summary>
        [SugarColumn(ColumnName = "dic_code")]
        public string DicCode { get; set; }
        /// <summary>
        /// 字典值 
        ///</summary>
        [SugarColumn(ColumnName = "dic_value")]
        public string DicValue { get; set; }
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
