using Mapster;
using SqlSugar;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class ItemDomainService : BaseDomainService<Item>, IItemDomainService
    {
        private readonly IMdItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitMeasureRepository _unitMeasureRepository;

        public ItemDomainService(IUnitOfWork unitOfWork,
            IUnitMeasureRepository unitMeasureRepository,
            IMdItemRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
            _unitMeasureRepository = unitMeasureRepository;
        }

        public async Task<PageDto<ItemFullPropertiesTreeDto>> PageFullTreeList(GetItemListReq req)
        {
            var pageDto = new PageDto<ItemFullPropertiesTreeDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req, true);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<ItemFullPropertiesTreeDto>>();
            return pageDto;
        }
        /// <summary>
        /// 获取产品代码列表,ItemOrProduct==2
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<List<ItemDto>> GetProductCodes(QueryItemCodeRequest req)
        {
            var dbClient = _unitOfWork.GetDbClient();
            var query = dbClient.Queryable<Item>();
            query = query.Where(p => p.IsDeleted == 0 && p.Status == 1 && p.ItemOrProduct == 2);

            if (!string.IsNullOrEmpty(req.Code))
            {
                query = query.Where(p => p.Code != null && p.Code.Contains(req.Code));
            }

            switch (req.QueryOrderBy)
            {
                case QueryOrderByEnum.OrderByCodeDesc:
                    query = query.OrderByDescending(item => item.Code);
                    break;
                case QueryOrderByEnum.OrderByCodeASC:
                    query = query.OrderBy(item => item.Code);
                    break;

                case QueryOrderByEnum.OrderByCreateTimeDesc:
                    query = query.OrderByDescending(item => item.CreateTime);
                    break;

                case QueryOrderByEnum.OrderByCreateTimeASC:
                    query = query.OrderBy(item => item.CreateTime);
                    break;

                default:
                    query = query.OrderByDescending(item => item.Code);
                    break;
            }

            var result = query.Select(item => item).ToList().Adapt<List<ItemDto>>();
            return result;
        }
        public async Task<PageDto<ItemDto>> PageList(GetItemListReq req)
        {
            var pageDto = new PageDto<ItemDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<ItemDto>>();

            return pageDto;
        }

        /// <summary>
        /// 获取物料信息（包含创建人和修改人姓名）
        /// </summary>
        /// <param name="id">物料ID</param>
        /// <returns>物料信息</returns>
        public async Task<Item> GetItemWithUserInfo(long id)
        {
            return await _repository.GetItemWithUserInfo(id);
        }
    }
}
