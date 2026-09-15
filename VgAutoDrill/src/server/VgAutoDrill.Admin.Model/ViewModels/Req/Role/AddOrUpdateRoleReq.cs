namespace VgAutoDrill.Admin.Model.ViewModels.Req.Role
{
    public class AddOrUpdateRoleReq
    {
        public int Id { get; set; }
        /// <summary>
        /// ɫ
        /// </summary>
        public string RoleName { get; set; }

        public int Sort { get; set; }

        public int Status { get; set; }
        /// <summary>
        /// ˵idб
        /// </summary>
        public List<int> MenuIds { get; set; }
        /// <summary>
        /// 应用Code
        /// </summary>
        public string AppCode { get; set; }
    }


    public class RoleListReq : Page
    {

    }
}