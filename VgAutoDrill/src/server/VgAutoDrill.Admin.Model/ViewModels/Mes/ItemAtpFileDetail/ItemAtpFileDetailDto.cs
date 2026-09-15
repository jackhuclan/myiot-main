namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFileDetail
{
    public class ItemAtpFileDetailDto : BaseDto
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

        /// <summary>
        /// 显示文本
        /// </summary>
        public virtual string? Text
        {
            get
            {
                if (Diameter > 0)
                {
                    return Math.Round(Diameter.Value, 3).ToString();
                }
                else
                {
                    return OrderNum.ToString();
                }
            }
        }
    }
}
