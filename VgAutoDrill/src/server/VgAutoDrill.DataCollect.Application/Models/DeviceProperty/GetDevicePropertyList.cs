namespace VgAutoDrill.DataCollect.Application.Models.DeviceProperty
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDevicePropertyList : DataLimit
    {
        /// <summary>
        /// DeviceCode,设备代码
        /// </summary>
        public virtual string DeviceCode { get; set; } = string.Empty;
    }
}
