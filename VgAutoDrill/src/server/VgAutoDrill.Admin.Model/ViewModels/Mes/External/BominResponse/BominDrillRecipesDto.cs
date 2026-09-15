namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominResponse
{
    /// <summary>
    /// 
    /// </summary>
    public class BominDrillRecipesDto
    {

        public BominDrillRecipesData? Data { get; set; } = new BominDrillRecipesData();

    }

    public class BominDrillRecipesData
    {
        public string? Lot { get; set; } = string.Empty;

        public string? DeviceId { get; set; } = string.Empty;

        public string? ProcessCode { get; set; } = string.Empty;

        public string? DRCS { get; set; } = string.Empty;

        public string? DRCX { get; set; } = string.Empty;

        public string? DRZD { get; set; } = string.Empty;

        public string? FtpBasicUrl { get; set; } = string.Empty;

        public string? FtpAccount { get; set; } = string.Empty;

        public string? FtpPassword { get; set; } = string.Empty;

        public bool Success { get; set; }

        public string? Content { get; set; } = string.Empty;
    }
}
