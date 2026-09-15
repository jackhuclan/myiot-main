using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Mes.Interface;

public interface IRestPointAdapter
{
    Task<List<RestPoint>> GetRestPoints();
}