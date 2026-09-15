using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 工序与工作站关系
    /// </summary>
    public class RouteProcessAndWorkStationController : BaseController
    {
        private readonly IRouteProcessAndWorkStationService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public RouteProcessAndWorkStationController(IRouteProcessAndWorkStationService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取工序与工作站关系列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<RouteProcessAndWorkStationDto>>), 200)]
        [PermissionAuthorize("produce:routeProcessAndWorkStation:list")]
        public async Task<ActionResult> GetList([FromBody] GetRouteProcessAndWorkStationListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取工序与工作站关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<RouteProcessAndWorkStationDto>), 200)]
        [PermissionAuthorize("produce:routeProcessAndWorkStation:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.MultiQueryByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 根据工作站获取关联的工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRoutesByWorkStation")]
        [ProducesResponseType(typeof(ResponseDto<List<RouteInfo>>), 200)]
        [PermissionAuthorize("produce:routeProcessAndWorkStation:list")]
        public async Task<ActionResult> GetRoutesByWorkStation([FromBody] GetRouteProcessAndWorkStationListReq req)
        {
            var result = await _mainService.GetRoutesByWorkStation(req);
            return Ok(result);
        }

        /// <summary>
        /// 根据工艺路线、工序获取工作站
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFitWorkStationListByRoute")]
        [ProducesResponseType(typeof(ResponseDto<List<FitWorkStationDto>>), 200)]
        [PermissionAuthorize("produce:routeProcessAndWorkStation:list")]
        public async Task<ActionResult> GetFitWorkStationListByRoute([FromBody] GetFitWorkStationListByRouteReq req)
        {
            var result = await _mainService.GetFitWorkStationListByRoute(req);
            return Ok(result);
        }

        /// <summary>
        /// 添加的时候判断工作站是否绑定其他工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExsitWorkStation")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:routeProcessAndWorkStation:add")]
        public async Task<ActionResult> ExsitWorkStation([FromBody] AddRouteProcessAndWorkStationReq req)
        {
            var result = await _mainService.ExsitWorkStation(req);
            return Ok(result);
        }
        /// <summary>
        /// 添加的时候判断工作站是否绑定其他工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExsitWorkStationByRoute")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:routeProcessAndWorkStation:add")]
        public async Task<ActionResult> ExsitWorkStationByRoute([FromBody] AddRPAndWByWorkStationReq req)
        {
            var result = await _mainService.ExsitWorkStationByRoute(req);
            return Ok(result);
        }
        /// <summary>
        /// 添加工序与工作站关系
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:routeProcessAndWorkStation:add")]
        public async Task<ActionResult> Post([FromBody] AddRouteProcessAndWorkStationReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 添加工作站与工艺路线关系
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddDataByWorkStation")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:routeProcessAndWorkStation:add")]
        public async Task<ActionResult> AddDataByWorkStation([FromBody] AddRPAndWByWorkStationReq req)
        {
            var result = await _mainService.AddDataByWorkStation(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改工序与工作站关系
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<RouteProcessAndWorkStationDto>), 200)]
        [PermissionAuthorize("produce:routeProcessAndWorkStation:edit")]
        public async Task<ActionResult> Put([FromBody] UpdateRouteProcessAndWorkStationReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除工序与工作站关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:routeProcessAndWorkStation:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.DeleteData(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除工序与工作站关系集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:routeProcessAndWorkStation:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _mainService.DeleteDataList(idList);
            return Ok(result);
        }
    }
}
