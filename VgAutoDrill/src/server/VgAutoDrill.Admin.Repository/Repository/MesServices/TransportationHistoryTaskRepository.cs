using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class TransportationHistoryTaskRepository : BaseRepository<TransportationHistoryTask>, ITransportationHistoryTaskRepository
    {
        public TransportationHistoryTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }    
}
