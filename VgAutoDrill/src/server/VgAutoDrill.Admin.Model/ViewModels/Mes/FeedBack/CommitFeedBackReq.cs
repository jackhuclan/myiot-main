namespace VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack
{
    public class CommitFeedBackReq
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual List<long> Ids { set; get; } = new List<long>();

        /// <summary>
        /// 完工状态(DRAFT/COMMITED)
        /// </summary>
        public virtual string? FeedBackStatus { get; set; }
    }
}
