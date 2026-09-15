using SqlSugar;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class RouteRepository : BaseRepository<Route>, IRouteRepository
    {
        public RouteRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> CheckKeyProcess(long Id)
        {
            var query = DBClient.Queryable<Route, RouteAndProcess, Process>
                ((rt, r, p) => new object[]
                    {
                        JoinType.Inner, r.RouteId == rt.Id,
                        JoinType.Left, r.ProcessId == p.Id,
                    });

            query = query.Where((rt, r, p) => rt.IsDeleted == 0 && r.IsDeleted == 0 && p.IsDeleted == 0);

            query.Where((rt, r, p) => rt.Id == Id);

            var data = await query.Select((rt, r, p) => new RouteAndProcessDto
            {
                Id = r.Id,
                RouteId = r.RouteId,
                ProcessId = r.ProcessId,
                ProcessCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                ProcessName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                OrderNum = r.OrderNum,
                Color = r.Color,
                KeyFlag = r.KeyFlag,
                RequiredTime = r.RequiredTime,
                IsManualCheck = r.IsManualCheck,
                SelfCheckNum = r.SelfCheckNum,
                Status = r.Status
            }).ToListAsync();

            if (data != null && data.Count > 0)
            {
                var exsitKeyData = data.FindAll(p => !string.IsNullOrEmpty(p.KeyFlag) && p.KeyFlag.Equals("1"));
                if (exsitKeyData == null || exsitKeyData.Count == 0)
                {
                    var unExsitKeyData = data.Exists(p => !string.IsNullOrEmpty(p.KeyFlag) && p.KeyFlag.Equals("0"));

                    return !unExsitKeyData;
                }
                else
                {
                    if (exsitKeyData.Count == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 校验是否存在已提交的任务与该工艺路线关联
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> ExsitTask(long Id)
        {
            var query = DBClient.Queryable<WorkOrder, Model.Entites.Mes.WorkTask>
                ((w, t) => new object[]
                    {
                        JoinType.Left, w.Id == t.WorkOrderId,
                    });

            query = query.Where((w, t) => w.IsDeleted == 0 && t.IsDeleted == 0);

            query = query.Where((w, t) => w.RouteId == Id);

            query = query.Where((w, t) => t.TaskStatus != null && t.TaskStatus == TaskStatusEnum.COMMITED);

            var data = await query.Select((w, t) => new Model.Entites.Mes.WorkTask { }).ToListAsync();

            if (data != null && data.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
