namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Subject
{
    public class GetSubjectListReq : Page
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string? Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string? Code { get; set; }

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
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { set; get; } = -1;
    }
}
