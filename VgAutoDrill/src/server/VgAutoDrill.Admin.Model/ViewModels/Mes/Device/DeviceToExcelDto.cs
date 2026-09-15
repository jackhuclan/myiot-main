using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    public class DeviceToExcelDto
    {
        [Column("设备编码（必填）")]
        public string Code { set; get; }

        [Column("设备名称（必填）")]
        public string Name { set; get; }

        [Column("设备类型代码（必填）")]
        public string DeviceTypeCode { get; set; }

        [Column("轴数（必填）")]
        public int SpindleNum { get; set; }
    }
}
