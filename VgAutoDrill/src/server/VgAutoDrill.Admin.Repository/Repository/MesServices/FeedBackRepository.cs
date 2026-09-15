using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class FeedBackRepository : BaseRepository<FeedBack>, IFeedBackRepository
    {
        public FeedBackRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IPageList<FeedBack>> GetList(GetFeedBackListReq req)
        {
            var query = DBClient.Queryable<FeedBack, ItemType>
                ((f, i) => new object[]
                    {
                        JoinType.Left, f.ItemTypeId == i.Id
                    });

            query = query.Where((f, i) => f.IsDeleted == 0 && i.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.ProcessName))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.ProcessName) && f.ProcessName.Contains(req.ProcessName));
            }
            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.ProcessCode) && f.ProcessCode.Contains(req.ProcessCode));
            }
            if (!string.IsNullOrEmpty(req.ItemName))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.ItemName) && f.ItemName.Contains(req.ItemName));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.ItemCode) && f.ItemCode.Contains(req.ItemCode));
            }
            if (!string.IsNullOrEmpty(req.WorkOrderName))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.WorkOrderName) && f.WorkOrderName.Contains(req.WorkOrderName));
            }
            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.WorkOrderCode) && f.WorkOrderCode.Contains(req.WorkOrderCode));
            }
            if (!string.IsNullOrEmpty(req.WorkStationName))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.WorkStationName) && f.WorkStationName.Contains(req.WorkStationName));
            }
            if (!string.IsNullOrEmpty(req.WorkStationCode))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.WorkStationCode) && f.WorkStationCode.Contains(req.WorkStationCode));
            }
            if (!string.IsNullOrEmpty(req.FeedBackType))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.FeedBackType) && f.FeedBackType.Equals(req.FeedBackType));
            }
            if (!string.IsNullOrEmpty(req.FeedBackStatus))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.FeedBackStatus) && f.FeedBackStatus.ToUpper().Equals(req.FeedBackStatus.ToUpper()));
            }

            if (req.ItemTypeId > 0)
            {
                var sql = $"select id from t_item_type where find_in_set({req.ItemTypeId},ancestors)";
                var typeIdList = DBClient.SqlQueryable<ItemType>(sql).Select(d => d.Id).ToList();

                query = query.Where((f, i) => f.ItemTypeId == req.ItemTypeId
                            || (f.ItemTypeId.HasValue && typeIdList.Contains(f.ItemTypeId.Value)));
            }

            RefAsync<int> totalCount = 0;
            var data = await query.Select((f, i) => f).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((f, i) => f).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<FeedBack>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 获取生产任务信息
        /// 过滤掉草稿状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<IPageList<Model.Entites.Mes.WorkTask>> GetTaskList(GetTaskListReq req)
        {
            var query = DBClient.Queryable<Model.Entites.Mes.WorkTask>();
            query = query.Where(p => p.IsDeleted == 0 && p.TaskStatus != TaskStatusEnum.DRAFT);

            if (!string.IsNullOrEmpty(req.Code))
            {
                query = query.Where(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }
            if (!string.IsNullOrEmpty(req.Name))
            {
                query = query.Where(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }
            if (!string.IsNullOrEmpty(req.BatchCode))
            {
                query = query.Where(p => !string.IsNullOrEmpty(p.BatchCode) && p.BatchCode.Contains(req.BatchCode));
            }
            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                query = query.Where(p => !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.StartsWith(req.ProcessCode.Trim()));
            }
            if (req.RouteCodes != null && req.RouteCodes.Count > 0)
            {
                query = query.Where(p => !string.IsNullOrEmpty(p.RouteCode) && req.RouteCodes.Contains(p.RouteCode));
            }
            if (req.TaskStatusList != null && req.TaskStatusList.Count > 0)
            {
                query = query.Where(p => p.TaskStatus != null && req.TaskStatusList.Contains((TaskStatusEnum)p.TaskStatus));
            }
            if (!string.IsNullOrEmpty(req.WorkStationCode))
            {
                query = query.Where(p => !string.IsNullOrEmpty(p.WorkStationCode) && p.WorkStationCode.Contains(req.WorkStationCode));
            }
            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                query = query.Where(p => !string.IsNullOrEmpty(p.WorkOrderCode) && p.WorkOrderCode.Contains(req.WorkOrderCode));
            }
            if (req.WorkOrderId > 0)
            {
                query = query.Where(p => p.WorkOrderId == req.WorkOrderId);
            }
            if (req.ProcessId > 0)
            {
                query = query.Where(p => p.ProcessId == req.ProcessId);
            }

            if (req.RequestDate != null)
            {
                query = query.Where(p => p.RequestDate != null && p.RequestDate.Value.Date.Equals(req.RequestDate.Value.Date));
            }

            if (req.QueryStartTime != null)
            {
                query = query.Where(p => p.CreateTime >= req.QueryStartTime.Value);
            }
            if (req.QueryEndTime != null)
            {
                query = query.Where(p => p.CreateTime <= req.QueryEndTime.Value);
            }

            if (req.Status > -1)
            {
                query = query.Where(p => p.Status == req.Status);
            }

            if (req.QueryOrderBy == null)
            {
                query = query.OrderByDescending(p => p.Code);
            }
            else
            {
                switch (req.QueryOrderBy)
                {
                    case QueryOrderByEnum.OrderByCodeDesc:
                        query = query.OrderByDescending(p => p.Code);
                        break;

                    case QueryOrderByEnum.OrderByCodeASC:
                        query = query.OrderBy(p => p.Code);
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeDesc:
                        query = query.OrderByDescending(p => p.CreateTime);
                        query = query.OrderByDescending(p => p.Code);
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeASC:
                        query = query.OrderBy(p => p.CreateTime);
                        query = query.OrderBy(p => p.Code);
                        break;

                    default:
                        query = query.OrderByDescending(p => p.Code);
                        break;
                }
            }

            RefAsync<int> totalCount = 0;
            var data = await query.Select(p => p).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select(p => p).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<Model.Entites.Mes.WorkTask>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }
    }
}
