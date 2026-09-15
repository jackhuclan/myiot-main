using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemDrillFile;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 钻带参数
    /// </summary>
    public class ItemDrillFileService : BaseServiceWithoutTree<ItemDrillFile, ItemDrillFileDto, AddOrUpdateItemDrillFileReq>, IItemDrillFileService
    {
        private readonly IItemDrillFileDomainService _itemDrillFileService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public ItemDrillFileService(IItemDrillFileDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
            _itemDrillFileService = domainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ItemDrillFileDto>>> GetList(GetItemDrillFileListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<ItemDrillFileDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<ItemDrillFile>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.DrillFileName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.DrillFileName) && p.DrillFileName.Contains(req.DrillFileName));
            }

            if (!string.IsNullOrEmpty(req.ItemName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemName) && p.ItemName.Contains(req.ItemName));
            }

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }

            if (req.ItemId > 0)
            {
                where = where.And(p => p.ItemId == req.ItemId);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<ItemDrillFile>, List<ItemDrillFileDto>>(result.ToList());
            return Success<PageDto<ItemDrillFileDto>>(pageDto);
        }

        /// <summary>
        /// 根据ItemTypeId获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ItemDrillFileDto>>> GetEquipmentList(GetItemDrillFileListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _itemDrillFileService.GetList(req);
            return Success(result);
        }
    }
}
