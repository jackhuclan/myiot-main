using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites
{
    public class BaseEntityWithTree : GeneralBaseEntityWithTree
    {
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnName = "name")]
        public override sealed string? Name { get; set; }

        /// <summary>
        /// 父对象Id(0表示是根对象) 
        ///</summary>
        [SugarColumn(ColumnName = "parent_id")]
        public override sealed long ParentId { get; set; }

        /// <summary>
        /// 所有层级父节点
        /// </summary>
        [SugarColumn(ColumnName = "ancestors")]
        public override sealed string? Ancestors { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public override sealed string? Code { get; set; }
    }

    public abstract class GeneralBaseEntityWithTree : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public abstract string? Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public abstract string? Code { get; set; }

        /// <summary>
        /// 父对象Id(0表示是根对象) 
        ///</summary>
        public abstract long ParentId { get; set; }

        /// <summary>
        /// 所有层级父节点
        /// </summary>
        public abstract string? Ancestors { get; set; }
    }
}
