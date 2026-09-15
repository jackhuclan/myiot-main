using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 分区配置表
    ///</summary>
    [SugarTable("t_partition_setting")]
    public class PartitionSetting : BaseEntity
    {
        /// <summary>
        /// 分区编号code
        /// </summary>
        [SugarColumn(ColumnName = "partition_id")]
        public virtual long? partition_id { get; set; }//有用到实体导航，只能跟数据库里面的字段保持一致，不然找不到字段匹配外键

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


    }
}
