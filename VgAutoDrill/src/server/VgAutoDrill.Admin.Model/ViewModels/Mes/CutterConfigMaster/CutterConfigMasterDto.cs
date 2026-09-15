namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CutterConfigMaster
{
    public class CutterConfigMasterDto : BaseDto
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        public virtual string? ConfigName { get; set; }

        /// <summary>
        /// 配置描述
        /// </summary>
        public virtual string? ConfigDesc { get; set; }

        /// <summary>
        /// dia文件路径
        /// </summary>
        public virtual string? DiaFilePath { get; set; }

        /// <summary>
        /// dia文件名
        /// </summary>
        public virtual string? DiaFileName { get; set; }

        /// <summary>
        /// 产品大类ID
        /// </summary>
        public virtual long? ProductCategoryId { get; set; }

        /// <summary>
        /// 产品大类编码
        /// </summary>
        public virtual string? ProductCategoryCode { get; set; }

        /// <summary>
        /// 产品大类名称
        /// </summary>
        public virtual string? ProductCategoryName { get; set; }
    }
}
