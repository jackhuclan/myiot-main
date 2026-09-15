using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Application.Services;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;
using VgAutoDrill.External.Application.Interfaces.V3;

namespace VgAutoDrill.External.Application.Services.V3
{
    public class ExternalSiloServiceV3 : BaseService, IExternalSiloServiceV3
    {
        private readonly ISiloService _siloService;
        private readonly IRackService _rackService;
        private readonly IMapper _mapper;
        public ExternalSiloServiceV3(IMapper mapper,
            ISiloService siloService,
            IRackService rackService)
        {
            _mapper = mapper;
            _siloService = siloService;
            _rackService = rackService;
        }

        /// <summary>
        /// 新增/批量新增料仓
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddSiloBatch(List<ExternalAddOrUpdateSiloReq> req)
        {
            if (req == null || req.Count == 0)
            {
                return Fail("信息格式错误!");
            }
            var siloInfo = _mapper.Map<List<AddOrUpdateSiloReq>>(req);
            if (siloInfo == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _siloService.AddBatch(siloInfo);
            return result;
        }

        /// <summary>
        /// 更新料仓信息及状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateSiloExternal(ExternalAddOrUpdateSiloReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _siloService.UpdateExternal(req);
            return result;
        }

        /// <summary>
        /// 移除料仓
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteExternalSiloInfo(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Fail("未识别有效的Code!");
            }

            var result = await _siloService.DeleteExternalSiloInfo(code);
            return result;
        }

        /// <summary>
        /// 查询料仓及载料信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<SiloInfo>>> GetExternalSiloInfo(ExternalSiloQueryReq req)
        {
            if (req == null)
            {
                return Fail<List<SiloInfo>>("信息格式错误!");
            }

            var result = await _siloService.GetExternalSiloInfo(req);
            return result;
        }

        /// <summary>
        /// 料仓绑定板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddOrUpdateExternalSiloWithPanel(ExternalAddOrUpdateSiloWithPanelReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _siloService.AddOrUpdateExternalSiloWithPanel(req);
            return result;
        }

        /// <summary>
        /// 解绑料仓与板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UnBindPanel(ExternalSiloUnBindReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _siloService.UnBindPanel(req);
            return result;
        }

        /// <summary>
        /// 一键解绑料仓与板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UnBindAllPanel(ExternalSiloAllUnBindReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _siloService.UnBindAllPanel(req);
            return result;
        }

        /// <summary>
        /// 设置手动
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> SetManual(ExternalSetSiloStatusReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _siloService.SetManual(req);
            return result;
        }

        /// <summary>
        /// 设置就绪
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> SetReady(ExternalSetSiloStatusReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _siloService.SetReady(req);
            return result;
        }

        /// <summary>
        /// 查询料架的实时库存信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<ExternalRackDto>>> GetExternalRackInfo(ExternalRackQueryReq req)
        {
            if (req == null)
            {
                return Fail<List<ExternalRackDto>>("信息格式错误!");
            }

            var result = await _rackService.GetExternalRackInfo(req);
            return result;
        }

        /// <summary>
        /// 新增/批量新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddRackBatch(List<AddOrUpdateRackReq> req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _rackService.AddBatch(req);
            return result;
        }

        /// <summary>
        /// 更新料架信息及状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateRackExternal(ExternalAddOrUpdateRackReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _rackService.UpdateExternal(req);
            return result;
        }

        /// <summary>
        /// 移除料架
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteExternalRackInfo(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Fail("未识别有效的Code!");
            }

            var result = await _rackService.DeleteExternalRackInfo(code);
            return result;
        }
    }
}
