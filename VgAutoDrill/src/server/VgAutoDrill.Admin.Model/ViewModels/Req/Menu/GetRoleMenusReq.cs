namespace VgAutoDrill.Admin.Model.ViewModels.Req.Menu
{
    /// <summary>
    /// 
    /// </summary>
    public class GetRoleMenusReq
    {
        /// <summary>
        /// app应用
        /// </summary>
        public string AppCode { set; get; }

        /// <summary>
        /// 角色id
        /// </summary>
        public int RoleId { set; get; }
    }
}
