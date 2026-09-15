namespace VgAutoDrill.Admin.Model.ViewModels.Req.User
{
    /// <summary>
    /// 超级管理员重置密码
    /// </summary>
    public class ResetUserPwdByAdminReq
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string? NewPassword { get; set; }
    }
}
