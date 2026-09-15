namespace VgAutoDrill.Admin.Model.ViewModels.Req.Menu
{
    public class GetMenuListReq
    {
        /// <summary>
        /// 应用Code
        /// </summary>
        public string AppCode { set; get; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { set; get; } = -1;
    }
}
