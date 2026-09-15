using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig
{
    public class SysConfigDto : BaseDto
    {
        /// <summary>
        /// 配置项编号
        /// </summary>
        public virtual string? ConfigCode { get; set; }

        /// <summary>
        /// 配置项的值
        /// </summary>
        public virtual string? ConfigValue { get; set; }

        /// <summary>
        /// 配置项的类型
        /// </summary>
        public virtual string? ConfigType { get; set; }

        /// <summary>
        /// 配置项值描述
        /// </summary>
        public virtual string? ConfigDescript { get; set; }

        /// <summary>
        /// 枚举配置项值
        /// </summary>
        public virtual string? ConfigEnumValue { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
        public virtual bool? IsQuickConfig { get; set; }
        /// <summary>
        /// 类别（1-default,2-Kinwong,3-Chongda)
        /// </summary>
        public virtual SysConfigCategoryEnum? Category { get; set; }

        /// <summary>
        /// 是否系统自带
        /// </summary>
        public virtual bool? IsSystem { get; set; }
    }

    public class SysConfigOfMachineTypeConfig
    {
        public string? DrillMachine { get; set; }
        public int CutterBoxNum { get; set; }
    }
}
