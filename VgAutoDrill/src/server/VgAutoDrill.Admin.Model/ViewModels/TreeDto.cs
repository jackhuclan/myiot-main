namespace VgAutoDrill.Admin.Model.ViewModels
{
    public class TreeDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Label { set; get; }
        /// <summary>
        /// 子部门
        /// </summary>
        public List<TreeDto> Children { set; get; }
    }
}
