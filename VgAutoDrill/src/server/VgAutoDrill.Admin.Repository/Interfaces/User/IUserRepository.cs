using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.ViewModels.Req.User;

namespace VgAutoDrill.Admin.Repository.Interfaces.User
{
    public interface IUserRepository : IBaseRepository<SysUser>
    {
        Task<IPageList<dynamic>> GetUserList(GetUserPageListReq req);
    }
}
