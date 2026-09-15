using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 工艺路线
    /// </summary>
    public class RouteController : BaseController
    {
        private readonly IRouteService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public RouteController(IRouteService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取工艺路线列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<RouteDto>>), 200)]
        [PermissionAuthorize("produce:route:list")]
        public async Task<ActionResult> GetList([FromBody] GetRouteListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取工艺路线列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDropSelectDatas")]
        [ProducesResponseType(typeof(ResponseDto<List<DropSelectDto>>), 200)]
        [PermissionAuthorize("produce:route:list")]
        public async Task<ActionResult> GetDropSelectDatas([FromBody] GetRouteListReq req)
        {
            var result = await _mainService.GetDropSelectDatas(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取工艺路线列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListDetail")]
        [ProducesResponseType(typeof(ResponseDto<List<RouteDto>>), 200)]
        [PermissionAuthorize("produce:route:list")]
        public async Task<ActionResult> GetListDetail([FromBody] GetRouteListReq req)
        {
            var result = await _mainService.GetListDetail(req);
            return Ok(result);
        }


        /// <summary>
        ///获取工艺路线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<RouteDto>), 200)]
        [PermissionAuthorize("produce:route:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:route:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateRouteReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<RouteDto>), 200)]
        [PermissionAuthorize("produce:route:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateRouteReq req)
        {
            var result = await _mainService.VerifyUpdate(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除工艺路线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:route:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.DeleteData(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除工艺路线集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:route:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _mainService.DeleteDataList(idList);
            return Ok(result);
        }
        /// <summary>
        /// 审批工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VettingRoute")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:route:edit")]
        public async Task<ActionResult> VettingRoute([FromBody] VettingRouteReq req)
        {
            var result = await _mainService.VettingRoute(req);
            return Ok(result);
        }

        /// <summary>
        /// 取消审批工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CancelVettingRoute")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:route:edit")]
        public async Task<ActionResult> CancelVettingRoute([FromBody] VettingRouteReq req)
        {
            var result = await _mainService.CancelVettingRoute(req);
            return Ok(result);
        }
    }
}
