namespace VgAutoDrill.Admin.Model.ViewModels.Mes.PartitionSetting
{
    public class PartitionSettingDto : BaseDto
    {
        /// <summary>
        /// 分区编号code
        /// </summary>
        public virtual string? PartitionId { get; set; }

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

    }
}
