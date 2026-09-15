using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanel;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem.VegaRawMaterial
{
    public class DeviceLocationDto
    {
        public string? Code { get; set; }


        public string? SiloCode { get; set; }


        public string? DeviceId { get; set; }


        public List<string>? UndrilledItemCodes { get; set; }


        public List<string>? DrilledItemCodes { get; set; }

        public List<DevicePanelDto>? Panels { get; set; }
    }


}
