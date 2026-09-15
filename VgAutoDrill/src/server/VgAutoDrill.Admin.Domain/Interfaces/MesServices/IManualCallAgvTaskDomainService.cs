using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask.Req;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IManualCallAgvTaskDomainService : IBaseDomainService<ManualCallAgvTask>
    {
        /// <summary>
        /// 根据钻机code查询手动呼叫agv任务信息
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        Task<List<ManualCallAgvTask>> QueryByDeviceCode(string deviceCode);

        /// <summary>
        /// 根据库位code查询任务信息
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        Task<ManualCallAgvTask> QueryByLocationCode(string locationCode);

        /// <summary>
        /// 根据条件查询task
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>

        Task<List<ManualCallAgvTask>> QueryTask(QueryTaskReq req);
    }
}
