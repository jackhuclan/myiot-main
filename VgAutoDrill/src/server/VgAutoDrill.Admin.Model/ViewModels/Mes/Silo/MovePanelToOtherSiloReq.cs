namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Silo
{
    public class MovePanelToOtherSiloReq
    {
        /// <summary>
        /// 板料号
        /// </summary>
        public virtual string? PanelCode { get; set; }
        /// <summary>
        /// 目标料仓号
        /// </summary>
        public virtual string? TargetSiloCode { get; set; }
        /// <summary>
        /// 目标位置
        /// </summary>
        public virtual int TargetFloor { get; set; }
    }
}
