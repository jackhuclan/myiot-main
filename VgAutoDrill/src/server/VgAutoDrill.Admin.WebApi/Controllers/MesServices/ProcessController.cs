using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MesProcess;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 工序管理
    /// </summary>
    public class ProcessController : BaseController
    {
        private readonly IMesProcessService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public ProcessController(IMesProcessService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取工序列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<MesProcessDto>>), 200)]
        [PermissionAuthorize("produce:process:list")]
        public async Task<ActionResult> GetList([FromBody] GetMesProcessListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        /// 获取工序列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDropSelectDatas")]
        [ProducesResponseType(typeof(ResponseDto<List<DropSelectDto>>), 200)]
        [PermissionAuthorize("produce:process:list")]
        public async Task<ActionResult> GetDropSelectDatas([FromBody] GetMesProcessListReq req)
        {
            var result = await _mainService.GetDropSelectDatas(req);
            return Ok(result);
        }
        /// <summary>
        ///获取工序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<MesProcessDto>), 200)]
        [PermissionAuthorize("produce:process:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加工序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:process:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateMesProcessReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改工序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<MesProcessDto>), 200)]
        [PermissionAuthorize("produce:process:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateMesProcessReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除工序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:process:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.DeleteProcess(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除工序集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:process:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteProcessList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 导入工序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:process:upload")]
        public async Task<ActionResult> UploadListAsync([FromForm(Name = "file")] IFormFile objFile)
        {
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }
                //工序List
                List<ProcessToExcelDto> list = new List<ProcessToExcelDto>();

                //文件转成流
                using (var fileStream = objFile.OpenReadStream())
                {
                    //excel转实体
                    list = ExcelHelper.ParseExcelToList<ProcessToExcelDto>(fileStream, "Sheet1");
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
        [PermissionAuthorize("produce:process:download")]
        public ActionResult DownLoad()
        {
            List<ProcessToExcelDto> list = new List<ProcessToExcelDto>();
            //添加示例数据
            ProcessToExcelDto model = new ProcessToExcelDto();
            model.Code = "drill";
            model.Name = "钻孔";
            model.Attention = "钻孔要求";
            list.Add(model);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "工序模板.xlsx");
        }
    }
}
