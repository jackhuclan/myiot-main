using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class ProduceTaskHistoryRepository : BaseRepository<ProduceTaskHistory>, IProduceTaskHistoryRepository
    {
        public ProduceTaskHistoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}