using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.Menu;
using VgAutoDrill.Admin.Model.ViewModels.Res.Menu;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    public interface IMenuService
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<MenuDto>>> GetMenuList(GetMenuListReq req);
        /// <summary>
        /// 获取部门树形结构数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<List<MenuTreeDto>>> GetMenuTreeList(string appCode);
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<MenuDto>> QueryMenuByID(long id);
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddMenu(MenuReq req);
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateMenu(MenuReq req);
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteMenu(long id);

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<List<int>>> GetRoleMenus(GetRoleMenusReq model);


    }
}
