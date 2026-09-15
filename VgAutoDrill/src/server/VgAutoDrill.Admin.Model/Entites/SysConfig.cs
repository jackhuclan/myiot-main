using SqlSugar;
using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.Entites
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("sys_config")]
    public class SysConfig : BaseEntity
    {
        /// <summary>
        /// 配置项编号
        /// </summary>
        [SugarColumn(ColumnName = "config_code")]
        public virtual string? ConfigCode { get; set; }

        /// <summary>
        /// 配置项的值
        /// </summary>
        [SugarColumn(ColumnName = "config_value")]
        public virtual string? ConfigValue { get; set; }

        /// <summary>
        /// 配置项的类型
        /// </summary>
        [SugarColumn(ColumnName = "config_type")]
        public virtual string? ConfigType { get; set; }

        /// <summary>
        /// 配置项值描述
        /// </summary>
        [SugarColumn(ColumnName = "config_descript")]
        public virtual string? ConfigDescript { get; set; }

        /// <summary>
        /// 枚举配置项值
        /// </summary>
        [SugarColumn(ColumnName = "config_enum_value")]
        public virtual string? ConfigEnumValue { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 是否系统自带
        /// </summary>
        [SugarColumn(ColumnName = "is_system")]
        public virtual bool? IsSystem { get; set; } = false;

        /// <summary>
        /// 是否快速配置项
        /// </summary>
        [SugarColumn(ColumnName = "is_quick_config")]
        public virtual bool? IsQuickConfig { get; set; } = true;

        /// <summary>
        /// 类别（1-default,2-Kinwong,3-Chongda)
        /// </summary>
        [SugarColumn(ColumnName = "category")]
        public virtual SysConfigCategoryEnum? Category { get; set; } = SysConfigCategoryEnum.Default;
    }
}
