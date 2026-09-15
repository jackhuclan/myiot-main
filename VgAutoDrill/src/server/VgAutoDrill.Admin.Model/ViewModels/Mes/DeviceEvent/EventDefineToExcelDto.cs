using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceEvent
{
    public class EventDefineToExcelDto
    {
        [Column("事件ID")]
        public string? EventId { get; set; }

        [Column("事件名称")]
        public string? EventName { get; set; }

        [Column("事件级别")]
        public int? EventLevel { get; set; }

        [Column("参数配置")]
        public string? ParameterJson { get; set; }
    }
}
