using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 工艺路线与产品大类
    /// </summary>
    public class RouteAndProductCategoryService : BaseServiceWithoutTree<RouteAndProductCategory, RouteAndProductCategoryDto, AddRouteAndProductCategoryReq>, IRouteAndProductCategoryService
    {
        private readonly IRouteAndProductCategoryDomainService _rdomainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public RouteAndProductCategoryService(IRouteAndProductCategoryDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
            _rdomainService = domainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<RouteAndProductCategoryDto>>> GetList(GetRouteAndProductCategoryListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _rdomainService.GetList(req);
            return Success(result);
        }

        /// <summary>
        /// 根据产品大类获取工艺路线数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<RouteInfoByProductCategoryDto>>> GetRouteInfoList(GetRouteAndProductCategoryListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _rdomainService.GetRouteInfoList(req);
            return Success(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<RouteAndProductCategoryDto>> MultiQueryByID(long Id)
        {
            var result = await _rdomainService.MultiQueryByID(Id);
            if (result == null)
            {
                return Fail<RouteAndProductCategoryDto>("信息不存在!");
            }
            return Success(result);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddRouteAndProductCategoryReq req)
        {
            if (req.FromRoute)
            {
                //前端需要传入等长的字符串（产品大类、工艺路线、优先级）
                if (req == null || req.ProductCategoryIds.Count != req.RouteIds.Count)
                {
                    return Fail("信息格式错误!");
                }

                List<RouteAndProductCategory> addList = new List<RouteAndProductCategory>();
                for (var i = 0; i < req.ProductCategoryIds.Count; i++)
                {
                    var routeId = req.RouteIds[i];
                    var productCategoryId = req.ProductCategoryIds[i];

                    long orderNum = 0;
                    //查找此大类关联的工艺路线集合中的最大优先级
                    var routes = await _domainService.QueryAsync(p => p.ProductCategoryId == productCategoryId, p => p.OrderNum, SqlSugar.OrderByType.Desc);
                    if (routes.FirstOrDefault() != null)
                    {
                        orderNum = Math.Max(orderNum, routes.FirstOrDefault().OrderNum ?? 0) + 1;
                    }

                    //删除原有关联关系
                    await _domainService.DeleteAsync(p => p.RouteId == routeId);

                    var addExsit = addList.Exists(p => p.RouteId == routeId && p.ProductCategoryId == productCategoryId);
                    if (addExsit)
                    {
                        continue;
                    }

                    var model = new RouteAndProductCategory();
                    model.RouteId = routeId;
                    model.ProductCategoryId = productCategoryId;
                    model.OrderNum = orderNum;
                    model.CreateTime = DateTime.Now;
                    model.CreatorId = UserId;
                    model.Status = (int)DataStatusEnum.Enable;

                    addList.Add(model);
                }

                await _domainService.BulkInsert(addList);

                return Success();
            }
            else
            {
                //前端需要传入等长的字符串（产品大类、工艺路线、优先级）
                if (req == null || req.ProductCategoryIds.Count != req.RouteIds.Count || req.ProductCategoryIds.Count != req.OrderNums.Count)
                {
                    return Fail("信息格式错误!");
                }

                List<RouteAndProductCategory> addList = new List<RouteAndProductCategory>();
                for (var i = 0; i < req.ProductCategoryIds.Count; i++)
                {
                    var routeId = req.RouteIds[i];
                    var productCategoryId = req.ProductCategoryIds[i];
                    var orderNum = req.OrderNums[i];

                    //删除原有关联关系
                    await _domainService.DeleteAsync(p => p.ProductCategoryId == productCategoryId);

                    var addExsit = addList.Exists(p => p.RouteId == routeId && p.ProductCategoryId == productCategoryId);
                    if (addExsit)
                    {
                        continue;
                    }

                    var model = new RouteAndProductCategory();
                    model.RouteId = routeId;
                    model.ProductCategoryId = productCategoryId;
                    model.OrderNum = orderNum;
                    model.CreateTime = DateTime.Now;
                    model.CreatorId = UserId;
                    model.Status = (int)DataStatusEnum.Enable;

                    addList.Add(model);
                }

                await _domainService.BulkInsert(addList);

                return Success();
            }
        }

        /// <summary>
        /// 修改信息,
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(UpdateRouteAndProductCategoryReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var isExist = await _domainService.IsExistAsync(p => p.RouteId == req.RouteId
            && p.ProductCategoryId == req.ProductCategoryId && p.Id != req.Id);
            if (isExist)
            {
                return Fail("关联关系已存在!");
            }

            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            //当只有在产品大类中维护优先级时
            var isThereSameOrderNum = await _domainService.IsExistAsync(p => p.ProductCategoryId == entity.ProductCategoryId
            && p.OrderNum == req.OrderNum && p.Id != req.Id);
            if (isThereSameOrderNum)
            {
                return Fail($"工艺路线优先级设置重复，{req.OrderNum}!");
            }

            RouteAndProductCategory model = new RouteAndProductCategory();
            model.RouteId = req.RouteId == null ? entity.RouteId : req.RouteId;
            model.ProductCategoryId = req.ProductCategoryId == null ? entity.ProductCategoryId : req.ProductCategoryId;
            model.Id = entity.Id;
            model.Status = req.Status;
            model.OrderNum = req.OrderNum;
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }

        /// <summary>
        /// 上移/下移 修改优先级
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateDataOrderNum(UpdateDataOrderNumReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }
            if (req.OldDataID == null || req.OldDataID == 0 || req.ReplaceDataID == null || req.ReplaceDataID == 0)
            {
                return Fail("未传递有效数据!");
            }

            var oldModel = await _domainService.QueryByID(req.OldDataID);
            if (oldModel == null)
            {
                return Fail(req.OldDataID + " 信息不存在!");
            }

            var replaceModel = await _domainService.QueryByID(req.ReplaceDataID);
            if (replaceModel == null)
            {
                return Fail(req.ReplaceDataID + " 信息不存在!");
            }

            if (oldModel.ProductCategoryId != replaceModel.ProductCategoryId)
            {
                return Fail("需要修改的优先级，不属于同一个产品大类！");
            }

            long oldOrderNum = 0;
            long replaceOrderNum = 0;
            if (oldModel.OrderNum == null || replaceModel.OrderNum == null)
            {
                long orderNum = 0;
                //查找此大类关联的工艺路线集合中的最大优先级
                var routes = await _domainService.QueryAsync(p => p.ProductCategoryId == oldModel.ProductCategoryId, p => p.OrderNum, SqlSugar.OrderByType.Desc);
                if (routes.FirstOrDefault() != null)
                {
                    orderNum = Math.Max(orderNum, routes.FirstOrDefault().OrderNum ?? 0) + 1;
                }

                if (oldModel.OrderNum == null && replaceModel.OrderNum == null)
                {
                    oldOrderNum = orderNum;
                    replaceOrderNum = orderNum + 1;
                }
                else if (oldModel.OrderNum == null && replaceModel.OrderNum != null)
                {
                    oldOrderNum = orderNum;
                }
                else if (oldModel.OrderNum != null && replaceModel.OrderNum == null)
                {
                    replaceOrderNum = orderNum;
                }
            }
            else
            {
                oldOrderNum = (long)oldModel.OrderNum;
                replaceOrderNum = (long)replaceModel.OrderNum;
            }

            List<RouteAndProductCategory> updateList = new List<RouteAndProductCategory>();
            oldModel.OrderNum = replaceOrderNum;
            oldModel.ModifierId = UserId;
            oldModel.ModifyTime = DateTime.Now;

            replaceModel.OrderNum = oldOrderNum;
            replaceModel.ModifyTime = DateTime.Now;
            replaceModel.ModifierId = UserId;

            updateList.Add(oldModel);
            updateList.Add(replaceModel);

            var result = await _domainService.BulkUpdate(updateList);

            if (!result)
            {
                return Fail("修改优先级失败！");
            }
            return Success();
        }
    }
}
