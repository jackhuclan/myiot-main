using System.ComponentModel.DataAnnotations;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask
{
    public class GetTaskReq
    {
        [Required]
        /// <summary>
        /// 钻机编号
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 是否包含已完成任务
        /// </summary>
        public virtual bool? IsContainCompleteTask { get; set; } = false;

        /// <summary>
        /// 是否包含草稿任务
        /// </summary>
        public virtual bool? IsContainDraftTask { get; set; } = false;
    }
}
