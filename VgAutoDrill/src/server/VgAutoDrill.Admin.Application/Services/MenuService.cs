using AutoMapper;
using Mapster;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.AppSecret;
using VgAutoDrill.Admin.Domain.Interfaces.Menu;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.Menu;
using VgAutoDrill.Admin.Model.ViewModels.Res.Menu;

namespace VgAutoDrill.Admin.Application.Services
{
    public class MenuService : BaseService, IMenuService
    {
        private readonly IMenuDomainService _domainService;
        private readonly IAppSecretDomainService _appSecretDomainService;
        private readonly IMenuAuthDomainService _menuAuthDomainService;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="appSecretDomainService"></param>
        /// <param name="menuAuthDomainService"></param>
        /// <param name="mapper"></param>
        public MenuService(IMenuDomainService domainService,
            IAppSecretDomainService appSecretDomainService,
             IMenuAuthDomainService menuAuthDomainService,
        IMapper mapper)
        {
            _domainService = domainService;
            _appSecretDomainService = appSecretDomainService;
            _menuAuthDomainService = menuAuthDomainService;
            _mapper = mapper;
        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<MenuDto>>> GetMenuList(GetMenuListReq req)
        {
            var where = PredicateBuilder.True<SysMenu>();
            where = where.And(p => p.IsDeleted == 0 && p.AppCode == req.AppCode);
            if (!string.IsNullOrEmpty(req.MenuName))
            {
                where = where.And(p => p.MenuName.Contains(req.MenuName));
            }
            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var list = await _domainService.QueryAsync(where, q => q.Sort, SqlSugar.OrderByType.Asc);
            var result = new ResponseDto<List<MenuDto>>();
            result.Data = list?.Adapt<List<MenuDto>>();
            return result;
        }

        /// <summary>
        /// 获取部门树形结构数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<List<MenuTreeDto>>> GetMenuTreeList(string appCode)
        {
            var list = await _domainService.QueryAsync(q => q.AppCode == appCode && q.IsDeleted == 0 && q.Status == 1, q => q.Id, SqlSugar.OrderByType.Asc);
            var result = new ResponseDto<List<MenuTreeDto>>();
            if (list == null || !list.Any())
            {
                return result;
            }
            result.Data = AddTreeChildN(list, 0);
            return result;
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<MenuDto>> QueryMenuByID(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail<MenuDto>("信息不存在!");
            }
            if (entity.IsDeleted == 1)
            {
                return Fail<MenuDto>("信息错误!");
            }
            var model = _mapper.Map<MenuDto>(entity);
            return Success<MenuDto>(model);
        }


        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddMenu(MenuReq req)
        {
            var model = _mapper.Map<SysMenu>(req);

            //菜单项 必须给定路由
            if (model.MenuType == 2 && !string.IsNullOrEmpty(model.Authorize) && string.IsNullOrEmpty(model.Path))
            {
                return Fail("必须给定路由");
            }

            if (string.IsNullOrEmpty(model.MenuIcon))
            {
                model.MenuIcon = "#";
            }

            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            var isExist = await _appSecretDomainService.IsExistAsync(q => q.AppCode == req.AppCode && q.IsDeleted == 0);
            if (!isExist)
            {
                return Fail("未找应用Code");
            }
            var result = await _domainService.Add(model);
            if (result)
            {
                return Success("");
            }
            return Fail("添加失败");
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateMenu(MenuReq req)
        {
            //  var model = _mapper.Map<SysMenu>(req);
            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("未找到应用菜单");
            }
            if (!entity.AppCode.Equals(req.AppCode))
            {
                return Fail("未找应用菜单code");
            }
            entity.ModifierId = UserId;
            entity.ModifyTime = DateTime.Now;
            entity.Sort = req.Sort;
            entity.Remark = req.Remark;
            entity.ParentId = req.ParentId;
            entity.MenuType = req.MenuType;
            entity.Path = req.Path;
            entity.MenuUrl = req.MenuUrl;
            entity.MenuIcon = req.MenuIcon;
            entity.MenuName = req.MenuName;
            entity.Authorize = req.Authorize;
            entity.Status = req.Status;
            var result = await _domainService.Update(entity);
            if (result)
            {
                return Success("");
            }
            return Fail("修改失败");
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteMenu(long id)
        {
            var isExist = await _domainService.IsExistAsync(q => q.Id == id);
            if (!isExist)
            {
                return Fail("信息不存在!");
            }
            isExist = await _domainService.IsExistAsync(q => q.ParentId == id);
            if (isExist)
            {
                return Fail("该菜单存在子菜单,请先删除子菜单!");
            }
            isExist = await _menuAuthDomainService.IsExistAsync(q => q.MenuId == id);
            if (isExist)
            {
                return Fail("该菜单已分配给用户,请先解绑在删除菜单!");
            }
            var result = await _domainService.DeleteById(id);
            if (result)
                return Success("");

            return Fail("删除失败");
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<int>>> GetRoleMenus(GetRoleMenusReq model)
        {
            if (model == null)
            {
                return Fail<List<int>>("请输入正确的入参");
            }
            if (string.IsNullOrEmpty(model.AppCode))
            {
                return Fail<List<int>>("请输入 AppCode");
            }
            if (model.RoleId == 0)
            {
                return Fail<List<int>>("请输入 角色id");
            }
            var list = await _menuAuthDomainService.GetRoleMenuIds(model.AppCode, model.RoleId);
            return Success(list);
        }

        #region 内部方法
        private List<MenuTreeDto> AddTreeChildN(List<SysMenu> menuList, long pid)
        {
            var data = menuList.Where(x => x.ParentId == pid);
            var list = new List<MenuTreeDto>();
            foreach (var item in data)
            {
                var childModel = new MenuTreeDto();
                childModel.Id = item.Id;
                childModel.Label = item.MenuName;
                childModel.ParentId = item.ParentId;
                childModel.AppCode = item.AppCode;
                childModel.Authorize = item.Authorize;
                childModel.CreateTime = item.CreateTime;
                childModel.MenuIcon = item.MenuIcon;
                childModel.MenuName = item.MenuName;
                childModel.MenuType = item.MenuType.ToString();
                childModel.MenuUrl = item.MenuUrl;
                childModel.Path = item.Path;
                childModel.Remark = item.Remark;
                childModel.Sort = item.Sort;
                childModel.Status = item.Status.ToString();
                childModel.Children = GetTreeChildList(menuList, childModel);
                list.Add(childModel);
            }
            return list;
        }
        private List<MenuTreeDto> GetTreeChildList(List<SysMenu> list, MenuTreeDto treeChild)
        {
            var flag = list.Where(x => x.ParentId == treeChild.Id).Count() > 0;
            if (!flag)
            {
                return null;
            }
            else
            {
                return AddTreeChildN(list, treeChild.Id);
            }
        }
        #endregion
    }
}
