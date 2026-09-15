namespace VgAutoDrill.Admin.Model.ViewModels.Mes.EncodeBuildRules
{
    /// <summary>
    /// 根据规则编号和规则生成编码
    /// </summary>
    public class GetEncodeByRulesListReq
    {
        /// <summary>
        /// 规则编号
        /// </summary>
        public virtual string? RulesCode { get; set; }

        /// <summary>
        /// 生成几个编码
        /// </summary>
        public virtual int? BuildCount { get; set; }
    }
}
