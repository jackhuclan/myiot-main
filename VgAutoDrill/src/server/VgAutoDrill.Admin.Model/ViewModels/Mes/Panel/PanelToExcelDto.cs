using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Panel
{
    public class PanelToExcelDto
    {
        [Column("板料编号")]
        public string? PanelCode { get; set; }

        [Column("物料代码")]
        public string? ItemCode { get; set; }

        [Column("批次号")]
        public string? BatchCode { get; set; }

        [Column("板料位置")]
        public string? BoardLocation { get; set; }

        [Column("产品状态")]
        public string? ProductStatus { get; set; }

        [Column("单叠数量")]
        public int? Pcs { get; set; }

        [Column("工位ID")]
        public long? StationId { get; set; }

        [Column("负载设备编号")]
        public string? DeviceCode { get; set; }

        [Column("设备负载板料时间")]
        public DateTime DevicePanelTime { get; set; }
        [Column("板宽")]        
        public virtual decimal? PanelWidth { get; set; }
        
        [Column("板料长度")]
        public virtual decimal? PanelLength { get; set; }
        
        [Column("偏移量")]
        public virtual decimal? PinOffset { get; set; }

    }
}
