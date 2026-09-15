namespace VegaIot.External.AgvEntity.STD;

public enum STDTaskStatusKind
{
    /// <summary>
    /// 已创建
    /// </summary>
    Created = 1,

    /// <summary>
    /// 正在执行
    /// </summary>
    Running = 2,

    /// <summary>
    /// 取消完成
    /// </summary>
    Cancel = 5,

    /// <summary>
    /// 已结束
    /// </summary>
    End = 9,

    /// <summary>
    /// 被打断
    /// </summary>
    Break = 10,
}
