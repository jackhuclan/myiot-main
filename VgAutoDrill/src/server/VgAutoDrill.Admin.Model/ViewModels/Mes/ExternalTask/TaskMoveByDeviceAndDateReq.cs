using System.ComponentModel.DataAnnotations;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask
{
    public class TaskMoveByDeviceAndDateReq
    {
        [Required]
        /// <summary>
        /// 被移动的任务编码列表
        /// </summary>
        public virtual List<string> Code { get; set; } = new List<string>();
        [Required]
        /// <summary>
        /// 目标钻机编码
        /// </summary>
        public virtual string? TargetDeviceCode { get; set; }
        [Required]
        /// <summary>
        /// 任务移动目标日期
        /// </summary>
        public virtual DateTime? TargetDate { get; set; }
    }
}
