using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 
    /// </summary>
    public class SysConfigController : BaseController
    {
        private readonly ISysConfigService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public SysConfigController(ISysConfigService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取系统配置列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<SysConfigDto>>), 200)]
        [PermissionAuthorize("system:sysConfig:list")]
        public async Task<ActionResult> GetList([FromBody] GetSysConfigListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取系统配置列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTreeList")]
        [ProducesResponseType(typeof(ResponseDto<List<SysConfigTreeDto>>), 200)]
        [PermissionAuthorize("system:sysConfig:list")]
        public async Task<ActionResult> GetTreeList([FromBody] GetSysConfigListReq req)
        {
            var result = await _mainService.GetTreeList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取系统配置类型树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSysConfigCategoryEnums")]
        [ProducesResponseType(typeof(ResponseDto<List<SysConfigTreeDto>>), 200)]
        [PermissionAuthorize("system:sysConfig:list")]
        public async Task<ActionResult> GetSysConfigCategoryEnums()
        {
            var result = await _mainService.GetSysConfigCategoryEnums();
            return Ok(result);
        }

        /// <summary>
        /// 根据配置项获取枚举列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSysConfigEnumList")]
        [ProducesResponseType(typeof(ResponseDto<SysConfigInfoByEnumType>), 200)]
        [PermissionAuthorize("system:sysConfig:list")]
        public async Task<ActionResult> GetSysConfigEnumList([FromBody] GetSysConfigByEnumType req)
        {
            var result = await _mainService.GetSysConfigEnumList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取配置项类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSysConfigType")]
        [ProducesResponseType(typeof(ResponseDto<List<string>>), 200)]
        [PermissionAuthorize("system:sysConfig:list")]
        public async Task<ActionResult> GetSysConfigType()
        {
            var result = new List<string> { "int", "string", "enum", "bool" };
            return Ok(result);
        }

        /// <summary>
        ///获取系统配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<SysConfigDto>), 200)]
        [PermissionAuthorize("system:sysConfig:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加系统配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("system:sysConfig:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateSysConfigReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改系统配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<SysConfigDto>), 200)]
        [PermissionAuthorize("system:sysConfig:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateSysConfigReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除系统配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("system:sysConfig:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.DeleteData(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除系统配置集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("system:sysConfig:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _mainService.DeleteDataList(idList);
            return Ok(result);
        }

        /// <summary>
        ///获取告警信息等及时信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSysTimelyInformation")]
        [ProducesResponseType(typeof(ResponseDto<SysTimelyInformationDto>), 200)]
        [PermissionAuthorize("system:sysConfig:view")]
        public async Task<ActionResult> GetSysTimelyInformation()
        {
            var result = await _mainService.GetSysTimelyInformation();
            return Ok(result);
        }

        /// <summary>
        /// 保存基本配置
        /// </summary>
        /// <param name="reqs"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveBasicSysData")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("system:sysConfig:add")]
        public async Task<ActionResult> SaveBasicSysData([FromBody] List<AddOrUpdateSysConfigReq> reqs)
        {
            var result = await _mainService.SaveBasicSysData(reqs);
            return Ok(result);
        }
    }
}
