using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class RouteAndProductCategoryRepository : BaseRepository<RouteAndProductCategory>, IRouteAndProductCategoryRepository
    {
        public RouteAndProductCategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<PageList<RouteAndProductCategoryDto>> GetList(GetRouteAndProductCategoryListReq req)
        {
            var query = DBClient.Queryable<RouteAndProductCategory, ProductCategory>
                ((r, p) => new object[]
                    {
                        JoinType.Left, r.ProductCategoryId == p.Id
                    });

            query = query.Where((r, p) => r.IsDeleted == 0 && p.IsDeleted == 0);

            if (req.RouteId > 0)
            {
                query = query.Where((r, p) => r.RouteId == req.RouteId);
            }

            if (req.ProductCategoryId > 0)
            {
                query = query.Where((r, p) => r.ProductCategoryId == req.ProductCategoryId);
            }

            query = query.OrderBy((r, p) => p.Code);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((r, p) => new RouteAndProductCategoryDto
            {
                Id = r.Id,
                RouteId = r.RouteId,
                ProductCategoryId = r.ProductCategoryId,
                ProductCategoryCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                ProductCategoryName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                Status = r.Status,
                CreateTime = r.CreateTime,
                CreatorId = r.CreatorId,
                ModifierId = r.ModifierId,
                ModifyTime = r.ModifyTime,
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((r, p) => new RouteAndProductCategoryDto
                {
                    Id = r.Id,
                    RouteId = r.RouteId,
                    ProductCategoryId = r.ProductCategoryId,
                    ProductCategoryCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                    ProductCategoryName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                    Status = r.Status,
                    CreateTime = r.CreateTime,
                    CreatorId = r.CreatorId,
                    ModifierId = r.ModifierId,
                    ModifyTime = r.ModifyTime,
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<RouteAndProductCategoryDto>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 根据产品大类获取工艺路线数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PageList<RouteInfoByProductCategoryDto>> GetRouteInfoList(GetRouteAndProductCategoryListReq req)
        {
            var query = DBClient.Queryable<RouteAndProductCategory, Route>
                ((r, p) => new object[]
                    {
                        JoinType.Left, r.RouteId == p.Id
                    });

            query = query.Where((r, p) => r.IsDeleted == 0 && p.IsDeleted == 0);

            if (req.RouteId > 0)
            {
                query = query.Where((r, p) => r.RouteId == req.RouteId);
            }

            if (req.ProductCategoryId > 0)
            {
                query = query.Where((r, p) => r.ProductCategoryId == req.ProductCategoryId);
            }

            query = query.OrderByDescending((r, p) => r.OrderNum);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((r, p) => new RouteInfoByProductCategoryDto
            {
                Id = r.Id,
                RouteId = r.RouteId,
                ProductCategoryId = r.ProductCategoryId,
                RouteCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                RouteName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                RouteDesc = string.IsNullOrEmpty(p.RouteDesc) ? "" : p.RouteDesc,
                RouteRemark = string.IsNullOrEmpty(p.Remark) ? "" : p.Remark,
                OrderNum = r.OrderNum,
                Status = r.Status,
                CreateTime = r.CreateTime,
                CreatorId = r.CreatorId,
                ModifierId = r.ModifierId,
                ModifyTime = r.ModifyTime,
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((r, p) => new RouteInfoByProductCategoryDto
                {
                    Id = r.Id,
                    RouteId = r.RouteId,
                    ProductCategoryId = r.ProductCategoryId,
                    RouteCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                    RouteName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                    RouteDesc = string.IsNullOrEmpty(p.RouteDesc) ? "" : p.RouteDesc,
                    RouteRemark = string.IsNullOrEmpty(p.Remark) ? "" : p.Remark,
                    OrderNum = r.OrderNum,
                    Status = r.Status,
                    CreateTime = r.CreateTime,
                    CreatorId = r.CreatorId,
                    ModifierId = r.ModifierId,
                    ModifyTime = r.ModifyTime,
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<RouteInfoByProductCategoryDto>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<RouteAndProductCategoryDto> MultiQueryByID(long Id)
        {
            var query = DBClient.Queryable<RouteAndProductCategory, ProductCategory>
                ((r, p) => new object[]
                    {
                        JoinType.Left, r.ProductCategoryId == p.Id,
                    });

            query = query.Where((r, p) => r.IsDeleted == 0 && p.IsDeleted == 0);

            query.Where((r, p) => r.Id == Id);

            var data = await query.Select((r, p) => new RouteAndProductCategoryDto
            {
                Id = r.Id,
                RouteId = r.RouteId,
                ProductCategoryId = r.ProductCategoryId,
                ProductCategoryCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                ProductCategoryName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                Status = r.Status,
                CreateTime = r.CreateTime,
                CreatorId = r.CreatorId,
                ModifierId = r.ModifierId,
                ModifyTime = r.ModifyTime,
            }).ToListAsync();

            if (data != null && data.Count > 0)
            {
                return data[0];
            }
            else
            {
                return null;
            }
        }
    }
}
