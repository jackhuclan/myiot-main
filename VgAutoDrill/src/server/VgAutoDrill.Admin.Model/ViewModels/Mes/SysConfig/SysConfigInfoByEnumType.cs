using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig
{
    public class SysConfigInfoByEnumType : BaseDto
    {
        /// <summary>
        /// 配置项编号
        /// </summary>
        public virtual string? ConfigCode { get; set; }

        /// <summary>
        /// 配置项的类型
        /// </summary>
        public virtual string? ConfigType { get; set; }

        /// <summary>
        /// 配置项的值
        /// </summary>
        public virtual string? ConfigValue { get; set; }

        /// <summary>
        /// 配置项值描述
        /// </summary>
        public virtual string? ConfigDescript { get; set; }

        /// <summary>
        /// 配置项的枚举值集合
        /// </summary>
        public virtual List<SysConfigEnum>? SysConfigEnums { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 类别（0-default,1-Kinwong,2-Chognda)
        /// </summary>
        public virtual SysConfigCategoryEnum? Category { get; set; }
    }

    public class SysConfigEnum
    {
        /// <summary>
        /// 配置项的枚举值
        /// </summary>
        public virtual string? ConfigEnumValue { get; set; }

        /// <summary>
        /// 枚举值描述
        /// </summary>
        public virtual string? ConfigEnumDescript { get; set; }

    }
}
