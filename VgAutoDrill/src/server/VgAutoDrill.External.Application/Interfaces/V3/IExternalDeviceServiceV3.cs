
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;

namespace VgAutoDrill.External.Application.Interfaces.ExternalService
{
    public interface IExternalDeviceServiceV3
    {
        /// <summary>
        /// 查询钻机信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<ExternalTaskDto>>> GetDrillDeviceList(GetDeviceReq req);

        /// <summary>
        /// 获取钻机任务信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<TaskDto>>> GetTaskList(GetTaskReq req);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="delData"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteData(List<long> delData);
    }
}
