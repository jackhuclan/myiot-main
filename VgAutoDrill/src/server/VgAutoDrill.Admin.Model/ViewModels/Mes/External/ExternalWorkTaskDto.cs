using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External
{
    public class ExternalWorkTaskDto : ExternalWorkOrderDto
    {
        /// <summary>
        /// 生产工单的状态(DRAFT/COMMITED/SCHEDULED/BEGIN/FINISH)
        /// </summary>
        public virtual string? WorkOrderStatus { get; set; } = "DRAFT";

        /// <summary>
        /// 完工状态(DRAFT/COMMITED/SCHEDULED/BEGIN/FINISH)
        /// </summary>
        public virtual ManuOrderStatusEnum? ManuOrderStatus { get; set; } = ManuOrderStatusEnum.DRAFT;
        /// <summary>
        /// 工单任务
        /// </summary>
        public List<TaskViewDto> WorkTasks { get; set; } = new List<TaskViewDto>();


    }
}
