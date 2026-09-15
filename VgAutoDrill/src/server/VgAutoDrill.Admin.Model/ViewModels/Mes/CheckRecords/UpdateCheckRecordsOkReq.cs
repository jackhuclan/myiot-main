namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CheckRecords
{
    public class UpdateCheckRecordsOkReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 是否检验合格(0/1)
        /// </summary>
        public virtual string? IsCheckOk { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
    }
}
