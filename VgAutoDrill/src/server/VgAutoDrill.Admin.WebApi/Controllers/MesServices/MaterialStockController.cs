using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStock;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 库存记录
    /// </summary>
    public class MaterialStockController : BaseController
    {
        private readonly IMaterialStockService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public MaterialStockController(IMaterialStockService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取库存记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<MaterialStockDto>>), 200)]
        [PermissionAuthorize("warehouse:materialStock:list")]
        public async Task<ActionResult> GetList([FromBody] GetMaterialStockListReq req)
        {
            var result = await _mainService.GetEquipmentList(req);
            return Ok(result);
        }
        /// <summary>
        /// 获取库存记录树形列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEquipmentTreeList")]
        [ProducesResponseType(typeof(ResponseDto<List<MaterialStockFullPropertiesTreeDto>>), 200)]
        [PermissionAuthorize("warehouse:materialStock:list")]
        public async Task<ActionResult> GetEquipmentTreeList([FromBody] GetMaterialStockListReq req)
        {
            var result = await _mainService.GetEquipmentTreeList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取库存记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<MaterialStockDto>), 200)]
        [PermissionAuthorize("warehouse:materialStock:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加库存记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:materialStock:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateMaterialStockReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改库存记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<MaterialStockDto>), 200)]
        [PermissionAuthorize("warehouse:materialStock:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateMaterialStockReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除库存记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:materialStock:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除库存记录集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:materialStock:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}
