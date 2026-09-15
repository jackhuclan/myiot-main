using System.Collections.Generic;
using System.Threading.Tasks;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    /// <summary>
    /// 料仓板料追溯详细
    /// </summary>
    public interface ISiloPanelTraceDetailService
    {
        /// <summary>
        /// 新增料仓板料追溯详细记录
        /// </summary>
        /// <param name="request">新增请求参数</param>
        /// <returns>新增结果</returns>
        Task<ResponseDto<string>> Add(AddOrUpdateSiloPanelTraceDetailReq request);

        /// <summary>
        /// 根据ID查询料仓板料追溯详细记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>追溯详细记录详情</returns>
        Task<ResponseDto<SiloPanelTraceDetailDto>> QueryByID(long id);

        /// <summary>
        /// 更新料仓板料追溯详细记录
        /// </summary>
        /// <param name="request">更新请求参数</param>
        /// <returns>更新结果</returns>
        Task<ResponseDto<string>> Update(AddOrUpdateSiloPanelTraceDetailReq request);

        /// <summary>
        /// 删除料仓板料追溯详细记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>删除结果</returns>
        Task<ResponseDto<string>> Delete(long id);

        /// <summary>
        /// 批量删除料仓板料追溯详细记录
        /// </summary>
        /// <param name="idList">记录ID数组</param>
        /// <returns>删除结果</returns>
        Task<ResponseDto<string>> DeleteList(object[] idList);

        /// <summary>
        /// 获取料仓板料追溯详细记录列表
        /// </summary>
        /// <param name="request">查询请求参数</param>
        /// <returns>详细记录列表</returns>
        Task<ResponseDto<PageDto<SiloPanelTraceDetailDto>>> GetList(GetSiloPanelTraceDetailListReq request);


    }
}