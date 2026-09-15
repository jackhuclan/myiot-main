using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.Role;
using VgAutoDrill.Admin.Model.ViewModels.Res.Role;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    public interface IRoleService
    {
        Task<ResponseDto<string>> Add(AddOrUpdateRoleReq req);

        Task<ResponseDto<string>> Update(AddOrUpdateRoleReq req);

        Task<ResponseDto<string>> Delete(long id);

        Task<ResponseDto<RoleDto>> QueryByID(long id);

        Task<ResponseDto<PageDto<RoleDto>>> GetPageList(GetRolePageListReq req);

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteList(object[] idList);
    }
}