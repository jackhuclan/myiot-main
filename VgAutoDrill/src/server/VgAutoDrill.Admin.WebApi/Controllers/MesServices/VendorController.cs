using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Vendor;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 供应商管理
    /// </summary>
    public class VendorController : BaseController
    {
        private readonly IVendorService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public VendorController(IVendorService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取供应商列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<VendorDto>>), 200)]
        [PermissionAuthorize("masterData:vendor:list")]
        public async Task<ActionResult> GetList([FromBody] GetVendorListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取供应商
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<VendorDto>), 200)]
        [PermissionAuthorize("masterData:vendor:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:vendor:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateVendorReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改供应商
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<VendorDto>), 200)]
        [PermissionAuthorize("masterData:vendor:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateVendorReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除供应商
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:vendor:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除供应商集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:vendor:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 导入供应商
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:vendor:upload")]
        public async Task<ActionResult> UploadListAsync([FromForm(Name = "file")] IFormFile objFile)
        {
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }
                //供应商List
                List<VendorToExcelDto> list = new List<VendorToExcelDto>();

                //文件转成流
                using (var fileStream = objFile.OpenReadStream())
                {
                    //excel转实体
                    list = ExcelHelper.ParseExcelToList<VendorToExcelDto>(fileStream, "Sheet1");
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
        [PermissionAuthorize("masterData:vendor:download")]
        public ActionResult DownLoad()
        {
            List<VendorToExcelDto> list = new List<VendorToExcelDto>();
            //添加示例数据
            VendorToExcelDto model = new VendorToExcelDto();
            model.Code = "15200000001";
            model.Name = "张老板";
            model.Remark = "铜料板供应";
            list.Add(model);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "供应商模板.xlsx");
        }
    }
}