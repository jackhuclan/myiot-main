using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 点检保养项目
    ///</summary>
    [SugarTable("t_subject")]
    public class DvSubject : BaseEntity
    {
        /// <summary 
        /// 名称 
        ///</summary>
        [SugarColumn(ColumnName = "name")]
        public virtual string? Name { get; set; }

        [SugarColumn(ColumnName = "code")]
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string? Code { get; set; }

        [SugarColumn(ColumnName = "subject_type")]
        /// <summary>
        /// 项目类型
        /// </summary>
        public virtual string? SubjectType { get; set; }

        /// <summary>
        /// 项目内容
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
