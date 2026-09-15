using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceServiceInvocation;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    public class DeviceServiceInvocationController : BaseController
    {
        public readonly IDeviceServiceInvocationService _deviceServiceInvocationService;
        public DeviceServiceInvocationController(IDeviceServiceInvocationService deviceServiceInvocationService)
        {
            _deviceServiceInvocationService = deviceServiceInvocationService;
        }

        /// <summary>
        /// 新增/更新 钻机服务调用
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddDeviceServiceInvocationList")]
        [ProducesResponseType(typeof(ResponseDto<List<string>>), 200)]
        [PermissionAuthorize("produce:DeviceServiceInvocation:list")]
        public async Task<ActionResult> AddDeviceServiceInvocationList([FromBody] List<AddOrUpdateDeviceServiceInvocationReq> req)
        {
            var result = await _deviceServiceInvocationService.AddDeviceServiceInvocationList(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除 钻机服务调用 记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteDeviceServiceInvocationList")]
        [ProducesResponseType(typeof(ResponseDto<List<string>>), 200)]
        [PermissionAuthorize("produce:DeviceServiceInvocation:list")]
        public async Task<ActionResult> DeleteDeviceServiceInvocationList([FromBody] List<int> req)
        {
            var result = await _deviceServiceInvocationService.DeleteDeviceServiceInvocationList(req);
            return Ok(result);
        }

        /// <summary>
        /// 查询 钻机服务调用 记录，分页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDeviceServiceInvocations")]
        [ProducesResponseType(typeof(ResponseDto<List<string>>), 200)]
        [PermissionAuthorize("produce:DeviceServiceInvocation:list")]
        public async Task<ActionResult> GetDeviceServiceInvocations([FromBody] GetDeviceServiceInvocationsReq req)
        {
            var result = await _deviceServiceInvocationService.GetDeviceServiceInvocations(req);
            return Ok(result);
        }

        /// <summary>
        /// 查询 钻机服务调用 记录，List
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDeviceServiceInvocationList")]
        [ProducesResponseType(typeof(ResponseDto<List<string>>), 200)]
        [PermissionAuthorize("produce:DeviceServiceInvocation:list")]
        public async Task<ActionResult> GetDeviceServiceInvocationList([FromBody] GetDeviceServiceInvocationListReq req)
        {
            var result = await _deviceServiceInvocationService.GetDeviceServiceInvocationList(req);
            return Ok(result);
        }
    }
}
