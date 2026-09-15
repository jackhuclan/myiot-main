using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class NotifySettingRepository : BaseRepository<NotifySetting>, INotifySettingRepository
    {
        public NotifySettingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
