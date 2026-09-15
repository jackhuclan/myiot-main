namespace VgAutoDrill.Central.Core.Mysql;

public class MysqlTaskSchedulerOptions
{
    public const string Options = "MysqlTaskSchedulerOptions";

    /// <summary>
    /// 系统数据库配置刷新频率
    /// </summary>
    public int SystemRefreshInterval { get; set; } = 5;

    public int ConsumingPerSeconds { get; set; } = 5;

    /// <summary>
    /// 钻机库位心跳检测是否触发中转位计算上下料任务
    /// </summary>
    public int DrillLocationHeartBeatSeconds { get; set; } = 10;

    /// <summary>
    /// 是否启用预约分区，以及预约休息点的功能
    /// </summary>
    public bool EnableAutoSetAgvRestPoint { get; set; } = false;

    /// <summary>
    /// 下pin机，呼叫上熟料时，是否启用限定物料代码
    /// </summary>
    public bool EnableUnpinLoadRequestByItem { get; set; } = false;

    /// <summary>
    /// 默认禁止 上pin，下pin直接 卸载料仓到插齿
    /// </summary>
    public bool EnableUsingForkForPinAndUnpinUnloading { get; set; } = false;

    /// <summary>
    /// 叉齿料仓 等待任务 超时自动移出的总秒数
    ///
    /// </summary>
    public int ForkTimeoutSeconds { get; set; } = 60 * 100;

    /// <summary>
    /// 叉齿上的生料等待超时时长，
    /// 用来判断生料是否需要
    /// </summary>
    public int ForkUndrilledWaitingTimeoutSeconds { get; set; } = 60 * 60;

    /// <summary>
    /// 料仓转移配置
    /// </summary>
    public List<PartitionAutoSiloTransferOptions> TransferOptions { get; set; } = new();
}
