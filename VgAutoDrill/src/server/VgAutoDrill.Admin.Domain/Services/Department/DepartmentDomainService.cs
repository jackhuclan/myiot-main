using VgAutoDrill.Admin.Domain.Interfaces.Department;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Department;

namespace VgAutoDrill.Admin.Domain.Services.Department
{
    public class DepartmentDomainService : BaseDomainService<SysDepartment>, IDepartmentDomainService
    {
        private readonly IDepartmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentDomainService(IUnitOfWork unitOfWork,
            IDepartmentRepository repository)
        {
            this._repository = repository;
            base._baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
