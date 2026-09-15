using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanelHistory;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace
{
    public class PanelFullPropertiesTreeDto : PanelDto
    {
        /// <summary>
        /// 前端使用，不需存数据库
        /// </summary>
        public virtual bool IsChecked { get; set; } = false;

        /// <summary>
        /// 前端使用，不需存数据库
        /// </summary>
        public virtual bool IsIndeterminate { get; set; } = false;

        /// <summary>
        /// 子对象
        /// </summary>
        public virtual List<DevicePanelHistoryDto> Children { set; get; } = new List<DevicePanelHistoryDto>();
    }
}
