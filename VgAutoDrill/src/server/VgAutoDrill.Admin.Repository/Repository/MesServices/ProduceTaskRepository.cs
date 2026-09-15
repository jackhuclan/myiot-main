using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class ProduceTaskRepository : BaseRepository<ProduceTask>, IProduceTaskRepository
    {
        public ProduceTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
