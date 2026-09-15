using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockStorageHistory;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 库存入库单历史表
    /// </summary>
    public class MaterialStockStorageHistoryService : BaseServiceWithoutTree<MaterialStockStorageHistory, MaterialStockStorageHistoryDto, AddOrUpdateMaterialStockStorageHistoryReq>, IMaterialStockStorageHistoryService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public MaterialStockStorageHistoryService(IMaterialStockStorageHistoryDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {

        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<MaterialStockStorageHistoryDto>>> GetList(GetMaterialStockStorageHistoryListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<MaterialStockStorageHistoryDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<MaterialStockStorageHistory>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.BatchCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.BatchCode) && p.BatchCode.Contains(req.BatchCode));
            }

            if (!string.IsNullOrEmpty(req.MaterialStockCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.MaterialStockCode) && p.MaterialStockCode.Contains(req.MaterialStockCode));
            }

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }

            if (!string.IsNullOrEmpty(req.ItemName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemName) && p.ItemName.Contains(req.ItemName));
            }

            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.Contains(req.ProcessCode));
            }

            if (!string.IsNullOrEmpty(req.ProcessName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ProcessName) && p.ProcessName.Contains(req.ProcessName));
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<MaterialStockStorageHistory>, List<MaterialStockStorageHistoryDto>>(result.ToList());
            return Success<PageDto<MaterialStockStorageHistoryDto>>(pageDto);
        }

    }
}
