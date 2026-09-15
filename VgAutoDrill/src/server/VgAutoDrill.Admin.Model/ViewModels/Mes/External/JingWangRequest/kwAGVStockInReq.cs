namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingWangRequest
{
    public class KwAGVStockInReq
    {
        /// <summary>
        /// lot号
        /// </summary>
        public virtual string? Container { get; set; }

        /// <summary>
        /// 货物所到达的线边仓
        /// </summary>
        public virtual string? Location { get; set; }
    }
}
