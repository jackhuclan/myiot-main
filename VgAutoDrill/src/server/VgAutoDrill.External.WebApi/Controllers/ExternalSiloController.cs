using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;

namespace VgAutoDrill.External.WebApi.Controllers
{
    /// <summary>
    /// 料仓对接相关接口
    /// </summary>
    public class ExternalSiloController : BaseController
    {
        private readonly ISiloService _siloService;
        private readonly IMapper _iMapper;

        /// <summary>
        /// 料仓
        /// </summary>
        /// <param name="siloService"></param>
        /// <param name="mapper"></param>
        public ExternalSiloController(ISiloService siloService, IMapper mapper)
        {
            _siloService = siloService;
            _iMapper = mapper;
        }

        /// <summary>
        /// 新增/批量新增料仓
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> Add([FromBody] List<ExternalAddOrUpdateSiloReq> req)
        {
            var siloInfo = _iMapper.Map<List<AddOrUpdateSiloReq>>(req);
            var result = await _siloService.AddBatch(siloInfo);
            return Ok(result);
        }

        /// <summary>
        /// 更新料仓信息及状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult> Update([FromBody] ExternalAddOrUpdateSiloReq req)
        {
            var result = await _siloService.UpdateExternal(req);
            return Ok(result);
        }

        /// <summary>
        /// 移除料仓
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        public async Task<ActionResult> Delete(string code)
        {
            var result = await _siloService.DeleteExternalSiloInfo(code);
            return Ok(result);
        }

        /// <summary>
        /// 查询料仓及载料信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query")]
        public async Task<ActionResult> Query(ExternalSiloQueryReq req)
        {
            var result = await _siloService.GetExternalSiloInfo(req);
            return Ok(result);
        }

        /// <summary>
        /// 料仓绑定板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BindPanel")]
        public async Task<ActionResult> AddOrUpdateExternalSiloWithPanel([FromBody] ExternalAddOrUpdateSiloWithPanelReq req)
        {
            var result = await _siloService.AddOrUpdateExternalSiloWithPanel(req);
            return Ok(result);
        }

        /// <summary>
        /// 解绑料仓与板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnBindPanel")]
        public async Task<ActionResult> UnBindPanel(ExternalSiloUnBindReq req)
        {
            var result = await _siloService.UnBindPanel(req);
            return Ok(result);
        }

        /// <summary>
        /// 一键解绑料仓与板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnBindAllPanel")]
        public async Task<ActionResult> UnBindAllPanel(ExternalSiloAllUnBindReq req)
        {
            var result = await _siloService.UnBindAllPanel(req);
            return Ok(result);
        }
        /// <summary>
        /// 设置手动
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetManual")]
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
        public async Task<ActionResult> SetReady(ExternalSetSiloStatusReq req)
        {
            var result = await _siloService.SetReady(req);
            return Ok(result);
        }
    }
}
