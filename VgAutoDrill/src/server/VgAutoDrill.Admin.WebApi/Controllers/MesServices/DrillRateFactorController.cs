using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    public class DrillRateFactorController : BaseController
    {
        private readonly IDrillRateFactorService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public DrillRateFactorController(IDrillRateFactorService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取稼动率因素
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<DrillRateFactorDto>>), 200)]
        [PermissionAuthorize("device:DrillRateFactor:list")]
        public async Task<ActionResult> GetList([FromBody] GetDrillRateFactorListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取稼动率因素
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<DrillRateFactorDto>), 200)]
        [PermissionAuthorize("device:DrillRateFactor:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加稼动率因素
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:DrillRateFactor:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateDrillRateFactorReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改稼动率因素
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<DrillRateFactorDto>), 200)]
        [PermissionAuthorize("device:DrillRateFactor:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateDrillRateFactorReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除稼动率因素
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:DrillRateFactor:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除稼动率因素集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:DrillRateFactor:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}

