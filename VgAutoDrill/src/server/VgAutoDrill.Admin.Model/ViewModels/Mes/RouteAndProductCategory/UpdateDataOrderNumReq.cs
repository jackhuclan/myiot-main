namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory
{
    /// <summary>
    /// 将两条数据序号互换
    /// </summary>
    public class UpdateDataOrderNumReq
    {
        /// <summary>
        /// 替换前数据ID
        /// </summary>
        public virtual long? OldDataID { get; set; }

        /// <summary>
        /// 替换后数据ID
        /// </summary>
        public virtual long? ReplaceDataID { get; set; }
    }
}
