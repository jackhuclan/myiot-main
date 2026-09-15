using System.Collections.Generic;
using System.Threading.Tasks;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    /// <summary>
    /// 料仓板料追溯详细
    /// </summary>
    public class SiloPanelTraceDetailRepository : BaseRepository<SiloPanelTraceDetail>, ISiloPanelTraceDetailRepository
    {
        public SiloPanelTraceDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


    }
}
