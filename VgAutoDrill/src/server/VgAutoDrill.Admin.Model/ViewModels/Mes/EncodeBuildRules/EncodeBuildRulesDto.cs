namespace VgAutoDrill.Admin.Model.ViewModels.Mes.EncodeBuildRules
{
    public class EncodeBuildRulesDto : BaseDto
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
        /// 流水号长度
        /// </summary>
        public virtual int? NumberLength { get; set; }

        /// <summary>
        /// 是否补齐(0/1)
        /// </summary>
        public virtual int? IsPadded { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public virtual string? Suffix { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 示例
        /// </summary>
        public virtual string? ExampleCode { get; set; }

        /// <summary>
        /// 当前编码
        /// </summary>
        public virtual string? CurrentCode { get; set; }
        /// <summary>
        /// 是否包含年份
        /// </summary>
        public virtual bool? HasYear { get; set; }
        /// <summary>
        /// 是否包含月份
        /// </summary>
        public virtual bool? HasMonth { get; set; }
        /// <summary>
        /// 是否包含天数
        /// </summary>
        public virtual bool? HasDay { get; set; }
    }
}
