namespace VgAutoDrill.Fundation.Iot.Configuration;

public class MqttServerConnectionOptions
{
    /// <summary>
    /// mqtt Server服务器地址
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// mqtt Server服务器端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 登录用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 登录用户的密码
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 登录客户端id
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用tls，默认为false
    /// </summary>
    public bool UseTls { get; set; } = false;

    /// <summary>
    /// 客户端证书路径
    /// </summary>
    public string CertPemFilePath { get; set; } = string.Empty;

    /// <summary>
    /// mqtt client keep alive time
    /// </summary>
    public int KeepAlivePeriod { get; set; } = 60;

    /// <summary>
    /// mqtt client heart beat time
    /// </summary>
    public int HeartbeatPeriod { get; set; } = 2;

    /// <summary>
    /// mqtt client 连接超时时间
    /// </summary>
    public int MqttConnectionTimeOut { get; set; } = 15;

    /// <summary>
    /// 是否启用心跳，默认false
    /// </summary>
    public bool EnableHeartbeat { get; set; } = false;

    /// <summary>
    /// 设备端，是否开启mqtt日志订阅，默认不订阅
    /// </summary>
    public bool EnableLogMqttClient { get; set; } = false;
}
