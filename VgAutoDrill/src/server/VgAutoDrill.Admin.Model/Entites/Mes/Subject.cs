using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 点检保养项目表
    ///</summary>
    [SugarTable("t_subject")]
    public class Subject : BaseEntityWithTree
    {
        /// <summary>
        /// 类型 点检or保养
        /// </summary>
        [SugarColumn(ColumnName = "subject_type")]
        public virtual string? SubjectType { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [SugarColumn(ColumnName = "subject_content")]
        public virtual string? SubjectContent { get; set; }

        /// <summary>
        /// 标准
        /// </summary>
        [SugarColumn(ColumnName = "standard")]
        public virtual string? Standard { get; set; }
    }
}
