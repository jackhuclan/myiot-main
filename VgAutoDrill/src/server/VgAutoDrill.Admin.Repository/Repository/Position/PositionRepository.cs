using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Position;

namespace VgAutoDrill.Admin.Repository.Repository.Position
{
    public class PositionRepository : BaseRepository<SysPosition>, IPositionRepository
    {
        public PositionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
