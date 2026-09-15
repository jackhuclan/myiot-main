namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingWangRequest
{
    public class StockInfoQueryReq
    {
        /// <summary>
        /// 请求编码（必填）
        /// </summary>
        public virtual string? ReqCode { get; set; }

        /// <summary>
        /// 目标区域（必填）
        /// </summary>
        public virtual string? TargetPosArea { get; set; }
    }
}
