using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.AppSecret;

namespace VgAutoDrill.Admin.Repository.Repository.AppSecret
{
    public class AppSecretRepository : BaseRepository<SysAppSecret>, IAppSecretRepository
    {
        public AppSecretRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
