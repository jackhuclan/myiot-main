namespace VgAutoDrill.Admin.Model.ViewModels
{
    public abstract class BaseTreeDto<T> : GeneralBaseTreeDto
    {
        /// <summary>
        /// id
        /// </summary>
        public override sealed long Id { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        public override sealed string Label { set; get; }
        /// <summary>
        /// 代号
        /// </summary>
        public override string Code { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public override sealed int Status { get; set; }
        /// <summary>
        /// 子对象
        /// </summary>
        public virtual List<T> Children { set; get; }
    }

    public abstract class GeneralBaseTreeDto
    {
        /// <summary>
        /// id
        /// </summary>
        public abstract long Id { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        public abstract string Label { set; get; }
        /// <summary>
        /// 代号
        /// </summary>
        public abstract string Code { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public abstract int Status { get; set; }
    }
}
