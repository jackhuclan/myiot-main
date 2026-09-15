using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceCutter;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 刀具管理
    /// </summary>
    public class DeviceCutterController : BaseController
    {
        private readonly IDeviceCutterService _deviceCutterService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceCutterService"></param>
        public DeviceCutterController(IDeviceCutterService deviceCutterService)
        {
            _deviceCutterService = deviceCutterService;
        }

        /// <summary>
        /// 获取刀具列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<CutterDto>>), 200)]
        [PermissionAuthorize("material:cutter:list")]
        public async Task<ActionResult> GetDeviceCutterList([FromBody] GetCutterListReq req)
        {
            var result = await _deviceCutterService.GetDeviceCutterList(req);
            return Ok(result);
        }

        /// <summary>
        ///获取刀具
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<CutterDto>), 200)]
        [PermissionAuthorize("material:cutter:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _deviceCutterService.QueryByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 添加刀具
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:cutter:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateCutterReq req)
        {
            req.InboundDate = DateTime.Now;
            var result = await _deviceCutterService.AddData(req);
            return Ok(result);
        }

        /// <summary>
        /// 修改刀具
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<CutterDto>), 200)]
        [PermissionAuthorize("material:cutter:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateCutterReq req)
        {
            var result = await _deviceCutterService.UpdateData(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除刀具
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:cutter:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _deviceCutterService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除刀具集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:cutter:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _deviceCutterService.DeleteList(idList);
            return Ok(result);
        }
    }
}
