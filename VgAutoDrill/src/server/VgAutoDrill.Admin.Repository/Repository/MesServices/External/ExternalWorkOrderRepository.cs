using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices.External;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices.External
{
    public class ExternalWorkOrderRepository : BaseRepository<ExternalWorkOrder>, IExternalWorkOrderRepository
    {
        public ExternalWorkOrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
