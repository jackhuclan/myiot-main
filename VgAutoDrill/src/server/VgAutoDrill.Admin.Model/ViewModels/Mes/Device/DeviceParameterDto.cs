using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    /// <summary>
    /// 设备参数
    /// 结构与DeviceDescriptor一致
    /// </summary>
    public class DeviceParameterDto
    {
        public virtual Dictionary<string, object>? Extra { get; set; }
        public virtual string? ProductId { get; set; }
        public virtual string? DeviceId { get; set; }
        public virtual string? DeviceName { get; set; }
        public virtual bool? AutoMode { get; set; }
        public bool SoloMode { get; set; }
        public virtual string? DeviceClazz { get; set; }
        public virtual int? DataCollectingPerSeconds { get; set; }
        public virtual int? KeepingPlcConnectionPerSeconds { get; set; }
        public virtual int? ScheduleTaskExecutingPerSeconds { get; set; }
        public int SpindleNum { get; set; }
        /// <summary>
        /// max number of silo 
        /// </summary>
        public virtual int? SiloLimit { get; set; }
        /// <summary>
        /// max number of panel 
        /// </summary>
        public virtual int? PanelLimit { get; set; }
        /// <summary>
        /// max panel number of one lot 
        /// </summary>
        public virtual int? LotLimit { get; set; }
        /// <summary>
        /// max number of layer 
        /// </summary>
        public virtual int? LayerLimit { get; set; }
        public virtual DeviceKind? DeviceKind { get; set; }
        public virtual List<ProductStatus>? InputCapabilities { get; set; }
        public virtual List<ProductStatus>? OutputCapabilities { get; set; }
    }
}
