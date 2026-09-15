using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("t_device_type")]
    public class DeviceType : BaseEntityWithTree
    {
        /// <summary>
        /// 是否属于生产设备
        /// </summary>
        [SugarColumn(ColumnName = "is_manufacture")]
        public virtual byte IsManufacture { get; set; } = 0;
    }
}
