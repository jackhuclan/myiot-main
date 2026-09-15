using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceTemporaryMaintenanceRecords
{
    public class DeviceTemporaryMaintenanceRecordsDto : BaseDto
    {
        /// <summary>
        /// 设备code
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 保养类型
        /// </summary>
        public virtual int? MaintenanceType { get; set; }

        /// <summary>
        /// 保养类型名称
        /// </summary>
        public virtual string? MaintenanceTypeName { get; set; }

        /// <summary>
        /// 计算类型
        /// </summary>
        public virtual DeviceTemporaryMaintenanceCountTypeEnum? CountType { get; set; }

        // <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime? StartTime { get; set; }

        // <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime? EndTime { get; set; }

        // <summary>
        /// 保养人id
        /// </summary>
        public virtual int? MaintenancePersonId { get; set; }

        // <summary>
        /// 保养人id
        /// </summary>
        public virtual string? MaintenancePersonName { get; set; }
    }
}
