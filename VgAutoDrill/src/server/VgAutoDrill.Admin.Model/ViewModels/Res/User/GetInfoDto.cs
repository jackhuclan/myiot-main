namespace VgAutoDrill.Admin.Model.ViewModels.Res.User
{
    public class GetInfoDto
    {
        public List<string> Permissions { set; get; }

        public List<string> Roles { set; get; }

        public UserInfoDto User { set; get; }
    }
}
