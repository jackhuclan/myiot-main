using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class MesProcessRecipeRepository : BaseRepository<MesProcessRecipe>, IMesProcessRecipeRepository
    {
        public MesProcessRecipeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
