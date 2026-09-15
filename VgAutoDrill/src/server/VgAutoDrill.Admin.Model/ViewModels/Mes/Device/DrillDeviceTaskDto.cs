using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    /// <summary>
    /// 钻机Commit任务信息
    /// </summary>
    public class DrillDeviceTaskDto
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public virtual string? DeviceId { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public virtual DeviceStatus? DeviceStatus { get; set; }

        /// <summary>
        /// 设备控制台地址
        /// </summary>
        public virtual string? DeviceConsoleAddress { get; set; }

        /// <summary>
        /// 最近一条Commit任务
        /// </summary>
        public virtual TaskDto? FirstTask { get; set; }

        /// <summary>
        /// 概述（剩余多少Commit任务）
        /// </summary>
        public virtual string? OverView { get; set; }

        /// <summary>
        /// 叫料信息
        /// </summary>
        public virtual string? CallAgvMessage { get; set; }

        /// <summary>
        /// 是否正在上下料
        /// 默认False
        /// </summary>
        public virtual bool? IsLoadingOrUnLoading { get; set; }

        /// <summary>
        /// 如果是true，那么call agv message 显示为红色；否则是正常色
        /// 默认为false
        /// </summary>
        public virtual bool? IsWarning { get; set; }
        /// <summary>
        /// 设备生产进度，百分比，0~100
        /// </summary>
        public virtual int Percentage { get; set; }

        public virtual string? RouteCode { get; set; }

        public virtual Dictionary<string, object>? Properties { get; set; }
    }
}
