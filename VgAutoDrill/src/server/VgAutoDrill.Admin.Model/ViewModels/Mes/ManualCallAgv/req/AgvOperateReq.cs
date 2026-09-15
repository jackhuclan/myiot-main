namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.req
{
    public class AgvOperateReq
    {
        /// <summary>
        /// 钻机code
        /// </summary>
        public string? DeviceCode { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public string? Lot { get; set; }
        /// <summary>
        /// 库位号
        /// </summary>
        public string? LocationCode { get; set; }
        /// <summary>
        /// 托盘号
        /// </summary>
        public string? PodCode { get; set; }
        /// <summary>
        /// 操作类型  1、上料 2、退空盘 3、上机解绑 4、叫空盘 5、下料 6、下机绑定
        /// </summary>
        public AgvOperateType AgvOperateType { get; set; }
        /// <summary>
        /// 熟料数量
        /// </summary>
        public int? ClinkerMaterialNum { get; set; }

    }
}
