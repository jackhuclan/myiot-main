using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class ManualCallAgvTaskRepository : BaseRepository<ManualCallAgvTask>, IManualCallAgvTaskRepository
    {
        public ManualCallAgvTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
