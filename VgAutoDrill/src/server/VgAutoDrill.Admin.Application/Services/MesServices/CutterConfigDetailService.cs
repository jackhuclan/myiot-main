using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterConfigDetail;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class CutterConfigDetailService : BaseServiceWithoutTree<CutterConfigDetail, CutterConfigDetailDto, AddOrUpdateCutterConfigDetailReq>, ICutterConfigDetailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public CutterConfigDetailService(ICutterConfigDetailDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<CutterConfigDetailDto>>> GetList(GetCutterConfigDetailListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<CutterConfigDetailDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<CutterConfigDetail>();
            where = where.And(p => p.IsDeleted == 0);
            if (req.MasterId > 0)
            {
                where = where.And(p => p.MasterId == req.MasterId);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<CutterConfigDetail>, List<CutterConfigDetailDto>>(result.ToList());
            return Success(pageDto);
        }
    }
}
