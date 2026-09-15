namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask
{
    /// <summary>
    /// 转发任务到 其他工作站
    /// </summary>
    public class TransferTaskReq
    {
        /// <summary>
        /// 需要转发的任务ID
        /// </summary>
        public virtual List<long> TaskIds { get; set; } = new List<long>();

        /// <summary>
        /// 目标工作站Id
        /// </summary>
        public virtual int? WorkStationId { get; set; }

        /// <summary>
        /// 目标工作站名称
        /// </summary>
        public virtual string? WorkStationName { get; set; }

        /// <summary>
        /// 目标工作站编号
        /// </summary>
        public virtual string? WorkStationCode { get; set; }
    }
}
