using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation
{
    public class FitWorkStationDto : WorkstationDto
    {
        /// <summary>
        /// 开始空闲时间
        /// </summary>
        public virtual DateTime? BeginFreeTime { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public virtual DeviceStatus? DeviceStatus { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 待做任务条数（Commit）
        /// </summary>
        public virtual int? ToDoTaskCount { get; set; }
    }
}
