using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJobLog
{
    public class TransferJobLogsDto : TransferJobDto
    {
        public List<TransferJobLogDto>? TransferJobLogDtos { get; set; }
    }
}
