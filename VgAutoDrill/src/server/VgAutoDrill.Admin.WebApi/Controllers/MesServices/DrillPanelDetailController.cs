using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    public class DrillPanelDetailController : BaseController
    {
        private readonly IDrillPanelDetailService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public DrillPanelDetailController(IDrillPanelDetailService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取钻机料仓记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<DrillPanelDetailDto>>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:list")]
        public async Task<ActionResult> GetList([FromBody] GetDrillPanelDetailListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 根据设备编码获取板料列表
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPanelList")]
        [ProducesResponseType(typeof(ResponseDto<List<DrillPanelDetailInfo>>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:list")]
        public async Task<ActionResult> GetPanelList(string deviceCode)
        {
            var result = await _mainService.GetPanelList(deviceCode);
            return Ok(result);
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoadPanelDetailData")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:add")]
        public async Task<ActionResult> LoadPanelDetailData(LoadDrillPanelDetailReq req)
        {
            var result = await _mainService.LoadPanelDetailData(req);
            return Ok(result);
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("BatchLoadPanelDetailData")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:add")]
        public async Task<ActionResult> BatchLoadPanelDetailData()
        {
            var result = await _mainService.BatchLoadPanelDetailData();
            return Ok(result);
        }
        /// <summary>
        ///获取钻机料仓记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<DrillPanelDetailDto>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加钻机料仓记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateDrillPanelDetailReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改钻机料仓记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<DrillPanelDetailDto>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateDrillPanelDetailReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除钻机料仓记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除钻机料仓记录集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 添加/修改钻机料仓记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BatchAddOrUpdateData")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:add")]
        public async Task<ActionResult> BatchAddOrUpdateData([FromBody] BatchAddOrUpdateDrillPanelDetailReq req)
        {
            var result = await _mainService.BatchAddOrUpdateData(req);
            return Ok(result);
        }

        /// <summary>
        /// 清空钻机料仓记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ClearData")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:edit")]
        public async Task<ActionResult> ClearData([FromBody] ClearDrillPanelDetailReq req)
        {
            var result = await _mainService.ClearData(req);
            return Ok(result);
        }

        /// <summary>
        /// 移动板料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("MovePanel")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:edit")]
        public async Task<ActionResult> MovePanel([FromBody] MovePanelReq req)
        {
            if (req == null)
            {
                return BadRequest();
            }
            var result = await _mainService.MovePanel(req.StartData, req.TargetData);
            return Ok(result);
        }

        /// <summary>
        /// 同步设备板料明细
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SynchronousPanelData")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:edit")]
        public async Task<ActionResult> SynchronousPanelData(string deviceCode)
        {
            var result = await _mainService.SynchronousPanelData(deviceCode);
            return Ok(result);
        }

        /// <summary>
        /// 下发板料数据到设备
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AllotsPanelData")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:edit")]
        public async Task<ActionResult> AllotsPanelData([FromBody] AllotsPanelDataReq req)
        {
            var result = await _mainService.AllotsPanelData(req);
            return Ok(result);
        }

        /// <summary>
        /// 钻机看板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDrillPanelFullData")]
        [ProducesResponseType(typeof(ResponseDto<PageDto<DrillPanelFullData>>), 200)]
        [PermissionAuthorize("device:drillpaneldetail:view")]
        public async Task<ActionResult> GetDrillPanelFullData([FromBody] GetDeviceListReq req)
        {
            var result = await _mainService.GetDrillPanelFullData(req);
            return Ok(result);
        }
    }
}
