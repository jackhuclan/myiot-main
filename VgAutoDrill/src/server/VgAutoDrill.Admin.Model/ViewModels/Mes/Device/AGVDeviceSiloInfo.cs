using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    /// <summary>
    /// AGV当前板料信息
    /// </summary>
    public class AGVDeviceSiloInfo
    {
        public virtual string? DeviceId { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public virtual DeviceStatus? DeviceStatus { get; set; }

        /// <summary>
        /// 总层数
        /// </summary>
        public virtual int? LayerCount { get; set; }

        /// <summary>
        /// 设备控制台地址
        /// </summary>
        public virtual string? DeviceConsoleAddress { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public virtual DeviceKind DeviceKind { get; set; }

        /// <summary>
        /// 对接设备
        /// </summary>
        public virtual string? TargetDevice { get; set; }

        /// <summary>
        /// 交互方式
        /// </summary>
        public virtual string? RequestInteractionBehaviorName { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 任务产品编号
        /// </summary>
        public virtual string? TaskItemCode { get; set; }

        public virtual string? RouteCode { get; set; }

        public virtual List<SiloItemSumInfo>? AgVSiloInfo { get; set; } = new List<SiloItemSumInfo>();

        /// <summary>
        /// 最近调度记录
        /// </summary>
        public virtual List<ScheduleDto>? Schedules { get; set; }

        public virtual Dictionary<string, object>? Properties { get; set; } = new Dictionary<string, object>();

        public virtual string? SiloCode { get; set; }
    }

    public class SiloItemSumInfo
    {
        public virtual string? SiloCode { get; set; }
        /// <summary>
        /// 板料或刀具的状态
        /// </summary>
        public virtual string? ProductStatus { get; set; }
        public virtual ProductStatus? SimpleProductStatus { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 板料或刀具数量
        /// </summary>
        public virtual int? SiloCount { get; set; }

        /// <summary>
        /// 钻机根据Layer进行分组
        /// </summary>
        public virtual int? Layer { get; set; }
    }
}
