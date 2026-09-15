using SqlSugar;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceMain;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DeviceMaintainRepository : BaseRepository<DeviceMaintain>, IDeviceMaintainRepository
    {
        public DeviceMaintainRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<List<DeviceMaintainToExcelDto>> GetToExcelList()
        {
            var query = DBClient.Queryable<DeviceMaintain, DeviceMaintainDetail>
                ((dm, dmd) => new object[]
                    {
                        JoinType.Left, dm.Id == dmd.MasterId,
                    });

            query = query.Where((dm, dmd) => dm.IsDeleted == 0 && dmd.IsDeleted == 0);

            var list = await query.Select((dm, dmd) => new DeviceMaintainToExcelDto
            {
                SubJectCode = dmd.SubjectCode,
                DeviceCode = dm.DeviceCode,
                MaintainResult = dmd.MaintainResult,
                BetterSteps = dmd.BetterSteps,
                MaintainPerson = dm.MaintainPerson,
                MaintainStatus = dm.MaintainStatus,
                MaintainTime = dm.MaintainTime,
                Remark = dm.Remark,
            }).ToListAsync();

            return list;
        }

    }
}
