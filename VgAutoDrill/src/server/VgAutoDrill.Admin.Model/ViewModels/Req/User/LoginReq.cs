namespace VgAutoDrill.Admin.Model.ViewModels.Req.User
{
    public class LoginReq
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { set; get; }
        /// <summary>
        /// 验证码请求ID
        /// </summary>
        public string Uuid { set; get; }


    }
}
