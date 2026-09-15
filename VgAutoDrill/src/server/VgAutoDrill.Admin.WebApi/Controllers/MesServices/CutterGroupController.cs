using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Res;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroupDetail;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 配刀组计划
    /// </summary>
    public class CutterGroupController : BaseController
    {
        private readonly ICutterGroupService _mainService;
        public CutterGroupController(ICutterGroupService mainService)
        {
            _mainService = mainService;
        }



        /// <summary>
        /// 获取配刀组列表(分页)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<PageDto<CutterGroupDto>>), 200)]
        public async Task<ActionResult> GetList(GetListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }


        /// <summary>
        /// 根据配刀组计划No获取配刀组计划详情
        /// </summary>
        /// <param name="cutterGroupNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDetail")]
        [ProducesResponseType(typeof(ResponseDto<List<CutterGroupDetailDto>>), 200)]
        public async Task<ActionResult> GetDetail(string cutterGroupNo)
        {
            var result = await _mainService.GetDetail(new List<string>() { cutterGroupNo });
            return Ok(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        [ProducesResponseType(typeof(ResponseDto<bool>), 200)]
        public async Task<ActionResult> UpdateCutterGroup(AddOrUpdateCutterGroupReq req)
        {
            var result = await _mainService.UpdateCutterGroup(req);
            return Ok(result);
        }

        /// <summary>
        /// (批量)删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete")]
        [ProducesResponseType(typeof(ResponseDto<bool>), 200)]
        public async Task<ActionResult> Delete([FromBody] List<long> ids)
        {
            var result = await _mainService.Delete(ids);
            return Ok(result);
        }

  
        /// <summary>
        /// 获取配刀组计划apt文件路径
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCutterGroupAtpFile")]
        [ProducesResponseType(typeof(ResponseDto<bool>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetCutterGroupAtpFile(GetCutterGroupAtpFileReq req)
        {
            var result = await _mainService.GetCutterGroupAtpFile(req);
            return Ok(result);
        }

        




    }
}
