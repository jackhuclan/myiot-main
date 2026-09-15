namespace VgAutoDrill.Admin.Model.ViewModels
{
    public class BaseReqGetList : GeneralBaseReqGetList
    {
        /// <summary>
        /// 名称
        /// </summary>
        public override sealed string Name { set; get; }
        /// <summary>
        /// 状态
        /// </summary>

        public override sealed int Status { set; get; } = -1;
    }

    public class GeneralBaseReqGetList : Page
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { set; get; }
        /// <summary>
        /// 状态
        /// </summary>

        public virtual int Status { set; get; } = -1;
    }
}
