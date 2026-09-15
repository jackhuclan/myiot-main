using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("sys_department")]
    public class SysDepartment
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 父对象Id(0表示是根对象) 
        ///</summary>
        [SugarColumn(ColumnName = "parent_id")]
        public int ParentId { get; set; }

        /// <summary>
        /// 部门名称 
        ///</summary>
        [SugarColumn(ColumnName = "department_name")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 电话/手机 
        ///</summary>
        [SugarColumn(ColumnName = "telephone")]
        public string Telephone { get; set; }
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
        /// 负责人
        ///</summary>
        [SugarColumn(ColumnName = "leader")]
        public string Leader { get; set; }

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
