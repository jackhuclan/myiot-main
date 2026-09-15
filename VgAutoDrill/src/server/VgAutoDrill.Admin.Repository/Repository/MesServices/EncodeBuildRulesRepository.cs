using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class EncodeBuildRulesRepository : BaseRepository<EncodeBuildRules>, IEncodeBuildRulesRepository
    {
        public EncodeBuildRulesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
