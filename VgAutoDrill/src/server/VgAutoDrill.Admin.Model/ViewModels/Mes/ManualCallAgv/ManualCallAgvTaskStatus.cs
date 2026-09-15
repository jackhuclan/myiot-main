using System.ComponentModel;
using System.Runtime.Serialization;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv
{
    /// <summary>
    /// 1、任务创建
    /// 2、任务下发
    /// 3、任务回调
    /// 4、任务完成
    /// 5、任务异常
    /// </summary>
    public enum ManualCallAgvTaskStatus
    {
        /// <summary>
        /// 任务创建
        /// </summary>
        [Description("任务创建")]
        [EnumMember(Value = "任务创建")]
        Created = 1,

        /// <summary>
        /// 任务下发
        /// </summary>
        [Description("任务下发")]
        [EnumMember(Value = "任务下发")]
        Distribute = 2,

        /// <summary>
        /// 任务回调
        /// </summary>
        [Description("任务回调")]
        [EnumMember(Value = "任务回调")]
        CallBack = 3,

        /// <summary>
        /// 任务完成
        /// </summary>
        [Description("任务完成")]
        [EnumMember(Value = "任务完成")]
        Completed = 4,

        /// <summary>
        /// 任务异常
        /// </summary>
        [Description("任务异常")]
        [EnumMember(Value = "任务异常")]
        Exception = 5,
    }
}
