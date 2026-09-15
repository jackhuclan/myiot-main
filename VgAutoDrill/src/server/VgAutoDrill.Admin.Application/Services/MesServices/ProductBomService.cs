using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProductBom;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 产品结构
    /// </summary>
    public class ProductBomService : BaseServiceWithTree<ProductBom, ProductBomTreeDto, ProductBomDto, AddOrUpdateProductBomReq>, IProductBomService
    {
        private readonly IProductBomDomainService _serviceProvider;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public ProductBomService(IProductBomDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
            _serviceProvider = domainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ProductBomDto>>> GetList(GetProductBomListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _serviceProvider.GetList(req);
            return Success(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<List<ProductBomTreeDto>>> GetTreeList()
        {
            var list = await _domainService.QueryAsync(q => q.IsDeleted == 0 && q.Status == 1, q => q.Id, SqlSugar.OrderByType.Asc);
            var result = new ResponseDto<List<ProductBomTreeDto>>();
            if (list == null || !list.Any())
            {
                return result;
            }

            var allCodes = AddChildN(list, 0);

            result.Data = allCodes;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<ProductBomDto>> MultiQueryByID(long id)
        {
            var result = await _serviceProvider.MultiQueryByID(id);
            if (result == null)
            {
                return Fail<ProductBomDto>("信息不存在!");
            }
            return Success(result);
        }
    }
}
