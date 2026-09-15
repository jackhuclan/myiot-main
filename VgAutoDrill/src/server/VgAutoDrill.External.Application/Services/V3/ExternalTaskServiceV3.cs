using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Application.Services;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask;
using VgAutoDrill.External.Application.Interfaces.V3;

namespace VgAutoDrill.External.Application.Services.V3
{
    public class ExternalTaskServiceV3 : BaseService, IExternalTaskServiceV3
    {
        private readonly IExternalTaskService _externalTaskService;
        public ExternalTaskServiceV3(IExternalTaskService externalTaskService)
        {
            _externalTaskService = externalTaskService;
        }

        /// <summary>
        /// 根据目标任务调整任务顺序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> TaskMoveByTargetTask(TaskMoveByTargetTaskReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _externalTaskService.TaskMoveByTargetTask(req);
            return result;
        }

        /// <summary>
        /// 根据DeviceAndDate调整任务顺序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> TaskMoveByDeviceAndDate(TaskMoveByDeviceAndDateReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _externalTaskService.TaskMoveByDeviceAndDate(req);
            return result;
        }
    }
}
