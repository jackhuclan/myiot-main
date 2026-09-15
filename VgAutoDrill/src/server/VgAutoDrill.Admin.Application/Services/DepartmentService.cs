using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.Department;
using VgAutoDrill.Admin.Domain.Interfaces.User;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.Department;
using VgAutoDrill.Admin.Model.ViewModels.Res.Department;

namespace VgAutoDrill.Admin.Application.Services
{
    public class DepartmentService : BaseService, IDepartmentService
    {
        private readonly IDepartmentDomainService _domainService;
        private readonly IMapper _mapper;
        private readonly IUserDomainService _userDomainService;

        /// <summary>
        /// 部门管理
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="userDomainService"></param>
        /// <param name="mapper"></param>
        public DepartmentService(IDepartmentDomainService domainService, IUserDomainService userDomainService, IMapper mapper)
        {
            _domainService = domainService;
            _mapper = mapper;
            _userDomainService = userDomainService;
        }
        /// <summary>
        /// 获取部门数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DepartmentDto>>> GetDepartmentList(GetDepartmentListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DepartmentDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<SysDepartment>();
            where = where.And(p => p.IsDeleted == 0);
            if (!string.IsNullOrEmpty(req.DepartmentName))
            {
                where = where.And(p => p.DepartmentName.Contains(req.DepartmentName));
            }
            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.Id, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<SysDepartment>, List<DepartmentDto>>(result.ToList());
            return Success(pageDto);
        }
        /// <summary>
        /// 获取部门树形结构数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<List<DepartmentTreeDto>>> GetDepartmentTreeList()
        {
            var list = await _domainService.QueryAsync(q => q.IsDeleted == 0, q => q.Id, SqlSugar.OrderByType.Asc);
            var result = new ResponseDto<List<DepartmentTreeDto>>();
            if (list == null || !list.Any())
            {
                return result;
            }
            result.Data = AddDepartmentChildN(list, 0);
            return result;
        }
        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<DepartmentInfoDto>> QueryByID(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail<DepartmentInfoDto>("信息不存在!");
            }
            if (entity.IsDeleted == 1)
            {
                return Fail<DepartmentInfoDto>("信息错误!");
            }
            var model = _mapper.Map<DepartmentInfoDto>(entity);

            return Success(model);
        }
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> Add(AddOrUpdateDepatmentReq req)
        {
            if (req.ParentId > 0)
            {
                var isExist = await _domainService.IsExistAsync(q => q.Id == req.ParentId);
                if (!isExist)
                {
                    return Fail<string>(" 父部门不存在!");
                }
            }
            var model = _mapper.Map<SysDepartment>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            var iSave = await _domainService.Add(model);
            return Success();
        }
        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<string>> Update(AddOrUpdateDepatmentReq req)
        {
            if (req.ParentId > 0)
            {
                var isExist = await _domainService.IsExistAsync(q => q.Id == req.ParentId);
                if (!isExist)
                {
                    return Fail<string>(" 父部门不存在!");
                }
            }
            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }
            var model = _mapper.Map<SysDepartment>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            var result = await _domainService.Update(model);
            return Success();
        }
        /// <summary>
        /// 删除部门信息
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

            var isExsitUser = await _userDomainService.IsExistAsync(p => p.DepartmentId == id);
            if (isExsitUser)
            {
                return Fail("存在用户，不允许删除!");
            }

            var result = await _domainService.DeleteById(id);
            if (result)
            {
                return Success("");
            }
            return Fail("删除失败");
        }
        #region common
        private List<DepartmentTreeDto> AddDepartmentChildN(List<SysDepartment> departList, long pid)
        {
            var data = departList.Where(x => x.ParentId == pid);
            var list = new List<DepartmentTreeDto>();
            foreach (var item in data)
            {
                var childModel = new DepartmentTreeDto();
                childModel.Id = item.Id;
                childModel.Label = item.DepartmentName;
                childModel.Status = item.Status;
                childModel.Children = GetDepartmentChildList(departList, childModel);
                list.Add(childModel);
            }
            return list;
        }

        private List<DepartmentTreeDto> GetDepartmentChildList(List<SysDepartment> list, DepartmentTreeDto treeChild)
        {
            var flag = list.Where(x => x.ParentId == treeChild.Id).Count() > 0;
            if (!flag)
            {
                return null;
            }
            else
            {
                return AddDepartmentChildN(list, treeChild.Id);
            }
        }
        #endregion

    }
}
