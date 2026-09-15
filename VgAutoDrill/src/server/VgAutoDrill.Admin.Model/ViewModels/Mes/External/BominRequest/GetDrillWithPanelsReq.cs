namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominRequest
{
    public class GetDrillWithPanelsReq
    {
        /// <summary>
        ///机号/设备编码
        /// </summary>
        public string? MachineCode { get; set; }

        /// <summary>
        ///上料前10，下料后20
        /// </summary>
        public int ActionType { get; set; }

        public List<PanelBarcodeItems>? PanelBarcodeItems { get; set; }
    }

    public class PanelBarcodeItems
    {
        /// <summary>
        ///轴号
        /// </summary>
        public int? AxleNum { get; set; }

        /// <summary>
        ///铝片码
        /// </summary>
        public string? AluminumSheetQrCode { get; set; }
    }
}