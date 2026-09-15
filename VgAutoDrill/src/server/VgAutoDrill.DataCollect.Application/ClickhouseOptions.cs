namespace VgAutoDrill.DataCollect.Application
{
    public class ClickhouseOptions
    {
        public const string Options = "ClickhouseOptions";
        public string ConnectionString { get; set; }
        public bool Compression { get; set; } = true;
        public bool Session { get; set; } = false;
        public bool CustomDecimals { get; set; } = true;
    }
}
