using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Res;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroupDetail;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface ICutterGroupService
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupNos"></param>
        /// <param name="cutterGroupStatus">配刀状态 0-待配刀 1-配刀锁定 </param>
        /// <returns></returns>
        Task<List<CutterGroupDto>> List(List<string> groupNos, CutterGroupStatusEnum cutterGroupStatus = 0);

        Task<List<CutterGroupDto>> List(List<string> groupNos);

        Task<CutterGroupDto> QueryByGroupNo(string groupNo);
        /// <summary>
        /// 获取配刀组列表(分页)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<CutterGroupDto>>> GetList(GetListReq req);


        /// <summary>
        /// (批量)删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
       Task<ResponseDto<bool>> Delete(List<long> ids);




        Task<ResponseDto<List<CutterGroupDetailDto>>> GetDetail(List<string> cutterGroupNos);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<bool>> UpdateCutterGroup(AddOrUpdateCutterGroupReq req);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskCode">任务code</param>
        /// <returns></returns>
        Task<CutterGroupDetailDto> DetailByTaskCode(string taskCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskCodes"></param>
        /// <returns></returns>
        Task<List<CutterGroupDetailDto>> DetailByTaskCode(List<string> taskCodes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskCodes"></param>
        /// <returns></returns>
        Task<(bool IsLoked, List<string> LockedTaskCodes)> IsLockedByTaskCode(List<string> taskCodes);

        /// <summary>
        /// 根据组code批量删除配刀组计划
        /// </summary>
        /// <param name="groupNos"></param>
        /// <returns></returns>
        Task DeleteItemAndDetails(List<string> groupNos);


        /// <summary>
        /// 自动生成排刀计划
        /// </summary>
        /// <returns></returns>
        Task AutoGenerateCutterGroupData();

  
        
        /// <summary>
        /// 获取配刀组计划atp文件路径
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<CutterGroupAptFileRes>> GetCutterGroupAtpFile(GetCutterGroupAtpFileReq req);


        /// <summary>
        /// 更新配刀组计划状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<bool>> UpdateCutterGroupStatus(UpdateCutterGroupStatusReq req);






    }
}
