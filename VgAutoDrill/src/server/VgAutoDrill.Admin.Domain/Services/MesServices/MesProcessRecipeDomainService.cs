using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class MesProcessRecipeDomainService : BaseDomainService<MesProcessRecipe>, IMesProcessRecipeDomainService
    {
        private readonly IMesProcessRecipeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MesProcessRecipeDomainService(IUnitOfWork unitOfWork,
            IMesProcessRecipeRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
