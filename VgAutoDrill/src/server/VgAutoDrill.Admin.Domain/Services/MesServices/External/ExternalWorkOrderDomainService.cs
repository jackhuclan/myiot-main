using VgAutoDrill.Admin.Domain.Interfaces.MesServices.External;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices.External;

namespace VgAutoDrill.Admin.Domain.Services.MesServices.External
{
    public class ExternalWorkOrderDomainService : BaseDomainService<ExternalWorkOrder>, IExternalWorkOrderDomainService
    {
        private readonly IExternalWorkOrderRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ExternalWorkOrderDomainService(IUnitOfWork unitOfWork,
            IExternalWorkOrderRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
