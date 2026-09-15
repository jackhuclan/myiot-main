using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.Position;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.Position;
using VgAutoDrill.Admin.Model.ViewModels.Res.Position;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class PositionService : BaseService, IPositionService
    {
        private readonly IPositionDomainService _domainService;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public PositionService(IPositionDomainService domainService,
            IMapper mapper)
        {
            _domainService = domainService;
            _mapper = mapper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> Add(AddOrUpdatePositionReq req)
        {
            var model = _mapper.Map<SysPosition>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;

            var result = await _domainService.Add(model);
            if (result)
                return Success("");

            return Fail("添加失败");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> Update(AddOrUpdatePositionReq req)
        {
            var model = _mapper.Map<SysPosition>(req);
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
                return Success("");

            return Fail("修改失败");
        }

        public async Task<ResponseDto<string>> Delete(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }
            var result = await _domainService.DeleteById(id);
            if (result)
                return Success("");

            return Fail("删除失败");
        }

        public async Task<ResponseDto<PositionDto>> QueryByID(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail<PositionDto>("信息不存在!");
            }
            if (entity.IsDeleted == 1)
            {
                return Fail<PositionDto>("信息已经软删除!");
            }
            var model = _mapper.Map<PositionDto>(entity);
            return Success<PositionDto>(model);
        }
        public async Task<ResponseDto<int>> GetMaxSort()
        {
            var result = await _domainService.GetMaxSort<SysPosition>();
            return Success(result);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>

        public async Task<ResponseDto<PageDto<PositionDto>>> GetList(PositionListReq req)
        {
            var pageDto = new PageDto<PositionDto>(req.PageNum, req.PageSize);
            var where = PredicateBuilder.True<SysPosition>();
            if (!string.IsNullOrEmpty(req.PositionName))
            {
                where = where.And(p => p.PositionName.Contains(req.PositionName));
            }
            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }
            var result = await _domainService.QueryPageAsync(where, p => p.Sort, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<SysPosition>, List<PositionDto>>(result.ToList());
            return Success<PageDto<PositionDto>>(pageDto);
        }

        /// <summary>
        /// 批量删除信息
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public virtual async Task<ResponseDto<string>> DeleteList(List<long> idList)
        {
            if (idList == null || idList.Count == 0)
            {
                return Fail("信息不存在!");
            }

            int notFound = 0;
            foreach (var idDelete in idList)
            {
                var entity = await _domainService.QueryByID(idDelete);
                if (entity == null)
                {
                    notFound++;
                    continue;
                }
                var result = await _domainService.DeleteById(idDelete);
                if (!result)
                {
                    return Fail("删除失败");
                }
            }

            if (notFound == idList.Count)
            {
                return Fail("信息不存在!");
            }

            return Success("删除成功");
        }
    }
}