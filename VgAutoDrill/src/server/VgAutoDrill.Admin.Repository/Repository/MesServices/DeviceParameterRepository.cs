using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class DeviceParameterRepository : BaseRepository<DeviceParameter>, IDeviceParameterRepository
    {
        public DeviceParameterRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
