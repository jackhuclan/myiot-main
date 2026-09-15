namespace VgAutoDrill.DataCollect.Application.Models.DeviceStatus
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDeviceStatusList : DataLimit
    {
        /// <summary>
        /// DeviceCode,设备代码
        /// </summary>
        public virtual string DeviceCode { get; set; } = string.Empty;
    }
}
