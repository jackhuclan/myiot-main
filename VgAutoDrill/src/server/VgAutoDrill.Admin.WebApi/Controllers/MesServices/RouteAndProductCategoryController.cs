using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 工艺路线与产品大类关系
    /// </summary>
    public class RouteAndProductCategoryController : BaseController
    {
        private readonly IRouteAndProductCategoryService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public RouteAndProductCategoryController(IRouteAndProductCategoryService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取工艺路线与产品大类关系列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<RouteAndProductCategoryDto>>), 200)]
        [PermissionAuthorize("produce:routeAndProductCategory:list")]
        public async Task<ActionResult> GetList([FromBody] GetRouteAndProductCategoryListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        /// 根据产品大类获取工艺路线数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRouteInfoList")]
        [ProducesResponseType(typeof(ResponseDto<List<RouteInfoByProductCategoryDto>>), 200)]
        [PermissionAuthorize("produce:routeAndProductCategory:list")]
        public async Task<ActionResult> GetRouteInfoList([FromBody] GetRouteAndProductCategoryListReq req)
        {
            var result = await _mainService.GetRouteInfoList(req);
            return Ok(result);
        }

        /// <summary>
        ///获取工艺路线与产品大类关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<RouteAndProductCategoryDto>), 200)]
        [PermissionAuthorize("produce:routeAndProductCategory:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.MultiQueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加工艺路线与产品大类关系
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:routeAndProductCategory:add")]
        public async Task<ActionResult> Post([FromBody] AddRouteAndProductCategoryReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改工艺路线与产品大类关系
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<RouteAndProductCategoryDto>), 200)]
        [PermissionAuthorize("produce:routeAndProductCategory:edit")]
        public async Task<ActionResult> Put([FromBody] UpdateRouteAndProductCategoryReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }

        /// <summary>
        /// 上移/下移 修改优先级
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateDataOrderNum")]
        [ProducesResponseType(typeof(ResponseDto<RouteAndProductCategoryDto>), 200)]
        [PermissionAuthorize("produce:routeAndProductCategory:edit")]
        public async Task<ActionResult> UpdateDataOrderNum([FromBody] UpdateDataOrderNumReq req)
        {
            var result = await _mainService.UpdateDataOrderNum(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除工艺路线与产品大类关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:routeAndProductCategory:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除工艺路线与产品大类关系集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:routeAndProductCategory:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}
