using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class SysConfigRepository : BaseRepository<SysConfig>, ISysConfigRepository
    {
        public SysConfigRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
