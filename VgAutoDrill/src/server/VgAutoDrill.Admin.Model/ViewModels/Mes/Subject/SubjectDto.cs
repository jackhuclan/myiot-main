namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Subject
{
    public class SubjectDto : BaseDtoWithTreeDto
    {
        /// <summary>
        /// 类型 点检or保养
        /// </summary>
        public virtual string? SubjectType { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual string? SubjectContent { get; set; }

        /// <summary>
        /// 标准
        /// </summary>
        public virtual string? Standard { get; set; }
    }
}
