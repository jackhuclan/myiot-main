namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask.Req
{
    public class BindSiloBoxReq
    {
        /// <summary>
        /// 托盘编号
        /// </summary>
        public string? podCode { get; set; }
        /// <summary>
        /// 1:绑定 0:解绑
        /// </summary>
        public string? indBind { get; set; }
        /// <summary>
        /// 托盘物料信息
        /// </summary>
        public List<PodInfoEntity>? podList { get; set; }
    }

    /// <summary>
    /// 托盘信息
    /// </summary>
    public class PodInfoEntity
    {
        /// <summary>
        /// 层号
        /// </summary>
        public int boxLayerNo { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string? lotNo { get; set; }
        /// <summary>
        /// 铝片码
        /// </summary>
        public string? foldQrCode { get; set; }
        /// <summary>
        /// PLN 码
        /// </summary>
        public List<string>? PanelList { get; set; }

    }

    public abstract class STDBaseEntity
    {
        /// <summary>
        /// 请求编号，每个请求都要一个唯一编号， 同一个请求重复提交， 使用同一编号。
        /// </summary>
        public string? reqCode { get; set; }
        /// <summary>
        /// 请求时间截 格式: “yyyy-MM-dd HH:mm:ss”。 
        /// </summary>
        public string? reqTime { get; set; }
        /// <summary>
        /// 客户端编号，如PDA，HCWMS等。
        /// </summary>
        public string? clientCode { get; set; }
    }

}
