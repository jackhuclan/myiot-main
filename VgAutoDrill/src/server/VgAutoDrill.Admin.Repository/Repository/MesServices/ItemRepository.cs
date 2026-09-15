using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Admin.Model.Entites;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    /// <summary>
    /// Item扩展方法
    /// </summary>
    public static class ItemExtensions
    {
        /// <summary>
        /// 复制Item对象的所有属性
        /// </summary>
        public static Item CopyFrom(this Item target, Item source)
        {
            target.Id = source.Id;
            target.Code = source.Code;
            target.Name = source.Name;
            target.PanelLength = source.PanelLength;
            target.IncodeNumber = source.IncodeNumber;
            target.ItemOrProduct = source.ItemOrProduct;
            target.Specification = source.Specification;
            target.UnitOfMeasure = source.UnitOfMeasure;
            target.ItemTypeId = source.ItemTypeId;
            target.ProductCategoryId = source.ProductCategoryId;
            target.ProductCategoryCode = source.ProductCategoryCode;
            target.ProductCategoryName = source.ProductCategoryName;
            target.WarehouseId = source.WarehouseId;
            target.WarehouseCode = source.WarehouseCode;
            target.WarehouseName = source.WarehouseName;
            target.DispenseMachines = source.DispenseMachines;
            target.DrillFilePath = source.DrillFilePath;
            target.PanelWidth = source.PanelWidth;
            target.LayerNum = source.LayerNum;
            target.BeforeDrillFilePath = source.BeforeDrillFilePath;
            target.AfterDrillFilePath = source.AfterDrillFilePath;
            target.SpecGroup = source.SpecGroup;
            target.ParentId = source.ParentId;
            target.Ancestors = source.Ancestors;
            target.Status = source.Status;
            target.IsDeleted = source.IsDeleted;
            target.CreateTime = source.CreateTime;
            target.CreatorId = source.CreatorId;
            target.ModifyTime = source.ModifyTime;
            target.ModifierId = source.ModifierId;
            target.PanelCount = source.PanelCount;
            return target;
        }
    }

    public class ItemRepository : BaseRepository<Item>, IMdItemRepository
    {
        public ItemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IPageList<Item>> GetList(GetItemListReq req, bool isTree = false)
        {
            var query = DBClient.Queryable<Item, ItemType>
                ((item, itemType) => new object[]
                    {
                        JoinType.Left, item.ItemTypeId == itemType.Id
                    });

            query = query.Where((item, itemType) => item.IsDeleted == 0);

            if (isTree)
            {
                query = query.Where((item, itemType) => item.ParentId == 0);
            }

            if (!string.IsNullOrEmpty(req.Name))
            {
                query = query.Where((item, itemType) => !string.IsNullOrEmpty(item.Name) && item.Name.Contains(req.Name));
            }
            if (!string.IsNullOrEmpty(req.Code))
            {
                query = query.Where((item, itemType) => !string.IsNullOrEmpty(item.Code) && item.Code.Contains(req.Code));
            }
            if (req.ItemOrProduct != null)
            {
                query = query.Where((item, itemType) => item.ItemOrProduct == req.ItemOrProduct);
            }
            if (req.Status > -1)
            {
                query = query.Where((item, itemType) => item.Status == req.Status);
            }
            if (req.ItemTypeId > 0)
            {
                var sql = $"select id from t_item_type where find_in_set({req.ItemTypeId},ancestors)";
                var typeIdList = DBClient.SqlQueryable<ItemType>(sql).Select(d => d.Id).ToList();

                query = query.Where((item, itemType) => itemType.Id == req.ItemTypeId
                            || (item.ItemTypeId.HasValue && typeIdList.Contains(item.ItemTypeId.Value))
                            );
            }

            if (req.QueryOrderBy == null)
            {
                query = query.OrderByDescending((item, itemType) => item.Code);
            }
            else
            {
                switch (req.QueryOrderBy)
                {
                    case QueryOrderByEnum.OrderByCodeDesc:
                        query = query.OrderByDescending((item, itemType) => item.Code);
                        break;

                    case QueryOrderByEnum.OrderByCodeASC:
                        query = query.OrderBy((item, itemType) => item.Code);
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeDesc:
                        query = query.OrderByDescending((item, itemType) => item.CreateTime);
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeASC:
                        query = query.OrderBy((item, itemType) => item.CreateTime);
                        break;

                    default:
                        query = query.OrderBy((item, itemType) => item.Code);
                        break;
                }
            }

            RefAsync<int> totalCount = 0;
            var data = await query.Select((item, itemType) => item).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((item, itemType) => item).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<Item>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 获取物料信息（包含创建人和修改人姓名）
        /// </summary>
        /// <param name="id">物料ID</param>
        /// <returns>物料信息</returns>
        public async Task<Item> GetItemWithUserInfo(long id)
        {
            var query = DBClient.Queryable<Item, ItemType, SysUser, SysUser>
                ((item, itemType, creator, modifier) => new object[]
                    {
                        JoinType.Left, item.ItemTypeId == itemType.Id,
                        JoinType.Left, item.CreatorId == creator.Id,
                        JoinType.Left, item.ModifierId == modifier.Id
                    });

            query = query.Where((item, itemType, creator, modifier) => item.Id == id && item.IsDeleted == 0);

            // 先获取物料基本信息和用户姓名
            var itemResult = await query.Select((item, itemType, creator, modifier) => new
            {
                Item = item,
                CreatorName = creator.UserName,
                ModifierName = modifier.UserName
            }).FirstAsync();
            
            if (itemResult != null)
            {
                // 使用扩展方法复制所有属性
                var result = new Item().CopyFrom(itemResult.Item);
                
                // 设置用户姓名字段
                result.CreatorName = itemResult.CreatorName;
                result.ModifierName = itemResult.ModifierName;
                
                return result;
            }
            
            return null;
        }
    }
}
