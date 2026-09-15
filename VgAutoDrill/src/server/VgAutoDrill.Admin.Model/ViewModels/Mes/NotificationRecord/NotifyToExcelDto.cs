using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.NotificationRecord
{
    public class NotifyToExcelDto
    {
        [Column("通知时间")]
        public DateTime? NotifyTime { get; set; }

        [Column("通知类型编号")]
        public string? NotifyCode { get; set; }

        [Column("通知类型名称")]
        public string? NotifyName { get; set; }

        [Column("通知内容")]
        public string? NotifyMsg { get; set; }

        [Column("通知方式")]
        public string? NotifyWaysName { get; set; }
    }
}
