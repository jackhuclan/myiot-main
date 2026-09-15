using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProductBom;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class ProductBomRepository : BaseRepository<ProductBom>, IProductBomRepository
    {
        public ProductBomRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<PageList<ProductBomDto>> GetList(GetProductBomListReq req)
        {
            var query = DBClient.Queryable<ProductBom, Item, Item>
                ((p, i, it) => new object[]
                    {
                        JoinType.Left, p.ItemId == i.Id,
                        JoinType.Left,p.ParentId == it.Id,
                    });

            query = query.Where((p, i, it) => p.IsDeleted == 0 && i.IsDeleted == 0 && it.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                query = query.Where((p, i, it) => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (!string.IsNullOrEmpty(req.Name))
            {
                query = query.Where((p, i, it) => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((p, i, it) => !string.IsNullOrEmpty(i.Code) && i.Code.Contains(req.ItemCode));
            }

            if (!string.IsNullOrEmpty(req.ParentName))
            {
                query = query.Where((p, i, it) => !string.IsNullOrEmpty(it.Name) && it.Name.Contains(req.ParentName));
            }

            if (!string.IsNullOrEmpty(req.ParentCode))
            {
                query = query.Where((p, i, it) => !string.IsNullOrEmpty(it.Code) && it.Code.Contains(req.ParentCode));
            }

            if (req.ItemId > 0)
            {
                query = query.Where((p, i, it) => p.ItemId == req.ItemId);
            }

            if (req.Quantity != null)
            {
                query = query.Where((p, i, it) => p.Quantity == req.Quantity);
            }

            query = query.OrderBy((p, i, it) => p.OrderNum);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((p, i, it) => new ProductBomDto
            {
                Id = p.Id,
                Quantity = p.Quantity,
                ItemId = p.ItemId,
                ItemCode = string.IsNullOrEmpty(i.Code) ? "" : i.Code,
                ItemName = string.IsNullOrEmpty(i.Name) ? "" : i.Name,
                ParentCode = string.IsNullOrEmpty(it.Code) ? "" : it.Code,
                ParentName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                ParentId = p.ParentId,
                OrderNum = p.OrderNum,
                Status = p.Status
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((p, i, it) => new ProductBomDto
                {
                    Id = p.Id,
                    Quantity = p.Quantity,
                    ItemId = p.ItemId,
                    ItemCode = string.IsNullOrEmpty(i.Code) ? "" : i.Code,
                    ItemName = string.IsNullOrEmpty(i.Name) ? "" : i.Name,
                    ParentCode = string.IsNullOrEmpty(it.Code) ? "" : it.Code,
                    ParentName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                    ParentId = p.ParentId,
                    OrderNum = p.OrderNum,
                    Status = p.Status
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<ProductBomDto>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ProductBomDto> MultiQueryByID(long Id)
        {
            var query = DBClient.Queryable<ProductBom, Item, Item>
                ((p, i, it) => new object[]
                    {
                        JoinType.Left, p.ItemId == i.Id,
                        JoinType.Left,p.ParentId == it.Id,
                    });

            query = query.Where((p, i, it) => p.IsDeleted == 0 && i.IsDeleted == 0 && it.IsDeleted == 0);

            query.Where((p, i, it) => p.Id == Id);

            var data = await query.Select((p, i, it) => new ProductBomDto
            {
                Id = p.Id,
                Quantity = p.Quantity,
                ItemId = p.ItemId,
                ItemCode = string.IsNullOrEmpty(i.Code) ? "" : i.Code,
                ItemName = string.IsNullOrEmpty(i.Name) ? "" : i.Name,
                ParentCode = string.IsNullOrEmpty(it.Code) ? "" : it.Code,
                ParentName = string.IsNullOrEmpty(it.Name) ? "" : it.Name,
                ParentId = p.ParentId,
                OrderNum = p.OrderNum,
                Status = p.Status
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
