namespace VgAutoDrill.Admin.Model.ViewModels.Res.Role
{
    public class RoleDto
    {
        public int Id { set; get; }

        public string RoleName { set; get; }

        public int Sort { set; get; }

        public int Status { set; get; }

        public string CreateTime { set; get; }
        /// <summary>
        /// �˵�id�б�
        /// </summary>
        public List<int> MenuIds { get; set; }
    }
}