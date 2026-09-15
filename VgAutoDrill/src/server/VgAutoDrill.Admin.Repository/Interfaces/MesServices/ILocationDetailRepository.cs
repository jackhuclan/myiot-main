using VgAutoDrill.Admin.Model.Entites.Mes;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface ILocationDetailRepository : IBaseRepository<LocationDetail>
    {
        Task<List<LocationDetail>> GetByCodeAsync(string code);
        Task<List<(LocationDetail LocationDetail, string SiloCode)>> GetByCodeWithSiloCodeAsync(string code);
    }
}