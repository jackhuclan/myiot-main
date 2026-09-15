using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemDrillFileDetail;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 钻带参数文件明细
    /// </summary>
    public class ItemDrillFileDetailService : BaseServiceWithoutTree<ItemDrillFileDetail, ItemDrillFileDetailDto, AddOrUpdateItemDrillFileDetailReq>, IItemDrillFileDetailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public ItemDrillFileDetailService(IItemDrillFileDetailDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {

        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ItemDrillFileDetailDto>>> GetList(GetItemDrillFileDetailListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<ItemDrillFileDetailDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<ItemDrillFileDetail>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (req.Diameter != null)
            {
                where = where.And(p => p.Diameter == req.Diameter);
            }

            if (req.ItemDrillFileId > 0)
            {
                where = where.And(p => p.ItemDrillFileId == req.ItemDrillFileId);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<ItemDrillFileDetail>, List<ItemDrillFileDetailDto>>(result.ToList());
            return Success<PageDto<ItemDrillFileDetailDto>>(pageDto);
        }
    }
}