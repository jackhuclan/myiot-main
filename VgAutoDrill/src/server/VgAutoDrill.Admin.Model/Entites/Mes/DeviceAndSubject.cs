using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 设备检验项目模板
    ///</summary>
    [SugarTable("t_device_subject")]
    public class DeviceAndSubject : BaseEntity
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        [SugarColumn(ColumnName = "device_id")]
        public int? DeviceId { get; set; }

        /// <summary>
        /// 点检保养项目id
        /// </summary>
        [SugarColumn(ColumnName = "subject_id")]
        public int? SubjectId { get; set; }
    }
}
