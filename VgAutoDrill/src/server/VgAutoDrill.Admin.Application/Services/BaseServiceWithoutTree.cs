using AutoMapper;
using VgAutoDrill.Admin.Domain.Interfaces;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// 不具有树结构功能的基础服务类
    /// </summary>
    public abstract class BaseServiceWithoutTree<TEntity, TDto, TAddOrUpdateReq> : BaseService
        where TEntity : BaseEntity
        where TAddOrUpdateReq : IDtoOnlyId
    {
        protected readonly IBaseDomainService<TEntity> _domainService;
        protected readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        protected BaseServiceWithoutTree(IBaseDomainService<TEntity> domainService, IMapper mapper)
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
                return Fail<TDto>("信息已标记为删除!");
            }
            var model = _mapper.Map<TDto>(entity);

            return Success(model);
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
        /// 新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public virtual async Task<ResponseDto<string>> Add(TAddOrUpdateReq req)
        {
            var model = _mapper.Map<TEntity>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            await _domainService.Add(model);
            return Success();
        }

        ///// <summary>
        ///// 新增并且返回新增对象
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        //public virtual async Task<ResponseDto<TDto>> AddWithReturn(TAddOrUpdateReq req)
        //{
        //    var model = _mapper.Map<TEntity>(req);
        //    model.CreateTime = DateTime.Now;
        //    model.CreatorId = UserId;
        //    model.Status = (int)DataStatusEnum.Enable;
        //    var id = await _domainService.AddReturnId(model);
        //    var entity = await _domainService.FindSingleAsync(x => x.Id == id);
        //    var dto = _mapper.Map<TDto>(entity);
        //    return Success(dto);
        //}

        /// <summary>
        /// 新增并且返回新增对象的ID
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public virtual async Task<ResponseDto<int>> AddAndReturnId(TAddOrUpdateReq req)
        {
            var model = _mapper.Map<TEntity>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            var id = await _domainService.AddReturnId(model);
            return Success(id);
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResponseDto<string>> Update(TAddOrUpdateReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }
            var model = _mapper.Map<TEntity>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;

            //// 对于Rack实体，需要特殊处理点坐标字段，避免被清空
            //if (typeof(TEntity) == typeof(VgAutoDrill.Admin.Model.Entites.Mes.Rack))
            //{
            //    var rackEntity = entity as VgAutoDrill.Admin.Model.Entites.Mes.Rack;
            //    var rackModel = model as VgAutoDrill.Admin.Model.Entites.Mes.Rack;
            //    if (rackEntity != null && rackModel != null)
            //    {
            //        // 如果新值为空，保留原有值
            //        if (string.IsNullOrEmpty(rackModel.InnerPoint))
            //        {
            //            rackModel.InnerPoint = rackEntity.InnerPoint;
            //        }
            //        if (string.IsNullOrEmpty(rackModel.OutPoint))
            //        {
            //            rackModel.OutPoint = rackEntity.OutPoint;
            //        }
            //        if (string.IsNullOrEmpty(rackModel.TransInnerPoint))
            //        {
            //            rackModel.TransInnerPoint = rackEntity.TransInnerPoint;
            //        }
            //        if (string.IsNullOrEmpty(rackModel.TransOutPoint))
            //        {
            //            rackModel.TransOutPoint = rackEntity.TransOutPoint;
            //        }
            //    }
            //}
            await _domainService.Update(model);
            return Success();
        }
    }
}
