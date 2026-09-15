using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;
using VgAutoDrill.Admin.Model.ViewModels.Mes.LocationDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndPanel;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface ISiloService
    {
        Task<ResponseDto<string>> Add(AddOrUpdateSiloReq req);
        Task<ResponseDto<string>> AddBatch(List<AddOrUpdateSiloReq> req);
        Task<ResponseDto<SiloDto>> QueryByID(long id);
        Task<ResponseDto<string>> DeleteData(long id);
        Task<ResponseDto<string>> DeleteDataList(List<long> delList);
        Task<ResponseDto<string>> Update(AddOrUpdateSiloReq req);
        Task<ResponseDto<PageDto<SiloDto>>> GetList(SiloQueryReq req);
        Task<ResponseDto<List<SiloDetailDto>>> GetSiloDetails(SiloDetailQueryReq req);
        Task<ResponseDto<List<SiloDetailDto>>> GetSiloDetailsByLocation(string location);
        Task<ResponseDto<List<LocationDetailDto>>> AddOrUpdate(AddOrUpdateSiloDetailReq req);
        Task<ResponseDto<string>> UpdateExternalSiloInfo(ExternalUpdateSiloReq req);
        Task<ResponseDto<string>> DeleteExternalSiloInfo(string code);
        Task<ResponseDto<List<SiloInfo>>> GetExternalSiloInfo(ExternalSiloQueryReq req);
        Task<ResponseDto<string>> AddOrUpdateExternalSiloWithPanel(ExternalAddOrUpdateSiloWithPanelReq req);
        Task<ResponseDto<string>> UpdateExternal(ExternalAddOrUpdateSiloReq req);
        Task<ResponseDto<string>> UnBindPanel(ExternalSiloUnBindReq req);
        Task<ResponseDto<string>> UnBindAllPanel(ExternalSiloAllUnBindReq req);
        Task<ResponseDto<WorkOrderAndPanelDto>> GetPanelInfo(SiloAndPanelReq req);
        Task<ResponseDto<BindSingleResultDto>> IsBindSingle(SiloAndPanelBindReq req);
        Task<ResponseDto<BindSingleResultDto>> BindSingle(SiloAndPanelBindReq req);
        Task<ResponseDto<string>> BulkInsert(List<SiloExcelDto> req);
        Task<ResponseDto<string>> SetManual(ExternalSetSiloStatusReq req);
        Task<ResponseDto<string>> SetReady(ExternalSetSiloStatusReq req);
        Task<ResponseDto<string>> SetAuto(ExternalSetSiloStatusReq req);
        Task<SiloStatus> GetSiloStatus(string siloCode);
        Task<ResponseDto<string>> AllotsPanelData(AllotsPanelDataReq req);
        Task<ResponseDto<List<SiloDetailDto>>> MovePanelToOtherSilo(MovePanelToOtherSiloReq req);

        Task<ResponseDto<string>> AgvBindSilo(string deviceCode, string siloCode);
    }
}
