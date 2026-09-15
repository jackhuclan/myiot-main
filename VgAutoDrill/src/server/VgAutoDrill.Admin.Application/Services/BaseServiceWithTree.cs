using AutoMapper;
using NPOI.SS.Formula.Functions;
using VgAutoDrill.Admin.Domain.Interfaces;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 具有树结构功能的基础服务类
    /// </summary>
    public abstract class BaseServiceWithTree<TEntity, TTreeDto, TDto, TAddOrUpdateReq> : BaseService
                    where TEntity : BaseEntityWithTree
                    where TTreeDto : BaseTreeDto<TTreeDto>, new()
        where TAddOrUpdateReq : BaseAddOrUpdateWithTreeDto

    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly IBaseDomainService<TEntity> _domainService;
        /// <summary>
        /// 
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        protected BaseServiceWithTree(IBaseDomainService<TEntity> domainService, IMapper mapper)
        {
            _domainService = domainService;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResponseDto<TDto>> QueryByID(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail<TDto>("信息不存在!");
            }
            if (entity.IsDeleted == 1)
            {
                return Fail<TDto>("信息错误!");
            }
            var model = _mapper.Map<TDto>(entity);

            return Success(model);
        }

        /// <summary>
        /// 批量删除信息
        /// </summary>
        /// <param name="delList"></param>
        /// <returns></returns>
        public virtual async Task<ResponseDto<string>> DeleteList(object[] delList)
        {
            if (delList == null)
            {
                return Fail("信息不存在!");
            }

            var result = await _domainService.DeleteByIds(delList);
            if (!result)
            {
                return Fail("删除失败");
            }

            return Success("删除成功");
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ResponseDto<string>> Delete(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }
            var result = await _domainService.DeleteById(id);
            if (result)
            {
                return Success("");
            }
            return Fail("删除失败");
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async virtual Task<ResponseDto<string>> Add(TAddOrUpdateReq req)
        {
            //所有层级父节点
            var ancestors = "0";
            if (req.ParentId > 0)
            {
                var parentType = await _domainService.QueryByID(req.ParentId);
                if (parentType == null)
                {
                    return Fail<string>(" 父对象不存在!");
                }

                ancestors = parentType.Ancestors + "," + req.ParentId.ToString();
            }

            var isExist = await _domainService.IsExistAsync(p => p.Code == req.Code);
            if (isExist)
            {
                return Fail<string>("编码已存在!");
            }

            var model = _mapper.Map<TEntity>(req);
            model.Ancestors = ancestors;
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            await _domainService.Add(model);
            return Success();
        }
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        public async virtual Task<ResponseDto<string>> Update(TAddOrUpdateReq req)
        {
            //所有层级父节点
            var ancestors = "0";
            if (req.ParentId > 0)
            {
                var parentType = await _domainService.QueryByID(req.ParentId);
                if (parentType == null)
                {
                    return Fail<string>(" 父对象不存在!");
                }

                ancestors = parentType.Ancestors + "," + req.ParentId.ToString();
            }
            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            var isExist = await _domainService.IsExistAsync(p => p.Code == req.Code && p.Id != req.Id);
            if (isExist)
            {
                return Fail<string>("编码已存在!");
            }

            var model = _mapper.Map<TEntity>(req);
            model.Ancestors = ancestors;
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;

            var result = await _domainService.Update(model);
            if (!result)
            {
                return Fail<string>("更新失败");
            }

            return Success();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        protected virtual List<TTreeDto> AddChildN(List<TEntity> entities, long pid)
        {
            var data = entities.Where(x => x.ParentId == pid).ToList();
            var list = new List<TTreeDto>();
            if (data?.Count <= 0)
            {
                return list;
            }         
            foreach (var item in data)
            {
                var childModel = new TTreeDto();
                childModel.Id = item.Id;
                childModel.Label = string.IsNullOrEmpty(item.Name) ? "" : item.Name;
                childModel.Code = string.IsNullOrEmpty(item.Code) ? "" : item.Code;
                childModel.Status = item.Status;
                var ortherList= entities.Where(x => x.Id != item.Id).ToList();
                childModel.Children = AddChildN(ortherList, childModel.Id);//在剩余的项中寻找子节点
                list.Add(childModel);
            }
            return list;
        }
       
    }
}
