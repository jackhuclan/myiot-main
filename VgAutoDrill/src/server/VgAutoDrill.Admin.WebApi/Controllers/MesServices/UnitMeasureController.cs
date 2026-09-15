using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.UnitMeasure;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 计量单位管理
    /// </summary>
    public class UnitMeasureController : BaseController
    {
        private readonly IUnitMeasureService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public UnitMeasureController(IUnitMeasureService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取计量单位列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<UnitMeasureDto>>), 200)]
        [PermissionAuthorize("masterData:unitMeasure:list")]
        public async Task<ActionResult> GetList([FromBody] GetUnitMeasureListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        /// 获取计量单位列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDropSelectDatas")]
        [ProducesResponseType(typeof(ResponseDto<List<DropSelectDto>>), 200)]
        [PermissionAuthorize("masterData:unitMeasure:list")]
        public async Task<ActionResult> GetDropSelectDatas([FromBody] GetUnitMeasureListReq req)
        {
            var result = await _mainService.GetDropSelectDatas(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取计量单位中的主单位列表
        /// </summary>
        /// <returns></returns>
        [Route("GetPrimaryUnitList")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto<List<UnitMeasureDto>>), 200)]
        [PermissionAuthorize("masterData:unitMeasure:list")]
        public async Task<ActionResult> GetPrimaryUnitList(int id)
        {
            var result = await _mainService.GetPrimaryUnitList(id);
            return Ok(result);
        }
        /// <summary>
        ///获取计量单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<UnitMeasureDto>), 200)]
        [PermissionAuthorize("masterData:unitMeasure:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加计量单位
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:unitMeasure:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateUnitMeasureReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改计量单位
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<UnitMeasureDto>), 200)]
        [PermissionAuthorize("masterData:unitMeasure:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateUnitMeasureReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除计量单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:unitMeasure:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除计量单位集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:unitMeasure:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 导入计量单位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("masterData:unitMeasure:upload")]
        public async Task<ActionResult> UploadListAsync([FromForm(Name = "file")] IFormFile objFile)
        {
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }
                //计量 单位List
                List<UnitMeasureToExcelDto> units = new List<UnitMeasureToExcelDto>();

                //文件转成流
                using (var fileStream = objFile.OpenReadStream())
                {
                    //excel转实体
                    units = ExcelHelper.ParseExcelToList<UnitMeasureToExcelDto>(fileStream, "Sheet1");
                }

                var result = await _mainService.BulkInsert(units);
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
        [PermissionAuthorize("masterData:unitMeasure:download")]
        public ActionResult DownLoad()
        {
            List<UnitMeasureToExcelDto> units = new List<UnitMeasureToExcelDto>();
            //添加示例数据
            UnitMeasureToExcelDto model = new UnitMeasureToExcelDto();
            model.Code = "Panel";
            model.Name = "单层Panel";
            model.ChangeRate = 2;
            model.PrimaryFlag = "Y";
            model.PrimaryId = 34;
            model.Remark = "单位";
            units.Add(model);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(units);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "计量单位模板.xlsx");
        }
    }
}