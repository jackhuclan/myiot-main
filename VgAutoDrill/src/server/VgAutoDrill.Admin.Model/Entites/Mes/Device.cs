using SqlSugar;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("t_device")]
    public class Device : BaseEntityWithTree
    {
        [SugarColumn(ColumnName = "device_type_id")]
        /// <summary>
        /// 设备类型ID
        /// </summary>
        public virtual int? DeviceTypeId { get; set; }

        [SugarColumn(ColumnName = "device_type_code")]
        /// <summary>
        /// 设备类型Code
        /// </summary>
        public virtual string? DeviceTypeCode { get; set; }

        [SugarColumn(ColumnName = "device_vendor_id")]
        /// <summary>
        /// 设备供应商
        /// </summary>
        public virtual int? DeviceVendorId { get; set; }

        [SugarColumn(ColumnName = "device_brand")]
        /// <summary>
        /// 品牌
        /// </summary>
        public virtual string? DeviceBrand { get; set; }

        [SugarColumn(ColumnName = "device_spec")]
        /// <summary>
        /// 规格型号
        /// </summary>
        public virtual string? DeviceSpec { get; set; }

        /// <summary>
        /// 工位，工作站
        /// </summary>
        [SugarColumn(ColumnName = "work_station_id")]
        public int? WorkStationId { get; set; }

        /// <summary>
        /// 维护周期天
        /// </summary>
        [SugarColumn(ColumnName = "maintain_period_days")]
        public int? MaintainPeriodDays { get; set; }

        /// <summary>
        /// 设备投产日期
        /// </summary>
        [SugarColumn(ColumnName = "production_time")]
        public DateTime? ProductionTime { get; set; }
        /// <summary>
        /// 下次设备维护时间
        /// </summary>
        [SugarColumn(ColumnName = "last_maintain_time")]
        public DateTime? LastMaintainTime { get; set; }

        /// <summary>
        /// 设备参数
        /// </summary>
        [SugarColumn(ColumnName = "parameters")]
        public virtual string? Parameters { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        [SugarColumn(ColumnName = "device_status")]
        public virtual DeviceStatus? DeviceStatus { get; set; }

        /// <summary>
        /// 轴数
        /// </summary>
        [SugarColumn(ColumnName = "spindle_num")]
        public virtual int? SpindleNum { get; set; }

        /// <summary>
        /// 设备类别
        /// </summary>
        [SugarColumn(ColumnName = "device_kind")]
        public virtual DeviceKind? DeviceKind { get; set; }

        /// <summary>
        /// 交互位置
        /// </summary>
        [SugarColumn(ColumnName = "interaction_position")]
        public virtual InteractionPosition? InteractionPosition { get; set; }


        /// <summary>
        /// 机器尺寸
        /// </summary>
        [SugarColumn(ColumnName = "machine_size")]
        public virtual string? MachineSize { get; set; }

        /// <summary>
        /// 自动机器还是手动机器
        /// </summary>
        [SugarColumn(ColumnName = "is_auto")]
        public virtual bool? IsAuto { get; set; }

        /// <summary>
        /// 是否是DUO机器
        /// </summary>
        [SugarColumn(ColumnName = "is_duo")]
        public virtual bool? IsDuo { get; set; }


        /// <summary>
        /// 生料库位
        /// </summary>
        [SugarColumn(ColumnName = "raw_location_code")]
        public virtual string? RawLocationCode { get; set; }


        /// <summary>
        /// 熟料库位
        /// </summary>
        [SugarColumn(ColumnName = "clinker_location_code")]
        public virtual string? ClinkerLocationCode { get; set; }

        /// <summary>
        /// 最大板长
        /// </summary>
        [SugarColumn(ColumnName = "max_board_length")]
        public virtual int? MaxBoardLength { get; set; }

    }
}
