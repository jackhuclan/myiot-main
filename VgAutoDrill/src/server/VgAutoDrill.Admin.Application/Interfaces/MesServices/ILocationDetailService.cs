using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.LocationDetail;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface ILocationDetailService
    {
        Task<ResponseDto<string>> Add(AddOrUpdateLocationDetailReq req);
        Task<ResponseDto<string>> AddBatch(List<AddOrUpdateLocationDetailReq> req);
        Task<ResponseDto<LocationDetailDto>> QueryByID(long id);
        Task<ResponseDto<string>> Delete(long id);
        Task<ResponseDto<string>> DeleteList(object[] delList);
        Task<ResponseDto<string>> Update(AddOrUpdateLocationDetailReq req);
        Task<LocationDetailDto> FindSingle(string code, int? floorNum);
        Task<ResponseDto<PageDto<LocationDetailDto>>> GetList(LocationDetailQueryReq req);
        Task<ResponseDto<List<LocationDetailDto>>> GetByCode(string code);
    }
}
