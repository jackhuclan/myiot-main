using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndSubject;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 设备点检项目模板
    /// </summary>
    public class DeviceAndSubjectController : BaseController
    {
        private readonly IDeviceAndSubjectService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public DeviceAndSubjectController(IDeviceAndSubjectService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取设备点检项目模板列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceAndSubjectDto>>), 200)]
        [PermissionAuthorize("device:deviceAndSubject:list")]
        public async Task<ActionResult> GetList([FromBody] GetDeviceAndSubjectListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取设备点检项目模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<DeviceAndSubjectDto>), 200)]
        [PermissionAuthorize("device:deviceAndSubject:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加设备点检项目模板
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:deviceAndSubject:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateDeviceAndSubjectReq req)
        {
            var result = await _mainService.AddDatas(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改设备点检项目模板
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<DeviceAndSubjectDto>), 200)]
        [PermissionAuthorize("device:deviceAndSubject:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateDeviceAndSubjectReq req)
        {
            var result = await _mainService.UpdateDatas(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备点检项目模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:deviceAndSubject:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备点检项目模板集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:deviceAndSubject:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}