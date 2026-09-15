namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail
{
    public class AllotsPanelDataReq
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 0生料仓, 1钻机,2熟料仓
        /// </summary>
        public virtual List<int>? Layers { get; set; }

        /// <summary>
        /// 料架编号
        /// </summary>
        public virtual string? RackCode { get; set; }
    }
}
