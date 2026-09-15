namespace VgAutoDrill.Admin.Model.ViewModels.Mes.EncodeBuildRules
{
    public class GetEncodeBuildRulesListReq : Page
    {
        /// <summary>
        /// 规则编码
        /// </summary>
        public virtual string? RulesCode { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public virtual string? RulesName { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public virtual string? Prefix { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public virtual string? Suffix { get; set; }
    }
}
