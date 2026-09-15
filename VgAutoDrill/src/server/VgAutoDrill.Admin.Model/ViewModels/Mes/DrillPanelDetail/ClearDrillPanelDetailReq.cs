namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail
{
    public class ClearDrillPanelDetailReq
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 0生料仓, 1钻机,2熟料仓
        /// </summary>
        public virtual int? Layer { get; set; }

        public virtual List<int>? SplindleIndexs { get; set; }
    }
}
