using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.LocationDetail;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 库位明细管理
    /// </summary>
    public class LocationDetailController : BaseController
    {
        private readonly ILocationDetailService _locationDetailService;

        public LocationDetailController(ILocationDetailService locationDetailService)
        {
            _locationDetailService = locationDetailService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<PageDto<LocationDetailDto>>), 200)]
        [PermissionAuthorize("warehouse:locationDetailManage:list")]
        public async Task<ActionResult> GetList([FromBody] LocationDetailQueryReq req)
        {
            var result = await _locationDetailService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]
        [ProducesResponseType(typeof(ResponseDto<LocationDetailDto>), 200)]
        [PermissionAuthorize("warehouse:locationDetailManage:list")]
        public async Task<ActionResult> GetById(long id)
        {
            var result = await _locationDetailService.QueryByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 根据库位编码查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByCode/{code}")]
        [ProducesResponseType(typeof(ResponseDto<List<LocationDetailDto>>), 200)]
        [PermissionAuthorize("warehouse:locationDetailManage:list")]
        public async Task<ActionResult> GetByCode(string code)
        {
            var result = await _locationDetailService.GetByCode(code);
            return Ok(result);
        }

        /// <summary>
        /// 根据库位编码获取库位明细
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLocationDetail/{locationCode}")]
        [ProducesResponseType(typeof(ResponseDto<List<LocationDetailDto>>), 200)]
        [PermissionAuthorize("warehouse:locationDetailManage:list")]
        public async Task<ActionResult> GetLocationDetail(string locationCode)
        {
            var result = await _locationDetailService.GetByCode(locationCode);
            return Ok(result);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:locationDetailManage:add")]
        public async Task<ActionResult> Add([FromBody] AddOrUpdateLocationDetailReq req)
        {
            var result = await _locationDetailService.Add(req);
            return Ok(result);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddBatch")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:locationDetailManage:add")]
        public async Task<ActionResult> AddBatch([FromBody] List<AddOrUpdateLocationDetailReq> req)
        {
            var result = await _locationDetailService.AddBatch(req);
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
        [PermissionAuthorize("warehouse:locationDetailManage:edit")]
        public async Task<ActionResult> Update([FromBody] AddOrUpdateLocationDetailReq req)
        {
            var result = await _locationDetailService.Update(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete/{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:locationDetailManage:delete")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _locationDetailService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="delList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:locationDetailManage:delete")]
        public async Task<ActionResult> DeleteList([FromBody] object[] delList)
        {
            var result = await _locationDetailService.DeleteList(delList);
            return Ok(result);
        }
    }
}
