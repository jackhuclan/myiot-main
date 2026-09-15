namespace VgAutoDrill.Admin.Common.Configuration
{
    public class ExternalOptions
    {
        public const string Options = "ExternalOptions";
        public int ConsumingPerSeconds { get; set; } = 5;
        public string DefaultRouteCode { get; set; } = "A0001";
        public string DefaultProductCategoryCode { get; set; } = "P01";
        public string DefaultUnitOfMeasure { get; set; } = "Panel";

    }
}
