using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.User;
using VgAutoDrill.Admin.Model.ViewModels.Res.User;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginReq"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Login(LoginReq loginReq);
        Task<ResponseDto<string>> LoginForDebug(LoginReq loginReq);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<GetInfoDto>> GetInfo();
        /// <summary>
        /// 获取当前用户权限菜单
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<List<GetRoutersDto>>> GetRouters();
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<CaptchaImageDto>> CaptchaImage();
        /// <summary>
        /// 获取用户分页列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<UserInfoDto>>> PageList(GetUserPageListReq req);

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<UserDetailDto>> QueryByID(long id);
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Add(AddUserReq req);
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Update(AddUserReq req);

        /// <summary>
        /// 修改用户基本信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateSimple(UpdateSimpleUserReq req);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Delete(long id);
        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> ChangeStatus(ChangeUserStatusReq req);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> ResetPwd(ResetUserPwdReq req);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> ResetPwdByAdmin(ResetUserPwdByAdminReq req);
        /// <summary>
        /// 批量添加用户
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> BatchAddUser(List<AddUserReq> req);

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteList(List<long> idList);
    }
}
