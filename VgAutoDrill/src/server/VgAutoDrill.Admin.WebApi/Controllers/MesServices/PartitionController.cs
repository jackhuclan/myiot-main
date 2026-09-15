using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Partition;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 分区信息
    /// </summary>
    public class PartitionController : BaseController
    {
        private readonly IPartitionService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public PartitionController(IPartitionService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取分区信息列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<PartitionDto>>), 200)]
        [PermissionAuthorize("warehouse:partition:list")]
        [AllowAnonymous]
        public async Task<ActionResult> GetList([FromBody] GetPartitionListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        /// 获取分区树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTreeSelect")]
        [ProducesResponseType(typeof(ResponseDto<List<PartitionTreeDto>>), 200)]
        [PermissionAuthorize("warehouse:partition:list")]
        public async Task<ActionResult> GetTreeSelectList()
        {
            var result = await _mainService.GetTreeList();
            return Ok(result);
        }
        /// <summary>
        ///获取分区信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<PartitionDto>), 200)]
        [PermissionAuthorize("warehouse:partition:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryWithSettingByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加分区信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:partition:add")]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] AddOrUpdatePartitionReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改分区信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<PartitionDto>), 200)]
        [PermissionAuthorize("warehouse:partition:edit")]
        [AllowAnonymous]
        public async Task<ActionResult> Put([FromBody] AddOrUpdatePartitionReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 启用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Enable")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:partition:edit")]
        public async Task<ActionResult> Enable(string code)
        {
            var result = await _mainService.SetStatus(code, 1);
            return Ok(result);
        }
        /// <summary>
        /// 禁用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Disable")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:partition:edit")]
        public async Task<ActionResult> Disable(string code)
        {
            var result = await _mainService.SetStatus(code, 0);
            return Ok(result);
        }
        /// <summary>
        /// 删除分区信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:partition:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除分区信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:partition:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}
