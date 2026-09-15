using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites
{
    public abstract class BaseEntity : GeneralBaseEntity
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public override sealed long Id { get; set; }

        /// <summary>
        /// 否已删除 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "is_deleted")]
        public override sealed byte IsDeleted { get; set; } = 0;

        /// <summary>
        /// 状态(1:启用;0禁用) 
        /// 默认值: 1
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public override sealed int Status { get; set; } = 1;

        /// <summary>
        /// 创建人Id 
        ///</summary>
        [SugarColumn(ColumnName = "creator_id")]
        public override sealed int? CreatorId { get; set; }
        /// <summary>
        /// 创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public override sealed DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改时间 
        ///</summary>
        [SugarColumn(ColumnName = "modify_time")]
        public override sealed DateTime? ModifyTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改人Id 
        ///</summary>
        [SugarColumn(ColumnName = "modifier_id")]
        public override sealed int? ModifierId { get; set; }
    }

    public abstract class GeneralBaseEntity
    {
        public abstract long Id { get; set; }
        /// <summary>
        /// 否已删除 
        /// 默认值: 0
        ///</summary>
        public abstract byte IsDeleted { get; set; }

        /// <summary>
        /// 状态(1:启用;0禁用) 
        /// 默认值: 1
        ///</summary>
        public abstract int Status { get; set; }

        /// <summary>
        /// 创建人Id 
        ///</summary>
        public abstract int? CreatorId { get; set; }
        /// <summary>
        /// 创建时间 
        ///</summary>
        public abstract DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间 
        ///</summary>
        public abstract DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 修改人Id 
        ///</summary>
        public abstract int? ModifierId { get; set; }
    }
}
