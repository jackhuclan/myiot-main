using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.LocationDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IRackService
    {
        Task<ResponseDto<string>> Add(AddOrUpdateRackReq req);
        Task<ResponseDto<string>> AddBatch(List<AddOrUpdateRackReq> req);
        Task<ResponseDto<RackDto>> QueryByID(long id);
        Task<ResponseDto<string>> Delete(long id);
        Task<ResponseDto<string>> DeleteList(object[] delList);
        Task<ResponseDto<string>> UpdateData(AddOrUpdateRackReq req);
        Task<RackDto> FindSingle(string code);
        Task<ResponseDto<PageDto<RackDto>>> GetList(RackQueryReq req);
        Task<ResponseDto<List<RackFullDataByPartition>>> GetFullDatasByPartition(RackFullQueryReq req);
        Task<ResponseDto<List<RackFullData>>> GetFullDatas(RackFullQueryReq req);
        Task<ResponseDto<List<ExternalRackDto>>> GetExternalRackInfo(ExternalRackQueryReq req);
        Task<ResponseDto<string>> UpdateStatus(ExternalAddOrUpdateRackReq req);
        Task<ResponseDto<string>> DeleteExternalRackInfo(string code);
        Task<ResponseDto<string>> UpdateExternal(ExternalAddOrUpdateRackReq req);
        Task<ResponseDto<string>> UnBind(UnBindRackAndSiolReq req);
        Task<ResponseDto<string>> BulkInsert(List<RackExcelDto> req);
        Task<int> GetRackStatus(string rackCode);
        Task<List<RackDto>> GetPanelForksWithoutTodoSchedule();
        Task<ResponseDto<List<LocationDetailDto>>> SyncLocationPanels(string locationCode);
        //Task<ResponseDto<List<SiloDetailDto>>> GetCentralRackPanels(string locationCode);
    }
}
