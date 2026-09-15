using System.Collections.Generic;
using System.Threading.Tasks;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    /// <summary>
    /// 料仓板料追溯
    /// </summary>
    public interface ISiloPanelTraceService
    {
        /// <summary>
        /// 获取料仓板料追溯列表
        /// </summary>
        /// <param name="request">查询请求参数</param>
        /// <returns>符合条件的追溯记录列表</returns>
        Task<ResponseDto<PageDto<SiloPanelTraceDto>>> GetList(GetSiloPanelTraceListReq request);

        /// <summary>
        /// 新增料仓板料追溯记录
        /// </summary>
        /// <param name="request">新增请求参数</param>
        /// <returns>新增结果</returns>
        Task<ResponseDto<int>> AddAndReturnId(AddOrUpdateSiloPanelTraceReq request);

        /// <summary>
        /// 根据ID查询料仓板料追溯记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>追溯记录详情</returns>
        Task<ResponseDto<SiloPanelTraceDto>> QueryByID(long id);

        /// <summary>
        /// 更新料仓板料追溯记录
        /// </summary>
        /// <param name="request">更新请求参数</param>
        /// <returns>更新结果</returns>
        Task<ResponseDto<string>> Update(AddOrUpdateSiloPanelTraceReq request);

        /// <summary>
        /// 删除料仓板料追溯记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>删除结果</returns>
        Task<ResponseDto<string>> Delete(long id);

        /// <summary>
        /// 批量删除料仓板料追溯记录
        /// </summary>
        /// <param name="idList">记录ID数组</param>
        /// <returns>删除结果</returns>
        Task<ResponseDto<string>> DeleteList(object[] idList);

        /// <summary>
        /// 从PanelList添加料仓板料追溯记录
        /// </summary>
        /// <param name="panels">板料列表</param>
        /// <param name="subject">操作主题描述</param>
        /// <param name="scheduleId">调度ID</param>
        /// <param name="transportationTaskId">运输任务ID</param>
        /// <returns>添加结果</returns>
        Task<ResponseDto<int>> AddFromPanelList(PanelList panels, string subject, long? scheduleId = null, long? transportationTaskId = null);

        /// <summary>
        /// 获取导出Excel的数据列表
        /// </summary>
        /// <param name="request">查询请求参数</param>
        /// <returns>导出数据列表</returns>
        Task<List<SiloPanelTraceToExcelDto>> GetToExcelList(GetSiloPanelTraceListReq request);

        /// <summary>
        /// 清理指定位置的缓存
        /// </summary>
        /// <param name="location">位置</param>
        Task ClearLocationCacheAsync(string location);

        /// <summary>
        /// 获取缓存信息
        /// </summary>
        /// <param name="location">位置</param>
        Task<string> GetCacheInfoAsync(string location);
    }
}
