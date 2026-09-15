using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 编码生成规则
    ///</summary>
    [SugarTable("t_encode_build_rules")]
    public class EncodeBuildRules : BaseEntity
    {
        /// <summary>
        /// 规则编码
        /// </summary>
        [SugarColumn(ColumnName = "rules_code")]
        public virtual string? RulesCode { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        [SugarColumn(ColumnName = "rules_name")]
        public virtual string? RulesName { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        [SugarColumn(ColumnName = "prefix")]
        public virtual string? Prefix { get; set; }

        /// <summary>
        /// 流水号长度
        /// </summary>
        [SugarColumn(ColumnName = "number_length")]
        public virtual int? NumberLength { get; set; }

        /// <summary>
        /// 是否补齐(0/1)
        /// </summary>
        [SugarColumn(ColumnName = "is_padded")]
        public virtual int? IsPadded { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        [SugarColumn(ColumnName = "suffix")]
        public virtual string? Suffix { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }
        /// <summary>
        /// 是否包含年份
        /// </summary>
        [SugarColumn(ColumnName = "has_year")]
        public virtual bool? HasYear { get; set; }
        /// <summary>
        /// 是否包含月份
        /// </summary>
        [SugarColumn(ColumnName = "has_month")]
        public virtual bool? HasMonth { get; set; }
        /// <summary>
        /// 是否包含天数
        /// </summary>
        [SugarColumn(ColumnName = "has_day")]
        public virtual bool? HasDay { get; set; }
    }
}
