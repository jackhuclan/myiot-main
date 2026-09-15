namespace VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack
{
    /// <summary>
    /// 统计近一周产量
    /// </summary>
    public class FeedBackStatsByTimeDto
    {
        /// <summary>
        /// 时间段
        /// </summary>
        public virtual List<string> Times { get; set; } = new List<string>();

        /// <summary>
        /// 每个工序产量以及合格率
        /// </summary>
        public virtual List<QuantitysStatsDto> QuantitysStats { get; set; } = new List<QuantitysStatsDto>();

    }

    /// <summary>
    /// 每个工序产量
    /// </summary>
    public class QuantitysStatsDto
    {
        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 数量集合
        /// </summary>
        public virtual List<string> Quantitys { get; set; } = new List<string>();

    }
}
