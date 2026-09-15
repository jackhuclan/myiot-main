using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class ProductCategoryDomainService : BaseDomainService<ProductCategory>, IProductCategoryDomainService
    {
        private readonly IProductCategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryDomainService(IUnitOfWork unitOfWork,
            IProductCategoryRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
