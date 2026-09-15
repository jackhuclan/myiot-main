using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;



namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    /// <summary>
    /// 料仓板料追溯
    /// </summary>
    public class SiloPanelTraceRepository : BaseRepository<SiloPanelTrace>, ISiloPanelTraceRepository
    {
        public SiloPanelTraceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 获取料仓板料追溯列表
        /// </summary>
        /// <param name="request">查询请求参数</param>
        /// <returns>符合条件的追溯记录列表</returns>
        public async Task<IPageList<SiloPanelTrace>> GetList(GetSiloPanelTraceListReq request)
        {
            var query = DBClient.Queryable<SiloPanelTrace>();

            query = query.Where(x => x.IsDeleted == 0 && x.Status == 1);

            if (request.ID.HasValue)
            {
                query = query.Where(x => request.ID == x.Id);
            }
            if (!string.IsNullOrEmpty(request.SiloCode))
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.SiloCode) && x.SiloCode.Contains(request.SiloCode));
            }
            if (!string.IsNullOrEmpty(request.Location))
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.Location) && x.Location.Contains(request.Location));
            }
            if (!string.IsNullOrEmpty(request.UndrilledItem))
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.UndrilledItem) && x.UndrilledItem.Contains(request.UndrilledItem));
            }
            if (!string.IsNullOrEmpty(request.DrilledItem))
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.DrilledItem) && x.DrilledItem.Contains(request.DrilledItem));
            }
            if (request.HasMultipleDrilled.HasValue)
            {
                query = query.Where(x => x.HasMultipleDrilled == request.HasMultipleDrilled.Value);
            }
            if (!string.IsNullOrEmpty(request.Subject))
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.Subject) && x.Subject.Contains(request.Subject));
            }
            if (request.ScheduleId.HasValue && request.ScheduleId.Value > 0)
            {
                query = query.Where(x => x.ScheduleId == request.ScheduleId.Value);
            }
            if (request.TransportationTaskId.HasValue && request.TransportationTaskId.Value > 0)
            {
                query = query.Where(x => x.TransportationTaskId == request.TransportationTaskId.Value);
            }
            if (!string.IsNullOrEmpty(request.CreatorName))
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.CreatorName) && x.CreatorName.Contains(request.CreatorName));
            }
            if (request.IsWarning.HasValue)
            {
                query = query.Where(x => x.IsWarning == request.IsWarning.Value);
            }

            if (request.StartTime.HasValue)
            {
                query = query.Where(x => x.CreateTime >= request.StartTime.Value);
            }
            if (request.EndTime.HasValue)
            {
                query = query.Where(x => x.CreateTime <= request.EndTime.Value);
            }
            if (request.ModifyStartTime.HasValue)
            {
                query = query.Where(x => x.ModifyTime >= request.ModifyStartTime.Value);
            }
            if (request.ModifyEndTime.HasValue)
            {
                query = query.Where(x => x.ModifyTime <= request.ModifyEndTime.Value);
            }

            if (!request.QueryOrderBy.HasValue)
            {
                query = query.OrderByDescending(x => x.Id);
            }
            else
            {
                switch (request.QueryOrderBy.Value)
                {
                    case 1: // ID正序
                        query = query.OrderBy(x => x.Id);
                        break;

                    case 2: // ID倒序
                        query = query.OrderByDescending(x => x.Id);
                        break;

                    default:
                        query = query.OrderByDescending(x => x.Id);
                        break;
                }
            }

            RefAsync<int> totalCount = 0;
            var data = await query.ToPageListAsync(request.PageNum, request.PageSize, totalCount);

            int pageCount = request.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / request.PageSize) : 0;
            if (pageCount < request.PageNum && (data == null || data.Count == 0))
            {
                request.PageNum = pageCount;
                data = await query.ToPageListAsync(request.PageNum, request.PageSize, totalCount);
            }

            var list = new PageList<SiloPanelTrace>(data, request.PageNum, request.PageSize, totalCount);
            return list;
        }
    }
}
