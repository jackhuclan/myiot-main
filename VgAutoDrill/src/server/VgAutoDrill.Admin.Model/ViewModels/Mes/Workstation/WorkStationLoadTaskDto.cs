using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation
{
    public class WorkStationLoadTaskDto : WorkstationDto
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public virtual string? RouteName { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public virtual DeviceStatus? DeviceStatus { get; set; }
        public virtual DeviceKind? DeviceKind { get; set; }
        /// <summary>
        /// 负载任务数量
        /// </summary>
        public virtual int TaskCount { get; set; }
        /// <summary>
        /// 设备控制台地址
        /// </summary>
        public virtual string? DeviceConsoleAddress { get; set; }

        public virtual string? CutterGroupNo { get; set; }

    }
}
