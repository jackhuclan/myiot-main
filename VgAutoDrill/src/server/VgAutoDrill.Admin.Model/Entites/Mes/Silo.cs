using SqlSugar;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 料仓
    /// </summary>
    [SugarTable("t_silo")]
    public class Silo : BaseEntity
    {
        // <summary>
        /// 编码
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }
        /// <summary>
        /// 运载尺寸
        /// </summary>
        [SugarColumn(ColumnName = "size")]
        public virtual string? Size { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        [SugarColumn(ColumnName = "floor_count")]
        public virtual int? FloorCount { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        [SugarColumn(ColumnName = "vendor_id")]
        public virtual long? VendorId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [SugarColumn(ColumnName = "vendor_name")]
        public virtual string? VendorName { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        [SugarColumn(ColumnName = "vendor_code")]
        public virtual string? VendorCode { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        [SugarColumn(ColumnName = "location")]
        public virtual string? Location { get; set; }
        /// <summary>
        /// 关联设备类别
        /// </summary>
        [SugarColumn(ColumnName = "related_device_kind")]
        public virtual DeviceKind? RelatedDeviceKind { get; set; }
        /// <summary>
        /// 料仓状态
        /// </summary>
        [SugarColumn(ColumnName = "silo_status")]
        public virtual SiloStatus SiloStatus { get; set; }
        /// <summary>
        /// 空料仓
        /// </summary>
        [SugarColumn(ColumnName = "empty_silo")]
        public virtual string? EmptySilo { get; set; }
    }
}
