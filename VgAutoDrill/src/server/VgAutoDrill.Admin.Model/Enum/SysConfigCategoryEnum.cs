using System.ComponentModel;

namespace VgAutoDrill.Admin.Model.Enum
{
    public enum SysConfigCategoryEnum
    {
        None = 0,

        /// <summary>
        /// 系统基本配置
        /// </summary>
        [Description("系统基本配置")]
        Default = 1,

        /// <summary>
        /// 景旺配置
        /// </summary>
        [Description("景旺配置")]
        Kinwong = 2,

        /// <summary>
        /// 崇达配置
        /// </summary>
        [Description("崇达配置")]
        Chongda = 3,

        /// <summary>
        /// 先进配置
        /// </summary>
        [Description("先进配置")]
        XianJin = 4,

        /// <summary>
        /// 系统基本配置-中控系统
        /// </summary>
        [Description("中控系统")]
        Central = 10,

        /// <summary>
        /// 系统基本配置-生产工单/任务
        /// </summary>
        [Description("生产工单/任务")]
        WorkOrderTask = 20,

        /// <summary>
        /// 系统基本配置-设备
        /// </summary>
        [Description("设备")]
        Device = 30,

        /// <summary>
        /// 系统基本配置-告警
        /// </summary>
        [Description("告警")]
        Alarm = 40,

        /// <summary>
        /// 设备保养配置
        /// </summary>
        [Description("设备保养配置")]
        DeviceMaintenance = 100,

        /// <summary>
        /// 分区转运配置
        /// </summary>
        [Description("分区转运配置")]
        PartitionTransportSetting = 110,
    }
}
