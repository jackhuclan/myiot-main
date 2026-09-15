using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFileDetail;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// ATP文件明细
    /// </summary>
    public class ItemAtpFileDetailController : BaseController
    {
        private readonly IItemAtpFileDetailService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public ItemAtpFileDetailController(IItemAtpFileDetailService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取ATP文件详细列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<ItemAtpFileDetailDto>>), 200)]
        [PermissionAuthorize("material:atpFileDetail:list")]
        public async Task<ActionResult> GetList([FromBody] GetItemAtpFileDetailListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        /// 获取ATP文件详细列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMultiList")]
        [ProducesResponseType(typeof(ResponseDto<List<List<List<ItemAtpFileDetailDto>>>>), 200)]
        [PermissionAuthorize("material:atpFileDetail:list")]
        public async Task<ActionResult> GetMultiList([FromBody] GetItemAtpFileDetailListReq req)
        {
            var result = await _mainService.GetMultiList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取ATP文件详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<ItemAtpFileDetailDto>), 200)]
        [PermissionAuthorize("material:atpFileDetail:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加ATP文件详细
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:atpFileDetail:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateItemAtpFileDetailReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改ATP文件详细
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<ItemAtpFileDetailDto>), 200)]
        [PermissionAuthorize("material:atpFileDetail:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateItemAtpFileDetailReq req)
        {
            var result = await _mainService.UpdateCheck(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改ATP文件详细
        /// </summary>
        /// <param name="reqList"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateList")]
        [ProducesResponseType(typeof(ResponseDto<ItemAtpFileDetailDto>), 200)]
        [PermissionAuthorize("material:atpFileDetail:edit")]
        public async Task<ActionResult> Put([FromBody] List<AddOrUpdateItemAtpFileDetailReq> reqList)
        {
            var result = await _mainService.UpdateList(reqList);
            return Ok(result);
        }
        /// <summary>
        /// 删除ATP文件详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:atpFileDetail:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除ATP文件详细集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:atpFileDetail:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}
