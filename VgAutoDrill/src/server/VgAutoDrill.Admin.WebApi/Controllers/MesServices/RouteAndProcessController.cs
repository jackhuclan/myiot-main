using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 工艺路线与工序关系
    /// </summary>
    public class RouteAndProcessController : BaseController
    {
        private readonly IRouteAndProcessService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public RouteAndProcessController(IRouteAndProcessService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取工艺路线与工序关系列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<RouteAndProcessDto>>), 200)]
        [PermissionAuthorize("produce:routeAndProcess:list")]
        public async Task<ActionResult> GetList([FromBody] GetRouteAndProcessListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取工艺路线与工序关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<RouteAndProcessDto>), 200)]
        [PermissionAuthorize("produce:routeAndProcess:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.MultiQueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 根据工序编码获取关联的工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRoutesByProcess")]
        [ProducesResponseType(typeof(ResponseDto<List<RouteInfo>>), 200)]
        [PermissionAuthorize("produce:routeAndProcess:list")]
        public async Task<ActionResult> GetRoutesByProcess([FromBody] GetRoutesByProcessReq req)
        {
            var result = await _mainService.GetRoutesByProcess(req);
            return Ok(result);
        }
        /// <summary>
        /// 添加工艺路线与工序关系
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:routeAndProcess:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateRouteAndProcessReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改工艺路线与工序关系
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<RouteAndProcessDto>), 200)]
        [PermissionAuthorize("produce:routeAndProcess:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateRouteAndProcessReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除工艺路线与工序关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:routeAndProcess:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除工艺路线与工序关系集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:routeAndProcess:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}
