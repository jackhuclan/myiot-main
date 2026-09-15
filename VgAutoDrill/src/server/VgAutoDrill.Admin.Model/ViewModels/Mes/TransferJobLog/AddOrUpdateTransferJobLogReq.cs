namespace VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJobLog
{
    public class AddOrUpdateTransferJobLogReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 主表Id
        /// </summary>
        public virtual long? MasterId { get; set; }

        /// <summary>
        /// 料仓任务明细
        /// </summary>
        public virtual string? Message { get; set; }
    }
}
