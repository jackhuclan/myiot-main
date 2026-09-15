using VgAutoDrill.Admin.Model.Entites.Mes;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IPartitionRepository : IBaseRepository<Partition>
    {
        Task<Partition> QueryWithSettingByID(object objId);

        Task<bool> AddOrUpdateWithSetting(Partition data);
    }
}
