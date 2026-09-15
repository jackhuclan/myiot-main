namespace VgAutoDrill.Admin.Model.ViewModels
{
    public abstract class BaseAddOrUpdateWithTreeDto : GeneralBaseAddOrUpdateWithTreeDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public override sealed string? Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public override sealed string? Code { get; set; }

        /// <summary>
        /// 父对象Id(0表示是根对象) 
        ///</summary>
        public override sealed long ParentId { get; set; }

        /// <summary>
        /// 所有层级父节点
        /// </summary>
        public override sealed string? Ancestors { get; set; }
    }

    public abstract class GeneralBaseAddOrUpdateWithTreeDto : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public abstract string? Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public abstract string? Code { get; set; }

        /// <summary>
        /// 父对象Id(0表示是根对象) 
        ///</summary>
        public abstract long ParentId { get; set; }

        /// <summary>
        /// 所有层级父节点
        /// </summary>
        public abstract string? Ancestors { get; set; }
    }
}
