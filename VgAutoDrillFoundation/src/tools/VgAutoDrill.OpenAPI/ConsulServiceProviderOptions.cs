namespace VgAutoDrill.OpenAPI;

public class ConsulServiceProviderOptions
{
    /// <summary>
    /// Consul服务器地址
    /// </summary>
    public string ConsulAddress { get; set; } = string.Empty;
    /// <summary>
    /// 服务名
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;
    /// <summary>
    /// 服务IP
    /// </summary>
    public string ServiceIP { get; set; } = string.Empty;
    /// <summary>
    /// 服务端口 因为要运行多个实例，端口不能在appsettings.json里配置，在docker容器运行时传入
    /// </summary>
    public int ServicePort { get; set; } = 8600;
    /// <summary>
    /// 健康检查时间间隔
    /// </summary>
    public int ServiceCheckInterval = 5;
    /// <summary>
    /// 超时时间
    /// </summary>
    public int ServiceCheckTimeout = 5;
    /// <summary>
    /// 健康检查地址
    /// </summary>
    public string ServiceHealthCheck { get; set; } = "/healthcheck";
    /// <summary>
    /// 权重
    /// </summary>
    public int Weight { get; set; } = 1;
    public string[] Tags { get; set; } = new string[] { };
    public string Namespace { get; set; } = string.Empty;
    public string Datacenter { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
