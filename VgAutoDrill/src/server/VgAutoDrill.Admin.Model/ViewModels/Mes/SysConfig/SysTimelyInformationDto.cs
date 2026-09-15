using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig
{
    public class SysTimelyInformationDto
    {
        public virtual P1TimelyInfo? P1TimelyInfo { get; set; }

        public virtual P2TimelyInfo? P2TimelyInfo { get; set; }

        public virtual AlarmTimelyInfo? AlarmTimelyInfo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class P1TimelyInfo
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class P2TimelyInfo
    {

    }

    /// <summary>
    /// 告警信息及时通知
    /// </summary>
    public class AlarmTimelyInfo
    {
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string? Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string? Description { get; set; }

        /// <summary>
        /// 详细告警信息
        /// </summary>
        public virtual List<AlarmDto>? AlarmInfoList { get; set; }
    }
}
