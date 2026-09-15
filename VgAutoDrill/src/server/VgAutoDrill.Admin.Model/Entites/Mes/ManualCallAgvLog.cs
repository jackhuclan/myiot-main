using SqlSugar;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 手动呼叫AGV记录
    /// </summary>
    [SugarTable("t_manual_call_agv_log")]
    public class ManualCallAgvLog : BaseEntity
    {
        /// <summary>
        /// 物料号
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public string? ItemCode { get; set; }

        /// <summary>
        /// 钻机编号
        /// </summary>
        [SugarColumn(ColumnName = "device_code")]
        public string? DeviceCode { get; set; }

        /// <summary>
        /// 库位号
        /// </summary>
        [SugarColumn(ColumnName = "location_code")]
        public string? LocationCode { get; set; }

        /// <summary>
        /// 托盘号
        /// </summary>
        [SugarColumn(ColumnName = "pod_code")]
        public string? PodCode { get; set; }

        /// <summary>
        /// agv操作类型名字
        /// </summary>
        [SugarColumn(ColumnName = "agv_operate_name")]
        public string? AgvOperateName { get; set; }

        /// <summary>
        /// agv操作类型
        /// </summary>
        [SugarColumn(ColumnName = "agv_operate_type")]
        public AgvOperateType? AgvOperateType { get; set; }

        /// <summary>
        /// 熟料数量
        /// </summary>
        [SugarColumn(ColumnName = "clinker_material_num")]
        public int? ClinkerMaterialNum { get; set; }


        /// <summary>
        /// 请求url
        /// </summary>
        [SugarColumn(ColumnName = "request_url")]
        public string? RequestUrl { get; set; }

        /// <summary>
        /// 呼叫返回信息
        /// </summary>
        [SugarColumn(ColumnName = "call_back_message")]
        public string? CallBackMessage { get; set; }



    }
}
