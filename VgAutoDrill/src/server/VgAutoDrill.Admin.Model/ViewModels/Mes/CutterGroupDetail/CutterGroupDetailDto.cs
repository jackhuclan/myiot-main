namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroupDetail
{
    public class CutterGroupDetailDto
    {
        /// <summary>
        /// 配刀组计划No
        /// </summary>
        public virtual string? CutterGroupNo { get; set; }
        /// <summary>
        /// lot编号
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 每趟的实际钻板数
        /// </summary>
        public virtual int? PanelNum { get; set; }

        /// <summary>
        /// 任务code
        /// </summary>
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 是否首次（首刀，用作钻机换刀标识）
        /// </summary>
        public virtual bool? IsFirstCutter { get; set; }
    }
}
