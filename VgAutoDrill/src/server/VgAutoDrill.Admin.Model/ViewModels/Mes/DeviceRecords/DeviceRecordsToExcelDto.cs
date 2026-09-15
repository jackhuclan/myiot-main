using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecords
{
    public class DeviceRecordsToExcelDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [Column("设备编号")]
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 工作时间
        /// </summary>
        [Column("工作时间")]
        public virtual string? WorkTime { get; set; }

        /// <summary>
        /// 等待时间
        /// </summary>
        [Column("等待时间")]
        public virtual string? WaitTime { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        [Column("异常时间")]
        public virtual string? ErrorTime { get; set; }

        /// <summary>
        /// 开机时间
        /// </summary>
        [Column("开机时间")]
        public virtual string? OpenTime { get; set; }

        /// <summary>
        /// 稼动率
        /// </summary>
        [Column("稼动率")]
        public virtual string? Duty { get; set; }

        /// <summary>
        /// 结束到开始总时间
        /// </summary>
        [Column("结束到开始总时间")]
        public virtual string? EndToStartTime { get; set; }

        /// <summary>
        /// 清洗夹头总时间
        /// </summary>
        [Column("清洗夹头总时间")]
        public virtual string? CollectClearTime { get; set; }

        /// <summary>
        /// 数据日期
        /// </summary>
        [Column("数据日期")]
        public virtual string? DateString { get; set; }
    }
}
