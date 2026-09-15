using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRestAndPart;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// AGV休息点和分区关联关系
    /// </summary>
    public class AgvRestAndPartController : BaseController
    {
        private readonly IAgvRestAndPartService _mainService;

        public AgvRestAndPartController(IAgvRestAndPartService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取AGV休息点和分区关联关系列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<RestAndPartDto>>), 200)]
        [PermissionAuthorize("agv:agvrestandpart:list")]
        public async Task<ActionResult> GetList([FromBody] GetRestAndPartListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        ///获取AGV休息点和分区关联关系记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<RestAndPartDto>), 200)]
        [PermissionAuthorize("agv:agvrest:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryDataByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 添加AGV休息点和分区关联关系记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("agv:agvrest:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateRestAndPartReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }

        /// <summary>
        /// 修改AGV休息点和分区关联关系记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<RestAndPartDto>), 200)]
        [PermissionAuthorize("agv:agvrest:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateRestAndPartReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除AGV休息点和分区关联关系记录
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
        /// 删除AGV休息点和分区关联关系记录集合
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
        /// 导入休息点配置
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
                List<RestAndPartToExcelDto> list = new List<RestAndPartToExcelDto>();

                //文件转成流
                using (var fileStream = objFile.OpenReadStream())
                {
                    //excel转实体
                    list = ExcelHelper.ParseExcelToList<RestAndPartToExcelDto>(fileStream, "Sheet1");
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
            List<RestAndPartToExcelDto> list = new List<RestAndPartToExcelDto>();
            //添加示例数据
            RestAndPartToExcelDto model = new RestAndPartToExcelDto();
            model.RestCode = "Rest0001";
            model.PartCode = "PFork001";
            model.RouteCode = "B0002";
            model.AgvDeviceKind = 6;
            model.Priority = 1;
            list.Add(model);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "休息点配置模板.xlsx");
        }
    }
}
