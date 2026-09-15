namespace VgAutoDrill.OpenAPI;

public class ConsulServiceConsumerOptions
{
    /// <summary>
    /// Consul服务器地址
    /// </summary>
    public string ConsulAddress { get; set; } = string.Empty;
    public string Namespace { get; set; } = string.Empty;
    public string Datacenter { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
