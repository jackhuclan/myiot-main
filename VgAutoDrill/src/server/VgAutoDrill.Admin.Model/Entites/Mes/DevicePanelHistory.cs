using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("t_device_panel_history")]
    public class DevicePanelHistory : DevicePanel
    {
        /// <summary>
        /// 设备负载板料时间
        /// </summary>
        [SugarColumn(ColumnName = "device_panel_time")]
        public virtual DateTime? DevicePanelTime { get; set; }
    }
}
