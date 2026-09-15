using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Subject;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 点检保养项目
    /// </summary>
    public class SubjectController : BaseController
    {
        private readonly ISubjectService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public SubjectController(ISubjectService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取树形点检保养项目信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTreeSelect")]
        [ProducesResponseType(typeof(ResponseDto<List<SubjectTreeDto>>), 200)]
        [PermissionAuthorize("produce:subject:list")]
        public async Task<ActionResult> GetTreeSelectList()
        {
            var result = await _mainService.GetTreeList();
            return Ok(result);
        }

        /// <summary>
        /// 获取点检保养项目列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<SubjectDto>>), 200)]
        [PermissionAuthorize("produce:subject:list")]
        public async Task<ActionResult> GetList([FromBody] GetSubjectListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取点检保养项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<SubjectDto>), 200)]
        [PermissionAuthorize("produce:subject:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加点检保养项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:subject:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateSubjectReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改点检保养项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<SubjectDto>), 200)]
        [PermissionAuthorize("produce:subject:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateSubjectReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除点检保养项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:subject:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除点检保养项目集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:subject:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}
