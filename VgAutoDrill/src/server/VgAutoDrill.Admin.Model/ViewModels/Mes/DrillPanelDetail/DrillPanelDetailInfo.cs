namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail
{
    public class DrillPanelDetailInfo
    {
        /// <summary>
        /// 0生料仓, 1钻机,2熟料仓
        /// </summary>
        public virtual int? Layer { get; set; }

        public virtual List<DrillPanelDetailDto>? PanelDetailDtos { get; set; }
    }
}
