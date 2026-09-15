namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CutterPlan
{
    public class AddOrUpdateCutterPlanReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 刀盘二维码
        /// </summary>
        public virtual string? DiskCode { get; set; }
        /// <summary>
        /// 刀盘参数文件路径
        /// </summary>
        public virtual string? Atp { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        public virtual string? ItemCode { get; set; }
        /// <summary>
        /// 换刀规则1按料号，2按加工板次，3按使用寿命
        /// </summary>
        public virtual int? ChangeRule { get; set; }
        /// <summary>
        /// 板次
        /// </summary>
        public virtual int? RuleBoardLimit { get; set; }
        /// <summary>
        /// 使用寿命
        /// </summary>
        public virtual int? RuleAgeLimit { get; set; }
    }
}