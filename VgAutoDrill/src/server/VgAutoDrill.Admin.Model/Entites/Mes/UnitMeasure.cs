using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 单位表
    ///</summary>
    [SugarTable("t_unit_measure")]
    public class UnitMeasure : BaseEntity
    {
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        [SugarColumn(ColumnName = "name")]
        public virtual string? Name { get; set; }
        /// <summary>
        /// 是否是主单位,
        /// 默认'Y'
        /// </summary>
        [SugarColumn(ColumnName = "primary_flag")]
        public virtual string? PrimaryFlag { get; set; }

        /// <summary>
        /// 主单位ID
        /// </summary>
        [SugarColumn(ColumnName = "primary_id")]
        public virtual int? PrimaryId { get; set; }

        /// <summary>
        /// 与主单位换算比例
        /// </summary>
        [SugarColumn(ColumnName = "change_rate")]
        public virtual decimal? ChangeRate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }
    }
}
