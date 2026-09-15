using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;

namespace VgAutoDrill.External.Application.Interfaces.V3
{
    public interface IExternalSiloServiceV3
    {
        /// <summary>
        /// 新增/批量新增料仓
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddSiloBatch(List<ExternalAddOrUpdateSiloReq> req);

        /// <summary>
        /// 更新料仓信息及状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateSiloExternal(ExternalAddOrUpdateSiloReq req);

        /// <summary>
        /// 移除料仓
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteExternalSiloInfo(string code);

        /// <summary>
        /// 查询料仓及载料信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<SiloInfo>>> GetExternalSiloInfo(ExternalSiloQueryReq req);

        /// <summary>
        /// 料仓绑定板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddOrUpdateExternalSiloWithPanel(ExternalAddOrUpdateSiloWithPanelReq req);

        /// <summary>
        /// 解绑料仓与板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> UnBindPanel(ExternalSiloUnBindReq req);

        /// <summary>
        /// 一键解绑料仓与板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> UnBindAllPanel(ExternalSiloAllUnBindReq req);

        /// <summary>
        /// 设置手动
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> SetManual(ExternalSetSiloStatusReq req);

        /// <summary>
        /// 设置就绪
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> SetReady(ExternalSetSiloStatusReq req);

        /// <summary>
        /// 查询料架的实时库存信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<ExternalRackDto>>> GetExternalRackInfo(ExternalRackQueryReq req);

        /// <summary>
        /// 新增/批量新增料架
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddRackBatch(List<AddOrUpdateRackReq> req);

        /// <summary>
        /// 更新料架信息及状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateRackExternal(ExternalAddOrUpdateRackReq req);

        /// <summary>
        /// 移除料架
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteExternalRackInfo(string code);
    }
}
