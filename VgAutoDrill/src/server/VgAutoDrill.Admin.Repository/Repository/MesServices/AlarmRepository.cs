using SqlSugar;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class AlarmRepository : BaseRepository<Alarm>, IAlarmRepository
    {
        public AlarmRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<List<AlarmToExcelDto>> GetToExcelList(GetAlarmListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 100) req.PageSize = 1000;

            var query = DBClient.Queryable<Alarm, Device>
                ((a, d) => new object[]
                    {
                        JoinType.Left, a.DeviceId == d.Id
                    });

            query = query.Where((a, d) => a.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.AlarmCode))
            {
                query = query.Where((a, d) => !string.IsNullOrEmpty(a.AlarmCode) && a.AlarmCode.Contains(req.AlarmCode));
            }
            if (!string.IsNullOrEmpty(req.AlarmName))
            {
                query = query.Where((a, d) => !string.IsNullOrEmpty(a.AlarmName) && a.AlarmName.Contains(req.AlarmName));
            }
            if (req.Status > -1)
            {
                query = query.Where((a, d) => a.Status == req.Status);
            }
            if (req.AlarmLevel > -1)
            {
                query = query.Where((a, d) => a.AlarmLevel == req.AlarmLevel);
            }

            if (req.IsHandled.HasValue)
            {
                query = query.Where((a, d) => a.IsHandled == req.IsHandled);
            }

            if (req.AlarmKinds != null && req.AlarmKinds.Any())
            {
                query = query.Where((a, d) => a.AlarmKind != null && req.AlarmKinds.Contains((AlarmKind)a.AlarmKind));
            }

            if (req.QueryStartTime != null)
            {
                query = query.Where((a, d) => a.AlarmTime >= req.QueryStartTime.Value);
            }
            if (req.QueryEndTime != null)
            {
                query = query.Where((a, d) => a.AlarmTime <= req.QueryEndTime.Value);
            }

            if (!string.IsNullOrEmpty(req.LocationCode))
            {
                query = query.Where((a, d) => !string.IsNullOrEmpty(a.LocationCode) && a.LocationCode.ToLower().Contains(req.LocationCode.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.PartitionCode))
            {
                query = query.Where((a, d) => !string.IsNullOrEmpty(a.PartitionCode) && a.PartitionCode.ToLower().Contains(req.PartitionCode.ToLower()));
            }

            query = query.OrderByDescending(a => a.AlarmLevel);

            RefAsync<int> totalCount = 0;

            var list = await query.Select((a, d) => new AlarmToExcelDto
            {
                AlarmCode = a.AlarmCode,
                AlarmName = a.AlarmName,
                AlarmLevel = a.AlarmLevel,
                AlarmTime = a.AlarmTime,
                DeviceCode = d.Code,
                DeviceName = d.Name,
                EventData = a.EventData,
                EventId = a.EventId,
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            return list;
        }
    }
}
