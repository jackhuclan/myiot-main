using VgAutoDrill.Admin.Model.Entites.Mes;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IPartitionDomainService : IBaseDomainService<Partition>
    {
        Task<Partition> QueryWithSettingByID(object objId);

        Task<bool> AddOrUpdateWithSetting(Partition data);
    }
}
