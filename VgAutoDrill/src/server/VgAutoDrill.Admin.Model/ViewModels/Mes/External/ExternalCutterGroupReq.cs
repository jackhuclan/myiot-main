namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External
{
    public class ExternalCutterGroupReq
    {
        /// <summary>
        /// 组计划ID
        /// /// </summary>
        public string? GroupNo { get; set; }

        /// <summary>
        /// 上一次生成组计划时间
        /// </summary>
        public DateTime GroupDate { get; set; }
    }
    public class ExternalCutterDetailReq
    {
        public List<string> DeviceCodes { get; set; } = new List<string>();
    }
}
