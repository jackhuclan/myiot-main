using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EquipmentType;
using VgAutoDrill.Admin.Model.ViewModels.Req.Equipment;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 设备管理
    /// </summary>
    public class DeviceTypeController : BaseController
    {
        private readonly IDeviceTypeService _deviceTypeService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTypeService"></param>
        public DeviceTypeController(IDeviceTypeService deviceTypeService)
        {
            _deviceTypeService = deviceTypeService;
        }
        /// <summary>
        /// 获取设备类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTreeSelect")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceTypeTreeDto>>), 200)]
        [PermissionAuthorize("device:equiptype:list")]
        public async Task<ActionResult> GetTreeSelectList()
        {
            var result = await _deviceTypeService.GetEquipmentTypeTreeList();
            return Ok(result);
        }
        /// <summary>
        /// 获取设备类别列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<PageDto<DeviceTypeDto>>), 200)]
        [PermissionAuthorize("device:equiptype:list")]
        public async Task<ActionResult> GetList([FromBody] GetEquipmentTypeListReq req)
        {
            var result = await _deviceTypeService.GetEquipmentTypeList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取设备类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<DeviceTypeInfoDto>), 200)]
        [PermissionAuthorize("device:equiptype:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _deviceTypeService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加设备类别
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:equiptype:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateDeviceTypeReq req)
        {
            var result = await _deviceTypeService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改设备类别
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<DeviceInfoDto>), 200)]
        [PermissionAuthorize("device:equiptype:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateDeviceTypeReq req)
        {
            var result = await _deviceTypeService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:equiptype:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _deviceTypeService.DeleteData(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备类别集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:equiptype:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _deviceTypeService.DeleteDataList(idList);
            return Ok(result);
        }
    }
}
