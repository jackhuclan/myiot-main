using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class WorkOrderRepository : BaseRepository<WorkOrder>, IProWorkOrderRepository
    {
        public WorkOrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IPageList<WorkOrder>> GetList(GetWorkOrderListReq req)
        {
            var query = DBClient.Queryable<WorkOrder, ItemType>
                ((p, i) => new object[]
                    {
                        JoinType.Left, p.ItemTypeId == i.Id
                    });

            query = query.Where((p, i) => p.IsDeleted == 0 && i.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }
            if (!string.IsNullOrEmpty(req.Name))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }

            if (!string.IsNullOrEmpty(req.ClientName))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.ClientName) && p.ClientName.Contains(req.ClientName));
            }
            if (!string.IsNullOrEmpty(req.ClientCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.ClientCode) && p.ClientCode.Contains(req.ClientCode));
            }

            if (!string.IsNullOrEmpty(req.OrderSource))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.OrderSource) && p.OrderSource.Contains(req.OrderSource));
            }

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }
            if (!string.IsNullOrEmpty(req.ItemName))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.ItemName) && p.ItemName.Contains(req.ItemName));
            }
            if (!string.IsNullOrEmpty(req.BatchCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.BatchCode) && p.BatchCode.Contains(req.BatchCode));
            }
            if (!string.IsNullOrEmpty(req.SourceCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.SourceCode) && p.SourceCode.Contains(req.SourceCode));
            }
            if (req.RequestDate != null)
            {
                query = query.Where((p, i) => p.RequestDate != null && p.RequestDate.Value.Date.Equals(req.RequestDate.Value.Date));
            }
            if (req.Status > -1)
            {
                query = query.Where((p, i) => p.Status == req.Status);
            }
            if (req.PanelCount > 0)
            {
                query = query.Where((p, i) => p.PanelCount == req.PanelCount);
            }
            if (req.DrillCount > 0)
            {
                query = query.Where((p, i) => p.DrillCount == req.DrillCount);
            }
            if (req.WadCount > 0)
            {
                query = query.Where((p, i) => p.WadCount == req.WadCount);
            }
            if (req.IsAddWorkOrder != null)
            {
                query = query.Where((p, i) => p.IsAddWorkOrder == req.IsAddWorkOrder);
            }
            if (req.LayerNumList != null && req.LayerNumList.Any())
            {
                query = query.Where((p, i) => req.LayerNumList.Contains((int)p.LayerNum));
            }
            if (req.IsExternal != null)
            {
                query = query.Where((p, i) => p.IsExternal == req.IsExternal);
            }

            if (req.ItemTypeId > 0)
            {
                var sql = $"select id from t_item_type where find_in_set({req.ItemTypeId},ancestors)";
                var typeIdList = DBClient.SqlQueryable<ItemType>(sql).Select(d => d.Id).ToList();

                query = query.Where((p, i) => p.ItemTypeId == req.ItemTypeId
                            || (p.ItemTypeId.HasValue && typeIdList.Contains(p.ItemTypeId.Value)));
            }

            if (req.QueryOrderBy == null)
            {
                query = query.OrderByDescending((p, i) => p.Code);
            }
            else
            {
                switch (req.QueryOrderBy)
                {
                    case QueryOrderByEnum.OrderByCodeDesc:
                        query = query.OrderByDescending((p, i) => p.Code);
                        break;

                    case QueryOrderByEnum.OrderByCodeASC:
                        query = query.OrderBy((p, i) => p.Code);
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeDesc:
                        query = query.OrderByDescending((p, i) => p.CreateTime);
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeASC:
                        query = query.OrderBy((p, i) => p.CreateTime);
                        break;

                    default:
                        query = query.OrderByDescending((p, i) => p.Code);
                        break;
                }
            }

            RefAsync<int> totalCount = 0;
            var data = await query.Select((p, i) => p).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((p, i) => p).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<WorkOrder>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

    }
}
