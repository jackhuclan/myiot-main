using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail
{
    public class DrillPanelFullData : DeviceDto
    {
        /// <summary>
        /// 设备控制台地址
        /// </summary>
        public virtual string? DeviceConsoleAddress { get; set; }
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

        /// <summary>
        /// 设备是否在线
        /// </summary>
        public virtual bool? isOnline { get; set; } = false;

        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 概述（剩余多少Commit任务）
        /// </summary>
        public virtual string? OverView { get; set; }

        /// <summary>
        /// 当前任务需要时间
        /// </summary>
        public virtual string? TaskDuration { get; set; }

        /// <summary>
        /// 设备异常时长
        /// </summary>
        public virtual string? AlarmTime { get; set; }

        /// <summary>
        /// 无任务时长
        /// </summary>
        public virtual string? WithoutTaskTime { get; set; }

        /// <summary>
        /// 手动时长
        /// </summary>
        public virtual string? WaitTime { get; set; }

        /// <summary>
        /// 上下料时长
        /// </summary>
        public virtual string? LoadOrUnLoadTime { get; set; }

        public virtual List<DrillSiloItemSumInfo>? SiloInfos { get; set; } = new List<DrillSiloItemSumInfo>();
    }

    public class DrillSiloItemSumInfo
    {
        public virtual string? Layer { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 板料或刀具数量
        /// </summary>
        public virtual int? SiloCount { get; set; }

        public virtual string? Position { get; set; }
    }
}
