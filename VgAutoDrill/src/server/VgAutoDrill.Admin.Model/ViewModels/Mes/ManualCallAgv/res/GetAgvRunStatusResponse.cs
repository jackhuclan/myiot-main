namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.res
{
    public class GetAgvRunStatusResponse
    {
        /// <summary>
        /// 库位
        /// </summary>
        public string? LocationCode { get; set; }

        /// <summary>
        /// agv状态信息
        /// </summary>
        public string? AgvRunStatus { get; set; }
    }
}
