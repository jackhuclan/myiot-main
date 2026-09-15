using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask
{
    public class UpdateTaskListByOutSideReq
    {
        public List<UpdateTaskByOutSideDto> UpdateTasks { get; set; } = new List<UpdateTaskByOutSideDto>();

        /// <summary>
        /// 完工状态(DRAFT/COMMITED/BEGIN/FINISH)
        /// </summary>
        public virtual TaskStatusEnum? TaskStatus { get; set; }
    }
}
