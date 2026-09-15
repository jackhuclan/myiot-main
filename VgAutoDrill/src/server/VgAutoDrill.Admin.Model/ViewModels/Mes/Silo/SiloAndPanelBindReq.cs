namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Silo
{
    public class SiloAndPanelBindReq
    {
        /// <summary>
        /// 板料编码
        /// </summary>
        public virtual string? PanelCode { get; set; }
        /// <summary>
        /// 层号
        /// </summary>
        public virtual int? FloorNum { get; set; }
        /// <summary>
        /// 是否生料
        /// </summary>
        public virtual bool IsRaw { get; set; }

        /// <summary>
        /// 库位编号
        /// </summary>
        public virtual string? LocationCode { get; set; }
    }
}
