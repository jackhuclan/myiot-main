using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class PartitionSettingRepository : BaseRepository<PartitionSetting>, IPartitionSettingRepository
    {
        public PartitionSettingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}
