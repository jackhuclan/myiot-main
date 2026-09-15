namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SchedulementDetail
{
    public class GetScheduleLogListReq : Page
    {
        /// <summary>
        /// 主表Id
        /// </summary>
        public virtual long? MasterId { get; set; }

        /// <summary>
        /// 调度记录明细
        /// </summary>
        public virtual string? Message { get; set; }
    }
}
