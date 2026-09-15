using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 工作站
    /// </summary>
    public class WorkstationController : BaseController
    {
        private readonly IWorkstationService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public WorkstationController(IWorkstationService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取工作站列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<WorkstationDto>>), 200)]
        [PermissionAuthorize("masterData:workstation:list")]
        public async Task<ActionResult> GetList([FromBody] GetWorkstationListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取工作站
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<WorkstationDto>), 200)]
        [PermissionAuthorize("masterData:workstation:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 设备负荷查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetWorkStationLoadTask")]
        [ProducesResponseType(typeof(ResponseDto<List<WorkStationLoadTaskDto>>), 200)]
        [PermissionAuthorize("masterData:workstation:list")]
        public async Task<ActionResult> GetWorkStationLoadTask([FromBody] GetWorkStationLoadTaskReq req)
        {
            var result = await _mainService.GetWorkStationLoadTask(req);
            return Ok(result);
        }

        /// <summary>
        /// 添加工作站
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:workstation:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateWorkstationReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改工作站
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<WorkstationDto>), 200)]
        [PermissionAuthorize("masterData:workstation:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateWorkstationReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除工作站
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:workstation:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.DeleteData(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除工作站集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:workstation:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _mainService.DeleteDataList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 导入工作站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:workstation:upload")]
        public async Task<ActionResult> UploadListAsync([FromForm(Name = "file")] IFormFile objFile)
        {
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }
                //工作站List
                List<WorkstationToExcelDto> list = new List<WorkstationToExcelDto>();

                //文件转成流
                using (var fileStream = objFile.OpenReadStream())
                {
                    //excel转实体
                    list = ExcelHelper.ParseExcelToList<WorkstationToExcelDto>(fileStream, "Sheet1");
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
        [PermissionAuthorize("masterData:workstation:download")]
        public ActionResult DownLoad()
        {
            List<WorkstationToExcelDto> list = new List<WorkstationToExcelDto>();
            //添加示例数据
            WorkstationToExcelDto model = new WorkstationToExcelDto();
            model.Code = "drill01-0001";
            model.Name = "钻机01-0001";
            model.Remark = "钻机工作站";
            model.WorkshopName = "车间02（测试）";
            model.WorkshopCode = "shop02";
            model.WorkshopId = 9;
            list.Add(model);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "工作站模板.xlsx");
        }
    }
}