using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workshop;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 车间管理
    /// </summary>
    public class WorkshopController : BaseController
    {
        private readonly IWorkshopService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public WorkshopController(IWorkshopService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取车间列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<WorkshopDto>>), 200)]
        [PermissionAuthorize("masterData:workshop:list")]
        public async Task<ActionResult> GetList([FromBody] GetWorkshopListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        /// 获取车间列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDropSelectDatas")]
        [ProducesResponseType(typeof(ResponseDto<List<DropSelectDto>>), 200)]
        [PermissionAuthorize("masterData:workshop:list")]
        public async Task<ActionResult> GetDropSelectDatas([FromBody] GetWorkshopListReq req)
        {
            var result = await _mainService.GetDropSelectDatas(req);
            return Ok(result);
        }
        /// <summary>
        ///获取车间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<WorkshopDto>), 200)]
        [PermissionAuthorize("masterData:workshop:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加车间
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:workshop:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateWorkshopReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改车间
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<WorkshopDto>), 200)]
        [PermissionAuthorize("masterData:workshop:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateWorkshopReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除车间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:workshop:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除车间集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:workshop:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}