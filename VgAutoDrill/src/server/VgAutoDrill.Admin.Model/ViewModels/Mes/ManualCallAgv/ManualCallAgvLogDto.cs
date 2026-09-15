using SqlSugar;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv
{
    public class ManualCallAgvLogDto
    {
        /// <summary>
        /// 物料号
        /// </summary>
        public string? ItemCode { get; set; }

        /// <summary>
        /// 钻机编号
        /// </summary>
        public string? DeviceCode { get; set; }

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

        /// <summary>
        /// agv操作类型名字
        /// </summary>
        public string? AgvOperateName { get; set; }

        /// <summary>
        /// 熟料数量
        /// </summary>
        public int? ClinkerMaterialNum { get; set; }


        /// <summary>
        /// 请求url
        /// </summary>
        public string? RequestUrl { get; set; }

        /// <summary>
        /// 呼叫返回信息
        /// </summary>
        public string? CallBackMessage { get; set; }

        /// <summary>
        /// 创建人Id 
        ///</summary>
        public int? CreatorId { get; set; }

        /// <summary>
        /// 创建人名字
        ///</summary>
        public string? Creator { get; set; }
        /// <summary>
        /// 创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }
}
