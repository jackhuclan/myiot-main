using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.res;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IManualCallAgvLogService
    {


        Task<ResponseDto<PageDto<ManualCallAgvLogDto>>> List(ListReq req);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<bool>> AgvOperate(AgvOperateReq req);

        /// <summary>
        /// 获取agv运行状态
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        Task<ResponseDto<GetAgvRunStatusResponse>> GetAgvRunStatus(string locationCode);


        /// <summary>
        /// 根据钻机code获取任务列表
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<TaskDto>>> TaskList(GetDrillOrAgvDeviceInfoReq req);


        /// <summary>
        /// 验证钻带文件
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        Task<ResponseDto<bool>> LoadingDrillFile(LoadingDrillfileReq req);


        /// <summary>
        /// 加载ATP文件
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        Task<ResponseDto<bool>> LoadingATPFile(LoadingAtpFileReq req);

        /// <summary>
        /// 更新任务状态(开始和结束)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<bool>> UpdateTaskStatus(UpdateTaskByOutSideReq req);
    }
}
