namespace VgAutoDrill.Admin.Model.ViewModels.Req.User
{
    public class ResetUserPwdReq
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 原密码
        /// </summary>
        public string? OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string? NewPassword { get; set; }
    }
}
