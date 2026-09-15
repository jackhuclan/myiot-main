using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;

namespace VgAutoDrill.External.WebApi.Controllers
{
    /// <summary>
    /// 料架对接相关接口
    /// </summary>
    public class ExternalRackController : BaseController
    {
        private readonly IRackService _rackService;
        private readonly IMapper _iMapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rackService"></param>
        /// <param name="mapper"></param>
        public ExternalRackController(IRackService rackService, IMapper mapper)
        {
            _rackService = rackService;
            _iMapper = mapper;
        }

        /// <summary>
        /// 查询料架的实时库存信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query")]
        public async Task<ActionResult> Query(ExternalRackQueryReq req)
        {
            var result = await _rackService.GetExternalRackInfo(req);
            return Ok(result);
        }

        /// <summary>
        /// 新增/批量新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> Add(List<ExternalAddOrUpdateRackReq> req)
        {
            var rackInfo = _iMapper.Map<List<AddOrUpdateRackReq>>(req);
            var result = await _rackService.AddBatch(rackInfo);
            return Ok(result);
        }

        /// <summary>
        ///更新料架信息及状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult> Update(ExternalAddOrUpdateRackReq req)
        {
            var result = await _rackService.UpdateExternal(req);
            return Ok(result);
        }

        /// <summary>
        /// 移除料架
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete("{code}")]

        public async Task<ActionResult> Delete(string code)
        {
            var result = await _rackService.DeleteExternalRackInfo(code);
            return Ok(result);
        }
    }
}
