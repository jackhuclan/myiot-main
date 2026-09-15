namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CutterPlan
{
    public class GetCutterPlanListReq : Page
    {
        /// <summary>
        /// 刀盘二维码
        /// </summary>
        public virtual string? DiskCode { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        public virtual string? ItemCode { get; set; }
        /// <summary>
        /// 换刀规则1按料号，2按加工板次，3按使用寿命
        /// </summary>
        public virtual int? ChangeRule { get; set; }
    }
}
