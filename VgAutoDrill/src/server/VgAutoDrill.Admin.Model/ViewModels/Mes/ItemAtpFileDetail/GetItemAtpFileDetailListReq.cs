namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFileDetail
{
    public class GetItemAtpFileDetailListReq : Page
    {
        /// <summary>
        /// ATP文件ID
        /// </summary>
        public virtual long? ItemAtpFileId { get; set; }

        /// <summary>
        /// ATP文件明细编号
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 直径
        /// </summary>
        public virtual decimal? Diameter { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public virtual int? OrderNum { get; set; }
    }
}
