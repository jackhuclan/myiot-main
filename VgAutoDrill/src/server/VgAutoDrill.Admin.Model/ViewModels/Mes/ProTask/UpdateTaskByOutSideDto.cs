namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask
{
    public class UpdateTaskByOutSideDto
    {
        public virtual string? Code { get; set; }

        /// <summary>
        /// 本次排产叠数
        /// </summary>
        public virtual decimal? NowWadCount { get; set; }

    }
}
