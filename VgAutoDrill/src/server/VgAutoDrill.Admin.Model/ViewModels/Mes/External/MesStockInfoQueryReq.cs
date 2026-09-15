namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External
{
    public class MesStockInfoQueryReq
    {
        /// <summary>
        /// 目标区域
        /// </summary>
        public virtual string? TargetPosArea { get; set; }

        /// <summary>
        /// lot号
        /// </summary>
        public virtual string? Lot { get; set; }

        /// <summary>
        /// 物料号
        /// </summary>
        public virtual string? product { get; set; }
        /// <summary>
        /// 托盘号
        /// </summary>
        public virtual string? podCode { get; set; }
        /// <summary>
        /// 所在位置
        /// </summary>
        public virtual string? address { get; set; }
        /// <summary>
        /// 所在位置区域
        /// </summary>
        public virtual string? addressName { get; set; }
    }
}
