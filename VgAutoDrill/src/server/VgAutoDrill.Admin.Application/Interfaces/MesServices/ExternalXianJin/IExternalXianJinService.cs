using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalXianJin;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices.ExternalXianJin
{
    public interface IExternalXianJinService
    {
        Task<ResponseDto<DrillTaskDto>> QueryTaskByDrill(string deviceId);

        Task<ResponseDto<List<ExternalWorkTaskDto>>> GetWorkTaskBatch(List<ExternalWorkOrderQueryReq> reqs);

        Task<ResponseDto<string>> SetBegin(SetDrillCommandReq req);

        Task<ResponseDto<List<ExternalTaskDto>>> QueryDrillInfo(List<string> deviceIds);

        Task<ResponseDto<List<ExternalTaskDto>>> MockQueryDrillInfo(string deviceId, bool isAllVerifyOK, bool existClinkerPanel);

        Task<ResponseDto<string>> UnderClinkerPanel(SetDrillCommandReq req);

        Task<ResponseDto<string>> SetComplete(SetDrillCommandReq req);

        Task<ResponseDto<string>> ClearTaskByDevice(string workStationCode);

        Task<ResponseDto<string>> DeleteTaskBySourceCode(string sourceCode);
    }
}
