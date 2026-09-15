using VgAutoDrill.Admin.Model.Entites.Mes;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface ILocationDetailDomainService : IBaseDomainService<LocationDetail>
    {
        Task<List<LocationDetail>> GetByCodeAsync(string code);
        Task<List<(LocationDetail LocationDetail, string SiloCode)>> GetByCodeWithSiloCodeAsync(string code);
    }
}
