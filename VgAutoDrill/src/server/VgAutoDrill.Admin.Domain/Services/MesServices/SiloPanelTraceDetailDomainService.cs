using System.Collections.Generic;
using System.Threading.Tasks;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    /// <summary>
    /// 料仓板料追溯详细
    /// </summary>
    public class SiloPanelTraceDetailDomainService : BaseDomainService<SiloPanelTraceDetail>, ISiloPanelTraceDetailDomainService
    {
        private readonly ISiloPanelTraceDetailRepository _repository;

        public SiloPanelTraceDetailDomainService(ISiloPanelTraceDetailRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
        }


    }
}
