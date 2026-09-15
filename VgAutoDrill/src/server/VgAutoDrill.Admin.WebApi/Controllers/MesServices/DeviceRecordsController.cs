using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecords;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    public class DeviceRecordsController : BaseController
    {
        private readonly IDeviceRecordsService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public DeviceRecordsController(IDeviceRecordsService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取设备记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceRecordsDto>>), 200)]
        [PermissionAuthorize("device:DeviceRecords:list")]
        public async Task<ActionResult> GetList([FromBody] GetDeviceRecordsListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取设备记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<DeviceRecordsDto>), 200)]
        [PermissionAuthorize("device:DeviceRecords:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加设备记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:DeviceRecords:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateDeviceRecordsReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改设备记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<DeviceRecordsDto>), 200)]
        [PermissionAuthorize("device:DeviceRecords:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateDeviceRecordsReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:DeviceRecords:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备记录集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:DeviceRecords:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
        /// <summary>
        /// 导出设备记录Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DownLoadList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("device:DeviceRecords:download")]
        public async Task<ActionResult> DownLoadListAsync([FromBody] GetDeviceRecordsListReq req)
        {
            List<DeviceRecordsToExcelDto> list = await _mainService.GetToExcelList(req);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "设备记录.xlsx");
        }

        /// <summary>
        /// 获取设备记录汇总
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSummaryList")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceRecordsSummaryDto>>), 200)]
        [PermissionAuthorize("device:DeviceRecords:list")]
        [AllowAnonymous]
        public async Task<ActionResult> GetSummaryList([FromBody] GetDeviceRecordsSummaryListReq req)
        {
            var result = await _mainService.GetSummaryList(req);
            return Ok(result);
        }

        /// <summary>
        /// 导出设备汇总记录Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DownLoadSummaryList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("device:DeviceRecords:download")]
        public async Task<ActionResult> DownLoadSummaryListAsync([FromBody] GetDeviceRecordsSummaryListReq req)
        {
            List<DeviceRecordsSummaryToExcelDto> list = await _mainService.GetSummaryToExcelList(req);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "设备记录汇总.xlsx");
        }

        /// <summary>
        /// 导出内部设备汇总记录Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DownLoadInnerSummaryListAsync")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("device:DeviceRecords:download")]
        public async Task<ActionResult> DownLoadInnerSummaryListAsync([FromBody] GetDeviceRecordsSummaryListReq req)
        {
            List<DeviceRecordsSummaryToExcelDtoInner> list = await _mainService.GetInnerSummaryToExcelList(req);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "设备记录汇总.xlsx");
        }

        /// <summary>
        /// 获取设备异常明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRateReasonDetails")]
        [ProducesResponseType(typeof(ResponseDto<List<DrillRateFactorDto>>), 200)]
        [PermissionAuthorize("device:DeviceRecords:list")]
        public async Task<ActionResult> GetRateReasonDetails(long recordSummaryId)
        {
            var result = await _mainService.GetRateReasonDetails(recordSummaryId);
            return Ok(result);
        }

        /// <summary>
        /// 计算稼动率数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RefreshSummaryDatas")]
        [PermissionAuthorize("device:DeviceRecords:list")]
        public async Task RefreshSummaryDatas()
        {
            await _mainService.RefreshSummaryDatas();
        }


        /// <summary>
        /// 获取设备保养配置
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDeviceMaintenanceConfigs")]
        [ProducesResponseType(typeof(ResponseDto<PageDto<SysConfigDto>>), 200)]
        //[PermissionAuthorize("device:DeviceRecords:MaintenanceConfigs")]
        public async Task<ResponseDto<PageDto<SysConfigDto>>> GetDeviceMaintenanceConfigs(SysConfigCategoryEnum category)
        {
            var req = new GetSysConfigListReq()
            {
                Category = SysConfigCategoryEnum.DeviceMaintenance,
                PageNum = 1,
                PageSize = int.MaxValue
            };
            return await _mainService.GetDeviceMaintenanceConfigs(req);
        }


        /// <summary>
        /// 保存基本配置
        /// </summary>
        /// <param name="reqs"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveDeviceMaintenanceConfigs")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        //[PermissionAuthorize("device:DeviceRecords:SaveMaintenanceConfigs")]
        public async Task<ActionResult> SaveDeviceMaintenanceConfigs([FromBody] List<AddOrUpdateSysConfigReq> reqs)
        {
            var result = await _mainService.SaveDeviceMaintenanceConfigs(reqs);
            return Ok(result);
        }
    }
}
