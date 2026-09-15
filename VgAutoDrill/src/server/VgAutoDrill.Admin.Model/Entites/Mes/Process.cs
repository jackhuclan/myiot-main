using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 工序
    ///</summary>
    [SugarTable("t_process")]
    public class Process : BaseEntity
    {
        /// <summary>
        /// 编码
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        [SugarColumn(ColumnName = "name")]
        public virtual string? Name { get; set; }

        [SugarColumn(ColumnName = "Attention")]
        /// <summary>
        /// 工艺要求
        /// </summary>
        public virtual string? Attention { get; set; }
    }
}
