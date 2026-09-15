using VgAutoDrill.Admin.Model.ViewModels.Mes.SchedulementDetail;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement
{
    public class ScheduleLogsDto : ScheduleDto
    {
        /// <summary>
        /// 调度详情
        /// </summary>
        public virtual DeviceEventReportRequest RequestJson { get; set; } = new DeviceEventReportRequest();
        public List<ScheduleLogDto>? ScheduleLogs { get; set; }
        public string? LocationCode { get { return SubDeviceCode; } }
    }
}
