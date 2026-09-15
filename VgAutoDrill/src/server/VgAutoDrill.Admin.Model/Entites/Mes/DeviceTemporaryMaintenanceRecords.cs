using SqlSugar;
using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 设备临时保养记录表
    /// </summary>
    [SugarTable("t_device_temporary_maintenance_records")]
    public class DeviceTemporaryMaintenanceRecords : BaseEntity
    {
        /// <summary>
        /// 设备code
        /// </summary>
        [SugarColumn(ColumnName = "device_code")]
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 保养类型
        /// </summary>
        [SugarColumn(ColumnName = "maintenance_type")]
        public virtual int? MaintenanceType { get; set; }

        /// <summary>
        /// 保养类型名称
        /// </summary>
        [SugarColumn(ColumnName = "maintenance_type_name")]
        public virtual string? MaintenanceTypeName { get; set; }


        /// <summary>
        /// 计算类型
        /// </summary>
        [SugarColumn(ColumnName = "count_type")]
        public virtual DeviceTemporaryMaintenanceCountTypeEnum? CountType { get; set; }



        // <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(ColumnName = "start_time")]
        public virtual DateTime? StartTime { get; set; }

        // <summary>
        /// 结束时间
        /// </summary>
        [SugarColumn(ColumnName = "end_time")]
        public virtual DateTime? EndTime { get; set; }

        // <summary>
        /// 保养人id
        /// </summary>
        [SugarColumn(ColumnName = "maintenance_person_id")]
        public virtual int? MaintenancePersonId { get; set; }

        // <summary>
        /// 保养人id
        /// </summary>
        [SugarColumn(ColumnName = "maintenance_person_name")]
        public virtual string? MaintenancePersonName { get; set; }


    }
}
