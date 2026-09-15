namespace VgAutoDrill.Core
{
    public class RedisCacheOptions
    {
        public const string Options = "RedisCacheOptions";
        public string ConnectionString { get; set; } = string.Empty;
        public string InstanceName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
