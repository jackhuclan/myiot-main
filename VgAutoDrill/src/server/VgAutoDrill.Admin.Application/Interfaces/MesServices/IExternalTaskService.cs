
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IExternalTaskService
    {

        /// <summary>
        /// 获取钻机信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<ExternalTaskDto>>> GetDeviceList(GetDeviceReq req);

        /// <summary>
        /// 查看钻机Task列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<TaskDto>>> GetTaskList(GetTaskReq req);

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
