namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalXianJin
{
    public class SetDrillCommandReq
    {
        public string? DeviceId { get; set; }

        public string? TaskCode { get; set; }

        public List<string>? BarCodeList { get; set; }
    }
}
