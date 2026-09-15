using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRest;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// AGV休息点
    /// </summary>
    public class AgvRestController : BaseController
    {
        private readonly IAgvRestService _mainService;

        public AgvRestController(IAgvRestService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取AGV休息点列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<RestCodeDto>>), 200)]
        [PermissionAuthorize("agv:agvrest:list")]
        public async Task<ActionResult> GetList([FromBody] GetRestCodeListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        ///获取AGV休息点记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<RestCodeDto>), 200)]
        [PermissionAuthorize("agv:agvrest:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 添加AGV休息点记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("agv:agvrest:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateRestCodeReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }

        /// <summary>
        /// 修改AGV休息点记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<RestCodeDto>), 200)]
        [PermissionAuthorize("agv:agvrest:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateRestCodeReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除AGV休息点记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("agv:agvrest:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// 删除AGV休息点记录集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("agv:agvrest:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Enable")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("agv:agvrest:edit")]
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
        [PermissionAuthorize("agv:agvrest:edit")]
        public async Task<ActionResult> Disable(string code)
        {
            var result = await _mainService.SetStatus(code, 0);
            return Ok(result);
        }

        /// <summary>
        /// 导入休息点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("agv:agvrest:upload")]
        public async Task<ActionResult> UploadListAsync([FromForm(Name = "file")] IFormFile objFile)
        {
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }
                //休息点List
                List<AgvRestToExcelDto> list = new List<AgvRestToExcelDto>();

                //文件转成流
                using (var fileStream = objFile.OpenReadStream())
                {
                    //excel转实体
                    list = ExcelHelper.ParseExcelToList<AgvRestToExcelDto>(fileStream, "Sheet1");
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
        [PermissionAuthorize("agv:agvrest:download")]
        public ActionResult DownLoad()
        {
            List<AgvRestToExcelDto> list = new List<AgvRestToExcelDto>();
            //添加示例数据
            AgvRestToExcelDto model = new AgvRestToExcelDto();
            model.Code = "Rest0001";
            model.Name = "插齿分区1大车休息点1";
            model.Point = "100";
            list.Add(model);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "休息点模板.xlsx");
        }

    }
}
