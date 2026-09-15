using System.ComponentModel.DataAnnotations;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask
{
    public class TaskMoveByTargetTaskReq
    {
        [Required]
        /// <summary>
        /// 被移动的任务编码列表
        /// </summary>
        public virtual List<string> Code { get; set; } = new List<string>();
        [Required]
        /// <summary>
        /// 目标位置的TaskCode
        /// </summary>
        public virtual string? TargetTaskCode { get; set; }

    }
}
