using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.AppSecret;
using VgAutoDrill.Admin.Model.ViewModels.Res.AppSecret;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 应用管理
    /// </summary>
    public class AppSecretController : BaseController
    {
        private readonly IAppSecretService _appSecretService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSecretService"></param>
        public AppSecretController(IAppSecretService appSecretService)
        {
            _appSecretService = appSecretService;
        }
        /// <summary>
        /// 添加应用
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("system:app:add")]
        public async Task<ActionResult> Post(AddAppSecretReq req)
        {
            var result = await _appSecretService.AddAppSecret(req);
            return Ok(result);
        }
        /// <summary>
        /// 获取应用列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPageList")]
        [ProducesResponseType(typeof(ResponseDto<PageDto<AppSecretDto>>), 200)]
        [PermissionAuthorize("system:app:list")]
        public async Task<ActionResult> GetPageList(GetAppSecretPageListReq req)
        {
            var result = await _appSecretService.GetPageList(req);
            return Ok(result);
        }
        /// <summary>
        /// 查看应用信息详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<AppSecretDto>), 200)]
        [PermissionAuthorize("system:app:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _appSecretService.GetAppSecret(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除应用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("system:app:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _appSecretService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 修改应用
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("system:app:edit")]
        public async Task<ActionResult> Put(SysAppSecretReq req)
        {
            var result = await _appSecretService.Update(req);
            return Ok(result);
        }
    }
}
