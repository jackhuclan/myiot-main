using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 车间表
    ///</summary>
    [SugarTable("t_workshop")]
    public class Workshop : BaseEntity
    {
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        [SugarColumn(ColumnName = "name")]
        public virtual string? Name { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        [SugarColumn(ColumnName = "charge")]
        public virtual string? Charge { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }
    }
}
