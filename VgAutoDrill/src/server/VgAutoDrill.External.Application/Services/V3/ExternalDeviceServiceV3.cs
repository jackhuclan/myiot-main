using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Application.Services;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.External.Application.Interfaces.ExternalService;

namespace VgAutoDrill.External.Application.Services.ExternalService
{
    public class ExternalDeviceServiceV3 : BaseService, IExternalDeviceServiceV3
    {
        private readonly IExternalTaskService _externalTaskService;
        private readonly IDeviceService _deviceService;

        /// <summary>
        /// 
        /// </summary>
        public ExternalDeviceServiceV3(
            IExternalTaskService externalTaskService,
            IDeviceService deviceService)
        {
            _externalTaskService = externalTaskService;
            _deviceService = deviceService;
        }

        /// <summary>
        /// 获取钻机信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<ExternalTaskDto>>> GetDrillDeviceList(GetDeviceReq req)
        {
            if (req == null)
            {
                return Fail<List<ExternalTaskDto>>("信息格式错误!");
            }

            var result = await _externalTaskService.GetDeviceList(req);
            return result;
        }

        public async Task<ResponseDto<List<TaskDto>>> GetTaskList(GetTaskReq req)
        {
            if (req == null)
            {
                return Fail<List<TaskDto>>("信息格式错误!");
            }
            if (string.IsNullOrEmpty(req.DeviceCode))
            {
                return Fail<List<TaskDto>>("未填写DeviceCode!");
            }

            var result = await _externalTaskService.GetTaskList(req);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="delData"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteData(List<long> delData)
        {
            if (delData == null || delData.Count == 0)
            {
                return Fail("未识别删除数据！");
            }
            var result = await _deviceService.DeleteDataList(delData);
            if (result == null)
            {
                return Fail("删除失败！");
            }
            return result;
        }
    }
}
