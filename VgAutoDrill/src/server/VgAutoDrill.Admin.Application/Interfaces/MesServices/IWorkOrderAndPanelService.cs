
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndPanel;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IWorkOrderAndPanelService
    {
        /// <summary>
        /// 新加板材和工单信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddWorkOrderAndPanelList(List<AddOrUpdateWorkOrderAndPanelReq> req);

        /// <summary>
        /// 删除板材和工单信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteList(List<string> req);
        Task<ResponseDto<string>> DeleteByExternalCode(string externalSourceCode);
        /// <summary>
        /// 获取板材和工单信息(不分页)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<List<WorkOrderAndPanelDto>> GetWorkOrderAndPanelInfors(GetWorkOrderAndPanelInforsReq req);

        /// <summary>
        /// 获取板材和工单信息(分页)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<WorkOrderAndPanelDto>>> GetWorkOrderAndPanelList(GetWorkOrderAndPanelReq req);

        /// <summary>
        /// 新加板材和工单信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateWorkOrderAndPanelList(List<AddOrUpdateWorkOrderAndPanelReq> req);
    }
}
