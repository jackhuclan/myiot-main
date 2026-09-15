using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class CutterGroupRepository : BaseRepository<Model.Entites.Mes.CutterGroup>, ICutterGroupRepository
    {
        public CutterGroupRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }




    }
}
