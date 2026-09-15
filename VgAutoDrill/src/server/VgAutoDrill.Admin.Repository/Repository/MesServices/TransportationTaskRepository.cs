using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class TransportationTaskRepository : BaseRepository<TransferJob>, ITransportationTaskRepository
    {
        public TransportationTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
