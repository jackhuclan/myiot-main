using System.Collections.Generic;
using System.Threading.Tasks;
using VgAutoDrill.Admin.Domain.Interfaces;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    /// <summary>
    /// 料仓板料追溯
    /// </summary>
    public interface ISiloPanelTraceDomainService : IBaseDomainService<SiloPanelTrace>
    {
        /// <summary>
        /// 获取料仓板料追溯记录
        /// </summary>
        /// <param name="request">查询请求参数</param>
        /// <returns>符合条件的追溯记录列表</returns>
        Task<PageDto<SiloPanelTraceDto>> GetList(GetSiloPanelTraceListReq request);
    }
}
