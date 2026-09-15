using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class DeviceTypeRepository : BaseRepository<DeviceType>, IDeviceTypeRepository
    {
        public DeviceTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
