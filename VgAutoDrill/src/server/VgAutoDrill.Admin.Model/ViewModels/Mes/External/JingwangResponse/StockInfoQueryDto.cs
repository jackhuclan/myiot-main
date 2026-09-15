namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingwangResponse
{
    public class StockInfoQueryDto
    {
        /// <summary>
        /// 请求编码
        /// </summary>
        public string reqcode { get; set; }

        /// <summary>
        /// 状态编号  0 – 成功
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string message { get; set; }

        public List<StockInfoQueryDataDto> data { get; set; }
    }

    public class StockInfoQueryDataDto
    {
        /// <summary>
        /// 物料号
        /// </summary>
        public string? product { get; set; }
        /// <summary>
        /// 托盘号
        /// </summary>
        public string? podCode { get; set; }
        /// <summary>
        /// 所在位置
        /// </summary>
        public string? address { get; set; }
        /// <summary>
        /// 所在位置区域
        /// </summary>
        public string? addressName { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string? lot { get; set; }
        /// <summary>
        /// 本托盘数量
        /// </summary>
        public decimal? qty { get; set; }
        /// <summary>
        /// 系统数量
        /// </summary>
        public decimal? sysQty { get; set; }
        /// <summary>
        /// 差数
        /// </summary>
        public decimal? difQty { get; set; }
    }
}
