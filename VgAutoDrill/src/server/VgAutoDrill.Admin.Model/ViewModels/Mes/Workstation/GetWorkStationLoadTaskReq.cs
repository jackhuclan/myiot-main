using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation
{
    public class GetWorkStationLoadTaskReq : Page
    {
        /// <summary>
        /// 任务编码
        /// </summary>
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 工作站编号
        /// </summary>
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public virtual string? ProcessCode { get; set; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 完工状态(DRAFT/COMMITED/BEGIN/FINISH)
        /// </summary>
        public virtual List<TaskStatusEnum>? TaskStatusList { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public virtual List<DeviceStatus>? DeviceStatusList { get; set; }

        /// <summary>
        /// 查询开始时间
        /// </summary>
        public DateTime? QueryStartTime { set; get; }

        /// <summary>
        /// 查询结束时间
        /// </summary>
        public DateTime? QueryEndTime { set; get; }
        public virtual List<DeviceKind>? RequestDeviceKindList { get; set; }
    }
}
