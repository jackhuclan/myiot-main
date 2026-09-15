namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder
{
    public class CommitWorkOrderReq
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual long? Id { set; get; }

        /// <summary>
        /// 是否紧急插单
        /// </summary>
        public virtual int? IsUrgent { get; set; }
    }
}
