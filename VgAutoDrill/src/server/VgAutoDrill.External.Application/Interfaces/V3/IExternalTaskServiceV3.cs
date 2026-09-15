using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask;

namespace VgAutoDrill.External.Application.Interfaces.V3
{
    public interface IExternalTaskServiceV3
    {
        /// <summary>
        /// 根据目标任务调整任务顺序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> TaskMoveByTargetTask(TaskMoveByTargetTaskReq req);

        /// <summary>
        /// 根据DeviceAndDate调整任务顺序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> TaskMoveByDeviceAndDate(TaskMoveByDeviceAndDateReq req);

    }
}
