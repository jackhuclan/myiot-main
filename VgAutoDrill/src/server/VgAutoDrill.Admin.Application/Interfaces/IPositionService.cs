using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.Position;
using VgAutoDrill.Admin.Model.ViewModels.Res.Position;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    public interface IPositionService
    {
        Task<ResponseDto<string>> Add(AddOrUpdatePositionReq req);

        Task<ResponseDto<string>> Update(AddOrUpdatePositionReq req);

        Task<ResponseDto<string>> Delete(long id);

        Task<ResponseDto<PositionDto>> QueryByID(long id);

        Task<ResponseDto<PageDto<PositionDto>>> GetList(PositionListReq req);

        Task<ResponseDto<int>> GetMaxSort();

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteList(List<long> idList);
    }
}