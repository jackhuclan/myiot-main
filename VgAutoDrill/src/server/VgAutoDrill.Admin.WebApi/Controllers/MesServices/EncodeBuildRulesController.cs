using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EncodeBuildRules;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 编码生成规则
    /// </summary>
    public class EncodeBuildRulesController : BaseController
    {
        private readonly IEncodeBuildRulesService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public EncodeBuildRulesController(IEncodeBuildRulesService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取编码生成规则列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<EncodeBuildRulesDto>>), 200)]
        [PermissionAuthorize("masterData:encodeBuildRules:list")]
        public async Task<ActionResult> GetList([FromBody] GetEncodeBuildRulesListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取编码生成规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<EncodeBuildRulesDto>), 200)]
        [PermissionAuthorize("masterData:encodeBuildRules:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.NewQueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 修改编码生成规则
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<EncodeBuildRulesDto>), 200)]
        [PermissionAuthorize("masterData:encodeBuildRules:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateEncodeBuildRulesReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }

        /// <summary>
        /// 添加编码生成规则
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:encodeBuildRules:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateEncodeBuildRulesReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除编码生成规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:encodeBuildRules:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// 根据规则编号生成编码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEncodeList")]
        [ProducesResponseType(typeof(ResponseDto<List<string>>), 200)]
        [PermissionAuthorize("masterData:encodeBuildRules:list")]
        public async Task<ActionResult> GetEncodeList([FromBody] GetEncodeByRulesListReq req)
        {
            var result = await _mainService.GetEncodeList(req);
            return Ok(result);
        }
    }
}
