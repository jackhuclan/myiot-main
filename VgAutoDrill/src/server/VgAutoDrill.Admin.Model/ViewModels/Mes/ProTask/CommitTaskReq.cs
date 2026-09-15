using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask
{
    public class CommitTaskReq
    {
        /// <summary>
        /// 任务ID集合
        /// </summary>
        public virtual List<long> Ids { set; get; } = new List<long>();

        /// <summary>
        /// 完工状态(DRAFT/COMMITED)
        /// </summary>
        public virtual TaskStatusEnum? TaskStatus { get; set; }
    }
}
