namespace VgAutoDrill.Admin.Model.ViewModels.Req.User
{
    public class UpdateSimpleUserReq
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 真实姓名 
        ///</summary>
        public virtual string RealName { get; set; }

        /// <summary>
        /// 性别(1:男 0:女) 
        ///</summary>
        public virtual int? Sex { get; set; }

        /// <summary>
        /// 手机 
        ///</summary>
        public virtual string Mobile { get; set; }

        /// <summary>
        /// 邮箱  
        ///</summary>
        public virtual string Email { get; set; }

    }
}
