using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 料仓
    /// </summary>
   // [AllowAnonymous]
    public class SiloController : BaseController
    {
        private readonly ISiloService _siloService;
        public SiloController(ISiloService siloService)
        {
            _siloService = siloService;
        }

        /// <summary>
        /// 获取料仓列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<SiloDto>>), 200)]
        [PermissionAuthorize("warehouse:siloManage:list")]
        [AllowAnonymous]
        public async Task<ActionResult> Get([FromBody] SiloQueryReq req)
        {
            var result = await _siloService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 根据ID获取单笔数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<SiloDto>), 200)]
        [PermissionAuthorize("warehouse:siloManage:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _siloService.QueryByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloManage:add")]
        public async Task<ActionResult> Add([FromBody] AddOrUpdateSiloReq req)
        {
            var result = await _siloService.Add(req);
            return Ok(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloManage:edit")]
        public async Task<ActionResult> Update([FromBody] AddOrUpdateSiloReq req)
        {
            var result = await _siloService.Update(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除单个料仓数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloManage:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _siloService.DeleteData(id);
            return Ok(result);
        }

        /// <summary>
        /// 删除料仓数据集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloManage:remove")]
        public async Task<ActionResult> DeleteList(List<long> idList)
        {
            var result = await _siloService.DeleteDataList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 查询料仓载料信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSiloDetails")]
        [ProducesResponseType(typeof(ResponseDto<List<SiloDto>>), 200)]
        [PermissionAuthorize("warehouse:siloDetailManage:list")]
        public async Task<ActionResult> GetSiloDetails([FromBody] SiloDetailQueryReq req)
        {
            var result = await _siloService.GetSiloDetails(req);
            return Ok(result);
        }

        /// <summary>
        /// 查询料仓载料信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSiloDetailsByLocation")]
        [ProducesResponseType(typeof(ResponseDto<List<SiloDto>>), 200)]
        [PermissionAuthorize("warehouse:siloDetailManage:list")]
        public async Task<ActionResult> GetSiloDetailsByLocation(string location)
        {
            var result = await _siloService.GetSiloDetailsByLocation(location);
            return Ok(result);
        }

        /// <summary>
        /// 添加或者更新料仓板料信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddOrUpdate")]
        [ProducesResponseType(typeof(ResponseDto<List<SiloDetailDto>>), 200)]
        [PermissionAuthorize("warehouse:siloDetailManage:add")]
        public async Task<ActionResult> AddOrUpdate([FromBody] AddOrUpdateSiloDetailReq req)
        {
            var result = await _siloService.AddOrUpdate(req);
            return Ok(result);
        }
        /// <summary>
        /// 设置手动
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetManual")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloManage:edit")]
        public async Task<ActionResult> SetManual(ExternalSetSiloStatusReq req)
        {
            var result = await _siloService.SetManual(req);
            return Ok(result);
        }
        /// <summary>
        /// 设置就绪
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetReady")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloManage:edit")]
        public async Task<ActionResult> SetReady(ExternalSetSiloStatusReq req)
        {
            var result = await _siloService.SetReady(req);
            return Ok(result);
        }
        /// <summary>
        /// 解绑料仓与板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnBindPanel")]
        [PermissionAuthorize("warehouse:siloDetailManage:unbind")]
        public async Task<ActionResult> UnBindPanel([FromBody] ExternalSiloUnBindReq req)
        {
            var result = await _siloService.UnBindPanel(req);
            return Ok(result);
        }

        /// <summary>
        /// 检查板料是否已绑定并执行绑定
        /// </summary>
        /// <param name="req">绑定请求</param>
        /// <returns></returns>
        [HttpPost]
        [Route("IsBindSingle")]
        [PermissionAuthorize("warehouse:siloDetailManage:bind")]
        public async Task<ActionResult> IsBindSingle([FromBody] SiloAndPanelBindReq req)
        {
            var result = await _siloService.IsBindSingle(req);
            return Ok(result);
        }

        /// <summary>
        /// 一键解绑料仓与板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnBindAllPanel")]
        [PermissionAuthorize("warehouse:siloDetailManage:unbind")]
        public async Task<ActionResult> UnBindAllPanel([FromBody] ExternalSiloAllUnBindReq req)
        {
            var result = await _siloService.UnBindAllPanel(req);
            return Ok(result);
        }

        /// <summary>
        /// 扫码绑定料仓与板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BindSingle")]
        [PermissionAuthorize("warehouse:siloDetailManage:bind")]
        public async Task<ActionResult> BindSingle([FromBody] SiloAndPanelBindReq req)
        {
            var result = await _siloService.BindSingle(req);
            return Ok(result);
        }

        /// <summary>
        /// 移动板料到料仓指定层
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("MovePanelToOtherSilo")]
        [PermissionAuthorize("warehouse:siloDetailManage:bind")]
        public async Task<ActionResult> MovePanelToOtherSilo([FromBody] MovePanelToOtherSiloReq req)
        {
            var result = await _siloService.MovePanelToOtherSilo(req);
            return Ok(result);
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoad")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("warehouse:siloDetailManage:download")]
        public ActionResult DownLoad()
        {
            List<SiloExcelDto> list = new List<SiloExcelDto>();
            //添加示例数据
            SiloExcelDto model = new SiloExcelDto();
            model.Code = "siloxxx";
            model.Supplier = "Vega";
            model.FloorCount = 10;
            model.Size = "120*20";
            model.Location = "A-12";
            list.Add(model);

            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "料仓模板.xlsx");
        }

        /// <summary>
        /// 导入料仓
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Upload")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloDetailManage:upload")]
        public async Task<ActionResult> UploadListAsync([FromForm(Name = "file")] IFormFile objFile)
        {
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }

                List<SiloExcelDto> list = new List<SiloExcelDto>();

                using (var fileStream = objFile.OpenReadStream())
                {
                    //excel转实体
                    list = ExcelHelper.ParseExcelToList<SiloExcelDto>(fileStream, "Sheet1");
                }

                var result = await _siloService.BulkInsert(list);
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
        /// AGV绑定/解绑料仓
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AgvBindSilo")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloDetailManage:edit")]
        public async Task<ActionResult> AgvBindSilo(string deviceCode, string siloCode)
        {
            var result = await _siloService.AgvBindSilo(deviceCode, siloCode);
            return Ok(result);
        }
    }
}
