using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DeviceMaintainDetailRepository : BaseRepository<DeviceMaintainDetail>, IDeviceMaintainDetailRepository
    {
        public DeviceMaintainDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}

