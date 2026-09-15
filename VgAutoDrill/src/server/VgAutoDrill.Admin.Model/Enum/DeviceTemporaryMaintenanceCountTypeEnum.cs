using System.ComponentModel;

namespace VgAutoDrill.Admin.Model.Enum
{
    /// <summary>
    ///  设备临时保养计算类型枚举 0:按频次  1:按耗时
    /// </summary>
    public enum DeviceTemporaryMaintenanceCountTypeEnum
    {

        /// <summary>
        /// 按频次
        /// </summary>
        [Description("按频次")]
        Frequency = 0,

        /// <summary>
        /// 按耗时
        /// </summary>
        [Description("按耗时")]
        TimeConsuming = 1,



    }
}
