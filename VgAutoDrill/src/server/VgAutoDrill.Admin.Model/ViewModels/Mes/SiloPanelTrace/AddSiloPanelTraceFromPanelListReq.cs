using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace
{
    /// <summary>
    /// 从PanelList添加料仓板料追溯记录请求
    /// </summary>
    public class AddSiloPanelTraceFromPanelListReq
    {
        /// <summary>
        /// 板料列表
        /// </summary>
        public PanelList Panels { get; set; } = new PanelList();

        /// <summary>
        /// 操作主题描述
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// 调度记录表ID
        /// </summary>
        public long? ScheduleId { get; set; }

        /// <summary>
        /// 运输任务表ID
        /// </summary>
        public long? TransportationTaskId { get; set; }
    }
}
