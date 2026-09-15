using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    public class RackController : BaseController
    {
        private readonly IRackService _rackService;
        private readonly IAPIHelper _apiHelper;
        private readonly InnerOptions _innerOptions;

        public RackController(IRackService rackService, IAPIHelper apiHelper, IOptions<InnerOptions> options)
        {
            _rackService = rackService;
            _apiHelper = apiHelper;
            _innerOptions = options.Value;
        }

        /// <summary>
        /// 获取料架列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<RackDto>>), 200)]
        [PermissionAuthorize("warehouse:rackManage:list")]
        public async Task<ActionResult> Get([FromBody] RackQueryReq req)
        {
            var result = await _rackService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取料架详细列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFullDatas")]
        [ProducesResponseType(typeof(ResponseDto<List<RackFullDataByPartition>>), 200)]
        [PermissionAuthorize("warehouse:rackManage:list")]
        public async Task<ActionResult> GetFullDatas([FromBody] RackFullQueryReq req)
        {
            var result = await _rackService.GetFullDatasByPartition(req);
            return Ok(result);
        }

        /// <summary>
        /// 根据Id获取单笔数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<RackDto>), 200)]
        [PermissionAuthorize("warehouse:rackManage:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _rackService.QueryByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 根据库位号，获取板料列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCentralRackPanels")]
        [ProducesResponseType(typeof(ResponseDto<List<RackFullData>>), 200)]
        [PermissionAuthorize("warehouse:rackManage:list")]
        public async Task<ActionResult> GetCentralRackPanels(string locationCode)
        {
            var result = await _rackService.SyncLocationPanels(locationCode);
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
        [PermissionAuthorize("warehouse:rackManage:add")]
        public async Task<ActionResult> Add([FromBody] AddOrUpdateRackReq req)
        {
            var result = await _rackService.Add(req);
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
        [PermissionAuthorize("warehouse:rackManage:edit")]
        public async Task<ActionResult> Update([FromBody] AddOrUpdateRackReq req)
        {
            var result = await _rackService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Enable")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:rackManage:edit")]
        public async Task<ActionResult> Enable([FromBody] AddOrUpdateRackReq req)
        {
            var result = await _rackService.UpdateStatus(new ExternalAddOrUpdateRackReq { Code = req.Code, Status = 1 });
            return Ok(result);
        }
        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Disable")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:rackManage:edit")]
        public async Task<ActionResult> Disable([FromBody] AddOrUpdateRackReq req)
        {
            var result = await _rackService.UpdateStatus(new ExternalAddOrUpdateRackReq { Code = req.Code, Status = 0 });
            if (result != null && result.Code == ResponseCode.Success)
            {
                ResponseDto<string> responseDto = new ResponseDto<string>();

                if (string.IsNullOrEmpty(_innerOptions.CancelScheduleUrl))
                {
                    responseDto.Code = ResponseCode.Fail;
                    responseDto.Message = "没有设置innerOptions.CancelScheduleUrl，取消设备的调度记录失败，请联系相关人员手动取消调度记录！";
                    return Ok(responseDto);
                }

                var request = new CancelScheduleTaskRequest
                {
                    Params = new Dictionary<string, object?>
                    {
                        { "DeviceCode", req.Code },
                        { "CancelReason", "库位被禁用" }
                    }
                };

                _apiHelper.RequestData(_innerOptions.CancelScheduleUrl, "post", JsonSerializer.Serialize(request));
            }

            return Ok(result);
        }
        /// <summary>
        /// 删除单个料架数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:rackManage:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _rackService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// 删除料架数据集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:rackManage:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _rackService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 解绑料架与料仓
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnBind")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:rackManage:unbind")]
        public async Task<ActionResult> UnBind(UnBindRackAndSiolReq req)
        {
            var result = await _rackService.UnBind(req);
            return Ok(result);
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoad")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("warehouse:rackManage:download")]
        public ActionResult DownLoad()
        {
            List<RackExcelDto> list = new List<RackExcelDto>();
            RackExcelDto model = new RackExcelDto();
            model.Code = "Rackxxx";
            model.WareHouseCode = "ware001";
            model.SiloCode = "siloxxx";
            model.InnerPoint = "123.2";
            model.OutPoint = "129.2";
            list.Add(model);

            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "料架模板.xlsx");
        }

        /// <summary>
        /// 导入料架
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Upload")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:rackManage:upload")]
        public async Task<ActionResult> UploadListAsync([FromForm(Name = "file")] IFormFile objFile)
        {
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }

                List<RackExcelDto> list = new List<RackExcelDto>();

                using (var fileStream = objFile.OpenReadStream())
                {
                    list = ExcelHelper.ParseExcelToList<RackExcelDto>(fileStream, "Sheet1");
                }

                var result = await _rackService.BulkInsert(list);
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
    }
}
