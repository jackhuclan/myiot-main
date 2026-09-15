using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.Menu;
using VgAutoDrill.Admin.Domain.Interfaces.Role;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.Role;
using VgAutoDrill.Admin.Model.ViewModels.Res.Role;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleService : BaseService, IRoleService
    {
        private readonly IRoleDomainService _domainService;
        private readonly IMenuDomainService _menuDomainService;
        private readonly IMenuAuthDomainService _menuAuthDomainService;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="menuDomainService"></param>
        /// <param name="menuAuthDomainService"></param>
        /// <param name="mapper"></param>
        public RoleService(IRoleDomainService domainService,
            IMenuDomainService menuDomainService,
            IMenuAuthDomainService menuAuthDomainService,
            IMapper mapper)
        {
            _domainService = domainService;
            _menuAuthDomainService = menuAuthDomainService;
            _menuDomainService = menuDomainService;
            _mapper = mapper;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> Add(AddOrUpdateRoleReq req)
        {
            var model = _mapper.Map<SysRole>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            if (req.MenuIds != null && req.MenuIds.Count > 0)
            {
                foreach (var item in req.MenuIds)
                {
                    var isExist = await _menuDomainService.IsExistAsync(q => q.Id == item);
                    if (!isExist)
                    {
                        return Fail($"添加失败  MenuIds:{item}");
                    }
                }
            }
            var id = await _domainService.AddReturnId(model);
            if (id > 0)
            {
                if (req.MenuIds != null && req.MenuIds.Count > 0)
                {
                    List<SysMenuAuth> addList = new List<SysMenuAuth>();
                    foreach (var item in req.MenuIds)
                    {
                        SysMenuAuth addModel = new SysMenuAuth
                        {
                            AuthorizeId = id,
                            AuthorizeType = 1,
                            CreateTime = DateTime.Now,
                            CreatorId = UserId,
                            MenuId = item
                        };
                        addList.Add(addModel);
                    }
                    await _menuAuthDomainService.BulkInsert(addList);
                }
                return Success("");
            }

            return Fail("添加失败");
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> Update(AddOrUpdateRoleReq req)
        {
            var model = _mapper.Map<SysRole>(req);
            var entity = await _domainService.QueryByID(req.Id);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            if (entity == null)
            {
                return Fail("信息不存在!");
            }
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            var result = await _domainService.Update(model);
            if (result)
            {
                await _menuAuthDomainService.DeletMenuAuth(entity.Id, (int)AuthorizeTypeEnum.Rule, req.AppCode);

                if (req.MenuIds != null && req.MenuIds.Count > 0)
                {
                    List<SysMenuAuth> addList = new List<SysMenuAuth>();
                    foreach (var item in req.MenuIds)
                    {
                        SysMenuAuth addModel = new SysMenuAuth
                        {
                            AuthorizeId = entity.Id,
                            AuthorizeType = (int)AuthorizeTypeEnum.Rule,
                            CreateTime = DateTime.Now,
                            CreatorId = UserId,
                            MenuId = item
                        };
                        addList.Add(addModel);
                    }
                    await _menuAuthDomainService.BulkInsert(addList);
                }

                return Success("");
            }
            return Fail("修改失败");
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> Delete(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }
            var result = await _domainService.DeleteById(id);
            if (result)
            {
                await _menuAuthDomainService.DeletMenuAuth(entity.Id, (int)AuthorizeTypeEnum.Rule);
                return Success("");

            }

            return Fail("删除失败");
        }

        /// <summary>
        /// 批量删除信息
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public virtual async Task<ResponseDto<string>> DeleteList(object[] idList)
         {
            if (idList == null)
            {
                return Fail("信息不存在!");
            }

            foreach (var item in idList)
            {
                var entity = await _domainService.QueryByID(item.ToInt());
                if (entity == null)
                {
                    continue;
                }
                await _menuAuthDomainService.DeletMenuAuth(entity.Id, (int)AuthorizeTypeEnum.Rule);
            }

            await _domainService.DeleteByIds(idList);

            return Success("删除成功");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<RoleDto>> QueryByID(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail<RoleDto>("信息不存在!");
            }
            if (entity.IsDeleted == 1)
            {
                return Fail<RoleDto>("信息错误!");
            }
            var model = _mapper.Map<RoleDto>(entity);

            return Success<RoleDto>(model);
        }
        /// <summary>
        /// 获取角色分页数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<RoleDto>>> GetPageList(GetRolePageListReq req)
        {
            var pageDto = new PageDto<RoleDto>(req.PageNum, req.PageSize);
            var where = PredicateBuilder.True<SysRole>();
            if (!string.IsNullOrEmpty(req.RoleName))
            {
                where = where.And(p => p.RoleName.Contains(req.RoleName));
            }
            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }
            if (!string.IsNullOrEmpty(req.Params.BeginTime) && !string.IsNullOrEmpty(req.Params.EndTime))
            {
                var beginTime = Convert.ToDateTime(req.Params.BeginTime);
                var endTime = Convert.ToDateTime(req.Params.EndTime).AddDays(1);
                where = where.And(p => p.CreateTime >= beginTime && p.CreateTime < endTime);
            }
            var result = await _domainService.QueryPageAsync(where, p => p.Sort, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<SysRole>, List<RoleDto>>(result.ToList());
            return Success<PageDto<RoleDto>>(pageDto);
        }
    }
}