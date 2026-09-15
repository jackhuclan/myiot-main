using System.Collections.Generic;
using System.Threading.Tasks;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    /// <summary>
    /// 料仓板料追溯
    /// </summary>
    public interface ISiloPanelTraceRepository : IBaseRepository<SiloPanelTrace>
    {
        /// <summary>
        /// 获取料仓板料追溯列表
        /// </summary>
        /// <param name="request">查询请求参数</param>
        /// <returns>符合条件的追溯记录列表</returns>
        Task<IPageList<SiloPanelTrace>> GetList(GetSiloPanelTraceListReq request);
    }
}
