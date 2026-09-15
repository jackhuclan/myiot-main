using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig
{
    public class GetSysConfigListReq : Page
    {
        /// <summary>
        /// 配置项编号
        /// </summary>
        public virtual string? ConfigCode { get; set; }
        /// <summary>
        /// 是否系统自带
        /// </summary>
        public virtual bool? IsSystem { get; set; }
        /// <summary>
        /// 是否快速配置项
        /// </summary>
        public virtual bool? IsQuickConfig { get; set; }
        /// <summary>
        /// 类别（1-default,2-Kinwong,3-Chongda)
        /// </summary>
        public virtual SysConfigCategoryEnum? Category { get; set; }

        /// <summary>
        /// 配置项值描述
        /// </summary>
        public virtual string? ConfigDescript { get; set; }
    }
}
