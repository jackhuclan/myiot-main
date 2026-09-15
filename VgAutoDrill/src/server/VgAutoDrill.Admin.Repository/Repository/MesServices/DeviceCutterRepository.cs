using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DeviceCutterRepository : BaseRepository<Cutter>, IDeviceCutterRepository
    {
        public DeviceCutterRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
