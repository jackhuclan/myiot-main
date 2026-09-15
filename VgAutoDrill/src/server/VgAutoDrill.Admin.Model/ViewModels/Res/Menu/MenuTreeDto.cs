namespace VgAutoDrill.Admin.Model.ViewModels.Res.Menu
{
    public class MenuTreeDto : MenuDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Label { set; get; }
        /// <summary>
        /// 子部门
        /// </summary>
        public List<MenuTreeDto> Children { set; get; }
    }
}
