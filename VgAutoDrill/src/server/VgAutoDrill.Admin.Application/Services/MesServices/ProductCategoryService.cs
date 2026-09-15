using AutoMapper;
using SqlSugar;
using System.Text;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProductCategory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductCategoryService : BaseServiceWithTree<ProductCategory, ProductCategoryTreeDto, ProductCategoryDto, AddOrUpdateProductCategoryReq>, IProductCategoryService
    {
        private readonly IRouteAndProductCategoryDomainService _routeAndProductCategoryDomainService;
        private readonly IItemDomainService _itemDomainService;
        private readonly ISysConfigManager _sysConfigManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="routeAndProductCategoryDomainService"></param>
        /// <param name="itemDomainService"></param>
        /// <param name="mapper"></param>
        public ProductCategoryService(IProductCategoryDomainService domainService,
            IRouteAndProductCategoryDomainService routeAndProductCategoryDomainService,
            IItemDomainService itemDomainService,
            ISysConfigManager sysConfigManager,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _routeAndProductCategoryDomainService = routeAndProductCategoryDomainService;
            _itemDomainService = itemDomainService;
            _sysConfigManager = sysConfigManager;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ProductCategoryDto>>> GetList(GetProductCategoryListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<ProductCategoryDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<ProductCategory>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (req.VettingStatus != null)
            {
                where = where.And(p => p.VettingStatus == req.VettingStatus);
            }

            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(result.ToList());
            return Success<PageDto<ProductCategoryDto>>(pageDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<List<ProductCategoryTreeDto>>> GetTreeList()
        {
            var list = await _domainService.QueryAsync(q => q.IsDeleted == 0 && q.Status == 1, q => q.Id, SqlSugar.OrderByType.Asc);
            var result = new ResponseDto<List<ProductCategoryTreeDto>>();
            if (list == null || !list.Any())
            {
                return result;
            }

            var allCodes = AddChildN(list, 0);

            result.Data = allCodes;

            return result;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BulkInsert(List<ProductCategoryToExcelDto> list)
        {
            if (list == null || list.Count == 0)
            {
                return Fail("未识别有效的数据！");
            }

            bool importStatus = await _sysConfigManager.GetBoolValue(MESConfigConstants.IMPORT_STATUS);

            int failCount = 0;
            StringBuilder sb = new StringBuilder();
            List<ProductCategory> ProductCategorys = new List<ProductCategory>();
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.Code) || string.IsNullOrEmpty(item.Name))
                {
                    sb.Append("编码：" + item.Code + " 或名称：" + item.Name + " 为空；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                var isExsitCode = await _domainService.IsExistAsync(p => p.Code == item.Code);
                if (isExsitCode)
                {
                    sb.Append("编码" + item.Code + " 数据库已存在；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                if (ProductCategorys.Exists(p => p.Code == item.Code))
                {
                    sb.Append("编码" + item.Code + " 导入列表中已存在；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                ProductCategory model = new ProductCategory();
                model.Code = item.Code;
                model.Name = item.Name;
                model.ParentId = item.ParentID;
                model.Remark = item.Remark;
                model.DispenseMachines = item.DispenseMachines;
                if (importStatus)
                {
                    model.Status = 1;
                }
                else
                {
                    model.Status = 0;
                }
                model.CreatorId = UserId;
                model.CreateTime = DateTime.Now;
                ProductCategorys.Add(model);
            }
            var result = await _domainService.BulkInsert(ProductCategorys);
            if (!result)
            {
                return Fail("导入失败！");
            }

            string str = string.Format("预计导入：{0} 条；成功导入：{1} 条；失败：{2} 条；\r\n", list.Count, list.Count - failCount, failCount);
            str = str + sb.ToString();

            return Success(str);
        }

        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateProductCategoryReq req)
        {
            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == req.Code && p.Id != req.Id);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<ProductCategory>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.VettingStatus = entity.VettingStatus;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;

            await _domainService.Update(model);
            return Success();
        }

        public override Task<ResponseDto<string>> Add(AddOrUpdateProductCategoryReq req)
        {
            return base.Add(req);
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteData(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            if (entity.VettingStatus == 1)
            {
                return Fail("已审批，不允许删除!");
            }

            var result = await _domainService.DeleteById(id);
            if (result)
            {
                //同步删除关联关系表

                return Success("");
            }
            return Fail("删除失败");
        }

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteDataList(List<long> idList)
        {
            if (idList != null)
            {
                object[] deleteList = new object[idList.Count];
                for (int i = 0; i < idList.Count; i++)
                {
                    var entity = await _domainService.QueryByID(idList[i]);
                    if (entity == null)
                    {
                        continue;
                    }

                    if (entity.VettingStatus == 1)
                    {
                        return Fail(idList[i] + " 已审批，不允许删除!");
                    }

                    deleteList[i] = idList[i];
                }

                var result = await _domainService.DeleteByIds(deleteList);
                if (result)
                {
                    return Success("");
                }
            }
            return Fail("删除失败");

        }

        /// <summary>
        /// 审批产品大类
        /// 校验产品大类是否匹配工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> VettingProductCategory(VettingProductCategoryReq req)
        {
            if (req == null || req.VettingProductCategorys == null || req.VettingProductCategorys.Count == 0)
            {
                return Fail("信息格式错误!");
            }

            var productCategoryList = await _domainService.QueryAsync(p => p.VettingStatus == null || p.VettingStatus == 0, p => p.Id, OrderByType.Asc);
            if (productCategoryList == null || productCategoryList.Count == 0)
            {
                return Fail("未查询到未审批的产品大类");
            }

            var vettingProductCategorys = productCategoryList.FindAll(p => req.VettingProductCategorys.Any(c => c.ProductCategoryId == p.Id));
            if (vettingProductCategorys == null || vettingProductCategorys.Count == 0)
            {
                return Fail("未查询到未审批的产品大类");
            }

            List<ProductCategory> updateList = new List<ProductCategory>();
            foreach (var item in vettingProductCategorys)
            {
                var exsitRAndP = await _routeAndProductCategoryDomainService.IsExistAsync(p => p.ProductCategoryId == item.Id);
                if (!exsitRAndP)
                {
                    return Fail(item.Code + " 产品大类未匹配工艺路线");
                }

                item.VettingStatus = 1;
                item.ModifierId = UserId;
                item.ModifyTime = DateTime.Now;
                updateList.Add(item);
            }

            var result = await _domainService.BulkUpdate(updateList);
            if (!result)
            {
                return Fail("审批 产品大类 失败！");
            }

            return Success();
        }

        /// <summary>
        /// 取消审批产品大类
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> CancelVettingProductCategory(VettingProductCategoryReq req)
        {
            if (req == null || req.VettingProductCategorys == null || req.VettingProductCategorys.Count == 0)
            {
                return Fail("信息格式错误!");
            }

            var productCategoryList = await _domainService.QueryAsync(p => p.VettingStatus == 1, p => p.Id, OrderByType.Asc);
            if (productCategoryList == null || productCategoryList.Count == 0)
            {
                return Fail("未查询到取消审批的产品大类");
            }

            var vettingProductCategorys = productCategoryList.FindAll(p => req.VettingProductCategorys.Any(c => c.ProductCategoryId == p.Id));
            if (vettingProductCategorys == null || vettingProductCategorys.Count == 0)
            {
                return Fail("未查询到取消审批的产品大类");
            }

            List<ProductCategory> updateList = new List<ProductCategory>();
            foreach (var item in vettingProductCategorys)
            {
                var isExsitItem = await _itemDomainService.IsExistAsync(p => p.ProductCategoryId == item.Id);
                if (isExsitItem)
                {
                    return Fail("存在引用的物料产品，不允许取消审批!");
                }

                item.VettingStatus = 0;
                item.ModifierId = UserId;
                item.ModifyTime = DateTime.Now;
                updateList.Add(item);
            }

            var result = await _domainService.BulkUpdate(updateList);
            if (!result)
            {
                return Fail("取消审批 产品大类 失败！");
            }

            return Success();
        }
    }
}
