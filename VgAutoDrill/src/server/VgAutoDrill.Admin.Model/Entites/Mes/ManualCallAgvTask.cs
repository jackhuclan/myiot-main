using SqlSugar;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 手动呼叫AGV任务
    /// </summary>
    [SugarTable("t_manual_call_agv_task")]
    public class ManualCallAgvTask : BaseEntity
    {
        /// <summary>
        /// 钻机编号
        /// </summary>
        [SugarColumn(ColumnName = "device_code")]
        public string? DeviceCode { get; set; }

        /// <summary>
        /// 物料lot号
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public string? ItemCode { get; set; }

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
        /// 库位区域类型
        /// </summary>
        [SugarColumn(ColumnName = "location_type")]
        public LocationType LocationType { get; set; }

        /// <summary>
        /// 库位区域类型名字
        /// </summary>
        [SugarColumn(ColumnName = "location_type_name")]
        public string? LocationTypeName { get; set; }


        /// <summary>
        /// 熟料数量
        /// </summary>
        [SugarColumn(ColumnName = "clinker_material_num")]
        public int? ClinkerMaterialNum { get; set; }

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
        /// 是否已绑定
        /// </summary>
        [SugarColumn(ColumnName = "is_bind")]
        public bool? IsBind { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        [SugarColumn(ColumnName = "task_status")]
        public ManualCallAgvTaskStatus TaskStatus { get; set; }

        /// <summary>
        /// 任务状态描述
        /// </summary>
        [SugarColumn(ColumnName = "task_status_description")]
        public string? TaskStatusDescription { get; set; }



    }
}
