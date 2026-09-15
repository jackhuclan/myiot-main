using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Application.Services;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace;
using SqlSugar;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 料仓板料追溯详细
    /// </summary>
    public class SiloPanelTraceDetailService : BaseServiceWithoutTree<SiloPanelTraceDetail, SiloPanelTraceDetailDto, AddOrUpdateSiloPanelTraceDetailReq>, ISiloPanelTraceDetailService
    {
        private readonly ISiloPanelTraceDetailDomainService _siloPanelTraceDetailDomainService;
        private readonly ILogger<SiloPanelTraceDetailService> _logger;

        public SiloPanelTraceDetailService(
            ISiloPanelTraceDetailDomainService siloPanelTraceDetailDomainService,
            ILogger<SiloPanelTraceDetailService> logger,
            IMapper mapper)
            : base(siloPanelTraceDetailDomainService, mapper)
        {
            _siloPanelTraceDetailDomainService = siloPanelTraceDetailDomainService;
            _logger = logger;
        }

        /// <summary>
        /// 获取料仓板料追溯详细记录列表
        /// </summary>
        /// <param name="request">查询请求参数</param>
        /// <returns>详细记录列表</returns>
        public async Task<ResponseDto<PageDto<SiloPanelTraceDetailDto>>> GetList(GetSiloPanelTraceDetailListReq request)
        {
            if (request.PageNum < 1) request.PageNum = 1;
            if (request.PageSize < 1) request.PageSize = 10;
            var pageDto = new PageDto<SiloPanelTraceDetailDto>(request.PageNum, request.PageSize);

            var where = PredicateBuilder.True<SiloPanelTraceDetail>();
            where = where.And(p => p.IsDeleted == 0);

            if (request.MasterId.HasValue && request.MasterId > 0)
            {
                where = where.And(p => p.MasterId == request.MasterId);
            }

            if (!string.IsNullOrEmpty(request.LocationCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.LocationCode) && p.LocationCode.Contains(request.LocationCode));
            }

            if (!string.IsNullOrEmpty(request.SiloCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.Contains(request.SiloCode));
            }

            if (!string.IsNullOrEmpty(request.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(request.ItemCode));
            }

            if (!string.IsNullOrEmpty(request.PanelCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.PanelCode) && p.PanelCode.Contains(request.PanelCode));
            }

            if (request.ProductStatus.HasValue)
            {
                where = where.And(p => p.ProductStatus == request.ProductStatus);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.FloorNum, OrderByType.Asc, request.PageNum, request.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<SiloPanelTraceDetailDto>>(result.ToList());
            return Success(pageDto);
        }



        /// <summary>
        /// 新增料仓板料追溯详细记录
        /// </summary>
        /// <param name="request">新增请求参数</param>
        /// <returns>新增结果</returns>
        public override async Task<ResponseDto<string>> Add(AddOrUpdateSiloPanelTraceDetailReq request)
        {
            if (request.MasterId <= 0)
            {
                return Fail("主表ID不能为空");
            }

            request.CreatorName = UserName;
            return await base.Add(request);
        }

        /// <summary>
        /// 更新料仓板料追溯详细记录
        /// </summary>
        /// <param name="request">更新请求参数</param>
        /// <returns>更新结果</returns>
        public override async Task<ResponseDto<string>> Update(AddOrUpdateSiloPanelTraceDetailReq request)
        {
            if (request == null || request.Id <= 0)
            {
                return Fail("更新请求参数无效");
            }

            if (request.MasterId <= 0)
            {
                return Fail("主表ID不能为空");
            }

            request.ModifierName = UserName;
            return await base.Update(request);
        }
    }
}