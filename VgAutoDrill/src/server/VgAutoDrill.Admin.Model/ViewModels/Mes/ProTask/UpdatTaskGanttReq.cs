namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask
{
    public class UpdatTaskGanttReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
