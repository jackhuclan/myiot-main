using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceTemporaryMaintenanceRecords;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 设备临时保养维护记录
    /// </summary>
    public class DeviceTemporaryMaintenanceRecordsController : BaseController
    {
        private readonly IDeviceTemporaryMaintenanceRecordsService _mainService;
        public DeviceTemporaryMaintenanceRecordsController(IDeviceTemporaryMaintenanceRecordsService mainService)
        {
            _mainService = mainService;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        //[PermissionAuthorize("device:DeviceRecords:list")]
        public async Task<ActionResult> Add([FromBody] AddOrUpdateDeviceTemporaryMaintenanceRecordsReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 获取设备记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceTemporaryMaintenanceRecordsDto>>), 200)]

        public async Task<ActionResult> GetList([FromBody] GetDeviceTemporaryMaintenanceRecordsListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryByID")]
        [ProducesResponseType(typeof(ResponseDto<DeviceTemporaryMaintenanceRecordsDto>), 200)]

        public async Task<ActionResult> QueryByID(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]

        public async Task<ActionResult> Update([FromBody] AddOrUpdateDeviceTemporaryMaintenanceRecordsReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]

        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }

    }
}
