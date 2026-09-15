using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask.Req;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IManualCallAgvTaskService
    {
        /// <summary>
        /// 根据钻机code查询手动呼叫agv任务信息
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        Task<ResponseDto<List<ManualCallAgvTaskDto>>> QueryByDeviceCode(string deviceCode);

        /// <summary>
        /// 更新手动呼叫agv任务信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<bool>> UpdateManualCallAgvTask(AddOrUpdateManualCallAgvTaskReq req);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Add(AddOrUpdateManualCallAgvTaskReq req);

        /// <summary>
        /// 根据库位code查询任务信息
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        Task<ManualCallAgvTaskDto> GetAgvTaskByLocationCode(string locationCode);

        /// <summary>
        /// 根据库位code查询任务信息
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        Task<ManualCallAgvTask> QueryByLocationCode(string locationCode);

        /// <summary>
        /// 更新任务信息
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        Task<bool> Update(ManualCallAgvTask task);
        /// <summary>
        /// 获取刀盒码
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        Task<ResponseDto<List<string>>> GetAptBoxBarcode(string groupNo, string deviceCode);

        Task<ResponseDto<string>> ConfirmAptBoxBarcode(AptBoxReq aptBoxReq);
        
    }
}
