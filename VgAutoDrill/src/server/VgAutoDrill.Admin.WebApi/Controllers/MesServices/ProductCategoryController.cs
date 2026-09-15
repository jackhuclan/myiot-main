using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProductCategory;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 产品大类
    /// </summary>
    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public ProductCategoryController(IProductCategoryService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取产品大类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTreeSelect")]
        [ProducesResponseType(typeof(ResponseDto<List<ProductCategoryTreeDto>>), 200)]
        [PermissionAuthorize("masterData:productCategory:list")]
        public async Task<ActionResult> GetTreeSelectList()
        {
            var result = await _mainService.GetTreeList();
            return Ok(result);
        }

        /// <summary>
        /// 获取产品大类列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<ProductCategoryDto>>), 200)]
        [PermissionAuthorize("masterData:productCategory:list")]
        public async Task<ActionResult> GetList([FromBody] GetProductCategoryListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取产品大类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<ProductCategoryDto>), 200)]
        [PermissionAuthorize("masterData:productCategory:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加产品大类
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:productCategory:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateProductCategoryReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改产品大类
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<ProductCategoryDto>), 200)]
        [PermissionAuthorize("masterData:productCategory:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateProductCategoryReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除产品大类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:productCategory:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.DeleteData(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除产品大类集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:productCategory:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _mainService.DeleteDataList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 审批产品大类
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VettingProductCategory")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:productCategory:edit")]
        public async Task<ActionResult> VettingProductCategory([FromBody] VettingProductCategoryReq req)
        {
            var result = await _mainService.VettingProductCategory(req);
            return Ok(result);
        }

        /// <summary>
        /// 取消审批产品大类
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CancelVettingProductCategory")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:productCategory:edit")]
        public async Task<ActionResult> CancelVettingProductCategory([FromBody] VettingProductCategoryReq req)
        {
            var result = await _mainService.CancelVettingProductCategory(req);
            return Ok(result);
        }

        /// <summary>
        /// 导入产品大类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:productCategory:upload")]
        public async Task<ActionResult> UploadListAsync([FromForm(Name = "file")] IFormFile objFile)
        {
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }
                //产品大类List
                List<ProductCategoryToExcelDto> products = new List<ProductCategoryToExcelDto>();

                //文件转成流
                using (var fileStream = objFile.OpenReadStream())
                {
                    //excel转实体
                    products = ExcelHelper.ParseExcelToList<ProductCategoryToExcelDto>(fileStream, "Sheet1");
                }

                var result = await _mainService.BulkInsert(products);
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
        [PermissionAuthorize("masterData:productCategory:download")]
        public ActionResult DownLoad()
        {
            List<ProductCategoryToExcelDto> products = new List<ProductCategoryToExcelDto>();
            //添加示例数据
            ProductCategoryToExcelDto model = new ProductCategoryToExcelDto();
            model.Code = "p01";
            model.Name = "单面版";
            model.ParentID = 0;
            model.Remark = "单面板";
            model.DispenseMachines = 2;
            products.Add(model);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(products);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "产品大类模板.xlsx");
        }
    }
}
