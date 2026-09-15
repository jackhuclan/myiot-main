using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.User;
using VgAutoDrill.Admin.Model.ViewModels.Res.User;

namespace VgAutoDrill.Admin.Domain.Interfaces.User
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserDomainService : IBaseDomainService<SysUser>
    {
        /// <summary>
        /// 添加修改用户信息
        /// </summary>
        /// <param name="req">用户信息</param>
        /// <param name="creatorId">操作人ID</param>
        /// <returns></returns>
        Task<string> AddOrUpdateUserInfo(AddUserReq req, int creatorId);
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> DeleteUserById(long id);
        /// <summary>
        /// 获取用户分页列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PageDto<UserInfoDto>> PageList(GetUserPageListReq req);
        /// <summary>
        /// 批量添加用户信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> BatchAdd(List<SysUser> list);

        Task<List<SysUser>> GetListByIds(List<int> userIds);
    }
}
