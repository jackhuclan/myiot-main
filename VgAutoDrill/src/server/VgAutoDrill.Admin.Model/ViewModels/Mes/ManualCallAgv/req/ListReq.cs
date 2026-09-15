namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.req
{
    public class ListReq : Page
    {
        /// <summary>
        /// 物料号
        /// </summary>
        public string? ItemCode { get; set; }

        /// <summary>
        /// 库位号
        /// </summary>
        public string? LocationCode { get; set; }

        /// <summary>
        /// 托盘号
        /// </summary>
        public string? PodCode { get; set; }


        /// <summary>
        /// agv操作类型
        /// </summary>
        public AgvOperateType? AgvOperateType { get; set; }





    }
}
