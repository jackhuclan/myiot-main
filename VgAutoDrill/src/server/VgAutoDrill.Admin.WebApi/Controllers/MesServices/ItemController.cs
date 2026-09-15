using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem.VegaRawMaterial;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemController : BaseController
    {
        private readonly IItemService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public ItemController(IItemService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTreeSelect")]
        [ProducesResponseType(typeof(ResponseDto<List<ItemTreeDto>>), 200)]
        [PermissionAuthorize("masterData:item:list")]
        public async Task<ActionResult> GetTreeSelectList()
        {
            var result = await _mainService.GetTreeList();
            return Ok(result);
        }

        /// <summary>
        /// 获取物料列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<ItemDto>>), 200)]
        [PermissionAuthorize("masterData:item:list")]
        public async Task<ActionResult> GetList([FromBody] GetItemListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取树形物料列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFullTreeList")]
        [ProducesResponseType(typeof(ResponseDto<List<ItemFullPropertiesTreeDto>>), 200)]
        [PermissionAuthorize("masterData:item:list")]
        public async Task<ActionResult> GetFullTreeList([FromBody] GetItemListReq req)
        {
            var result = await _mainService.GetFullTreeList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取物料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<ItemDto>), 200)]
        [PermissionAuthorize("masterData:item:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加物料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:item:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateItemReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改物料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<ItemDto>), 200)]
        [PermissionAuthorize("masterData:item:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateItemReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除物料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:item:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除物料集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:item:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 导入物料数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:item:upload")]
        public async Task<ActionResult> UploadListAsync([FromForm(Name = "file")] IFormFile objFile)
        {
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }
                //物料数据List
                List<ItemToExcelDto> list = new List<ItemToExcelDto>();

                //文件转成流
                using (var fileStream = objFile.OpenReadStream())
                {
                    //excel转实体
                    list = ExcelHelper.ParseExcelToList<ItemToExcelDto>(fileStream, "Sheet1");
                }

                var result = await _mainService.BulkInsert(list);
                return Ok(result);
            }
            catch
            {
                var result = new ResponseDto<string>();
                result.Code = ResponseCode.Fail;
                result.Message = "请参考模板准备导入数据！";
                return Ok(result);
            }
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoad")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("masterData:item:download")]
        public ActionResult DownLoad()
        {
            List<ItemToExcelDto> list = new List<ItemToExcelDto>();
            //添加示例数据
            ItemToExcelDto model = new ItemToExcelDto();
            model.Code = "IF2023040500002";
            model.Name = "PCB单层板";
            model.ParentID = 35;
            model.ItemOrProduct = 1;
            model.ItemTypeId = 22;
            model.ProductCategoryCode = "产品大类编码";
            model.ProductCategoryId = 6;
            model.ProductCategoryName = "产品大类名称";
            model.Specification = "DDD XXXX DD";
            model.UnitOfMeasure = "Panel";
            model.WarehouseCode = "库房编码";
            model.WarehouseId = 6;
            model.WarehouseName = "库房名称";
            list.Add(model);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "物料模板.xlsx");
        }

        /// <summary>
        /// 获取维嘉生料(中转位和AGV)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVegaRawMaterial")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseDto<List<GetVegaRawMaterialDto>>), 200)]
        public async Task<ActionResult> GetVegaRawMaterial(GetVegaRawMaterialReq req)
        {
            var result = await _mainService.GetVegaRawMaterial(req);
            return Ok(result);
        }
    }
}
