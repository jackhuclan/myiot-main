namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ItemDrillFileDetail
{
    public class AddOrUpdateItemDrillFileDetailReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 主表文件ID
        /// </summary>
        public virtual long? ItemDrillFileId { get; set; }

        /// <summary>
        /// 钻带参数文件明细编号
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 直径
        /// </summary>
        public virtual decimal? Diameter { get; set; }
    }
}
