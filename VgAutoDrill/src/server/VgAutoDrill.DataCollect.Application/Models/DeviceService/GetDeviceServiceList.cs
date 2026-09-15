namespace VgAutoDrill.DataCollect.Application.Models.DeviceService
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDeviceServiceList : DataLimit
    {
        /// <summary>
        /// DeviceCode,设备代码
        /// </summary>
        public virtual string DeviceCode { get; set; } = string.Empty;
    }
}
