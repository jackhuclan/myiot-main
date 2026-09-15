using SqlSugar;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask.Req
{
    public class AddOrUpdateManualCallAgvTaskReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 钻机编号
        /// </summary>

        public string? DeviceCode { get; set; }

        /// <summary>
        /// 物料lot号
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
        /// 库位区域类型
        /// </summary>
        public LocationType LocationType { get; set; }

        /// <summary>
        /// 库位区域类型名字
        /// </summary>
        public string? LocationTypeName { get; set; }

        /// <summary>
        /// agv操作类型名字
        /// </summary>
        public string? AgvOperateName { get; set; }

        /// <summary>
        /// agv操作类型
        /// </summary>
        public AgvOperateType? AgvOperateType { get; set; }

        /// <summary>
        /// 熟料数量
        /// </summary>
        public int? ClinkerMaterialNum { get; set; }

        /// <summary>
        /// 是否已绑定
        /// </summary>
        public bool? IsBind { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public ManualCallAgvTaskStatus TaskStatus { get; set; }

        /// <summary>
        /// 任务状态描述
        /// </summary>
        public string? TaskStatusDescription { get; set; }
    }
}
