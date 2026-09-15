using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.Position;
using VgAutoDrill.Admin.Model.ViewModels.Res.Position;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 职位管理
    /// </summary>
    public class PositionController : BaseController
    {
        private readonly IPositionService _positionService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="positionService"></param>
        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }
        /// <summary>
        /// 添加职位信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("system:post:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdatePositionReq req)
        {
            var result = await _positionService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改职位信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("system:post:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdatePositionReq req)
        {
            var result = await _positionService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 查看职位信息详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<PositionDto>), 200)]
        [PermissionAuthorize("system:post:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _positionService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除职位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("system:post:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _positionService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 获取职位信息列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<PageDto<PositionDto>>), 200)]
        [PermissionAuthorize("system:post:list")]
        public async Task<ActionResult> GetList([FromBody] PositionListReq req)
        {
            var result = await _positionService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除职位信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("system:post:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _positionService.DeleteList(idList);
            return Ok(result);
        }
    }
}
