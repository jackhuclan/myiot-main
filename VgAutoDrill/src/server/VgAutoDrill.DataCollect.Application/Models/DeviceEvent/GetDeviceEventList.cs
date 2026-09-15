namespace VgAutoDrill.DataCollect.Application.Models.DeviceEvent
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDeviceEventList : DataLimit
    {
        /// <summary>
        /// DeviceCode,设备代码
        /// </summary>
        public virtual string DeviceCode { get; set; } = string.Empty;
    }
}
