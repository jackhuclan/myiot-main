using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 钻机排刀
    ///</summary>
    [SugarTable("t_cutter_plan")]
    public class CutterPlan : BaseEntity
    {
        /// <summary>
        /// 刀盘二维码
        /// </summary>
        [SugarColumn(ColumnName = "disk_code")]
        public virtual string? DiskCode { get; set; }
        /// <summary>
        /// 刀盘参数文件路径
        /// </summary>
        [SugarColumn(ColumnName = "atp")]
        public virtual string? Atp { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }
        /// <summary>
        /// 换刀规则1按料号，2按加工板次，3按使用寿命
        /// </summary>
        [SugarColumn(ColumnName = "change_rule")]
        public virtual int? ChangeRule { get; set; }
        /// <summary>
        /// 板次
        /// </summary>
        [SugarColumn(ColumnName = "rule_board_limit")]
        public virtual int? RuleBoardLimit { get; set; }
        /// <summary>
        /// 使用寿命
        /// </summary>
        [SugarColumn(ColumnName = "rule_age_limit")]
        public virtual int? RuleAgeLimit { get; set; }
    }
}
