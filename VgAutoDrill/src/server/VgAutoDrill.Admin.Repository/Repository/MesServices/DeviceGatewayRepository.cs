using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DeviceGatewayRepository : BaseRepository<DeviceGateway>, IDeviceGatewayRepository
    {
        public DeviceGatewayRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
