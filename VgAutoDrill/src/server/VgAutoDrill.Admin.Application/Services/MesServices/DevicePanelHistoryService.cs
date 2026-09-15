using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanelHistory;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 设备负载板料历史表
    /// </summary>
    public class DevicePanelHistoryService : BaseServiceWithoutTree<DevicePanelHistory, DevicePanelHistoryDto, AddOrUpdateDevicePanelHistoryReq>, IDevicePanelHistoryService
    {
        private readonly IDevicePanelHistoryDomainService _hisDomainService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public DevicePanelHistoryService(IDevicePanelHistoryDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
            _hisDomainService = domainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DevicePanelHistoryDto>>> GetList(GetDevicePanelHistoryListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DevicePanelHistoryDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<DevicePanelHistory>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.BatchCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.BatchCode) && p.BatchCode.Contains(req.BatchCode));
            }
            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.DeviceCode) && p.DeviceCode.Contains(req.DeviceCode));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }
            if (!string.IsNullOrEmpty(req.LotId))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.LotId) && p.LotId.Contains(req.LotId));
            }
            if (!string.IsNullOrEmpty(req.PanelCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.PanelCode) && p.PanelCode.Contains(req.PanelCode));
            }
            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.Contains(req.SiloCode));
            }

            if (req.ProductStatus > 0)
            {
                where = where.And(p => p.ProductStatus == req.ProductStatus);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<DevicePanelHistory>, List<DevicePanelHistoryDto>>(result.ToList());
            return Success(pageDto);
        }

        /// <summary>
        /// 仅获取PanelCode存在的数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DevicePanelHistoryDto>>> GetListByPanel(GetDevicePanelHistoryListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _hisDomainService.PageList(req);
            return Success(result);
        }

    }
}
