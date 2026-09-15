using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MesProcessRecipe;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 工艺参数
    /// </summary>
    public class ProcessRecipeController : BaseController
    {
        private readonly IMesProcessRecipeService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public ProcessRecipeController(IMesProcessRecipeService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取工艺参数列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<MesProcessRecipeDto>>), 200)]
        [PermissionAuthorize("produce:recipe:list")]
        public async Task<ActionResult> GetList([FromBody] GetMesProcessRecipeListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取工艺参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<MesProcessRecipeDto>), 200)]
        [PermissionAuthorize("produce:recipe:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加工艺参数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:recipe:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateMesProcessRecipeReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改工艺参数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<MesProcessRecipeDto>), 200)]
        [PermissionAuthorize("produce:recipe:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateMesProcessRecipeReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除工艺参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:recipe:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除工艺参数集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:recipe:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}
