namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail
{
    public class LoadDrillPanelDetailReq
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public virtual string? DeviceCode { get; set; }
        /// <summary> 
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }
        /// <summary>
        /// 每叠块数
        /// </summary>
        public virtual int? Pcs { get; set; }
        /// <summary>
        /// 板宽
        /// </summary>
        public virtual decimal? PanelWidth { get; set; }
    }
}
