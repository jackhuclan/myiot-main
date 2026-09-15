using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 客户表
    ///</summary>
    [SugarTable("t_client")]
    public class Client : BaseEntity
    {
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        [SugarColumn(ColumnName = "name")]
        public virtual string? Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }
    }
}
