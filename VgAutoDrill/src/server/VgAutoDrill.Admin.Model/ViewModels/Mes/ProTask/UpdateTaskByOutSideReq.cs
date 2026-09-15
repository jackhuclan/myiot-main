using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask
{
    public class UpdateTaskByOutSideReq : UpdateTaskByOutSideDto
    {
        /// <summary>
        /// 完工状态(DRAFT/COMMITED/BEGIN/FINISH)
        /// </summary>
        public virtual TaskStatusEnum? TaskStatus { get; set; }

        /// <summary>
        /// 是否来自后端网页
        /// </summary>
        public virtual bool? IsFromAdminWeb { get; set; }
    }
}
